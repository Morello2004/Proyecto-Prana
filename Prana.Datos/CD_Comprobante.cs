using Microsoft.Data.SqlClient;
using Prana.Entidades;
using static Prana.Datos.GestionVentaSql;

namespace Prana.Datos;

public class CD_Comprobante
{
    public ComprobanteVenta Obtener(int venta)
    {
        using var cn = new SqlConnection(Conexion.ObtenerCadena()); cn.Open();
        using var tx = cn.BeginTransaction();
        var documento = LeerVenta(cn, tx, venta);
        tx.Commit();
        return documento;
    }

    private static ComprobanteVenta LeerVenta(SqlConnection cn, SqlTransaction tx, int venta)
    {
        using var cmd = Comando(cn, tx, @"SELECT v.fecha_venta,CONCAT(c.nombre_cliente,' ',c.apellido_cliente),c.dni,
            COALESCE(c.telefono,''),COALESCE(c.direccion,''),COALESCE(l.nombre_localidad,''),
            COALESCE(ve.domicilio,''),m.nombre_modalidad_entrega,COALESCE(ve.observaciones,''),
            COALESCE(v.descuento,0),COALESCE(ve.costo_envio,0),v.total,
            COALESCE((SELECT SUM(monto_final) FROM Pago WHERE id_venta=v.id_venta),0),
            COALESCE((SELECT SUM(monto_final-monto) FROM Pago WHERE id_venta=v.id_venta),0)
            FROM Venta v JOIN Cliente c ON c.id_cliente=v.id_cliente LEFT JOIN Localidad l ON l.id_localidad=c.id_localidad
            JOIN Modalidad_Entrega m ON m.id_modalidad_entrega=v.id_modalidad_entrega
            LEFT JOIN Venta_Entrega ve ON ve.id_venta=v.id_venta WHERE v.id_venta=@venta", ("@venta",venta));
        ComprobanteVenta doc;
        using (var r=cmd.ExecuteReader())
        {
            if(!r.Read()) throw new InvalidOperationException("No se encontró la venta.");
            doc = new ComprobanteVenta { Numero=venta, Fecha=r.GetDateTime(0),Cliente=r.GetString(1),Dni=r.GetString(2),
                Telefono=r.GetString(3),Direccion=r.GetString(4),Localidad=r.GetString(5),Destino=r.GetString(6),Modalidad=r.GetString(7),
                Observaciones=r.GetString(8),Descuento=r.GetDecimal(9),Envio=r.GetDecimal(10),Total=r.GetDecimal(11),Pagado=r.GetDecimal(12),Recargos=r.GetDecimal(13) };
        }
        cmd.CommandText = @"SELECT d.id_variante,CONCAT(p.nombre_producto,' ',p.modelo),CONCAT(vp.color,' / ',vp.condicion_producto),d.cantidad,d.precio_unitario
            FROM Detalle_Venta d JOIN Variante_Producto vp ON vp.id_variante=d.id_variante JOIN Producto p ON p.id_producto=vp.id_producto
            WHERE d.id_venta=@venta ORDER BY d.id_detalle_venta";
        using (var r=cmd.ExecuteReader()) while(r.Read()) doc.Productos.Add(new DetalleNuevaVenta {
            IdVariante=r.GetInt32(0),Producto=r.GetString(1),Variante=r.GetString(2),Cantidad=r.GetInt32(3),PrecioUnitario=r.GetDecimal(4) });
        CompletarEntrega(cn,tx,doc);
        return doc;
    }

    private static void CompletarEntrega(SqlConnection cn, SqlTransaction tx, ComprobanteVenta doc)
    {
        using var cmd=Comando(cn,tx,@"SELECT ve.fecha_estimada,COALESCE(ve.horario_sugerido,''),
            COALESCE(ve.tipo_logistica,''),COALESCE(ve.observacion_pago,''),COALESCE(ve.condicion_pago,''),
            COALESCE(c.codigo_postal,'')
            FROM Venta v JOIN Cliente c ON c.id_cliente=v.id_cliente
            LEFT JOIN Localidad l ON l.id_localidad=c.id_localidad
            LEFT JOIN Venta_Entrega ve ON ve.id_venta=v.id_venta WHERE v.id_venta=@venta",("@venta",doc.Numero));
        using var r=cmd.ExecuteReader();
        if(r.Read())
        {
            doc.FechaEntrega=r.IsDBNull(0)?null:r.GetDateTime(0);
            doc.HorarioSugerido=r.GetString(1); doc.TipoLogistica=r.GetString(2);
            doc.ObservacionPago=r.GetString(3); doc.CondicionPago=r.GetString(4);
            doc.CodigoPostal=r.GetString(5); doc.DatosEntregaCompletos=true;
        }
    }
}
