using Prana.Datos;
using Prana.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Prana.negocio
{
    // valida la venta y coordina su registro en la capa de datos
    public class CN_RegistroVenta
    {
        public const int MaximoProductosPorVenta = 99;

        // calcula productos menos descuento y agrega el costo de envío
        public decimal CalcularTotalBase(IEnumerable<DetalleNuevaVenta> detalles, decimal descuento, decimal costoEnvio)
        {
            return Math.Max(0, detalles.Sum(d => d.Cantidad * d.PrecioUnitario) - descuento) + costoEnvio;
        }

        // obtiene el porcentaje del medio según la cantidad de cuotas
        public decimal PorcentajeRecargo(MedioPago medio, int cuotas)
        {
            return cuotas switch { 2 => medio.Recargo2, 3 => medio.Recargo3, 6 => medio.Recargo6, 9 => medio.Recargo9, 12 => medio.Recargo12, _ => medio.RecargoMedioPago };
        }

        // calcula el recargo monetario de un importe base
        public decimal CalcularRecargo(decimal montoBase, decimal porcentaje)
        {
            return Math.Round(montoBase * porcentaje / 100m, 2, MidpointRounding.AwayFromZero);
        }

        // separa el dinero recibido entre base e interés
        public (decimal montoBase, decimal recargo) DesglosarCobro(decimal cobrado, decimal porcentaje)
        {
            decimal montoBase = Math.Round(cobrado / (1m + porcentaje / 100m), 2, MidpointRounding.AwayFromZero);
            return (montoBase, cobrado - montoBase);
        }

        // Comprueba la solicitud sin acceder a controles ni guardar datos.
        public void Validar(NuevaVenta venta)
        {
            ArgumentNullException.ThrowIfNull(venta);

            //la venta no puede tener mas de 99 productos distintos.
            if (venta.Detalles.Count > MaximoProductosPorVenta)
            {
                throw new InvalidOperationException("La venta no puede tener más de 99 productos distintos");
            }

            // La venta debe identificar al cliente y al usuario responsable.
            if (venta.IdCliente <= 0)
                throw new InvalidOperationException("Seleccione un cliente.");
            if (venta.IdUsuario <= 0)
                throw new InvalidOperationException("No hay usuario autenticado.");

            // Cada producto debe tener una variante, cantidad y precio válidos.
            if (venta.Detalles == null || venta.Detalles.Count == 0)
                throw new InvalidOperationException("Agregue al menos un producto.");
            if (venta.Detalles.Any(d => d == null || d.IdVariante <= 0 || d.Cantidad <= 0 || d.PrecioUnitario < 0))
                throw new InvalidOperationException("La venta requiere productos válidos.");

            // El descuento no puede ser negativo ni superar el subtotal.
            decimal subtotal = venta.Detalles.Sum(d => d.Cantidad * d.PrecioUnitario);
            if (venta.Descuento < 0)
                throw new InvalidOperationException("El descuento no puede ser negativo.");
            if (venta.Descuento > subtotal)
                throw new InvalidOperationException("El descuento no puede superar el subtotal.");

            // La modalidad debe ser una de las admitidas por el sistema.
            if (venta.ModalidadEntrega != "Retiro en el local" &&
                venta.ModalidadEntrega != "Retiro posterior en el local" &&
                venta.ModalidadEntrega != "Envío a domicilio")
                throw new InvalidOperationException("Seleccione una forma de entrega.");

            // Un envío requiere domicilio y un tipo de logística seleccionado.
            if (venta.ModalidadEntrega == "Envío a domicilio" &&
                (string.IsNullOrWhiteSpace(venta.DomicilioEntrega) || string.IsNullOrWhiteSpace(venta.TipoLogistica)))
                throw new InvalidOperationException("Complete domicilio y logística.");
            if (venta.CostoEnvio < 0)
                throw new InvalidOperationException("El costo del envío no puede ser negativo.");

            // La condición de pago debe estar definida, sin depender de índices de un ComboBox.
            if (venta.CondicionPago != "Pago parcial" && venta.CondicionPago != "Pago total")
                throw new InvalidOperationException("Seleccione la condición del pago.");

            // Los cobros se registran con la fecha actual.
            if (venta.FechaPago.Date != DateTime.Today)
                throw new InvalidOperationException("Los cobros se registran con la fecha actual.");

            // Los pagos deben identificar el medio y contener importes válidos.
            if (venta.Pagos == null || venta.Total < 0 ||
                venta.Pagos.Any(p => p == null || p.IdMedioPago <= 0 || p.Monto <= 0 || p.Recargo < 0))
                throw new InvalidOperationException("Los importes de la venta no son válidos.");
            if (venta.Pagos.Any(p => (p.CuentaDestino?.Length ?? 0) > 100))
                throw new ArgumentException("La cuenta destino admite hasta 100 caracteres.");

            decimal totalBase = CalcularTotalBase(venta.Detalles, venta.Descuento, venta.CostoEnvio);
            decimal pagado = venta.Pagos.Sum(p => p.Monto);

            // La suma de los pagos nunca puede superar el total base.
            if (pagado > totalBase)
                throw new InvalidOperationException("Los pagos superan el total actualizado. Ajuste los medios de pago.");

            // El pago total debe cancelar exactamente la venta.
            if (venta.CondicionPago == "Pago total" && pagado != totalBase)
                throw new InvalidOperationException("El pago total debe cubrir la venta.");

            // La seña y el pago parcial deben dejar un saldo pendiente.
            if (venta.CondicionPago != "Pago total" && (pagado <= 0 || pagado >= totalBase))
                throw new InvalidOperationException("Ingrese un pago inicial parcial.");

            // El total recibido debe coincidir con productos, descuento, envío y recargos.
            if (venta.Total != totalBase + venta.Pagos.Sum(p => p.Recargo))
                throw new InvalidOperationException("El total de la venta no coincide con sus importes.");
        }

        // Valida el producto antes de incorporarlo al detalle temporal.
        public DetalleNuevaVenta PrepararProducto(producto? producto, VarianteProducto? variante,
            int cantidad, IEnumerable<DetalleNuevaVenta> detalles)
        {
            if (producto == null || variante == null)
                throw new InvalidOperationException("Seleccione un producto de la lista.");
            if (variante.IdVariante <= 0 || cantidad <= 0 || variante.Precio < 0)
                throw new InvalidOperationException("El producto, la cantidad o el precio no son válidos.");
            // Permite registrar productos pendientes aunque todavía no haya stock.
            // La disponibilidad se controla al preparar la entrega o registrar el retiro inmediato.
            return new DetalleNuevaVenta
            {
                IdVariante = variante.IdVariante, Producto = producto.NombreProducto,
                Variante = variante.Color + " " + variante.CondicionProducto,
                Cantidad = cantidad, PrecioUnitario = variante.Precio
            };
        }

        // Valida y desglosa un cobro antes de incorporarlo a los pagos temporales.
        public PagoNuevaVenta PrepararPago(MedioPago? medio, int cuotas, decimal cobrado,
            decimal totalBase, IEnumerable<PagoNuevaVenta> pagos, string cuentaDestino)
        {
            if (medio == null || medio.IdMedioPago <= 0 || !medio.Activo || cobrado <= 0)
                throw new InvalidOperationException("Seleccione un medio activo e ingrese un monto.");
            if (cuotas != 1 && cuotas != 2 && cuotas != 3 && cuotas != 6 && cuotas != 9 && cuotas != 12)
                throw new InvalidOperationException("Seleccione una cantidad de cuotas válida.");
            if (cuotas > 1 && medio.TipoCaja != "Crédito")
                throw new InvalidOperationException("Solo los medios de tipo Crédito admiten pagos en cuotas.");
            if ((cuentaDestino?.Length ?? 0) > 100)
                throw new ArgumentException("La cuenta destino admite hasta 100 caracteres.");
            decimal porcentaje = PorcentajeRecargo(medio, cuotas);
            if (porcentaje < 0)
                throw new InvalidOperationException("El recargo no puede ser negativo.");
            var (montoBase, recargo) = DesglosarCobro(cobrado, porcentaje);
            if (montoBase <= 0 || pagos.Sum(p => p.Monto) + montoBase > totalBase)
                throw new InvalidOperationException("El importe cobrado supera el saldo de la venta.");
            return new PagoNuevaVenta
            {
                CuentaDestino = cuentaDestino?.Trim() ?? string.Empty,
                IdMedioPago = medio.IdMedioPago, Medio = medio.NombreMedioPago,
                Monto = montoBase, Recargo = recargo, Cuotas = cuotas
            };
        }

        // Toda venta pasa por las reglas de negocio antes de llegar a datos.
        public int Registrar(NuevaVenta venta)
        {
            Validar(venta);
            // El retiro inmediato se registra como entregado
            bool retiroInmediato = venta.ModalidadEntrega == "Retiro en el local";

            return CD_RegistroVenta.Guardar(venta,retiroInmediato);
        }
    }
}
