using Microsoft.Data.SqlClient;
using System.Data;

namespace Prana.Datos
{
    // utilidades internas: todas las escrituras comparten conexion y transaccion
    internal static class GestionVentaSql
    {
        // resuelve comando con los datos recibidos y entrega el resultado al codigo que lo llama
        internal static SqlCommand Comando(SqlConnection cn, SqlTransaction tx, string sql, params (string nombre, object? valor)[] parametros)
        {
            // prepara la consulta y su conexion los valores variables se envian mediante parametros
            SqlCommand comando = new SqlCommand(sql, cn, tx);
            // procesa cada p de parametros sin cambiar el orden de la coleccion
            foreach ((string nombre, object? valor) p in parametros)
            {
                // sql necesita dbnull para valores opcionales como destino de cierre automatico o filtro de caja
                comando.Parameters.AddWithValue(p.nombre, p.valor ?? DBNull.Value);
            }

            return comando;
        }

        // ejecuta la consulta de obtener ocrear id por nombre y obtiene el primer valor del resultado
        internal static int ObtenerOCrearIdPorNombre(SqlConnection cn, SqlTransaction tx, string tabla, string id, string nombre, string valor)
        {
            // los identificadores son constantes internas; el valor siempre es un parametro los bloqueos protegen la busqueda mientras otra operacion intenta crear el mismo nombre
            using SqlCommand comando = Comando(cn, tx, $"SELECT {id} FROM {tabla} WITH (UPDLOCK,HOLDLOCK) WHERE {nombre}=@valor", ("@valor", valor));
            object? encontrado = comando.ExecuteScalar();
            if (encontrado != null)
            {
                return Convert.ToInt32(encontrado);
            }

            comando.CommandText = $"INSERT INTO {tabla} ({nombre}) OUTPUT INSERTED.{id} VALUES (@valor)";
            return Convert.ToInt32(comando.ExecuteScalar());
        }

        // comprueba validar venta; si no se cumple la regla interrumpe con un mensaje antes de continuar
        internal static void ValidarVenta(SqlConnection cn, SqlTransaction tx, int venta)
        {
            // bloquea el encabezado antes de leer pagos o remitos para evitar duplicados concurrentes
            using SqlCommand comando = Comando(cn, tx, @"SELECT ev.nombre_estado_venta FROM Venta v WITH (UPDLOCK,HOLDLOCK)
                JOIN Estado_Venta ev ON ev.id_estado_venta=v.id_estado_venta WHERE v.id_venta=@venta", ("@venta", venta));
            // el operador de acceso condicional evita convertir a texto un resultado nulo
            string? estado = comando.ExecuteScalar()?.ToString();
            if (estado == null || estado.StartsWith("Anulad", StringComparison.OrdinalIgnoreCase) || estado.StartsWith("Cancelad", StringComparison.OrdinalIgnoreCase))
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new InvalidOperationException("La venta no existe o está anulada.");
            }
        }

        // envia los parametros de registrar actividad y ejecuta la escritura en la base de datos
        internal static void RegistrarActividad(SqlConnection cn, SqlTransaction tx, int usuario, string accion, int venta, string detalle)
        {
            using SqlCommand comando = Comando(cn, tx, @"IF OBJECT_ID('dbo.LogActividad') IS NOT NULL
                INSERT INTO LogActividad(id_usuario,accion,entidad,id_entidad,detalle)
                VALUES(@usuario,@accion,'Venta',@venta,@detalle)", ("@usuario", usuario), ("@accion", accion), ("@venta", venta), ("@detalle", detalle));
            comando.ExecuteNonQuery();
        }
    }
}
