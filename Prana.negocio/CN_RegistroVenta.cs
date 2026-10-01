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
        // calcula productos menos descuento y agrega el costo de envío
        public decimal CalcularTotalBase(IEnumerable<DetalleNuevaVenta> detalles, decimal descuento, decimal costoEnvio)
        {
            return Math.Max(0, detalles.Sum(d => d.Cantidad * d.PrecioUnitario) - descuento) + costoEnvio;
        }

        // obtiene el porcentaje del medio según la cantidad de cuotas
        public decimal PorcentajeRecargo(MedioPago medio, int cuotas)
        {
            return cuotas switch { 3 => medio.Recargo3, 6 => medio.Recargo6, 9 => medio.Recargo9, 12 => medio.Recargo12, _ => medio.RecargoMedioPago };
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

        // comprueba los importes y envía la venta completa a datos
        public int Registrar(NuevaVenta venta)
        {
            if (venta.IdCliente <= 0 || venta.IdUsuario <= 0)
                throw new InvalidOperationException("La venta requiere un cliente y un usuario válidos.");
            if (venta.Detalles.Count == 0 || venta.Detalles.Any(d => d.IdVariante <= 0 || d.Cantidad <= 0 || d.PrecioUnitario < 0))
                throw new InvalidOperationException("La venta requiere productos válidos.");
            if (venta.Descuento < 0 || venta.Total < 0 || venta.Pagos.Any(p => p.IdMedioPago <= 0 || p.Monto <= 0 || p.Recargo < 0))
                throw new InvalidOperationException("Los importes de la venta no son válidos.");
            if (venta.FechaPago.Date != DateTime.Today)
                throw new InvalidOperationException("Los cobros se registran con la fecha actual.");
            if (venta.Pagos.Any(p=>p.CuentaDestino.Length>100)) throw new ArgumentException("La cuenta destino admite hasta 100 caracteres.");
            decimal subtotal = venta.Detalles.Sum(d => d.Cantidad * d.PrecioUnitario);
            if (venta.Descuento > subtotal || venta.CostoEnvio < 0)
                throw new InvalidOperationException("El descuento o el envío no son válidos.");
            decimal totalBase = CalcularTotalBase(venta.Detalles, venta.Descuento, venta.CostoEnvio);
            if (venta.Total != totalBase + venta.Pagos.Sum(p => p.Recargo))
                throw new InvalidOperationException("El total de la venta no coincide con sus importes.");
            decimal basePagada = venta.Pagos.Sum(p => p.Monto);
            if (basePagada > totalBase || (venta.CondicionPago == "Pago total" && basePagada != totalBase))
                throw new InvalidOperationException("Los pagos no coinciden con el total de la venta.");
            return CD_RegistroVenta.Guardar(venta);
        }
    }
}
