// guia del archivo: une cobros de ventas con ingresos y gastos manuales, segun su fecha de movimiento regla del comercio: los cobros de caja usan pagomonto (base), no monto_final (base con recargo) los cierres se guardan como una fotografia json; al leer cierres antiguos se ajusta la base de pagos solo en memoria el saldo inicial no es un cobro nuevo; los movimientos en ars y usd se mantienen separados el bloqueo por fecha protege el cierre frente a otros cobros concurrentes del mismo dia
using Microsoft.Data.SqlClient;
using Prana.Entidades;
using System.Data;
using System.Text.Json;
using static Prana.Datos.GestionVentaSql;

namespace Prana.Datos;
// administra las consultas y operaciones de movimientos y cierres de caja
public class CD_Caja
{
    // usa el mismo bloqueo que los pagos para no cerrar mientras se registra un movimiento del dia
    private static void BloquearDia(SqlConnection conexion, SqlTransaction transaccion, DateTime fecha)
    {
        using SqlCommand bloqueo = Comando(conexion, transaccion, @"
            DECLARE @r int;
            EXEC @r=sys.sp_getapplock @Resource=@clave,@LockMode='Exclusive',@LockOwner='Transaction',@LockTimeout=15000;
            IF @r<0 THROW 50003,N'La caja está ocupada. Reintente.',1;",
            ("@clave", "Prana.Caja." + fecha.ToString("yyyyMMdd")));
        bloqueo.ExecuteNonQuery();
    }

    // ejecuta la consulta de leer caja abierta y obtiene el primer valor del resultado
    private static CierreCaja LeerCajaAbierta(SqlConnection conexion, SqlTransaction transaccion, DateTime fecha)
    {
        using SqlCommand comando = Comando(conexion, transaccion, "SELECT datos FROM Caja_Abierta WHERE fecha=@fecha", ("@fecha", fecha.Date));
        if (comando.ExecuteScalar() is string datos)
        {
            return JsonSerializer.Deserialize<CierreCaja>(datos)!;
        }

        return new CierreCaja { Fecha = fecha.Date, Inicio = fecha.Date + DateTime.Now.TimeOfDay };
    }

    // guarda solo los datos editables los movimientos se consultan nuevamente al cerrar
    public void GuardarCajaAbierta(CierreCaja caja)
    {
        // prepara la conexion configurada y la libera al terminar este alcance
        using SqlConnection conexion = new SqlConnection(Conexion.ObtenerCadena());
        conexion.Open();
        using SqlTransaction transaccion = conexion.BeginTransaction();
        ExigirDiaAbierto(conexion, transaccion, caja.Fecha);
        if (caja.Fecha.Date != Ahora(conexion, transaccion).Date)
        {
            throw new InvalidOperationException("Cambió el día. Actualice la caja antes de modificar sus datos.");
        }

        CierreCaja datos = new CierreCaja
        {
            Fecha = caja.Fecha.Date, Inicio = caja.Inicio, SaldoInicial = caja.SaldoInicial,
            SaldoInicialUsd = caja.SaldoInicialUsd, Observaciones = caja.Observaciones
        };
        using SqlCommand guardar = Comando(conexion, transaccion, @"
            UPDATE Caja_Abierta SET datos=@datos WHERE fecha=@fecha;
            IF @@ROWCOUNT=0 INSERT Caja_Abierta(fecha,datos) VALUES(@fecha,@datos);",
            ("@fecha", datos.Fecha), ("@datos", JsonSerializer.Serialize(datos)));
        guardar.ExecuteNonQuery();
        transaccion.Commit();
    }

