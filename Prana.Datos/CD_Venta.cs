using Microsoft.Data.SqlClient;
using Prana.Entidades;
using System.Data;

namespace Prana.Datos
{
    // agrupa los datos y operaciones de cd venta
    public class CD_Venta
    {
        // suma los pagos antes de unir detalles para no multiplicar los importes total incluye recargos: se descuenta el importe cobrado, tambien con recargos
        internal const string SaldosSql = @"
            SELECT v.id_venta, v.id_cliente, v.fecha_hora_venta, v.total,
                   ev.nombre_estado_venta,
                   COALESCE(p.pagado, 0) AS pagado,
                   CASE WHEN ev.nombre_estado_venta LIKE N'Anulad%'
                              OR ev.nombre_estado_venta LIKE N'Cancelad%'
                              OR v.total <= COALESCE(p.pagado, 0) THEN 0
                        ELSE v.total - COALESCE(p.pagado, 0) END AS saldo
            FROM Venta v
            INNER JOIN Estado_Venta ev ON ev.id_estado_venta = v.id_estado_venta
            LEFT JOIN (SELECT id_venta, SUM(importe_cobrado) AS pagado FROM Pago GROUP BY id_venta) p
                ON p.id_venta = v.id_venta";
        // resuelve top10 ventas con los datos recibidos y entrega el resultado al codigo que lo llama
        public static List<venta> Top10Ventas() => Buscar(null, null, true);
        // los filtros se aplican en sql antes del limite de ultimas ventas
        public static List<venta> Buscar(string? busqueda, int? idCliente = null, bool ultimasDiez = false)
        {
            using SqlConnection conexion = new(Conexion.ObtenerCadena());
            using SqlCommand comando = conexion.CreateCommand();
            string limiteResultados;
            if (ultimasDiez)
            {
                limiteResultados = "TOP (10) ";
            }
            else
            {
                limiteResultados = "";
            }
            // consolida cantidades por variante para reconocer entregas parciales sin duplicar productos
            string consulta = "SELECT " + (limiteResultados) + @"
                v.*, c.nombre_cliente, c.apellido_cliente, c.numero_documento, entrega.estado_entrega,
                COALESCE((SELECT STRING_AGG(CAST(CONCAT(d.cantidad, N' × ', pr.nombre_producto,
                    N' (', vp.color, N', ', vp.condicion_producto, N')') AS nvarchar(max)), N'; ')
                    FROM Detalle_Venta d
                    INNER JOIN Variante_Producto vp ON vp.id_variante = d.id_variante
                    INNER JOIN Producto pr ON pr.id_producto = vp.id_producto
                    WHERE d.id_venta = v.id_venta), N'') AS productos
                FROM (" + SaldosSql + @") v
                INNER JOIN Cliente c ON c.id_cliente = v.id_cliente
                OUTER APPLY (
                    SELECT SUM(d.cantidad) vendidos,
                        SUM(CASE WHEN COALESCE(r.entregados,0)>d.cantidad THEN d.cantidad ELSE COALESCE(r.entregados,0) END) entregados,
                        SUM(CASE WHEN COALESCE(r.preparados,0)>d.cantidad THEN d.cantidad ELSE COALESCE(r.preparados,0) END) preparados
                    FROM (SELECT id_variante,SUM(cantidad) cantidad FROM Detalle_Venta
                        WHERE id_venta=v.id_venta GROUP BY id_variante) d
                    OUTER APPLY (
                        SELECT SUM(dr.cantidad) preparados,
                            SUM(CASE WHEN e.entregado=1 THEN dr.cantidad ELSE 0 END) entregados
                        FROM Remito rem JOIN Detalle_Remito dr ON dr.id_remito=rem.id_remito
                        JOIN (SELECT id_remito, MAX(CONVERT(int, entregado)) entregado FROM Envio GROUP BY id_remito) e ON e.id_remito=rem.id_remito
                        WHERE rem.id_venta=v.id_venta AND dr.id_variante=d.id_variante
                    ) r
                ) cantidades
                CROSS APPLY (SELECT CASE
                    WHEN v.nombre_estado_venta LIKE N'Anulad%' OR v.nombre_estado_venta LIKE N'Cancelad%' THEN N'Cancelada'
                    WHEN cantidades.vendidos>0 AND cantidades.entregados>=cantidades.vendidos THEN N'Entregada'
                    WHEN cantidades.entregados>0 THEN N'Entrega parcial'
                    WHEN cantidades.vendidos>0 AND cantidades.preparados>=cantidades.vendidos THEN N'Preparada'
                    WHEN cantidades.preparados>0 THEN N'Preparación parcial'
                    ELSE N'Pendiente' END estado_entrega) entrega
                WHERE 1 = 1";
            // comprueba si se recibio el valor opcional antes de utilizarlo
            if (idCliente.HasValue)
            {
                consulta += " AND v.id_cliente = @cliente AND v.saldo > 0";
                comando.Parameters.Add("@cliente", SqlDbType.Int).Value = idCliente.Value;
            }

            string[] palabras = (busqueda ?? "").Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < palabras.Length; i++)
            {
                string parametro = "@texto" + i;
                consulta += $" AND CONCAT(v.id_venta, ' ', c.nombre_cliente, ' ', c.apellido_cliente, ' ', c.numero_documento, ' ', v.nombre_estado_venta, ' ', entrega.estado_entrega) COLLATE Latin1_General_CI_AI LIKE {parametro} ESCAPE '~'";
                comando.Parameters.Add(parametro, SqlDbType.NVarChar, -1).Value = "%" + EscaparBusqueda(palabras[i]) + "%";
            }

