using Microsoft.Data.SqlClient;
using System.Data;

namespace Prana.Datos
{
    // Utilidades internas: todas las escrituras comparten conexión y transacción.
    internal static class GestionVentaSql
    {
        internal static SqlCommand Comando(SqlConnection cn, SqlTransaction tx, string sql, params (string nombre, object valor)[] parametros)
        {
            var cmd = new SqlCommand(sql, cn, tx);
            foreach (var p in parametros) cmd.Parameters.AddWithValue(p.nombre, p.valor);
            return cmd;
        }
        internal static int Catalogo(SqlConnection cn, SqlTransaction tx, string tabla, string id, string nombre, string valor)
        {
            // Los identificadores son constantes internas; el valor siempre es un parámetro.
            using var cmd = Comando(cn, tx, $"SELECT {id} FROM {tabla} WITH (UPDLOCK,HOLDLOCK) WHERE {nombre}=@valor", ("@valor", valor));
            object? encontrado = cmd.ExecuteScalar();
            if (encontrado != null) return Convert.ToInt32(encontrado);
            cmd.CommandText = $"INSERT INTO {tabla} ({nombre}) OUTPUT INSERTED.{id} VALUES (@valor)";
            return Convert.ToInt32(cmd.ExecuteScalar());
        }
        internal static void ValidarVenta(SqlConnection cn, SqlTransaction tx, int venta)
        {
            // Bloquea el encabezado antes de leer pagos o remitos para evitar duplicados concurrentes.
            using var cmd = Comando(cn, tx, @"SELECT ev.nombre_estado_venta FROM Venta v WITH (UPDLOCK,HOLDLOCK)
                JOIN Estado_Venta ev ON ev.id_estado_venta=v.id_estado_venta WHERE v.id_venta=@venta", ("@venta", venta));
            string? estado = cmd.ExecuteScalar()?.ToString();
            if (estado == null || estado.StartsWith("Anulad", StringComparison.OrdinalIgnoreCase) || estado.StartsWith("Cancelad", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("La venta no existe o está anulada.");
        }
        internal static void RegistrarActividad(SqlConnection cn, SqlTransaction tx, int usuario, string accion, int venta, string detalle)
        {
            using var cmd = Comando(cn, tx, @"IF OBJECT_ID('dbo.LogActividad') IS NOT NULL
                INSERT INTO LogActividad(id_usuario,accion,entidad,id_entidad,detalle)
                VALUES(@usuario,@accion,'Venta',@venta,@detalle)", ("@usuario", usuario), ("@accion", accion), ("@venta", venta), ("@detalle", detalle));
            cmd.ExecuteNonQuery();
        }
    }
}
