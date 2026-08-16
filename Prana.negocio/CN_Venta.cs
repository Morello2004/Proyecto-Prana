using Prana.Datos;
using Prana.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace Prana.negocio
{
    public class CN_Venta
    {
        public List<venta> ObtenerUltimasVentas()
        {
            return CD_Venta.Top10Ventas();
        }
    }
}
