using Prana.Datos;
using Prana.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace Prana.negocio
{
    public class CN_Proveedor
    {
        private readonly CD_Proveedor datosProveedor = new CD_Proveedor();

        public bool NuevoProveedor(Proveedor proveedor)
        {
            if (proveedor == null)
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(proveedor.nombreProveedor))
            {
                return false;
            }

            return datosProveedor.NuevoProveedor(proveedor);
        }

        public List<Proveedor> buscarProveedor(string nombreProveedor)
        {
            try
            {
                return datosProveedor.buscarProveedor(nombreProveedor) ?? new List<Proveedor>();
            }
            catch (Exception)
            {
                throw;
            }
        }





    }
}
