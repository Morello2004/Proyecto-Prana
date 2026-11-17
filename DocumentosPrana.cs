using Prana.Entidades;
using Prana.negocio;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Text;

namespace Prana.Presentacion;
// agrupa los datos y operaciones de documentos prana
public static class DocumentosPrana
{
    // conserva cultura entre las operaciones de esta clase
    private static readonly CultureInfo Cultura = CultureInfo.GetCultureInfo("es-AR");
    // resuelve h con los datos recibidos y entrega el resultado al codigo que lo llama
    private static string H(string? texto) => WebUtility.HtmlEncode(texto ?? "");
    // resuelve dato opcional con los datos recibidos y entrega el resultado al codigo que lo llama
    private static string DatoOpcional(string etiqueta, string? valor, bool bloque = false)
    {
        // distingue un dato vacio de un valor ingresado antes de continuar
        if (string.IsNullOrWhiteSpace(valor))
        {
            return "";
        }
        else
        {
            if (bloque)
            {
                return $"<div><strong>{H(etiqueta)}:</strong> {H(valor)}</div>";
            }
            else
            {
                return $"<br><strong>{H(etiqueta)}:</strong> {H(valor)}";
            }
        }
    }

    // incluye documentos guardados anteriormente con la condicion junto al color
    private static string VarianteImpresa(string? variante) => System.Text.RegularExpressions.Regex.Replace(variante ?? "", @"(?:\s*/\s*|\s+|^)(?:de\s+)?primera\s*$", "", System.Text.RegularExpressions.RegexOptions.IgnoreCase).Trim();
    // resuelve m con los datos recibidos y entrega el resultado al codigo que lo llama
    private static string M(decimal valor) => valor.ToString("N2", Cultura);
    // resuelve logo con los datos recibidos y entrega el resultado al codigo que lo llama
    private static string Logo()
    {
        using Stream stream = typeof(DocumentosPrana).Assembly.GetManifestResourceStream("Prana.LogoComprobante");
        if (stream == null)
        {
            return "<strong class='marca'>PRANA Electrónica</strong>";
        }

        using StreamReader lector = new StreamReader(stream);
        return $"<img class='logo' alt='PRANA Electrónica' src='{lector.ReadToEnd().Trim()}' />";
    }

