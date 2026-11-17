namespace Prana.Entidades;
// agrupa los datos y operaciones de producto por entregar
public class ProductoPorEntregar
{
    // identificador de la venta relaciona sus productos, cobros, entrega y comprobantes
    public int IdVenta
    {
        get; set;
    }
    // identificador de la presentacion concreta del producto; distingue color y condicion
    public int IdVariante
    {
        get; set;
    }
    // conserva cliente como string para intercambiar este dato entre las capas
    public string Cliente { get; set; } = "";
    // conserva producto como string para intercambiar este dato entre las capas
    public string Producto { get; set; } = "";
    // conserva modalidad como string para intercambiar este dato entre las capas
    public string Modalidad { get; set; } = "";
    // conserva vendidos como int para intercambiar este dato entre las capas
    public int Vendidos
    {
        get; set;
    }
    // conserva preparados como int para intercambiar este dato entre las capas
    public int Preparados
    {
        get; set;
    }
    // dato pendientes de tipo int se obtiene al consultar la propiedad mediante la expresion siguiente
    public int Pendientes => Vendidos - Preparados;
    // conserva direccion como string para intercambiar este dato entre las capas
    public string DomicilioSugerido { get; set; } = "";
    public string LocalidadSugerida { get; set; } = "";
    public string Direccion => string.IsNullOrWhiteSpace(LocalidadSugerida) ? DomicilioSugerido : $"{DomicilioSugerido}, {LocalidadSugerida}";
}
