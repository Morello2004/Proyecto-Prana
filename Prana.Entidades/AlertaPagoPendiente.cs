using System;

namespace Prana.Entidades
{
    public class AlertaPagoPendiente
    {
        public int IdVenta { get; set; }
        public string Cliente { get; set; } = string.Empty;
        public string Productos { get; set; } = string.Empty;
        public DateTime PrimerPago { get; set; }
        public decimal Total { get; set; }
        public decimal Pagado { get; set; }
        public decimal Saldo { get; set; }
        public int DiasVencida { get; set; }
    }
}
