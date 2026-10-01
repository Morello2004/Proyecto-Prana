using Microsoft.Data.SqlClient;
using Prana.Entidades;
using System.Data;

namespace Prana.Datos
{
    public class CD_InformeVentas
    {
        public Dictionary<string, List<string>> ObtenerOpciones()
        {
            const string sql = @"
SELECT DISTINCT pr.nombre_producto FROM Producto pr ORDER BY pr.nombre_producto;
SELECT DISTINCT nombre_categoria FROM Categoria ORDER BY nombre_categoria;
SELECT DISTINCT nombre_marca FROM Marca ORDER BY nombre_marca;
SELECT DISTINCT CONCAT(nombre_cliente, N' ', apellido_cliente) FROM Cliente ORDER BY CONCAT(nombre_cliente, N' ', apellido_cliente);
SELECT DISTINCT valor FROM (
    SELECT tipo_logistica valor FROM Venta_Entrega WHERE NULLIF(tipo_logistica, N'') IS NOT NULL
    UNION
    SELECT nombre FROM Estado_Envio
) envios ORDER BY valor;
SELECT DISTINCT nombre_localidad FROM Localidad ORDER BY nombre_localidad;";
            using SqlConnection conexion = new(Conexion.ObtenerCadena());
            using SqlCommand comando = new(sql, conexion);
            conexion.Open();
            using SqlDataReader lector = comando.ExecuteReader();
            var resultado = new Dictionary<string, List<string>>();
            foreach (string nombre in new[] { "Producto", "Categoria", "Marca", "Cliente", "Envio", "Ubicacion" })
            {
                var valores = new List<string>();
                while (lector.Read()) valores.Add(lector.IsDBNull(0) ? string.Empty : lector.GetString(0));
                resultado[nombre] = valores;
                if (nombre != "Ubicacion") lector.NextResult();
            }
            return resultado;
        }

        public List<InformeVenta> Buscar(FiltroInformeVentas filtro)
        {
            const string sql = @"
SELECT v.id_venta, v.fecha_venta,
       CONCAT(c.nombre_cliente, N' ', c.apellido_cliente) AS cliente,
       COALESCE(loc.nombre_localidad, N'') AS ubicacion,
       COALESCE(productos.texto, N'') AS productos,
       COALESCE(categorias.texto, N'') AS categorias,
       COALESCE(marcas.texto, N'') AS marcas,
       v.total, COALESCE(pagado.monto, 0) AS pagado,
       CASE WHEN v.total > COALESCE(pagado.monto, 0) THEN v.total - COALESCE(pagado.monto, 0) ELSE 0 END AS saldo,
       COALESCE(envio.estado, COALESCE(ve.tipo_logistica, N'Sin envío')) AS envio
FROM Venta v
JOIN Cliente c ON c.id_cliente = v.id_cliente
LEFT JOIN Localidad loc ON loc.id_localidad = c.id_localidad
LEFT JOIN Venta_Entrega ve ON ve.id_venta = v.id_venta
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
OUTER APPLY (SELECT SUM(monto_final) monto FROM Pago WHERE id_venta=v.id_venta) pagado
OUTER APPLY (
    SELECT TOP (1) ee.nombre estado
    FROM Remito r JOIN Envio e ON e.id_remito=r.id_remito JOIN Estado_Envio ee ON ee.id_estado_envio=e.id_estado_envio
    WHERE r.id_venta=v.id_venta ORDER BY e.id_envio DESC
) envio
WHERE v.fecha_venta >= @Desde AND v.fecha_venta < @Hasta
  AND (@Producto = N'' OR EXISTS (SELECT 1 FROM Detalle_Venta d JOIN Variante_Producto vp ON vp.id_variante=d.id_variante JOIN Producto pr ON pr.id_producto=vp.id_producto WHERE d.id_venta=v.id_venta AND pr.nombre_producto COLLATE Latin1_General_CI_AI LIKE @Producto))
  AND (@Categoria = N'' OR categorias.texto COLLATE Latin1_General_CI_AI LIKE @Categoria)
  AND (@Marca = N'' OR marcas.texto COLLATE Latin1_General_CI_AI LIKE @Marca)
  AND (@Cliente = N'' OR CONCAT(c.nombre_cliente, N' ', c.apellido_cliente, N' ', c.dni) COLLATE Latin1_General_CI_AI LIKE @Cliente)
  AND (@Envio = N'' OR CONCAT(COALESCE(envio.estado, N''), N' ', COALESCE(ve.tipo_logistica, N'')) COLLATE Latin1_General_CI_AI LIKE @Envio)
  AND (@Ubicacion = N'' OR CONCAT(COALESCE(loc.nombre_localidad, N''), N' ', COALESCE(ve.domicilio, N'')) COLLATE Latin1_General_CI_AI LIKE @Ubicacion)
  AND (@MontoMinimo IS NULL OR v.total >= @MontoMinimo)
  AND (@MontoMaximo IS NULL OR v.total <= @MontoMaximo)
ORDER BY v.fecha_venta DESC, v.id_venta DESC;";

            using SqlConnection conexion = new(Conexion.ObtenerCadena());
            using SqlCommand comando = new(sql, conexion);
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
            while (lector.Read()) resultado.Add(new InformeVenta
            {
                IdVenta=(int)lector["id_venta"], Fecha=(DateTime)lector["fecha_venta"], Cliente=(string)lector["cliente"],
                Ubicacion=(string)lector["ubicacion"], Productos=(string)lector["productos"], Categorias=(string)lector["categorias"],
                Marcas=(string)lector["marcas"], Total=(decimal)lector["total"], Pagado=(decimal)lector["pagado"],
                Saldo=(decimal)lector["saldo"], Envio=(string)lector["envio"]
            });
            return resultado;
        }

