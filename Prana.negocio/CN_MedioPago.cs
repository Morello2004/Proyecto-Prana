using Prana.Datos;
using Prana.Entidades;
using System;
using System.Collections.Generic;

namespace Prana.negocio
{
    // Valida y coordina el ABM de medios de pago.
    public class CN_MedioPago
    {
        // Accede a la capa de datos de medios de pago.
        private readonly CD_MedioPago datosMedioPago = new CD_MedioPago();

        // Obtiene los medios de pago activos.
        public List<MedioPago> ObtenerTodos()
        {
            return datosMedioPago.ObtenerTodos();
        }

        // Crea un medio de pago después de validar sus datos.
        public void Crear(MedioPago medio)
        {
            Validar(medio);
            datosMedioPago.Crear(medio);
        }

        // Modifica un medio de pago después de validar sus datos.
        public void Modificar(MedioPago medio)
        {
            Validar(medio);
            datosMedioPago.Modificar(medio);
        }

        // Desactiva un medio de pago.
        public void Desactivar(int idMedioPago)
        {
            datosMedioPago.Desactivar(idMedioPago);
        }

        // Valida el nombre y los porcentajes de recargo.
        private void Validar(MedioPago medio)
        {
            if (string.IsNullOrWhiteSpace(medio.NombreMedioPago)) throw new ArgumentException("El nombre del medio de pago es obligatorio.");
            if (medio.RecargoMedioPago < 0 || medio.Recargo3 < 0 || medio.Recargo6 < 0 || medio.Recargo9 < 0 || medio.Recargo12 < 0) throw new ArgumentException("Los recargos no pueden ser negativos.");
        }
    }
}
