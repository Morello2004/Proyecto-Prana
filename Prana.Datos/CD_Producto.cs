using Microsoft.Data.SqlClient;
using Prana.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;



namespace Prana.Datos
{
    public class CD_Producto
    {



        // Desactiva una variante sin eliminarla de la base de datos
        public bool DesactivarVariante(int idVariante)
        {
            string consulta = @"
                                UPDATE Variante_Producto
                                        SET activo = 0
                                        WHERE id_variante = @idVariante
                                        AND activo = 1;";

            using SqlConnection conexion = new SqlConnection(Conexion.ObtenerCadena());

            using SqlCommand comando = new SqlCommand(consulta, conexion);

            comando.Parameters.Add( "@idVariante", SqlDbType.Int).Value = idVariante;

            conexion.Open();

            return comando.ExecuteNonQuery() > 0;
        }

        // Registra un ingreso de stock con sus detalles y actualiza el stock de las variantes
        public bool RegistrarIngreso(int idProveedor, DateTime fechaIngreso, string remitoProveedor, List<(int idVariante, int cantidad, decimal costoUnitario)> detalles)
        {
            // Consultas para insertar ingreso y sus detalles y actualizar variantes
            string consultaInsertIngreso = @"
                INSERT INTO Ingreso_Stock (id_proveedor, remito_proveedor, fecha_ingreso)
                OUTPUT INSERTED.id_ingreso
                VALUES (@idProveedor, @remitoProveedor, @fechaIngreso)";

            string consultaInsertDetalle = @"
                INSERT INTO Detalle_Ingreso (id_ingreso, id_variante, cantidad, costo_unitario)
                VALUES (@idIngreso, @idVariante, @cantidad, @costoUnitario)";

            string consultaActualizarVariante = @"
                UPDATE Variante_Producto
                SET stock_disponible = stock_disponible + @cantidad,
                    costo = @costoUnitario
                WHERE id_variante = @idVariante";

            using SqlConnection conexion = new SqlConnection(Conexion.ObtenerCadena());
            conexion.Open();

            using SqlTransaction transaccion = conexion.BeginTransaction();

            try
            {
                int idIngreso;

                // Inserta el ingreso y obtiene el id generado
                using (SqlCommand cmdInsertIngreso = new SqlCommand(consultaInsertIngreso, conexion, transaccion))
                {
                    cmdInsertIngreso.Parameters.Add("@idProveedor", SqlDbType.Int).Value = idProveedor;
                    cmdInsertIngreso.Parameters.Add("@remitoProveedor", SqlDbType.NVarChar, 100).Value = string.IsNullOrWhiteSpace(remitoProveedor) ? (object)DBNull.Value : remitoProveedor;
                    cmdInsertIngreso.Parameters.Add("@fechaIngreso", SqlDbType.DateTime2).Value = fechaIngreso;

                    object resultado = cmdInsertIngreso.ExecuteScalar();
                    if (resultado == null)
                    {
                        transaccion.Rollback();
                        return false;
                    }

                    idIngreso = Convert.ToInt32(resultado);
                }

                // Inserta cada detalle y actualiza la variante correspondiente
                foreach (var detalle in detalles)
                {
                    using SqlCommand cmdDetalle = new SqlCommand(consultaInsertDetalle, conexion, transaccion);
                    cmdDetalle.Parameters.Add("@idIngreso", SqlDbType.Int).Value = idIngreso;
                    cmdDetalle.Parameters.Add("@idVariante", SqlDbType.Int).Value = detalle.idVariante;
                    cmdDetalle.Parameters.Add("@cantidad", SqlDbType.Int).Value = detalle.cantidad;

                    SqlParameter paramCosto = cmdDetalle.Parameters.Add("@costoUnitario", SqlDbType.Decimal);
                    paramCosto.Precision = 18;
                    paramCosto.Scale = 2;
                    paramCosto.Value = detalle.costoUnitario;

                    cmdDetalle.ExecuteNonQuery();

                    using SqlCommand cmdActualizar = new SqlCommand(consultaActualizarVariante, conexion, transaccion);
                    cmdActualizar.Parameters.Add("@cantidad", SqlDbType.Int).Value = detalle.cantidad;
                    SqlParameter paramCosto2 = cmdActualizar.Parameters.Add("@costoUnitario", SqlDbType.Decimal);
                    paramCosto2.Precision = 18;
                    paramCosto2.Scale = 2;
                    paramCosto2.Value = detalle.costoUnitario;
                    cmdActualizar.Parameters.Add("@idVariante", SqlDbType.Int).Value = detalle.idVariante;

                    int filas = cmdActualizar.ExecuteNonQuery();
                    if (filas == 0)
                    {
                        // variante no encontrada -> rollback
                        transaccion.Rollback();
                        return false;
                    }
                }

                transaccion.Commit();
                return true;
            }
            catch
            {
                try { transaccion.Rollback(); } catch { }
                return false;
            }
        }