        private static void AgregarTexto(SqlCommand comando, string nombre, string valor)
        {
            comando.Parameters.Add(nombre, SqlDbType.NVarChar, -1).Value = string.IsNullOrWhiteSpace(valor) ? "" : "%" + valor.Trim() + "%";
        }

        public List<AlertaPagoPendiente> ObtenerAlertas(DateTime ahora)
        {
            const string sql = @"
SELECT v.id_venta, CONCAT(c.nombre_cliente, N' ', c.apellido_cliente) cliente,
       COALESCE(productos.texto, N'') productos, primero.fecha_pago primer_pago, v.total,
       pagos.monto pagado, v.total - pagos.monto saldo,
       DATEDIFF(DAY, DATEADD(MONTH, 1, primero.fecha_pago), @Ahora) dias_vencida
FROM Venta v JOIN Cliente c ON c.id_cliente=v.id_cliente
CROSS APPLY (SELECT MIN(fecha_pago) fecha_pago FROM Pago WHERE id_venta=v.id_venta) primero
CROSS APPLY (SELECT SUM(monto_final) monto FROM Pago WHERE id_venta=v.id_venta) pagos
OUTER APPLY (SELECT STRING_AGG(CAST(CONCAT(d.cantidad, N' x ', pr.nombre_producto) AS nvarchar(max)), N'; ') texto
    FROM Detalle_Venta d JOIN Variante_Producto vp ON vp.id_variante=d.id_variante JOIN Producto pr ON pr.id_producto=vp.id_producto
    WHERE d.id_venta=v.id_venta) productos
WHERE DATEADD(MONTH, 1, primero.fecha_pago) <= @Ahora AND v.total > pagos.monto
  AND NOT EXISTS (SELECT 1 FROM Estado_Venta ev WHERE ev.id_estado_venta=v.id_estado_venta AND (ev.nombre_estado_venta LIKE N'Anulad%' OR ev.nombre_estado_venta LIKE N'Cancelad%'))
ORDER BY dias_vencida DESC, primero.fecha_pago;";
            using SqlConnection conexion = new(Conexion.ObtenerCadena());
            using SqlCommand comando = new(sql, conexion);
            comando.Parameters.Add("@Ahora", SqlDbType.DateTime2).Value = ahora;
            conexion.Open();
            using SqlDataReader lector = comando.ExecuteReader();
            List<AlertaPagoPendiente> resultado = new();
            while (lector.Read()) resultado.Add(new AlertaPagoPendiente
            {
                IdVenta=(int)lector["id_venta"], Cliente=(string)lector["cliente"], Productos=(string)lector["productos"],
                PrimerPago=(DateTime)lector["primer_pago"], Total=(decimal)lector["total"], Pagado=(decimal)lector["pagado"],
                Saldo=(decimal)lector["saldo"], DiasVencida=(int)lector["dias_vencida"]
            });
            return resultado;
        }
    }
}
