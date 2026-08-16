using System;
using System.Collections.Generic;
using System.Text;

namespace Prana.Entidades
{
    public class producto
    {

        public int IdProducto { get; set; }
        public string NombreProducto { get; set; } = string.Empty;
        public string Marca { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string Modelo { get; set; } = string.Empty;

        public List<VarianteProducto> Variantes { get; set; } = new();

    }
}