        public bool ActualizarProductoDesdeStock(int idVariante,string modelo,string nombreProducto,string descripcion, string color,string condicion,decimal costo, decimal precio, int stockDisponible)
        {
            // Prepara la actualización del producto y su variante
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
            costo = @costo,
            precio = @precio,
            stock_disponible = @stockDisponible
        WHERE id_variante = @idVariante;";

            // Crea la conexión con SQL Server
            using SqlConnection conexion =new SqlConnection(Conexion.ObtenerCadena());

            // Prepara el comando que ejecutará la actualización
            using SqlCommand comando = new SqlCommand(consulta, conexion);

            // Envía el identificador de la variante
            comando.Parameters.AddWithValue( "@idVariante",idVariante);

            // Envía el modelo
            comando.Parameters.AddWithValue( "@modelo", modelo);

            // Envía el nombre del producto
            comando.Parameters.AddWithValue("@nombreProducto", nombreProducto);

            // Envía la descripción
            comando.Parameters.AddWithValue( "@descripcion", descripcion);

            // Envía el color
            comando.Parameters.AddWithValue( "@color",color);

            // Envía la condición
            comando.Parameters.AddWithValue( "@condicion", condicion);

            // Envía el costo
            comando.Parameters.AddWithValue("@costo", costo);

            // Envía el precio
            comando.Parameters.AddWithValue("@precio", precio );

            // Envía el stock disponible
            comando.Parameters.AddWithValue("@stockDisponible", stockDisponible );

            // Abre la conexión
            conexion.Open();

            // Ejecuta las dos actualizaciones
            return comando.ExecuteNonQuery() > 0;
        }









        // Obtiene el producto completo al que pertenece una variante
        public producto? obtenerProductoPorVariante(int idVariante)
        {
            // Selección corregida: alias de columnas y unión del cálculo de stock similar a buscarProducto
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
                                    v.costo AS Costo,
                                    v.precio AS Precio,
                                    v.stock_disponible
                                        + COALESCE(stock.stock_comprometido, 0)
                                        AS StockFisico,
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
                                    SELECT
                                        dr.id_variante,
                                        SUM(dr.cantidad) AS stock_comprometido
                                    FROM Detalle_Remito AS dr
                                    INNER JOIN Remito AS r
                                        ON r.id_remito = dr.id_remito
                                    INNER JOIN Envio AS e
                                        ON e.id_remito = r.id_remito
                                    INNER JOIN Estado_Envio AS ee
                                        ON ee.id_estado_envio = e.id_estado_envio
                                    WHERE ee.nombre = 'Pendiente'
                                    GROUP BY dr.id_variante
                                ) AS stock
                                    ON stock.id_variante = v.id_variante
                                WHERE v.id_variante = @idVariante
                                    AND v.activo = 1
                                ORDER BY v.id_variante";

            // Crea la conexión con SQL Server
            using SqlConnection conexion =new SqlConnection(Conexion.ObtenerCadena());

            // Prepara la consulta
            using SqlCommand comando =new SqlCommand(consulta, conexion);

            // Envía el identificador de la variante
            comando.Parameters.Add("@idVariante",SqlDbType.Int).Value = idVariante;

            // Abre la conexión
            conexion.Open();

            // Ejecuta la consulta
            using SqlDataReader lector = comando.ExecuteReader();

            // Guarda el producto encontrado
            producto? productoEncontrado = null;

