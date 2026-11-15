namespace Prana.Entidades
{
    // agrupa los datos y operaciones de usuario
    public class usuario
    {
        // identifica al usuario en la base de datos identificador de la cuenta responsable de la operacion
        public int idUsuario
        {
            get; set;
        }
        // conserva el nombre utilizado para iniciar sesion
        public string nombreUsuario { get; set; } = string.Empty;
        // conserva el hash de la contrasena almacenada hash de la contrasena almacenada; no contiene la contrasena legible
        public string contraseña { get; set; } = string.Empty;
        // identifica el rol asignado al usuario
        public int IdRolUsuario
        {
            get; set;
        }
        // indica si el usuario puede acceder al sistema marca si el registro esta habilitado para el uso habitual; no elimina su historial
        public bool activo
        {
            get; set;
        }
        // muestra el estado de la cuenta junto al nombre en la lista administrativa dato nombre para mostrar de tipo string se obtiene al consultar la propiedad mediante la expresion siguiente
        public string NombreParaMostrar
        {
            get
            {
                if (idUsuario == 0 || activo)
                {
                    return nombreUsuario;
                }
                else
                {
                    return nombreUsuario + " (dado de baja)";
                }
            }
        }

        // conserva la cantidad de intentos fallidos consecutivos contador de contrasenas incorrectas consecutivas empleado para el bloqueo temporal
        public int IntentosLoginFallidos
        {
            get; set;
        }
        // conserva el fin del bloqueo temporal cuando la cuenta esta bloqueada fecha y hora hasta la que se impide el acceso por intentos fallidos
        public DateTime? BloqueadoHasta
        {
            get; set;
        }
        // conserva la hora de la base utilizada para comparar el bloqueo de acceso
        public DateTime HoraServidor
        {
            get; set;
        }
        // indica si el usuario esta bloqueado al consultar la lista de administracion
        public bool EstaBloqueado
        {
            get; set;
        }
    }
}
