namespace Prana.Entidades;
// agrupa los datos y operaciones de pago
public class Pago
{
    // conserva id pago como int para intercambiar este dato entre las capas
    public int IdPago
    {
        get; set;
    }
    // identificador de la venta relaciona sus productos, cobros, entrega y comprobantes
    public int IdVenta
    {
        get; set;
    }
    // conserva id medio pago como int para intercambiar este dato entre las capas
    public int IdMedioPago
    {
        get; set;
    }
    // conserva fecha como date time para intercambiar este dato entre las capas
    public DateTime Fecha
    {
        get; set;
    }
    // cuenta o referencia del cobro; sirve para identificar el movimiento recibido
    public string CuentaDestino { get; set; } = "";
    // conserva medio como string para intercambiar este dato entre las capas
    public string Medio { get; set; } = string.Empty;
    // cantidad de cuotas elegida para credito; los otros medios trabajan con una como valor interno
    public int Cuotas
    {
        get; set;
    }
    // importe que cancela deuda, sin el recargo de este cobro
    public decimal ImporteBase
    {
        get; set;
    }
    // mantiene el nombre utilizado al preparar los pagos de una venta en pago es el importe base: no incluye el recargo del medio elegido
    public decimal Monto
    {
        get => ImporteBase; set => ImporteBase = value;
    }
    // porcentaje aplicado a la base del cobro; diez representa diez por ciento
    public decimal Porcentaje
    {
        get; set;
    }
    // importe adicional del cobro no confundir este dinero con el porcentaje usado para calcularlo
    public decimal Recargo
    {
        get; set;
    }
    // dinero recibido con el recargo incluido; se distingue de la base aplicada a la deuda
    public decimal Cobrado
    {
        get; set;
    }
}
