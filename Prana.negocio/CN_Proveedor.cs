using Prana.Datos;
using Prana.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace Prana.negocio
{
    // agrupa los datos y operaciones de cn proveedor
    public class CN_Proveedor
    {
        // conserva datos proveedor entre las operaciones de esta clase
        private readonly CD_Proveedor datosProveedor = new CD_Proveedor();
        // resuelve nuevo proveedor con los datos recibidos y entrega el resultado al codigo que lo llama
        public bool NuevoProveedor(Proveedor proveedor)
        {
            if (proveedor == null)
            {
                return false;
            }

            proveedor.nombreProveedor = NormalizarTexto.Mayusculas(proveedor.nombreProveedor);
            // distingue un dato vacio de un valor ingresado antes de continuar
            if (string.IsNullOrWhiteSpace(proveedor.nombreProveedor))
            {
                return false;
            }

            return datosProveedor.NuevoProveedor(proveedor);
        }

        // resuelve buscar proveedor con los datos recibidos y entrega el resultado al codigo que lo llama
        public List<Proveedor> buscarProveedor(string nombreProveedor)
        {
            try
            {
                return datosProveedor.buscarProveedor(nombreProveedor) ?? new List<Proveedor>();
            }
            // atiende el error de esta operacion antes de informar o propagar el problema
            catch (Exception)
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw;
            }
        }
    }
}
