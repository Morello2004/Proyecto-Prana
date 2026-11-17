using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Prana.Entidades;
using Microsoft.Data.SqlClient;

namespace Prana.Datos
{
    // agrupa los datos y operaciones de cd usuario
    public class CD_Usuario
    {
        // busca un usuario en sql server utilizando su nombre devuelve un objeto usuario si lo encuentra devuelve null si no existe
        public usuario? BuscarUsuario(string nombreUsuario)
        {
            // guardo en un string la consulta sql para buscar el usuario por nombre en la bd
            string consulta = @"
                                SELECT
                                    id_usuario,
                                     nombre_usuario,
                                     hash_contrasena,
                                     id_rol_usuario,
                                     activo,
                                     intentos_login_fallidos,
                                     fecha_hora_fin_bloqueo,
                                     SYSDATETIME() AS hora_servidor
                                 FROM Usuario
                                 WHERE nombre_usuario = @NombreUsuario;";
            // crea la conexion usando la cadena obtenida desde appconfig el using cerrara y liberara automaticamente la conexion
            using SqlConnection conexion = new SqlConnection(Conexion.ObtenerCadena());
            // prepara la consulta y su conexion los valores variables se envian mediante parametros
            using SqlCommand comando = new SqlCommand(consulta, conexion);
            // reemplaza @nombreusuario con el nombre recibido se especifica que sql server recibira un nvarchar de 100 caracteres
            comando.Parameters.Add("@NombreUsuario", SqlDbType.NVarChar, 100).Value = nombreUsuario;
            // abre la conexion con sql server abre el recurso antes de usarlo; una conexion debe estar abierta para enviar consultas
            conexion.Open();
            // ejecuta el select y obtiene un lector con los resultados
            using SqlDataReader lector = comando.ExecuteReader();
            // read intenta leer la primera fila si devuelve false significa que no se encontro el usuario
            if (!lector.Read())
            {
                return null;
            }
            DateTime? valorBloqueadoHasta;
            if (lector.IsDBNull(lector.GetOrdinal("fecha_hora_fin_bloqueo")))
            {
                valorBloqueadoHasta = null;
            }
            else
            {
                valorBloqueadoHasta = lector.GetDateTime(lector.GetOrdinal("fecha_hora_fin_bloqueo"));
            }

            // crea un objeto usuario
            usuario usuario = new usuario
            {
                idUsuario = lector.GetInt32(lector.GetOrdinal("id_usuario")),
                nombreUsuario = lector.GetString(lector.GetOrdinal("nombre_usuario")),
                contraseña = lector.GetString(lector.GetOrdinal("hash_contrasena")),
                IdRolUsuario = lector.GetInt32(lector.GetOrdinal("id_rol_usuario")),
                activo = lector.GetBoolean(lector.GetOrdinal("activo")),
                IntentosLoginFallidos = lector.GetInt32(lector.GetOrdinal("intentos_login_fallidos")),
                BloqueadoHasta = valorBloqueadoHasta,
                HoraServidor = lector.GetDateTime(lector.GetOrdinal("hora_servidor"))

            };
            return usuario;
        }

