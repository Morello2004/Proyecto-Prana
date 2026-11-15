using Microsoft.Data.SqlClient;
using Prana.Entidades;
using System.Collections.Generic;
using System.Data;
using static Prana.Datos.GestionVentaSql;

namespace Prana.Datos
{
    // agrupa los datos y operaciones de cd entrega
    public class CD_Entrega
    {
        // un remito reserva unidades; una entrega confirmada cambia su estado, no vuelve a descontar stock
        private const string PreparadosSql = @"SELECT r.id_venta,dr.id_variante,SUM(dr.cantidad) cantidad
            FROM Remito r JOIN Detalle_Remito dr ON dr.id_remito=r.id_remito
            WHERE NOT EXISTS(SELECT 1 FROM Envio e JOIN Estado_Envio ee ON ee.id_estado_envio=e.id_estado_envio
                WHERE e.id_remito=r.id_remito AND ee.nombre LIKE N'Cancelad%') GROUP BY r.id_venta,dr.id_variante";
        // consulta las cantidades pendientes de preparar y el stock disponible de cada variante
        public List<AlertaStockPendiente> ObtenerNecesidadesStock()
        {
            // crea la conexion con la base de datos
            using SqlConnection conexion = new SqlConnection(Conexion.ObtenerCadena());
            // vendidos suma las unidades de cada variante dentro de cada venta preparados reutiliza la consulta existente de unidades incluidas en remitos vigentes necesidades resta las unidades preparadas y suma lo pendiente entre todas las ventas se excluyen las ventas anuladas o canceladas
            using SqlCommand comando = new SqlCommand(@"
        WITH Vendidos AS
        (
            SELECT id_venta, id_variante, SUM(cantidad) cantidad
            FROM Detalle_Venta
            GROUP BY id_venta, id_variante
        ),
        Preparados AS
        (
            " + PreparadosSql + @"
        ),
        Necesidades AS
        (
            SELECT d.id_variante, SUM(d.cantidad - COALESCE(r.cantidad, 0)) pendientes
            FROM Vendidos d
            JOIN Venta v ON v.id_venta = d.id_venta
            JOIN Estado_Venta ev ON ev.id_estado_venta = v.id_estado_venta
            LEFT JOIN Preparados r ON r.id_venta = d.id_venta AND r.id_variante = d.id_variante
            WHERE d.cantidad > COALESCE(r.cantidad, 0)
              AND ev.nombre_estado_venta NOT LIKE N'Anulad%'
              AND ev.nombre_estado_venta NOT LIKE N'Cancelad%'
            GROUP BY d.id_variante
        )
        SELECT vp.id_variante, CONCAT(p.nombre_producto, ' ', p.modelo, ' (', vp.color, ', ', vp.condicion_producto, ')') producto, n.pendientes, vp.stock_disponible disponible
        FROM Necesidades n
        JOIN Variante_Producto vp ON vp.id_variante = n.id_variante
        JOIN Producto p ON p.id_producto = vp.id_producto
        ORDER BY p.nombre_producto, vp.id_variante", conexion);
            // abre la conexion y ejecuta la consulta abre el recurso antes de usarlo; una conexion debe estar abierta para enviar consultas
            conexion.Open();
            using SqlDataReader lector = comando.ExecuteReader();
            List<AlertaStockPendiente> resultado = new List<AlertaStockPendiente>();
            // recorre cada variante obtenida y carga sus cantidades avanza el lector de sql una fila por vez hasta agotar el resultado
            while (lector.Read())
            {
                resultado.Add(new AlertaStockPendiente
                {
                    IdVariante = lector.GetInt32(0),
                    Producto = lector.GetString(1),
                    Pendientes = lector.GetInt32(2),
                    Disponible = lector.GetInt32(3) // unidades disponibles para preparar

                });
            }

            // devuelve los datos; la capa de negocio calcula y filtra los faltantes
            return resultado;
        }

        // consulta pendientes y transforma cada fila recibida en los objetos que devuelve
        public List<ProductoPorEntregar> Pendientes(string texto)
        {
            // prepara la conexion configurada y la libera al terminar este alcance
            using SqlConnection conexion = new SqlConnection(Conexion.ObtenerCadena());
            conexion.Open();
            // prepara la consulta y su conexion los valores variables se envian mediante parametros
            using SqlCommand existe = new SqlCommand("SELECT OBJECT_ID('dbo.Venta_Entrega')", conexion);
            bool tieneEntrega = existe.ExecuteScalar() != DBNull.Value;
            string direccion;
            if (tieneEntrega)
            {
                direccion = "COALESCE(NULLIF(ve.domicilio,''),c.direccion,'')";
            }
            else
            {
                direccion = "COALESCE(c.direccion,'')";
            }

            string valorSeleccionado;
            if (tieneEntrega)
            {
                valorSeleccionado = " LEFT JOIN Venta_Entrega ve ON ve.id_venta=v.id_venta ";
            }
            else
            {
                valorSeleccionado = "";
            }
            // prepara la consulta y su conexion los valores variables se envian mediante parametros
            using SqlCommand comando = new SqlCommand(@"SELECT v.id_venta,d.id_variante,CONCAT(c.nombre_cliente,' ',c.apellido_cliente) cliente,
                CONCAT(p.nombre_producto,' (',vp.color,', ',vp.condicion_producto,')') producto,
                m.nombre_modalidad_entrega modalidad,d.cantidad vendidos,COALESCE(r.cantidad,0) preparados," + direccion + @" direccion
                FROM Venta v JOIN Cliente c ON c.id_cliente=v.id_cliente JOIN Estado_Venta ev ON ev.id_estado_venta=v.id_estado_venta
                JOIN Modalidad_Entrega m ON m.id_modalidad_entrega=v.id_modalidad_entrega
                JOIN (SELECT id_venta,id_variante,SUM(cantidad) cantidad FROM Detalle_Venta GROUP BY id_venta,id_variante) d ON d.id_venta=v.id_venta
                JOIN Variante_Producto vp ON vp.id_variante=d.id_variante JOIN Producto p ON p.id_producto=vp.id_producto
                LEFT JOIN (" + PreparadosSql + @") r ON r.id_venta=v.id_venta AND r.id_variante=d.id_variante " + (valorSeleccionado) + @"
                WHERE d.cantidad>COALESCE(r.cantidad,0) AND ev.nombre_estado_venta NOT LIKE N'Anulad%' AND ev.nombre_estado_venta NOT LIKE N'Cancelad%'", conexion);
            AgregarFiltro(comando, texto, "CONCAT(v.id_venta,' ',c.nombre_cliente,' ',c.apellido_cliente,' ',c.dni,' ',p.nombre_producto)");
            comando.CommandText += " ORDER BY v.fecha_venta,v.id_venta,d.id_variante";
            using SqlDataReader lector = comando.ExecuteReader();
            List<ProductoPorEntregar> lista = new List<ProductoPorEntregar>();
            while (lector.Read())
            {
                lista.Add(new ProductoPorEntregar
                {
                    IdVenta = (int)lector["id_venta"],
                    IdVariante = (int)lector["id_variante"],
                    Cliente = (string)lector["cliente"],
                    Producto = (string)lector["producto"],
                    Modalidad = (string)lector["modalidad"],
                    Vendidos = (int)lector["vendidos"],
                    Preparados = (int)lector["preparados"],
                    Direccion = (string)lector["direccion"]

                });
            }

            return lista;
        }

        // consulta listar y transforma cada fila recibida en los objetos que devuelve
        public List<EntregaRegistrada> Listar(string texto, bool soloPendientes)
        {
            // prepara la conexion configurada y la libera al terminar este alcance
            using SqlConnection conexion = new SqlConnection(Conexion.ObtenerCadena());
            // prepara la consulta y su conexion los valores variables se envian mediante parametros
            using SqlCommand comando = new SqlCommand(@"SELECT e.id_envio,r.id_venta,r.id_remito,CONCAT(c.nombre_cliente,' ',c.apellido_cliente) cliente,
                e.direccion,ee.nombre,e.fecha_entrega,
                COALESCE((SELECT STRING_AGG(CAST(CONCAT(dr.cantidad,' × ',dr.descripcion) AS nvarchar(max)),'; ')
                    FROM Detalle_Remito dr WHERE dr.id_remito=r.id_remito),'') productos
                FROM Envio e JOIN Remito r ON r.id_remito=e.id_remito JOIN Venta v ON v.id_venta=r.id_venta
                JOIN Cliente c ON c.id_cliente=v.id_cliente JOIN Estado_Envio ee ON ee.id_estado_envio=e.id_estado_envio WHERE 1=1", conexion);
            if (soloPendientes)
            {
                comando.CommandText += " AND ee.nombre=N'Pendiente'";
            }

            AgregarFiltro(comando, texto, "CONCAT(v.id_venta,' ',c.nombre_cliente,' ',c.apellido_cliente,' ',c.dni)");
            comando.CommandText += " ORDER BY r.fecha_emision DESC,e.id_envio DESC";
            conexion.Open();
            using SqlDataReader lector = comando.ExecuteReader();
            List<EntregaRegistrada> lista = new List<EntregaRegistrada>();
            while (lector.Read())
            {
                DateTime? valorFechaEntrega;
                if (lector["fecha_entrega"] == DBNull.Value)
                {
                    valorFechaEntrega = null;
                }
                else
                {
                    valorFechaEntrega = (DateTime)lector["fecha_entrega"];
                }
                lista.Add(new EntregaRegistrada
                {
                    IdEnvio = (int)lector["id_envio"],
                    IdVenta = (int)lector["id_venta"],
                    IdRemito = (int)lector["id_remito"],
                    Cliente = (string)lector["cliente"],
                    Productos = (string)lector["productos"],
                    Direccion = (string)lector["direccion"],
                    Estado = (string)lector["nombre"],
                    FechaEntrega = valorFechaEntrega
                });
            }

            return lista;
        }

        // resuelve agregar filtro con los datos recibidos y entrega el resultado al codigo que lo llama
        private static void AgregarFiltro(SqlCommand cmd, string texto, string expresion)
        {
            string[] palabras = texto.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < palabras.Length; i++)
            {
                cmd.CommandText += $" AND {expresion} COLLATE Latin1_General_CI_AI LIKE @p{i} ESCAPE '~'";
                cmd.Parameters.Add("@p" + i, SqlDbType.NVarChar, -1).Value = "%" + CD_Venta.EscaparBusqueda(palabras[i]) + "%";
            }
        }

        // coordina preparar dentro de una transaccion para confirmar o revertir sus escrituras juntas
        public void Preparar(int venta, int variante, int cantidad, string direccion, string horario, int usuario)
        {
            // prepara la conexion configurada y la libera al terminar este alcance
            using SqlConnection conexion = new SqlConnection(Conexion.ObtenerCadena());
            conexion.Open();
            using SqlTransaction transaccion = conexion.BeginTransaction(IsolationLevel.Serializable);
            // protege el acceso a los datos para atender los errores de la operacion
            try
            {
                ValidarVenta(conexion, transaccion, venta);
                using SqlCommand pendiente = Comando(conexion, transaccion, @"SELECT COALESCE(SUM(cantidad),0)-(SELECT COALESCE(SUM(cantidad),0) FROM (" + PreparadosSql + @") r
                    WHERE r.id_venta=@venta AND r.id_variante=@variante) FROM Detalle_Venta WHERE id_venta=@venta AND id_variante=@variante", ("@venta", venta), ("@variante", variante));
                if (cantidad <= 0 || cantidad > Convert.ToInt32(pendiente.ExecuteScalar()))
                {
                    // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                    throw new InvalidOperationException("La cantidad supera las unidades pendientes. Actualice el listado.");
                }

                // el descuento condicional impide stock negativo incluso con preparaciones simultaneas
                using SqlCommand stock = Comando(conexion, transaccion, "UPDATE Variante_Producto SET stock_disponible=stock_disponible-@cantidad WHERE id_variante=@variante AND stock_disponible>=@cantidad", ("@cantidad", cantidad), ("@variante", variante));
                if (stock.ExecuteNonQuery() != 1)
                {
                    // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                    throw new InvalidOperationException("No hay stock disponible para preparar esa cantidad.");
                }

                int estado = ObtenerOCrearIdPorNombre(conexion, transaccion, "Estado_Envio", "id_estado_envio", "nombre", "Pendiente");
                using SqlCommand modalidad = Comando(conexion, transaccion, @"SELECT m.nombre_modalidad_entrega FROM Venta v JOIN Modalidad_Entrega m ON m.id_modalidad_entrega=v.id_modalidad_entrega WHERE id_venta=@venta", ("@venta", venta));
                string tipo;
                if (Convert.ToString(modalidad.ExecuteScalar())!.StartsWith("Retiro", StringComparison.OrdinalIgnoreCase))
                {
                    tipo = "Retiro en local";
                }
                else
                {
                    tipo = "Envío a domicilio";
                }
                if (tipo == "Retiro en local")
                {
                    direccion = "Retiro en local";
                }

                using SqlCommand remito = Comando(conexion, transaccion, @"INSERT INTO Remito(id_venta,fecha_emision) OUTPUT INSERTED.id_remito
                    VALUES(@venta,SYSDATETIME())", ("@venta", venta));
                int idRemito = Convert.ToInt32(remito.ExecuteScalar());
                using SqlCommand detalle = Comando(conexion, transaccion, @"INSERT INTO Detalle_Remito(id_remito,id_variante,cantidad,descripcion)
                    SELECT @remito,vp.id_variante,@cantidad,LEFT(CONCAT(p.nombre_producto,' (',vp.color,', ',vp.condicion_producto,')'),300)
                    FROM Variante_Producto vp JOIN Producto p ON p.id_producto=vp.id_producto WHERE vp.id_variante=@variante;
                    INSERT INTO Envio(id_remito,direccion,horario_sugerido,tipo_envio,costo,id_estado_envio)
                    VALUES(@remito,@direccion,@horario,@tipo,0,@estado)", ("@remito", idRemito), ("@cantidad", cantidad), ("@variante", variante), ("@direccion", direccion), ("@horario", horario), ("@tipo", tipo), ("@estado", estado));
                detalle.ExecuteNonQuery();
                RegistrarActividad(conexion, transaccion, usuario, "PREPARAR_ENTREGA", venta, $"Remito {venta}; preparación interna {idRemito}; variante {variante}; cantidad {cantidad}");
                // confirma todos los cambios de esta transaccion como una unidad
                transaccion.Commit();
            }
            // atiende el error de esta operacion antes de informar o propagar el problema
            catch
            {
                // revierte los cambios no confirmados de esta transaccion
                transaccion.Rollback();
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw;
            }
        }

        // coordina confirmar dentro de una transaccion para confirmar o revertir sus escrituras juntas
        public void Confirmar(int envio, int usuario)
        {
            // prepara la conexion configurada y la libera al terminar este alcance
            using SqlConnection conexion = new SqlConnection(Conexion.ObtenerCadena());
            conexion.Open();
            using SqlTransaction transaccion = conexion.BeginTransaction(IsolationLevel.Serializable);
            // protege el acceso a los datos para atender los errores de la operacion
            try
            {
                using SqlCommand buscar = Comando(conexion, transaccion, "SELECT r.id_venta FROM Envio e JOIN Remito r ON r.id_remito=e.id_remito WHERE e.id_envio=@envio", ("@envio", envio));
                object? id = buscar.ExecuteScalar();
                if (id == null)
                {
                    // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                    throw new InvalidOperationException("Seleccione una entrega existente.");
                }

                int venta = Convert.ToInt32(id);
                ValidarVenta(conexion, transaccion, venta);
                int estado = ObtenerOCrearIdPorNombre(conexion, transaccion, "Estado_Envio", "id_estado_envio", "nombre", "Entregado");
                using SqlCommand actualizar = Comando(conexion, transaccion, @"UPDATE e SET id_estado_envio=@estado,fecha_entrega=SYSDATETIME()
                    FROM Envio e JOIN Estado_Envio ee ON ee.id_estado_envio=e.id_estado_envio WHERE e.id_envio=@envio AND ee.nombre=N'Pendiente'", ("@estado", estado), ("@envio", envio));
                if (actualizar.ExecuteNonQuery() != 1)
                {
                    // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                    throw new InvalidOperationException("La entrega ya fue confirmada o no está pendiente.");
                }

                int entregado = ObtenerOCrearIdPorNombre(conexion, transaccion, "Estado_Variante_Producto", "id_estado_variante_producto", "nombre_estado_variante_producto", "Entregado");
                // marca completos los productos cuya cantidad vendida ya se entrego; el estado de pago se conserva
                using SqlCommand detalles = Comando(conexion, transaccion, @"UPDATE d SET id_estado_variante_producto=@estado FROM Detalle_Venta d
                    WHERE d.id_venta=@venta AND (SELECT SUM(dv.cantidad) FROM Detalle_Venta dv WHERE dv.id_venta=d.id_venta AND dv.id_variante=d.id_variante)
                    <=(SELECT COALESCE(SUM(dr.cantidad),0) FROM Detalle_Remito dr JOIN Remito r ON r.id_remito=dr.id_remito
                    WHERE r.id_venta=d.id_venta AND dr.id_variante=d.id_variante AND EXISTS(SELECT 1 FROM Envio e JOIN Estado_Envio ee
                    ON ee.id_estado_envio=e.id_estado_envio WHERE e.id_remito=r.id_remito AND ee.nombre=N'Entregado'))", ("@estado", entregado), ("@venta", venta));
                detalles.ExecuteNonQuery();
                RegistrarActividad(conexion, transaccion, usuario, "CONFIRMAR_ENTREGA", venta, $"Entrega {envio} confirmada");
                // confirma todos los cambios de esta transaccion como una unidad
                transaccion.Commit();
            }
            // atiende el error de esta operacion antes de informar o propagar el problema
            catch
            {
                // revierte los cambios no confirmados de esta transaccion
                transaccion.Rollback();
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw;
            }
        }

        // envia los parametros de registrar retiro inmediato y ejecuta la escritura en la base de datos
        internal static void RegistrarRetiroInmediato(SqlConnection conexion, SqlTransaction transaccion, int idVenta, venta venta)
        {
            // comprueba la venta y bloquea su encabezado durante la operacion
            ValidarVenta(conexion, transaccion, idVenta);
            int estadoEnvio = ObtenerOCrearIdPorNombre(conexion, transaccion, "Estado_Envio", "id_estado_envio", "nombre", "Entregado");
            int estadoDetalle = ObtenerOCrearIdPorNombre(conexion, transaccion, "Estado_Variante_Producto", "id_estado_variante_producto", "nombre_estado_variante_producto", "Entregado");
            // crea un remito para todos los productos retirados
            using SqlCommand remito = Comando(conexion, transaccion, @"INSERT INTO Remito
            (id_venta, fecha_emision)
          OUTPUT INSERTED.id_remito
          VALUES (@venta, SYSDATETIME())", ("@venta", idVenta));
            int idRemito = Convert.ToInt32(remito.ExecuteScalar());
            // procesa cada producto de ventadetalles sin cambiar el orden de la coleccion
            foreach (DetalleNuevaVenta producto in venta.Detalles)
            {
                // descuenta stock solamente si alcanza para la cantidad solicitada
                using SqlCommand stock = Comando(conexion, transaccion, @"UPDATE Variante_Producto
              SET stock_disponible = stock_disponible - @cantidad
              WHERE id_variante = @variante
                AND stock_disponible >= @cantidad", ("@cantidad", producto.Cantidad), ("@variante", producto.IdVariante));
                if (stock.ExecuteNonQuery() != 1)
                {
                    // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                    throw new InvalidOperationException("No hay stock suficiente para registrar el retiro.");
                }

                // registra la cantidad retirada y la descripcion del producto
                using SqlCommand detalle = Comando(conexion, transaccion, @"INSERT INTO Detalle_Remito
                (id_remito, id_variante, cantidad, descripcion)
              SELECT
                @remito,
                vp.id_variante,
                @cantidad,
                LEFT(
                    CONCAT(
                        p.nombre_producto,
                        ' (', vp.color,
                        ', ', vp.condicion_producto, ')'),
                    300)
              FROM Variante_Producto vp
              JOIN Producto p ON p.id_producto = vp.id_producto
              WHERE vp.id_variante = @variante", ("@remito", idRemito), ("@cantidad", producto.Cantidad), ("@variante", producto.IdVariante));
                if (detalle.ExecuteNonQuery() != 1)
                {
                    // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                    throw new InvalidOperationException("No se pudo registrar un producto en el remito.");
                }
            }

            // registra el retiro como entregado desde el momento de la venta
            using SqlCommand envio = Comando(conexion, transaccion, @"INSERT INTO Envio
            (id_remito, direccion, horario_sugerido,
             tipo_envio, costo, id_estado_envio, fecha_entrega)
          VALUES
            (@remito, @direccion, @horario,
             N'Retiro en local', 0, @estado, SYSDATETIME())", ("@remito", idRemito), ("@direccion", "Retiro en local"), ("@horario", ""), ("@estado", estadoEnvio));
            envio.ExecuteNonQuery();
            // marca los productos de esta venta como entregados
            using SqlCommand actualizarDetalles = Comando(conexion, transaccion, @"UPDATE Detalle_Venta
          SET id_estado_variante_producto = @estado
          WHERE id_venta = @venta", ("@estado", estadoDetalle), ("@venta", idVenta));
            actualizarDetalles.ExecuteNonQuery();
            // deja constancia del retiro en el historial
            RegistrarActividad(conexion, transaccion, venta.IdUsuario, "CONFIRMAR_ENTREGA", idVenta, $"Retiro inmediato en local; remito interno {idRemito}");
        }
    }
}
