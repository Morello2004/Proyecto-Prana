using System;
using System.Collections.Generic;
using System.Text;

namespace Prana.Entidades
{
    public class venta
    {
        public int IdVenta { get; set; }

        //Esta línea significa que cada Venta tiene un objeto Cliente asociado
        public Cliente Cliente { get; set; } = new Cliente();
        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }
        public string Estado { get; set; } = string.Empty;
    }
}
