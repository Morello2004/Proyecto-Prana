namespace Prana.Entidades;

public static class TiposCaja
{
    public static readonly string[] Todos = { "Efectivo", "Transferencia", "Débito", "Crédito", "Otros" };
}

public class MovimientoCaja
{
    public int Id { get; set; }
    public int? Venta { get; set; }
    public DateTime Fecha { get; set; }
    public string Concepto { get; set; } = "";
    public string Tipo { get; set; } = "Gasto";
    public string Medio { get; set; } = "Efectivo";
    public string Moneda { get; set; } = "ARS";
    public string Cuenta { get; set; } = "";
    public decimal Importe { get; set; }
    public string Usuario { get; set; } = "";
}

public class ConteoBillete
{
    public string Moneda { get; set; } = "ARS";
    public decimal Denominacion { get; set; }
    public int Cantidad { get; set; }
    public decimal Total => Denominacion * Cantidad;
}

public class CierreCaja
{
    public int Numero { get; set; }
    public DateTime Fecha { get; set; } = DateTime.Today;
    public DateTime Inicio { get; set; } = DateTime.Today.AddHours(9);
    public DateTime Fin { get; set; }
    public decimal SaldoInicial { get; set; }
    public decimal SaldoInicialUsd { get; set; }
    public string Observaciones { get; set; } = "";
    public string Usuario { get; set; } = "";
    public List<MovimientoCaja> Movimientos { get; set; } = new();
    public List<ConteoBillete> Billetes { get; set; } = new();
    public decimal Ingresos(string moneda, string? medio = null) => Movimientos.Where(m => m.Moneda == moneda && m.Tipo != "Gasto" && (medio == null || m.Medio == medio)).Sum(m => m.Importe);
    public decimal Gastos(string moneda, string? medio = null) => Movimientos.Where(m => m.Moneda == moneda && m.Tipo == "Gasto" && (medio == null || m.Medio == medio)).Sum(m => m.Importe);
    public decimal Contado(string moneda) => Billetes.Where(b => b.Moneda == moneda).Sum(b => b.Total);
    public decimal Esperado(string moneda) => (moneda == "USD" ? SaldoInicialUsd : SaldoInicial) + Ingresos(moneda, "Efectivo") - Gastos(moneda, "Efectivo");
    public decimal Diferencia(string moneda) => Contado(moneda) - Esperado(moneda);
    public decimal BalanceArs => SaldoInicial + Ingresos("ARS") - Gastos("ARS");
    public decimal CafeArs => Movimientos.Where(m => m.Tipo == "Café" && m.Moneda == "ARS").Sum(m => m.Importe);
}
