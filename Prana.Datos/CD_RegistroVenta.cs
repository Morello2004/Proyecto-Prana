using Microsoft.Data.SqlClient;
using Prana.Entidades;
using System;
using System.Data;

namespace Prana.Datos
{
    // guarda la venta y sus datos relacionados en una sola transaccion
    public static class CD_RegistroVenta
    {
        // registra encabezado detalles pagos y entrega juntos
        public static int Guardar(venta venta, bool retiroInmediato)
        {
            // using libera la conexion al salir del metodo incluso si ocurre un error
            using SqlConnection conexion = new(Conexion.ObtenerCadena());
            conexion.Open();
            // agrupa las escrituras para confirmar todo junto o deshacer todo si algo falla
            using SqlTransaction transaccion = conexion.BeginTransaction();
            bool insercionExplicitaActivada = false;
            // protege el acceso a los datos para atender los errores de la operacion
            try
            {
                // consulta la hora de sql server para usarla en los cobros
                DateTime ahora = CD_Caja.Ahora(conexion, transaccion);
                // obtiene la apertura vigente para relacionar todos los cobros de esta venta con el mismo turno
                int idCaja = CD_Caja.ExigirCajaAbierta(conexion, transaccion, ahora);
                // obtiene los identificadores de modalidad de entrega y estados necesarios para esta venta
                int modalidad = ObtenerOCrearIdPorNombre(conexion, transaccion, "Modalidad_Entrega", "id_modalidad_entrega", "nombre_modalidad_entrega", venta.ModalidadEntrega);
                string nombreEstadoVenta;
                // comprueba si hay elementos para continuar con esta operacion
                if (venta.Pagos.Count == 0)
                {
                    nombreEstadoVenta = "Pendiente";
                }
                else
                {
                    if (venta.CondicionPago == "Pago total")
                    {
                        nombreEstadoVenta = "Pagada";
                    }
                    else
                    {
                        nombreEstadoVenta = "Parcial";
                    }
                }
                // elige pendiente sin pagos pagada con pago total o parcial en los demas casos
                int estadoVenta = ObtenerOCrearIdPorNombre(conexion, transaccion, "Estado_Venta", "id_estado_venta", "nombre_estado_venta", nombreEstadoVenta);
                string nombreEstadoDetalle;
                if (retiroInmediato)
                {
                    nombreEstadoDetalle = "Entregado";
                }
                else
                {
                    nombreEstadoDetalle = "Pendiente";
                }
                // marca los productos entregados solo cuando el cliente los retira en este momento
                int estadoDetalle = ObtenerOCrearIdPorNombre(conexion, transaccion, "Estado_Variante_Producto", "id_estado_variante_producto", "nombre_estado_variante_producto", nombreEstadoDetalle);
                // bloquea la tabla hasta confirmar o revertir para que dos ventas no reciban el mismo numero
                using SqlCommand siguienteNumero = new("SELECT COALESCE(MAX(id_venta), 2499) + 1 FROM dbo.Venta WITH (TABLOCKX, HOLDLOCK)", conexion, transaccion);
                int idVenta = Convert.ToInt32(siguienteNumero.ExecuteScalar());
                // si falla el insert la limpieza final sabe que debe intentar restaurar esta opcion de sesion
                // permite guardar el numero calculado sin depender del contador identity de sql server
                insercionExplicitaActivada = true;
                using SqlCommand activarInsercion = new("SET IDENTITY_INSERT dbo.Venta ON", conexion, transaccion);
                activarInsercion.ExecuteNonQuery();
                using SqlCommand encabezado = new("INSERT INTO dbo.Venta (id_venta,id_cliente,id_usuario,fecha_venta,id_estado_venta,total,id_modalidad_entrega,descuento) VALUES (@venta,@cliente,@usuario,SYSDATETIME(),@estado,@total,@modalidad,@descuento)", conexion, transaccion);
                // asigna valores a los parametros sin concatenarlos dentro de la consulta
                encabezado.Parameters.AddWithValue("@cliente", venta.IdCliente);
                encabezado.Parameters.AddWithValue("@venta", idVenta);
                encabezado.Parameters.AddWithValue("@usuario", venta.IdUsuario);
                encabezado.Parameters.AddWithValue("@estado", estadoVenta);
                encabezado.Parameters.AddWithValue("@total", venta.Total);
                encabezado.Parameters.AddWithValue("@modalidad", modalidad);
                encabezado.Parameters.AddWithValue("@descuento", venta.Descuento);
                encabezado.ExecuteNonQuery();
                // desactiva la insercion explicita antes de guardar detalles pagos y entrega
                using SqlCommand desactivarInsercion = new("SET IDENTITY_INSERT dbo.Venta OFF", conexion, transaccion);
                desactivarInsercion.ExecuteNonQuery();
                insercionExplicitaActivada = false;
                // relaciona cada variante con la venta y conserva la cantidad y el precio aplicado en esta operacion
                foreach (DetalleNuevaVenta item in venta.Detalles)
                {
                    using SqlCommand detalle = new("INSERT INTO Detalle_Venta (id_venta,id_variante,cantidad,precio_unitario,subtotal,id_estado_variante_producto) VALUES (@venta,@variante,@cantidad,@precio,@subtotal,@estado)", conexion, transaccion);
                    // relaciona este producto con el identificador de la venta recien creada
                    detalle.Parameters.AddWithValue("@venta", idVenta);
                    detalle.Parameters.AddWithValue("@variante", item.IdVariante);
                    detalle.Parameters.AddWithValue("@cantidad", item.Cantidad);
                    detalle.Parameters.AddWithValue("@precio", item.PrecioUnitario);
                    detalle.Parameters.AddWithValue("@subtotal", item.Cantidad * item.PrecioUnitario);
                    detalle.Parameters.AddWithValue("@estado", estadoDetalle);
                    // ejecuta la escritura sin solicitar un valor de resultado
                    detalle.ExecuteNonQuery();
                }

                // conserva por separado la base y el importe final cobrado
                foreach (Pago item in venta.Pagos)
                {
                    // el select toma el tipo de caja del medio de pago y lo incluye en el registro
                    // todos los medios del pago inicial quedan asociados a la misma apertura protegida por la transaccion
                    using SqlCommand pago = new("INSERT INTO Pago (id_venta,id_usuario,id_medio_pago,cuotas,monto,monto_final,fecha_pago,tipo_caja,cuenta_destino,id_caja) SELECT @venta,@usuario,@medio,@cuotas,@monto,@final,@fecha,tipo_caja,@cuenta,@caja FROM Medio_Pago WHERE id_medio_pago=@medio", conexion, transaccion);
                    pago.Parameters.AddWithValue("@caja", idCaja);
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

                // guarda los datos necesarios para preparar el remito mas adelante
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
                    // dbnull representa un valor nulo de sql cuando no se recibe un domicilio
                    entrega.Parameters.AddWithValue("@domicilio", (object)venta.DomicilioEntrega ?? DBNull.Value);
                    entrega.Parameters.AddWithValue("@fecha", (object?)venta.FechaEntrega ?? DBNull.Value);
                    entrega.Parameters.AddWithValue("@horario", venta.HorarioSugerido);
                    entrega.Parameters.AddWithValue("@observaciones", venta.ObservacionesEntrega);
                    entrega.Parameters.AddWithValue("@logistica", venta.TipoLogistica);
                    entrega.Parameters.AddWithValue("@costo", venta.CostoEnvio);
                    entrega.Parameters.AddWithValue("@pago", venta.ObservacionPago);
                    entrega.Parameters.AddWithValue("@condicion", venta.CondicionPago);
                    entrega.ExecuteNonQuery();
                }

                // registra el remito y descuenta stock dentro de la misma transaccion
                if (retiroInmediato)
                {
                    CD_Entrega.RegistrarRetiroInmediato(conexion, transaccion, idVenta, venta);
                }

                // registra la venta y agrega otra actividad solo si hubo descuento
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

                // confirma todas las escrituras realizadas dentro de la transaccion
                transaccion.Commit();
                return idVenta;
            }
            // atiende el error de esta operacion antes de informar o propagar el problema
            catch
            {
                // deshace las escrituras de esta transaccion porque la operacion no pudo completarse
                transaccion.Rollback();
                // propaga el error original para que la capa que llamo al metodo pueda informarlo interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw;
            }
            // realiza la limpieza de salida tanto si la operacion termina bien como si falla
            finally
            {
                // restaura la configuracion de la conexion si fallo el encabezado antes de desactivarla
                if (insercionExplicitaActivada)
                {
                    // protege el acceso a los datos para atender los errores de la operacion
                    try
                    {
                        using SqlCommand desactivarInsercion = new("SET IDENTITY_INSERT dbo.Venta OFF", conexion);
                        desactivarInsercion.ExecuteNonQuery();
                    }
                    // atiende el error de esta operacion antes de informar o propagar el problema
                    catch
                    {
                        // descarta la conexion para no reutilizar una sesion con esta opcion activada
                        SqlConnection.ClearPool(conexion);
                    }
                }
            }
        }

