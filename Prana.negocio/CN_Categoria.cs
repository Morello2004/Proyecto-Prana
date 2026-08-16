using Prana.Datos;
using Prana.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace Prana.negocio
{
    public class CN_Categoria
    {
        private readonly CD_Categoria cdCategoria = new CD_Categoria();

        // Solicita todas las categorías a la capa de Datos
        public List<Categoria> obtenerCategorias()
        {
            return cdCategoria.obtenerCategorias();
        }
    }
}
