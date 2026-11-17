// guía del archivo: crea la estructura completa cuando la base está vacía y actualiza las instalaciones anteriores con respaldo. La creación del archivo de base ocurre antes de la transacción del esquema; el bloqueo serializa instalaciones simultáneas y la transacción revierte el armado si falla un script.
using Microsoft.Data.SqlClient;
using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Prana.Datos
{
    // crea la base inicial y actualiza el esquema de las bases anteriores
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

        // Recibe una conexión abierta; crea el esquema vacío o valida el resultado de la actualización de una base existente.
        internal static void PrepararEsquema(SqlConnection conexion)
        {
            ActualizadorBaseDatos.ActualizarSiNecesario(conexion);
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
                    EjecutarScript(conexion, transaccion, LeerScript("InstalarPrana.sql"));
                }
                else
                {
                    if (!EsquemaVigente(conexion, transaccion))
                    {
                        throw new InvalidOperationException("La base no tiene una estructura compatible con esta versión de Prana. Se conservaron los datos.");
                    }
                }
                // confirma todos los cambios de esta transaccion como una unidad
                transaccion.Commit();
            }
            // atiende el error de esta operacion antes de informar o propagar el problema
            catch
            {
                // revierte la instalación inicial o la validación si ocurre un error
                if (transaccion.Connection != null)
                {
                    // revierte los cambios no confirmados de esta transaccion
                    transaccion.Rollback();
                }

                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw;
            }
        }

        internal static bool EsquemaVigente(SqlConnection conexion, SqlTransaction? transaccion)
        {
            using SqlCommand comando = new SqlCommand(@"
                        SELECT CASE WHEN OBJECT_ID(N'dbo.Log_Actividad',N'U') IS NOT NULL
                            AND OBJECT_ID(N'dbo.Venta_Datos_Entrega',N'U') IS NOT NULL
                            AND COL_LENGTH(N'dbo.Cliente',N'numero_documento') IS NOT NULL
                            AND COL_LENGTH(N'dbo.Venta',N'fecha_hora_venta') IS NOT NULL
                            AND COL_LENGTH(N'dbo.Envio',N'entregado') IS NOT NULL
                            AND COL_LENGTH(N'dbo.Cliente',N'id_localidad') IS NOT NULL
                            AND OBJECT_ID(N'dbo.Localidad',N'U') IS NOT NULL
                            AND COL_LENGTH(N'dbo.Localidad',N'id_localidad') IS NOT NULL
                            AND COL_LENGTH(N'dbo.Localidad',N'nombre_localidad') IS NOT NULL
                            AND COL_LENGTH(N'dbo.Localidad',N'activo_localidad') IS NOT NULL
                            AND COL_LENGTH(N'dbo.Pago',N'importe_cobrado') IS NOT NULL
                            AND COL_LENGTH(N'dbo.Medio_Pago',N'porcentaje_recargo_base') IS NOT NULL
                            AND COL_LENGTH(N'dbo.Caja',N'id_caja') IS NOT NULL
                            AND COL_LENGTH(N'dbo.Caja_Movimiento',N'tipo_movimiento') IS NOT NULL
                            AND NOT EXISTS (
                                SELECT 1 FROM (VALUES
                                    (N'Cliente',N'fecha_hora_registro'),
                                    (N'Log_Actividad',N'fecha_hora_actividad'),
                                    (N'Pago',N'importe_base'),(N'Pago',N'cantidad_cuotas'),
                                    (N'Pago',N'clasificacion_cobro'),(N'Pago',N'fecha_hora_pago'),(N'Pago',N'id_caja'),
                                    (N'Medio_Pago',N'clasificacion_cobro'),
                                    (N'Medio_Pago',N'porcentaje_recargo_2_cuotas'),(N'Medio_Pago',N'porcentaje_recargo_3_cuotas'),
                                    (N'Medio_Pago',N'porcentaje_recargo_6_cuotas'),(N'Medio_Pago',N'porcentaje_recargo_9_cuotas'),
                                    (N'Medio_Pago',N'porcentaje_recargo_12_cuotas'),
                                    (N'Variante_Producto',N'precio_venta_unitario'),(N'Variante_Producto',N'costo_unitario'),
                                    (N'Ingreso_Stock',N'numero_remito_proveedor'),(N'Ingreso_Stock',N'fecha_hora_ingreso'),
                                    (N'Venta_Datos_Entrega',N'fecha_estimada_entrega'),(N'Venta_Datos_Entrega',N'direccion_sugerida'),
                                    (N'Venta_Datos_Entrega',N'localidad_sugerida'),(N'Venta_Datos_Entrega',N'observaciones_entrega'),
                                    (N'Caja',N'fecha_operativa'),(N'Caja',N'fecha_hora_apertura'),
                                    (N'Caja',N'fecha_hora_fin_turno'),(N'Caja',N'fecha_hora_registro_cierre'),
                                    (N'Caja',N'id_usuario_cierre'),(N'Caja',N'id_usuario_apertura'),
                                    (N'Caja_Movimiento',N'id_movimiento_caja'),(N'Caja_Movimiento',N'fecha_hora_movimiento'),
                                    (N'Caja_Movimiento',N'medio_movimiento'),(N'Caja_Movimiento',N'id_caja'),
                                    (N'Remito',N'fecha_hora_emision'),(N'Envio',N'fecha_hora_entrega'),
                                    (N'Usuario',N'fecha_hora_fin_bloqueo')
                                ) esperadas(tabla,columna)
                                WHERE COL_LENGTH(N'dbo.'+QUOTENAME(esperadas.tabla),esperadas.columna) IS NULL)
                            AND EXISTS (SELECT 1 FROM sys.foreign_key_columns
                                WHERE parent_object_id=OBJECT_ID(N'dbo.Cliente')
                                  AND parent_column_id=COLUMNPROPERTY(OBJECT_ID(N'dbo.Cliente'),N'id_localidad',N'ColumnId')
                                  AND referenced_object_id=OBJECT_ID(N'dbo.Localidad'))
                        THEN 1 ELSE 0 END", conexion, transaccion);
            return Convert.ToInt32(comando.ExecuteScalar()) == 1;
        }
        // resuelve leer script con los datos recibidos y entrega el resultado al codigo que lo llama
        internal static string LeerScript(string nombre)
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
        internal static void EjecutarScript(SqlConnection conexion, SqlTransaction transaccion, string scriptCompleto)
        {
            // Las secciones históricas declaran variables propias y requieren lotes separados.
            scriptCompleto = Regex.Replace(scriptCompleto, @"^(-- ===== .+ =====)\r?$", "GO\n$1", RegexOptions.Multiline);
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
