using System;
using System.Collections.Generic;
using System.Text;

namespace Prana.Entidades
{
    // agrupa los datos y operaciones de marca
    public class Marca
    {
        // conserva id marca como int para intercambiar este dato entre las capas
        public int IdMarca
        {
            get; set;
        }
        // conserva nombre marca como string para intercambiar este dato entre las capas
        public string NombreMarca { get; set; } = string.Empty;
    }
}
