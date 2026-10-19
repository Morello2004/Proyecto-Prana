using Microsoft.Data.SqlClient;
using Prana.Entidades;
using System.Data;
using System.Text.Json;
using static Prana.Datos.GestionVentaSql;

namespace Prana.Datos;

public class CD_Caja
{
    internal static DateTime Ahora(SqlConnection cn, SqlTransaction tx)
    {
        using var cmd = Comando(cn, tx, "SELECT SYSDATETIME()");
        return (DateTime)cmd.ExecuteScalar()!;
    }

    // Cobros, egresos y cierre usan el mismo bloqueo: no quedan movimientos fuera del cierre.
    internal static void ExigirDiaAbierto(SqlConnection cn, SqlTransaction tx, DateTime fecha)
    {
        using var cmd = Comando(cn, tx, @"
            DECLARE @r int;
            EXEC @r=sys.sp_getapplock @Resource=@clave,@LockMode='Exclusive',@LockOwner='Transaction',@LockTimeout=15000;
            IF @r<0 THROW 50003,N'La caja está ocupada. Reintente.',1;
            IF EXISTS(SELECT 1 FROM Caja_Cierre WHERE fecha=@fecha)
                THROW 50004,N'La caja de ese día ya está cerrada. No se pueden agregar cobros ni gastos.',1;",
            ("@clave", "Prana.Caja." + fecha.ToString("yyyyMMdd")), ("@fecha", fecha.Date));
        cmd.ExecuteNonQuery();
    }

    public CierreCaja Consultar(DateTime fecha)
    {
        using var cn = new SqlConnection(Conexion.ObtenerCadena()); cn.Open();
        using var tx = cn.BeginTransaction(IsolationLevel.Serializable);
        using var cmd = Comando(cn, tx, "SELECT id,datos FROM Caja_Cierre WHERE fecha=@fecha", ("@fecha", fecha.Date));
        CierreCaja? resultado = null;
        using (var r = cmd.ExecuteReader())
            if (r.Read()) { resultado = JsonSerializer.Deserialize<CierreCaja>(r.GetString(1))!; resultado.Numero = r.GetInt32(0); }
        resultado ??= new CierreCaja { Fecha = fecha.Date, Inicio = fecha.Date.AddHours(9), Movimientos = LeerMovimientos(cn, tx, fecha) };
        tx.Commit();
        return resultado;
    }

    private static List<MovimientoCaja> LeerMovimientos(SqlConnection cn, SqlTransaction tx, DateTime fecha)
        => LeerMovimientos(cn, tx, fecha.Date, fecha.Date.AddDays(1));

    public List<MovimientoCaja> ConsultarPeriodo(DateTime desde, DateTime hasta)
    {
        using var cn = new SqlConnection(Conexion.ObtenerCadena()); cn.Open();
        using var tx = cn.BeginTransaction(IsolationLevel.Serializable);
        var resultado = LeerMovimientos(cn, tx, desde, hasta);
        tx.Commit();
        return resultado;
    }

    private static List<MovimientoCaja> LeerMovimientos(SqlConnection cn, SqlTransaction tx, DateTime desde, DateTime hasta)
    {
        using var cmd = Comando(cn, tx, @"
            SELECT p.id_pago id,p.id_venta venta,p.fecha_pago fecha,
                CONCAT(c.nombre_cliente,' ',c.apellido_cliente,' - ',m.nombre_medio_pago) concepto,
                N'Cobro venta' tipo,COALESCE(p.tipo_caja,m.tipo_caja) medio,'ARS' moneda,p.monto_final importe,
                p.cuenta_destino cuenta,CAST(p.id_usuario AS nvarchar(20)) usuario
            FROM Pago p JOIN Venta v ON v.id_venta=p.id_venta JOIN Cliente c ON c.id_cliente=v.id_cliente
            JOIN Medio_Pago m ON m.id_medio_pago=p.id_medio_pago
            WHERE p.fecha_pago>=@desde AND p.fecha_pago<@hasta
            UNION ALL
            SELECT id,NULL,fecha,concepto,tipo,medio,moneda,importe,cuenta,CAST(id_usuario AS nvarchar(20))
            FROM Caja_Movimiento WHERE fecha>=@desde AND fecha<@hasta AND anulado=0 ORDER BY fecha,id",
            ("@desde", desde), ("@hasta", hasta));
        using var r = cmd.ExecuteReader();
        var movimientos = new List<MovimientoCaja>();
        while (r.Read()) movimientos.Add(new MovimientoCaja { Id=r.GetInt32(0), Venta=r.IsDBNull(1)?null:r.GetInt32(1),
            Fecha=r.GetDateTime(2), Concepto=r.GetString(3), Tipo=r.GetString(4), Medio=r.GetString(5), Moneda=r.GetString(6),
            Importe=r.GetDecimal(7), Cuenta=r.GetString(8), Usuario=r.GetString(9) });
        return movimientos;
    }

