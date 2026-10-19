using Prana.Datos;
using Prana.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Prana.negocio
{
    public class CN_Entrega
    {
        private readonly CD_Entrega datos = new();
        private static void ValidarBusqueda(string texto)
        {
            CN_Pago.Verificar(Permisos.ConsultarEntregas);
            if (texto.Length > 200) throw new ArgumentException("La búsqueda admite hasta 200 caracteres.");
        }
        public List<ProductoPorEntregar> Pendientes(string texto) { ValidarBusqueda(texto); return datos.Pendientes(texto.Trim()); }
        public List<EntregaRegistrada> Listar(string texto, bool pendientes) { ValidarBusqueda(texto); return datos.Listar(texto.Trim(), pendientes); }
        public void Preparar(ProductoPorEntregar producto, int cantidad, string direccion, string horario)
        {
            CN_Pago.Verificar(Permisos.GestionarEntregas);
            if (producto == null || producto.IdVenta <= 0 || cantidad <= 0 || cantidad > producto.Pendientes) throw new ArgumentException("Seleccione un producto y una cantidad pendiente válida.");
            if (producto.Modalidad.StartsWith("Retiro", StringComparison.OrdinalIgnoreCase)) direccion = "Retiro en local";
            if (string.IsNullOrWhiteSpace(direccion) || direccion.Trim().Length > 200 || horario.Trim().Length > 50) throw new ArgumentException("Ingrese una dirección (hasta 200 caracteres) y un horario de hasta 50 caracteres.");
            datos.Preparar(producto.IdVenta, producto.IdVariante, cantidad, direccion.Trim(), horario.Trim(), SesionActual.Usuario!.idUsuario);
        }
        public void Confirmar(int envio)
        {
            CN_Pago.Verificar(Permisos.GestionarEntregas);
            if (envio <= 0) throw new ArgumentException("Seleccione una entrega pendiente.");
            datos.Confirmar(envio, SesionActual.Usuario!.idUsuario);
        }

        // Obtiene las variantes cuyo stock disponible no alcanza para cubrir las ventas pendientes.
        public List<AlertaStockPendiente> ObtenerAlertasStock()
        {
            // Comprueba que el usuario tenga permiso para consultar entregas.
            CN_Pago.Verificar(Permisos.ConsultarEntregas);

            // Solicita a la capa de datos las cantidades pendientes y el stock disponible.
            var necesidades = datos.ObtenerNecesidadesStock();

            // Calcula cuántas unidades faltan de cada variante.
            foreach (var item in necesidades)
            {
                // Si el stock alcanza, el faltante queda en cero.
                item.Faltantes = Math.Max(0, item.Pendientes - item.Disponible);
            }

            // Devuelve solamente las variantes que necesitan más stock.
            return necesidades.Where(item => item.Faltantes > 0).ToList();
        }
    }
}
