namespace Prana.Entidades;
// agrupa los datos y operaciones de entrega registrada
public class EntregaRegistrada
{
    // conserva id envio como int para intercambiar este dato entre las capas
    public int IdEnvio
    {
        get; set;
    }
    // identificador de la venta relaciona sus productos, cobros, entrega y comprobantes
    public int IdVenta
    {
        get; set;
    }
    // conserva id remito como int para intercambiar este dato entre las capas
    public int IdRemito
    {
        get; set;
    }
    // el numero impreso es el de venta; idremito identifica una preparacion parcial interna dato numero remito de tipo int se obtiene al consultar la propiedad mediante la expresion siguiente
    public int NumeroRemito => IdVenta;
    // conserva cliente como string para intercambiar este dato entre las capas
    public string Cliente { get; set; } = "";
    // conserva productos como string para intercambiar este dato entre las capas
    public string Productos { get; set; } = "";
    // conserva direccion como string para intercambiar este dato entre las capas
    public string Domicilio { get; set; } = "";
    public string Localidad { get; set; } = "";
    public string Direccion => string.IsNullOrWhiteSpace(Localidad) ? Domicilio : $"{Domicilio}, {Localidad}";
    // conserva estado como string para intercambiar este dato entre las capas
    public string Estado { get; set; } = "";
    // conserva fecha entrega como date time? para intercambiar este dato entre las capas
    public DateTime? FechaEntrega
    {
        get; set;
    }
}
