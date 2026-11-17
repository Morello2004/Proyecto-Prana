using Microsoft.Data.SqlClient;
using Prana.Entidades;
using System.Data;

namespace Prana.Datos
{
    // agrupa los datos y operaciones de cd informe ventas
    public class CD_InformeVentas
    {
        // consulta obtener opciones y transforma cada fila recibida en los objetos que devuelve
        public Dictionary<string, List<string>> ObtenerOpciones()
        {
            // combina localidades de clientes y sugerencias de entrega para los filtros
            const string consultaSql = @"
SELECT DISTINCT pr.nombre_producto FROM Producto pr ORDER BY pr.nombre_producto;
SELECT DISTINCT nombre_categoria FROM Categoria ORDER BY nombre_categoria;
SELECT DISTINCT nombre_marca FROM Marca ORDER BY nombre_marca;
SELECT DISTINCT CONCAT(nombre_cliente, N' ', apellido_cliente) FROM Cliente ORDER BY CONCAT(nombre_cliente, N' ', apellido_cliente);
SELECT DISTINCT valor FROM (
    SELECT tipo_logistica valor FROM Venta_Datos_Entrega WHERE NULLIF(tipo_logistica, N'') IS NOT NULL
    UNION
    SELECT CASE WHEN entregado=1 THEN N'Entregado' ELSE N'Pendiente' END FROM Envio
) envios ORDER BY valor;
SELECT l.nombre_localidad AS localidad FROM Cliente c JOIN Localidad l ON l.id_localidad=c.id_localidad
UNION SELECT localidad_sugerida FROM Venta_Datos_Entrega WHERE localidad_sugerida IS NOT NULL ORDER BY localidad;";
            using SqlConnection conexion = new(Conexion.ObtenerCadena());
            using SqlCommand comando = new(consultaSql, conexion);
            conexion.Open();
            using SqlDataReader lector = comando.ExecuteReader();
            Dictionary<string, List<string>> resultado = new Dictionary<string, List<string>>();
            // procesa cada nombre de new[] { "producto", "categoria", "marca", "cliente", "envio", "ubicacion" } sin cambiar el orden de la coleccion
            foreach (string nombre in new[]
            {
                "Producto",
                "Categoria",
                "Marca",
                "Cliente",
                "Envio",
                "Ubicacion"
            }

            )
            {
                List<string> valores = new List<string>();
                while (lector.Read())
                {
                    string valorSeleccionado;
                    if (lector.IsDBNull(0))
                    {
                        valorSeleccionado = string.Empty;
                    }
                    else
                    {
                        valorSeleccionado = lector.GetString(0);
                    }
                    valores.Add(valorSeleccionado);
                }

                resultado[nombre] = valores;
                if (nombre != "Ubicacion")
                {
                    lector.NextResult();
                }
            }

            return resultado;
        }

        // consulta obtener categoria marca producto y transforma cada fila recibida en los objetos que devuelve
        public (string Categoria, string Marca)? ObtenerCategoriaMarcaProducto(string nombreProducto)
        {
            const string consultaSql = @"
SELECT DISTINCT c.nombre_categoria, m.nombre_marca
FROM Producto p
JOIN Categoria c ON c.id_categoria = p.id_categoria
JOIN Marca m ON m.id_marca = p.id_marca
WHERE LTRIM(RTRIM(p.nombre_producto)) = @producto
ORDER BY c.nombre_categoria, m.nombre_marca;";
            using SqlConnection conexion = new(Conexion.ObtenerCadena());
            using SqlCommand comando = new(consultaSql, conexion);
            comando.Parameters.Add("@producto", SqlDbType.NVarChar, 150).Value = nombreProducto.Trim();
            conexion.Open();
            using SqlDataReader lector = comando.ExecuteReader();
            if (!lector.Read())
            {
                return null;
            }

            (string Categoria, string Marca) resultado = (Categoria: lector.GetString(0), Marca: lector.GetString(1));
            if (lector.Read())
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new InvalidOperationException("Hay productos con el mismo nombre y distintas categorías o marcas; no se pueden completar esos filtros automáticamente.");
            }

            return resultado;
        }

