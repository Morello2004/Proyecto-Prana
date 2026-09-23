using System;
using System.Collections.Generic;

namespace Prana.Entidades
{
    // conserva la variante y el precio elegidos para una línea de venta
    public class DetalleNuevaVenta
    {
        public int IdVariante { get; set; }
        public string Producto { get; set; } = string.Empty;
        public string Variante { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
    }

    // separa el importe base del recargo para registrar el cobro real
    public class PagoNuevaVenta
    {
        public int IdMedioPago { get; set; }
        public string Medio { get; set; } = string.Empty;
        public decimal Monto { get; set; }
        public decimal Recargo { get; set; }
        public int Cuotas { get; set; }
    }

    // reúne los datos que se guardan al confirmar una venta
    public class NuevaVenta
    {
        public int IdCliente { get; set; }
        public int IdUsuario { get; set; }
        public decimal Descuento { get; set; }
        public decimal Total { get; set; }
        public string CondicionPago { get; set; } = string.Empty;
        public DateTime FechaPago { get; set; }
        public string ObservacionPago { get; set; } = string.Empty;
        public string ModalidadEntrega { get; set; } = string.Empty;
        public string DomicilioEntrega { get; set; } = string.Empty;
        public DateTime FechaEntrega { get; set; }
        public string HorarioSugerido { get; set; } = string.Empty;
        public string ObservacionesEntrega { get; set; } = string.Empty;
        public string TipoLogistica { get; set; } = string.Empty;
        public decimal CostoEnvio { get; set; }
        public bool GenerarRemito { get; set; }
        public List<DetalleNuevaVenta> Detalles { get; set; } = new();
        public List<PagoNuevaVenta> Pagos { get; set; } = new();
    }
}
