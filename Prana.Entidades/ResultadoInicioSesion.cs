namespace Prana.Entidades
{
    // describe el resultado de una autenticacion y los intentos disponibles representa resultado inicio sesion agrupa los datos que intercambian las consultas, las reglas de negocio y las pantallas
    public class ResultadoInicioSesion
    {
        // conserva el usuario cuando la autenticacion finaliza correctamente
        public usuario? Usuario
        {
            get; set;
        }
        // indica si las credenciales permitieron iniciar sesion
        public bool Autenticado
        {
            get; set;
        }
        // conserva el mensaje que debe presentar la interfaz
        public string Mensaje { get; set; } = string.Empty;
        // conserva la cantidad de intentos restantes antes del bloqueo
        public int? IntentosRestantes
        {
            get; set;
        }
        // conserva el momento en que finaliza el bloqueo temporal fecha y hora hasta la que se impide el acceso por intentos fallidos
        public DateTime? BloqueadoHasta
        {
            get; set;
        }
    }
}
