using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Prana.Entidades;
using Microsoft.Data.SqlClient;

namespace Prana.Datos
{
    public class CD_Usuario
    {
        // Busca un usuario en SQL Server utilizando su nombre
        // Devuelve un objeto Usuario si lo encuentra
        // Devuelve null si no existe
        public usuario? BuscarUsuario(string nombreUsuario)
        {
            //guardo en un string la consulta sql para buscar el usuario por nombre en la BD
            string consulta = @"
                                SELECT
                                    id_usuario,
                                     nombre_usuario,
                                     email,
                                     hash_contrasena,
                                     id_rol_usuario,
                                     activo
                                 FROM Usuario
                                 WHERE nombre_usuario = @NombreUsuario;";


            // Crea la conexión usando la cadena obtenida desde App.config
            // El using cerrará y liberará automáticamente la conexión
            using SqlConnection conexion = new SqlConnection(Conexion.ObtenerCadena());

            // Prepara la consulta para ejecutarla mediante la conexión
            using SqlCommand comando = new SqlCommand(consulta, conexion);


            // Reemplaza @NombreUsuario con el nombre recibido.
            // Se especifica que SQL Server recibirá un NVARCHAR de 100 caracteres
            comando.Parameters.Add("@NombreUsuario", SqlDbType.NVarChar, 100).Value = nombreUsuario;

            // Abre la conexión con SQL Server.
            conexion.Open();

            // Ejecuta el SELECT y obtiene un lector con los resultados
            using SqlDataReader lector = comando.ExecuteReader();

            // Read intenta leer la primera fila
            // Si devuelve false significa que no se encontró el usuario
            if (!lector.Read())
            {
                return null;
            }

            //crea un objeto Usuario con los valores encontrados

            usuario usuario = new usuario {
                idUsuario = lector.GetInt32(lector.GetOrdinal("id_usuario")),
            nombreUsuario = lector.GetString(lector.GetOrdinal("nombre_usuario")),
            mail = lector.GetString(lector.GetOrdinal("email")),
            contraseña = lector.GetString(lector.GetOrdinal("hash_contrasena")),
                IdRolUsuario = lector.GetInt32(lector.GetOrdinal("id_rol_usuario")),
                activo = lector.GetBoolean(lector.GetOrdinal("activo"))
            };

            return usuario;
        }
    }
}
