using System;
using System.Collections.Generic;
using System.Text;

namespace Prana.Entidades
{
    public class Proveedor
    {
        public int idProveedor { get; set; }
        public string nombreProveedor { get; set; } = string.Empty;// con esto se inicializa "" para evitar errores al comprender un posible valor null

        public string? mail { get; set; }

        public string? Telefono { get; set; }

    }
}
