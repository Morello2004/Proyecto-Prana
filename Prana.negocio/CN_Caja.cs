using Prana.Datos;
using Prana.Entidades;

namespace Prana.negocio;
// agrupa los datos y operaciones de cn caja
public class CN_Caja
{
    // conserva datos entre las operaciones de esta clase
    private readonly CD_Caja datos = new();
    // abrir es una accion explicita y toma la hora real del servidor sin copiar saldos anteriores
    public CierreCaja Abrir(CierreCaja caja)
    {
        if (SesionActual.Usuario == null)
        {
            throw new InvalidOperationException("Inicie sesión para abrir una caja.");
        }
        ValidarObservaciones(caja);
        return datos.Abrir(caja, SesionActual.Usuario.idUsuario);
    }

    // mantiene la misma validacion para la apertura y las correcciones posteriores
    private static void ValidarObservaciones(CierreCaja caja)
    {
        if (caja.Observaciones.Length > 1000)
        {
            throw new ArgumentException("Las observaciones admiten hasta 1000 caracteres.");
        }
    }

    // devuelve las aperturas de una fecha para seleccionar un turno concreto
    public List<CierreCaja> Listar(DateTime fecha)
    {
        return datos.Listar(fecha);
    }
    // conserva los datos del turno abierto sin modificar otras cajas de la misma fecha
    public void GuardarCajaAbierta(CierreCaja caja)
    {
        ValidarObservaciones(caja);
        datos.GuardarCajaAbierta(caja);
    }

    // resuelve cerrar dias anteriores con los datos recibidos y entrega el resultado al codigo que lo llama
    public DateTime CerrarDiasAnteriores()
    {
        return datos.CerrarDiasAnteriores();
    }
    // consulta el turno elegido o la ultima apertura de la fecha cuando no se indica identificador
    public CierreCaja Consultar(DateTime fecha, int? idCaja = null)
    {
        return datos.Consultar(fecha, idCaja);
    }

    // prepara el resumen de movimientos del mes
    public ResumenMensualCaja ConsultarMes(DateTime fecha)
    {
        DateTime mes = new DateTime(fecha.Year, fecha.Month, 1);
        return CrearResumenMensual(mes, datos.ConsultarPeriodo(mes, mes.AddMonths(1)));
    }

    // Calcula ingresos, gastos y resultado por moneda desde los movimientos del mes.
    public static ResumenMensualCaja CrearResumenMensual(DateTime fecha, IEnumerable<MovimientoCaja> movimientos)
    {
        DateTime mes = new DateTime(fecha.Year, fecha.Month, 1);
        List<MovimientoCaja> lista = movimientos.Where(movimiento => movimiento.Fecha >= mes && movimiento.Fecha < mes.AddMonths(1)).OrderBy(m => m.Fecha).ThenBy(m => m.Id).ToList();
        TotalMensualCaja Total(string concepto, string moneda, IEnumerable<MovimientoCaja> filas)
        {
            List<MovimientoCaja> items = filas.ToList();
            decimal ingresos = items.Where(movimiento => movimiento.Tipo != "Gasto").Sum(m => m.Importe);
            decimal gastos = items.Where(m => m.Tipo == "Gasto").Sum(m => m.Importe);
            return new TotalMensualCaja
            {
                Concepto = concepto,
                Moneda = moneda,
                Ingresos = ingresos,
                Gastos = gastos,
                Resultado = ingresos - gastos

            };
        }

        return new ResumenMensualCaja
        {
            Mes = mes,
            Movimientos = lista,
            Totales = new[]
            {
                "ARS",
                "USD"
            }.Select(moneda => Total("Total del mes", moneda, lista.Where(m => m.Moneda == moneda))).ToList(),
            PorDia = lista.GroupBy(m => new { Dia = m.Fecha.Date, m.Moneda }).OrderBy(g => g.Key.Dia).ThenBy(g => g.Key.Moneda).Select(g => Total(g.Key.Dia.ToString("dd/MM/yyyy"), g.Key.Moneda, g)).ToList(),
            PorMedio = lista.GroupBy(m => new { m.Medio, m.Moneda }).OrderBy(g => g.Key.Medio).ThenBy(g => g.Key.Moneda).Select(g => Total(g.Key.Medio, g.Key.Moneda, g)).ToList(),
            PorTipo = lista.GroupBy(m => new { m.Tipo, m.Moneda }).OrderBy(g => g.Key.Tipo).ThenBy(g => g.Key.Moneda).Select(g => Total(g.Key.Tipo, g.Key.Moneda, g)).ToList()

        };
    }

    // valida los datos antes de registrar un movimiento manual
    public void Registrar(MovimientoCaja movimiento)
    {
        movimiento.Concepto = NormalizarTexto.Mayusculas(movimiento.Concepto);
        movimiento.Cuenta = movimiento.Cuenta.Trim();
        if (movimiento.Concepto.Length is < 1 or > 200)
        {
            // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
            throw new ArgumentException("Ingrese un motivo de hasta 200 caracteres.");
        }

        if (movimiento.Importe <= 0 || movimiento.Importe != decimal.Round(movimiento.Importe, 2))
        {
            // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
            throw new ArgumentException("Ingrese un importe positivo con hasta dos decimales.");
        }

        if (!TiposCaja.Todos.Contains(movimiento.Medio) || !new[]
        {
            "ARS",
            "USD"
        }.Contains(movimiento.Moneda) || !new[]
        {
            "Gasto",
            "Ingreso extra",
            "Café"
        }.Contains(movimiento.Tipo))
        {
            // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
            throw new ArgumentException("Seleccione tipo, medio y moneda válidos.");
        }

        if (movimiento.Moneda == "USD" && movimiento.Medio != "Efectivo")
        {
            // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
            throw new ArgumentException("Los movimientos en dólares se registran como efectivo, separados de los pesos.");
        }

        if (movimiento.Cuenta.Length > 100)
        {
            // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
            throw new ArgumentException("La cuenta admite hasta 100 caracteres.");
        }

        datos.Registrar(movimiento, SesionActual.Usuario!.idUsuario);
    }

    // valida el motivo antes de anular un movimiento
    public void Anular(int id, string motivo)
    {
        // comprueba el permiso aunque se intente anular sin usar el boton de la pantalla
        Permisos.Exigir(Permisos.AnularMovimientosCaja);
        motivo = NormalizarTexto.Mayusculas(motivo);
        // distingue un dato vacio de un valor ingresado antes de continuar
        if (id <= 0 || string.IsNullOrWhiteSpace(motivo) || motivo.Trim().Length > 200)
        {
            // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
            throw new ArgumentException("Indique el motivo de anulación (hasta 200 caracteres).");
        }

        datos.Anular(id, motivo.Trim(), SesionActual.Usuario!.idUsuario);
    }

    // valida los datos del cierre y solicita su guardado en la capa de datos
    public CierreCaja Cerrar(CierreCaja cierre)
    {
        if (cierre.Fecha.Date > DateTime.Today || cierre.Inicio.Date != cierre.Fecha.Date || cierre.Inicio > DateTime.Now)
        {
            // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
            throw new ArgumentException("La fecha y hora de apertura no pueden estar en el futuro.");
        }


        cierre.Observaciones = NormalizarTexto.Mayusculas(cierre.Observaciones);
        if (cierre.Observaciones.Length > 1000)
        {
            // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
            throw new ArgumentException("Las observaciones admiten hasta 1000 caracteres.");
        }

        return datos.Cerrar(cierre, SesionActual.Usuario!.idUsuario);
    }
}
