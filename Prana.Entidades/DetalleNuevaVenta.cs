namespace Prana.Entidades;
// agrupa los datos y operaciones de detalle nueva venta
public class DetalleNuevaVenta
{
    // identificador de la presentacion concreta del producto; distingue color y condicion
    public int IdVariante
    {
        get; set;
    }
    // conserva producto como string para intercambiar este dato entre las capas
    public string Producto { get; set; } = string.Empty;
    // conserva variante como string para intercambiar este dato entre las capas
    public string Variante { get; set; } = string.Empty;
    // numero de unidades de esta fila se multiplica por el precio o costo unitario segun el detalle
    public int Cantidad
    {
        get; set;
    }
    // precio aplicado a una unidad en el detalle; permite reconstruir el importe de la operacion
    public decimal PrecioUnitario
    {
        get; set;
    }
    // unidades fisicas registradas para esta variante
    public int StockFisico
    {
        get; set;
    }
    // unidades vendidas que todavia esperan preparacion, segun la consulta de existencias
    public int StockComprometido
    {
        get; set;
    }
}
