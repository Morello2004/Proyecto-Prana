namespace Prana.Entidades
{
    // Representa un usuario y el estado de sus intentos de inicio de sesion.
    public class usuario
    {
        // Identifica al usuario en la base de datos.
        public int idUsuario { get; set; }

        // Conserva el nombre utilizado para iniciar sesion.
        public string nombreUsuario { get; set; } = string.Empty;

        // Conserva el correo asociado al usuario.
        public string mail { get; set; } = string.Empty;

        // Conserva el hash de la contrasena almacenada.
        public string contraseña { get; set; } = string.Empty;

        // Identifica el rol asignado al usuario.
        public int IdRolUsuario { get; set; }

        // Indica si el usuario puede acceder al sistema.
        public bool activo { get; set; }

        // Conserva la cantidad de intentos fallidos consecutivos.
        public int IntentosLoginFallidos { get; set; }

        // Conserva el fin del bloqueo temporal cuando la cuenta esta bloqueada.
        public DateTime? BloqueadoHasta { get; set; }

        // Conserva la hora de la base utilizada para comparar el bloqueo de acceso.
        public DateTime HoraServidor { get; set; }

        // Indica si el usuario esta bloqueado al consultar la lista de administracion.
        public bool EstaBloqueado { get; set; }
    }
}
