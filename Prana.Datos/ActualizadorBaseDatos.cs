using Microsoft.Data.SqlClient;
using System;
using System.IO;

namespace Prana.Datos
{
    // Reutiliza las etapas del esquema inicial para actualizar instalaciones anteriores.
    internal static class ActualizadorBaseDatos
    {
        private static bool Existe(SqlConnection conexion, SqlTransaction? transaccion, string tabla, string? columna = null)
        {
            using SqlCommand comando = new SqlCommand(columna == null
                ? "SELECT CASE WHEN OBJECT_ID(@tabla,N'U') IS NOT NULL THEN 1 ELSE 0 END"
                : "SELECT CASE WHEN COL_LENGTH(@tabla,@columna) IS NOT NULL THEN 1 ELSE 0 END", conexion, transaccion);
            comando.Parameters.AddWithValue("@tabla", "dbo." + tabla);
            if (columna != null) comando.Parameters.AddWithValue("@columna", columna);
            return Convert.ToInt32(comando.ExecuteScalar()) == 1;
        }

        private static bool NecesitaActualizar(SqlConnection conexion, SqlTransaction? transaccion)
        {
            // Una base vacía se instala por el procedimiento habitual.
            if (!Existe(conexion, transaccion, "Cliente")) return false;
            return !InicializadorBaseDatos.EsquemaVigente(conexion, transaccion);
        }

        internal static void ActualizarSiNecesario(SqlConnection conexion)
        {
            if (!NecesitaActualizar(conexion, null)) return;
            string script = InicializadorBaseDatos.LeerScript("InstalarPrana.sql");
            // BACKUP no puede ejecutarse dentro de una transacción de usuario.
            string carpeta = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "PranaElectronica", "Backups");
            Directory.CreateDirectory(carpeta);
            string respaldo = Path.Combine(carpeta, "Prana-antes-1.0.5-" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + "-" + Guid.NewGuid().ToString("N") + ".bak");
            using (SqlCommand backup = new SqlCommand("BACKUP DATABASE " + new SqlCommandBuilder().QuoteIdentifier(conexion.Database)
                + " TO DISK=@ruta WITH COPY_ONLY, CHECKSUM", conexion))
            {
                backup.Parameters.AddWithValue("@ruta", respaldo);
                backup.CommandTimeout = 300;
                backup.ExecuteNonQuery();
            }

