namespace Prana.Entidades;
// agrupa los datos y operaciones de venta
public class venta
{
    // identificador de la venta relaciona sus productos, cobros, entrega y comprobantes
    public int IdVenta
    {
        get; set;
    }
    // esta linea significa que cada venta tiene un objeto cliente asociado
    public Cliente Cliente { get; set; } = new Cliente();
    // conserva fecha como date time para intercambiar este dato entre las capas
    public DateTime Fecha
    {
        get; set;
    }
    // conserva total como decimal para intercambiar este dato entre las capas
    public decimal Total
    {
        get; set;
    }
    // propiedades planas para enlazar las columnas de windows forms dato nombre completo cliente de tipo string se obtiene al consultar la propiedad mediante la expresion siguiente
    public string NombreCompletoCliente => $"{Cliente.Nombre} {Cliente.Apellido}";
    // total abonado que utiliza esta consulta o comprobante; revisar si incluye recargos en su origen
    public decimal Pagado
    {
        get; set;
    }
    // deuda que queda por cancelar segun los importes de ventas y pagos consultados
    public decimal SaldoPendiente
    {
        get; set;
    }
    // conserva productos como string para intercambiar este dato entre las capas
    public string Productos { get; set; } = string.Empty;
    // conserva estado como string para intercambiar este dato entre las capas
    public string Estado { get; set; } = string.Empty;
    // se consulta por separado del estado de pago de la venta
    public string EstadoEntrega { get; set; } = string.Empty;
    // conserva los datos necesarios para registrar la venta identificador del cliente; permite distinguir personas aunque tengan el mismo nombre
    public int IdCliente
    {
        get; set;
    }
    // identificador de la cuenta responsable de la operacion
    public int IdUsuario
    {
        get; set;
    }
    // importe monetario descontado de los productos; no es un porcentaje
    public decimal Descuento
    {
        get; set;
    }
    // conserva condicion pago como string para intercambiar este dato entre las capas
    public string CondicionPago { get; set; } = string.Empty;
    // conserva fecha pago como date time para intercambiar este dato entre las capas
    public DateTime FechaPago
    {
        get; set;
    }
    // conserva observacion pago como string para intercambiar este dato entre las capas
    public string ObservacionPago { get; set; } = string.Empty;
    // conserva modalidad entrega como string para intercambiar este dato entre las capas
    public string ModalidadEntrega { get; set; } = string.Empty;
    // conserva domicilio entrega como string para intercambiar este dato entre las capas
    public string DomicilioEntrega { get; set; } = string.Empty;
    public string LocalidadEntrega { get; set; } = string.Empty;
    public string HorarioSugerido { get; set; } = string.Empty;
    public string DireccionEntregaCompleta => string.Join(", ", new[] { DomicilioEntrega, LocalidadEntrega }.Where(x => !string.IsNullOrWhiteSpace(x)));
    // puede no existir una fecha estimada en ventas anteriores
    public DateTime? FechaEntrega
    {
        get; set;
    }
    // conserva observaciones entrega como string para intercambiar este dato entre las capas
    public string ObservacionesEntrega { get; set; } = string.Empty;
    // conserva tipo logistica como string para intercambiar este dato entre las capas
    public string TipoLogistica { get; set; } = string.Empty;
    // importe de logistica incorporado a la venta cuando corresponde un envio
    public decimal CostoEnvio
    {
        get; set;
    }
    // conserva detalles como list<detalle nueva venta> para intercambiar este dato entre las capas
    public List<DetalleNuevaVenta> Detalles { get; set; } = new();
    // conserva pagos como list<pago> para intercambiar este dato entre las capas
    public List<Pago> Pagos { get; set; } = new();
    // conserva segundo telefono como string para intercambiar este dato entre las capas
    public string SegundoTelefono { get; set; } = string.Empty;
}
