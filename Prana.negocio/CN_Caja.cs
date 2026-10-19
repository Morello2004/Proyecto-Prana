using Prana.Datos;
using Prana.Entidades;

namespace Prana.negocio;

public class CN_Caja
{
    private readonly CD_Caja datos = new();
    public CierreCaja Consultar(DateTime fecha) { CN_Pago.Verificar(Permisos.GestionarCaja); return datos.Consultar(fecha); }
    public ResumenMensualCaja ConsultarMes(DateTime fecha)
    {
        CN_Pago.Verificar(Permisos.GestionarCaja);
        var mes = new DateTime(fecha.Year, fecha.Month, 1);
        return CrearResumenMensual(mes, datos.ConsultarPeriodo(mes, mes.AddMonths(1)));
    }

    // El resultado representa ingresos menos gastos; no suma saldos iniciales de cada día.
    public static ResumenMensualCaja CrearResumenMensual(DateTime fecha, IEnumerable<MovimientoCaja> movimientos)
    {
        var mes = new DateTime(fecha.Year, fecha.Month, 1);
        var lista = movimientos.Where(m => m.Fecha >= mes && m.Fecha < mes.AddMonths(1))
            .OrderBy(m => m.Fecha).ThenBy(m => m.Id).ToList();
        TotalMensualCaja Total(string concepto, string moneda, IEnumerable<MovimientoCaja> filas)
        {
            var items = filas.ToList();
            decimal ingresos = items.Where(m => m.Tipo != "Gasto").Sum(m => m.Importe);
            decimal gastos = items.Where(m => m.Tipo == "Gasto").Sum(m => m.Importe);
            return new TotalMensualCaja { Concepto = concepto, Moneda = moneda, Cantidad = items.Count,
                Ingresos = ingresos, Gastos = gastos, Resultado = ingresos - gastos };
        }
        return new ResumenMensualCaja
        {
            Mes = mes, Movimientos = lista,
            Totales = new[] { "ARS", "USD" }.Select(moneda => Total("Total del mes", moneda,
                lista.Where(m => m.Moneda == moneda))).ToList(),
            PorDia = lista.GroupBy(m => new { Dia = m.Fecha.Date, m.Moneda }).OrderBy(g => g.Key.Dia).ThenBy(g => g.Key.Moneda)
                .Select(g => Total(g.Key.Dia.ToString("dd/MM/yyyy"), g.Key.Moneda, g)).ToList(),
            PorMedio = lista.GroupBy(m => new { m.Medio, m.Moneda }).OrderBy(g => g.Key.Medio).ThenBy(g => g.Key.Moneda)
                .Select(g => Total(g.Key.Medio, g.Key.Moneda, g)).ToList(),
            PorTipo = lista.GroupBy(m => new { m.Tipo, m.Moneda }).OrderBy(g => g.Key.Tipo).ThenBy(g => g.Key.Moneda)
                .Select(g => Total(g.Key.Tipo, g.Key.Moneda, g)).ToList()
        };
    }

    public void Registrar(MovimientoCaja movimiento)
    {
        CN_Pago.Verificar(Permisos.GestionarCaja);
        movimiento.Concepto = movimiento.Concepto.Trim(); movimiento.Cuenta = movimiento.Cuenta.Trim();
        if (movimiento.Concepto.Length is < 1 or > 200) throw new ArgumentException("Ingrese un motivo de hasta 200 caracteres.");
        if (movimiento.Importe <= 0 || movimiento.Importe != decimal.Round(movimiento.Importe,2)) throw new ArgumentException("Ingrese un importe positivo con hasta dos decimales.");
        if (!TiposCaja.Todos.Contains(movimiento.Medio) || !new[]{"ARS","USD"}.Contains(movimiento.Moneda) ||
            !new[]{"Gasto","Ingreso extra","Café"}.Contains(movimiento.Tipo)) throw new ArgumentException("Seleccione tipo, medio y moneda válidos.");
        if (movimiento.Moneda == "USD" && movimiento.Medio != "Efectivo") throw new ArgumentException("Los movimientos en dólares se registran como efectivo, separados de los pesos.");
        if (movimiento.Cuenta.Length > 100) throw new ArgumentException("La cuenta admite hasta 100 caracteres.");
        datos.Registrar(movimiento, SesionActual.Usuario!.idUsuario);
    }
    public void Anular(int id, string motivo)
    {
        CN_Pago.Verificar(Permisos.GestionarCaja);
        if (id<=0 || string.IsNullOrWhiteSpace(motivo) || motivo.Trim().Length>200) throw new ArgumentException("Indique el motivo de anulación (hasta 200 caracteres).");
        datos.Anular(id,motivo.Trim(),SesionActual.Usuario!.idUsuario);
    }
    public CierreCaja Cerrar(CierreCaja cierre)
    {
        CN_Pago.Verificar(Permisos.GestionarCaja);
        if (cierre.Fecha.Date > DateTime.Today || cierre.Inicio.Date != cierre.Fecha.Date || cierre.Inicio > DateTime.Now)
            throw new ArgumentException("La fecha y hora de apertura no pueden estar en el futuro.");
        if (cierre.SaldoInicial<0 || cierre.SaldoInicialUsd<0 || cierre.Billetes.Any(b=>b.Cantidad<0 || b.Denominacion<=0 || !new[]{"ARS","USD"}.Contains(b.Moneda)))
            throw new ArgumentException("El saldo inicial y el conteo de billetes no pueden ser negativos.");
        if (cierre.Observaciones.Length>1000) throw new ArgumentException("Las observaciones admiten hasta 1000 caracteres.");
        return datos.Cerrar(cierre,SesionActual.Usuario!.idUsuario);
    }
}
