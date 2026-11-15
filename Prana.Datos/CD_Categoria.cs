using Microsoft.Data.SqlClient;
using Prana.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace Prana.Datos
{
    // agrupa los datos y operaciones de cd categoria
    public class CD_Categoria
    {
        // consulta obtener categorias y transforma cada fila recibida en los objetos que devuelve
        public List<Categoria> obtenerCategorias()
        {
            // crea la lista donde se guardaran las categorias
            List<Categoria> categorias = new List<Categoria>();
            string consulta = @"
        SELECT
            id_categoria,
            nombre_categoria
        FROM Categoria
        ORDER BY nombre_categoria";
            // crea la conexion con sql server
            using SqlConnection conexion = new SqlConnection(Conexion.ObtenerCadena());
            // crea el comando que contiene la consulta
            using SqlCommand comando = new SqlCommand(consulta, conexion);
            // abre la conexion abre el recurso antes de usarlo; una conexion debe estar abierta para enviar consultas
            conexion.Open();
            // ejecuta la consulta
            using SqlDataReader lector = comando.ExecuteReader();
            // recorre todas las categorias devueltas avanza el lector de sql una fila por vez hasta agotar el resultado
            while (lector.Read())
            {
                // crea una categoria con los datos de la fila actual
                Categoria categoriaEncontrada = new Categoria
                {
                    IdCategoria = lector.GetInt32(lector.GetOrdinal("id_categoria")),
                    NombreCategoria = lector.GetString(lector.GetOrdinal("nombre_categoria"))

                };
                // agrega la categoria a la lista
                categorias.Add(categoriaEncontrada);
            }

            // devuelve todas las categorias
            return categorias;
        }
    }
}
