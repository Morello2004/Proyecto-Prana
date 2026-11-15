using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using Prana.Entidades;
using Prana.negocio;
using Prana.Presentacion;

// agrupa los datos y operaciones de prueba resumen mensual
class PruebaResumenMensual
{
    // punto de entrada: prepara el entorno y ejecuta el flujo inicial definido en este programa
    [STAThread]
    static void Main()
    {
        DateTime mes = new DateTime(2026, 10, 1);
        MovimientoCaja Movimiento(DateTime fecha, string tipo, string moneda, decimal importe, string medio = "Efectivo") => new()
        {
            Fecha = fecha,
            Tipo = tipo,
            Moneda = moneda,
            Importe = importe,
            Medio = medio

        };
        ResumenMensualCaja resumen = CN_Caja.CrearResumenMensual(mes.AddDays(10), new[] { Movimiento(mes.AddTicks(-1), "Cobro venta", "ARS", 999), Movimiento(mes, "Cobro venta", "ARS", 100), Movimiento(mes.AddDays(2), "Café", "ARS", 20), Movimiento(mes.AddDays(2), "Ingreso extra", "ARS", 30, "Transferencia"), Movimiento(mes.AddDays(2), "Gasto", "ARS", 40), Movimiento(mes.AddDays(3), "Ingreso extra", "USD", 15), Movimiento(mes.AddMonths(1).AddTicks(-1), "Gasto", "USD", 5), Movimiento(mes.AddMonths(1), "Cobro venta", "ARS", 999) });
        void Exigir(bool condicion, string mensaje)
        {
            if (!condicion)
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new Exception(mensaje);
            }
        }

        TotalMensualCaja ars = resumen.Totales.Single(t => t.Moneda == "ARS");
        TotalMensualCaja usd = resumen.Totales.Single(t => t.Moneda == "USD");
        Exigir(resumen.Movimientos.Count == 6, "Límites del mes");
        Exigir(ars.Ingresos == 150 && ars.Gastos == 40 && ars.Resultado == 110, "Totales ARS");
        Exigir(usd.Ingresos == 15 && usd.Gastos == 5 && usd.Resultado == 10, "Totales USD");
        // procesa cada filas de new[] { resumenpor dia, resumenpor medio, resumenpor tipo } sin cambiar el orden de la coleccion
        foreach (List<TotalMensualCaja> filas in new[]
        {
            resumen.PorDia,
            resumen.PorMedio,
            resumen.PorTipo
        }

        )
        {
            // procesa cada total de resumentotales sin cambiar el orden de la coleccion
            foreach (TotalMensualCaja total in resumen.Totales)
            {
                Exigir(filas.Where(t => t.Moneda == total.Moneda).Sum(t => t.Resultado) == total.Resultado, "Desglose conciliado");
            }
        }

        ResumenMensualCaja vacio = CN_Caja.CrearResumenMensual(mes, Array.Empty<MovimientoCaja>());
        Exigir(vacio.Totales.Count == 2 && vacio.Totales.All(t => t.Resultado == 0), "Mes vacío");
        using frmCaja caja = new frmCaja();
        Exigir(caja.Controls.Find("resumenMes", true).Single() is Button, "Botón visible en diseñador");
        using frmResumenMensualCaja pantalla = new frmResumenMensualCaja(mes.AddDays(4));
        // llama a typeof(frmresumenmensualcaja)getmethod("mostrar", bindingflagsinstance | bindingflagsnonpublic)!invoke para realizar este paso del flujo
        typeof(frmResumenMensualCaja).GetMethod("Mostrar", BindingFlags.Instance | BindingFlags.NonPublic)!.Invoke(pantalla, new object[] { resumen });
        // procesa cada nombre de new[] { "totales", "por dia", "por medio", "por tipo", "detalle" } sin cambiar el orden de la coleccion
        foreach (string nombre in new[]
        {
            "totales",
            "porDia",
            "porMedio",
            "porTipo",
            "detalle"
        }

        )
        {
            DataGridView grilla = (DataGridView)pantalla.Controls.Find(nombre, true).Single();
            Exigir(grilla.ReadOnly && grilla.DataSource != null && grilla.Columns.Count > 0, "Grilla " + nombre);
        }

        Exigir(((DateTimePicker)pantalla.Controls.Find("mes", true).Single()).Value == mes, "Mes seleccionado");
        Console.WriteLine("OK: límites del mes, ARS/USD, desgloses conciliados, mes vacío y controles del formulario.");
    }
}
