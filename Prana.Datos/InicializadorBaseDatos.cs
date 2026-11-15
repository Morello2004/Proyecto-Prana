// guia del archivo: el primer inicio crea la base si falta y prepara sus tablas antes de mostrar el acceso orden: crear esquema si no hay tablas, aplicar actualizarestructura y luego cajaycomprobantes la creacion fisica de la base sucede antes de la transaccion del esquema; son pasos distintos el bloqueo de aplicacion serializa dos preparaciones simultaneas; la transaccion revierte cambios si falla un lote no ejecutar pruebas de integracion contra el catalogo comercial para comprobar estas migraciones
using Microsoft.Data.SqlClient;
using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Prana.Datos
{
    // crea la base inicial y aplica las actualizaciones de estructura en cada arranque
    public static class InicializadorBaseDatos
    {
        // nombre de la base utilizada por la aplicacion
        private const string NombreBaseDatos = "PranaElectronica";
        // conexion a la base master de localdb master se utiliza para comprobar y crear pranaelectronica
        private const string ConexionMaster = @"Data Source=(localdb)\MSSQLLocalDB;" + @"Initial Catalog=master;" + @"Integrated Security=True;" + @"Encrypt=False;" + @"Connect Timeout=30;";
        // este metodo sera llamado desde programcs antes de abrir el login comprueba localdb, crea la base si falta y solicita preparar el esquema antes de mostrar el acceso
        public static void AsegurarBaseDeDatos()
        {
            // obtiene la carpeta appdata\local del usuario actual de windows
            string carpetaAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            // construye la carpeta donde se guardaran los archivos de la base
            string carpetaDatos = Path.Combine(carpetaAppData, "PranaElectronica", "Data");
            // crea la carpeta si todavia no existe
            Directory.CreateDirectory(carpetaDatos);
            // construye la ruta completa del archivo principal de la base
            string rutaMdf = Path.Combine(carpetaDatos, "PranaElectronica.mdf");
            // construye la ruta completa del archivo de registro de la base
            string rutaLdf = Path.Combine(carpetaDatos, "PranaElectronica_log.ldf");
            // abre una conexion con master para comprobar si existe la base
            using SqlConnection conexionMaster = new SqlConnection(ConexionMaster);
            // inicia la conexion con localdb abre el recurso antes de usarlo; una conexion debe estar abierta para enviar consultas
            conexionMaster.Open();
            // si la base todavia no existe, la crea
            if (!BaseDatosExiste(conexionMaster))
            {
                CrearBaseDatos(conexionMaster, rutaMdf, rutaLdf);
            }

            // define la conexion directa con la base pranaelectronica
            string conexionPrana = @"Data Source=(localdb)\MSSQLLocalDB;" + @"Initial Catalog=PranaElectronica;" + @"Integrated Security=True;" + @"Encrypt=False;" + @"Connect Timeout=30;";
            // crea una conexion con pranaelectronica
            using SqlConnection conexionBase = new SqlConnection(conexionPrana);
            // abre la conexion con la base abre el recurso antes de usarlo; una conexion debe estar abierta para enviar consultas
            conexionBase.Open();
            // crea la estructura vacia o actualiza la instalada en cada arranque
            PrepararEsquema(conexionBase);
        }

        // comprueba si localdb ya tiene registrada la base pranaelectronica
        private static bool BaseDatosExiste(SqlConnection conexion)
        {
            // consulta db_id, que devuelve el identificador de una base existente
            const string consulta = "SELECT DB_ID(@nombreBase);";
            // prepara la consulta y su conexion los valores variables se envian mediante parametros
            using SqlCommand comando = new SqlCommand(consulta, conexion);
            // envia el nombre de la base como parametro seguro
            comando.Parameters.AddWithValue("@nombreBase", NombreBaseDatos);
            // ejecuta la consulta y obtiene el resultado
            object? resultado = comando.ExecuteScalar();
            // si devuelve un numero, la base existe si devuelve null, todavia no existe
            return resultado != null && resultado != DBNull.Value;
        }

        // crea fisicamente pranaelectronica y sus archivos mdf y ldf
        private static void CrearBaseDatos(SqlConnection conexion, string rutaMdf, string rutaLdf)
        {
            // duplica las comillas simples para evitar errores si alguna carpeta contiene ese caracter
            string rutaMdfSegura = rutaMdf.Replace("'", "''");
            string rutaLdfSegura = rutaLdf.Replace("'", "''");
            // construye la instruccion de creacion de la base
            string instruccion = $"""
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
            // prepara la consulta y su conexion los valores variables se envian mediante parametros
            using SqlCommand comando = new SqlCommand(instruccion, conexion);
            // permite mas tiempo para la creacion inicial
            comando.CommandTimeout = 120;
            // crea la base y sus dos archivos
            comando.ExecuteNonQuery();
        }

        // no depende del nombre del administrador: renombrarlo no vuelve a crear la base recibe una conexion abierta para poder verificar el proceso en una base aislada ejecuta creacion y migraciones bajo una transaccion y un bloqueo comun para que dos inicios no actualicen simultaneamente
        internal static void PrepararEsquema(SqlConnection conexion)
        {
            string actualizaciones = LeerScript("ActualizarEstructuraPrana_LocalDB.sql");
            using SqlTransaction transaccion = conexion.BeginTransaction();
            // protege el acceso a los datos para atender los errores de la operacion
            try
            {
                // serializa el arranque de dos instancias sobre la misma base
                using (SqlCommand bloqueo = new SqlCommand(@"
                    SET XACT_ABORT ON;
                    DECLARE @resultado int;
                    EXEC @resultado = sys.sp_getapplock @Resource=N'Prana.ActualizarEsquema',
                        @LockMode=N'Exclusive', @LockOwner=N'Transaction', @LockTimeout=15000;
                    IF @resultado < 0 THROW 50002, N'No se pudo bloquear la actualización de la base. Reintente el inicio.', 1;", conexion, transaccion))
                {
                    bloqueo.ExecuteNonQuery();
                }

                // prepara la consulta y su conexion los valores variables se envian mediante parametros
                using SqlCommand tablas = new SqlCommand("SELECT COUNT(*) FROM sys.tables WHERE is_ms_shipped=0", conexion, transaccion);
                bool vacia = Convert.ToInt32(tablas.ExecuteScalar()) == 0;
                if (vacia)
                {
                    EjecutarScript(conexion, transaccion, LeerScript("CrearEstructuraPrana_LocalDB.sql"));
                }

                // el script comprueba las estructuras y aplica tambien eliminaciones de elementos antiguos; requiere respaldo previo
                EjecutarScript(conexion, transaccion, actualizaciones);
                EjecutarScript(conexion, transaccion, LeerScript("CajaYComprobantes.sql"));
                // relaciona operaciones anteriores y habilita varias aperturas diarias dentro de la misma migracion
                EjecutarScript(conexion, transaccion, LeerScript("CajasPorTurno.sql"));
                // confirma todos los cambios de esta transaccion como una unidad
                transaccion.Commit();
            }
            // atiende el error de esta operacion antes de informar o propagar el problema
            catch
            {
                // incluye tanto creacion como migraciones: un fallo no deja un esquema a medias
                if (transaccion.Connection != null)
                {
                    // revierte los cambios no confirmados de esta transaccion
                    transaccion.Rollback();
                }

                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw;
            }
        }

        // resuelve leer script con los datos recibidos y entrega el resultado al codigo que lo llama
        private static string LeerScript(string nombre)
        {
            string[] rutas = new[]
            {
                Path.Combine(AppContext.BaseDirectory, nombre),
                Path.Combine(AppContext.BaseDirectory, "BaseDeDatos", nombre),
                Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "BaseDeDatos", nombre),
                Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "BaseDeDatos", nombre)
            };
            // procesa cada ruta de rutas sin cambiar el orden de la coleccion
            foreach (string ruta in rutas)
            {
                string rutaNormalizada = Path.GetFullPath(ruta);
                if (File.Exists(rutaNormalizada))
                {
                    return File.ReadAllText(rutaNormalizada);
                }
            }

            // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
            throw new FileNotFoundException($"No se encontró el script necesario para preparar la base de datos: {nombre}", nombre);
        }

        // go separa lotes de sql server y se procesa en c#, dentro de la misma transaccion divide el archivo por go y ejecuta cada lote dentro de la transaccion recibida; go no se envia como instruccion sql
        private static void EjecutarScript(SqlConnection conexion, SqlTransaction transaccion, string scriptCompleto)
        {
            string[] bloques = Regex.Split(scriptCompleto, @"^\s*GO\s*;?\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            // procesa cada bloque de bloques sin cambiar el orden de la coleccion
            foreach (string bloque in bloques)
            {
                // distingue un dato vacio de un valor ingresado antes de continuar
                if (string.IsNullOrWhiteSpace(bloque))
                {
                    continue;
                }

                // prepara la consulta y su conexion los valores variables se envian mediante parametros
                using SqlCommand comando = new SqlCommand(bloque, conexion, transaccion);
                comando.CommandTimeout = 120;
                comando.ExecuteNonQuery();
            }
        }
    }
}
