namespace Prana.Entidades;
// agrupa los datos y operaciones de movimiento caja
public class MovimientoCaja
{
    // conserva id como int para intercambiar este dato entre las capas
    public int Id
    {
        get; set;
    }
    // conserva venta como int? para intercambiar este dato entre las capas
    public int? Venta
    {
        get; set;
    }
    // conserva fecha como date time para intercambiar este dato entre las capas
    public DateTime Fecha
    {
        get; set;
    }
    // conserva concepto como string para intercambiar este dato entre las capas
    public string Concepto { get; set; } = "";
    // conserva tipo como string para intercambiar este dato entre las capas
    public string Tipo { get; set; } = "Gasto";
    // conserva medio como string para intercambiar este dato entre las capas
    public string Medio { get; set; } = "Efectivo";
    // conserva moneda como string para intercambiar este dato entre las capas
    public string Moneda { get; set; } = "ARS";
    // conserva cuenta como string para intercambiar este dato entre las capas
    public string Cuenta { get; set; } = "";
    // conserva importe como decimal para intercambiar este dato entre las capas
    public decimal Importe
    {
        get; set;
    }
    // conserva usuario como string para intercambiar este dato entre las capas
    public string Usuario { get; set; } = "";
}
