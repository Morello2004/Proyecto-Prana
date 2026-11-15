using Microsoft.Data.SqlClient;
using System;
using System.Data;

namespace Prana.Datos
{
    // agrupa los datos y operaciones de cd localidad
    public class CD_Localidad
    {
        // busca una localidad existente o crea una nueva
        public int ObtenerOCrearLocalidad(string nombreLocalidad, SqlConnection conexion, SqlTransaction transaccion)
        {
            string consultaBuscar = "SELECT id_localidad FROM Localidad WHERE UPPER(LTRIM(RTRIM(nombre_localidad))) = UPPER(@NombreLocalidad)";
            // crea el comando que ejecutara la busqueda
            using (SqlCommand comandoBuscar = new SqlCommand(consultaBuscar, conexion, transaccion))
            {
                comandoBuscar.Parameters.Add("@NombreLocalidad", SqlDbType.NVarChar, 100).Value = nombreLocalidad;
                // ejecuta la consulta y obtiene el identificador si la localidad existe
                object? resultado = comandoBuscar.ExecuteScalar();
                // comprueba si se encontro la localidad
                if (resultado != null)
                {
                    // devuelve el identificador de la localidad encontrada
                    return Convert.ToInt32(resultado);
                }
            }

            string consultaInsertar = "INSERT INTO Localidad (nombre_localidad, codigo_postal) OUTPUT INSERTED.id_localidad VALUES (@NombreLocalidad, NULL)";
            // crea el comando que insertara la nueva localidad
            using SqlCommand comandoInsertar = new SqlCommand(consultaInsertar, conexion, transaccion);
            comandoInsertar.Parameters.Add("@NombreLocalidad", SqlDbType.NVarChar, 100).Value = nombreLocalidad;
            // crea la localidad y devuelve el identificador generado
            return Convert.ToInt32(comandoInsertar.ExecuteScalar());
        }
    }
}