    // completa los dias pendientes incluso si la aplicacion estaba apagada a medianoche
    public DateTime CerrarDiasAnteriores()
    {
        // prepara la conexion configurada y la libera al terminar este alcance
        using SqlConnection conexion = new SqlConnection(Conexion.ObtenerCadena());
        conexion.Open();
        List<DateTime> pendientes = new List<DateTime>();
        DateTime hoy;
        using (SqlTransaction transaccion = conexion.BeginTransaction())
        {
            hoy = Ahora(conexion, transaccion).Date;
            using SqlCommand consulta = Comando(conexion, transaccion, @"
                SELECT fecha FROM (
                    SELECT fecha FROM Caja_Abierta
                    UNION SELECT CAST(fecha_pago AS date) FROM Pago
                    UNION SELECT CAST(fecha AS date) FROM Caja_Movimiento
                ) dias WHERE fecha<@hoy AND NOT EXISTS(SELECT 1 FROM Caja_Cierre c WHERE c.fecha=dias.fecha)
                ORDER BY fecha", ("@hoy", hoy));
            using (SqlDataReader lector = consulta.ExecuteReader())
            {
                while (lector.Read()) pendientes.Add(lector.GetDateTime(0));
            }
            transaccion.Commit();
        }

        // procesa cada fecha de pendientes sin cambiar el orden de la coleccion
        foreach (DateTime fecha in pendientes)
        {
            using SqlTransaction transaccion = conexion.BeginTransaction();
            BloquearDia(conexion, transaccion, fecha);
            using SqlCommand existe = Comando(conexion, transaccion, "SELECT COUNT(*) FROM Caja_Cierre WHERE fecha=@fecha", ("@fecha", fecha));
            if (Convert.ToInt32(existe.ExecuteScalar()) == 0)
            {
                CierreCaja cierre = LeerCajaAbierta(conexion, transaccion, fecha);
                cierre.Movimientos = LeerMovimientos(conexion, transaccion, fecha);
                // la medianoche es el limite del dia cerrado no la hora a la que se recupero el cierre pendiente
                cierre.Fin = fecha.AddDays(1);
                cierre.Usuario = "CIERRE AUTOMÁTICO";
                using SqlCommand guardar = Comando(conexion, transaccion,
                    "INSERT Caja_Cierre(fecha,id_usuario,datos) VALUES(@fecha,NULL,@datos)",
                    ("@fecha", fecha), ("@datos", JsonSerializer.Serialize(cierre)));
                guardar.ExecuteNonQuery();
            }
            using SqlCommand limpiar = Comando(conexion, transaccion, "DELETE Caja_Abierta WHERE fecha=@fecha", ("@fecha", fecha));
            limpiar.ExecuteNonQuery();
            transaccion.Commit();
        }
        return hoy;
    }

    // obtiene la fecha y hora actual desde el servidor de base de datos
    internal static DateTime Ahora(SqlConnection cn, SqlTransaction tx)
    {
        // usa el reloj del servidor de base de datos en lugar del reloj del equipo cliente
        using SqlCommand comando = Comando(cn, tx, "SELECT SYSDATETIME()");
        return (DateTime)comando.ExecuteScalar()!;
    }

    // bloquea el dia dentro de la transaccion y evita operar sobre una caja ya cerrada comprueba exigir dia abierto; si no se cumple la regla interrumpe con un mensaje antes de continuar
    internal static void ExigirDiaAbierto(SqlConnection cn, SqlTransaction tx, DateTime fecha, bool permitirDiaAnterior = false)
    {
        using SqlCommand comando = Comando(cn, tx, @"
            DECLARE @r int;
            EXEC @r=sys.sp_getapplock @Resource=@clave,@LockMode='Exclusive',@LockOwner='Transaction',@LockTimeout=15000;
            IF @r<0 THROW 50003,N'La caja está ocupada. Reintente.',1;
            IF @permitirAnterior=0 AND @fecha<CONVERT(date,SYSDATETIME())
                THROW 50006,N'Cambió el día. No se pueden modificar movimientos de una fecha anterior.',1;
            IF EXISTS(SELECT 1 FROM Caja_Cierre WHERE fecha=@fecha)
                THROW 50004,N'La caja de ese día ya está cerrada. No se pueden agregar cobros ni gastos.',1;", ("@clave", "Prana.Caja." + fecha.ToString("yyyyMMdd")), ("@fecha", fecha.Date), ("@permitirAnterior", permitirDiaAnterior));
        comando.ExecuteNonQuery();
    }

    // busca el cierre del dia o construye una caja abierta con sus movimientos actuales
    public CierreCaja Consultar(DateTime fecha)
    {
        // prepara la conexion configurada y la libera al terminar este alcance
        using SqlConnection conexion = new SqlConnection(Conexion.ObtenerCadena());
        conexion.Open();
        using SqlTransaction transaccion = conexion.BeginTransaction(IsolationLevel.Serializable);
        BloquearDia(conexion, transaccion, fecha);
        // busca el cierre guardado como texto json para la fecha solicitada
        using SqlCommand comando = Comando(conexion, transaccion, "SELECT id,datos FROM Caja_Cierre WHERE fecha=@fecha", ("@fecha", fecha.Date));
        // permite distinguir entre un cierre guardado y un dia que todavia no tiene cierre
        CierreCaja? resultado = null;
        // limita la vida del recurso al bloque; al salir se ejecuta su liberacion incluso ante una excepcion
        using (SqlDataReader r = comando.ExecuteReader())
        {
            if (r.Read())
            {
                resultado = JsonSerializer.Deserialize<CierreCaja>(r.GetString(1))!;
                resultado.Numero = r.GetInt32(0);
            }
        }

        // los cierres anteriores pueden contener el cobro con recargo al consultarlos, presenta solo la base de cada pago, conservando los demas datos del cierre
        if (resultado != null)
        {
            using SqlCommand pagos = Comando(conexion, transaccion, "SELECT id_pago,monto FROM Pago WHERE fecha_pago>=@desde AND fecha_pago<@hasta", ("@desde", fecha.Date), ("@hasta", fecha.Date.AddDays(1)));
            using SqlDataReader lector = pagos.ExecuteReader();
            Dictionary<int, decimal> importes = new Dictionary<int, decimal>();
            while (lector.Read())
            {
                importes[lector.GetInt32(0)] = lector.GetDecimal(1);
            }

            // procesa cada movimiento de resultadomovimientoswhere(m => mventahas value) sin cambiar el orden de la coleccion
            foreach (MovimientoCaja movimiento in resultado.Movimientos.Where(m => m.Venta.HasValue))
            {
                if (importes.TryGetValue(movimiento.Id, out decimal importeBase))
                {
                    movimiento.Importe = importeBase;
                }
            }
        }

        // crea una caja abierta solo cuando la consulta anterior no encontro un cierre
        if (resultado == null)
        {
            resultado = LeerCajaAbierta(conexion, transaccion, fecha);
            resultado.Movimientos = LeerMovimientos(conexion, transaccion, fecha);
        }
        // confirma todos los cambios de esta transaccion como una unidad
        transaccion.Commit();
        return resultado;
    }

    // obtiene los movimientos desde el inicio del dia hasta el inicio del dia siguiente
    private static List<MovimientoCaja> LeerMovimientos(SqlConnection cn, SqlTransaction tx, DateTime fecha) => LeerMovimientos(cn, tx, fecha.Date, fecha.Date.AddDays(1));
    // consulta los movimientos de un intervalo usando la fecha inicial y el limite final exclusivo
    public List<MovimientoCaja> ConsultarPeriodo(DateTime desde, DateTime hasta)
    {
        // prepara la conexion configurada y la libera al terminar este alcance
        using SqlConnection conexion = new SqlConnection(Conexion.ObtenerCadena());
        conexion.Open();
        using SqlTransaction transaccion = conexion.BeginTransaction(IsolationLevel.Serializable);
        List<MovimientoCaja> resultado = LeerMovimientos(conexion, transaccion, desde, hasta);
        // confirma todos los cambios de esta transaccion como una unidad
        transaccion.Commit();
        return resultado;
    }

    // combina los cobros por fecha de pago con los movimientos manuales no anulados
    private static List<MovimientoCaja> LeerMovimientos(SqlConnection cn, SqlTransaction tx, DateTime desde, DateTime hasta)
    {
        using SqlCommand comando = Comando(cn, tx, @"
            SELECT p.id_pago id,p.id_venta venta,p.fecha_pago fecha,
                CONCAT(c.nombre_cliente,' ',c.apellido_cliente,' - ',m.nombre_medio_pago) concepto,
                N'Cobro venta' tipo,COALESCE(p.tipo_caja,m.tipo_caja) medio,'ARS' moneda,p.monto importe,
                p.cuenta_destino cuenta,CAST(p.id_usuario AS nvarchar(20)) usuario
            FROM Pago p JOIN Venta v ON v.id_venta=p.id_venta JOIN Cliente c ON c.id_cliente=v.id_cliente
            JOIN Medio_Pago m ON m.id_medio_pago=p.id_medio_pago
            WHERE p.fecha_pago>=@desde AND p.fecha_pago<@hasta
            UNION ALL
            SELECT id,NULL,fecha,concepto,tipo,medio,moneda,importe,cuenta,CAST(id_usuario AS nvarchar(20))
            FROM Caja_Movimiento WHERE fecha>=@desde AND fecha<@hasta AND anulado=0 ORDER BY fecha,id", ("@desde", desde), ("@hasta", hasta));
        using SqlDataReader lector = comando.ExecuteReader();
        List<MovimientoCaja> movimientos = new List<MovimientoCaja>();
        while (lector.Read())
        {
            int? valorVenta;
            if (lector.IsDBNull(1))
            {
                valorVenta = null;
            }
            else
            {
                valorVenta = lector.GetInt32(1);
            }
            movimientos.Add(new MovimientoCaja
            {
                Id = lector.GetInt32(0),
                Venta = valorVenta,
                Fecha = lector.GetDateTime(2),
                Concepto = lector.GetString(3),
                Tipo = lector.GetString(4),
                Medio = lector.GetString(5),
                Moneda = lector.GetString(6),
                Importe = lector.GetDecimal(7),
                Cuenta = lector.GetString(8),
                Usuario = lector.GetString(9)

            });
        }

        return movimientos;
    }

    // registra un movimiento manual si la caja del dia actual sigue abierta
    public void Registrar(MovimientoCaja movimiento, int usuario)
    {
        // prepara la conexion configurada y la libera al terminar este alcance
        using SqlConnection conexion = new SqlConnection(Conexion.ObtenerCadena());
        conexion.Open();
        using SqlTransaction transaccion = conexion.BeginTransaction();
        DateTime ahora = Ahora(conexion, transaccion);
        ExigirDiaAbierto(conexion, transaccion, ahora);
        using SqlCommand comando = Comando(conexion, transaccion, @"INSERT Caja_Movimiento(fecha,concepto,tipo,medio,moneda,importe,cuenta,id_usuario)
            VALUES(@fecha,@concepto,@tipo,@medio,@moneda,@importe,@cuenta,@usuario)", ("@fecha", ahora), ("@concepto", movimiento.Concepto), ("@tipo", movimiento.Tipo), ("@medio", movimiento.Medio), ("@moneda", movimiento.Moneda), ("@importe", movimiento.Importe), ("@cuenta", movimiento.Cuenta), ("@usuario", usuario));
        comando.ExecuteNonQuery();
        // confirma todos los cambios de esta transaccion como una unidad
        transaccion.Commit();
    }

    // marca un movimiento como anulado y registra el motivo en la auditoria
    public void Anular(int id, string motivo, int usuario)
    {
        // prepara la conexion configurada y la libera al terminar este alcance
        using SqlConnection conexion = new SqlConnection(Conexion.ObtenerCadena());
        conexion.Open();
        using SqlTransaction transaccion = conexion.BeginTransaction();
        using SqlCommand fechaCmd = Comando(conexion, transaccion, "SELECT fecha FROM Caja_Movimiento WHERE id=@id", ("@id", id));
        if (fechaCmd.ExecuteScalar() is not DateTime fecha)
        {
            // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
            throw new InvalidOperationException("El movimiento no existe.");
        }

        ExigirDiaAbierto(conexion, transaccion, fecha);
        using SqlCommand comando = Comando(conexion, transaccion, @"UPDATE Caja_Movimiento SET anulado=1,motivo_anulacion=@motivo
            WHERE id=@id AND anulado=0;
            IF @@ROWCOUNT<>1 THROW 50005,N'El movimiento ya fue anulado.',1;
            INSERT LogActividad(id_usuario,accion,entidad,id_entidad,detalle)
            VALUES(@usuario,N'ANULAR_MOVIMIENTO',N'Caja',@id,@motivo)", ("@id", id), ("@motivo", motivo), ("@usuario", usuario));
        comando.ExecuteNonQuery();
        // confirma todos los cambios de esta transaccion como una unidad
        transaccion.Commit();
    }

    // guarda el cierre despues de comprobar que los movimientos no cambiaron desde la consulta
    public CierreCaja Cerrar(CierreCaja solicitud, int usuario)
    {
        // prepara la conexion configurada y la libera al terminar este alcance
        using SqlConnection conexion = new SqlConnection(Conexion.ObtenerCadena());
        conexion.Open();
        using SqlTransaction transaccion = conexion.BeginTransaction();
        ExigirDiaAbierto(conexion, transaccion, solicitud.Fecha, true);
        List<MovimientoCaja> movimientos = LeerMovimientos(conexion, transaccion, solicitud.Fecha);
        if (JsonSerializer.Serialize(solicitud.Movimientos) != JsonSerializer.Serialize(movimientos))
        {
            // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
            throw new InvalidOperationException("Se registraron movimientos nuevos. Actualice la caja y revise los totales antes de cerrar.");
        }

        // conserva los movimientos comprobados en la base antes de guardar el cierre
        solicitud.Movimientos = movimientos;
        // registra la hora real del servidor al momento de cerrar
        solicitud.Fin = Ahora(conexion, transaccion);
        using SqlCommand operador = Comando(conexion, transaccion, "SELECT nombre_usuario FROM Usuario WHERE id_usuario=@usuario", ("@usuario", usuario));
        solicitud.Usuario = Convert.ToString(operador.ExecuteScalar()) ?? usuario.ToString();
        using SqlCommand comando = Comando(conexion, transaccion, @"INSERT Caja_Cierre(fecha,id_usuario,datos) OUTPUT INSERTED.id VALUES(@fecha,@usuario,@datos)", ("@fecha", solicitud.Fecha.Date), ("@usuario", usuario), ("@datos", JsonSerializer.Serialize(solicitud)));
        solicitud.Numero = Convert.ToInt32(comando.ExecuteScalar());
        using SqlCommand limpiar = Comando(conexion, transaccion, "DELETE Caja_Abierta WHERE fecha=@fecha", ("@fecha", solicitud.Fecha.Date));
        limpiar.ExecuteNonQuery();
        // confirma todos los cambios de esta transaccion como una unidad
        transaccion.Commit();
        return solicitud;
    }
}
