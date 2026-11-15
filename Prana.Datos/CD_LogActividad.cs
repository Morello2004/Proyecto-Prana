using Microsoft.Data.SqlClient;
using Prana.Entidades;
using System.Collections.Generic;
using System.Data;

namespace Prana.Datos
{
    // guarda las actividades realizadas por los usuarios
    public class CD_LogActividad
    {
        // inserta un registro de actividad en la base de datos
        public void Registrar(int idUsuario, string accion, string entidad, int? idEntidad, string detalle)
        {
            // define la consulta para guardar el log
            string consulta = "INSERT INTO dbo.LogActividad (id_usuario, accion, entidad, id_entidad, detalle) VALUES (@IdUsuario, @Accion, @Entidad, @IdEntidad, @Detalle);";
            // abre la conexion con la base de datos
            using SqlConnection conexion = new SqlConnection(Conexion.ObtenerCadena());
            // prepara la consulta y su conexion los valores variables se envian mediante parametros
            using SqlCommand comando = new SqlCommand(consulta, conexion);
            // agrega el usuario que realizo la accion
            comando.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = idUsuario;
            // agrega el nombre de la accion realizada
            comando.Parameters.Add("@Accion", SqlDbType.NVarChar, 100).Value = accion;
            // agrega el tipo de entidad afectada
            comando.Parameters.Add("@Entidad", SqlDbType.NVarChar, 100).Value = entidad;
            // agrega el identificador de la entidad afectada
            if (idEntidad.HasValue)
            {
                comando.Parameters.Add("@IdEntidad", SqlDbType.Int).Value = idEntidad.Value;
            }
            else
            {
                comando.Parameters.Add("@IdEntidad", SqlDbType.Int).Value = DBNull.Value;
            }
            // agrega los detalles del cambio
            if (string.IsNullOrWhiteSpace(detalle))
            {
                comando.Parameters.Add("@Detalle", SqlDbType.NVarChar, 1000).Value = DBNull.Value;
            }
            else
            {
                comando.Parameters.Add("@Detalle", SqlDbType.NVarChar, 1000).Value = detalle;
            }
            // abre la conexion abre el recurso antes de usarlo; una conexion debe estar abierta para enviar consultas
            conexion.Open();
            // ejecuta el registro de actividad
            comando.ExecuteNonQuery();
        }

        // obtiene todas las actividades ordenadas desde la mas reciente
        public List<LogActividad> ObtenerTodos()
        {
            // consulta las ultimas 10 actividades sin limitar el periodo
            return ObtenerFiltrado(DateTime.MinValue, DateTime.MaxValue, 10);
        }

        // obtiene una cantidad limitada de actividades dentro de un periodo
        public List<LogActividad> ObtenerFiltrado(DateTime desde, DateTime hasta, int limite, int? remito = null, string accion = "", string usuario = "")
        {
            // consulta las actividades junto con el nombre del usuario y el rango solicitado
            string consulta = "SELECT TOP (@Limite) l.id_log, l.id_usuario, u.nombre_usuario, l.accion, l.entidad, l.id_entidad, l.detalle, l.fecha FROM dbo.LogActividad AS l INNER JOIN dbo.Usuario AS u ON u.id_usuario = l.id_usuario WHERE l.fecha >= @Desde AND l.fecha < @Hasta AND (@Remito IS NULL OR (l.entidad=N'Venta' AND l.id_entidad=@Remito)) AND (@Accion=N'' OR CHARINDEX(@Accion,l.accion)>0) AND (@Usuario=N'' OR CHARINDEX(@Usuario,u.nombre_usuario)>0) ORDER BY l.fecha DESC,l.id_log DESC;";
            // crea la conexion con la base de datos
            using SqlConnection conexion = new SqlConnection(Conexion.ObtenerCadena());
            // prepara la consulta y su conexion los valores variables se envian mediante parametros
            using SqlCommand comando = new SqlCommand(consulta, conexion);
            // agrega el limite de registros solicitado
            comando.Parameters.Add("@Limite", SqlDbType.Int).Value = limite;
            comando.Parameters.Add("@Remito", SqlDbType.Int).Value = (object?)remito ?? DBNull.Value;
            comando.Parameters.Add("@Accion", SqlDbType.NVarChar, 100).Value = accion.Trim();
            comando.Parameters.Add("@Usuario", SqlDbType.NVarChar, 100).Value = usuario.Trim();
            // agrega el inicio del periodo
            comando.Parameters.Add("@Desde", SqlDbType.DateTime2).Value = desde;
            // agrega el final exclusivo del periodo
            comando.Parameters.Add("@Hasta", SqlDbType.DateTime2).Value = hasta;
            // abre la conexion abre el recurso antes de usarlo; una conexion debe estar abierta para enviar consultas
            conexion.Open();
            // ejecuta la consulta y obtiene sus resultados
            using SqlDataReader lector = comando.ExecuteReader();
            // crea la lista de actividades
            List<LogActividad> actividades = new List<LogActividad>();
            // recorre cada actividad devuelta por la base avanza el lector de sql una fila por vez hasta agotar el resultado
            while (lector.Read())
            {
                int? valorIdEntidad;
                if (lector.IsDBNull(lector.GetOrdinal("id_entidad")))
                {
                    valorIdEntidad = null;
                }
                else
                {
                    valorIdEntidad = lector.GetInt32(lector.GetOrdinal("id_entidad"));
                }

                string valorDetalle;
                if (lector.IsDBNull(lector.GetOrdinal("detalle")))
                {
                    valorDetalle = null;
                }
                else
                {
                    valorDetalle = lector.GetString(lector.GetOrdinal("detalle"));
                }
                actividades.Add(new LogActividad
                {
                    IdLog = lector.GetInt32(lector.GetOrdinal("id_log")),
                    IdUsuario = lector.GetInt32(lector.GetOrdinal("id_usuario")),
                    NombreUsuario = lector.GetString(lector.GetOrdinal("nombre_usuario")),
                    Accion = lector.GetString(lector.GetOrdinal("accion")),
                    Entidad = lector.GetString(lector.GetOrdinal("entidad")),
                    IdEntidad = valorIdEntidad,
                    Detalle = valorDetalle,
                    Fecha = lector.GetDateTime(lector.GetOrdinal("fecha"))

                });
            }

            // devuelve las actividades encontradas
            return actividades;
        }
    }
}
