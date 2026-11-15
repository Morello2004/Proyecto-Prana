using Prana.Datos;
using Prana.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Prana.negocio
{
    // agrupa los datos y operaciones de cn entrega
    public class CN_Entrega
    {
        // conserva datos entre las operaciones de esta clase
        private readonly CD_Entrega datos = new();
        // comprueba validar busqueda; si no se cumple la regla interrumpe con un mensaje antes de continuar
        private static void ValidarBusqueda(string texto)
        {
            Permisos.Exigir(Permisos.ConsultarEntregas);
            if (texto.Length > 200)
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new ArgumentException("La búsqueda admite hasta 200 caracteres.");
            }
        }

        // resuelve pendientes con los datos recibidos y entrega el resultado al codigo que lo llama
        public List<ProductoPorEntregar> Pendientes(string texto)
        {
            ValidarBusqueda(texto);
            return datos.Pendientes(texto.Trim());
        }

        // resuelve listar con los datos recibidos y entrega el resultado al codigo que lo llama
        public List<EntregaRegistrada> Listar(string texto, bool pendientes)
        {
            ValidarBusqueda(texto);
            return datos.Listar(texto.Trim(), pendientes);
        }

        // resuelve preparar con los datos recibidos y entrega el resultado al codigo que lo llama
        public void Preparar(ProductoPorEntregar producto, int cantidad, string direccion, string horario)
        {
            Permisos.Exigir(Permisos.GestionarEntregas);
            direccion = NormalizarTexto.Mayusculas(direccion);
            horario = NormalizarTexto.Mayusculas(horario);
            if (producto == null || producto.IdVenta <= 0 || cantidad <= 0 || cantidad > producto.Pendientes)
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new ArgumentException("Seleccione un producto y una cantidad pendiente válida.");
            }

            if (producto.Modalidad.StartsWith("Retiro", StringComparison.OrdinalIgnoreCase))
            {
                direccion = "Retiro en local";
            }

            // distingue un dato vacio de un valor ingresado antes de continuar
            if (string.IsNullOrWhiteSpace(direccion) || direccion.Trim().Length > 200 || horario.Trim().Length > 50)
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new ArgumentException("Ingrese una dirección (hasta 200 caracteres) y un horario de hasta 50 caracteres.");
            }

            datos.Preparar(producto.IdVenta, producto.IdVariante, cantidad, direccion.Trim(), horario.Trim(), SesionActual.Usuario!.idUsuario);
        }

        // resuelve confirmar con los datos recibidos y entrega el resultado al codigo que lo llama
        public void Confirmar(int envio)
        {
            Permisos.Exigir(Permisos.GestionarEntregas);
            if (envio <= 0)
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new ArgumentException("Seleccione una entrega pendiente.");
            }

            datos.Confirmar(envio, SesionActual.Usuario!.idUsuario);
        }

        // obtiene las variantes cuyo stock disponible no alcanza para cubrir las ventas pendientes
        public List<AlertaStockPendiente> ObtenerAlertasStock()
        {
            // comprueba que el usuario tenga permiso para consultar entregas
            Permisos.Exigir(Permisos.ConsultarEntregas);
            // solicita a la capa de datos las cantidades pendientes y el stock disponible
            List<AlertaStockPendiente> necesidades = datos.ObtenerNecesidadesStock();
            // calcula cuantas unidades faltan de cada variante
            foreach (AlertaStockPendiente item in necesidades)
            {
                // si el stock alcanza, el faltante queda en cero
                item.Faltantes = Math.Max(0, item.Pendientes - item.Disponible);
            }

            // devuelve solamente las variantes que necesitan mas stock
            return necesidades.Where(item => item.Faltantes > 0).ToList();
        }
    }
}
