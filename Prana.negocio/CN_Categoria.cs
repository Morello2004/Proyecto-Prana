using Prana.Datos;
using Prana.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace Prana.negocio
{
    // agrupa los datos y operaciones de cn categoria
    public class CN_Categoria
    {
        // conserva cd categoria entre las operaciones de esta clase
        private readonly CD_Categoria cdCategoria = new CD_Categoria();
        // solicita todas las categorias a la capa de datos
        public List<Categoria> obtenerCategorias()
        {
            return cdCategoria.obtenerCategorias();
        }
    }
}
