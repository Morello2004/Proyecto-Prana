using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Prana.Entidades;
using Microsoft.Data.SqlClient;

namespace Prana.Datos
{
    public class CD_Usuario
    {
        // Busca un usuario en SQL Server utilizando su nombre
        // Devuelve un objeto Usuario si lo encuentra
        // Devuelve null si no existe
        public usuario? BuscarUsuario(string nombreUsuario)
        {
            //guardo en un string la consulta sql para buscar el usuario por nombre en la BD
            string consulta = @"
                                SELECT
                                    id_usuario,
                                     nombre_usuario,
                                     email,
                                     hash_contrasena,
                                     id_rol_usuario,
                                     activo,
                                     intentos_login_fallidos,
                                     bloqueado_hasta,
                                     SYSDATETIME() AS hora_servidor
                                 FROM Usuario
                                 WHERE nombre_usuario = @NombreUsuario;";


            // Crea la conexion usando la cadena obtenida desde App.config
            // El using cerrara y liberara automaticamente la conexion
            using SqlConnection conexion = new SqlConnection(Conexion.ObtenerCadena());

            // Prepara la consulta para ejecutarla mediante la conexion
            using SqlCommand comando = new SqlCommand(consulta, conexion);


            // Reemplaza @NombreUsuario con el nombre recibido.
            // Se especifica que SQL Server recibira un NVARCHAR de 100 caracteres
            comando.Parameters.Add("@NombreUsuario", SqlDbType.NVarChar, 100).Value = nombreUsuario;

            // Abre la conexion con SQL Server
            conexion.Open();

            // Ejecuta el SELECT y obtiene un lector con los resultados
            using SqlDataReader lector = comando.ExecuteReader();

            // Read intenta leer la primera fila
            // Si devuelve false significa que no se encontro el usuario
            if (!lector.Read())
            {
                return null;
            }

            //crea un objeto Usuario con los valores encontrados

            usuario usuario = new usuario {
                idUsuario = lector.GetInt32(lector.GetOrdinal("id_usuario")),
            nombreUsuario = lector.GetString(lector.GetOrdinal("nombre_usuario")),
                // Si email es NULL en la base, guarda un texto vacio en el objeto
                mail = lector.IsDBNull(lector.GetOrdinal("email")) ? string.Empty : lector.GetString(lector.GetOrdinal("email")),
            contraseña = lector.GetString(lector.GetOrdinal("hash_contrasena")),
                IdRolUsuario = lector.GetInt32(lector.GetOrdinal("id_rol_usuario")),
            activo = lector.GetBoolean(lector.GetOrdinal("activo")),
            IntentosLoginFallidos = lector.GetInt32(lector.GetOrdinal("intentos_login_fallidos")),
            BloqueadoHasta = lector.IsDBNull(lector.GetOrdinal("bloqueado_hasta")) ? null : lector.GetDateTime(lector.GetOrdinal("bloqueado_hasta")),
            HoraServidor = lector.GetDateTime(lector.GetOrdinal("hora_servidor"))
            };

            return usuario;
        }