    // resuelve pagina con los datos recibidos y entrega el resultado al codigo que lo llama
    private static string Pagina(string titulo, string cuerpo) => $$"""
        <!doctype html><html lang="es"><head><meta charset="utf-8"><title>{{H(titulo)}}</title>
        <style>
        @page { size:A4; margin:15mm; } *{box-sizing:border-box} body{font:13px Arial,Helvetica,sans-serif;color:#111;background:#eee;margin:0}
        main{background:white;max-width:180mm;margin:20px auto;padding:12mm} h1{font-size:27px;color:#666;margin:0 0 18px} h2{font-size:16px;margin:20px 0 8px}
        header{display:flex;justify-content:space-between;gap:20px;line-height:1.5} .derecha{text-align:right}.marca{color:#ed2331}.empresa{font-size:12px}
        .cliente{margin:25px 0 20px;line-height:1.8;padding:12px;border:1px solid #aaa;border-radius:8px}
        table{border-collapse:collapse;width:100%;margin:10px 0;table-layout:fixed} th,td{border:1px solid #aaa;padding:8px;overflow-wrap:anywhere;vertical-align:top} th{background:#edf2f2;text-align:left}
        thead{display:table-header-group} tr{break-inside:avoid} .numero{text-align:right;white-space:nowrap}.totales{width:65%;margin:15px 0 15px auto;break-inside:avoid}.totales th{width:62%}
        .nota{font-size:11px;line-height:1.5;overflow-wrap:anywhere}.firma{margin-top:22px;border:1px solid #aaa;border-radius:8px;padding:15px;height:105px;break-inside:avoid}
        .firma span{display:block;text-align:right;margin-top:40px}.logo{width:65mm;max-width:100%} footer{text-align:center;margin-top:25px;break-inside:avoid}
        .acciones{text-align:center;padding:12px}.acciones button{padding:10px 20px;cursor:pointer}.resumen{background:#f5f8ef}.alerta{font-weight:bold}
        @media print{body{background:white}main{padding:0;margin:0;max-width:none}.acciones{display:none}h2{break-after:avoid} }
        </style></head><body><div class="acciones"><button onclick="window.print()">Imprimir / guardar como PDF</button></div><main>{{cuerpo}}</main></body></html>
        """;
    // resuelve venta html con los datos recibidos y entrega el resultado al codigo que lo llama
    public static string VentaHtml(venta venta, bool remito)
    {
        if (remito)
        {
            return RemitoHtml(venta);
        }

        string tipo;
        if (remito)
        {
            tipo = "REMITO";
        }
        else
        {
            tipo = "GARANTÍA";
        }
        StringBuilder html = new StringBuilder($"<header><div class='empresa'><strong class='marca'>PRANA Electrónica</strong><br><i>Electrodomésticos de primera y outlet</i><br>CUIT: 20-23132979-0<br>San Martín 687<br>Luján, Buenos Aires, 6700<br>+54 9 11 2671-0298</div><div class='derecha'><h1>{tipo}</h1><strong>Venta N.º {venta.IdVenta}</strong><br>FECHA: {venta.Fecha:dd/MM/yyyy}</div></header>");
        html.Append($"<section class='cliente'><strong>NOMBRE:</strong> {H(venta.NombreCompletoCliente)}<br><strong>{H(venta.Cliente.TipoDocumento)}:</strong> {H(venta.Cliente.DNI)}<br><strong>TELÉFONO:</strong> {H(venta.Cliente.Telefono)}{DatoOpcional("DIRECCIÓN", venta.Cliente.Direccion)}<br><strong>CIUDAD:</strong> {H(venta.Cliente.NombreLocalidad)}</section>");
        string encabezadoProductos;
        if (remito)
        {
            encabezadoProductos = "<table><thead><tr><th style='width:15%'>Artículo</th><th>Descripción</th><th style='width:16%'>Cantidad</th></tr></thead><tbody>";
        }
        else
        {
            encabezadoProductos = "<table><thead><tr><th style='width:14%'>Cantidad</th><th>Descripción</th></tr></thead><tbody>";
        }
        html.Append(encabezadoProductos);
        // procesa cada detalle producto de ventadetalles sin cambiar el orden de la coleccion
        foreach (DetalleNuevaVenta detalleProducto in venta.Detalles)
        {
            string filaProducto;
            if (remito)
            {
                filaProducto = $"<tr><td>{detalleProducto.IdVariante}</td><td>{H(detalleProducto.Producto)}<br>{H(VarianteImpresa(detalleProducto.Variante))}</td><td class='numero'>{detalleProducto.Cantidad}</td></tr>";
            }
            else
            {
                filaProducto = $"<tr><td>{detalleProducto.Cantidad}</td><td>{H(detalleProducto.Producto)}<br>{H(VarianteImpresa(detalleProducto.Variante))}</td></tr>";
            }
            html.Append(filaProducto);
        }

        html.Append("</tbody></table>");
        if (!remito)
        {
            html.Append("<table class='totales'>");
            foreach ((string etiqueta, decimal valor) in new[]
            {
                ("Total final ARS", venta.Total),
                ("Abonado al emitir (con recargos)", venta.Pagado),
                ("Saldo pendiente al emitir", venta.SaldoPendiente)
            }

            )
                html.Append($"<tr><th>{etiqueta}</th><td class='numero'>{M(valor)}</td></tr>");
            html.Append("</table>");
            bool mostrarCuotas = venta.Pagos.Any(pagoActual => EsCredito(pagoActual.Medio));
            string encabezadoCuotas;
            if (mostrarCuotas)
            {
                encabezadoCuotas = "<th>Cuotas</th>";
            }
            else
            {
                encabezadoCuotas = "";
            }
            html.Append("<h2>Pagos realizados</h2><table><thead><tr><th>Fecha</th><th>Medio de pago</th>" + (encabezadoCuotas) + "<th>Abonado ARS (con recargos)</th></tr></thead><tbody>");
            // procesa cada pago de ventapagos sin cambiar el orden de la coleccion
            foreach (Pago pago in venta.Pagos)
            {
                string celdaCuotas;
                if (mostrarCuotas)
                {
                    string textoCuotas;
                    if (EsCredito(pago.Medio))
                    {
                        textoCuotas = pago.Cuotas.ToString();
                    }
                    else
                    {
                        textoCuotas = "";
                    }

                    celdaCuotas = $"<td>{(textoCuotas)}</td>";
                }
                else
                {
                    celdaCuotas = "";
                }
                html.Append($"<tr><td>{pago.Fecha:dd/MM/yyyy}</td><td>{H(pago.Medio)}</td>" + (celdaCuotas) + $"<td class='numero'>{M(pago.Cobrado)}</td></tr>");
            }

            // comprueba si hay elementos para continuar con esta operacion
            if (venta.Pagos.Count == 0)
            {
                html.Append("<tr><td colspan='3'>Sin pagos registrados.</td></tr>");
            }

            html.Append("</tbody></table>");
        }
        else
        {
            html.Append($"<p><strong>Saldo pendiente al emitir (ARS): {M(venta.SaldoPendiente)}</strong></p><div class='firma'>RECIBÍ CONFORME:<span>____________________________<br>Firma y aclaración</span></div>");
            string lugarEntrega;
            if (venta.ModalidadEntrega.StartsWith("Retiro", StringComparison.OrdinalIgnoreCase))
            {
                lugarEntrega = "Retiro en local";
            }
            else
            {
                lugarEntrega = venta.DireccionEntregaCompleta;
            }
            html.Append($"<section class='cliente'><strong>Lugar de entrega:</strong> {H(lugarEntrega)}<br><strong>Modalidad:</strong> {H(venta.ModalidadEntrega)}<br>{H(venta.ObservacionesEntrega)}</section>");
        }
        string notaDocumento;
        if (remito)
        {
            notaDocumento = "<p class='nota'>Para consultas, comuníquese con PRANA Electrónica al +54 9 11 2671-0298.</p>";
        }
        else
        {
            notaDocumento = "<p class='nota'>Si tiene alguna pregunta relacionada con facturación, póngase en contacto con nosotros al +54 9 11 2671 0298<br>Para validar la garantía, los productos deben estar instalados por un matriculado, recuerde firmar y sellar el documento.</p>";
        }

        html.Append(notaDocumento);
        html.Append($"<footer><strong>¡GRACIAS POR SU CONFIANZA!</strong><br><br>{Logo()}</footer>");
        return Pagina($"{tipo} {venta.IdVenta} - PRANA", html.ToString());
    }