            comando.CommandText = consulta + " ORDER BY v.fecha_hora_venta DESC, v.id_venta DESC";
            conexion.Open();
            using SqlDataReader lector = comando.ExecuteReader();
            List<venta> ventas = new();
            while (lector.Read())
            {
                ventas.Add(new venta
                {
                    IdVenta = (int)lector["id_venta"],
                    Fecha = (DateTime)lector["fecha_hora_venta"],
                    Total = (decimal)lector["total"],
                    Estado = (string)lector["nombre_estado_venta"],
                    Pagado = (decimal)lector["pagado"],
                    SaldoPendiente = (decimal)lector["saldo"],
                    Productos = (string)lector["productos"],
                    EstadoEntrega = (string)lector["estado_entrega"],
                    Cliente = new Cliente
                    {
                        IdCliente = (int)lector["id_cliente"],
                        Nombre = (string)lector["nombre_cliente"],
                        Apellido = (string)lector["apellido_cliente"],
                        DNI = (string)lector["numero_documento"]

                    }

                });
            }

            return ventas;
        }

        // trata los comodines escritos por el usuario como texto literal
        internal static string EscaparBusqueda(string texto) => texto.Replace("~", "~~").Replace("%", "~%").Replace("_", "~_").Replace("[", "~[");
        // consulta obtener resumen y transforma cada fila recibida en los objetos que devuelve
        public static ResumenVentas ObtenerResumen(DateTime dia)
        {
            using SqlConnection conexion = new(Conexion.ObtenerCadena());
            using SqlCommand comando = new(@"
                SELECT COALESCE(SUM(CASE WHEN fecha_hora_venta >= @desde AND fecha_hora_venta < @hasta THEN total ELSE 0 END), 0) AS total_hoy,
                    COUNT(CASE WHEN fecha_hora_venta >= @desde AND fecha_hora_venta < @hasta THEN 1 END) AS cantidad_hoy,
                    COALESCE(SUM(saldo), 0) AS saldo,
                    COUNT(CASE WHEN saldo > 0 THEN 1 END) AS pendientes
                FROM (" + SaldosSql + @") v
                WHERE nombre_estado_venta NOT LIKE N'Anulad%' AND nombre_estado_venta NOT LIKE N'Cancelad%'", conexion);
            comando.Parameters.Add("@desde", SqlDbType.DateTime2).Value = dia.Date;
            comando.Parameters.Add("@hasta", SqlDbType.DateTime2).Value = dia.Date.AddDays(1);
            conexion.Open();
            using SqlDataReader lector = comando.ExecuteReader();
            lector.Read();
            return new ResumenVentas
            {
                TotalHoy = (decimal)lector["total_hoy"],
                CantidadHoy = (int)lector["cantidad_hoy"],
                SaldoPendiente = (decimal)lector["saldo"],
                VentasPendientes = (int)lector["pendientes"]

            };
        }
    }
}
