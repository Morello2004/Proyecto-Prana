using System;
using System.Collections.Generic;
using Prana.Entidades;
using Prana.negocio;

internal static class Program
{
    private static readonly CN_RegistroVenta Negocio = new();
    private static int comprobaciones;
    private static NuevaVenta Venta() => new()
    {
        IdCliente = 1, IdUsuario = 1, FechaPago = DateTime.Today,
        ModalidadEntrega = "Retiro en el local", CondicionPago = "Pago total", Total = 100,
        Detalles = [new() { IdVariante = 1, Cantidad = 1, PrecioUnitario = 100 }],
        Pagos = [new() { IdMedioPago = 1, Monto = 100, Cuotas = 1 }]
    };
    private static void Rechaza(Action accion, string mensaje)
    {
        try { accion(); }
        catch (Exception e) when (e is InvalidOperationException || e is ArgumentException)
        {
            if (!e.Message.Contains(mensaje)) throw new Exception($"Error inesperado: {e.Message}");
            comprobaciones++; return;
        }
        throw new Exception($"No rechazó: {mensaje}");
    }
    private static void Invalida(Action<NuevaVenta> cambiar, string mensaje)
    {
        var venta = Venta(); cambiar(venta);
        // Registrar debe rechazar la solicitud antes de abrir una conexión a datos.
        Rechaza(() => Negocio.Registrar(venta), mensaje);
    }
    private static void Main()
    {
        Negocio.Validar(Venta()); comprobaciones++;
        Invalida(v => v.IdCliente = 0, "cliente");
        Invalida(v => v.IdUsuario = 0, "usuario");
        Invalida(v => v.Detalles.Clear(), "producto");
        Invalida(v => v.Detalles[0].Cantidad = 0, "productos válidos");
        Invalida(v => v.Descuento = -1, "negativo");
        Invalida(v => v.Descuento = 101, "subtotal");
        Invalida(v => v.ModalidadEntrega = "", "forma de entrega");
        Invalida(v => { v.ModalidadEntrega = "Envío a domicilio"; v.TipoLogistica = "Propia"; }, "domicilio y logística");
        Invalida(v => { v.ModalidadEntrega = "Envío a domicilio"; v.DomicilioEntrega = "Calle 1"; }, "domicilio y logística");
        Invalida(v => v.CostoEnvio = -1, "negativo");
        Invalida(v => v.CondicionPago = "", "condición");
        Invalida(v => v.CondicionPago = "inexistente", "condición");
        Invalida(v => v.FechaPago = DateTime.Today.AddDays(-1), "fecha actual");
        Invalida(v => v.FechaPago = DateTime.Today.AddDays(1), "fecha actual");
        Invalida(v => v.Pagos[0].Monto = -1, "importes");
        Invalida(v => v.Pagos[0].CuentaDestino = new string('x', 101), "100 caracteres");
        Invalida(v => v.Pagos[0].Monto = 101, "superan el total");
        Invalida(v => v.Pagos[0].Monto = 99, "cubrir la venta");
        foreach (var condicion in new[] { "Pago parcial" })
        {
            Invalida(v => { v.CondicionPago = condicion; v.Pagos.Clear(); }, "pago inicial parcial");
            Invalida(v => v.CondicionPago = condicion, "pago inicial parcial");
            var parcial = Venta(); parcial.CondicionPago = condicion; parcial.Pagos[0].Monto = 40;
            Negocio.Validar(parcial); comprobaciones++;
        }
        Invalida(v => v.Total = 101, "no coincide");
        var envio = Venta(); envio.ModalidadEntrega = "Envío a domicilio";
        envio.DomicilioEntrega = "Calle 1"; envio.TipoLogistica = "Propia";
        envio.CostoEnvio = 10; envio.Total = 115; envio.Pagos[0].Monto = 110; envio.Pagos[0].Recargo = 5;
        Negocio.Validar(envio); comprobaciones++;
        var producto = new producto { NombreProducto = "Heladera" };
        var variante = new VarianteProducto { IdVariante = 1, Precio = 100, StockFisico = 5, StockComprometido = 2 };
        Rechaza(() => Negocio.PrepararProducto(null, null, 1, []), "Seleccione un producto");
        var sinDisponible = Negocio.PrepararProducto(producto, variante, 4, []);
        if (sinDisponible.Cantidad != 4) throw new Exception("Debe permitir superar el stock libre al agregar");
        variante.StockFisico = 0;
        var sinStock = Negocio.PrepararProducto(producto, variante, 2, [new() { IdVariante = 1, Cantidad = 2 }]);
        if (sinStock.Cantidad != 2) throw new Exception("Debe permitir agregar productos sin stock físico");
        comprobaciones += 2;
        Rechaza(() => Negocio.PrepararProducto(producto, variante, 0, []), "no son válidos");
        var detalle = Negocio.PrepararProducto(producto, variante, 3, []);
        if (detalle.Cantidad != 3 || detalle.PrecioUnitario != 100) throw new Exception("Detalle incorrecto");
        comprobaciones++;
        var medio = new MedioPago { IdMedioPago = 1, Activo = true, RecargoMedioPago = 10 };
        Rechaza(() => Negocio.PrepararPago(null, 1, 10, 100, [], ""), "medio activo");
        Rechaza(() => Negocio.PrepararPago(medio, 1, 111, 100, [], ""), "supera el saldo");
        Rechaza(() => Negocio.PrepararPago(medio, 4, 10, 100, [], ""), "cuotas válida");
        var pago = Negocio.PrepararPago(medio, 1, 110, 100, [], " referencia ");
        if (pago.Monto != 100 || pago.Recargo != 10 || pago.CuentaDestino != "referencia") throw new Exception("Cobro incorrecto");
        comprobaciones++;
        Console.WriteLine($"OK: {comprobaciones} comprobaciones de negocio, sin controles ni acceso a base de datos.");
    }
}
