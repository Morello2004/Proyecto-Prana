using System;
using System.Collections.Generic;
using System.Text;

namespace Prana.Entidades
{
    internal class Rol
    {
        public int idRolUsuario { get; set; }
        public string nombreRolUsuario { get; set; }= string.Empty;// con esto se inicializa " " para evitar errores al comprender un posible valor null


    }
}