            using SqlTransaction transaccion = conexion.BeginTransaction();
            try
            {
                using (SqlCommand bloqueo = new SqlCommand(@"
                    SET XACT_ABORT ON;
                    DECLARE @resultado int;
                    EXEC @resultado=sys.sp_getapplock @Resource=N'Prana.ActualizarEsquema',
                        @LockMode=N'Exclusive',@LockOwner=N'Transaction',@LockTimeout=15000;
                    IF @resultado<0 THROW 50110,N'La base se está actualizando. Reintente.',1;", conexion, transaccion))
                    bloqueo.ExecuteNonQuery();

                if (NecesitaActualizar(conexion, transaccion))
                {
                    bool nombresActuales = Existe(conexion, transaccion, "Cliente", "numero_documento");
                    bool cajaActual = Existe(conexion, transaccion, "Caja", "id_caja");
                    if (!nombresActuales && !cajaActual)
                    {
                        // Completa las estructuras históricas antes de cambiar sus nombres.
                        Ejecutar(conexion, transaccion, Seccion(script, "ActualizarEstructuraPrana_LocalDB.sql", "CajaYComprobantes.sql"));
                        Ejecutar(conexion, transaccion, Seccion(script, "CajaYComprobantes.sql", "AclararNombresCaja.sql"));
                    }
                    if (!cajaActual)
                        Ejecutar(conexion, transaccion, Seccion(script, "AclararNombresCaja.sql", "Localidades.sql"));

                    ActualizarLocalidades(conexion, transaccion);
                    Ejecutar(conexion, transaccion, Seccion(script, "AclararNombresBaseDatos.sql", null));

                    if (NecesitaActualizar(conexion, transaccion) || !InicializadorBaseDatos.EsquemaVigente(conexion, transaccion))
                        throw new InvalidOperationException("No se pudo completar el esquema de la versión anterior.");
                }
                transaccion.Commit();
            }
            catch (Exception error)
            {
                if (transaccion.Connection != null) transaccion.Rollback();
                throw new InvalidOperationException("No se pudo actualizar la base anterior. Los cambios se revirtieron. Respaldo: " + respaldo + "\n\n" + error.Message, error);
            }
        }

        private static string Seccion(string script, string inicio, string? fin)
        {
            string marcador = "-- ===== " + inicio + " =====";
            int desde = script.IndexOf(marcador, StringComparison.Ordinal);
            if (desde < 0) throw new InvalidOperationException("Falta la etapa de actualización: " + inicio);
            int hasta = fin == null ? script.Length : script.IndexOf("-- ===== " + fin + " =====", desde + marcador.Length, StringComparison.Ordinal);
            if (hasta < 0) throw new InvalidOperationException("Falta el final de la etapa: " + inicio);
            return script.Substring(desde, hasta - desde);
        }

        private static void Ejecutar(SqlConnection conexion, SqlTransaction transaccion, string script)
            => InicializadorBaseDatos.EjecutarScript(conexion, transaccion, script);

        private static void ActualizarLocalidades(SqlConnection conexion, SqlTransaction transaccion)
        {
            Ejecutar(conexion, transaccion, @"
                IF OBJECT_ID(N'dbo.Localidad',N'U') IS NULL
                    CREATE TABLE dbo.Localidad(id_localidad int IDENTITY PRIMARY KEY,
                        nombre_localidad nvarchar(25) NOT NULL UNIQUE,
                        activo_localidad bit NOT NULL DEFAULT(1));
                IF COL_LENGTH(N'dbo.Localidad',N'activo_localidad') IS NULL
                    ALTER TABLE dbo.Localidad ADD activo_localidad bit NOT NULL DEFAULT(1) WITH VALUES;
                IF COL_LENGTH(N'dbo.Cliente',N'id_localidad') IS NULL
                    ALTER TABLE dbo.Cliente ADD id_localidad int NULL;
                GO
                IF COL_LENGTH(N'dbo.Cliente',N'localidad') IS NOT NULL
                    EXEC(N'
                        IF EXISTS(SELECT 1 FROM dbo.Cliente WHERE LEN(LTRIM(RTRIM(localidad)))>25)
                            THROW 50111,N''Hay localidades de más de 25 caracteres. No se truncaron los datos.'',1;
                        INSERT dbo.Localidad(nombre_localidad)
                        SELECT DISTINCT UPPER(LTRIM(RTRIM(c.localidad))) FROM dbo.Cliente c
                        WHERE NULLIF(LTRIM(RTRIM(c.localidad)),N'''') IS NOT NULL
                            AND NOT EXISTS(SELECT 1 FROM dbo.Localidad l WHERE l.nombre_localidad=UPPER(LTRIM(RTRIM(c.localidad))));
                        IF EXISTS(SELECT 1 FROM dbo.Cliente c JOIN dbo.Localidad l ON l.id_localidad=c.id_localidad
                            WHERE NULLIF(LTRIM(RTRIM(c.localidad)),N'''') IS NOT NULL
                              AND l.nombre_localidad<>UPPER(LTRIM(RTRIM(c.localidad))))
                            THROW 50112,N''La localidad del cliente tiene dos valores distintos. Revise los datos.'',1;
                        UPDATE c SET id_localidad=l.id_localidad FROM dbo.Cliente c JOIN dbo.Localidad l
                            ON l.nombre_localidad=UPPER(LTRIM(RTRIM(c.localidad))) WHERE c.id_localidad IS NULL;');
                IF NOT EXISTS(SELECT 1 FROM sys.foreign_key_columns
                    WHERE parent_object_id=OBJECT_ID(N'dbo.Cliente')
                      AND parent_column_id=COLUMNPROPERTY(OBJECT_ID(N'dbo.Cliente'),N'id_localidad',N'ColumnId')
                      AND referenced_object_id=OBJECT_ID(N'dbo.Localidad'))
                    ALTER TABLE dbo.Cliente WITH CHECK ADD CONSTRAINT FK_Cliente_Localidad
                        FOREIGN KEY(id_localidad) REFERENCES dbo.Localidad(id_localidad);
                ");
            // Conserva también las localidades usadas únicamente en entregas históricas.
            foreach (string tabla in new[] { "Envio", "Venta_Entrega", "Venta_Datos_Entrega" })
            {
                string columna = tabla == "Venta_Datos_Entrega" ? "localidad_sugerida" : "localidad";
                if (!Existe(conexion, transaccion, tabla, columna)) continue;
                string origen = "dbo." + new SqlCommandBuilder().QuoteIdentifier(tabla);
                string campo = new SqlCommandBuilder().QuoteIdentifier(columna);
                Ejecutar(conexion, transaccion, $@"
                    IF EXISTS(SELECT 1 FROM {origen} WHERE LEN(LTRIM(RTRIM({campo})))>25)
                        THROW 50111,N'Hay localidades de más de 25 caracteres. No se truncaron los datos.',1;
                    INSERT dbo.Localidad(nombre_localidad)
                    SELECT DISTINCT UPPER(LTRIM(RTRIM(o.{campo}))) FROM {origen} o
                    WHERE NULLIF(LTRIM(RTRIM(o.{campo})),N'') IS NOT NULL
                        AND NOT EXISTS(SELECT 1 FROM dbo.Localidad l WHERE l.nombre_localidad=UPPER(LTRIM(RTRIM(o.{campo}))));");
            }
            // La columna de texto anterior se conserva como dato histórico en las bases actualizadas.
        }
    }
}
