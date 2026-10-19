using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using Prana.Entidades;
using Prana.negocio;
using Prana.Presentacion;

class PruebaResumenMensual
{
    [STAThread]
    static void Main()
    {
        var mes = new DateTime(2026, 10, 1);
        MovimientoCaja Movimiento(DateTime fecha, string tipo, string moneda, decimal importe, string medio = "Efectivo")
            => new() { Fecha = fecha, Tipo = tipo, Moneda = moneda, Importe = importe, Medio = medio };
        var resumen = CN_Caja.CrearResumenMensual(mes.AddDays(10), new[] {
            Movimiento(mes.AddTicks(-1), "Cobro venta", "ARS", 999),
            Movimiento(mes, "Cobro venta", "ARS", 100),
            Movimiento(mes.AddDays(2), "Café", "ARS", 20),
            Movimiento(mes.AddDays(2), "Ingreso extra", "ARS", 30, "Transferencia"),
            Movimiento(mes.AddDays(2), "Gasto", "ARS", 40),
            Movimiento(mes.AddDays(3), "Ingreso extra", "USD", 15),
            Movimiento(mes.AddMonths(1).AddTicks(-1), "Gasto", "USD", 5),
            Movimiento(mes.AddMonths(1), "Cobro venta", "ARS", 999)
        });
        void Exigir(bool condicion, string mensaje) { if (!condicion) throw new Exception(mensaje); }
        var ars = resumen.Totales.Single(t => t.Moneda == "ARS");
        var usd = resumen.Totales.Single(t => t.Moneda == "USD");
        Exigir(resumen.Movimientos.Count == 6, "Límites del mes");
        Exigir(ars.Ingresos == 150 && ars.Gastos == 40 && ars.Resultado == 110, "Totales ARS");
        Exigir(usd.Ingresos == 15 && usd.Gastos == 5 && usd.Resultado == 10, "Totales USD");
        foreach (var filas in new[] { resumen.PorDia, resumen.PorMedio, resumen.PorTipo })
            foreach (var total in resumen.Totales)
                Exigir(filas.Where(t => t.Moneda == total.Moneda).Sum(t => t.Resultado) == total.Resultado, "Desglose conciliado");
        var vacio = CN_Caja.CrearResumenMensual(mes, Array.Empty<MovimientoCaja>());
        Exigir(vacio.Totales.Count == 2 && vacio.Totales.All(t => t.Resultado == 0), "Mes vacío");
        using var caja = new frmCaja();
        Exigir(caja.Controls.Find("resumenMes", true).Single() is Button, "Botón visible en diseñador");
        using var pantalla = new frmResumenMensualCaja(mes.AddDays(4));
        typeof(frmResumenMensualCaja).GetMethod("Mostrar", BindingFlags.Instance | BindingFlags.NonPublic)!.Invoke(pantalla, new object[] { resumen });
        foreach (var nombre in new[] { "totales", "porDia", "porMedio", "porTipo", "detalle" })
        {
            var grilla = (DataGridView)pantalla.Controls.Find(nombre, true).Single();
            Exigir(grilla.ReadOnly && grilla.DataSource != null && grilla.Columns.Count > 0, "Grilla " + nombre);
        }
        Exigir(((DateTimePicker)pantalla.Controls.Find("mes", true).Single()).Value == mes, "Mes seleccionado");
        Console.WriteLine("OK: límites del mes, ARS/USD, desgloses conciliados, mes vacío y controles del formulario.");
    }
}
