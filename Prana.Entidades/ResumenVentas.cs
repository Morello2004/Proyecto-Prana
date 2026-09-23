namespace Prana.Entidades
{
    // Datos de consulta del panel; no modifica ventas ni pagos.
    public class ResumenVentas
    {
        public decimal TotalHoy { get; set; }
        public int CantidadHoy { get; set; }
        public decimal SaldoPendiente { get; set; }
        public int VentasPendientes { get; set; }
    }
}
