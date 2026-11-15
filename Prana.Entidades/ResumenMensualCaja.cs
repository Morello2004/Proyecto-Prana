namespace Prana.Entidades;
// agrupa los datos y operaciones de resumen mensual caja
public class ResumenMensualCaja
{
    // conserva mes como date time para intercambiar este dato entre las capas
    public DateTime Mes
    {
        get; set;
    }
    // conserva totales como list<total mensual caja> para intercambiar este dato entre las capas
    public List<TotalMensualCaja> Totales { get; set; } = new();
    // conserva por dia como list<total mensual caja> para intercambiar este dato entre las capas
    public List<TotalMensualCaja> PorDia { get; set; } = new();
    // conserva por medio como list<total mensual caja> para intercambiar este dato entre las capas
    public List<TotalMensualCaja> PorMedio { get; set; } = new();
    // conserva por tipo como list<total mensual caja> para intercambiar este dato entre las capas
    public List<TotalMensualCaja> PorTipo { get; set; } = new();
    // conserva movimientos como list<movimiento caja> para intercambiar este dato entre las capas
    public List<MovimientoCaja> Movimientos { get; set; } = new();
}
