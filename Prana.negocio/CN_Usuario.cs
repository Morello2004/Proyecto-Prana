using System;
using Prana.Datos;
using Prana.Entidades;
using System.Collections.Generic;
using System.Text;


namespace Prana.negocio
{
    // Valida credenciales y coordina las operaciones de usuario.
    public class CN_Usuario
    {
        // Accede a los usuarios almacenados en la base de datos.
        private readonly CD_Usuario datosUsuario = new CD_Usuario();

        // Comprueba las credenciales y aplica el bloqueo de tres intentos por quince minutos.
        public ResultadoInicioSesion IniciarSesion(string nombreUsuario, string contrasenaIngresada)
        {
            usuario? usuarioEncontrado = datosUsuario.BuscarUsuario(nombreUsuario.Trim());
            if (usuarioEncontrado == null || !usuarioEncontrado.activo)
                return new ResultadoInicioSesion { Mensaje = "Nombre de usuario o contrasena incorrectos." };

            if (usuarioEncontrado.BloqueadoHasta > usuarioEncontrado.HoraServidor)
                return new ResultadoInicioSesion
                {
                    BloqueadoHasta = usuarioEncontrado.BloqueadoHasta,
                    Mensaje = "La cuenta esta bloqueada por 15 minutos. Contactate con el administrador."
                };

            if (!BCrypt.Net.BCrypt.Verify(contrasenaIngresada ?? string.Empty, usuarioEncontrado.contraseña))
            {
                (int intentos, DateTime? bloqueo) = datosUsuario.RegistrarIntentoFallido(usuarioEncontrado.idUsuario);
                int restantes = Math.Max(0, 3 - intentos);
                string mensaje = bloqueo.HasValue
                    ? "La contrasena es incorrecta. Se agotaron los 3 intentos. La cuenta queda bloqueada por 15 minutos. Contactate con el administrador."
                    : $"La contrasena es incorrecta. Te quedan {restantes} intentos.";
                return new ResultadoInicioSesion { IntentosRestantes = restantes, BloqueadoHasta = bloqueo, Mensaje = mensaje };
            }

            if (!datosUsuario.RestablecerIntentosLogin(usuarioEncontrado.idUsuario))
                return new ResultadoInicioSesion { Mensaje = "Nombre de usuario o contrasena incorrectos." };

            return new ResultadoInicioSesion { Usuario = usuarioEncontrado, Autenticado = true };
        }

        // Convierte una contrasena en un hash de BCrypt.
        public string GenerarHash(string contrasena)
        {
            if (contrasena.Length < 5 || !contrasena.Any(char.IsUpper) || !contrasena.Any(char.IsDigit))
                throw new ArgumentException("La contrasena debe tener al menos 5 caracteres, una mayuscula y un numero.");
            return BCrypt.Net.BCrypt.HashPassword(contrasena);
        }

        // Obtiene la lista de usuarios para la administracion.
        public List<usuario> ListarUsuarios() => datosUsuario.ListarUsuarios();

        // Modifica el usuario y conserva el hash cuando no se informa una nueva contrasena.
        public void ModificarUsuario(int id, string nombre, string contrasena, int rol)
        {
            nombre = nombre.Trim();
            if (id <= 0) throw new ArgumentException("Seleccione un usuario.");
            if (string.IsNullOrWhiteSpace(nombre) || nombre.Length > 100) throw new ArgumentException("El nombre debe tener entre 1 y 100 caracteres.");
            if (rol != 1 && rol != 2) throw new ArgumentException("Seleccione un rol válido.");
            datosUsuario.ModificarUsuario(id, nombre, string.IsNullOrEmpty(contrasena) ? null : GenerarHash(contrasena), rol);
        }

        // Valida los datos, genera el hash y crea un usuario nuevo.
        public void CrearUsuario(string nombreUsuario, string contrasena, int idRolUsuario)
        {
            if (string.IsNullOrWhiteSpace(nombreUsuario)) throw new ArgumentException("El nombre de usuario es obligatorio.");
            if (string.IsNullOrWhiteSpace(contrasena)) throw new ArgumentException("La contraseña es obligatoria.");
            if (idRolUsuario != 1 && idRolUsuario != 2) throw new ArgumentException("El rol seleccionado no es válido.");
            if (contrasena.Length < 5 || !contrasena.Any(char.IsUpper) || !contrasena.Any(char.IsDigit))
                throw new ArgumentException("La contraseña debe tener al menos 5 caracteres, una mayúscula y un número.");
            string hashContrasena = GenerarHash(contrasena);
            datosUsuario.CrearUsuario(nombreUsuario, hashContrasena, idRolUsuario);
        }

        // Desbloquea un usuario desde una sesion autenticada con permiso de administracion.
        public void DesbloquearUsuario(int idUsuario)
        {
            CN_Pago.Verificar(Permisos.GestionarUsuarios);
            if (idUsuario <= 0) throw new ArgumentException("Seleccione un usuario valido.");
            datosUsuario.DesbloquearUsuario(idUsuario, SesionActual.Usuario!.idUsuario);
        }
    }
}
  