            // Recorre todas las variantes del producto
            while (lector.Read())
            {
                // Crea el producto solamente en la primera fila
                if (productoEncontrado == null)
                {
                    productoEncontrado = new producto
                    {
                        IdProducto = lector.GetInt32(lector.GetOrdinal("id_producto")),

                        NombreProducto = lector.GetString(lector.GetOrdinal("NombreProducto")),

                        Descripcion = lector.IsDBNull(lector.GetOrdinal("Descripcion"))? string.Empty: lector.GetString(lector.GetOrdinal("Descripcion")),

                        Categoria = lector.GetString(lector.GetOrdinal("nombre_categoria") ),

                        Marca = lector.GetString(lector.GetOrdinal("nombre_marca")),

                        Modelo = lector.IsDBNull(lector.GetOrdinal("Modelo")) ? string.Empty : lector.GetString(lector.GetOrdinal("Modelo"))
                    };
                }

                VarianteProducto varianteEncontrada =new VarianteProducto
                {
                    IdVariante = lector.GetInt32(lector.GetOrdinal("IdVariante")),
                    
                    Color = lector.IsDBNull(lector.GetOrdinal("Color"))? string.Empty : lector.GetString(lector.GetOrdinal("Color")),
                    
                    CondicionProducto = lector.GetString(lector.GetOrdinal("CondicionProducto")),
                    
                    Costo = lector.GetDecimal(lector.GetOrdinal("Costo")),
                    
                    Precio = lector.GetDecimal(lector.GetOrdinal("Precio")),
                    
                    StockFisico = lector.GetInt32(lector.GetOrdinal("StockFisico")),
                    
                    StockComprometido = lector.GetInt32(lector.GetOrdinal("StockComprometido"))
                };

                // Agrega la variante al producto
                productoEncontrado.Variantes.Add(varianteEncontrada);
            }

            // Devuelve el producto encontrado
            return productoEncontrado;
        }






        // Desactiva una variante sin eliminarla físicamente
        public bool desactivarVariante(int idVariante)
        {
            string consulta = @"
                               UPDATE Variante_Producto
                                       SET activo = 0
                                       WHERE id_variante = @idVariante
                                         AND activo = 1";

            // Crea la conexión
            using SqlConnection conexion =new SqlConnection(Conexion.ObtenerCadena());

            // Prepara la consulta
            using SqlCommand comando = new SqlCommand(consulta, conexion);

            // Envía el identificador
            comando.Parameters.Add("@idVariante", SqlDbType.Int).Value = idVariante;

            // Abre la conexión
            conexion.Open();

            // Devuelve true si se modificó una fila
            return comando.ExecuteNonQuery() > 0;
        }





