using Prana.Datos;
using Prana.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Prana.negocio
{
    public class CN_Pago
    {
        private readonly CD_Pago datos = new();
        public List<venta> Buscar(string texto, bool pendientes)
        {
            Verificar(Permisos.ConsultarPagos);
            if (texto.Length > 200) throw new ArgumentException("La búsqueda admite hasta 200 caracteres.");
            return datos.Buscar(texto.Trim(), pendientes);
        }
        public List<PagoRegistrado> Listar(int venta) { Verificar(Permisos.ConsultarPagos); return datos.Listar(venta); }
        public CobroVenta Preparar(int venta, MedioPago medio, int cuotas, decimal importe)
        {
            if (venta <= 0 || medio == null || !medio.Activo) throw new ArgumentException("Seleccione una venta y un medio activo.");
            if (!new[] { 1, 3, 6, 9, 12 }.Contains(cuotas)) throw new ArgumentException("Seleccione una cantidad de cuotas válida.");
            if (importe <= 0 || importe != decimal.Round(importe, 2)) throw new ArgumentException("Ingrese un importe positivo con hasta dos decimales.");
            decimal porcentaje = new CN_RegistroVenta().PorcentajeRecargo(medio, cuotas);
            return new CobroVenta { IdVenta = venta, IdMedioPago = medio.IdMedioPago, Cuotas = cuotas, ImporteBase = importe,
                Porcentaje = porcentaje, Recargo = new CN_RegistroVenta().CalcularRecargo(importe, porcentaje) };
        }
        public void Registrar(int venta, MedioPago medio, int cuotas, decimal importe)
        {
            Verificar(Permisos.RegistrarPagos);
            datos.Registrar(Preparar(venta, medio, cuotas, importe), SesionActual.Usuario!.idUsuario);
        }
        // El usuario de la operación siempre proviene de la sesión, nunca del formulario.
        internal static void Verificar(string permiso)
        {
            if (SesionActual.Usuario == null || !Permisos.TienePermiso(permiso)) throw new InvalidOperationException("No tiene permisos para esta operación.");
        }
    }
}
