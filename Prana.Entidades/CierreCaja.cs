namespace Prana.Entidades;
// agrupa los datos y operaciones de cierre caja
public class CierreCaja
{
    // identifica la apertura tanto abierta como cerrada sin depender de su fecha ni del numero visible del cierre
    public int IdCaja { get; set; }
    // refleja el estado de la fila de caja y permite distinguirlo del numero del cierre
    public bool Cerrada { get; set; }
    // Numeración de cierre desde 500, independiente de la apertura.
    public int Numero
    {
        get; set;
    }
    // conserva fecha como date time para intercambiar este dato entre las capas
    public DateTime Fecha { get; set; } = DateTime.Today;
    // conserva inicio como date time para intercambiar este dato entre las capas
    public DateTime Inicio { get; set; } = DateTime.Now;
    // conserva fin como date time para intercambiar este dato entre las capas
    public DateTime Fin
    {
        get; set;
    }
    // conserva observaciones como string para intercambiar este dato entre las capas
    public string Observaciones { get; set; } = "";
    // conserva usuario como string para intercambiar este dato entre las capas
    public string Usuario { get; set; } = "";
    // conserva movimientos como list<movimiento caja> para intercambiar este dato entre las capas
    public List<MovimientoCaja> Movimientos { get; set; } = new();

    // resuelve ingresos con los datos recibidos y entrega el resultado al codigo que lo llama
    public decimal Ingresos(string moneda, string? medio = null) => Movimientos.Where(movimiento => movimiento.Moneda == moneda && movimiento.Tipo != "Gasto" && (medio == null || movimiento.Medio == medio)).Sum(m => m.Importe);
    // resuelve gastos con los datos recibidos y entrega el resultado al codigo que lo llama
    public decimal Gastos(string moneda, string? medio = null) => Movimientos.Where(movimiento => movimiento.Moneda == moneda && movimiento.Tipo == "Gasto" && (medio == null || movimiento.Medio == medio)).Sum(m => m.Importe);
    // dato balance ars de tipo decimal se obtiene al consultar la propiedad mediante la expresion siguiente
    public decimal BalanceArs => Ingresos("ARS") - Gastos("ARS");
    // dato cafe ars de tipo decimal se obtiene al consultar la propiedad mediante la expresion siguiente
    public decimal CafeArs => Movimientos.Where(movimiento => movimiento.Tipo == "Café" && movimiento.Moneda == "ARS").Sum(m => m.Importe);

    // describe el turno en el selector de cajas de una misma fecha
    public override string ToString()
    {
        string estado = "Abierta";
        if (Cerrada)
        {
            estado = "Cerrada";
        }
        return Cerrada ? $"Cierre N.º {Numero} · {Inicio:HH:mm} · {estado}" : $"Caja abierta · {Inicio:HH:mm}";
    }
}