    public void Registrar(MovimientoCaja movimiento, int usuario)
    {
        using var cn = new SqlConnection(Conexion.ObtenerCadena()); cn.Open();
        using var tx = cn.BeginTransaction();
        DateTime ahora = Ahora(cn, tx);
        ExigirDiaAbierto(cn, tx, ahora);
        using var cmd = Comando(cn, tx, @"INSERT Caja_Movimiento(fecha,concepto,tipo,medio,moneda,importe,cuenta,id_usuario)
            VALUES(@fecha,@concepto,@tipo,@medio,@moneda,@importe,@cuenta,@usuario)",
            ("@fecha",ahora),("@concepto",movimiento.Concepto),("@tipo",movimiento.Tipo),("@medio",movimiento.Medio),
            ("@moneda",movimiento.Moneda),("@importe",movimiento.Importe),("@cuenta",movimiento.Cuenta),("@usuario",usuario));
        cmd.ExecuteNonQuery();
        tx.Commit();
    }

    public void Anular(int id, string motivo, int usuario)
    {
        using var cn = new SqlConnection(Conexion.ObtenerCadena()); cn.Open(); using var tx = cn.BeginTransaction();
        using var fechaCmd = Comando(cn, tx, "SELECT fecha FROM Caja_Movimiento WHERE id=@id", ("@id",id));
        if (fechaCmd.ExecuteScalar() is not DateTime fecha) throw new InvalidOperationException("El movimiento no existe.");
        ExigirDiaAbierto(cn, tx, fecha);
        using var cmd = Comando(cn, tx, @"UPDATE Caja_Movimiento SET anulado=1,motivo_anulacion=@motivo
            WHERE id=@id AND anulado=0;
            IF @@ROWCOUNT<>1 THROW 50005,N'El movimiento ya fue anulado.',1;
            INSERT LogActividad(id_usuario,accion,entidad,id_entidad,detalle)
            VALUES(@usuario,N'ANULAR_MOVIMIENTO',N'Caja',@id,@motivo)", ("@id",id),("@motivo",motivo),("@usuario",usuario));
        cmd.ExecuteNonQuery(); tx.Commit();
    }

    public CierreCaja Cerrar(CierreCaja solicitud, int usuario)
    {
        using var cn = new SqlConnection(Conexion.ObtenerCadena()); cn.Open(); using var tx = cn.BeginTransaction();
        ExigirDiaAbierto(cn, tx, solicitud.Fecha);
        var movimientos = LeerMovimientos(cn, tx, solicitud.Fecha);
        if (JsonSerializer.Serialize(solicitud.Movimientos) != JsonSerializer.Serialize(movimientos))
            throw new InvalidOperationException("Se registraron movimientos nuevos. Actualice la caja y revise los totales antes de cerrar.");
        solicitud.Movimientos = movimientos;
        solicitud.Fin = Ahora(cn, tx);
        using var operador = Comando(cn, tx, "SELECT nombre_usuario FROM Usuario WHERE id_usuario=@usuario", ("@usuario",usuario));
        solicitud.Usuario = Convert.ToString(operador.ExecuteScalar()) ?? usuario.ToString();
        using var cmd = Comando(cn, tx, @"INSERT Caja_Cierre(fecha,id_usuario,datos) OUTPUT INSERTED.id VALUES(@fecha,@usuario,@datos)",
            ("@fecha",solicitud.Fecha.Date),("@usuario",usuario),("@datos",JsonSerializer.Serialize(solicitud)));
        solicitud.Numero = Convert.ToInt32(cmd.ExecuteScalar());
        tx.Commit();
        return solicitud;
    }
}
