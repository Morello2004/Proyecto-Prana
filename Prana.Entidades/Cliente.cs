using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Prana.Entidades
{
    // agrupa los datos y operaciones de cliente
    public class Cliente
    {
        // identificador del cliente; permite distinguir personas aunque tengan el mismo nombre
        public int IdCliente
        {
            get; set;
        }
        // conserva nombre como string para intercambiar este dato entre las capas
        public string Nombre { get; set; } = string.Empty;
        // conserva apellido como string para intercambiar este dato entre las capas
        public string Apellido { get; set; } = string.Empty;
        // indica si el numero informado corresponde a dni o cuit
        public string TipoDocumento { get; set; } = "DNI";
        // numero del documento sin separadores; tipodocumento indica como debe validarse
        public string DNI { get; set; } = string.Empty;
        // conserva telefono como string para intercambiar este dato entre las capas
        public string Telefono { get; set; } = string.Empty;
        // conserva fecha registro como date time para intercambiar este dato entre las capas
        public DateTime FechaRegistro
        {
            get; set;
        }
        // conserva direccion como string para intercambiar este dato entre las capas
        public string Direccion { get; set; } = string.Empty;
        // conserva saldo favor como decimal para intercambiar este dato entre las capas
        public decimal SaldoFavor
        {
            get; set;
        }
        // marca si el registro esta habilitado para el uso habitual; no elimina su historial
        public bool Activo { get; set; } = true;
        // marca la baja logica del cliente; sus operaciones anteriores se conservan
        public bool Eliminado
        {
            get; set;
        }
        // saldo de ventas sin cancelar; se consulta, no se edita en el cliente
        public decimal DeudaPendiente
        {
            get; set;
        }
        // dato estado cuenta de tipo string se obtiene al consultar la propiedad mediante la expresion siguiente
        public string EstadoCuenta
        {
            get
            {
                if (DeudaPendiente > 0)
                {
                    return "Con deuda";
                }
                else
                {
                    return "Sin deuda";
                }
            }
        }

        public string NombreLocalidad { get; set; } = string.Empty;
        public string DireccionCompleta => string.Join(", ", new[] { Direccion, NombreLocalidad }.Where(x => !string.IsNullOrWhiteSpace(x)));
    }
}
