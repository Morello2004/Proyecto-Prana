namespace Prana.Entidades;
// agrupa los datos y operaciones de total mensual caja
public class TotalMensualCaja
{
    // conserva concepto como string para intercambiar este dato entre las capas
    public string Concepto { get; set; } = "";
    // conserva moneda como string para intercambiar este dato entre las capas
    public string Moneda { get; set; } = "";
    // conserva ingresos como decimal para intercambiar este dato entre las capas
    public decimal Ingresos
    {
        get; set;
    }
    // conserva gastos como decimal para intercambiar este dato entre las capas
    public decimal Gastos
    {
        get; set;
    }
    // conserva resultado como decimal para intercambiar este dato entre las capas
    public decimal Resultado
    {
        get; set;
    }
}