    // resuelve es credito con los datos recibidos y entrega el resultado al codigo que lo llama
    private static bool EsCredito(string medio) => Cultura.CompareInfo.Compare(medio.Trim(), "Crédito", CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace) == 0;
    // resuelve remito html con los datos recibidos y entrega el resultado al codigo que lo llama
    private static string RemitoHtml(venta venta)
    {
        string telefonos;
        // distingue un dato vacio de un valor ingresado antes de continuar
        if (string.IsNullOrWhiteSpace(venta.SegundoTelefono))
        {
            telefonos = venta.Cliente.Telefono;
        }
        else
        {
            telefonos = $"{venta.Cliente.Telefono} / {venta.SegundoTelefono}";
        }

        string Texto(string valor) => H(valor).Replace("\r\n", "\n").Replace("\n", "<br>");
        StringBuilder html = new StringBuilder("""
            <style>
            .remito{border:2px solid #777;padding:4px;font-size:11px}
            .remito .bloque{border:1px solid #888;border-radius:10px;padding:10px;margin-bottom:5px;overflow-wrap:anywhere}
            .remito header{gap:5px;align-items:stretch}.remito .empresa{flex:1;display:flex;gap:12px;font-size:10px;align-items:center}
            .remito .logo{width:29mm}.remito .identificacion{width:34mm;line-height:1.8}
            .remito h1{font-size:16px;color:#111;margin:0 0 8px}.remito .datos{display:grid;grid-template-columns:1fr 1fr;gap:7px 18px}
            .remito .amplio{grid-column:1 / -1}.remito table{margin:0;font-size:11px}
            .remito th{background:#16b98e;color:white;font-size:10px}.remito td{border-color:#ddd;padding:5px;white-space:normal}
            .remito .vacia td{height:6mm;padding:0}.remito .firma-remito{height:24mm;break-inside:avoid}
            .remito .firma-remito span{display:block;width:55mm;text-align:center;border-top:2px solid #111;margin:12mm 0 0 auto;font-size:10px}
            .remito .transporte{min-height:26mm;line-height:1.6;break-inside:avoid;margin-bottom:0}
            </style><article class='remito'><header>
            """);
        html.Append($"<div class='bloque empresa'>{Logo()}<div><strong>Razón social:</strong> PRANA ELECTRÓNICA<br><strong>CUIT:</strong> 20-23132979-0<br><strong>Web:</strong> www.pranaelectronica.com<br><strong>Dirección:</strong> San Martín 687<br><strong>Localidad:</strong> Luján<br><strong>Teléfono:</strong> +54 9 11 2671 0298</div></div>");
        html.Append($"<div class='bloque identificacion'><h1>REMITO</h1><strong>N.º {venta.IdVenta}</strong><br><strong>Fecha:</strong> {venta.Fecha:dd/MM/yyyy}</div></header>");
        html.Append($"<section class='bloque datos'><div><strong>Nombre:</strong> {H(venta.NombreCompletoCliente)}</div><div><strong>Teléfono:</strong> {H(telefonos)}</div><div><strong>{H(venta.Cliente.TipoDocumento)}:</strong> {H(venta.Cliente.DNI)}</div>{DatoOpcional("Dirección", venta.Cliente.Direccion, true)}<div><strong>Ciudad:</strong> {H(venta.Cliente.NombreLocalidad)}</div><div class='amplio'><strong>Otros datos / pago:</strong> {H(venta.CondicionPago)}<br>{Texto(venta.ObservacionPago)}</div></section>");
        html.Append("<section class='bloque'><table><thead><tr><th style='width:15%'>ARTÍCULO</th><th>DESCRIPCIÓN</th><th style='width:17%'>CANTIDAD</th></tr></thead><tbody>");
        // procesa cada detalle producto de ventadetalles sin cambiar el orden de la coleccion
        foreach (DetalleNuevaVenta detalleProducto in venta.Detalles)
        {
            html.Append($"<tr><td>{detalleProducto.IdVariante}</td><td>{Texto(detalleProducto.Producto)}<br>{Texto(VarianteImpresa(detalleProducto.Variante))}</td><td class='numero'>{detalleProducto.Cantidad}</td></tr>");
        }

        for (int i = venta.Detalles.Count; i < 9; i++)
        {
            html.Append("<tr class='vacia'><td>&nbsp;</td><td></td><td></td></tr>");
        }

        html.Append($"</tbody></table><p><strong>Observaciones:</strong><br>{Texto(venta.ObservacionesEntrega)}</p><p><strong>Saldo pendiente al emitir (ARS): {M(venta.SaldoPendiente)}</strong></p></section>");
        html.Append("<section class='bloque firma-remito'><strong>RECIBÍ CONFORME:</strong><span>FIRMA Y SELLO</span></section>");
        string destino;
        if (venta.ModalidadEntrega.StartsWith("Retiro", StringComparison.OrdinalIgnoreCase))
        {
            destino = "Retiro en local";
        }
        else
        {
            destino = venta.DireccionEntregaCompleta;
        }
        html.Append($"<section class='bloque transporte'><strong>DATOS DE ENTREGA</strong><br><strong>Lugar de entrega:</strong> {Texto(destino)}<br><strong>Modalidad:</strong> {H(venta.ModalidadEntrega)} &nbsp; <strong>Logística:</strong> {H(venta.TipoLogistica)}<br><strong>Fecha estimada:</strong> {venta.FechaEntrega:dd/MM/yyyy} &nbsp; <strong>Horario:</strong> {Texto(venta.HorarioSugerido)}</section></article>");
        return Pagina($"REMITO {venta.IdVenta} - PRANA", html.ToString());
    }

