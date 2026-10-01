using Microsoft.Data.SqlClient;
using Prana.Entidades;
using System.Collections.Generic;
using System.Data;

namespace Prana.Datos
{
    // Guarda las actividades realizadas por los usuarios.
    public class CD_LogActividad
    {
        // Inserta un registro de actividad en la base de datos.
        public void Registrar(int idUsuario, string accion, string entidad, int? idEntidad, string detalle)
        {
            // Define la consulta para guardar el log.
            string consulta = "INSERT INTO dbo.LogActividad (id_usuario, accion, entidad, id_entidad, detalle) VALUES (@IdUsuario, @Accion, @Entidad, @IdEntidad, @Detalle);";

            // Abre la conexión con la base de datos.
            using SqlConnection conexion = new SqlConnection(Conexion.ObtenerCadena());

            // Prepara la consulta SQL.
            using SqlCommand comando = new SqlCommand(consulta, conexion);

            // Agrega el usuario que realizó la acción.
            comando.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = idUsuario;

            // Agrega el nombre de la acción realizada.
            comando.Parameters.Add("@Accion", SqlDbType.NVarChar, 100).Value = accion;

            // Agrega el tipo de entidad afectada.
            comando.Parameters.Add("@Entidad", SqlDbType.NVarChar, 100).Value = entidad;

            // Agrega el identificador de la entidad afectada.
            comando.Parameters.Add("@IdEntidad", SqlDbType.Int).Value = idEntidad.HasValue ? idEntidad.Value : DBNull.Value;

            // Agrega los detalles del cambio.
            comando.Parameters.Add("@Detalle", SqlDbType.NVarChar, 1000).Value = string.IsNullOrWhiteSpace(detalle) ? DBNull.Value : detalle;

            // Abre la conexión.
            conexion.Open();

            // Ejecuta el registro de actividad.
            comando.ExecuteNonQuery();
        }

        // Obtiene todas las actividades ordenadas desde la más reciente.
        public List<LogActividad> ObtenerTodos()
        {
            // Consulta las últimas 10 actividades sin limitar el período.
            return ObtenerFiltrado(DateTime.MinValue, DateTime.MaxValue, 10);
        }

        // Obtiene una cantidad limitada de actividades dentro de un período.
        public List<LogActividad> ObtenerFiltrado(DateTime desde, DateTime hasta, int limite, int? remito = null, string accion = "", string usuario = "")
        {
            // Consulta las actividades junto con el nombre del usuario y el rango solicitado.
            string consulta = "SELECT TOP (@Limite) l.id_log, l.id_usuario, u.nombre_usuario, l.accion, l.entidad, l.id_entidad, l.detalle, l.fecha FROM dbo.LogActividad AS l INNER JOIN dbo.Usuario AS u ON u.id_usuario = l.id_usuario WHERE l.fecha >= @Desde AND l.fecha < @Hasta AND (@Remito IS NULL OR (l.entidad=N'Venta' AND l.id_entidad=@Remito)) AND (@Accion=N'' OR CHARINDEX(@Accion,l.accion)>0) AND (@Usuario=N'' OR CHARINDEX(@Usuario,u.nombre_usuario)>0) ORDER BY l.fecha DESC,l.id_log DESC;";

            // Crea la conexión con la base de datos.
            using SqlConnection conexion = new SqlConnection(Conexion.ObtenerCadena());

            // Prepara la consulta SQL.
            using SqlCommand comando = new SqlCommand(consulta, conexion);

            // Agrega el límite de registros solicitado.
            comando.Parameters.Add("@Limite", SqlDbType.Int).Value = limite;
            comando.Parameters.Add("@Remito", SqlDbType.Int).Value = (object?)remito ?? DBNull.Value;
            comando.Parameters.Add("@Accion", SqlDbType.NVarChar, 100).Value = accion.Trim();
            comando.Parameters.Add("@Usuario", SqlDbType.NVarChar, 100).Value = usuario.Trim();

            // Agrega el inicio del período.
            comando.Parameters.Add("@Desde", SqlDbType.DateTime2).Value = desde;

            // Agrega el final exclusivo del período.
            comando.Parameters.Add("@Hasta", SqlDbType.DateTime2).Value = hasta;

            // Abre la conexión.
            conexion.Open();

            // Ejecuta la consulta y obtiene sus resultados.
            using SqlDataReader lector = comando.ExecuteReader();

            // Crea la lista de actividades.
            List<LogActividad> actividades = new List<LogActividad>();

            // Recorre cada actividad devuelta por la base.
            while (lector.Read())
            {
                actividades.Add(new LogActividad { IdLog = lector.GetInt32(lector.GetOrdinal("id_log")), IdUsuario = lector.GetInt32(lector.GetOrdinal("id_usuario")), NombreUsuario = lector.GetString(lector.GetOrdinal("nombre_usuario")), Accion = lector.GetString(lector.GetOrdinal("accion")), Entidad = lector.GetString(lector.GetOrdinal("entidad")), IdEntidad = lector.IsDBNull(lector.GetOrdinal("id_entidad")) ? null : lector.GetInt32(lector.GetOrdinal("id_entidad")), Detalle = lector.IsDBNull(lector.GetOrdinal("detalle")) ? null : lector.GetString(lector.GetOrdinal("detalle")), Fecha = lector.GetDateTime(lector.GetOrdinal("fecha")) });
            }

            // Devuelve las actividades encontradas.
            return actividades;
        }
    }
}
