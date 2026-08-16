using System;
using Prana.Datos;
using Prana.Entidades;
using System.Collections.Generic;
using System.Text;


namespace Prana.negocio
{
    public class CN_Usuario
    {
        // Se crea un objeto de la capa de Datos para poder buscar usuarios almacenados en SQL Server
    
        private readonly CD_Usuario datosUsuario = new CD_Usuario();

        // Recibe el nombre y la contraseña escritos en el formulario
        // Devuelve el usuario si los datos son correctos
        // Devuelve null si el usuario no existe, está inactivo  o la contraseña es incorrecta
       
        public usuario? IniciarSesion(
            string nombreUsuario,
            string contrasenaIngresada)
        {
            // Le solicita a la capa de Datos que busque al usuario mediante su nombre de usuario
            usuario? usuario =
                datosUsuario.BuscarUsuario(nombreUsuario);

            // Si Datos no encontró ningún usuario, devuelve null
            if (usuario == null)
            {
                return null;
            }

            // Comprueba que el usuario esté habilitado para utilizar el sistema. Si Activo es false, no permite el ingreso
            if (!usuario.activo)
            {
                return null;
            }

            // BCrypt compara la contraseña escrita en el formulario con el hash almacenado en la base de datos.
            // No desencripta el hash. Utiliza la información incluida en el propio hash para comprobar si corresponden.
            bool contrasenaCorrecta = BCrypt.Net.BCrypt.Verify(
                contrasenaIngresada,
                usuario.contraseña
            );

            // Si BCrypt indica que no coinciden, devuelve null.
            if (!contrasenaCorrecta)
            {
                return null;
            }

            // Si el usuario existe, está activo y la contraseña
            // coincide, devuelve el objeto Usuario a Presentación.
            return usuario;
        }

        // Convierte una contraseña normal en un hash de BCrypt.
        // Este método se utiliza al crear un usuario o cuando se modifica su contraseña.
        public string GenerarHash(string contrasena)
        {
            // No se permite generar un hash de una contraseña vacía
            if (string.IsNullOrWhiteSpace(contrasena))
            {
                throw new ArgumentException(
                    "La contraseña no puede estar vacía."
                );
            }

            // Devuelve el hash que posteriormente debe almacenarse en la columna hash_contrasena de SQL Server.
            return BCrypt.Net.BCrypt.HashPassword(contrasena);
        }
    }
}
  
