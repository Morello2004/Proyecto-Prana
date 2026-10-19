namespace Prana.Entidades;

public class TotalMensualCaja
{
    public string Concepto { get; set; } = "";
    public string Moneda { get; set; } = "";
    public int Cantidad { get; set; }
    public decimal Ingresos { get; set; }
    public decimal Gastos { get; set; }
    public decimal Resultado { get; set; }
}

public class ResumenMensualCaja
{
    public DateTime Mes { get; set; }
    public List<TotalMensualCaja> Totales { get; set; } = new();
    public List<TotalMensualCaja> PorDia { get; set; } = new();
    public List<TotalMensualCaja> PorMedio { get; set; } = new();
    public List<TotalMensualCaja> PorTipo { get; set; } = new();
    public List<MovimientoCaja> Movimientos { get; set; } = new();
}
