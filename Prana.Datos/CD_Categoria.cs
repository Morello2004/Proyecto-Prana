using Microsoft.Data.SqlClient;
using Prana.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace Prana.Datos
{
    public class CD_Categoria
    {

        public List<Categoria> obtenerCategorias()
        {
            // Crea la lista donde se guardarán las categorías
            List<Categoria> categorias = new List<Categoria>();

            string consulta = @"
        SELECT
            id_categoria,
            nombre_categoria
        FROM Categoria
        ORDER BY nombre_categoria";

            // Crea la conexión con SQL Server
            using SqlConnection conexion =
                new SqlConnection(Conexion.ObtenerCadena());

            // Crea el comando que contiene la consulta
            using SqlCommand comando =
                new SqlCommand(consulta, conexion);

            // Abre la conexión
            conexion.Open();

            // Ejecuta la consulta
            using SqlDataReader lector = comando.ExecuteReader();

            // Recorre todas las categorías devueltas
            while (lector.Read())
            {
                // Crea una categoría con los datos de la fila actual
                Categoria categoriaEncontrada = new Categoria
                {
                    IdCategoria = lector.GetInt32(
                        lector.GetOrdinal("id_categoria")
                    ),

                    NombreCategoria = lector.GetString(
                        lector.GetOrdinal("nombre_categoria")
                    )
                };

                // Agrega la categoría a la lista
                categorias.Add(categoriaEncontrada);
            }

            // Devuelve todas las categorías
            return categorias;
        }
    }
}
