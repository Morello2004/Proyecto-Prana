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
        // conserva maximo productos por venta entre las operaciones de esta clase
        public const int MaximoProductosPorVenta = 99;
        // conserva maximo importe entre las operaciones de esta clase
        private const decimal MaximoImporte = 99999999.99m;
        // comprueba que el importe pueda guardarse sin desbordes ni redondeos comprueba validar importe; si no se cumple la regla interrumpe con un mensaje antes de continuar
        private static void ValidarImporte(decimal importe, string campo)
        {
            if (importe < 0 || importe > MaximoImporte)
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new InvalidOperationException($"{campo} debe estar entre 0 y 99.999.999,99.");
            }

            if (importe != decimal.Round(importe, 2))
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new InvalidOperationException($"{campo} admite hasta dos decimales.");
            }
        }

        // calcula productos menos descuento y agrega el costo de envio calcula calcular total base a partir de los valores recibidos; revisar las operaciones siguientes para distinguir base, recargo y total
        public decimal CalcularTotalBase(IEnumerable<DetalleNuevaVenta> detalles, decimal descuento, decimal costoEnvio)
        {
            // suma cantidad por precio resta el descuento y agrega el costo de envio
            return Math.Max(0, detalles.Sum(detalleProducto => detalleProducto.Cantidad * detalleProducto.PrecioUnitario) - descuento) + costoEnvio;
        }

        // obtiene el porcentaje del medio segun la cantidad de cuotas
        public decimal PorcentajeRecargo(MedioPago medio, int cuotas)
        {
            // selecciona el recargo de las cuotas indicadas y usa el general para los demas casos
            return cuotas switch
            {
                2 => medio.Recargo2,
                3 => medio.Recargo3,
                6 => medio.Recargo6,
                9 => medio.Recargo9,
                12 => medio.Recargo12,
                _ => medio.RecargoMedioPago
            };
        }

        // calcula el recargo monetario de un importe base calcula calcular recargo a partir de los valores recibidos; revisar las operaciones siguientes para distinguir base, recargo y total
        public decimal CalcularRecargo(decimal montoBase, decimal porcentaje)
        {
            // redondea a dos decimales y resuelve los puntos medios alejandolos de cero
            return Math.Round(montoBase * porcentaje / 100m, 2, MidpointRounding.AwayFromZero);
        }

        // separa el dinero recibido entre base e interes
        public (decimal montoBase, decimal recargo) DesglosarCobro(decimal cobrado, decimal porcentaje)
        {
            // divide el cobro por el factor del recargo para recuperar el importe sin interes
            decimal montoBase = Math.Round(cobrado / (1m + porcentaje / 100m), 2, MidpointRounding.AwayFromZero);
            // devuelve dos valores juntos y calcula el recargo como la diferencia con lo cobrado
            return (montoBase, cobrado - montoBase);
        }

        // comprueba la solicitud sin acceder a controles ni guardar datos comprueba validar; si no se cumple la regla interrumpe con un mensaje antes de continuar
        public void Validar(venta venta)
        {
            ArgumentNullException.ThrowIfNull(venta);
            // normaliza los textos libres sin modificar las opciones de envio ni los medios
            venta.DomicilioEntrega = NormalizarTexto.Mayusculas(venta.DomicilioEntrega);
            venta.HorarioSugerido = NormalizarTexto.Mayusculas(venta.HorarioSugerido);
            if (venta.HorarioSugerido.Length > 50)
                throw new InvalidOperationException("El horario sugerido admite hasta 50 caracteres.");
            venta.ObservacionesEntrega = NormalizarTexto.Mayusculas(venta.ObservacionesEntrega);
            venta.ObservacionPago = NormalizarTexto.Mayusculas(venta.ObservacionPago);
            // la venta no puede tener mas de 99 productos distintos
            if (venta.Detalles.Count > MaximoProductosPorVenta)
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new InvalidOperationException("La venta no puede tener más de 99 productos distintos");
            }

            // la venta debe identificar al cliente y al usuario responsable
            if (venta.IdCliente <= 0)
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new InvalidOperationException("Seleccione un cliente.");
            }

            if (venta.IdUsuario <= 0)
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new InvalidOperationException("No hay usuario autenticado.");
            }

            // cada producto debe tener una variante, cantidad y precio validos
            if (venta.Detalles == null || venta.Detalles.Count == 0)
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new InvalidOperationException("Agregue al menos un producto.");
            }

            if (venta.Detalles.Any(detalleProducto => detalleProducto == null || detalleProducto.IdVariante <= 0 || detalleProducto.Cantidad <= 0 || detalleProducto.PrecioUnitario < 0))
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new InvalidOperationException("La venta requiere productos válidos.");
            }

            // controla el precio y el subtotal que se guardara por cada producto recorre cada detalle de la coleccion; el bloque se aplica a un elemento por vez
            foreach (DetalleNuevaVenta detalle in venta.Detalles)
            {
                ValidarImporte(detalle.PrecioUnitario, "El precio unitario"); // el texto sirve para mostrar el mensaje de error si corresponde
                decimal subtotalProducto = detalle.Cantidad * detalle.PrecioUnitario;
                ValidarImporte(subtotalProducto, $"El subtotal de la variante {detalle.IdVariante}");
            }

            ValidarImporte(venta.Descuento, "El descuento"); // el texto sirve para mostrar el mensaje de error si corresponde
            ValidarImporte(venta.CostoEnvio, "El costo de envío"); // el texto sirve para mostrar el mensaje de error si corresponde el descuento debe estar entre cero y la mitad del subtotal de los productos
            decimal subtotal = venta.Detalles.Sum(d => d.Cantidad * d.PrecioUnitario);
            if (venta.Descuento < 0)
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new InvalidOperationException("El descuento no puede ser negativo.");
            }

            if (venta.Descuento > subtotal * 0.50m)
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new InvalidOperationException("El descuento no puede superar el 50% del subtotal de los productos.");
            }

            // la modalidad debe ser una de las admitidas por el sistema
            if (venta.ModalidadEntrega != "Retiro en el local" && venta.ModalidadEntrega != "Retiro posterior en el local" && venta.ModalidadEntrega != "Envío a domicilio")
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new InvalidOperationException("Seleccione una forma de entrega.");
            }

            venta.LocalidadEntrega = Direcciones.NormalizarLocalidad(venta.LocalidadEntrega);
            if (venta.ModalidadEntrega == "Envío a domicilio")
            {
                venta.DomicilioEntrega = Direcciones.ValidarCalleNumero(venta.DomicilioEntrega, true);
                if (string.IsNullOrWhiteSpace(venta.LocalidadEntrega))
                    throw new InvalidOperationException("Ingrese la localidad de entrega.");
            }
            else
            {
                venta.DomicilioEntrega = string.Empty;
                venta.LocalidadEntrega = string.Empty;
            }
            // un envio requiere domicilio y un tipo de logistica seleccionado
            if (venta.ModalidadEntrega == "Envío a domicilio" && (string.IsNullOrWhiteSpace(venta.DomicilioEntrega) || string.IsNullOrWhiteSpace(venta.TipoLogistica)))
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new InvalidOperationException("Complete domicilio y logística.");
            }

            if (venta.CostoEnvio < 0)
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new InvalidOperationException("El costo del envío no puede ser negativo.");
            }

            // la condicion de pago debe estar definida, sin depender de indices de un combobox
            if (venta.CondicionPago != "Pago parcial" && venta.CondicionPago != "Pago total")
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new InvalidOperationException("Seleccione la condición del pago.");
            }

            // los cobros se registran con la fecha actual
            if (venta.FechaPago.Date != DateTime.Today)
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new InvalidOperationException("Los cobros se registran con la fecha actual.");
            }

            // los pagos deben identificar el medio y contener importes validos
            if (venta.Pagos == null || venta.Total < 0 || venta.Pagos.Any(pagoActual => pagoActual == null || pagoActual.IdMedioPago <= 0 || pagoActual.Monto <= 0 || pagoActual.Recargo < 0))
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new InvalidOperationException("Los importes de la venta no son válidos.");
            }

            if (venta.Pagos.Any(p => (p.CuentaDestino?.Length ?? 0) > 100))
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new ArgumentException("La cuenta destino admite hasta 100 caracteres.");
            }

            decimal totalBase = CalcularTotalBase(venta.Detalles, venta.Descuento, venta.CostoEnvio);
            // suma solo los importes base porque los recargos se comparan por separado
            decimal pagado = venta.Pagos.Sum(p => p.Monto);
            // la suma de los pagos nunca puede superar el total base
            if (pagado > totalBase)
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new InvalidOperationException("Los pagos superan el total actualizado. Ajuste los medios de pago.");
            }

            // el pago total debe cancelar exactamente la venta
            if (venta.CondicionPago == "Pago total" && pagado != totalBase)
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new InvalidOperationException("El pago total debe cubrir la venta.");
            }

            // la sena y el pago parcial deben dejar un saldo pendiente
            if (venta.CondicionPago == "Pago parcial" && pagado == totalBase && totalBase > 0)
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new InvalidOperationException("Ya se cubrió el total de la venta. Seleccione Pago total en la condición del pago inicial.");
            }

            if (venta.CondicionPago != "Pago total" && pagado <= 0)
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new InvalidOperationException("Ingrese un pago inicial parcial.");
            }

            // el total recibido debe coincidir con productos, descuento, envio y recargos
            if (venta.Total != totalBase + venta.Pagos.Sum(p => p.Recargo))
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new InvalidOperationException("El total de la venta no coincide con sus importes.");
            }
        }

        // valida el producto antes de incorporarlo al detalle temporal valida la variante, cantidad y precio y rechaza filas repetidas; devuelve un detalle temporal, sin registrar todavia la venta
        public DetalleNuevaVenta PrepararProducto(producto? producto, VarianteProducto? variante, int cantidad, IEnumerable<DetalleNuevaVenta> detalles)
        {
            if (producto == null || variante == null)
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new InvalidOperationException("Seleccione un producto de la lista.");
            }

            if (variante.IdVariante <= 0 || cantidad <= 0 || variante.Precio < 0)
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new InvalidOperationException("El producto, la cantidad o el precio no son válidos.");
            }

            if (detalles.Any(detalleProducto => detalleProducto.IdVariante == variante.IdVariante))
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new InvalidOperationException("Ese producto y variante ya están agregados. Quítelo de la lista y vuelva a agregarlo con la cantidad requerida.");
            }

            // permite registrar productos pendientes aunque todavia no haya stock la disponibilidad se controla al preparar la entrega o registrar el retiro inmediato
            return new DetalleNuevaVenta
            {
                IdVariante = variante.IdVariante,
                Producto = producto.NombreProducto,
                Variante = variante.Color + " " + variante.CondicionProducto,
                Cantidad = cantidad,
                PrecioUnitario = variante.Precio,
                StockFisico = variante.StockFisico,
                StockComprometido = variante.StockComprometido

            };
        }

        // valida y desglosa un cobro antes de incorporarlo a los pagos temporales separa el cobro recibido en base y recargo y comprueba que la base no supere lo pendiente de la venta en preparacion
        public Pago PrepararPago(MedioPago? medio, int cuotas, decimal cobrado, decimal totalBase, IEnumerable<Pago> pagos, string cuentaDestino)
        {
            if (medio == null || medio.IdMedioPago <= 0 || !medio.Activo || cobrado <= 0)
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new InvalidOperationException("Seleccione un medio activo e ingrese un monto.");
            }

            if (cuotas != 1 && cuotas != 2 && cuotas != 3 && cuotas != 6 && cuotas != 9 && cuotas != 12)
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new InvalidOperationException("Seleccione una cantidad de cuotas válida.");
            }

            if (cuotas > 1 && medio.TipoCaja != "Crédito")
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new InvalidOperationException("Solo los medios de tipo Crédito admiten pagos en cuotas.");
            }

            if ((cuentaDestino?.Length ?? 0) > 100)
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new ArgumentException("La cuenta destino admite hasta 100 caracteres.");
            }

            decimal porcentaje = PorcentajeRecargo(medio, cuotas);
            if (porcentaje < 0)
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new InvalidOperationException("El recargo no puede ser negativo.");
            }

            // separa los dos valores devueltos por el metodo en variables individuales
            (decimal montoBase, decimal recargo) = DesglosarCobro(cobrado, porcentaje);
            // suma el nuevo importe base a los pagos anteriores para evitar superar el saldo
            if (montoBase <= 0 || pagos.Sum(pagoActual => pagoActual.Monto) + montoBase > totalBase)
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new InvalidOperationException("El importe cobrado supera el saldo de la venta.");
            }

            return new Pago
            {
                CuentaDestino = cuentaDestino?.Trim() ?? string.Empty,
                IdMedioPago = medio.IdMedioPago,
                Medio = medio.NombreMedioPago,
                Monto = montoBase,
                Recargo = recargo,
                Cuotas = cuotas

            };
        }

        // toda venta pasa por las reglas de negocio antes de llegar a datos
        public int Registrar(venta venta)
        {
            // comprueba las reglas antes de abrir la conexion y comenzar las escrituras
            Validar(venta);
            // el retiro inmediato se registra como entregado
            bool retiroInmediato = venta.ModalidadEntrega == "Retiro en el local";
            return CD_RegistroVenta.Guardar(venta, retiroInmediato);
        }
    }
}
