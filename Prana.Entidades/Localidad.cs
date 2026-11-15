using System;
using System.Collections.Generic;
using System.Text;

namespace Prana.Entidades
{
    // agrupa los datos y operaciones de localidad
    public class Localidad
    {
        // conserva id localidad como int para intercambiar este dato entre las capas
        public int IdLocalidad
        {
            get; set;
        }
        // conserva nombre localidad como string para intercambiar este dato entre las capas
        public string NombreLocalidad { get; set; } = string.Empty;
        // codigo postal como texto, para admitir tanto numeros como letras del cpa
        public string CodigoPostal { get; set; } = string.Empty;
    }
}
