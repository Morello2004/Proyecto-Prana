namespace Prana.Entidades
{
    // agrupa los datos y operaciones de log actividad
    public class LogActividad
    {
        // identifica el registro del log
        public int IdLog
        {
            get; set;
        }
        // identifica al usuario que realizo la accion identificador de la cuenta responsable de la operacion
        public int IdUsuario
        {
            get; set;
        }
        // muestra el nombre del usuario que realizo la accion
        public string NombreUsuario { get; set; } = string.Empty;
        // describe la accion realizada
        public string Accion { get; set; } = string.Empty;
        // indica que tipo de entidad fue afectada
        public string Entidad { get; set; } = string.Empty;
        // identifica el registro afectado
        public int? IdEntidad
        {
            get; set;
        }
        // guarda los detalles del cambio realizado
        public string? Detalle
        {
            get; set;
        }
        // guarda la fecha y hora de la actividad
        public DateTime Fecha
        {
            get; set;
        }
    }
}