        // consulta buscar y transforma cada fila recibida en los objetos que devuelve
        public List<InformeVenta> Buscar(FiltroInformeVentas filtro)
        {
            const string consultaSql = @"
SELECT v.id_venta, v.fecha_hora_venta,
       CONCAT(c.nombre_cliente, N' ', c.apellido_cliente) AS cliente,
       COALESCE(ve.localidad_sugerida, N'') AS ubicacion,
       COALESCE(productos.texto, N'') AS productos,
       COALESCE(categorias.texto, N'') AS categorias,
       COALESCE(marcas.texto, N'') AS marcas,
              v.total, COALESCE(pagado.monto, 0) AS pagado,
              CASE WHEN v.total > COALESCE(pagado.monto, 0) THEN v.total - COALESCE(pagado.monto, 0) ELSE 0 END AS saldo,
              COALESCE(envio.estado, COALESCE(ve.tipo_logistica, N'Sin envío')) AS envio
       FROM Venta v
       JOIN Cliente c ON c.id_cliente = v.id_cliente
       LEFT JOIN Venta_Datos_Entrega ve ON ve.id_venta = v.id_venta
OUTER APPLY (
    SELECT STRING_AGG(CAST(CONCAT(d.cantidad, N' x ', pr.nombre_producto, N' (', vp.color, N')') AS nvarchar(max)), N'; ') texto
    FROM Detalle_Venta d JOIN Variante_Producto vp ON vp.id_variante=d.id_variante
    JOIN Producto pr ON pr.id_producto=vp.id_producto WHERE d.id_venta=v.id_venta
) productos
OUTER APPLY (
    SELECT STRING_AGG(CAST(cat.nombre_categoria AS nvarchar(max)), N', ') texto
    FROM (SELECT DISTINCT pr.id_categoria FROM Detalle_Venta d JOIN Variante_Producto vp ON vp.id_variante=d.id_variante JOIN Producto pr ON pr.id_producto=vp.id_producto WHERE d.id_venta=v.id_venta) x
    JOIN Categoria cat ON cat.id_categoria=x.id_categoria
) categorias
OUTER APPLY (
    SELECT STRING_AGG(CAST(m.nombre_marca AS nvarchar(max)), N', ') texto
    FROM (SELECT DISTINCT pr.id_marca FROM Detalle_Venta d JOIN Variante_Producto vp ON vp.id_variante=d.id_variante JOIN Producto pr ON pr.id_producto=vp.id_producto WHERE d.id_venta=v.id_venta) x
    JOIN Marca m ON m.id_marca=x.id_marca
) marcas
OUTER APPLY (SELECT SUM(importe_cobrado) monto FROM Pago WHERE id_venta=v.id_venta) pagado
 OUTER APPLY (
     SELECT TOP (1) CASE WHEN e.entregado=1 THEN N'Entregado' ELSE N'Pendiente' END estado
     FROM Remito r JOIN Envio e ON e.id_remito=r.id_remito
    WHERE r.id_venta=v.id_venta ORDER BY e.id_envio DESC
 ) envio
WHERE v.fecha_hora_venta >= @Desde AND v.fecha_hora_venta < @Hasta
  AND (@Producto = N'' OR EXISTS (SELECT 1 FROM Detalle_Venta d JOIN Variante_Producto vp ON vp.id_variante=d.id_variante JOIN Producto pr ON pr.id_producto=vp.id_producto WHERE d.id_venta=v.id_venta AND pr.nombre_producto COLLATE Latin1_General_CI_AI LIKE @Producto))
  AND (@Categoria = N'' OR categorias.texto COLLATE Latin1_General_CI_AI LIKE @Categoria)
  AND (@Marca = N'' OR marcas.texto COLLATE Latin1_General_CI_AI LIKE @Marca)
  AND (@Cliente = N'' OR CONCAT(c.nombre_cliente, N' ', c.apellido_cliente, N' ', c.numero_documento) COLLATE Latin1_General_CI_AI LIKE @Cliente)
  AND (@Envio = N'' OR CONCAT(COALESCE(envio.estado, N''), N' ', COALESCE(ve.tipo_logistica, N'')) COLLATE Latin1_General_CI_AI LIKE @Envio)
  AND (@Ubicacion = N'' OR CONCAT(COALESCE(ve.localidad_sugerida, N''), N' ', COALESCE(ve.direccion_sugerida, N'')) COLLATE Latin1_General_CI_AI LIKE @Ubicacion)
  AND (@MontoMinimo IS NULL OR v.total >= @MontoMinimo)
  AND (@MontoMaximo IS NULL OR v.total <= @MontoMaximo)
ORDER BY v.fecha_hora_venta DESC, v.id_venta DESC;";
            using SqlConnection conexion = new(Conexion.ObtenerCadena());
            using SqlCommand comando = new(consultaSql, conexion);
            comando.Parameters.Add("@Desde", SqlDbType.DateTime2).Value = filtro.Desde;
            comando.Parameters.Add("@Hasta", SqlDbType.DateTime2).Value = filtro.Hasta;
            AgregarTexto(comando, "@Producto", filtro.Producto);
            AgregarTexto(comando, "@Categoria", filtro.Categoria);
            AgregarTexto(comando, "@Marca", filtro.Marca);
            AgregarTexto(comando, "@Cliente", filtro.Cliente);
            AgregarTexto(comando, "@Envio", filtro.Envio);
            AgregarTexto(comando, "@Ubicacion", filtro.Ubicacion);
            comando.Parameters.Add("@MontoMinimo", SqlDbType.Decimal).Value = (object?)filtro.MontoMinimo ?? DBNull.Value;
            comando.Parameters.Add("@MontoMaximo", SqlDbType.Decimal).Value = (object?)filtro.MontoMaximo ?? DBNull.Value;
            conexion.Open();
            using SqlDataReader lector = comando.ExecuteReader();
            List<InformeVenta> resultado = new();
            while (lector.Read())
            {
                resultado.Add(new InformeVenta
                {
                    IdVenta = (int)lector["id_venta"],
                    Fecha = (DateTime)lector["fecha_hora_venta"],
                    Cliente = (string)lector["cliente"],
                    Ubicacion = (string)lector["ubicacion"],
                    Productos = (string)lector["productos"],
                    Categorias = (string)lector["categorias"],
                    Marcas = (string)lector["marcas"],
                    Total = (decimal)lector["total"],
                    Pagado = (decimal)lector["pagado"],
                    Saldo = (decimal)lector["saldo"],
                    Envio = (string)lector["envio"]

                });
            }

            return resultado;
        }

