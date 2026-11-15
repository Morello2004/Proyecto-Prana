using Prana.Datos;
using Prana.Entidades;
using System;
using System.Collections.Generic;

namespace Prana.negocio
{
    // valida y coordina los medios de pago fijos
    public class CN_MedioPago
    {
        // accede a la capa de datos de medios de pago
        private readonly CD_MedioPago datosMedioPago = new CD_MedioPago();
        // obtiene los medios de pago predefinidos activos
        public List<MedioPago> ObtenerTodos()
        {
            return datosMedioPago.ObtenerTodos();
        }

        // modifica los recargos de un medio de pago predefinido
        public void Modificar(MedioPago medio)
        {
            Validar(medio);
            datosMedioPago.Modificar(medio);
        }

        // valida el medio predefinido y sus porcentajes de recargo comprueba validar; si no se cumple la regla interrumpe con un mensaje antes de continuar
        private void Validar(MedioPago medio)
        {
            if (medio == null)
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new ArgumentNullException(nameof(medio));
            }

            if (medio.IdMedioPago <= 0)
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new ArgumentException("Seleccione un medio de pago válido.");
            }

            if (!EsMedioPredefinido(medio.NombreMedioPago, medio.TipoCaja))
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new ArgumentException("Solo se pueden modificar los cuatro medios de pago predefinidos.");
            }

            if (medio.RecargoMedioPago < 0 || medio.Recargo2 < 0 || medio.Recargo3 < 0 || medio.Recargo6 < 0 || medio.Recargo9 < 0 || medio.Recargo12 < 0)
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new ArgumentException("Los recargos no pueden ser negativos.");
            }

            if (medio.TipoCaja != "Crédito" && (medio.Recargo2 > 0 || medio.Recargo3 > 0 || medio.Recargo6 > 0 || medio.Recargo9 > 0 || medio.Recargo12 > 0))
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new ArgumentException("Solo los medios de tipo Crédito pueden tener recargos por cuotas.");
            }

            if (medio.RecargoMedioPago > 999.99m || medio.Recargo2 > 999.99m || medio.Recargo3 > 999.99m || medio.Recargo6 > 999.99m || medio.Recargo9 > 999.99m || medio.Recargo12 > 999.99m)
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new ArgumentException("Los recargos no pueden superar 999,99 %.");
            }
        }

        // comprueba que nombre y tipo coincidan con uno de los cuatro medios fijos
        private static bool EsMedioPredefinido(string nombre, string tipoCaja)
        {
            return (nombre == "Efectivo" && tipoCaja == "Efectivo") || (nombre == "Transferencia" && tipoCaja == "Transferencia") || (nombre == "Débito" && tipoCaja == "Débito") || (nombre == "Crédito" && tipoCaja == "Crédito");
        }
    }
}
