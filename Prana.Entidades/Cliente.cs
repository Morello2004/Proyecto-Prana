using System;
using System.Collections.Generic;
using System.Text;

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
        // codigo postal como texto, para admitir tanto numeros como letras del cpa
        public string CodigoPostal { get; set; } = string.Empty;
        // conserva direccion como string para intercambiar este dato entre las capas
        public string Direccion { get; set; } = string.Empty;
        // conserva id localidad como int para intercambiar este dato entre las capas
        public int IdLocalidad
        {
            get; set;
        }
        // conserva localidad como localidad para intercambiar este dato entre las capas
        public Localidad Localidad { get; set; } = new Localidad();
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

        // devuelve el nombre de la localidad para mostrarlo en el grid permite mostrar y modificar el nombre de la localidad desde el grid
        public string NombreLocalidad
        {
            // devuelve el nombre de la localidad
            get
            {
                return Localidad?.NombreLocalidad ?? string.Empty;
            }

            // guarda el nombre ingresado dentro del objeto localidad
            set
            {
                if (Localidad == null)
                {
                    Localidad = new Localidad();
                }

                Localidad.NombreLocalidad = value;
            }
        }
    }
}
