using System;
using System.Collections.Generic;
using System.Text;

namespace Prana.Entidades
{
    public class Localidad
    {
        public int IdLocalidad { get; set; }
        public string NombreLocalidad { get; set; } = string.Empty;
        public string CodigoPostal { get; set; } = string.Empty;
    }
}
