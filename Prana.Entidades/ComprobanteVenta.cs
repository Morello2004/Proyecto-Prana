namespace Prana.Entidades;

public class ComprobanteVenta
{
    public int Numero { get; set; }
    public DateTime Fecha { get; set; }
    public string Cliente { get; set; } = "";
    public string Dni { get; set; } = "";
    public string Telefono { get; set; } = "";
    public string Direccion { get; set; } = "";
    public string Localidad { get; set; } = "";
    public string Destino { get; set; } = "";
    public string Modalidad { get; set; } = "";
    public string Observaciones { get; set; } = "";
    public bool DatosEntregaCompletos { get; set; }
    public string CodigoPostal { get; set; } = "";
    public DateTime? FechaEntrega { get; set; }
    public string HorarioSugerido { get; set; } = "";
    public string TipoLogistica { get; set; } = "";
    public string ObservacionPago { get; set; } = "";
    public string CondicionPago { get; set; } = "";
    public decimal Descuento { get; set; }
    public decimal Envio { get; set; }
    public decimal Recargos { get; set; }
    public decimal Total { get; set; }
    public decimal Pagado { get; set; }
    public string SegundoTelefono { get; set; } = string.Empty;
    public decimal Saldo => Math.Max(0, Total - Pagado);
    public List<DetalleNuevaVenta> Productos { get; set; } = new();
}
