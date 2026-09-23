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
        // Propiedades planas para enlazar las columnas de Windows Forms.
        public string NombreCompletoCliente => $"{Cliente.Nombre} {Cliente.Apellido}";
        public decimal Pagado { get; set; }
        public decimal SaldoPendiente { get; set; }
        public string Productos { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        // Se consulta por separado del estado de pago de la venta.
        public string EstadoEntrega { get; set; } = string.Empty;
    }
}
