using Prana.Datos;
using Prana.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Prana.negocio
{
    // agrupa los datos y operaciones de cn pago
    public class CN_Pago
    {
        // reutiliza las mismas reglas de calculo para cotizar la base y el recargo del cobro
        private readonly CN_RegistroVenta calculadoraVenta = new();
        // conserva datos entre las operaciones de esta clase
        private readonly CD_Pago datos = new();
        // resuelve buscar con los datos recibidos y entrega el resultado al codigo que lo llama
        public List<venta> Buscar(string texto, bool pendientes)
        {
            Permisos.Exigir(Permisos.ConsultarPagos);
            if (texto.Length > 200)
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new ArgumentException("La búsqueda admite hasta 200 caracteres.");
            }

            return datos.Buscar(texto.Trim(), pendientes);
        }

        // resuelve listar con los datos recibidos y entrega el resultado al codigo que lo llama
        public List<Pago> Listar(int venta)
        {
            Permisos.Exigir(Permisos.ConsultarPagos);
            return datos.Listar(venta);
        }

        // resuelve preparar con los datos recibidos y entrega el resultado al codigo que lo llama
        public Pago Preparar(int venta, MedioPago medio, int cuotas, decimal importe)
        {
            if (venta <= 0 || medio == null || !medio.Activo)
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new ArgumentException("Seleccione una venta y un medio activo.");
            }

            if (!new[]
            {
                1,
                2,
                3,
                6,
                9,
                12
            }.Contains(cuotas))
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new ArgumentException("Seleccione una cantidad de cuotas válida.");
            }

            if (cuotas > 1 && medio.TipoCaja != "Crédito")
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new ArgumentException("Solo los medios de tipo Crédito admiten pagos en cuotas.");
            }

            if (importe <= 0 || importe != decimal.Round(importe, 2))
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new ArgumentException("Ingrese un importe positivo con hasta dos decimales.");
            }

            decimal porcentaje = calculadoraVenta.PorcentajeRecargo(medio, cuotas);
            if (porcentaje < 0)
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new ArgumentException("El recargo no puede ser negativo.");
            }

            decimal recargo = calculadoraVenta.CalcularRecargo(importe, porcentaje);
            return new Pago
            {
                IdVenta = venta,
                IdMedioPago = medio.IdMedioPago,
                Cuotas = cuotas,
                ImporteBase = importe,
                Porcentaje = porcentaje,
                Recargo = recargo,
                Cobrado = importe + recargo

            };
        }

        // resuelve registrar con los datos recibidos y entrega el resultado al codigo que lo llama
        public void Registrar(int venta, MedioPago medio, int cuotas, decimal importe, string cuentaDestino = "")
        {
            Permisos.Exigir(Permisos.RegistrarPagos);
            if (cuentaDestino.Trim().Length > 100)
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new ArgumentException("La cuenta admite hasta 100 caracteres.");
            }

            Pago cobro = Preparar(venta, medio, cuotas, importe);
            cobro.CuentaDestino = cuentaDestino.Trim();
            datos.Registrar(cobro, SesionActual.Usuario!.idUsuario);
        }
    }
}
