using Prana.Datos;
using Prana.Entidades;
using System;
using System.Collections.Generic;

namespace Prana.negocio
{
    // Valida y coordina los medios de pago fijos.
    public class CN_MedioPago
    {
        // Accede a la capa de datos de medios de pago.
        private readonly CD_MedioPago datosMedioPago = new CD_MedioPago();

        // Obtiene los medios de pago predefinidos activos.
        public List<MedioPago> ObtenerTodos()
        {
            return datosMedioPago.ObtenerTodos();
        }

        // Modifica los recargos de un medio de pago predefinido.
        public void Modificar(MedioPago medio)
        {
            Validar(medio);
            datosMedioPago.Modificar(medio);
        }

        // Valida el medio predefinido y sus porcentajes de recargo.
        private void Validar(MedioPago medio)
        {
            if (medio == null) throw new ArgumentNullException(nameof(medio));
            if (medio.IdMedioPago <= 0) throw new ArgumentException("Seleccione un medio de pago válido.");
            if (!EsMedioPredefinido(medio.NombreMedioPago, medio.TipoCaja)) throw new ArgumentException("Solo se pueden modificar los cuatro medios de pago predefinidos.");
            if (medio.RecargoMedioPago < 0 || medio.Recargo2 < 0 || medio.Recargo3 < 0 || medio.Recargo6 < 0 || medio.Recargo9 < 0 || medio.Recargo12 < 0) throw new ArgumentException("Los recargos no pueden ser negativos.");
            if (medio.TipoCaja != "Crédito" && (medio.Recargo2 > 0 || medio.Recargo3 > 0 || medio.Recargo6 > 0 || medio.Recargo9 > 0 || medio.Recargo12 > 0)) throw new ArgumentException("Solo los medios de tipo Crédito pueden tener recargos por cuotas.");
            if (medio.RecargoMedioPago > 999.99m || medio.Recargo2 > 999.99m || medio.Recargo3 > 999.99m || medio.Recargo6 > 999.99m || medio.Recargo9 > 999.99m || medio.Recargo12 > 999.99m) throw new ArgumentException("Los recargos no pueden superar 999,99 %.");
        }

        // Comprueba que nombre y tipo coincidan con uno de los cuatro medios fijos.
        private static bool EsMedioPredefinido(string nombre, string tipoCaja)
        {
            return (nombre == "Efectivo" && tipoCaja == "Efectivo")
                || (nombre == "Transferencia" && tipoCaja == "Transferencia")
                || (nombre == "Débito" && tipoCaja == "Débito")
                || (nombre == "Crédito" && tipoCaja == "Crédito");
        }
    }
}
