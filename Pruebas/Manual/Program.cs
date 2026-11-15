using System.ComponentModel;
using System.Reflection;
using System.Drawing.Imaging;
using System.Text.Json;
using Prana.Presentacion;
using Prana.Entidades;
using Prana.negocio;

// agrupa los datos y operaciones de program
internal static class Program
{
    // resuelve all con los datos recibidos y entrega el resultado al codigo que lo llama
    static IEnumerable<Control> All(Control c)
    {
        // procesa cada control de ccontrols sin cambiar el orden de la coleccion
        foreach (Control control in c.Controls)
        {
            yield return control;
            // procesa cada y de all(control) sin cambiar el orden de la coleccion
            foreach (Control y in All(control))
            {
                yield return y;
            }
        }
    }

    // remove form load callbacks: documentation must never query the commercial database
    static void RemoveLoad(Form f)
    {
        EventHandlerList events = (EventHandlerList)typeof(Component).GetProperty("Events", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(f)!;
        // procesa cada key de typeof(form)get fields(binding flagsstatic | binding flagsnon public)where(x => xnamecontains("load", string comparisonordinal ignore case)) sin cambiar el orden de la coleccion
        foreach (FieldInfo key in typeof(Form).GetFields(BindingFlags.Static | BindingFlags.NonPublic).Where(x => x.Name.Contains("Load", StringComparison.OrdinalIgnoreCase)))
        {
            object k = key.GetValue(null);
            if (k != null && events[k] is Delegate d)
            {
                events.RemoveHandler(k, d);
            }
        }
    }

    // retira los eventos del sistema comercial para capturar pantallas sin consultas ni escrituras
    static void QuitarEventosComerciales(Control control)
    {
        EventHandlerList eventos = (EventHandlerList)typeof(Component).GetProperty("Events", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(control)!;
        for (Type? tipo = control.GetType(); tipo != null; tipo = tipo.BaseType)
        {
            // procesa cada campo de tipoget fields(binding flagsstatic | binding flagsnon public | binding flagsdeclared only) sin cambiar el orden de la coleccion
            foreach (FieldInfo campo in tipo.GetFields(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.DeclaredOnly))
            {
                object? clave = campo.GetValue(null);
                if (clave != null && eventos[clave] is Delegate manejadores)
                {
                    // procesa cada manejador de manejadoresget invocation list() sin cambiar el orden de la coleccion
                    foreach (Delegate manejador in manejadores.GetInvocationList())
                    {
                        if (manejador.Method.DeclaringType?.Assembly == typeof(frmNuevaVenta).Assembly)
                        {
                            eventos.RemoveHandler(clave, manejador);
                        }
                    }
                }
            }
        }
        // procesa cada hijo de controlcontrols sin cambiar el orden de la coleccion
        foreach (Control hijo in control.Controls) QuitarEventosComerciales(hijo);
    }

    // punto de entrada: prepara el entorno y ejecuta el flujo inicial definido en este programa
    [STAThread]
    static void Main(string[] args)
    {
        Application.SetHighDpiMode(HighDpiMode.DpiUnaware);
        Application.EnableVisualStyles();
        SesionActual.IniciarSesion(new usuario
        {
            idUsuario = 1,
            IdRolUsuario = 1

        });
        string dir = Path.GetFullPath(args[0]);
        Directory.CreateDirectory(dir);
        if (args.Contains("--ejemplos"))
        {
            Ejemplos(dir);
            return;
        }

        Func<Form>[] forms = [() => new login(), () => new Interfaz(), () => new frmPanelPrincipal(), () => new frmClientes(), () => new frmStock(), () => new buttonSeleccionarProducto(), () => new frmNuevaVenta(), () => new frmPagos(), () => new frmEntregas(), () => new frmCaja(), () => new frmResumenMensualCaja(), () => new frmAlertas(), () => new frmInformesVentas(), () => new frmHistorial(), () => new frmUsuarios(), () => new frmMediosPagos(), () => new frmMotivoAnulacion(), () => new frmComprobantes()];
        forms = forms.Append(() => new frmDeudaCliente(new Cliente
        {
            Nombre = "CLIENTE",
            Apellido = "DE EJEMPLO",
            DNI = "00000000"

        })).ToArray();
        List<object> catalog = new List<object>();
        // procesa cada create de forms sin cambiar el orden de la coleccion
        foreach (Func<Form> create in forms)
        {
            using Form formulario = create();
            // distingue un dato vacio de un valor ingresado antes de continuar
            if (string.IsNullOrEmpty(formulario.Name))
            {
                formulario.Name = formulario.GetType().Name;
            }

            RemoveLoad(formulario);
            QuitarEventosComerciales(formulario);
            formulario.FormBorderStyle = FormBorderStyle.None;
            formulario.ShowInTaskbar = false;
            formulario.StartPosition = FormStartPosition.Manual;
            formulario.Location = new Point(-20000, -20000);
            formulario.Show();
            formulario.PerformLayout();
            // procesa cada tab de all(formulario)of type<tab control>() sin cambiar el orden de la coleccion
            foreach (TabControl tab in All(formulario).OfType<TabControl>())
            {
                tab.SelectedIndex = 0;
            }

            Capture(formulario, formulario.Name, dir, catalog);
            // procesa cada panel de all(formulario)where(control => controlname == "tlp secciones" && controlheight > formularioheight)to list() sin cambiar el orden de la coleccion
            foreach (Control panel in All(formulario).Where(control => control.Name == "tlpSecciones" && control.Height > formulario.Height).ToList())
            {
                Capture(panel, formulario.Name + "_completa", dir, catalog);
            }

            if (formulario.AutoScroll)
            {
                // captura tambien los controles inferiores sin recortarlos por el alto de la ventana
                if (!catalog.Any(elemento => elemento.GetType().GetProperty("name")?.GetValue(elemento)?.ToString() == formulario.Name + "_completa"))
                {
                    Size tamanoOriginal = formulario.ClientSize;
                    int altoCompleto = Math.Max(tamanoOriginal.Height, formulario.AutoScrollMinSize.Height);
                    // procesa cada control de all(formulario) sin cambiar el orden de la coleccion
                    foreach (Control control in All(formulario))
                    {
                        Point posicion = formulario.PointToClient(control.PointToScreen(Point.Empty));
                        altoCompleto = Math.Max(altoCompleto, posicion.Y + control.Height + 20);
                    }
                    formulario.ClientSize = new Size(tamanoOriginal.Width, altoCompleto);
                    formulario.AutoScrollPosition = Point.Empty;
                    formulario.PerformLayout();
                    Capture(formulario, formulario.Name + "_completa", dir, catalog);
                    formulario.ClientSize = tamanoOriginal;
                    formulario.PerformLayout();
                }
                formulario.AutoScrollPosition = new Point(0, 10000);
                Capture(formulario, formulario.Name + "_inferior", dir, catalog);
            }

            // procesa cada tab de all(formulario)of type<tab control>()to list() sin cambiar el orden de la coleccion
            foreach (TabControl tab in All(formulario).OfType<TabControl>().ToList())
            {
                for (int i = 1; i < tab.TabCount; i++)
                {
                    tab.SelectedIndex = i;
                    formulario.PerformLayout();
                    Capture(formulario, formulario.Name + "_" + i, dir, catalog);
                }
            }
        }

        File.WriteAllText(Path.Combine(dir, "catalogo.json"), JsonSerializer.Serialize(catalog, new JsonSerializerOptions { WriteIndented = true }));
    }

    // resuelve ejemplos con los datos recibidos y entrega el resultado al codigo que lo llama
    static void Ejemplos(string dir)
    {
        DateTime day = new DateTime(2026, 11, 10);
        venta venta = new venta
        {
            IdVenta = 999001,
            Fecha = day,
            Cliente = new Cliente
            {
                Nombre = "CLIENTE", Apellido = "DE DEMOSTRACIÓN", DNI = "00000000",
                Telefono = "0000000000", Direccion = "DOMICILIO FICTICIO 123",
                NombreLocalidad = "LUJÁN", CodigoPostal = "6700"
            },
            DomicilioEntrega = "DOMICILIO FICTICIO 123",
            ModalidadEntrega = "Envío a domicilio",
            ObservacionesEntrega = "EJEMPLO PARA CAPACITACIÓN. SIN VALIDEZ COMERCIAL.",
            FechaEntrega = day.AddDays(1),
            HorarioSugerido = "9 A 12",
            TipoLogistica = "Logística propia",
            CondicionPago = "Pago parcial",
            ObservacionPago = "SEÑA DE EJEMPLO EN EFECTIVO",
            Total = 105000,
            Pagado = 30000,
            SaldoPendiente = 75000,
            CostoEnvio = 5000,
            Pagos = [new Pago { Fecha = day, Medio = "Efectivo", Cuotas = 1, ImporteBase = 30000, Cobrado = 30000 }],
            Detalles = [new DetalleNuevaVenta
            {
                IdVariante = 999,
                Producto = "HELADERA DE EJEMPLO",
                Variante = "BLANCO PRIMERA",
                Cantidad = 1,
                PrecioUnitario = 100000

}

            ]

        };
        CierreCaja cierre = new CierreCaja
        {
            Numero = 999001,
            Fecha = day,
            Inicio = day.AddHours(9),
            Fin = day.AddHours(18),
            Usuario = "OPERADOR DEMO",
            SaldoInicial = 10000,
            Observaciones = "EJEMPLO PARA CAPACITACIÓN. SIN VALIDEZ COMERCIAL.",
            Movimientos = [new MovimientoCaja
            {
                Venta = 999001,
                Concepto = "CLIENTE DE DEMOSTRACIÓN",
                Tipo = "Cobro",
                Medio = "Efectivo",
                Moneda = "ARS",
                Importe = 30000,
                Fecha = day

}, new MovimientoCaja
            {
                Concepto = "GASTO DE EJEMPLO",
                Tipo = "Gasto",
                Medio = "Efectivo",
                Moneda = "ARS",
                Importe = 2000,
                Fecha = day

}

            ]

        };
        File.WriteAllText(Path.Combine(dir, "ejemplo_garantia.html"), DocumentosPrana.VentaHtml(venta, false));
        File.WriteAllText(Path.Combine(dir, "ejemplo_remito.html"), DocumentosPrana.VentaHtml(venta, true));
        File.WriteAllText(Path.Combine(dir, "ejemplo_cierre.html"), DocumentosPrana.CierreHtml(cierre));
    }

    // resuelve capture con los datos recibidos y entrega el resultado al codigo que lo llama
    static void Capture(Control f, string name, string dir, List<object> catalog)
    {
        using Bitmap captura = new Bitmap(f.Width, f.Height);
        f.DrawToBitmap(captura, new Rectangle(0, 0, captura.Width, captura.Height));
        captura.Save(Path.Combine(dir, name + ".png"), ImageFormat.Png);
        var controls = All(f).Where(control => control.Visible && control.Width > 0 && control.Height > 0).Select(c =>
        {
            Point p = f.PointToClient(c.PointToScreen(Point.Empty));
            return new
            {
                name = c.Name,
                text = c.Text,
                type = c.GetType().Name,
                x = p.X,
                y = p.Y,
                w = c.Width,
                h = c.Height
            };
        }).ToList();
        catalog.Add(new
        {
            name,
            width = f.Width,
            height = f.Height,
            controls
        });
        Console.WriteLine(name + " " + f.Size);
    }
}
