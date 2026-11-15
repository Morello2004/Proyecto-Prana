namespace Prana.Entidades;
// agrupa los datos y operaciones de conteo billete
public class ConteoBillete
{
    // conserva moneda como string para intercambiar este dato entre las capas
    public string Moneda { get; set; } = "ARS";
    // conserva denominacion como decimal para intercambiar este dato entre las capas
    public decimal Denominacion
    {
        get; set;
    }
    // numero de unidades de esta fila se multiplica por el precio o costo unitario segun el detalle
    public int Cantidad
    {
        get; set;
    }
    // dato total de tipo decimal se obtiene al consultar la propiedad mediante la expresion siguiente
    public decimal Total => Denominacion * Cantidad;
}
