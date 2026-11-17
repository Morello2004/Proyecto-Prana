using Microsoft.Data.SqlClient;
using Prana.Entidades;
using System.Data;
using static Prana.Datos.GestionVentaSql;

namespace Prana.Datos;

// administra aperturas por identificador para separar turnos de una misma fecha sin sumar recargos
public class CD_Caja
{
    private const string CabeceraSql = @"SELECT c.id_caja,c.fecha_operativa,c.estado,c.fecha_hora_apertura,c.fecha_hora_fin_turno,c.observaciones,c.numero_cierre,
        CASE WHEN c.estado=N'Cerrada' AND c.id_usuario_cierre IS NULL THEN N'CIERRE AUTOMÁTICO'
             ELSE COALESCE(u.nombre_usuario,N'') END usuario
        FROM Caja c LEFT JOIN Usuario u ON u.id_usuario=c.id_usuario_cierre";
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
        using SqlCommand comando = Comando(conexion, transaccion, "SELECT id_caja,fecha_operativa FROM Caja WHERE estado=N'Abierta'");
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

    // La cabecera se lee de columnas; los detalles se consultan por su clave foránea.
    private static CierreCaja LeerCabecera(SqlDataReader lector)
    {
        CierreCaja caja = new()
        {
            IdCaja = lector.GetInt32(0), Fecha = lector.GetDateTime(1),
            Cerrada = lector.GetString(2) == "Cerrada", Inicio = lector.GetDateTime(3),
            Observaciones = lector.GetString(5), Usuario = lector.GetString(7)
        };
        if (!lector.IsDBNull(4))
        {
            caja.Fin = lector.GetDateTime(4);
        }
        caja.Numero = 0;
        if (caja.Cerrada)
        {
            caja.Numero = lector.GetInt32(6);
        }
        return caja;
    }

