using Prana.Datos;
using Prana.Entidades;
using System;
using System.Collections.Generic;

namespace Prana.negocio
{
    // agrupa los datos y operaciones de cn venta
    public class CN_Venta
    {
        // resuelve obtener ultimas ventas con los datos recibidos y entrega el resultado al codigo que lo llama
        public List<venta> ObtenerUltimasVentas() => CD_Venta.Top10Ventas();
        // una busqueda vacia vuelve al resumen de las diez ventas mas recientes
        public List<venta> BuscarVentas(string? texto)
        {
            string filtro = (texto ?? "").Trim();
            if (filtro.Length > 200)
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new ArgumentException("La búsqueda admite hasta 200 caracteres.");
            }

            if (filtro.Length == 0)
            {
                return ObtenerUltimasVentas();
            }
            else
            {
                return CD_Venta.Buscar(filtro);
            }
        }

        // resuelve obtener deudas cliente con los datos recibidos y entrega el resultado al codigo que lo llama
        public List<venta> ObtenerDeudasCliente(int idCliente)
        {
            if (idCliente <= 0)
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new ArgumentException("Seleccione un cliente válido.");
            }

            return CD_Venta.Buscar(null, idCliente);
        }

        // resuelve obtener resumen con los datos recibidos y entrega el resultado al codigo que lo llama
        public ResumenVentas ObtenerResumen() => CD_Venta.ObtenerResumen(DateTime.Today);
    }
}
