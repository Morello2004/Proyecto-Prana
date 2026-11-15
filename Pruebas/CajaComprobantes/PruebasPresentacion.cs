using Prana.Entidades;
using Prana.Presentacion;
using System.ComponentModel;
using System.Reflection;

// agrupa los datos y operaciones de pruebas presentacion
internal static class PruebasPresentacion
{
    // resuelve verificar con los datos recibidos y entrega el resultado al codigo que lo llama
    private static void Verificar(bool condicion, string mensaje)
    {
        if (!condicion)
        {
            // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
            throw new Exception(mensaje);
        }

        Console.WriteLine("OK: " + mensaje);
    }

    // resuelve campo con los datos recibidos y entrega el resultado al codigo que lo llama
    private static T Campo<T>(object instancia, string nombre) => (T)instancia.GetType().GetField(nombre, BindingFlags.NonPublic | BindingFlags.Instance)!.GetValue(instancia)!;
    // resuelve invocar con los datos recibidos y entrega el resultado al codigo que lo llama
    private static void Invocar(object instancia, string nombre) => instancia.GetType().GetMethod(nombre, BindingFlags.NonPublic | BindingFlags.Instance)!.Invoke(instancia, null);
    // resuelve ejecutar con los datos recibidos y entrega el resultado al codigo que lo llama
    internal static void Ejecutar()
    {
        venta documento = new venta
        {
            IdVenta = 2500,
            Fecha = DateTime.Today,
            Cliente = new Cliente
            {
                Nombre = "Cliente", Apellido = "de prueba", CodigoPostal = "6700",
                TipoDocumento = "CUIT", DNI = "20123456789", Telefono = "2323000000",
                Direccion = "Calle <uno>", NombreLocalidad = "Luján"
            },
            DomicilioEntrega = "Calle de prueba 123",
            ModalidadEntrega = "Envío a domicilio",
            Total = 1140,
            Pagado = 400,
            SaldoPendiente = 740,
            Descuento = 10,
            CostoEnvio = 50,
            ObservacionesEntrega = "Llamar antes de llegar\nNo apoyar de costado",
            ObservacionPago = "Seña recibida <transferencia>",
            FechaEntrega = DateTime.Today.AddDays(1),
            HorarioSugerido = "De 14 a 18 hs",
            TipoLogistica = "Flete",
            Detalles = new()
            {
                new()
                {
                    IdVariante = 1,
                    Producto = "Heladera <prueba>",
                    Variante = "Blanca / outlet",
                    Cantidad = 1,
                    PrecioUnitario = 1000

}
            }

        };
        string garantiaCliente = DocumentosPrana.VentaHtml(documento, false);
        Verificar(garantiaCliente.Contains("Cliente de prueba") && garantiaCliente.Contains("CUIT") && garantiaCliente.Contains("20123456789")
            && garantiaCliente.Contains("2323000000") && garantiaCliente.Contains("Calle &lt;uno&gt;") && garantiaCliente.Contains("6700")
            && garantiaCliente.Contains(System.Net.WebUtility.HtmlEncode("Luján")), "Los documentos utilizan los datos del cliente asociado a venta");
        Verificar(!garantiaCliente.Contains("<th>Cuotas</th>"), "No agrega columna de cuotas sin pagos con credito");
        // procesa cada remito de new[] { false, true } sin cambiar el orden de la coleccion
        foreach (bool remito in new[]
        {
            false,
            true
        }

        )
        {
            string html = DocumentosPrana.VentaHtml(documento, remito);
            if (remito)
            {
                Verificar(!html.Contains("recargo", StringComparison.OrdinalIgnoreCase), "Remito sin mención de recargos");
            }

            Verificar(html.Contains("2500") && html.Contains("740,00") && html.Contains("Heladera &lt;prueba&gt;"), "Identificación, saldo y descripción conservados");
            if (!remito)
            {
                Verificar(html.Contains("1.140,00") && html.Contains("400,00") && !html.Contains("1.000,00") && !html.Contains("Subtotal"), "Garantía muestra total final sin importes base ni desglose");
            }
            else
            {
                Verificar(html.Contains("Calle de prueba 123") && html.Contains("Llamar antes de llegar<br>No apoyar de costado") && html.Contains(System.Net.WebUtility.HtmlEncode(documento.ObservacionPago)) && html.Contains("De 14 a 18 hs") && html.Contains("FIRMA Y SELLO"), "Remito conserva entrega, otros datos, observaciones y firma");
            }
        }

        documento.Pagos.AddRange(new[] { new Pago {
Fecha = DateTime.Today,
Medio = "Efectivo",
Cuotas = 1,
ImporteBase = 100,
Cobrado = 100

}, new Pago {
Fecha = DateTime.Today,
Medio = "Crédito",
Cuotas = 3,
ImporteBase = 250,
Recargo = 50,
Cobrado = 300

} });
        string garantiaPagos = System.Net.WebUtility.HtmlDecode(DocumentosPrana.VentaHtml(documento, false));
        Verificar(garantiaPagos.Contains("Efectivo") && garantiaPagos.Contains("Crédito") && garantiaPagos.Contains("300,00") && garantiaPagos.Contains("Abonado al emitir (con recargos)"), "Garantia incluye todos los medios y los importes abonados con recargos");
        Verificar(garantiaPagos.Contains("<th>Cuotas</th>") && garantiaPagos.Contains("<td>3</td>"), "Conserva las cuotas del pago con credito");
        // procesa cada remito de new[] { false, true } sin cambiar el orden de la coleccion
        foreach (bool remito in new[]
        {
            false,
            true
        }

        )
        {
            documento.Detalles[0].Variante = "Acero inoxidable / Primera";
            string html = DocumentosPrana.VentaHtml(documento, remito);
            Verificar(html.Contains("Acero inoxidable") && !html.Contains("/ Primera"), "No imprime condición Primera");
            documento.Detalles[0].Variante = "Blanca / outlet";
            Verificar(DocumentosPrana.VentaHtml(documento, remito).Contains("Blanca / outlet"), "Conserva condición outlet");
        }

        DateTime? fechaEntrega = documento.FechaEntrega;
        documento.FechaEntrega = null;
        Verificar(!DocumentosPrana.VentaHtml(documento, true).Contains("01/01/0001"), "Una venta sin fecha estimada no imprime una fecha inventada");
        documento.FechaEntrega = fechaEntrega;
        string carpetaDocumentos = DocumentosPrana.GuardarVenta(documento, "obj/CajaComprobantes/venta-unificada");
        Verificar(File.ReadAllText(Path.Combine(carpetaDocumentos, "Garantia-2500.html")) == DocumentosPrana.VentaHtml(documento, false)
            && File.ReadAllText(Path.Combine(carpetaDocumentos, "Remito-2500.html")) == DocumentosPrana.VentaHtml(documento, true),
            "Guarda ambos documentos con el identificador de venta y conserva su contenido");

        // limita la vida del recurso al bloque; al salir se ejecuta su liberacion incluso ante una excepcion
        using (frmNuevaVenta venta = new frmNuevaVenta())
        {
            ComboBox selector = Campo<ComboBox>(venta, "cboBuscarProducto");
            Type tipoOpcion = typeof(frmNuevaVenta).GetNestedType("OpcionProducto", BindingFlags.NonPublic)!;
            // procesa cada nombre de new[] { "heladera whirlpool", "cocina" } sin cambiar el orden de la coleccion
            foreach (string nombre in new[]
            {
                "HELADERA WHIRLPOOL",
                "COCINA"
            }

            )
            {
                selector.Items.Add(Activator.CreateInstance(tipoOpcion, new producto { NombreProducto = nombre }, new VarianteProducto
                {
                    IdVariante = selector.Items.Count + 1,
                    Color = "BLANCA",
                    CondicionProducto = "PRIMERA",
                    Precio = 100,
                    StockFisico = 10

                })!);
            }

            selector.SelectedIndex = -1;
            selector.SelectAll();
            selector.SelectedText = "HELADERA WHIRLPOOL - BLANCA PRIMERA";
            Invocar(venta, "AgregarProducto");
            Verificar(Campo<List<DetalleNuevaVenta>>(venta, "detallesVenta").Single().Producto == "HELADERA WHIRLPOOL", "Agregar reconoce el texto completo del autocompletado nativo");
            Verificar(selector.Items.Count == 2 && selector.Text == "", "Agregar limpia el campo y conserva las opciones");
            selector.SelectedIndex = 1;
            Invocar(venta, "AgregarProducto");
            Verificar(Campo<List<DetalleNuevaVenta>>(venta, "detallesVenta").Count == 2 && selector.Items.Count == 2, "Permite agregar otro producto sin perder el catalogo");
            List<DetalleNuevaVenta> detalleProductos = Campo<List<DetalleNuevaVenta>>(venta, "detallesVenta");
            bool duplicadoRechazado = false;
            try
            {
                // llama a new prananegociocn_registroventa()prepararproducto para realizar este paso del flujo
                new Prana.negocio.CN_RegistroVenta().PrepararProducto(new producto { NombreProducto = "HELADERA WHIRLPOOL" }, new VarianteProducto
                {
                    IdVariante = 1,
                    Precio = 100

                }, 3, detalleProductos);
            }
            // atiende el error de esta operacion antes de informar o propagar el problema
            catch (InvalidOperationException ex)
            {
                duplicadoRechazado = ex.Message.Contains("cantidad requerida");
            }

            Verificar(duplicadoRechazado && detalleProductos.Count == 2, "Rechaza repetir la variante e indica quitar y agregar con la cantidad requerida");
            Verificar(Convert.ToInt32(Campo<DataGridView>(venta, "dgvDetalleVenta").Rows[0].Cells["colCantidadVenta"].Value) == 1, "La cantidad se muestra en su propia columna");
            if (Environment.GetCommandLineArgs().Contains("--selector-productos"))
            {
                Prana.negocio.SesionActual.IniciarSesion(new usuario
                {
                    idUsuario = 1,
                    IdRolUsuario = 1

                });
                using frmPanelPrincipal panel = new frmPanelPrincipal();
                DataGridView listado = Campo<DataGridView>(panel, "dgvUltimasVentas");
                // anade una fila visible; el orden de los valores debe coincidir con el de las columnas
                listado.Rows.Add(1, DateTime.Today, "Cliente uno", 100);
                // anade una fila visible; el orden de los valores debe coincidir con el de las columnas
                listado.Rows.Add(2, DateTime.Today, "Cliente dos", 200);
                // procesa cada fila de listadorows sin cambiar el orden de la coleccion
                foreach (DataGridViewRow fila in listado.Rows)
                {
                    DataGridViewCellStyle estilo = fila.Cells[0].GetInheritedStyle(null, fila.Index, true);
                    Verificar(estilo.ForeColor == System.Drawing.Color.Black && estilo.SelectionForeColor == System.Drawing.Color.Black, "Texto negro en filas normales y alternadas, seleccionadas o sin seleccionar");
                }

                return;
            }

            Campo<List<DetalleNuevaVenta>>(venta, "detallesVenta").Clear();
            // llama a campo>(venta, "detallesventa")add para realizar este paso del flujo
            Campo<List<DetalleNuevaVenta>>(venta, "detallesVenta").Add(new()
            {
                Cantidad = 1,
                PrecioUnitario = 1000

            });
            // llama a campo(venta, "cbomediopago")itemsadd para realizar este paso del flujo
            Campo<ComboBox>(venta, "cboMedioPago").Items.Add(new MedioPago
            {
                IdMedioPago = 1,
                NombreMedioPago = "Efectivo"

            });
            // campo(venta,"cbomediopago")selectedindex: indice de la opcion actual; menos uno significa que no hay una opcion seleccionada
            Campo<ComboBox>(venta, "cboMedioPago").SelectedIndex = 0;
            // campo(venta,"nudmontopago")value: valor actual del control numerico o de fecha
            Campo<NumericUpDown>(venta, "nudMontoPago").Value = 400;
            Invocar(venta, "AgregarPago");
            DataGridView grid = Campo<DataGridView>(venta, "dgvPagos");
            Verificar(grid.Rows.Count == 1 && grid.Rows[0].Cells[0].Value?.ToString() == "Efectivo" && Campo<List<Pago>>(venta, "pagosVenta")[0].Monto == 400, "Agregar pago actualiza grilla y monto");
            Verificar(grid.Height >= 180 && Campo<Label>(venta, "lblTotalPagoActual").Top >= grid.Bottom, "Pagos con espacio visible y resumen sin superposición");
        }

        DocumentosPrana.GuardarVenta(documento, "obj/CajaComprobantes/presentacion");
        using frmCaja caja = new frmCaja();
        DataGridView movimientos = Campo<DataGridView>(caja, "movimientos");
        CierreCaja actual = Campo<CierreCaja>(caja, "actual");
        actual.Movimientos.AddRange(new[] { new MovimientoCaja {
Venta = 2500,
Tipo = "Cobro venta",
Medio = "Efectivo",
Importe = 300

}, new MovimientoCaja {
Venta = 2500,
Tipo = "Cobro venta",
Medio = "Transferencia",
Importe = 100

}, new MovimientoCaja {
Venta = 2501,
Tipo = "Cobro venta",
Medio = "Transferencia",
Importe = 200

}, new MovimientoCaja {
Tipo = "Gasto",
Medio = "Efectivo",
Importe = 50

}, new MovimientoCaja {
Tipo = "Ingreso extra",
Moneda = "USD",
Importe = 20

} });
        Invocar(caja, "ActualizarResumen");
        Verificar(actual.Ingresos("ARS") == 600 && actual.BalanceArs == 550 && actual.Ingresos("USD") == 20, "Cobros parciales sumados, gastos descontados y USD separado");
        Verificar(movimientos.ReadOnly, "Movimientos de solo lectura");
        string cierre = DocumentosPrana.CierreHtml(actual);
        Verificar(cierre.Contains("RTO 2500") && cierre.Contains("RTO 2501") && cierre.Contains("600,00") && cierre.Contains("550,00"), "Cierre identifica remitos e ingresos reales");
        Verificar(!cierre.Contains("Billete") && !cierre.Contains("Contado") && !cierre.Contains("Diferencia"), "Cierre sin conteo de billetes");
        File.WriteAllText("obj/CajaComprobantes/presentacion/Cierre.html", cierre);
        Panel pagina = (Panel)caja.Controls["paginaAdaptable"]!;
        caja.Controls.Remove(pagina);
        pagina.Dock = DockStyle.None;
        pagina.Size = new Size(840, 690);
        pagina.CreateControl();
        // procesa cada ancho de new[] { 780, 840, 1100 } sin cambiar el orden de la coleccion
        foreach (int ancho in new[]
        {
            780,
            840,
            1100
        }

        )
        {
            pagina.Size = new Size(ancho, 690);
            pagina.PerformLayout();
            Verificar(pagina.Controls.Cast<Control>().All(control => control.Right <= pagina.ClientSize.Width), "Controles dentro del ancho " + ancho);
            pagina.ScrollControlIntoView(pagina.Controls[pagina.Controls.Count - 1]);
            Verificar(pagina.Controls[pagina.Controls.Count - 1].Bottom <= pagina.ClientSize.Height + 1, "Acceso al cierre mediante desplazamiento");
            pagina.AutoScrollPosition = Point.Empty;
        }

        pagina.Dispose();
        Console.WriteLine("PRUEBAS DE PRESENTACIÓN APROBADAS");
    }
}
