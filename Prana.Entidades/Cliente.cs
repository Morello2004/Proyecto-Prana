using System;
using System.Collections.Generic;
using System.Text;

namespace Prana.Entidades

{
    public class Cliente
    {
        public int IdCliente { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string DNI { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public DateTime FechaRegistro { get; set; }
        public string Direccion { get; set; } = string.Empty;
        public int IdLocalidad { get; set; }
        public Localidad Localidad { get; set; } = new Localidad();
        public decimal SaldoFavor { get; set; }

        // devuelve el nombre de la localidad para mostrarlo en el grid
        // permite mostrar y modificar el nombre de la localidad desde el grid
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