    // resuelve cierre html con los datos recibidos y entrega el resultado al codigo que lo llama
    public static string CierreHtml(CierreCaja cierre)
    {
        StringBuilder html = new StringBuilder($"<header><div>{Logo()}</div><div class='derecha'><h1>CIERRE DE CAJA DIARIO</h1><strong>Cierre N.º {cierre.Numero}</strong><br>Fecha: {cierre.Fecha:dd/MM/yyyy}<br>Inicio: {cierre.Inicio:HH:mm}<br>Cierre registrado: {cierre.Fin:dd/MM/yyyy HH:mm}<br>Usuario: {H(cierre.Usuario)}</div></header>");
        html.Append("<h2>Cobros del día por venta / remito</h2><table><thead><tr><th style='width:16%'>Remito</th><th>Cliente / medio</th><th style='width:18%'>Tipo</th><th style='width:23%'>Cobrado ARS</th></tr></thead><tbody>");
        // procesa cada movimiento de cierremovimientoswhere(m => mventahas value) sin cambiar el orden de la coleccion
        foreach (MovimientoCaja movimiento in cierre.Movimientos.Where(m => m.Venta.HasValue))
        {
            html.Append($"<tr><td>RTO {movimiento.Venta}</td><td>{H(movimiento.Concepto)}<br>Cuenta: {H(movimiento.Cuenta)}</td><td>{H(movimiento.Medio)}</td><td class='numero'>{M(movimiento.Importe)}</td></tr>");
        }

        html.Append("</tbody></table><h2>Gastos e ingresos adicionales (incluye café)</h2><table><thead><tr><th>Motivo / cuenta</th><th style='width:20%'>Tipo</th><th style='width:18%'>Medio</th><th style='width:25%'>Importe</th></tr></thead><tbody>");
        // procesa cada m de cierremovimientoswhere(m => !mventahas value) sin cambiar el orden de la coleccion
        foreach (MovimientoCaja m in cierre.Movimientos.Where(m => !m.Venta.HasValue))
        {
            html.Append($"<tr><td>{H(m.Concepto)}<br>{H(m.Cuenta)}</td><td>{H(m.Tipo)}</td><td>{H(m.Medio)}</td><td class='numero'>{H(m.Moneda)} {M(m.Importe)}</td></tr>");
        }

        html.Append("</tbody></table><h2>Resumen ARS</h2><table class='resumen'>");
        // procesa cada medio de tipos cajatodos sin cambiar el orden de la coleccion
        foreach (string medio in TiposCaja.Todos)
        {
            html.Append($"<tr><th>Ingresos: {H(medio)}</th><td class='numero'>{M(cierre.Ingresos("ARS", medio))}</td></tr>");
        }

        html.Append($"<tr><th>De los ingresos, café ARS</th><td class='numero'>{M(cierre.CafeArs)}</td></tr>");
        html.Append($"<tr><th>Total ingresos ARS</th><td class='numero'>{M(cierre.Ingresos("ARS"))}</td></tr><tr><th>Gastos / egresos ARS</th><td class='numero'>{M(cierre.Gastos("ARS"))}</td></tr><tr><th>Balance diario ARS</th><td class='numero'>{M(cierre.BalanceArs)}</td></tr></table>");
        html.Append($"<p class='nota'>USD: ingresos {M(cierre.Ingresos("USD"))}; gastos {M(cierre.Gastos("USD"))}; balance {M(cierre.Ingresos("USD") - cierre.Gastos("USD"))}. Se informan separados, sin conversión a pesos.<br>Los ingresos se suman desde los cobros registrados, identificados por remito. El saldo pendiente de las ventas no se suma como ingreso.</p><p><strong>Observaciones:</strong> {H(cierre.Observaciones)}</p>");
        return Pagina($"Cierre {cierre.Numero} - {cierre.Fecha:yyyy-MM-dd}", html.ToString());
    }

