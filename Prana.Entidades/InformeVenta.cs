using System;

namespace Prana.Entidades
{
    public class InformeVenta
    {
        public int IdVenta { get; set; }
        public DateTime Fecha { get; set; }
        public string Cliente { get; set; } = string.Empty;
        public string Ubicacion { get; set; } = string.Empty;
        public string Productos { get; set; } = string.Empty;
        public string Categorias { get; set; } = string.Empty;
        public string Marcas { get; set; } = string.Empty;
        public decimal Total { get; set; }
        public decimal Pagado { get; set; }
        public decimal Saldo { get; set; }
        public string Envio { get; set; } = string.Empty;
    }
}
