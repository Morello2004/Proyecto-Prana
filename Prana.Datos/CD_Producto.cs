using Microsoft.Data.SqlClient;
using Prana.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Prana.Datos
{
    // agrupa los datos y operaciones de cd producto
    public class CD_Producto
    {
        // recupera los colores ya guardados para reutilizarlos sin crear una tabla adicional
        public List<string> ObtenerColores()
        {
            List<string> colores = new List<string>();
            using SqlConnection conexion = new SqlConnection(Conexion.ObtenerCadena());
            using SqlCommand comando = new SqlCommand("SELECT DISTINCT UPPER(LTRIM(RTRIM(color))) AS color FROM dbo.Variante_Producto WHERE NULLIF(LTRIM(RTRIM(color)), N'') IS NOT NULL ORDER BY color", conexion);
            conexion.Open();
            using SqlDataReader lector = comando.ExecuteReader();
            while (lector.Read())
            {
                colores.Add(lector.GetString(0));
            }
            return colores;
        }
        // calcula las unidades vendidas no anuladas que todavia no fueron preparadas
        private const string StockComprometidoSql = @"
            SELECT vendidos.id_variante,
                SUM(CASE WHEN vendidos.cantidad > COALESCE(preparados.cantidad, 0)
                    THEN vendidos.cantidad - COALESCE(preparados.cantidad, 0) ELSE 0 END) AS stock_comprometido
            FROM
            (
                SELECT d.id_venta, d.id_variante, SUM(d.cantidad) AS cantidad
                FROM Detalle_Venta d
                GROUP BY d.id_venta, d.id_variante
            ) vendidos
            INNER JOIN Venta venta ON venta.id_venta = vendidos.id_venta
            INNER JOIN Estado_Venta estado ON estado.id_estado_venta = venta.id_estado_venta
            OUTER APPLY
            (
                SELECT SUM(dr.cantidad) AS cantidad
                FROM Remito r
                INNER JOIN Detalle_Remito dr ON dr.id_remito = r.id_remito
                WHERE r.id_venta = vendidos.id_venta
                    AND dr.id_variante = vendidos.id_variante
            ) preparados
            WHERE estado.nombre_estado_venta NOT LIKE N'Anulad%'
                AND estado.nombre_estado_venta NOT LIKE N'Cancelad%'
            GROUP BY vendidos.id_variante";
        // desactiva una variante sin eliminarla de la base de datos
        public bool DesactivarVariante(int idVariante)
        {
            string consulta = @"
                                UPDATE Variante_Producto
                                        SET activo = 0
                                        WHERE id_variante = @idVariante
                                        AND activo = 1;";
            // prepara la conexion configurada y la libera al terminar este alcance
            using SqlConnection conexion = new SqlConnection(Conexion.ObtenerCadena());
            // prepara la consulta y su conexion los valores variables se envian mediante parametros
            using SqlCommand comando = new SqlCommand(consulta, conexion);
            comando.Parameters.Add("@idVariante", SqlDbType.Int).Value = idVariante;
            conexion.Open();
            return comando.ExecuteNonQuery() > 0;
        }

        // registra un ingreso de stock con sus detalles y actualiza el stock de las variantes
        public bool RegistrarIngreso(int idProveedor, DateTime fechaIngreso, string remitoProveedor, List<(int idVariante, int cantidad, decimal costoUnitario)> detalles, string? nombreProveedor = null)
        {
            // consultas para insertar ingreso y sus detalles y actualizar variantes
            string consultaInsertIngreso = @"
                INSERT INTO Ingreso_Stock (id_proveedor, numero_remito_proveedor, fecha_hora_ingreso)
                OUTPUT INSERTED.id_ingreso
                VALUES (@idProveedor, @remitoProveedor, @fechaIngreso)";
            string consultaInsertDetalle = @"
                INSERT INTO Detalle_Ingreso (id_ingreso, id_variante, cantidad, costo_unitario)
                VALUES (@idIngreso, @idVariante, @cantidad, @costoUnitario)";
            string consultaActualizarVariante = @"
                UPDATE Variante_Producto
                SET stock_disponible = stock_disponible + @cantidad,
                    costo_unitario = @costoUnitario
                WHERE id_variante = @idVariante";
            // prepara la conexion configurada y la libera al terminar este alcance
            using SqlConnection conexion = new SqlConnection(Conexion.ObtenerCadena());
            conexion.Open();
            using SqlTransaction transaccion = conexion.BeginTransaction();
            // protege el acceso a los datos para atender los errores de la operacion
            try
            {
                if (nombreProveedor != null)
                {
                    idProveedor = CD_Proveedor.ObtenerOCrear(conexion, transaccion, nombreProveedor);
                }

                int idIngreso;
                // inserta el ingreso y obtiene el id generado
                using (SqlCommand cmdInsertIngreso = new SqlCommand(consultaInsertIngreso, conexion, transaccion))
                {
                    cmdInsertIngreso.Parameters.Add("@idProveedor", SqlDbType.Int).Value = idProveedor;
                    // distingue un dato vacio de un valor ingresado antes de continuar
                    if (string.IsNullOrWhiteSpace(remitoProveedor))
                    {
                        cmdInsertIngreso.Parameters.Add("@remitoProveedor", SqlDbType.NVarChar, 100).Value = (object)DBNull.Value;
                    }
                    else
                    {
                        cmdInsertIngreso.Parameters.Add("@remitoProveedor", SqlDbType.NVarChar, 100).Value = remitoProveedor;
                    }
                    cmdInsertIngreso.Parameters.Add("@fechaIngreso", SqlDbType.DateTime2).Value = fechaIngreso;
                    object resultado = cmdInsertIngreso.ExecuteScalar();
                    if (resultado == null)
                    {
                        // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                        throw new InvalidOperationException("La base de datos no devolvió el identificador del ingreso.");
                    }

                    idIngreso = Convert.ToInt32(resultado);
                }

                // inserta cada detalle y actualiza la variante correspondiente recorre cada detalle de la coleccion; el bloque se aplica a un elemento por vez
                foreach ((int idVariante, int cantidad, decimal costoUnitario) detalle in detalles)
                {
                    // prepara la consulta y su conexion los valores variables se envian mediante parametros
                    using SqlCommand cmdDetalle = new SqlCommand(consultaInsertDetalle, conexion, transaccion);
                    cmdDetalle.Parameters.Add("@idIngreso", SqlDbType.Int).Value = idIngreso;
                    cmdDetalle.Parameters.Add("@idVariante", SqlDbType.Int).Value = detalle.idVariante;
                    cmdDetalle.Parameters.Add("@cantidad", SqlDbType.Int).Value = detalle.cantidad;
                    SqlParameter paramCosto = cmdDetalle.Parameters.Add("@costoUnitario", SqlDbType.Decimal);
                    paramCosto.Precision = 10;
                    paramCosto.Scale = 2;
                    paramCosto.Value = detalle.costoUnitario;
                    cmdDetalle.ExecuteNonQuery();
                    // prepara la consulta y su conexion los valores variables se envian mediante parametros
                    using SqlCommand cmdActualizar = new SqlCommand(consultaActualizarVariante, conexion, transaccion);
                    cmdActualizar.Parameters.Add("@cantidad", SqlDbType.Int).Value = detalle.cantidad;
                    SqlParameter paramCosto2 = cmdActualizar.Parameters.Add("@costoUnitario", SqlDbType.Decimal);
                    paramCosto2.Precision = 10;
                    paramCosto2.Scale = 2;
                    paramCosto2.Value = detalle.costoUnitario;
                    cmdActualizar.Parameters.Add("@idVariante", SqlDbType.Int).Value = detalle.idVariante;
                    int filas = cmdActualizar.ExecuteNonQuery();
                    if (filas == 0)
                    {
                        // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                        throw new InvalidOperationException("No se encontró el producto del ingreso. Vuelva a seleccionarlo.");
                    }
                }

                // confirma todos los cambios de esta transaccion como una unidad
                transaccion.Commit();
                return true;
            }
            // atiende el error de esta operacion antes de informar o propagar el problema
            catch
            {
                try
                {
                    // revierte los cambios no confirmados de esta transaccion
                    transaccion.Rollback();
                }
                // atiende el error de esta operacion antes de informar o propagar el problema
                catch
                {
                }

                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw;
            }
        }

        // envia los parametros de actualizar producto desde stock y ejecuta la escritura en la base de datos
        public bool ActualizarProductoDesdeStock(int idVariante, string modelo, string nombreProducto, string descripcion, string color, string condicion, decimal costo, decimal precio, int stockFisico)
        {
            string consulta = @"
        UPDATE Producto
        SET nombre_producto = @nombreProducto,
            descripcion = @descripcion,
            modelo = @modelo
        WHERE id_producto =
        (
            SELECT id_producto
            FROM Variante_Producto
            WHERE id_variante = @idVariante
        );

        UPDATE Variante_Producto
        SET color = @color,
            condicion_producto = @condicion,
            costo_unitario = @costo,
            precio_venta_unitario = @precio,
            stock_disponible = @stockFisico
        WHERE id_variante = @idVariante;";
            // crea la conexion con sql server
            using SqlConnection conexion = new SqlConnection(Conexion.ObtenerCadena());
            // prepara la consulta y su conexion los valores variables se envian mediante parametros
            using SqlCommand comando = new SqlCommand(consulta, conexion);
            // envia el identificador de la variante
            comando.Parameters.AddWithValue("@idVariante", idVariante);
            // envia el modelo
            comando.Parameters.AddWithValue("@modelo", modelo);
            // envia el nombre del producto
            comando.Parameters.AddWithValue("@nombreProducto", nombreProducto);
            // envia la descripcion
            comando.Parameters.AddWithValue("@descripcion", descripcion);
            // envia el color
            comando.Parameters.AddWithValue("@color", color);
            // envia la condicion
            comando.Parameters.AddWithValue("@condicion", condicion);
            // envia el costo
            comando.Parameters.AddWithValue("@costo", costo);
            // envia el precio
            comando.Parameters.AddWithValue("@precio", precio);
            // envia el stock fisico
            comando.Parameters.AddWithValue("@stockFisico", stockFisico);
            // abre la conexion abre el recurso antes de usarlo; una conexion debe estar abierta para enviar consultas
            conexion.Open();
            // El artículo y su variante se actualizan juntos; un error revierte ambos cambios.
            using SqlTransaction transaccion = conexion.BeginTransaction();
            comando.Transaction = transaccion;
            try
            {
                int filas = comando.ExecuteNonQuery();
                transaccion.Commit();
                return filas > 0;
            }
            catch
            {
                if (transaccion.Connection != null) transaccion.Rollback();
                throw;
            }
        }

        // obtiene el producto completo al que pertenece una variante
        public producto? obtenerProductoPorVariante(int idVariante)
        {
            // seleccion corregida: alias de columnas y union del calculo de stock similar a buscarproducto
            string consulta = @"
                                SELECT
                                    p.id_producto AS id_producto,
                                    p.nombre_producto AS NombreProducto,
                                    p.descripcion AS Descripcion,
                                    p.modelo AS Modelo,
                                    c.nombre_categoria AS nombre_categoria,
                                    m.nombre_marca AS nombre_marca,
                                    v.id_variante AS IdVariante,
                                    v.color AS Color,
                                    v.condicion_producto AS CondicionProducto,
                                    v.costo_unitario AS Costo,
                                    v.precio_venta_unitario AS Precio,
                                    v.stock_disponible AS StockFisico,
                                    COALESCE(stock.stock_comprometido, 0) AS StockComprometido
                                FROM Producto AS p
                                INNER JOIN Categoria AS c
                                    ON c.id_categoria = p.id_categoria
                                INNER JOIN Marca AS m
                                    ON m.id_marca = p.id_marca
                                INNER JOIN Variante_Producto AS v
                                    ON v.id_producto = p.id_producto
                                LEFT JOIN
                                (
                                    " + StockComprometidoSql + @"
                                ) AS stock
                                    ON stock.id_variante = v.id_variante
                                WHERE v.id_variante = @idVariante
                                    AND v.activo = 1
                                ORDER BY v.id_variante";
            // crea la conexion con sql server
            using SqlConnection conexion = new SqlConnection(Conexion.ObtenerCadena());
            // prepara la consulta y su conexion los valores variables se envian mediante parametros
            using SqlCommand comando = new SqlCommand(consulta, conexion);
            // envia el identificador de la variante
            comando.Parameters.Add("@idVariante", SqlDbType.Int).Value = idVariante;
            // abre la conexion abre el recurso antes de usarlo; una conexion debe estar abierta para enviar consultas
            conexion.Open();
            // ejecuta la consulta
            using SqlDataReader lector = comando.ExecuteReader();
            // guarda el producto encontrado
            producto? productoEncontrado = null;
            // recorre todas las variantes del producto avanza el lector de sql una fila por vez hasta agotar el resultado
            while (lector.Read())
            {
                // crea el producto solamente en la primera fila
                if (productoEncontrado == null)
                {
                    string valorDescripcion;
                    if (lector.IsDBNull(lector.GetOrdinal("Descripcion")))
                    {
                        valorDescripcion = string.Empty;
                    }
                    else
                    {
                        valorDescripcion = lector.GetString(lector.GetOrdinal("Descripcion"));
                    }

                    string valorModelo;
                    if (lector.IsDBNull(lector.GetOrdinal("Modelo")))
                    {
                        valorModelo = string.Empty;
                    }
                    else
                    {
                        valorModelo = lector.GetString(lector.GetOrdinal("Modelo"));
                    }
                    productoEncontrado = new producto
                    {
                        IdProducto = lector.GetInt32(lector.GetOrdinal("id_producto")),
                        NombreProducto = lector.GetString(lector.GetOrdinal("NombreProducto")),
                        Descripcion = valorDescripcion,
                        Categoria = lector.GetString(lector.GetOrdinal("nombre_categoria")),
                        Marca = lector.GetString(lector.GetOrdinal("nombre_marca")),
                        Modelo = valorModelo
                    };
                }
                string valorColor;
                if (lector.IsDBNull(lector.GetOrdinal("Color")))
                {
                    valorColor = string.Empty;
                }
                else
                {
                    valorColor = lector.GetString(lector.GetOrdinal("Color"));
                }

                VarianteProducto varianteEncontrada = new VarianteProducto
                {
                    IdVariante = lector.GetInt32(lector.GetOrdinal("IdVariante")),
                    Color = valorColor,
                    CondicionProducto = lector.GetString(lector.GetOrdinal("CondicionProducto")),
                    Costo = lector.GetDecimal(lector.GetOrdinal("Costo")),
                    Precio = lector.GetDecimal(lector.GetOrdinal("Precio")),
                    StockFisico = lector.GetInt32(lector.GetOrdinal("StockFisico")),
                    StockComprometido = lector.GetInt32(lector.GetOrdinal("StockComprometido"))

                };
                // agrega la variante al producto
                productoEncontrado.Variantes.Add(varianteEncontrada);
            }

            // devuelve el producto encontrado
            return productoEncontrado;
        }

        // desactiva una variante sin eliminarla fisicamente
        public bool desactivarVariante(int idVariante)
        {
            string consulta = @"
                               UPDATE Variante_Producto
                                       SET activo = 0
                                       WHERE id_variante = @idVariante
                                         AND activo = 1";
            // crea la conexion
            using SqlConnection conexion = new SqlConnection(Conexion.ObtenerCadena());
            // prepara la consulta y su conexion los valores variables se envian mediante parametros
            using SqlCommand comando = new SqlCommand(consulta, conexion);
            // envia el identificador
            comando.Parameters.Add("@idVariante", SqlDbType.Int).Value = idVariante;
            // abre la conexion abre el recurso antes de usarlo; una conexion debe estar abierta para enviar consultas
            conexion.Open();
            // devuelve true si se modifico una fila
            return comando.ExecuteNonQuery() > 0;
        }

        // consulta buscar producto y transforma cada fila recibida en los objetos que devuelve
        public List<producto> buscarProducto(string nombreProducto, int? idCategoria, int? idMarca, string condicionProducto, string modelo, bool buscarPorPrefijo = false)
        {
            // guarda los productos encontrados
            List<producto> productosEncontrados = new List<producto>();
            string consulta = @"
                                SELECT
                                        p.id_producto AS IdProducto,
                                        p.nombre_producto AS NombreProducto,
                                        p.descripcion AS Descripcion,
                                        p.modelo AS Modelo,

                                        v.id_variante AS IdVariante,
                                        v.color AS Color,
                                        v.condicion_producto AS CondicionProducto,
                                        v.costo_unitario AS Costo,
                                        v.precio_venta_unitario AS Precio,

                                        v.stock_disponible AS StockFisico,

                                        COALESCE(stock.stock_comprometido, 0)
                                            AS StockComprometido

                                    FROM Producto AS p

                                    INNER JOIN Variante_Producto AS v
                                        ON v.id_producto = p.id_producto

                                    LEFT JOIN
                                    (
                                        " + StockComprometidoSql + @"
                                    ) AS stock
                                        ON stock.id_variante = v.id_variante

                                    WHERE
                                    (
                                        UPPER(p.nombre_producto) LIKE UPPER(@nombreProducto)
                                        OR UPPER(p.modelo) LIKE UPPER(@nombreProducto)
                                    )

                                        AND
                                        (
                                            @idCategoria IS NULL
                                            OR p.id_categoria = @idCategoria
                                        )

                                        AND
                                        (
                                            @idMarca IS NULL
                                            OR p.id_marca = @idMarca
                                        )

                                        AND
                                        (
                                            @condicionProducto = ''
                                            OR UPPER(LTRIM(RTRIM(v.condicion_producto))) = UPPER(@condicionProducto)
                                        )

                                        AND (@modelo = '' OR UPPER(p.modelo) LIKE UPPER(@modelo))
                                        AND v.activo = 1;";
            // prepara la conexion configurada y la libera al terminar este alcance
            using SqlConnection conexion = new SqlConnection(Conexion.ObtenerCadena());
            // prepara la consulta y su conexion los valores variables se envian mediante parametros
            using SqlCommand comando = new SqlCommand(consulta, conexion);
            if (buscarPorPrefijo)
            {
                comando.Parameters.Add("@nombreProducto", SqlDbType.NVarChar, 100).Value = nombreProducto.Trim() + "%";
            }
            else
            {
                comando.Parameters.Add("@nombreProducto", SqlDbType.NVarChar, 100).Value = "%" + nombreProducto.Trim() + "%";
            }
            // comprueba si se recibio el valor opcional antes de utilizarlo
            if (idCategoria.HasValue)
            {
                comando.Parameters.Add("@idCategoria", SqlDbType.Int).Value = idCategoria.Value;
            }
            else
            {
                comando.Parameters.Add("@idCategoria", SqlDbType.Int).Value = DBNull.Value;
            }
            // comprueba si se recibio el valor opcional antes de utilizarlo
            if (idMarca.HasValue)
            {
                comando.Parameters.Add("@idMarca", SqlDbType.Int).Value = idMarca.Value;
            }
            else
            {
                comando.Parameters.Add("@idMarca", SqlDbType.Int).Value = DBNull.Value;
            }
            comando.Parameters.Add("@condicionProducto", SqlDbType.NVarChar, 20).Value = condicionProducto;
            // distingue un dato vacio de un valor ingresado antes de continuar
            if (string.IsNullOrWhiteSpace(modelo))
            {
                comando.Parameters.Add("@modelo", SqlDbType.NVarChar, 100).Value = string.Empty;
            }
            else
            {
                comando.Parameters.Add("@modelo", SqlDbType.NVarChar, 100).Value = "%" + modelo.Trim() + "%";
            }
            conexion.Open();
            using SqlDataReader lector = comando.ExecuteReader();
            while (lector.Read())
            {
                int idProducto = lector.GetInt32(lector.GetOrdinal("IdProducto"));
                producto? productoEncontrado = null;
                // procesa cada producto de la lista de productos encontrados sin cambiar el orden de la coleccion
                foreach (producto productoDeLaLista in productosEncontrados)
                {
                    if (productoDeLaLista.IdProducto == idProducto)
                    {
                        productoEncontrado = productoDeLaLista;
                        break;
                    }
                }

                if (productoEncontrado == null)
                {
                    string valorDescripcion1;
                    if (lector.IsDBNull(lector.GetOrdinal("Descripcion")))
                    {
                        valorDescripcion1 = string.Empty;
                    }
                    else
                    {
                        valorDescripcion1 = lector.GetString(lector.GetOrdinal("Descripcion"));
                    }

                    string valorModelo1;
                    if (lector.IsDBNull(lector.GetOrdinal("Modelo")))
                    {
                        valorModelo1 = string.Empty;
                    }
                    else
                    {
                        valorModelo1 = lector.GetString(lector.GetOrdinal("Modelo"));
                    }
                    productoEncontrado = new producto
                    {
                        IdProducto = idProducto,
                        NombreProducto = lector.GetString(lector.GetOrdinal("NombreProducto")),
                        Descripcion = valorDescripcion1,
                        Modelo = valorModelo1
                    };
                    productosEncontrados.Add(productoEncontrado);
                }
                string valorColor1;
                if (lector.IsDBNull(lector.GetOrdinal("Color")))
                {
                    valorColor1 = string.Empty;
                }
                else
                {
                    valorColor1 = lector.GetString(lector.GetOrdinal("Color"));
                }

                VarianteProducto varianteEncontrada = new VarianteProducto
                {
                    IdVariante = lector.GetInt32(lector.GetOrdinal("IdVariante")),
                    Color = valorColor1,
                    CondicionProducto = lector.GetString(lector.GetOrdinal("CondicionProducto")),
                    Costo = lector.GetDecimal(lector.GetOrdinal("Costo")),
                    Precio = lector.GetDecimal(lector.GetOrdinal("Precio")),
                    StockFisico = lector.GetInt32(lector.GetOrdinal("StockFisico")),
                    StockComprometido = lector.GetInt32(lector.GetOrdinal("StockComprometido"))

                };
                productoEncontrado.Variantes.Add(varianteEncontrada);
            }

            return productosEncontrados;
        }

        // coordina nuevo producto dentro de una transaccion para confirmar o revertir sus escrituras juntas
        public bool nuevoProducto(producto producto)
        {
            {
                // consulta que busca una marca por su nombre
                string consultaBuscarMarca = @"
                                             SELECT id_marca
                                             FROM Marca
                                             WHERE UPPER(LTRIM(RTRIM(nombre_marca))) = UPPER(@nombreMarca)";
                // consulta que crea una marca y devuelve su identificador
                string consultaCrearMarca = @"
                                             INSERT INTO Marca (nombre_marca)
                                             OUTPUT INSERTED.id_marca
                                             VALUES (@nombreMarca)";
                // consulta que busca una categoria por su nombre
                string consultaBuscarCategoria = @"
                                             SELECT id_categoria
                                             FROM Categoria
                                             WHERE UPPER(LTRIM(RTRIM(nombre_categoria))) = UPPER(@nombreCategoria)";
                // consulta que crea una categoria y devuelve su identificador
                string consultaCrearCategoria = @"
                                             INSERT INTO Categoria (nombre_categoria)
                                             OUTPUT INSERTED.id_categoria
                                             VALUES (@nombreCategoria)";
                // consulta que crea el producto y devuelve su identificador
                string consultaProducto = @"
                                             INSERT INTO Producto
                                                 (nombre_producto,modelo, id_marca, descripcion, id_categoria)
                                             OUTPUT INSERTED.id_producto
                                             VALUES
                                                 (@nombreProducto,@modelo, @idMarca, @descripcion, @idCategoria)";
                // consulta que crea una variante del producto
                string consultaVariante = @"
                                             INSERT INTO Variante_Producto
                                                 (id_producto, color, condicion_producto, precio_venta_unitario, stock_disponible)
                                             VALUES
                                                 (@idProducto, @color, @condicionProducto, @precio, @stockDisponible)";
                // crea y abre la conexion con sql server
                using SqlConnection conexion = new SqlConnection(Conexion.ObtenerCadena());
                conexion.Open();
                // inicia una transaccion para guardar todo junto
                using SqlTransaction transaccion = conexion.BeginTransaction();
                // protege el acceso a los datos para atender los errores de la operacion
                try
                {
                    int idMarca;
                    // busca la marca por su nombre
                    using (SqlCommand comandoBuscarMarca = new SqlCommand(consultaBuscarMarca, conexion, transaccion))
                    {
                        comandoBuscarMarca.Parameters.Add("@nombreMarca", SqlDbType.NVarChar, 100).Value = producto.Marca;
                        object resultadoMarca = comandoBuscarMarca.ExecuteScalar();
                        if (resultadoMarca != null)
                        {
                            // utiliza la marca existente
                            idMarca = Convert.ToInt32(resultadoMarca);
                        }
                        else
                        {
                            // crea la marca si todavia no existe
                            using SqlCommand comandoCrearMarca = new SqlCommand(consultaCrearMarca, conexion, transaccion);
                            comandoCrearMarca.Parameters.Add("@nombreMarca", SqlDbType.NVarChar, 100).Value = producto.Marca;
                            idMarca = Convert.ToInt32(comandoCrearMarca.ExecuteScalar());
                        }
                    }

                    int idCategoria;
                    // busca la categoria por su nombre
                    using (SqlCommand comandoBuscarCategoria = new SqlCommand(consultaBuscarCategoria, conexion, transaccion))
                    {
                        comandoBuscarCategoria.Parameters.Add("@nombreCategoria", SqlDbType.NVarChar, 100).Value = producto.Categoria;
                        object resultadoCategoria = comandoBuscarCategoria.ExecuteScalar();
                        if (resultadoCategoria != null)
                        {
                            // utiliza la categoria existente
                            idCategoria = Convert.ToInt32(resultadoCategoria);
                        }
                        else
                        {
                            // crea la categoria si todavia no existe
                            using SqlCommand comandoCrearCategoria = new SqlCommand(consultaCrearCategoria, conexion, transaccion);
                            comandoCrearCategoria.Parameters.Add("@nombreCategoria", SqlDbType.NVarChar, 100).Value = producto.Categoria;
                            idCategoria = Convert.ToInt32(comandoCrearCategoria.ExecuteScalar());
                        }
                    }

                    int idProducto;
                    // crea el producto
                    using (SqlCommand comandoProducto = new SqlCommand(consultaProducto, conexion, transaccion))
                    {
                        comandoProducto.Parameters.Add("@nombreProducto", SqlDbType.NVarChar, 150).Value = producto.NombreProducto;
                        comandoProducto.Parameters.Add("@idMarca", SqlDbType.Int).Value = idMarca;
                        // distingue un dato vacio de un valor ingresado antes de continuar
                        if (string.IsNullOrWhiteSpace(producto.Descripcion))
                        {
                            comandoProducto.Parameters.Add("@descripcion", SqlDbType.NVarChar, 500).Value = (object)DBNull.Value;
                        }
                        else
                        {
                            comandoProducto.Parameters.Add("@descripcion", SqlDbType.NVarChar, 500).Value = producto.Descripcion;
                        }
                        comandoProducto.Parameters.Add("@idCategoria", SqlDbType.Int).Value = idCategoria;
                        comandoProducto.Parameters.Add("@modelo", SqlDbType.VarChar, 15).Value = (object?)producto.Modelo ?? DBNull.Value;
                        idProducto = Convert.ToInt32(comandoProducto.ExecuteScalar());
                    }

                    // recorre todas las variantes agregadas al producto recorre cada variante de la coleccion; el bloque se aplica a un elemento por vez
                    foreach (VarianteProducto variante in producto.Variantes)
                    {
                        // prepara la consulta y su conexion los valores variables se envian mediante parametros
                        using SqlCommand comandoVariante = new SqlCommand(consultaVariante, conexion, transaccion);
                        comandoVariante.Parameters.Add("@idProducto", SqlDbType.Int).Value = idProducto;
                        // guarda null cuando la variante no posee color
                        if (string.IsNullOrWhiteSpace(variante.Color))
                        {
                            comandoVariante.Parameters.Add("@color", SqlDbType.NVarChar, 50).Value = DBNull.Value;
                        }
                        else
                        {
                            comandoVariante.Parameters.Add("@color", SqlDbType.NVarChar, 50).Value = variante.Color;
                        }
                        comandoVariante.Parameters.Add("@condicionProducto", SqlDbType.NVarChar, 10).Value = variante.CondicionProducto;
                        SqlParameter parametroPrecio = comandoVariante.Parameters.Add("@precio", SqlDbType.Decimal);
                        parametroPrecio.Precision = 10;
                        parametroPrecio.Scale = 2;
                        parametroPrecio.Value = variante.Precio;
                        comandoVariante.Parameters.Add("@stockDisponible", SqlDbType.Int).Value = 0;
                        // guarda la variante actual
                        comandoVariante.ExecuteNonQuery();
                    }

                    // confirma el producto y todas sus variantes
                    transaccion.Commit();
                    return true;
                }
                // atiende el error de esta operacion antes de informar o propagar el problema
                catch
                {
                    // deshace todos los cambios si falla alguna operacion
                    transaccion.Rollback();
                    return false;
                }
            }
        }
    }
}
