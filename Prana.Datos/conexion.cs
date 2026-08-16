using System;
using System.Configuration;
using System.Runtime.Intrinsics.X86;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Prana.Datos
{// se crea la clase conexion para que todas las clases obtengan la cadena de conexion a la base de datos desde el archivo App.config
    public static class Conexion
    {
        public static string ObtenerCadena()
        {
            string? cadena = ConfigurationManager //es una clase que permite leer y acceder a la configuración de la aplicación guardada en App.config
                .ConnectionStrings["ConexionPrana"]? //Accede a la sección<connectionStrings> y busca la conexión llamada "ConexionPrana"
                .ConnectionString;// Obtiene el texto almacenado en connectionString="..."

            // Comprueba que la conexión exista y no esté vacía
            if (string.IsNullOrWhiteSpace(cadena))
            {
                throw new InvalidOperationException(
                    "No se encontró la conexión ConexionPrana en App.config."
                );
            }

            return cadena;
        }
    }
}