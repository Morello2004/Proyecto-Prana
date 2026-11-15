using System;
using System.Collections.Generic;
using System.Text;

namespace Prana.Entidades
{
    // agrupa los datos y operaciones de producto
    public class producto
    {
        // conserva id producto como int para intercambiar este dato entre las capas
        public int IdProducto
        {
            get; set;
        }
        // denominacion comercial el modelo, el color y la condicion se conservan por separado
        public string NombreProducto { get; set; } = string.Empty;
        // conserva marca como string para intercambiar este dato entre las capas
        public string Marca { get; set; } = string.Empty;
        // conserva categoria como string para intercambiar este dato entre las capas
        public string Categoria { get; set; } = string.Empty;
        // conserva descripcion como string para intercambiar este dato entre las capas
        public string Descripcion { get; set; } = string.Empty;
        // codigo o referencia del fabricante; no representa una cantidad ni un precio
        public string Modelo { get; set; } = string.Empty;
        // conserva variantes como list<variante producto> para intercambiar este dato entre las capas
        public List<VarianteProducto> Variantes { get; set; } = new();
    }
}
