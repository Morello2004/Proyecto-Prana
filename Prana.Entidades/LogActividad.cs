namespace Prana.Entidades
{
    // Representa una actividad realizada por un usuario.
    public class LogActividad
    {
        // Identifica el registro del log.
        public int IdLog { get; set; }

        // Identifica al usuario que realizó la acción.
        public int IdUsuario { get; set; }

        // Muestra el nombre del usuario que realizó la acción.
        public string NombreUsuario { get; set; } = string.Empty;

        // Describe la acción realizada.
        public string Accion { get; set; } = string.Empty;

        // Indica qué tipo de entidad fue afectada.
        public string Entidad { get; set; } = string.Empty;

        // Identifica el registro afectado.
        public int? IdEntidad { get; set; }

        // Guarda los detalles del cambio realizado.
        public string? Detalle { get; set; }

        // Guarda la fecha y hora de la actividad.
        public DateTime Fecha { get; set; }
    }
}
