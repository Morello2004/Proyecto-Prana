using System;
using Prana.Datos;
using Prana.Entidades;
using System.Collections.Generic;
using System.Text;

namespace Prana.negocio
{
    // valida credenciales y coordina las operaciones de usuario
    public class CN_Usuario
    {
        // accede a los usuarios almacenados en la base de datos
        private readonly CD_Usuario datosUsuario = new CD_Usuario();
        // comprueba las credenciales y aplica el bloqueo de tres intentos por quince minutos
        public ResultadoInicioSesion IniciarSesion(string nombreUsuario, string contrasenaIngresada)
        {
            usuario? usuarioEncontrado = datosUsuario.BuscarUsuario(nombreUsuario.Trim());
            if (usuarioEncontrado == null || !usuarioEncontrado.activo)
            {
                return new ResultadoInicioSesion
                {
                    Mensaje = "Nombre de usuario o contrasena incorrectos."
                };
            }

            if (usuarioEncontrado.BloqueadoHasta > usuarioEncontrado.HoraServidor)
            {
                return new ResultadoInicioSesion
                {
                    BloqueadoHasta = usuarioEncontrado.BloqueadoHasta,
                    Mensaje = "La cuenta esta bloqueada por 15 minutos. Contactate con el administrador."

                };
            }

            if (!BCrypt.Net.BCrypt.Verify(contrasenaIngresada ?? string.Empty, usuarioEncontrado.contraseña))
            {
                (int intentos, DateTime? bloqueo) = datosUsuario.RegistrarIntentoFallido(usuarioEncontrado.idUsuario);
                int restantes = Math.Max(0, 3 - intentos);
                string mensaje;
                // comprueba si se recibio el valor opcional antes de utilizarlo
                if (bloqueo.HasValue)
                {
                    mensaje = "La contrasena es incorrecta. Se agotaron los 3 intentos. La cuenta queda bloqueada por 15 minutos. Contactate con el administrador.";
                }
                else
                {
                    mensaje = $"La contrasena es incorrecta. Te quedan {restantes} intentos.";
                }
                return new ResultadoInicioSesion
                {
                    IntentosRestantes = restantes,
                    BloqueadoHasta = bloqueo,
                    Mensaje = mensaje

                };
            }

            if (!datosUsuario.RestablecerIntentosLogin(usuarioEncontrado.idUsuario))
            {
                return new ResultadoInicioSesion
                {
                    Mensaje = "Nombre de usuario o contrasena incorrectos."
                };
            }

            return new ResultadoInicioSesion
            {
                Usuario = usuarioEncontrado,
                Autenticado = true

            };
        }

        // convierte una contrasena en un hash de bcrypt
        public string GenerarHash(string contrasena)
        {
            if (contrasena.Length < 5 || !contrasena.Any(char.IsUpper) || !contrasena.Any(char.IsDigit))
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new ArgumentException("La contrasena debe tener al menos 5 caracteres, una mayuscula y un numero.");
            }

            return BCrypt.Net.BCrypt.HashPassword(contrasena);
        }

        // obtiene la lista de usuarios para la administracion
        public List<usuario> ListarUsuarios() => datosUsuario.ListarUsuarios();
        // modifica el usuario y conserva el hash cuando no se informa una nueva contrasena
        public void ModificarUsuario(int id, string nombre, string contrasena, int rol)
        {
            nombre = nombre.Trim();
            if (id <= 0)
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new ArgumentException("Seleccione un usuario.");
            }

            // distingue un dato vacio de un valor ingresado antes de continuar
            if (string.IsNullOrWhiteSpace(nombre))
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new ArgumentException("El nombre de usuario es obligatorio.");
            }

            if (rol != 1 && rol != 2)
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new ArgumentException("Seleccione un rol válido.");
            }
            string valorSeleccionado;
            // distingue un dato vacio de un valor ingresado antes de continuar
            if (string.IsNullOrEmpty(contrasena))
            {
                valorSeleccionado = null;
            }
            else
            {
                valorSeleccionado = GenerarHash(contrasena);
            }

            datosUsuario.ModificarUsuario(id, nombre, valorSeleccionado, rol);
        }

        // valida los datos, genera el hash y crea un usuario nuevo
        public void CrearUsuario(string nombreUsuario, string contrasena, int idRolUsuario)
        {
            nombreUsuario = nombreUsuario.Trim();
            // distingue un dato vacio de un valor ingresado antes de continuar
            if (string.IsNullOrWhiteSpace(nombreUsuario))
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new ArgumentException("El nombre de usuario es obligatorio.");
            }

            // distingue un dato vacio de un valor ingresado antes de continuar
            if (string.IsNullOrWhiteSpace(contrasena))
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new ArgumentException("La contraseña es obligatoria.");
            }

            if (idRolUsuario != 1 && idRolUsuario != 2)
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new ArgumentException("El rol seleccionado no es válido.");
            }

            if (contrasena.Length < 5 || !contrasena.Any(char.IsUpper) || !contrasena.Any(char.IsDigit))
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new ArgumentException("La contraseña debe tener al menos 5 caracteres, una mayúscula y un número.");
            }

            string hashContrasena = GenerarHash(contrasena);
            datosUsuario.CrearUsuario(nombreUsuario, hashContrasena, idRolUsuario);
        }

        // desbloquea un usuario desde una sesion autenticada con permiso de administracion
        public void DesbloquearUsuario(int idUsuario)
        {
            Permisos.Exigir(Permisos.GestionarUsuarios);
            if (idUsuario <= 0)
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new ArgumentException("Seleccione un usuario valido.");
            }

            datosUsuario.DesbloquearUsuario(idUsuario, SesionActual.Usuario!.idUsuario);
        }

        // autoriza al administrador actual y solicita la baja logica de la cuenta
        public void ActivarUsuario(int idUsuario)
        {
            Permisos.Exigir(Permisos.GestionarUsuarios);
            if (idUsuario <= 0) throw new ArgumentException("Seleccione un usuario valido.");
            datosUsuario.ActivarUsuario(idUsuario, SesionActual.Usuario!.idUsuario);
        }

        public void DarDeBajaUsuario(int idUsuario)
        {
            Permisos.Exigir(Permisos.GestionarUsuarios);
            if (idUsuario <= 0)
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new ArgumentException("Seleccione un usuario valido.");
            }

            if (idUsuario == 1)
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new InvalidOperationException("La cuenta administradora principal no puede darse de baja.");
            }

            datosUsuario.DarDeBajaUsuario(idUsuario, SesionActual.Usuario!.idUsuario);
        }
    }
}
