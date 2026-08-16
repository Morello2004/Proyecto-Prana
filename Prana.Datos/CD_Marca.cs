using Microsoft.Data.SqlClient;
using Prana.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace Prana.Datos
{
    public class CD_Marca
    {
        public List<Marca> obtenerMarcas()
        {
            // Crea la lista donde se guardarán las marcas
            List<Marca> marcas = new List<Marca>();

            // Consulta todas las marcas registradas
            string consulta = @"
                SELECT
                    id_marca,
                    nombre_marca
                FROM Marca
                ORDER BY nombre_marca";

            // Crea la conexión con SQL Server
            using SqlConnection conexion =
                new SqlConnection(Conexion.ObtenerCadena());

            // Prepara la consulta para ejecutarla
            using SqlCommand comando =
                new SqlCommand(consulta, conexion);

            // Abre la conexión
            conexion.Open();

            // Ejecuta la consulta y obtiene los resultados
            using SqlDataReader lector = comando.ExecuteReader();

            // Recorre cada marca devuelta por SQL
            while (lector.Read())
            {
                // Crea una marca con los datos de la fila actual
                Marca marcaEncontrada = new Marca
                {
                    IdMarca = lector.GetInt32(
                        lector.GetOrdinal("id_marca")
                    ),

                    NombreMarca = lector.GetString(
                        lector.GetOrdinal("nombre_marca")
                    )
                };

                // Agrega la marca a la lista
                marcas.Add(marcaEncontrada);
            }

            // Devuelve todas las marcas
            return marcas;
        }
    }
}
