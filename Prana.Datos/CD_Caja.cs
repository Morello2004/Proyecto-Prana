using Microsoft.Data.SqlClient;
using Prana.Entidades;
using System.Data;
using System.Text.Json;
using static Prana.Datos.GestionVentaSql;

namespace Prana.Datos;

// administra aperturas por identificador para separar turnos de una misma fecha sin sumar recargos
public class CD_Caja
{
    // serializa aperturas cierres y cobros para que ninguna operacion cambie de caja durante su guardado
    private static void BloquearCaja(SqlConnection conexion, SqlTransaction transaccion)
    {
        using SqlCommand comando = Comando(conexion, transaccion, @"
            DECLARE @r int;
            EXEC @r=sys.sp_getapplock @Resource=N'Prana.Caja.Activa',@LockMode='Exclusive',@LockOwner='Transaction',@LockTimeout=15000;
            IF @r<0 THROW 50003,N'La caja está ocupada. Reintente.',1;");
        comando.ExecuteNonQuery();
    }

    // toma el reloj del servidor para registrar horarios reales
    internal static DateTime Ahora(SqlConnection conexion, SqlTransaction transaccion)
    {
        using SqlCommand comando = Comando(conexion, transaccion, "SELECT SYSDATETIME()");
        return (DateTime)comando.ExecuteScalar()!;
    }

    // exige una apertura explicita no crea otra caja silenciosamente despues de cerrar
    internal static int ExigirCajaAbierta(SqlConnection conexion, SqlTransaction transaccion, DateTime fecha, int? esperada = null)
    {
        BloquearCaja(conexion, transaccion);
        if (fecha.Date != Ahora(conexion, transaccion).Date)
        {
            throw new InvalidOperationException("Cambió el día mientras se registraba la operación. Actualice y vuelva a intentar.");
        }
        using SqlCommand comando = Comando(conexion, transaccion, "SELECT id,fecha FROM Caja WHERE estado=N'Abierta'");
        using SqlDataReader lector = comando.ExecuteReader();
        if (!lector.Read())
        {
            throw new InvalidOperationException("No hay una caja abierta. Abra una desde Caja y gastos antes de registrar operaciones.");
        }
        int id = lector.GetInt32(0);
        if (lector.GetDateTime(1).Date != fecha.Date)
        {
            throw new InvalidOperationException("Cambió el día. Actualice Caja para completar el cierre anterior y abrir una nueva.");
        }
        if (esperada.HasValue && esperada.Value != id)
        {
            throw new InvalidOperationException("La caja seleccionada ya no está abierta. Actualice la pantalla.");
        }
        return id;
    }

    // el identificador y los horarios de sql prevalecen sobre los json de versiones anteriores
    private static CierreCaja LeerCabecera(SqlDataReader lector)
    {
        CierreCaja caja = JsonSerializer.Deserialize<CierreCaja>(lector.GetString(5))!;
        caja.IdCaja = lector.GetInt32(0);
        caja.Fecha = lector.GetDateTime(1);
        caja.Cerrada = lector.GetString(2) == "Cerrada";
        caja.Inicio = lector.GetDateTime(3);
        if (!lector.IsDBNull(4))
        {
            caja.Fin = lector.GetDateTime(4);
        }
        caja.Numero = 0;
        if (caja.Cerrada)
        {
            caja.Numero = caja.IdCaja;
        }
        return caja;
    }

    // ofrece todas las aperturas de una fecha para consultar cada cierre individual
    public List<CierreCaja> Listar(DateTime fecha)
    {
        using SqlConnection conexion = new SqlConnection(Conexion.ObtenerCadena());
        using SqlCommand comando = new SqlCommand("SELECT id,fecha,estado,inicio,fin,datos FROM Caja WHERE fecha=@fecha ORDER BY id DESC", conexion);
        comando.Parameters.Add("@fecha", SqlDbType.Date).Value = fecha.Date;
        conexion.Open();
        using SqlDataReader lector = comando.ExecuteReader();
        List<CierreCaja> cajas = new List<CierreCaja>();
        while (lector.Read())
        {
            cajas.Add(LeerCabecera(lector));
        }
        return cajas;
    }

    // cada apertura tiene su propio saldo inicial y no hereda el balance de otro turno
    public CierreCaja Abrir(CierreCaja solicitud, int usuario)
    {
        using SqlConnection conexion = new SqlConnection(Conexion.ObtenerCadena());
        conexion.Open();
        using SqlTransaction transaccion = conexion.BeginTransaction();
        BloquearCaja(conexion, transaccion);
        using SqlCommand existe = Comando(conexion, transaccion, "SELECT COUNT(*) FROM Caja WHERE estado=N'Abierta'");
        if (Convert.ToInt32(existe.ExecuteScalar()) > 0)
        {
            throw new InvalidOperationException("Ya hay una caja abierta. Ciérrela antes de abrir otra.");
        }
        DateTime ahora = Ahora(conexion, transaccion);
        CierreCaja caja = new CierreCaja
        {
            Fecha = ahora.Date, Inicio = ahora, SaldoInicial = solicitud.SaldoInicial,
            SaldoInicialUsd = solicitud.SaldoInicialUsd, Observaciones = solicitud.Observaciones
        };
        using SqlCommand abrir = Comando(conexion, transaccion, @"
            INSERT Caja(fecha,estado,inicio,id_usuario_apertura,datos)
            OUTPUT INSERTED.id VALUES(@fecha,N'Abierta',@inicio,@usuario,@datos);",
            ("@fecha", caja.Fecha), ("@inicio", caja.Inicio), ("@usuario", usuario), ("@datos", JsonSerializer.Serialize(caja)));
        caja.IdCaja = Convert.ToInt32(abrir.ExecuteScalar());
        GuardarDatos(conexion, transaccion, caja);
        transaccion.Commit();
        return caja;
    }

    // la seleccion debe seguir siendo la caja activa para que una pantalla vieja no edite el turno siguiente
    public void GuardarCajaAbierta(CierreCaja caja)
    {
        if (caja.IdCaja <= 0)
        {
            throw new InvalidOperationException("Abra una caja antes de guardar sus datos.");
        }
        using SqlConnection conexion = new SqlConnection(Conexion.ObtenerCadena());
        conexion.Open();
        using SqlTransaction transaccion = conexion.BeginTransaction();
        ExigirCajaAbierta(conexion, transaccion, Ahora(conexion, transaccion), caja.IdCaja);
        GuardarDatos(conexion, transaccion, caja);
        transaccion.Commit();
    }

    // modifica solo los datos del turno abierto conservando su fila y la hora real de apertura
    private static void GuardarDatos(SqlConnection conexion, SqlTransaction transaccion, CierreCaja caja)
    {
        using SqlCommand comando = Comando(conexion, transaccion, @"
            UPDATE Caja SET datos=@datos WHERE id=@id AND estado=N'Abierta';
            IF @@ROWCOUNT<>1 THROW 50007,N'La caja ya no está abierta. Actualice la pantalla.',1;",
            ("@id", caja.IdCaja), ("@datos", JsonSerializer.Serialize(caja)));
        comando.ExecuteNonQuery();
    }

    // consulta una apertura concreta o la ultima de esa fecha sin abrir otra automaticamente
    public CierreCaja Consultar(DateTime fecha, int? idCaja = null)
    {
        using SqlConnection conexion = new SqlConnection(Conexion.ObtenerCadena());
        conexion.Open();
        using SqlTransaction transaccion = conexion.BeginTransaction(IsolationLevel.Serializable);
        BloquearCaja(conexion, transaccion);
        CierreCaja? resultado = null;
        using (SqlCommand comando = Comando(conexion, transaccion, @"
            SELECT TOP (1) id,fecha,estado,inicio,fin,datos FROM Caja
            WHERE fecha=@fecha AND (@id IS NULL OR id=@id) ORDER BY id DESC;", ("@fecha", fecha.Date), ("@id", idCaja)))
        using (SqlDataReader lector = comando.ExecuteReader())
        {
            if (lector.Read())
            {
                resultado = LeerCabecera(lector);
            }
        }
        if (resultado == null)
        {
            if (idCaja.HasValue)
            {
                throw new InvalidOperationException("La caja seleccionada no existe en esa fecha.");
            }
            resultado = new CierreCaja { Fecha = fecha.Date, Inicio = fecha.Date + DateTime.Now.TimeOfDay };
        }
        else if (!resultado.Cerrada)
        {
            resultado.Movimientos = LeerMovimientos(conexion, transaccion, fecha.Date, fecha.Date.AddDays(1), resultado.IdCaja);
        }
        else
        {
            // corrige bases de cierres antiguos en memoria sin incluir pagos de otros turnos
            using SqlCommand pagos = Comando(conexion, transaccion, "SELECT id_pago,monto FROM Pago WHERE id_caja=@id", ("@id", resultado.IdCaja));
            using SqlDataReader lector = pagos.ExecuteReader();
            Dictionary<int, decimal> bases = new Dictionary<int, decimal>();
            while (lector.Read())
            {
                bases[lector.GetInt32(0)] = lector.GetDecimal(1);
            }
            foreach (MovimientoCaja movimiento in resultado.Movimientos)
            {
                movimiento.IdCaja = resultado.IdCaja;
                if (movimiento.Venta.HasValue && bases.TryGetValue(movimiento.Id, out decimal importe))
                {
                    movimiento.Importe = importe;
                }
            }
        }
        transaccion.Commit();
        return resultado;
    }

    // el resumen mensual consulta operaciones originales sin sumar varias veces los cierres diarios
    public List<MovimientoCaja> ConsultarPeriodo(DateTime desde, DateTime hasta)
    {
        using SqlConnection conexion = new SqlConnection(Conexion.ObtenerCadena());
        conexion.Open();
        using SqlTransaction transaccion = conexion.BeginTransaction(IsolationLevel.Serializable);
        List<MovimientoCaja> movimientos = LeerMovimientos(conexion, transaccion, desde, hasta, null);
        transaccion.Commit();
        return movimientos;
    }

    // id_caja separa las aperturas del dia y el intervalo de fechas limita la consulta mensual
    private static List<MovimientoCaja> LeerMovimientos(SqlConnection conexion, SqlTransaction transaccion, DateTime desde, DateTime hasta, int? idCaja)
    {
        using SqlCommand comando = Comando(conexion, transaccion, @"
            SELECT p.id_pago id,p.id_venta venta,p.fecha_pago fecha,
                CONCAT(c.nombre_cliente,' ',c.apellido_cliente,' - ',m.nombre_medio_pago) concepto,
                N'Cobro venta' tipo,COALESCE(p.tipo_caja,m.tipo_caja) medio,'ARS' moneda,p.monto importe,
                p.cuenta_destino cuenta,CAST(p.id_usuario AS nvarchar(20)) usuario,p.id_caja
            FROM Pago p JOIN Venta v ON v.id_venta=p.id_venta JOIN Cliente c ON c.id_cliente=v.id_cliente
            JOIN Medio_Pago m ON m.id_medio_pago=p.id_medio_pago
            WHERE p.fecha_pago>=@desde AND p.fecha_pago<@hasta AND (@caja IS NULL OR p.id_caja=@caja)
            UNION ALL
            SELECT id,NULL,fecha,concepto,tipo,medio,moneda,importe,cuenta,CAST(id_usuario AS nvarchar(20)),id_caja
            FROM Caja_Movimiento WHERE fecha>=@desde AND fecha<@hasta AND anulado=0
                AND (@caja IS NULL OR id_caja=@caja) ORDER BY fecha,id,venta;", ("@desde", desde), ("@hasta", hasta), ("@caja", idCaja));
        using SqlDataReader lector = comando.ExecuteReader();
        List<MovimientoCaja> movimientos = new List<MovimientoCaja>();
        while (lector.Read())
        {
            int? venta = null;
            if (!lector.IsDBNull(1))
            {
                venta = lector.GetInt32(1);
            }
            movimientos.Add(new MovimientoCaja
            {
                Id = lector.GetInt32(0), Venta = venta, Fecha = lector.GetDateTime(2), Concepto = lector.GetString(3),
                Tipo = lector.GetString(4), Medio = lector.GetString(5), Moneda = lector.GetString(6),
                Importe = lector.GetDecimal(7), Cuenta = lector.GetString(8), Usuario = lector.GetString(9), IdCaja = lector.GetInt32(10)
            });
        }
        return movimientos;
    }

    // vincula el movimiento con la apertura vigente y rechaza la seleccion de un turno ya cerrado
    public void Registrar(MovimientoCaja movimiento, int usuario)
    {
        using SqlConnection conexion = new SqlConnection(Conexion.ObtenerCadena());
        conexion.Open();
        using SqlTransaction transaccion = conexion.BeginTransaction();
        DateTime ahora = Ahora(conexion, transaccion);
        int? esperada = null;
        if (movimiento.IdCaja > 0)
        {
            esperada = movimiento.IdCaja;
        }
        int caja = ExigirCajaAbierta(conexion, transaccion, ahora, esperada);
        using SqlCommand comando = Comando(conexion, transaccion, @"
            INSERT Caja_Movimiento(fecha,concepto,tipo,medio,moneda,importe,cuenta,id_usuario,id_caja)
            VALUES(@fecha,@concepto,@tipo,@medio,@moneda,@importe,@cuenta,@usuario,@caja);",
            ("@fecha", ahora), ("@concepto", movimiento.Concepto), ("@tipo", movimiento.Tipo), ("@medio", movimiento.Medio),
            ("@moneda", movimiento.Moneda), ("@importe", movimiento.Importe), ("@cuenta", movimiento.Cuenta), ("@usuario", usuario), ("@caja", caja));
        comando.ExecuteNonQuery();
        transaccion.Commit();
    }

    // permite anular solamente un movimiento manual de la caja que sigue abierta
    public void Anular(int id, string motivo, int usuario)
    {
        using SqlConnection conexion = new SqlConnection(Conexion.ObtenerCadena());
        conexion.Open();
        using SqlTransaction transaccion = conexion.BeginTransaction();
        BloquearCaja(conexion, transaccion);
        using SqlCommand buscar = Comando(conexion, transaccion, "SELECT id_caja FROM Caja_Movimiento WHERE id=@id", ("@id", id));
        object? encontrada = buscar.ExecuteScalar();
        if (encontrada == null)
        {
            throw new InvalidOperationException("El movimiento no existe.");
        }
        ExigirCajaAbierta(conexion, transaccion, Ahora(conexion, transaccion), Convert.ToInt32(encontrada));
        using SqlCommand comando = Comando(conexion, transaccion, @"
            UPDATE Caja_Movimiento SET anulado=1,motivo_anulacion=@motivo WHERE id=@id AND anulado=0;
            IF @@ROWCOUNT<>1 THROW 50005,N'El movimiento ya fue anulado.',1;
            INSERT LogActividad(id_usuario,accion,entidad,id_entidad,detalle)
            VALUES(@usuario,N'ANULAR_MOVIMIENTO',N'Caja',@id,@motivo);", ("@id", id), ("@motivo", motivo), ("@usuario", usuario));
        comando.ExecuteNonQuery();
        transaccion.Commit();
    }

    // comprueba el estado por identificador dentro del bloqueo compartido con los cobros
    private static CierreCaja ObtenerParaCerrar(SqlConnection conexion, SqlTransaction transaccion, int idCaja)
    {
        using SqlCommand comando = Comando(conexion, transaccion, "SELECT id,fecha,estado,inicio,fin,datos FROM Caja WHERE id=@id", ("@id", idCaja));
        using SqlDataReader lector = comando.ExecuteReader();
        if (!lector.Read())
        {
            throw new InvalidOperationException("Seleccione una caja abierta.");
        }
        CierreCaja caja = LeerCabecera(lector);
        if (caja.Cerrada)
        {
            throw new InvalidOperationException("La caja seleccionada ya está cerrada.");
        }
        return caja;
    }

    // cierra solo la apertura seleccionada permite abrir otra caja de la misma fecha despues de confirmar
    public CierreCaja Cerrar(CierreCaja solicitud, int usuario)
    {
        using SqlConnection conexion = new SqlConnection(Conexion.ObtenerCadena());
        conexion.Open();
        using SqlTransaction transaccion = conexion.BeginTransaction();
        BloquearCaja(conexion, transaccion);
        CierreCaja vigente = ObtenerParaCerrar(conexion, transaccion, solicitud.IdCaja);
        List<MovimientoCaja> movimientos = LeerMovimientos(conexion, transaccion, vigente.Fecha, vigente.Fecha.AddDays(1), vigente.IdCaja);
        if (JsonSerializer.Serialize(solicitud.Movimientos) != JsonSerializer.Serialize(movimientos))
        {
            throw new InvalidOperationException("Se registraron movimientos nuevos. Actualice y revise los totales antes de cerrar.");
        }
        vigente.Movimientos = movimientos;
        vigente.SaldoInicial = solicitud.SaldoInicial;
        vigente.SaldoInicialUsd = solicitud.SaldoInicialUsd;
        vigente.Observaciones = solicitud.Observaciones;
        vigente.Billetes = solicitud.Billetes;
        vigente.Fin = Ahora(conexion, transaccion);
        using SqlCommand operador = Comando(conexion, transaccion, "SELECT nombre_usuario FROM Usuario WHERE id_usuario=@usuario", ("@usuario", usuario));
        vigente.Usuario = Convert.ToString(operador.ExecuteScalar()) ?? usuario.ToString();
        GuardarCierre(conexion, transaccion, vigente, usuario);
        transaccion.Commit();
        return vigente;
    }

    // conserva la fila y el identificador de apertura al guardar el json definitivo
    private static void GuardarCierre(SqlConnection conexion, SqlTransaction transaccion, CierreCaja caja, int? usuario)
    {
        caja.Cerrada = true;
        caja.Numero = caja.IdCaja;
        using SqlCommand comando = Comando(conexion, transaccion, @"
            UPDATE Caja SET estado=N'Cerrada',fin=@fin,id_usuario=@usuario,fecha_cierre=SYSDATETIME(),datos=@datos
            WHERE id=@id AND estado=N'Abierta';
            IF @@ROWCOUNT<>1 THROW 50004,N'La caja ya está cerrada. Actualice la pantalla.',1;",
            ("@id", caja.IdCaja), ("@fin", caja.Fin), ("@usuario", usuario), ("@datos", JsonSerializer.Serialize(caja)));
        comando.ExecuteNonQuery();
    }

    // recupera aperturas de dias anteriores sin modificar los cierres manuales de otros turnos
    public DateTime CerrarDiasAnteriores()
    {
        using SqlConnection conexion = new SqlConnection(Conexion.ObtenerCadena());
        conexion.Open();
        using SqlTransaction transaccion = conexion.BeginTransaction();
        BloquearCaja(conexion, transaccion);
        DateTime hoy = Ahora(conexion, transaccion).Date;
        List<int> pendientes = new List<int>();
        using (SqlCommand comando = Comando(conexion, transaccion, "SELECT id FROM Caja WHERE estado=N'Abierta' AND fecha<@hoy ORDER BY id", ("@hoy", hoy)))
        using (SqlDataReader lector = comando.ExecuteReader())
        {
            while (lector.Read())
            {
                pendientes.Add(lector.GetInt32(0));
            }
        }
        foreach (int id in pendientes)
        {
            CierreCaja caja = ObtenerParaCerrar(conexion, transaccion, id);
            caja.Movimientos = LeerMovimientos(conexion, transaccion, caja.Fecha, caja.Fecha.AddDays(1), id);
            caja.Fin = caja.Fecha.AddDays(1);
            caja.Usuario = "CIERRE AUTOMÁTICO";
            GuardarCierre(conexion, transaccion, caja, null);
        }
        transaccion.Commit();
        return hoy;
    }
}
