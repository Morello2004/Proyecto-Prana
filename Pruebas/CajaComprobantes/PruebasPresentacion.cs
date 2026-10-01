using Prana.Entidades;
using Prana.Presentacion;
using System.ComponentModel;
using System.Reflection;

internal static class PruebasPresentacion
{
    private static void Verificar(bool condicion, string mensaje)
    {
        if(!condicion)throw new Exception(mensaje);
        Console.WriteLine("OK: "+mensaje);
    }
    private static T Campo<T>(object instancia,string nombre) =>
        (T)instancia.GetType().GetField(nombre,BindingFlags.NonPublic|BindingFlags.Instance)!.GetValue(instancia)!;
    private static void Invocar(object instancia,string nombre) =>
        instancia.GetType().GetMethod(nombre,BindingFlags.NonPublic|BindingFlags.Instance)!.Invoke(instancia,null);

    internal static void Ejecutar()
    {
        var documento=new ComprobanteVenta {
            Numero=2500,Fecha=DateTime.Today,Cliente="Cliente de prueba",Destino="Calle de prueba 123",Modalidad="Envío a domicilio",
            Total=1140,Pagado=400,Descuento=10,Envio=50,Recargos=100,
            Observaciones="Llamar antes de llegar\nNo apoyar de costado",ObservacionPago="Seña recibida <transferencia>",
            FechaEntrega=DateTime.Today.AddDays(1),HorarioSugerido="De 14 a 18 hs",TipoLogistica="Flete",CodigoPostal="6700",
            Productos=new(){new(){IdVariante=1,Producto="Heladera <prueba>",Variante="Blanca / outlet",Cantidad=1,PrecioUnitario=1000}}
        };
        foreach(bool remito in new[]{false,true})
        {
            string html=DocumentosPrana.VentaHtml(documento,remito);
            Verificar(!html.Contains("recargo",StringComparison.OrdinalIgnoreCase),"Comprobante sin mención de recargos");
            Verificar(html.Contains("2500") && html.Contains("740,00") && html.Contains("Heladera &lt;prueba&gt;"),"Identificación, saldo y descripción conservados");
            if(!remito)
                Verificar(html.Contains("1.140,00") && html.Contains("400,00") && !html.Contains("1.000,00") && !html.Contains("Subtotal"),"Garantía muestra total final sin importes base ni desglose");
            else Verificar(html.Contains("Calle de prueba 123") && html.Contains("Llamar antes de llegar<br>No apoyar de costado")
                && html.Contains(System.Net.WebUtility.HtmlEncode(documento.ObservacionPago)) && html.Contains("De 14 a 18 hs") && html.Contains("FIRMA Y SELLO"),
                "Remito conserva entrega, otros datos, observaciones y firma");
        }
        foreach(bool remito in new[]{false,true})
        {
            documento.Productos[0].Variante="Acero inoxidable / Primera";
            string html=DocumentosPrana.VentaHtml(documento,remito);
            Verificar(html.Contains("Acero inoxidable") && !html.Contains("/ Primera"),"No imprime condición Primera");
            documento.Productos[0].Variante="Blanca / outlet";
            Verificar(DocumentosPrana.VentaHtml(documento,remito).Contains("Blanca / outlet"),"Conserva condición outlet");
        }
        using(var venta=new frmNuevaVenta())
        {
            Campo<List<DetalleNuevaVenta>>(venta,"detallesVenta").Add(new(){Cantidad=1,PrecioUnitario=1000});
            Campo<ComboBox>(venta,"cboMedioPago").Items.Add(new MedioPago {IdMedioPago=1,NombreMedioPago="Efectivo"});
            Campo<ComboBox>(venta,"cboMedioPago").SelectedIndex=0;
            Campo<NumericUpDown>(venta,"nudMontoPago").Value=400;
            Invocar(venta,"AgregarPago");
            var grid=Campo<DataGridView>(venta,"dgvPagos");
            Verificar(grid.Rows.Count==1 && grid.Rows[0].Cells[0].Value?.ToString()=="Efectivo"
                && Campo<List<PagoNuevaVenta>>(venta,"pagosVenta")[0].Monto==400,"Agregar pago actualiza grilla y monto");
            Verificar(grid.Height>=180 && Campo<Label>(venta,"lblTotalPagoActual").Top>=grid.Bottom,"Pagos con espacio visible y resumen sin superposición");
        }
        DocumentosPrana.GuardarVenta(documento,"obj/CajaComprobantes/presentacion");

        using var caja=new frmCaja();
        var movimientos=Campo<DataGridView>(caja,"movimientos");
        var actual=Campo<CierreCaja>(caja,"actual");
        actual.Movimientos.AddRange(new[]{
            new MovimientoCaja{Venta=2500,Tipo="Cobro venta",Medio="Efectivo",Importe=300},
            new MovimientoCaja{Venta=2500,Tipo="Cobro venta",Medio="Transferencia",Importe=100},
            new MovimientoCaja{Venta=2501,Tipo="Cobro venta",Medio="Transferencia",Importe=200},
            new MovimientoCaja{Tipo="Gasto",Medio="Efectivo",Importe=50},
            new MovimientoCaja{Tipo="Ingreso extra",Moneda="USD",Importe=20}});
        Invocar(caja,"ActualizarResumen");
        Verificar(actual.Ingresos("ARS")==600 && actual.BalanceArs==550 && actual.Ingresos("USD")==20,"Cobros parciales sumados, gastos descontados y USD separado");
        Verificar(movimientos.ReadOnly,"Movimientos de solo lectura");
        string cierre=DocumentosPrana.CierreHtml(actual);
        Verificar(cierre.Contains("RTO 2500") && cierre.Contains("RTO 2501") && cierre.Contains("600,00") && cierre.Contains("550,00"),"Cierre identifica remitos e ingresos reales");
        Verificar(!cierre.Contains("Billete") && !cierre.Contains("Contado") && !cierre.Contains("Diferencia"),"Cierre sin conteo de billetes");
        File.WriteAllText("obj/CajaComprobantes/presentacion/Cierre.html",cierre);
        var pagina=(Panel)caja.Controls["paginaAdaptable"]!;
        caja.Controls.Remove(pagina); pagina.Dock=DockStyle.None; pagina.Size=new Size(840,690); pagina.CreateControl();
        foreach(int ancho in new[]{780,840,1100})
        {
            pagina.Size=new Size(ancho,690); pagina.PerformLayout();
            Verificar(pagina.Controls.Cast<Control>().All(c=>c.Right<=pagina.ClientSize.Width),"Controles dentro del ancho "+ancho);
            pagina.ScrollControlIntoView(pagina.Controls[pagina.Controls.Count-1]);
            Verificar(pagina.Controls[pagina.Controls.Count-1].Bottom<=pagina.ClientSize.Height+1,"Acceso al cierre mediante desplazamiento");
            pagina.AutoScrollPosition=Point.Empty;
        }
        pagina.Dispose();
        Console.WriteLine("PRUEBAS DE PRESENTACIÓN APROBADAS");
    }
}