        // Lista los usuarios y marca las cuentas bloqueadas en este momento.
        public List<usuario> ListarUsuarios()
        {
            var resultado = new List<usuario>();
            using var conexion = new SqlConnection(Conexion.ObtenerCadena());
            using var comando = new SqlCommand(@"SELECT id_usuario, nombre_usuario, id_rol_usuario, intentos_login_fallidos, bloqueado_hasta,
                CAST(CASE WHEN bloqueado_hasta > SYSDATETIME() THEN 1 ELSE 0 END AS bit)
                FROM Usuario ORDER BY nombre_usuario", conexion);
            conexion.Open();
            using var lector = comando.ExecuteReader();
            while (lector.Read()) resultado.Add(new usuario
            {
                idUsuario = lector.GetInt32(0),
                nombreUsuario = lector.GetString(1),
                IdRolUsuario = lector.GetInt32(2),
                IntentosLoginFallidos = lector.GetInt32(3),
                BloqueadoHasta = lector.IsDBNull(4) ? null : lector.GetDateTime(4),
                EstaBloqueado = lector.GetBoolean(5)
            });
            return resultado;
        }

        // Registra un fallo de autenticacion de forma atomica y aplica el bloqueo temporal.
        public (int Intentos, DateTime? BloqueadoHasta) RegistrarIntentoFallido(int idUsuario)
        {
            const string consulta = @"
DECLARE @ahora datetime2(7) = SYSDATETIME();
DECLARE @estado TABLE (intentos int, bloqueo datetime2(7));
UPDATE dbo.Usuario
SET intentos_login_fallidos = CASE
        WHEN bloqueado_hasta > @ahora THEN intentos_login_fallidos
        WHEN bloqueado_hasta IS NOT NULL THEN 1
        WHEN intentos_login_fallidos < 3 THEN intentos_login_fallidos + 1
        ELSE 3
    END,
    bloqueado_hasta = CASE
        WHEN bloqueado_hasta > @ahora THEN bloqueado_hasta
        WHEN (CASE WHEN bloqueado_hasta IS NOT NULL THEN 1
                   WHEN intentos_login_fallidos < 3 THEN intentos_login_fallidos + 1
                   ELSE 3 END) >= 3 THEN DATEADD(MINUTE, 15, @ahora)
        ELSE NULL
    END
OUTPUT INSERTED.intentos_login_fallidos, INSERTED.bloqueado_hasta INTO @estado
WHERE id_usuario = @IdUsuario AND activo = 1;
SELECT intentos, bloqueo FROM @estado;";
            using SqlConnection conexion = new SqlConnection(Conexion.ObtenerCadena());
            using SqlCommand comando = new SqlCommand(consulta, conexion);
            comando.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = idUsuario;
            conexion.Open();
            using SqlDataReader lector = comando.ExecuteReader();
            if (!lector.Read()) throw new InvalidOperationException("El usuario no existe o esta inactivo.");
            return (lector.GetInt32(0), lector.IsDBNull(1) ? null : lector.GetDateTime(1));
        }

        // Restablece los intentos despues de una autenticacion correcta.
        public bool RestablecerIntentosLogin(int idUsuario)
        {
            using SqlConnection conexion = new SqlConnection(Conexion.ObtenerCadena());
            using SqlCommand comando = new SqlCommand(@"UPDATE Usuario SET intentos_login_fallidos = 0, bloqueado_hasta = NULL
                WHERE id_usuario = @IdUsuario AND activo = 1;", conexion);
            comando.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = idUsuario;
            conexion.Open();
            return comando.ExecuteNonQuery() == 1;
        }
        // Modifica los datos permitidos del usuario sin reemplazar su contrasena vacia.
        public void ModificarUsuario(int id, string nombre, string? hash, int rol)
        {
            using var conexion = new SqlConnection(Conexion.ObtenerCadena());
            using var comando = new SqlCommand(@"UPDATE Usuario SET nombre_usuario=@Nombre,
                hash_contrasena=COALESCE(@Hash,hash_contrasena),
                intentos_login_fallidos=CASE WHEN @Hash IS NULL THEN intentos_login_fallidos ELSE 0 END,
                bloqueado_hasta=CASE WHEN @Hash IS NULL THEN bloqueado_hasta ELSE NULL END,
                id_rol_usuario=@Rol WHERE id_usuario=@Id", conexion);
            comando.Parameters.Add("@Nombre", SqlDbType.NVarChar, 100).Value = nombre;
            comando.Parameters.Add("@Hash", SqlDbType.NVarChar, 255).Value = (object?)hash ?? DBNull.Value;
            comando.Parameters.Add("@Id", SqlDbType.Int).Value = id;
            comando.Parameters.Add("@Rol", SqlDbType.Int).Value = rol;
            conexion.Open();
            try {
                if (comando.ExecuteNonQuery() != 1) throw new InvalidOperationException("El usuario ya no existe.");
            } catch (SqlException ex) when (ex.Number == 2601 || ex.Number == 2627) {
                throw new InvalidOperationException("Ya existe otro usuario con ese nombre.", ex);
            }
        }
        // Crea un usuario con su hash y el rol seleccionado.
        public void CrearUsuario(string nombreUsuario, string hashContrasena, int idRolUsuario)
        {
            // Define la consulta parametrizada para crear el usuario.
            string consulta = @" INSERT INTO Usuario( nombre_usuario,email,hash_contrasena, id_rol_usuario,activo) VALUES(@NombreUsuario, NULL, @HashContrasena, @IdRolUsuario, 1);";

            // Crea la conexion con la base de datos
            using SqlConnection conexion = new SqlConnection(Conexion.ObtenerCadena());

            // Prepara la consulta SQL.
            using SqlCommand comando = new SqlCommand(consulta, conexion);

            // Envia el nombre de usuario como parametro
            comando.Parameters.Add("@NombreUsuario", SqlDbType.NVarChar, 100).Value = nombreUsuario;

            // Envia el hash de la contrasena como parametro
            comando.Parameters.Add("@HashContrasena", SqlDbType.NVarChar, 255).Value = hashContrasena;

            // Envia el rol seleccionado como parametro
            comando.Parameters.Add("@IdRolUsuario", SqlDbType.Int).Value = idRolUsuario;

            // Abre la conexion
            conexion.Open();

            // Ejecuta la insercion
            comando.ExecuteNonQuery();
        }

        // Desbloquea una cuenta y registra la accion del administrador.
        public void DesbloquearUsuario(int idUsuario, int idAdministrador)
        {
            using SqlConnection conexion = new SqlConnection(Conexion.ObtenerCadena());
            conexion.Open();
            using SqlTransaction transaccion = conexion.BeginTransaction();
            try
            {
                using SqlCommand desbloqueo = new SqlCommand(@"UPDATE Usuario SET intentos_login_fallidos = 0, bloqueado_hasta = NULL
                    WHERE id_usuario = @IdUsuario AND activo = 1;", conexion, transaccion);
                desbloqueo.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = idUsuario;
                if (desbloqueo.ExecuteNonQuery() != 1) throw new InvalidOperationException("El usuario no existe o esta inactivo.");
                using SqlCommand historial = new SqlCommand(@"INSERT INTO LogActividad(id_usuario, accion, entidad, id_entidad, detalle)
                    VALUES(@Administrador, N'DESBLOQUEAR_USUARIO', N'Usuario', @Usuario, N'El administrador restablecio el acceso');", conexion, transaccion);
                historial.Parameters.Add("@Administrador", SqlDbType.Int).Value = idAdministrador;
                historial.Parameters.Add("@Usuario", SqlDbType.Int).Value = idUsuario;
                historial.ExecuteNonQuery();
                transaccion.Commit();
            }
            catch
            {
                transaccion.Rollback();
                throw;
            }
        }
    }
}
