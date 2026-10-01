using Microsoft.Data.SqlClient;
using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Prana.Datos
{
    // Crea la base inicial y aplica las actualizaciones de estructura en cada arranque.
    public static class InicializadorBaseDatos
    {
        // Nombre de la base utilizada por la aplicacion
        private const string NombreBaseDatos = "PranaElectronica";

        // Conexión a la base master de LocalDB
        // Master se utiliza para comprobar y crear PranaElectronica
        private const string ConexionMaster =
            @"Data Source=(localdb)\MSSQLLocalDB;" +
            @"Initial Catalog=master;" +
            @"Integrated Security=True;" +
            @"Encrypt=False;" +
            @"Connect Timeout=30;";

        // Este método será llamado desde Program.cs antes de abrir el login
        public static void AsegurarBaseDeDatos()
        {
            // Obtiene la carpeta AppData\Local del usuario actual de Windows.
            string carpetaAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            // Construye la carpeta donde se guardarán los archivos de la base.
            string carpetaDatos = Path.Combine(carpetaAppData, "PranaElectronica", "Data");

            // Crea la carpeta si todavía no existe.
            Directory.CreateDirectory(carpetaDatos);

            // Construye la ruta completa del archivo principal de la base.
            string rutaMdf = Path.Combine(carpetaDatos, "PranaElectronica.mdf");

            // Construye la ruta completa del archivo de registro de la base.
            string rutaLdf = Path.Combine(carpetaDatos, "PranaElectronica_log.ldf");

            // Abre una conexión con master para comprobar si existe la base.
            using SqlConnection conexionMaster = new SqlConnection(ConexionMaster);

            // Inicia la conexión con LocalDB.
            conexionMaster.Open();

            // Si la base todavía no existe, la crea.
            if (!BaseDatosExiste(conexionMaster))
            {
                CrearBaseDatos(conexionMaster, rutaMdf, rutaLdf);
            }

            // Define la conexión directa con la base PranaElectronica.
            string conexionPrana =
                @"Data Source=(localdb)\MSSQLLocalDB;" +
                @"Initial Catalog=PranaElectronica;" +
                @"Integrated Security=True;" +
                @"Encrypt=False;" +
                @"Connect Timeout=30;";

            // Crea una conexión con PranaElectronica.
            using SqlConnection conexionBase = new SqlConnection(conexionPrana);

            // Abre la conexión con la base.
            conexionBase.Open();

            // Crea la estructura vacía o actualiza la instalada en cada arranque.
            PrepararEsquema(conexionBase);
        }

        // Comprueba si LocalDB ya tiene registrada la base PranaElectronica.
        private static bool BaseDatosExiste(SqlConnection conexion)
        {
            // Consulta DB_ID, que devuelve el identificador de una base existente.
            const string consulta = "SELECT DB_ID(@nombreBase);";

            // Prepara la consulta para ejecutarla en SQL Server.
            using SqlCommand comando = new SqlCommand(consulta, conexion);

            // Envía el nombre de la base como parámetro seguro.
            comando.Parameters.AddWithValue("@nombreBase", NombreBaseDatos);

            // Ejecuta la consulta y obtiene el resultado.
            object? resultado = comando.ExecuteScalar();

            // Si devuelve un número, la base existe.
            // Si devuelve NULL, todavía no existe.
            return resultado != null && resultado != DBNull.Value;
        }

        // Crea físicamente PranaElectronica y sus archivos MDF y LDF.
        private static void CrearBaseDatos(SqlConnection conexion, string rutaMdf, string rutaLdf)
        {
            // Duplica las comillas simples para evitar errores
            // si alguna carpeta contiene ese carácter.
            string rutaMdfSegura = rutaMdf.Replace("'", "''");
            string rutaLdfSegura = rutaLdf.Replace("'", "''");

            // Construye la instrucción de creación de la base.
            string instruccion =
                $"""
                CREATE DATABASE [{NombreBaseDatos}]
                ON PRIMARY
                (
                    NAME = N'{NombreBaseDatos}',
                    FILENAME = N'{rutaMdfSegura}'
                )
                LOG ON
                (
                    NAME = N'{NombreBaseDatos}_log',
                    FILENAME = N'{rutaLdfSegura}'
                );
                """;

            // Prepara la instrucción CREATE DATABASE.
            using SqlCommand comando = new SqlCommand(instruccion, conexion);

            // Permite más tiempo para la creación inicial.
            comando.CommandTimeout = 120;

            // Crea la base y sus dos archivos.
            comando.ExecuteNonQuery();
        }

        // No depende del nombre del administrador: renombrarlo no vuelve a crear la base.
        // Recibe una conexión abierta para poder verificar el proceso en una base aislada.
        internal static void PrepararEsquema(SqlConnection conexion)
        {
            string actualizaciones = LeerScript("ActualizarEstructuraPrana_LocalDB.sql");
            using SqlTransaction transaccion = conexion.BeginTransaction();
            try
            {
                // Serializa el arranque de dos instancias sobre la misma base.
                using (SqlCommand bloqueo = new SqlCommand(@"
                    SET XACT_ABORT ON;
                    DECLARE @resultado int;
                    EXEC @resultado = sys.sp_getapplock @Resource=N'Prana.ActualizarEsquema',
                        @LockMode=N'Exclusive', @LockOwner=N'Transaction', @LockTimeout=15000;
                    IF @resultado < 0 THROW 50002, N'No se pudo bloquear la actualización de la base. Reintente el inicio.', 1;",
                    conexion, transaccion))
                {
                    bloqueo.ExecuteNonQuery();
                }

                using SqlCommand tablas = new SqlCommand("SELECT COUNT(*) FROM sys.tables WHERE is_ms_shipped=0", conexion, transaccion);
                bool vacia = Convert.ToInt32(tablas.ExecuteScalar()) == 0;
                if (vacia)
                {
                    EjecutarScript(conexion, transaccion, LeerScript("CrearEstructuraPrana_LocalDB.sql"));
                }

                // El script comprueba cada columna y tabla: nunca borra los datos existentes.
                EjecutarScript(conexion, transaccion, actualizaciones);
                EjecutarScript(conexion, transaccion, LeerScript("CajaYComprobantes.sql"));
                transaccion.Commit();
            }
            catch
            {
                // Incluye tanto creación como migraciones: un fallo no deja un esquema a medias.
                if (transaccion.Connection != null) transaccion.Rollback();
                throw;
            }
        }

        private static string LeerScript(string nombre)
        {
            string[] rutas = new[]
            {
                Path.Combine(AppContext.BaseDirectory, nombre),
                Path.Combine(AppContext.BaseDirectory, "BaseDeDatos", nombre),
                Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "BaseDeDatos", nombre),
                Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "BaseDeDatos", nombre)
            };

            foreach (string ruta in rutas)
            {
                string rutaNormalizada = Path.GetFullPath(ruta);
                if (File.Exists(rutaNormalizada))
                    return File.ReadAllText(rutaNormalizada);
            }

            throw new FileNotFoundException($"No se encontró el script necesario para preparar la base de datos: {nombre}", nombre);
        }

        // GO separa lotes de SQL Server y se procesa en C#, dentro de la misma transacción.
        private static void EjecutarScript(SqlConnection conexion, SqlTransaction transaccion, string scriptCompleto)
        {
            string[] bloques = Regex.Split(scriptCompleto, @"^\s*GO\s*;?\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            foreach (string bloque in bloques)
            {
                if (string.IsNullOrWhiteSpace(bloque)) continue;
                using SqlCommand comando = new SqlCommand(bloque, conexion, transaccion);
                comando.CommandTimeout = 120;
                comando.ExecuteNonQuery();
            }
        }
    }
}
