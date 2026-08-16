using System;
using System.Collections.Generic;
using System.Text;

namespace Prana.Entidades
{
    public class usuario
    {
        public int idUsuario { get; set; }
        public string nombreUsuario { get; set; } = string.Empty;// con esto se inicializa "" para evitar errores al comprender un posible valor null

        public string mail { get; set; } = string.Empty;
        public string contraseña { get; set; } = string.Empty;

        public int IdRolUsuario { get; set; }

        public Boolean activo { get; set; } 


    }
}