    // resuelve guardar venta con los datos recibidos y entrega el resultado al codigo que lo llama
    public static string GuardarVenta(venta venta, string? carpeta = null)
    {
        carpeta ??= Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "PranaElectronica", "Comprobantes", venta.IdVenta.ToString());
        Directory.CreateDirectory(carpeta);
        Guardar(Path.Combine(carpeta, $"Garantia-{venta.IdVenta}.html"), VentaHtml(venta, false));
        Guardar(Path.Combine(carpeta, $"Remito-{venta.IdVenta}.html"), VentaHtml(venta, true));
        return carpeta;
    }

    // resuelve guardar cierre con los datos recibidos y entrega el resultado al codigo que lo llama
    public static string GuardarCierre(CierreCaja cierre)
    {
        if (cierre.Numero <= 0)
        {
            // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
            throw new InvalidOperationException("Primero debe guardar el cierre de caja.");
        }

        string carpeta = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "PranaElectronica", "Cierres");
        Directory.CreateDirectory(carpeta);
        string ruta = Path.Combine(carpeta, $"Cierre-{cierre.Fecha:yyyy-MM-dd}-{cierre.Numero}.html");
        Guardar(ruta, CierreHtml(cierre));
        return ruta;
    }

    // resuelve guardar con los datos recibidos y entrega el resultado al codigo que lo llama
    private static void Guardar(string ruta, string html)
    {
        string temporal = ruta + "." + Guid.NewGuid().ToString("N") + ".tmp";
        try
        {
            File.WriteAllText(temporal, html, new UTF8Encoding(false));
            File.Move(temporal, ruta, true);
        }
        // realiza la limpieza de salida tanto si la operacion termina bien como si falla
        finally
        {
            if (File.Exists(temporal))
            {
                File.Delete(temporal);
            }
        }
    }

    // resuelve abrir con los datos recibidos y entrega el resultado al codigo que lo llama
    public static void Abrir(string ruta) => Process.Start(new ProcessStartInfo(ruta) { UseShellExecute = true });
    // resuelve mostrar venta con los datos recibidos y entrega el resultado al codigo que lo llama
    public static void MostrarVenta(IWin32Window propietario, int numero, string segundoTelefono = "")
    {
        venta documento = new CN_Comprobante().Obtener(numero);
        documento.SegundoTelefono = segundoTelefono.Trim();
        string carpeta = GuardarVenta(documento);
        using frmComprobantes ventana = new frmComprobantes(numero, carpeta);
        ventana.ShowDialog(propietario);
    }
}
