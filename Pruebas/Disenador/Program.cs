using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.ComponentModel;
using System.Reflection;
using Prana.Presentacion;
using Prana.Entidades;
using Prana.negocio;

// agrupa los datos y operaciones de program
internal static class Program
{
    // punto de entrada: prepara el entorno y ejecuta el flujo inicial definido en este programa
    [STAThread]
    private static void Main()
    {
        Application.SetHighDpiMode(HighDpiMode.SystemAware);
        Application.EnableVisualStyles();
        SesionActual.IniciarSesion(new usuario
        {
            idUsuario = 1,
            IdRolUsuario = 1

        });
        // se construyen los controles sin mostrar ventanas ni ejecutar load o consultas
        Func<Form>[] crear = [() => new frmNuevaVenta(), () => new frmPagos(), () => new frmEntregas(), () => new frmCaja(), () => new frmAlertas(), () => new frmInformesVentas(), () => new frmClientes(), () => new frmUsuarios(), () => new frmPanelPrincipal(), () => new frmMediosPagos(), () => new frmHistorial(), () => new frmStock(), () => new buttonSeleccionarProducto(), () => new frmDeudaCliente(new Cliente()), () => new frmComprobantes(), () => new frmMotivoAnulacion(), () => new login()];
        // procesa cada constructor de crear sin cambiar el orden de la coleccion
        foreach (Func<Form> constructor in crear)
        {
            using Form formulario = constructor();
            List<Control> controles = Descendientes(formulario).ToList();
            // procesa cada boton de controlesof type<button>() sin cambiar el orden de la coleccion
            foreach (Button boton in controles.OfType<Button>())
            {
                Delegate[] acciones = Clicks(boton);
                if (acciones.Any(a => a.Method.Name.Contains('<')))
                {
                    // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                    throw new Exception($"{formulario.Name}/{boton.Name}: Click anónimo.");
                }

                if (acciones.GroupBy(a => a.Method).Any(g => g.Count() > 1))
                {
                    // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                    throw new Exception($"{formulario.Name}/{boton.Name}: Click duplicado.");
                }
            }

            if (formulario is frmNuevaVenta)
            {
                Button boton = controles.OfType<Button>().Single(b => b.Name == "btnRegistrarVenta");
                if (boton.Text != "Registrar venta" || boton.Parent?.Name != "pnlDocumentos")
                {
                    // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                    throw new Exception("Registrar venta debe estar en Documentos desde la construcción.");
                }

                Delegate[] acciones = Clicks(boton);
                if (acciones.Length != 1 || acciones[0].Method.Name != "btnRegistrarVenta_Click")
                {
                    // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                    throw new Exception("Registrar venta debe abrir un único manejador con nombre.");
                }

                if (boton.Bottom > boton.Parent.ClientSize.Height || boton.Right > boton.Parent.ClientSize.Width)
                {
                    // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                    throw new Exception("Registrar venta queda fuera del panel.");
                }
            }

            Console.WriteLine($"OK {formulario.GetType().Name}: {controles.Count} controles; eventos sin duplicados.");
        }
    }

    // resuelve descendientes con los datos recibidos y entrega el resultado al codigo que lo llama
    private static IEnumerable<Control> Descendientes(Control padre)
    {
        // procesa cada hijo de padrecontrols sin cambiar el orden de la coleccion
        foreach (Control hijo in padre.Controls)
        {
            yield return hijo;
            // procesa cada nieto de descendientes(hijo) sin cambiar el orden de la coleccion
            foreach (Control nieto in Descendientes(hijo))
            {
                yield return nieto;
            }
        }
    }

    // resuelve clicks con los datos recibidos y entrega el resultado al codigo que lo llama
    private static Delegate[] Clicks(Control control)
    {
        EventHandlerList eventos = (EventHandlerList)typeof(Component).GetProperty("Events", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(control)!;
        object clave = typeof(Control).GetFields(BindingFlags.Static | BindingFlags.NonPublic).Single(f => f.Name.Equals("s_clickEvent", StringComparison.OrdinalIgnoreCase) || f.Name.Equals("EventClick", StringComparison.OrdinalIgnoreCase)).GetValue(null)!;
        return eventos[clave]?.GetInvocationList() ?? [];
    }
}
