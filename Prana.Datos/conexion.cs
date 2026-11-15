using System;
using System.Configuration;
using System.Runtime.Intrinsics.X86;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Prana.Datos
{ // se crea la clase conexion para que todas las clases obtengan la cadena de conexion a la base de datos desde el archivo appconfig
    // agrupa los datos y operaciones de conexion
    public static class Conexion
    {
        // resuelve obtener cadena con los datos recibidos y entrega el resultado al codigo que lo llama
        public static string ObtenerCadena()
        {
            string? cadena = ConfigurationManager // es una clase que permite leer y acceder a la configuracion de la aplicacion guardada en appconfig
            .ConnectionStrings["ConexionPrana"]? // accede a la seccion y busca la conexion llamada "conexionprana"
            .ConnectionString; // obtiene el texto almacenado en connectionstring="" comprueba que la conexion exista y no este vacia
            // distingue un dato vacio de un valor ingresado antes de continuar
            if (string.IsNullOrWhiteSpace(cadena))
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new InvalidOperationException("No se encontró la conexión ConexionPrana en App.config.");
            }

            return cadena;
        }
    }
}
