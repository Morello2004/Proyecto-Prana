using Microsoft.Data.SqlClient;
using Prana.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace Prana.Datos
{
    // agrupa los datos y operaciones de cd marca
    public class CD_Marca
    {
        // consulta obtener marcas y transforma cada fila recibida en los objetos que devuelve
        public List<Marca> obtenerMarcas()
        {
            // crea la lista donde se guardaran las marcas
            List<Marca> marcas = new List<Marca>();
            // consulta todas las marcas registradas
            string consulta = @"
                SELECT
                    id_marca,
                    nombre_marca
                FROM Marca
                ORDER BY nombre_marca";
            // crea la conexion con sql server
            using SqlConnection conexion = new SqlConnection(Conexion.ObtenerCadena());
            // prepara la consulta y su conexion los valores variables se envian mediante parametros
            using SqlCommand comando = new SqlCommand(consulta, conexion);
            // abre la conexion abre el recurso antes de usarlo; una conexion debe estar abierta para enviar consultas
            conexion.Open();
            // ejecuta la consulta y obtiene los resultados
            using SqlDataReader lector = comando.ExecuteReader();
            // recorre cada marca devuelta por sql avanza el lector de sql una fila por vez hasta agotar el resultado
            while (lector.Read())
            {
                // crea una marca con los datos de la fila actual
                Marca marcaEncontrada = new Marca
                {
                    IdMarca = lector.GetInt32(lector.GetOrdinal("id_marca")),
                    NombreMarca = lector.GetString(lector.GetOrdinal("nombre_marca"))

                };
                // agrega la marca a la lista
                marcas.Add(marcaEncontrada);
            }

            // devuelve todas las marcas
            return marcas;
        }
    }
}
