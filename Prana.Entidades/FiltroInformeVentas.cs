using System;

namespace Prana.Entidades
{
    public class FiltroInformeVentas
    {
        public DateTime Desde { get; set; }
        public DateTime Hasta { get; set; }
        public string Producto { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public string Marca { get; set; } = string.Empty;
        public string Cliente { get; set; } = string.Empty;
        public string Envio { get; set; } = string.Empty;
        public string Ubicacion { get; set; } = string.Empty;
        public decimal? MontoMinimo { get; set; }
        public decimal? MontoMaximo { get; set; }
    }
}
