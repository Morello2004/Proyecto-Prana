using System;
using System.Collections.Generic;
using System.Text;

namespace Prana.Entidades
{
    // agrupa los datos y operaciones de categoria
    public class Categoria
    {
        // conserva id categoria como int para intercambiar este dato entre las capas
        public int IdCategoria
        {
            get; set;
        }
        // conserva nombre categoria como string para intercambiar este dato entre las capas
        public string NombreCategoria { get; set; } = string.Empty;
    }
}
