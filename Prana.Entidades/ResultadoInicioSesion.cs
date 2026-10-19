namespace Prana.Entidades
{
    // Describe el resultado de una autenticacion y los intentos disponibles.
    public class ResultadoInicioSesion
    {
        // Conserva el usuario cuando la autenticacion finaliza correctamente.
        public usuario? Usuario { get; set; }

        // Indica si las credenciales permitieron iniciar sesion.
        public bool Autenticado { get; set; }

        // Conserva el mensaje que debe presentar la interfaz.
        public string Mensaje { get; set; } = string.Empty;

        // Conserva la cantidad de intentos restantes antes del bloqueo.
        public int? IntentosRestantes { get; set; }

        // Conserva el momento en que finaliza el bloqueo temporal.
        public DateTime? BloqueadoHasta { get; set; }
    }
}
