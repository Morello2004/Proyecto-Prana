namespace Prana.Entidades
{
    // Datos de consulta y solicitudes entre presentación, negocio y datos.
    public class PagoRegistrado
    {
        public int IdPago { get; set; }
        public DateTime Fecha { get; set; }
        public string Medio { get; set; } = "";
        public int Cuotas { get; set; }
        public decimal ImporteBase { get; set; }
        public decimal Cobrado { get; set; }
    }
    public class CobroVenta
    {
        public int IdVenta { get; set; }
        public int IdMedioPago { get; set; }
        public int Cuotas { get; set; }
        public decimal ImporteBase { get; set; }
        public decimal Porcentaje { get; set; }
        public decimal Recargo { get; set; }
    }
    public class ProductoPorEntregar
    {
        public int IdVenta { get; set; }
        public int IdVariante { get; set; }
        public string Cliente { get; set; } = "";
        public string Producto { get; set; } = "";
        public string Modalidad { get; set; } = "";
        public int Vendidos { get; set; }
        public int Preparados { get; set; }
        public int Pendientes => Vendidos - Preparados;
        public string Direccion { get; set; } = "";
    }
    public class EntregaRegistrada
    {
        public int IdEnvio { get; set; }
        public int IdVenta { get; set; }
        public int IdRemito { get; set; }
        public string Cliente { get; set; } = "";
        public string Productos { get; set; } = "";
        public string Direccion { get; set; } = "";
        public string Estado { get; set; } = "";
        public DateTime? FechaEntrega { get; set; }
    }
}