        // lista las cuentas y marca su estado activo y de bloqueo
        public List<usuario> ListarUsuarios()
        {
            List<usuario> resultado = new List<usuario>();
            // prepara la conexion configurada y la libera al terminar este alcance
            using SqlConnection conexion = new SqlConnection(Conexion.ObtenerCadena());
            // prepara la consulta y su conexion los valores variables se envian mediante parametros
            using SqlCommand comando = new SqlCommand(@"SELECT id_usuario, nombre_usuario, id_rol_usuario, activo, intentos_login_fallidos, fecha_hora_fin_bloqueo,
                CAST(CASE WHEN fecha_hora_fin_bloqueo > SYSDATETIME() THEN 1 ELSE 0 END AS bit)
                FROM Usuario ORDER BY nombre_usuario", conexion);
            conexion.Open();
            using SqlDataReader lector = comando.ExecuteReader();
            while (lector.Read())
            {
                DateTime? valorBloqueadoHasta1;
                if (lector.IsDBNull(5))
                {
                    valorBloqueadoHasta1 = null;
                }
                else
                {
                    valorBloqueadoHasta1 = lector.GetDateTime(5);
                }
                resultado.Add(new usuario
                {
                    idUsuario = lector.GetInt32(0),
                    nombreUsuario = lector.GetString(1),
                    IdRolUsuario = lector.GetInt32(2),
                    activo = lector.GetBoolean(3),
                    IntentosLoginFallidos = lector.GetInt32(4),
                    BloqueadoHasta = valorBloqueadoHasta1,
                    EstaBloqueado = lector.GetBoolean(6)

                });
            }

            return resultado;
        }

        // registra un fallo de autenticacion de forma atomica y aplica el bloqueo temporal
        public (int Intentos, DateTime? BloqueadoHasta) RegistrarIntentoFallido(int idUsuario)
        {
            const string consulta = @"
DECLARE @ahora datetime2(7) = SYSDATETIME();
DECLARE @estado TABLE (intentos int, bloqueo datetime2(7));
UPDATE dbo.Usuario
SET intentos_login_fallidos = CASE
        WHEN fecha_hora_fin_bloqueo > @ahora THEN intentos_login_fallidos
        WHEN fecha_hora_fin_bloqueo IS NOT NULL THEN 1
        WHEN intentos_login_fallidos < 3 THEN intentos_login_fallidos + 1
        ELSE 3
    END,
    fecha_hora_fin_bloqueo = CASE
        WHEN fecha_hora_fin_bloqueo > @ahora THEN fecha_hora_fin_bloqueo
        WHEN (CASE WHEN fecha_hora_fin_bloqueo IS NOT NULL THEN 1
                   WHEN intentos_login_fallidos < 3 THEN intentos_login_fallidos + 1
                   ELSE 3 END) >= 3 THEN DATEADD(MINUTE, 15, @ahora)
        ELSE NULL
    END
OUTPUT INSERTED.intentos_login_fallidos, INSERTED.fecha_hora_fin_bloqueo INTO @estado
WHERE id_usuario = @IdUsuario AND activo = 1;
SELECT intentos, bloqueo FROM @estado;";
            // prepara la conexion configurada y la libera al terminar este alcance
            using SqlConnection conexion = new SqlConnection(Conexion.ObtenerCadena());
            // prepara la consulta y su conexion los valores variables se envian mediante parametros
            using SqlCommand comando = new SqlCommand(consulta, conexion);
            comando.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = idUsuario;
            conexion.Open();
            using SqlDataReader lector = comando.ExecuteReader();
            if (!lector.Read())
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new InvalidOperationException("El usuario no existe o esta inactivo.");
            }
            DateTime? valorSeleccionado;
            if (lector.IsDBNull(1))
            {
                valorSeleccionado = null;
            }
            else
            {
                valorSeleccionado = lector.GetDateTime(1);
            }

            return (lector.GetInt32(0), valorSeleccionado);
        }

