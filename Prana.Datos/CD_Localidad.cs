using Microsoft.Data.SqlClient;
using Prana.Entidades;
using System.Data;

namespace Prana.Datos;

public sealed class CD_Localidad
{
    // obtiene las localidades activas para sugerirlas en el combo
    public List<string> Obtener()
    {
        using SqlConnection conexion = new(Conexion.ObtenerCadena());
        using SqlCommand comando = new("SELECT nombre_localidad FROM dbo.Localidad WHERE activo_localidad=1 ORDER BY nombre_localidad", conexion);
        conexion.Open();
        using SqlDataReader lector = comando.ExecuteReader();
        List<string> localidades = new();
        while (lector.Read()) localidades.Add(lector.GetString(0));
        return localidades;
    }

    // incluye las localidades inactivas para administrarlas o reactivarlas
    public List<Localidad> ObtenerTodas()
    {
        using SqlConnection conexion = new(Conexion.ObtenerCadena());
        using SqlCommand comando = new("SELECT id_localidad,nombre_localidad,activo_localidad FROM dbo.Localidad ORDER BY activo_localidad DESC,nombre_localidad", conexion);
        conexion.Open();
        using SqlDataReader lector = comando.ExecuteReader();
        List<Localidad> localidades = new();
        while (lector.Read()) localidades.Add(new Localidad(lector.GetInt32(0), lector.GetString(1), lector.GetBoolean(2)));
        return localidades;
    }

    // agrega una localidad nueva o renombra una existente dentro de una transaccion
    public void Guardar(string? localidadActual, string localidadNueva)
    {
        using SqlConnection conexion = new(Conexion.ObtenerCadena());
        conexion.Open();
        using SqlTransaction transaccion = conexion.BeginTransaction(IsolationLevel.Serializable);
        try
        {
            // crea una opcion nueva y rechaza nombres duplicados
            if (string.IsNullOrWhiteSpace(localidadActual))
            {
                using SqlCommand insertar = new(@"
                    IF EXISTS (SELECT 1 FROM dbo.Localidad WHERE nombre_localidad=@nueva)
                        THROW 50002,N'La localidad ya existe; selecciónela para reactivarla si está inactiva.',1;
                    INSERT INTO dbo.Localidad(nombre_localidad,activo_localidad) VALUES(@nueva,1);", conexion, transaccion);
                insertar.Parameters.Add("@nueva", SqlDbType.NVarChar, 25).Value = localidadNueva;
                insertar.ExecuteNonQuery();
            }
            // verifica el origen y actualiza los textos historicos antes de cambiar el nombre
            else
            {
                using SqlCommand actualizar = new(@"
                    IF NOT EXISTS (SELECT 1 FROM dbo.Localidad WITH (UPDLOCK,HOLDLOCK) WHERE nombre_localidad=@actual)
                        THROW 50001,N'La localidad seleccionada ya no existe.',1;
                    IF EXISTS (SELECT 1 FROM dbo.Localidad WHERE nombre_localidad=@nueva)
                        THROW 50002,N'Ya existe una localidad con ese nombre.',1;
                    UPDATE dbo.Venta_Datos_Entrega SET localidad_sugerida=@nueva WHERE UPPER(LTRIM(RTRIM(localidad_sugerida)))=@actual;
                    UPDATE dbo.Envio SET localidad=@nueva WHERE UPPER(LTRIM(RTRIM(localidad)))=@actual;
                    UPDATE dbo.Localidad SET nombre_localidad=@nueva WHERE nombre_localidad=@actual;", conexion, transaccion);
                actualizar.Parameters.Add("@actual", SqlDbType.NVarChar, 25).Value = localidadActual;
                actualizar.Parameters.Add("@nueva", SqlDbType.NVarChar, 25).Value = localidadNueva;
                actualizar.ExecuteNonQuery();
            }

            transaccion.Commit();
        }
        catch
        {
            transaccion.Rollback();
            throw;
        }
    }

    // activa o desactiva una localidad sin borrar su identificador
    public void CambiarEstado(string localidad, bool activa)
    {
        using SqlConnection conexion = new(Conexion.ObtenerCadena());
        using SqlCommand comando = new("UPDATE dbo.Localidad SET activo_localidad=@activa WHERE nombre_localidad=@localidad; IF @@ROWCOUNT=0 THROW 50004,N'La localidad seleccionada ya no existe.',1;", conexion);
        comando.Parameters.Add("@activa", SqlDbType.Bit).Value = activa;
        comando.Parameters.Add("@localidad", SqlDbType.NVarChar, 25).Value = localidad;
        conexion.Open();
        comando.ExecuteNonQuery();
    }

    // busca o crea la localidad y devuelve su identificador para asociarlo al cliente
    internal static int? Asegurar(SqlConnection conexion, SqlTransaction transaccion, string? localidad)
    {
        if (string.IsNullOrWhiteSpace(localidad)) return null;
        using SqlCommand asegurar = new(@"
            IF EXISTS (SELECT 1 FROM dbo.Localidad WITH (UPDLOCK,HOLDLOCK) WHERE nombre_localidad=@localidad)
                UPDATE dbo.Localidad SET activo_localidad=1 WHERE nombre_localidad=@localidad AND activo_localidad=0;
            ELSE
                INSERT INTO dbo.Localidad(nombre_localidad,activo_localidad) VALUES(@localidad,1);
            SELECT id_localidad FROM dbo.Localidad WHERE nombre_localidad=@localidad;", conexion, transaccion);
        asegurar.Parameters.Add("@localidad", SqlDbType.NVarChar, 25).Value = localidad;
        return Convert.ToInt32(asegurar.ExecuteScalar());
    }
}
