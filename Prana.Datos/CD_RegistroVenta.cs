using Microsoft.Data.SqlClient;
using Prana.Entidades;
using System;
using System.Data;

namespace Prana.Datos
{
    // guarda la venta y sus datos relacionados en una sola transacción
    public static class CD_RegistroVenta
    {
        // registra encabezado detalles pagos y entrega juntos
        public static int Guardar(NuevaVenta venta,bool retiroInmediato)
        {
            using SqlConnection conexion = new(Conexion.ObtenerCadena());
            conexion.Open();
            using SqlTransaction transaccion = conexion.BeginTransaction();
            try
            {
                DateTime ahora = CD_Caja.Ahora(conexion, transaccion);
                if (venta.Pagos.Count > 0) CD_Caja.ExigirDiaAbierto(conexion, transaccion, ahora);
                // asegura los valores de catálogo necesarios para esta venta
                int modalidad = Catalogo(conexion, transaccion, "Modalidad_Entrega", "id_modalidad_entrega", "nombre_modalidad_entrega", venta.ModalidadEntrega);
                int estadoVenta = Catalogo(conexion, transaccion, "Estado_Venta", "id_estado_venta", "nombre_estado_venta", venta.Pagos.Count == 0 ? "Pendiente" : venta.CondicionPago == "Pago total" ? "Pagada" : "Parcial");
                int estadoDetalle = Catalogo(conexion, transaccion, "Estado_Variante_Producto", "id_estado_variante_producto", "nombre_estado_variante_producto", retiroInmediato ? "Entregado" : "Pendiente");
                // guarda el encabezado y obtiene el número de venta
                using SqlCommand encabezado = new("INSERT INTO Venta (id_cliente,id_usuario,fecha_venta,id_estado_venta,total,id_modalidad_entrega,descuento) OUTPUT INSERTED.id_venta VALUES (@cliente,@usuario,SYSDATETIME(),@estado,@total,@modalidad,@descuento)", conexion, transaccion);
                encabezado.Parameters.AddWithValue("@cliente", venta.IdCliente);
                encabezado.Parameters.AddWithValue("@usuario", venta.IdUsuario);
                encabezado.Parameters.AddWithValue("@estado", estadoVenta);
                encabezado.Parameters.AddWithValue("@total", venta.Total);
                encabezado.Parameters.AddWithValue("@modalidad", modalidad);
                encabezado.Parameters.AddWithValue("@descuento", venta.Descuento);
                int idVenta = Convert.ToInt32(encabezado.ExecuteScalar());

                // guarda cada variante con su cantidad y precio
                foreach (DetalleNuevaVenta item in venta.Detalles)
                {
                    using SqlCommand detalle = new("INSERT INTO Detalle_Venta (id_venta,id_variante,cantidad,precio_unitario,subtotal,id_estado_variante_producto) VALUES (@venta,@variante,@cantidad,@precio,@subtotal,@estado)", conexion, transaccion);
                    detalle.Parameters.AddWithValue("@venta", idVenta);
                    detalle.Parameters.AddWithValue("@variante", item.IdVariante);
                    detalle.Parameters.AddWithValue("@cantidad", item.Cantidad);
                    detalle.Parameters.AddWithValue("@precio", item.PrecioUnitario);
                    detalle.Parameters.AddWithValue("@subtotal", item.Cantidad * item.PrecioUnitario);
                    detalle.Parameters.AddWithValue("@estado", estadoDetalle);
                    detalle.ExecuteNonQuery();
                }

                // conserva por separado la base y el importe final cobrado
                foreach (PagoNuevaVenta item in venta.Pagos)
                {
                    using SqlCommand pago = new("INSERT INTO Pago (id_venta,id_usuario,id_medio_pago,cuotas,monto,monto_final,fecha_pago,tipo_caja,cuenta_destino) SELECT @venta,@usuario,@medio,@cuotas,@monto,@final,@fecha,tipo_caja,@cuenta FROM Medio_Pago WHERE id_medio_pago=@medio", conexion, transaccion);
                    pago.Parameters.AddWithValue("@cuenta", item.CuentaDestino);
                    pago.Parameters.AddWithValue("@venta", idVenta);
                    pago.Parameters.AddWithValue("@usuario", venta.IdUsuario);
                    pago.Parameters.AddWithValue("@medio", item.IdMedioPago);
                    pago.Parameters.AddWithValue("@cuotas", item.Cuotas);
                    pago.Parameters.AddWithValue("@monto", item.Monto);
                    pago.Parameters.AddWithValue("@final", item.Monto + item.Recargo);
                    pago.Parameters.AddWithValue("@fecha", ahora);
                    pago.ExecuteNonQuery();
                }

                // guarda los datos necesarios para preparar el remito más adelante
                using (SqlCommand entrega = new(@"
IF OBJECT_ID(N'dbo.Venta_Entrega', N'U') IS NULL
    CREATE TABLE dbo.Venta_Entrega (
        id_venta int NOT NULL PRIMARY KEY REFERENCES dbo.Venta(id_venta),
        domicilio nvarchar(200) NULL, fecha_estimada datetime2 NULL,
        horario_sugerido nvarchar(50) NULL, observaciones nvarchar(500) NULL,
        tipo_logistica nvarchar(50) NULL, costo_envio decimal(10,2) NOT NULL,
        observacion_pago nvarchar(500) NULL, condicion_pago nvarchar(30) NOT NULL);
INSERT INTO dbo.Venta_Entrega (id_venta,domicilio,fecha_estimada,horario_sugerido,observaciones,tipo_logistica,costo_envio,observacion_pago,condicion_pago)
VALUES (@venta,@domicilio,@fecha,@horario,@observaciones,@logistica,@costo,@pago,@condicion)", conexion, transaccion))
                {
                    entrega.Parameters.AddWithValue("@venta", idVenta);
                    entrega.Parameters.AddWithValue("@domicilio", (object)venta.DomicilioEntrega ?? DBNull.Value);
                    entrega.Parameters.AddWithValue("@fecha", venta.FechaEntrega);
                    entrega.Parameters.AddWithValue("@horario", venta.HorarioSugerido);
                    entrega.Parameters.AddWithValue("@observaciones", venta.ObservacionesEntrega);
                    entrega.Parameters.AddWithValue("@logistica", venta.TipoLogistica);
                    entrega.Parameters.AddWithValue("@costo", venta.CostoEnvio);
                    entrega.Parameters.AddWithValue("@pago", venta.ObservacionPago);
                    entrega.Parameters.AddWithValue("@condicion", venta.CondicionPago);
                    entrega.ExecuteNonQuery();
                }

                if (retiroInmediato)
                {
                    CD_Entrega.RegistrarRetiroInmediato(
                        conexion,
                        transaccion,
                        idVenta,
                        venta);
                }

                using (SqlCommand historial = new(@"
INSERT LogActividad(id_usuario,accion,entidad,id_entidad,detalle,fecha)
SELECT v.id_usuario,N'REGISTRAR_VENTA',N'Venta',v.id_venta,
    CONCAT(N'Venta / RTO ',v.id_venta,N' | Cliente: ',c.nombre_cliente,N' ',c.apellido_cliente,
        N' | Total ARS: ',FORMAT(v.total,'N2','es-AR'),N' | Descuento ARS: ',FORMAT(v.descuento,'N2','es-AR')),v.fecha_venta
FROM Venta v JOIN Cliente c ON c.id_cliente=v.id_cliente WHERE v.id_venta=@venta;
INSERT LogActividad(id_usuario,accion,entidad,id_entidad,detalle,fecha)
SELECT id_usuario,N'APLICAR_DESCUENTO',N'Venta',id_venta,
    CONCAT(N'Venta / RTO ',id_venta,N' | Descuento ARS: ',FORMAT(descuento,'N2','es-AR'),
        N' | Total final ARS: ',FORMAT(total,'N2','es-AR')),fecha_venta
FROM Venta WHERE id_venta=@venta AND descuento>0;", conexion, transaccion))
                {
                    historial.Parameters.AddWithValue("@venta", idVenta);
                    historial.ExecuteNonQuery();
                }
                transaccion.Commit();
                return idVenta;
            }
            catch
            {
                transaccion.Rollback();
                throw;
            }
        }

        // busca un valor de catálogo y lo crea si todavía no existe
        private static int Catalogo(SqlConnection conexion, SqlTransaction transaccion, string tabla, string id, string nombre, string buscado)
        {
            // los nombres de tabla y columna son constantes internas de datos
            using SqlCommand comando = new($"SELECT TOP (1) {id} FROM {tabla} WITH (UPDLOCK, HOLDLOCK) WHERE {nombre} = @nombre", conexion, transaccion);
            comando.Parameters.Add("@nombre", SqlDbType.NVarChar, 100).Value = buscado;
            object? resultado = comando.ExecuteScalar();
            if (resultado != null) return Convert.ToInt32(resultado);
            using SqlCommand insertar = new($"INSERT INTO {tabla} ({nombre}) OUTPUT INSERTED.{id} VALUES (@nombre)", conexion, transaccion);
            insertar.Parameters.Add("@nombre", SqlDbType.NVarChar, 100).Value = buscado;
            return Convert.ToInt32(insertar.ExecuteScalar());
        }
    }
}