        // busca el identificador por nombre y crea el registro si no existe
        private static int ObtenerOCrearIdPorNombre(SqlConnection conexion, SqlTransaction transaccion, string tabla, string id, string nombre, string buscado)
        {
            // los nombres de tabla y columna son constantes internas de datos
            using SqlCommand comando = new($"SELECT TOP (1) {id} FROM {tabla} WITH (UPDLOCK, HOLDLOCK) WHERE {nombre} = @nombre", conexion, transaccion);
            comando.Parameters.Add("@nombre", SqlDbType.NVarChar, 100).Value = buscado;
            // obtiene la primera columna de la primera fila o null si la busqueda no devuelve filas
            object? resultado = comando.ExecuteScalar();
            // reutiliza el identificador existente y evita insertar otro registro con ese nombre
            if (resultado != null)
            {
                return Convert.ToInt32(resultado);
            }

            // si el nombre no existe
            using SqlCommand insertar = new($"INSERT INTO {tabla} ({nombre}) OUTPUT INSERTED.{id} VALUES (@nombre)", conexion, transaccion);
            insertar.Parameters.Add("@nombre", SqlDbType.NVarChar, 100).Value = buscado;
            // ejecuta el alta y devuelve el identificador de la fila creada
            return Convert.ToInt32(insertar.ExecuteScalar());
        }
    }
}
