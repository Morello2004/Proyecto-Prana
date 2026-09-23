using System;
using System.Collections.Generic;
using System.Text;
using Prana.Entidades;

namespace Prana.negocio
{
    public static class SesionActual
    {
        public static usuario? Usuario { get; private set; }

        public static void IniciarSesion(usuario usuarioAutenticado)
        {
            Usuario = usuarioAutenticado;
        }

        public static void CerrarSesion()
        {
            Usuario = null;
        }
    }
}
