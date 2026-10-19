using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.ComponentModel;
using System.Reflection;
using Prana.Presentacion;
using Prana.Entidades;
using Prana.negocio;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        Application.SetHighDpiMode(HighDpiMode.SystemAware);
        Application.EnableVisualStyles();
        SesionActual.IniciarSesion(new usuario { idUsuario = 1, IdRolUsuario = 1 });
        // Se construyen los controles sin mostrar ventanas ni ejecutar Load o consultas.
        Func<Form>[] crear = [() => new frmNuevaVenta(), () => new frmPagos(),
            () => new frmEntregas(), () => new frmCaja(), () => new frmAlertas(),
            () => new frmInformesVentas(), () => new frmClientes(), () => new frmUsuarios(),
            () => new frmPanelPrincipal(), () => new frmMediosPagos(), () => new frmHistorial(),
            () => new frmStock(), () => new buttonSeleccionarProducto(),
            () => new frmDeudaCliente(new Cliente()), () => new frmComprobantes(),
            () => new frmMotivoAnulacion(), () => new login()];
        foreach (var constructor in crear)
        {
            using var formulario = constructor();
            var controles = Descendientes(formulario).ToList();
            foreach (var boton in controles.OfType<Button>())
            {
                var acciones = Clicks(boton);
                if (acciones.Any(a => a.Method.Name.Contains('<')))
                    throw new Exception($"{formulario.Name}/{boton.Name}: Click anónimo.");
                if (acciones.GroupBy(a => a.Method).Any(g => g.Count() > 1))
                    throw new Exception($"{formulario.Name}/{boton.Name}: Click duplicado.");
            }
            if (formulario is frmNuevaVenta)
            {
                var boton = controles.OfType<Button>().Single(b => b.Name == "btnRegistrarVenta");
                if (boton.Text != "Registrar venta" || boton.Parent?.Name != "pnlDocumentos")
                    throw new Exception("Registrar venta debe estar en Documentos desde la construcción.");
                var acciones = Clicks(boton);
                if (acciones.Length != 1 || acciones[0].Method.Name != "btnRegistrarVenta_Click")
                    throw new Exception("Registrar venta debe abrir un único manejador con nombre.");
                if (boton.Bottom > boton.Parent.ClientSize.Height || boton.Right > boton.Parent.ClientSize.Width)
                    throw new Exception("Registrar venta queda fuera del panel.");
            }
            Console.WriteLine($"OK {formulario.GetType().Name}: {controles.Count} controles; eventos sin duplicados.");
        }
    }
    private static IEnumerable<Control> Descendientes(Control padre)
    {
        foreach (Control hijo in padre.Controls)
        {
            yield return hijo;
            foreach (var nieto in Descendientes(hijo)) yield return nieto;
        }
    }
    private static Delegate[] Clicks(Control control)
    {
        var eventos = (EventHandlerList)typeof(Component).GetProperty("Events", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(control)!;
        var clave = typeof(Control).GetFields(BindingFlags.Static | BindingFlags.NonPublic)
            .Single(f => f.Name.Equals("s_clickEvent", StringComparison.OrdinalIgnoreCase) || f.Name.Equals("EventClick", StringComparison.OrdinalIgnoreCase))
            .GetValue(null)!;
        return eventos[clave]?.GetInvocationList() ?? [];
    }
}
