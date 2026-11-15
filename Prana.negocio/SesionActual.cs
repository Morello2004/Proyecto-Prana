using System;
using System.Collections.Generic;
using System.Text;
using Prana.Entidades;

namespace Prana.negocio
{
    // agrupa los datos y operaciones de sesion actual
    public static class SesionActual
    {
        // conserva usuario como usuario? para intercambiar este dato entre las capas
        public static usuario? Usuario
        {
            get; private set;
        }

        // resuelve iniciar sesion con los datos recibidos y entrega el resultado al codigo que lo llama
        public static void IniciarSesion(usuario usuarioAutenticado)
        {
            Usuario = usuarioAutenticado;
        }

        // resuelve cerrar sesion con los datos recibidos y entrega el resultado al codigo que lo llama
        public static void CerrarSesion()
        {
            Usuario = null;
        }
    }
}
