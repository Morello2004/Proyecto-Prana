// guia del archivo: reconstruye el comprobante desde la venta, su cliente, articulos y cobros registrados el nombre impreso agrega el modelo al nombre del producto; el selector de nueva venta usa otra representacion pagado suma monto_final, incluido el recargo pagos conserva el desglose por fecha, medio e importe recibido consultar un comprobante no registra una venta ni un pago adicional
using Microsoft.Data.SqlClient;
using Prana.Entidades;
using static Prana.Datos.GestionVentaSql;

namespace Prana.Datos;
// agrupa los datos y operaciones de cd comprobante
public class CD_Comprobante
{
    // coordina obtener dentro de una transaccion para confirmar o revertir sus escrituras juntas
    public venta Obtener(int idVenta)
    {
        // prepara la conexion configurada y la libera al terminar este alcance
        using SqlConnection conexion = new SqlConnection(Conexion.ObtenerCadena());
        conexion.Open();
        using SqlTransaction transaccion = conexion.BeginTransaction();
        venta documento = LeerVenta(conexion, transaccion, idVenta);
        // confirma todos los cambios de esta transaccion como una unidad
        transaccion.Commit();
        return documento;
    }

    // consulta leer venta y transforma cada fila recibida en los objetos que devuelve
    private static venta LeerVenta(SqlConnection cn, SqlTransaction tx, int idVenta)
    {
        using SqlCommand comando = Comando(cn, tx, @"SELECT v.fecha_venta,c.nombre_cliente,c.dni,
            COALESCE(c.telefono,''),COALESCE(c.direccion,''),COALESCE(l.nombre_localidad,''),
            COALESCE(ve.domicilio,''),m.nombre_modalidad_entrega,COALESCE(ve.observaciones,''),
            COALESCE(v.descuento,0),COALESCE(ve.costo_envio,0),v.total,
            COALESCE((SELECT SUM(monto_final) FROM Pago WHERE id_venta=v.id_venta),0),
            c.apellido_cliente, c.tipo_documento,v.id_cliente,v.id_usuario,COALESCE(c.id_localidad,0)
            FROM Venta v JOIN Cliente c ON c.id_cliente=v.id_cliente LEFT JOIN Localidad l ON l.id_localidad=c.id_localidad
            JOIN Modalidad_Entrega m ON m.id_modalidad_entrega=v.id_modalidad_entrega
            LEFT JOIN Venta_Entrega ve ON ve.id_venta=v.id_venta WHERE v.id_venta=@venta", ("@venta", idVenta));
        venta documento;
        // limita la vida del recurso al bloque; al salir se ejecuta su liberacion incluso ante una excepcion
        using (SqlDataReader lector = comando.ExecuteReader())
        {
            if (!lector.Read())
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new InvalidOperationException("No se encontró la venta.");
            }

            documento = new venta
            {
                IdVenta = idVenta,
                IdCliente = lector.GetInt32(15),
                IdUsuario = lector.GetInt32(16),
                Fecha = lector.GetDateTime(0),
                // los datos personales se conservan en el cliente de la venta sin duplicarlos como textos sueltos
                Cliente = new Cliente
                {
                    IdCliente = lector.GetInt32(15),
                    Nombre = lector.GetString(1),
                    Apellido = lector.GetString(13),
                    DNI = lector.GetString(2),
                    TipoDocumento = lector.GetString(14),
                    Telefono = lector.GetString(3),
                    Direccion = lector.GetString(4),
                    NombreLocalidad = lector.GetString(5),
                    IdLocalidad = lector.GetInt32(17)
                },
                DomicilioEntrega = lector.GetString(6),
                ModalidadEntrega = lector.GetString(7),
                ObservacionesEntrega = lector.GetString(8),
                Descuento = lector.GetDecimal(9),
                CostoEnvio = lector.GetDecimal(10),
                Total = lector.GetDecimal(11),
                Pagado = lector.GetDecimal(12),
                SaldoPendiente = Math.Max(0, lector.GetDecimal(11) - lector.GetDecimal(12))

            };
        }

        comando.CommandText = @"SELECT d.id_variante,CONCAT(p.nombre_producto,' ',p.modelo),CONCAT(vp.color,' / ',vp.condicion_producto),d.cantidad,d.precio_unitario
            FROM Detalle_Venta d JOIN Variante_Producto vp ON vp.id_variante=d.id_variante JOIN Producto p ON p.id_producto=vp.id_producto
            WHERE d.id_venta=@venta ORDER BY d.id_detalle_venta";
        // limita la vida del recurso al bloque; al salir se ejecuta su liberacion incluso ante una excepcion
        using (SqlDataReader r = comando.ExecuteReader())
        {
            while (r.Read())
            {
                documento.Detalles.Add(new DetalleNuevaVenta
                {
                    IdVariante = r.GetInt32(0),
                    Producto = r.GetString(1),
                    Variante = r.GetString(2),
                    Cantidad = r.GetInt32(3),
                    PrecioUnitario = r.GetDecimal(4)

                });
            }
        }

        comando.CommandText = @"SELECT p.fecha_pago,m.nombre_medio_pago,COALESCE(p.cuotas,1),p.monto,p.monto_final
            FROM Pago p JOIN Medio_Pago m ON m.id_medio_pago=p.id_medio_pago
            WHERE p.id_venta=@venta ORDER BY p.fecha_pago,p.id_pago";
        // limita la vida del recurso al bloque; al salir se ejecuta su liberacion incluso ante una excepcion
        using (SqlDataReader r = comando.ExecuteReader())
        {
            while (r.Read())
            {
                documento.Pagos.Add(new Pago
                {
                    Fecha = r.GetDateTime(0),
                    Medio = r.GetString(1),
                    Cuotas = r.GetInt32(2),
                    ImporteBase = r.GetDecimal(3),
                    Cobrado = r.GetDecimal(4),
                    Recargo = r.GetDecimal(4) - r.GetDecimal(3)

                });
            }
        }

        CompletarEntrega(cn, tx, documento);
        return documento;
    }

    // consulta completar entrega y transforma cada fila recibida en los objetos que devuelve
    private static void CompletarEntrega(SqlConnection cn, SqlTransaction tx, venta doc)
    {
        using SqlCommand comando = Comando(cn, tx, @"SELECT ve.fecha_estimada,COALESCE(ve.horario_sugerido,''),
            COALESCE(ve.tipo_logistica,''),COALESCE(ve.observacion_pago,''),COALESCE(ve.condicion_pago,''),
            COALESCE(c.codigo_postal,'')
            FROM Venta v JOIN Cliente c ON c.id_cliente=v.id_cliente
            LEFT JOIN Localidad l ON l.id_localidad=c.id_localidad
            LEFT JOIN Venta_Entrega ve ON ve.id_venta=v.id_venta WHERE v.id_venta=@venta", ("@venta", doc.IdVenta));
        using SqlDataReader lector = comando.ExecuteReader();
        if (lector.Read())
        {
            if (lector.IsDBNull(0))
            {
                doc.FechaEntrega = null;
            }
            else
            {
                doc.FechaEntrega = lector.GetDateTime(0);
            }
            doc.HorarioSugerido = lector.GetString(1);
            doc.TipoLogistica = lector.GetString(2);
            doc.ObservacionPago = lector.GetString(3);
            doc.CondicionPago = lector.GetString(4);
            doc.Cliente.CodigoPostal = lector.GetString(5);
        }
    }
}
