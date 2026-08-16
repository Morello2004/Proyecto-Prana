using Microsoft.Data.SqlClient;
using System;
using System.Data;

namespace Prana.Datos
{
    public class CD_Localidad
    {
        // busca una localidad existente o crea una nueva
        public int ObtenerOCrearLocalidad(string nombreLocalidad, SqlConnection conexion, SqlTransaction transaccion)
        {
            // prepara la consulta para buscar la localidad por su nombre
            string consultaBuscar = "SELECT id_localidad FROM Localidad WHERE nombre_localidad = @NombreLocalidad";

            // crea el comando que ejecutara la busqueda
            using (SqlCommand comandoBuscar = new SqlCommand(consultaBuscar, conexion, transaccion))
            {
                // envia el nombre de la localidad a la consulta
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

            // prepara la consulta para crear una localidad y devolver su identificador
            string consultaInsertar = "INSERT INTO Localidad (nombre_localidad, codigo_postal) OUTPUT INSERTED.id_localidad VALUES (@NombreLocalidad, NULL)";

            // crea el comando que insertara la nueva localidad
            using SqlCommand comandoInsertar = new SqlCommand(consultaInsertar, conexion, transaccion);

            // envia el nombre de la nueva localidad a la consulta
            comandoInsertar.Parameters.Add("@NombreLocalidad", SqlDbType.NVarChar, 100).Value = nombreLocalidad;

            // crea la localidad y devuelve el identificador generado
            return Convert.ToInt32(comandoInsertar.ExecuteScalar());
        }
    }
}