        // resuelve agregar texto con los datos recibidos y entrega el resultado al codigo que lo llama
        private static void AgregarTexto(SqlCommand comando, string nombre, string valor)
        {
            // distingue un dato vacio de un valor ingresado antes de continuar
            if (string.IsNullOrWhiteSpace(valor))
            {
                comando.Parameters.Add(nombre, SqlDbType.NVarChar, -1).Value = "";
            }
            else
            {
                comando.Parameters.Add(nombre, SqlDbType.NVarChar, -1).Value = "%" + valor.Trim() + "%";
            }
        }

        // consulta obtener alertas y transforma cada fila recibida en los objetos que devuelve
        public List<AlertaPagoPendiente> ObtenerAlertas(DateTime ahora)
        {
            const string consultaSql = @"
WITH Pagos AS
(
    SELECT id_venta, MIN(fecha_hora_pago) primer_pago, SUM(importe_cobrado) monto
    FROM Pago
    GROUP BY id_venta
),
Productos AS
(
    SELECT d.id_venta,
           STRING_AGG(CAST(CONCAT(d.cantidad, N' x ', pr.nombre_producto) AS nvarchar(max)), N'; ') texto
    FROM Detalle_Venta d
    JOIN Variante_Producto vp ON vp.id_variante=d.id_variante
    JOIN Producto pr ON pr.id_producto=vp.id_producto
    GROUP BY d.id_venta
)
SELECT v.id_venta, CONCAT(c.nombre_cliente, N' ', c.apellido_cliente) cliente,
       COALESCE(productos.texto, N'') productos, pagos.primer_pago, v.total,
       pagos.monto pagado, v.total - pagos.monto saldo,
       DATEDIFF(DAY, DATEADD(MONTH, 1, pagos.primer_pago), @Ahora) dias_vencida
FROM Venta v JOIN Cliente c ON c.id_cliente=v.id_cliente
JOIN Pagos pagos ON pagos.id_venta=v.id_venta
LEFT JOIN Productos productos ON productos.id_venta=v.id_venta
WHERE DATEADD(MONTH, 1, pagos.primer_pago) <= @Ahora AND v.total > pagos.monto
  AND NOT EXISTS (SELECT 1 FROM Estado_Venta ev WHERE ev.id_estado_venta=v.id_estado_venta AND (ev.nombre_estado_venta LIKE N'Anulad%' OR ev.nombre_estado_venta LIKE N'Cancelad%'))
ORDER BY dias_vencida DESC, pagos.primer_pago;";
            using SqlConnection conexion = new(Conexion.ObtenerCadena());
            using SqlCommand comando = new(consultaSql, conexion);
            comando.Parameters.Add("@Ahora", SqlDbType.DateTime2).Value = ahora;
            conexion.Open();
            using SqlDataReader lector = comando.ExecuteReader();
            List<AlertaPagoPendiente> resultado = new();
            while (lector.Read())
            {
                resultado.Add(new AlertaPagoPendiente
                {
                    IdVenta = (int)lector["id_venta"],
                    Cliente = (string)lector["cliente"],
                    Productos = (string)lector["productos"],
                    PrimerPago = (DateTime)lector["primer_pago"],
                    Total = (decimal)lector["total"],
                    Pagado = (decimal)lector["pagado"],
                    Saldo = (decimal)lector["saldo"],
                    DiasVencida = (int)lector["dias_vencida"]

                });
            }

            return resultado;
        }
    }
}