        // restablece los intentos despues de una autenticacion correcta
        public bool RestablecerIntentosLogin(int idUsuario)
        {
            // prepara la conexion configurada y la libera al terminar este alcance
            using SqlConnection conexion = new SqlConnection(Conexion.ObtenerCadena());
            // prepara la consulta y su conexion los valores variables se envian mediante parametros
            using SqlCommand comando = new SqlCommand(@"UPDATE Usuario SET intentos_login_fallidos = 0, fecha_hora_fin_bloqueo = NULL
                WHERE id_usuario = @IdUsuario AND activo = 1;", conexion);
            comando.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = idUsuario;
            conexion.Open();
            return comando.ExecuteNonQuery() == 1;
        }

        // modifica los datos permitidos del usuario sin reemplazar su contrasena vacia
        public void ModificarUsuario(int id, string nombre, string? hash, int rol)
        {
            // prepara la conexion configurada y la libera al terminar este alcance
            using SqlConnection conexion = new SqlConnection(Conexion.ObtenerCadena());
            // prepara la consulta y su conexion los valores variables se envian mediante parametros
            using SqlCommand comando = new SqlCommand(@"UPDATE Usuario SET nombre_usuario=@Nombre,
                hash_contrasena=COALESCE(@Hash,hash_contrasena),
                intentos_login_fallidos=CASE WHEN @Hash IS NULL THEN intentos_login_fallidos ELSE 0 END,
                fecha_hora_fin_bloqueo=CASE WHEN @Hash IS NULL THEN fecha_hora_fin_bloqueo ELSE NULL END,
                id_rol_usuario=@Rol WHERE id_usuario=@Id", conexion);
            comando.Parameters.Add("@Nombre", SqlDbType.NVarChar, 100).Value = nombre;
            comando.Parameters.Add("@Hash", SqlDbType.NVarChar, 255).Value = (object?)hash ?? DBNull.Value;
            comando.Parameters.Add("@Id", SqlDbType.Int).Value = id;
            comando.Parameters.Add("@Rol", SqlDbType.Int).Value = rol;
            conexion.Open();
            using SqlTransaction transaccion = conexion.BeginTransaction(IsolationLevel.Serializable);
            comando.Transaction = transaccion;
            ExigirNombreDisponible(conexion, transaccion, nombre, id);
            // protege el acceso a los datos para atender los errores de la operacion
            try
            {
                if (comando.ExecuteNonQuery() != 1)
                {
                    // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                    throw new InvalidOperationException("El usuario ya no existe.");
                }

                // confirma todos los cambios de esta transaccion como una unidad
                transaccion.Commit();
            }
            // atiende el error de esta operacion antes de informar o propagar el problema
            catch (SqlException error) when (error.Number == 2601 || error.Number == 2627)
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new InvalidOperationException("Ese nombre de usuario ya está registrado, aunque la cuenta esté dada de baja. Elija otro nombre.", error);
            }
        }

        // da de baja una cuenta sin borrar sus datos y registra la accion
        public void DarDeBajaUsuario(int idUsuario, int idAdministrador)
        {
            // prepara la conexion configurada y la libera al terminar este alcance
            using SqlConnection conexion = new SqlConnection(Conexion.ObtenerCadena());
            conexion.Open();
            using SqlTransaction transaccion = conexion.BeginTransaction();
            // protege el acceso a los datos para atender los errores de la operacion
            try
            {
                // prepara la consulta y su conexion los valores variables se envian mediante parametros
                using SqlCommand baja = new SqlCommand(@"UPDATE Usuario
                    SET activo = 0, intentos_login_fallidos = 0, fecha_hora_fin_bloqueo = NULL
                    WHERE id_usuario = @Usuario AND activo = 1;", conexion, transaccion);
                baja.Parameters.Add("@Usuario", SqlDbType.Int).Value = idUsuario;
                if (baja.ExecuteNonQuery() != 1)
                {
                    // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                    throw new InvalidOperationException("El usuario no existe o ya esta dado de baja.");
                }

                // prepara la consulta y su conexion los valores variables se envian mediante parametros
                using SqlCommand historial = new SqlCommand(@"INSERT INTO Log_Actividad(id_usuario, accion, entidad, id_entidad, detalle)
                    VALUES(@Administrador, N'DAR_DE_BAJA_USUARIO', N'Usuario', @Usuario, N'Cuenta desactivada logicamente');", conexion, transaccion);
                historial.Parameters.Add("@Administrador", SqlDbType.Int).Value = idAdministrador;
                historial.Parameters.Add("@Usuario", SqlDbType.Int).Value = idUsuario;
                historial.ExecuteNonQuery();
                // confirma todos los cambios de esta transaccion como una unidad
                transaccion.Commit();
            }
            // atiende el error de esta operacion antes de informar o propagar el problema
            catch
            {
                // revierte los cambios no confirmados de esta transaccion
                transaccion.Rollback();
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw;
            }
        }

        // crea un usuario con su hash y el rol seleccionado
        public void CrearUsuario(string nombreUsuario, string hashContrasena, int idRolUsuario)
        {
            // define la consulta parametrizada para crear el usuario
            string consulta = @" INSERT INTO Usuario( nombre_usuario,hash_contrasena, id_rol_usuario,activo) VALUES(@NombreUsuario, @HashContrasena, @IdRolUsuario, 1);";
            // crea la conexion con la base de datos
            using SqlConnection conexion = new SqlConnection(Conexion.ObtenerCadena());
            // prepara la consulta y su conexion los valores variables se envian mediante parametros
            using SqlCommand comando = new SqlCommand(consulta, conexion);
            // envia el nombre de usuario como parametro
            comando.Parameters.Add("@NombreUsuario", SqlDbType.NVarChar, 100).Value = nombreUsuario;
            // envia el hash de la contrasena como parametro
            comando.Parameters.Add("@HashContrasena", SqlDbType.NVarChar, 255).Value = hashContrasena;
            // envia el rol seleccionado como parametro
            comando.Parameters.Add("@IdRolUsuario", SqlDbType.Int).Value = idRolUsuario;
            // abre la conexion abre el recurso antes de usarlo; una conexion debe estar abierta para enviar consultas
            conexion.Open();
            using SqlTransaction transaccion = conexion.BeginTransaction(IsolationLevel.Serializable);
            comando.Transaction = transaccion;
            ExigirNombreDisponible(conexion, transaccion, nombreUsuario, 0);
            // el indice unico incluye tambien las cuentas dadas de baja
            try
            {
                comando.ExecuteNonQuery();
                // confirma todos los cambios de esta transaccion como una unidad
                transaccion.Commit();
            }
            // atiende el error de esta operacion antes de informar o propagar el problema
            catch (SqlException error) when (error.Number == 2601 || error.Number == 2627)
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new InvalidOperationException("Ese nombre de usuario ya está registrado, aunque la cuenta esté dada de baja. Elija otro nombre.", error);
            }
        }

        // bloquea la comprobacion y el guardado juntos, incluso en bases antiguas sin indice unico comprueba exigir nombre disponible; si no se cumple la regla interrumpe con un mensaje antes de continuar
        private static void ExigirNombreDisponible(SqlConnection conexion, SqlTransaction transaccion, string nombre, int idActual)
        {
            // prepara la consulta y su conexion los valores variables se envian mediante parametros
            using SqlCommand consulta = new SqlCommand(@"SELECT COUNT(*) FROM dbo.Usuario WITH (UPDLOCK, HOLDLOCK)
                WHERE nombre_usuario=@Nombre AND id_usuario<>@Id", conexion, transaccion);
            consulta.Parameters.Add("@Nombre", SqlDbType.NVarChar, 100).Value = nombre;
            consulta.Parameters.Add("@Id", SqlDbType.Int).Value = idActual;
            if (Convert.ToInt32(consulta.ExecuteScalar()) > 0)
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new InvalidOperationException("Ese nombre de usuario ya está registrado, aunque la cuenta esté dada de baja. Elija otro nombre.");
            }
        }

        // desbloquea una cuenta y registra la accion del administrador
        public void DesbloquearUsuario(int idUsuario, int idAdministrador)
        {
            // prepara la conexion configurada y la libera al terminar este alcance
            using SqlConnection conexion = new SqlConnection(Conexion.ObtenerCadena());
            conexion.Open();
            using SqlTransaction transaccion = conexion.BeginTransaction();
            // protege el acceso a los datos para atender los errores de la operacion
            try
            {
                // prepara la consulta y su conexion los valores variables se envian mediante parametros
                using SqlCommand desbloqueo = new SqlCommand(@"UPDATE Usuario SET intentos_login_fallidos = 0, fecha_hora_fin_bloqueo = NULL
                    WHERE id_usuario = @IdUsuario AND activo = 1;", conexion, transaccion);
                desbloqueo.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = idUsuario;
                if (desbloqueo.ExecuteNonQuery() != 1)
                {
                    // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                    throw new InvalidOperationException("El usuario no existe o esta inactivo.");
                }

                // prepara la consulta y su conexion los valores variables se envian mediante parametros
                using SqlCommand historial = new SqlCommand(@"INSERT INTO Log_Actividad(id_usuario, accion, entidad, id_entidad, detalle)
                    VALUES(@Administrador, N'DESBLOQUEAR_USUARIO', N'Usuario', @Usuario, N'El administrador restablecio el acceso');", conexion, transaccion);
                historial.Parameters.Add("@Administrador", SqlDbType.Int).Value = idAdministrador;
                historial.Parameters.Add("@Usuario", SqlDbType.Int).Value = idUsuario;
                historial.ExecuteNonQuery();
                // confirma todos los cambios de esta transaccion como una unidad
                transaccion.Commit();
            }
            // atiende el error de esta operacion antes de informar o propagar el problema
            catch
            {
                // revierte los cambios no confirmados de esta transaccion
                transaccion.Rollback();
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw;
            }
        }
    }
}
