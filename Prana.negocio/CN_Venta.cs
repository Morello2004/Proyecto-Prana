using Prana.Datos;
using Prana.Entidades;
using System;
using System.Collections.Generic;

namespace Prana.negocio
{
    public class CN_Venta
    {
        public List<venta> ObtenerUltimasVentas() => CD_Venta.Top10Ventas();

        // Una búsqueda vacía vuelve al resumen de las diez ventas más recientes.
        public List<venta> BuscarVentas(string? texto)
        {
            string filtro = (texto ?? "").Trim();
            if (filtro.Length > 200) throw new ArgumentException("La búsqueda admite hasta 200 caracteres.");
            return filtro.Length == 0 ? ObtenerUltimasVentas() : CD_Venta.Buscar(filtro);
        }

        public List<venta> ObtenerDeudasCliente(int idCliente)
        {
            if (idCliente <= 0) throw new ArgumentException("Seleccione un cliente válido.");
            return CD_Venta.Buscar(null, idCliente);
        }

        public ResumenVentas ObtenerResumen() => CD_Venta.ObtenerResumen(DateTime.Today);
    }
}