    // ofrece todas las aperturas de una fecha para consultar cada cierre individual
    public List<CierreCaja> Listar(DateTime fecha)
    {
        using SqlConnection conexion = new SqlConnection(Conexion.ObtenerCadena());
        using SqlCommand comando = new SqlCommand(CabeceraSql + " WHERE c.fecha_operativa=@fecha ORDER BY c.id_caja DESC", conexion);
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

    // Cada apertura registra sus propios movimientos.
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
            Fecha = ahora.Date, Inicio = ahora,
            Observaciones = solicitud.Observaciones
        };
        using SqlCommand abrir = Comando(conexion, transaccion, @"
            INSERT Caja(fecha_operativa,estado,fecha_hora_apertura,id_usuario_apertura,observaciones)
            OUTPUT INSERTED.id_caja VALUES(@fecha,N'Abierta',@inicio,@usuario,@observaciones);",
            ("@fecha", caja.Fecha), ("@inicio", caja.Inicio), ("@usuario", usuario), ("@observaciones", caja.Observaciones));
        caja.IdCaja = Convert.ToInt32(abrir.ExecuteScalar());
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
        GuardarObservaciones(conexion, transaccion, caja);
        transaccion.Commit();
    }

    // modifica solo los datos del turno abierto conservando su fila y la hora real de apertura
    private static void GuardarObservaciones(SqlConnection conexion, SqlTransaction transaccion, CierreCaja caja)
    {
        using SqlCommand comando = Comando(conexion, transaccion, @"
            UPDATE Caja SET observaciones=@observaciones WHERE id_caja=@id AND estado=N'Abierta';
            IF @@ROWCOUNT<>1 THROW 50007,N'La caja ya no está abierta. Actualice la pantalla.',1;",
            ("@id", caja.IdCaja), ("@observaciones", caja.Observaciones));
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
        using (SqlCommand comando = Comando(conexion, transaccion,
            CabeceraSql.Replace("SELECT c.id_caja", "SELECT TOP (1) c.id_caja") +
            " WHERE c.fecha_operativa=@fecha AND (@id IS NULL OR c.id_caja=@id) ORDER BY c.id_caja DESC;", ("@fecha", fecha.Date), ("@id", idCaja)))
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
        else
        {
            resultado.Movimientos = LeerMovimientos(conexion, transaccion, fecha.Date, fecha.Date.AddDays(1), resultado.IdCaja);
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
            SELECT p.id_pago id,p.id_venta venta,p.fecha_hora_pago fecha,
                CONCAT(c.nombre_cliente,' ',c.apellido_cliente,' - ',m.nombre_medio_pago) concepto,
                N'Cobro venta' tipo,COALESCE(p.clasificacion_cobro,m.clasificacion_cobro) medio,'ARS' moneda,p.importe_base importe,
                p.cuenta_destino cuenta,COALESCE(u.nombre_usuario,N'USUARIO ELIMINADO') usuario,p.id_caja
            FROM Pago p JOIN Venta v ON v.id_venta=p.id_venta JOIN Cliente c ON c.id_cliente=v.id_cliente
            JOIN Medio_Pago m ON m.id_medio_pago=p.id_medio_pago LEFT JOIN Usuario u ON u.id_usuario=p.id_usuario
            WHERE p.fecha_hora_pago>=@desde AND p.fecha_hora_pago<@hasta AND (@caja IS NULL OR p.id_caja=@caja)
            UNION ALL
            SELECT cm.id_movimiento_caja,NULL,cm.fecha_hora_movimiento,concepto,cm.tipo_movimiento,cm.medio_movimiento,moneda,importe,cuenta,COALESCE(u.nombre_usuario,N'USUARIO ELIMINADO'),cm.id_caja
            FROM Caja_Movimiento cm LEFT JOIN Usuario u ON u.id_usuario=cm.id_usuario WHERE cm.fecha_hora_movimiento>=@desde AND cm.fecha_hora_movimiento<@hasta AND anulado=0
                AND (@caja IS NULL OR cm.id_caja=@caja) ORDER BY fecha,id,venta;", ("@desde", desde), ("@hasta", hasta), ("@caja", idCaja));
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
            INSERT Caja_Movimiento(fecha_hora_movimiento,concepto,tipo_movimiento,medio_movimiento,moneda,importe,cuenta,id_usuario,id_caja)
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
        using SqlCommand buscar = Comando(conexion, transaccion, "SELECT id_caja FROM Caja_Movimiento WHERE id_movimiento_caja=@id", ("@id", id));
        object? encontrada = buscar.ExecuteScalar();
        if (encontrada == null)
        {
            throw new InvalidOperationException("El movimiento no existe.");
        }
        ExigirCajaAbierta(conexion, transaccion, Ahora(conexion, transaccion), Convert.ToInt32(encontrada));
        using SqlCommand comando = Comando(conexion, transaccion, @"
            UPDATE Caja_Movimiento SET anulado=1,motivo_anulacion=@motivo WHERE id_movimiento_caja=@id AND anulado=0;
            IF @@ROWCOUNT<>1 THROW 50005,N'El movimiento ya fue anulado.',1;
            INSERT Log_Actividad(id_usuario,accion,entidad,id_entidad,detalle)
            VALUES(@usuario,N'ANULAR_MOVIMIENTO',N'Caja',@id,@motivo);", ("@id", id), ("@motivo", motivo), ("@usuario", usuario));
        comando.ExecuteNonQuery();
        transaccion.Commit();
    }

    // comprueba el estado por identificador dentro del bloqueo compartido con los cobros
    private static CierreCaja ObtenerParaCerrar(SqlConnection conexion, SqlTransaction transaccion, int idCaja)
    {
        using SqlCommand comando = Comando(conexion, transaccion, CabeceraSql + " WHERE c.id_caja=@id", ("@id", idCaja));
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
        static object Operacion(MovimientoCaja m) => (m.Id, m.Venta, m.Fecha, m.Concepto, m.Tipo, m.Medio, m.Moneda, m.Importe, m.Cuenta, m.Usuario, m.IdCaja);
        if (!solicitud.Movimientos.Select(Operacion).SequenceEqual(movimientos.Select(Operacion)))
        {
            throw new InvalidOperationException("Se registraron movimientos nuevos. Actualice y revise los totales antes de cerrar.");
        }
        vigente.Movimientos = movimientos;
        vigente.Observaciones = solicitud.Observaciones;
        vigente.Fin = Ahora(conexion, transaccion);
        using SqlCommand operador = Comando(conexion, transaccion, "SELECT nombre_usuario FROM Usuario WHERE id_usuario=@usuario", ("@usuario", usuario));
        vigente.Usuario = Convert.ToString(operador.ExecuteScalar()) ?? usuario.ToString();
        GuardarCierre(conexion, transaccion, vigente, usuario);
        transaccion.Commit();
        return vigente;
    }

    // Confirma el cierre sin copiar pagos ni movimientos ni totales calculados.
    private static void GuardarCierre(SqlConnection conexion, SqlTransaction transaccion, CierreCaja caja, int? usuario)
    {
        caja.Cerrada = true;
        using (SqlCommand numerar = Comando(conexion, transaccion,
            "SELECT CASE WHEN COALESCE(MAX(numero_cierre),499)<499 THEN 500 ELSE COALESCE(MAX(numero_cierre),499)+1 END FROM Caja WITH (UPDLOCK,HOLDLOCK)"))
        {
            caja.Numero = Convert.ToInt32(numerar.ExecuteScalar());
        }
        using SqlCommand comando = Comando(conexion, transaccion, @"
            UPDATE Caja SET estado=N'Cerrada',numero_cierre=@numero,fecha_hora_fin_turno=@fin,id_usuario_cierre=@usuario,fecha_hora_registro_cierre=SYSDATETIME(),observaciones=@observaciones
            WHERE id_caja=@id AND estado=N'Abierta';
            IF @@ROWCOUNT<>1 THROW 50004,N'La caja ya está cerrada. Actualice la pantalla.',1;",
            ("@numero", caja.Numero), ("@id", caja.IdCaja), ("@fin", caja.Fin), ("@usuario", usuario), ("@observaciones", caja.Observaciones));
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
        using (SqlCommand comando = Comando(conexion, transaccion, "SELECT id_caja FROM Caja WHERE estado=N'Abierta' AND fecha_operativa<@hoy ORDER BY id_caja", ("@hoy", hoy)))
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