        public List<producto> buscarProducto(string nombreProducto,int? idCategoria, int? idMarca,string condicionProducto, string modelo){
            // Guarda los productos encontrados
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
                                        v.costo AS Costo,
                                        v.precio AS Precio,
                                        
                                        --aca hace si stock.stock_comprometido tiene un valor, usalo si es NULL, usá 0.
                                        v.stock_disponible
                                            + COALESCE(stock.stock_comprometido, 0)
                                            AS StockFisico,

                                        COALESCE(stock.stock_comprometido, 0)
                                            AS StockComprometido

                                    FROM Producto AS p

                                    INNER JOIN Variante_Producto AS v
                                        ON v.id_producto = p.id_producto

                                    LEFT JOIN
                                    (
                                        SELECT
                                            dr.id_variante,
                                            SUM(dr.cantidad) AS stock_comprometido

                                        FROM Detalle_Remito AS dr

                                        INNER JOIN Remito AS r
                                            ON r.id_remito = dr.id_remito

                                        INNER JOIN Envio AS e
                                            ON e.id_remito = r.id_remito

                                        INNER JOIN Estado_Envio AS ee
                                            ON ee.id_estado_envio = e.id_estado_envio

                                        WHERE ee.nombre = 'Pendiente'

                                        GROUP BY dr.id_variante
                                    ) AS stock
                                        ON stock.id_variante = v.id_variante

                                    WHERE
                                    (
                                        p.nombre_producto LIKE @nombreProducto
                                        OR p.modelo LIKE @nombreProducto
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
                                            OR v.condicion_producto = @condicionProducto
                                        )

                                        AND v.activo = 1;";

            using SqlConnection conexion =new SqlConnection(Conexion.ObtenerCadena());

            using SqlCommand comando =new SqlCommand(consulta, conexion);

            comando.Parameters.Add( "@nombreProducto",SqlDbType.NVarChar,100 ).Value = "%" + nombreProducto.Trim() + "%";

            comando.Parameters.Add( "@idCategoria",SqlDbType.Int).Value = idCategoria.HasValue? idCategoria.Value: DBNull.Value;

            comando.Parameters.Add("@idMarca",SqlDbType.Int).Value = idMarca.HasValue ? idMarca.Value: DBNull.Value;

            comando.Parameters.Add("@condicionProducto",SqlDbType.NVarChar,20).Value = condicionProducto;

            comando.Parameters.Add("@modelo",SqlDbType.NVarChar,100).Value = string.IsNullOrWhiteSpace(modelo)? string.Empty: "%" + modelo.Trim() + "%";

            conexion.Open();

            using SqlDataReader lector =comando.ExecuteReader();

            while (lector.Read())
            {
                int idProducto = lector.GetInt32(lector.GetOrdinal("IdProducto"));

                producto? productoEncontrado = null;

                foreach (producto productoDeLaLista
                    in productosEncontrados)
                {
                    if (productoDeLaLista.IdProducto == idProducto)
                    {
                        productoEncontrado = productoDeLaLista;
                        break;
                    }
                }

                if (productoEncontrado == null)
                {
                    productoEncontrado = new producto
                    {
                        
                        IdProducto = idProducto,

                        NombreProducto = lector.GetString(lector.GetOrdinal("NombreProducto")),

                        Descripcion = lector.IsDBNull(lector.GetOrdinal("Descripcion"))? string.Empty : lector.GetString(lector.GetOrdinal("Descripcion")),

                        Modelo = lector.IsDBNull(lector.GetOrdinal("Modelo"))? string.Empty: lector.GetString(lector.GetOrdinal("Modelo"))
                    };

                    productosEncontrados.Add(productoEncontrado);
                }

                VarianteProducto varianteEncontrada =
                    new VarianteProducto
                    {
                        IdVariante = lector.GetInt32(lector.GetOrdinal("IdVariante")),
                        
                        Color = lector.IsDBNull(lector.GetOrdinal("Color"))? string.Empty : lector.GetString(lector.GetOrdinal("Color")),

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





        public bool nuevoProducto(producto producto)
        {

            {
                // Consulta que busca una marca por su nombre
                string consultaBuscarMarca = @"
                                             SELECT id_marca
                                             FROM Marca
                                             WHERE nombre_marca = @nombreMarca";

                // Consulta que crea una marca y devuelve su identificador
                string consultaCrearMarca = @"
                                             INSERT INTO Marca (nombre_marca)
                                             OUTPUT INSERTED.id_marca
                                             VALUES (@nombreMarca)";

                // Consulta que busca una categoría por su nombre
                string consultaBuscarCategoria = @"
                                             SELECT id_categoria
                                             FROM Categoria
                                             WHERE nombre_categoria = @nombreCategoria";

                // Consulta que crea una categoría y devuelve su identificador
                string consultaCrearCategoria = @"
                                             INSERT INTO Categoria (nombre_categoria)
                                             OUTPUT INSERTED.id_categoria
                                             VALUES (@nombreCategoria)";

                // Consulta que crea el producto y devuelve su identificador
                string consultaProducto = @"
                                             INSERT INTO Producto
                                                 (nombre_producto,modelo, id_marca, descripcion, id_categoria)
                                             OUTPUT INSERTED.id_producto
                                             VALUES
                                                 (@nombreProducto,@modelo, @idMarca, @descripcion, @idCategoria)";

                // Consulta que crea una variante del producto
                string consultaVariante = @"
                                             INSERT INTO Variante_Producto
                                                 (id_producto, color, condicion_producto, precio, stock_disponible)
                                             VALUES
                                                 (@idProducto, @color, @condicionProducto, @precio, @stockDisponible)";

                // Crea y abre la conexión con SQL Server
                using SqlConnection conexion = new SqlConnection(Conexion.ObtenerCadena());

                conexion.Open();

                // Inicia una transacción para guardar todo junto
                using SqlTransaction transaccion = conexion.BeginTransaction();

                try
                {
                    int idMarca;

                    // Busca la marca por su nombre
                    using (SqlCommand comandoBuscarMarca =new SqlCommand(consultaBuscarMarca, conexion, transaccion))
                    {
                        comandoBuscarMarca.Parameters.Add("@nombreMarca", SqlDbType.NVarChar, 100).Value = producto.Marca;

                        object resultadoMarca = comandoBuscarMarca.ExecuteScalar();

                        if (resultadoMarca != null)
                        {
                            // Utiliza la marca existente
                            idMarca = Convert.ToInt32(resultadoMarca);
                        }
                        else
                        {
                            // Crea la marca si todavía no existe
                            using SqlCommand comandoCrearMarca =new SqlCommand(consultaCrearMarca, conexion, transaccion);

                            comandoCrearMarca.Parameters.Add( "@nombreMarca", SqlDbType.NVarChar,100 ).Value = producto.Marca;

                            idMarca = Convert.ToInt32(comandoCrearMarca.ExecuteScalar() );
                        }
                    }

                    int idCategoria;

                    // Busca la categoría por su nombre
                    using (SqlCommand comandoBuscarCategoria =new SqlCommand(consultaBuscarCategoria, conexion, transaccion))
                    {
                        comandoBuscarCategoria.Parameters.Add( "@nombreCategoria",SqlDbType.NVarChar, 100).Value = producto.Categoria;

                        object resultadoCategoria = comandoBuscarCategoria.ExecuteScalar();

                        if (resultadoCategoria != null)
                        {
                            // Utiliza la categoría existente
                            idCategoria = Convert.ToInt32(resultadoCategoria);
                        }
                        else
                        {
                            // Crea la categoría si todavía no existe
                            using SqlCommand comandoCrearCategoria = new SqlCommand(consultaCrearCategoria,conexion,transaccion);

                            comandoCrearCategoria.Parameters.Add("@nombreCategoria", SqlDbType.NVarChar,100).Value = producto.Categoria;

                            idCategoria = Convert.ToInt32( comandoCrearCategoria.ExecuteScalar());
                        }
                    }

                    int idProducto;

                    // Crea el producto
                    using (SqlCommand comandoProducto =new SqlCommand(consultaProducto, conexion, transaccion))
                    {
                        comandoProducto.Parameters.Add( "@nombreProducto",SqlDbType.NVarChar,100).Value = producto.NombreProducto;

                        comandoProducto.Parameters.Add( "@idMarca",SqlDbType.Int).Value = idMarca;

                        comandoProducto.Parameters.Add("@descripcion",SqlDbType.NVarChar,255).Value = string.IsNullOrWhiteSpace(producto.Descripcion)? (object)DBNull.Value: producto.Descripcion;

                        comandoProducto.Parameters.Add("@idCategoria", SqlDbType.Int).Value = idCategoria;

                        comandoProducto.Parameters.Add("@modelo",SqlDbType.NVarChar,50).Value = producto.Modelo;
                        
                        idProducto = Convert.ToInt32(comandoProducto.ExecuteScalar());
                    }

                    // Recorre todas las variantes agregadas al producto
                    foreach (VarianteProducto variante in producto.Variantes)
                    {
                        using SqlCommand comandoVariante =new SqlCommand(consultaVariante, conexion, transaccion);

                        comandoVariante.Parameters.Add(  "@idProducto", SqlDbType.Int).Value = idProducto;

                        // Guarda NULL cuando la variante no posee color
                        comandoVariante.Parameters.Add(  "@color",SqlDbType.NVarChar,50).Value = string.IsNullOrWhiteSpace(variante.Color)? DBNull.Value : variante.Color;

                        comandoVariante.Parameters.Add( "@condicionProducto", SqlDbType.NVarChar, 50 ).Value = variante.CondicionProducto;

                        SqlParameter parametroPrecio =comandoVariante.Parameters.Add( "@precio",SqlDbType.Decimal );

                        parametroPrecio.Precision = 18;
                        parametroPrecio.Scale = 2;
                        parametroPrecio.Value = variante.Precio;

                        comandoVariante.Parameters.Add("@stockDisponible", SqlDbType.Int).Value = 0;

                        // Guarda la variante actual
                        comandoVariante.ExecuteNonQuery();
                    }

                    // Confirma el producto y todas sus variantes
                    transaccion.Commit();

                    return true;
                }
                catch
                {
                    // Deshace todos los cambios si falla alguna operación
                    transaccion.Rollback();

                    return false;
                }
            }

        }
    }
}

    