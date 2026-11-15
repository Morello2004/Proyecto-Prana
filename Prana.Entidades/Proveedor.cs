using System;
using System.Collections.Generic;
using System.Text;

namespace Prana.Entidades
{
    // agrupa los datos y operaciones de proveedor
    public class Proveedor
    {
        // conserva id proveedor como int para intercambiar este dato entre las capas
        public int idProveedor
        {
            get; set;
        }
        // conserva nombre proveedor como string para intercambiar este dato entre las capas
        public string nombreProveedor { get; set; } = string.Empty; // con esto se inicializa "" para evitar errores al comprender un posible valor null
        // conserva telefono como string? para intercambiar este dato entre las capas
        public string? Telefono
        {
            get; set;
        }
    }
}
