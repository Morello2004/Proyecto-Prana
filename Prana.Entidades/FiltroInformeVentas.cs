using System;

namespace Prana.Entidades
{
    // agrupa los datos y operaciones de filtro informe ventas
    public class FiltroInformeVentas
    {
        // conserva desde como date time para intercambiar este dato entre las capas
        public DateTime Desde
        {
            get; set;
        }
        // conserva hasta como date time para intercambiar este dato entre las capas
        public DateTime Hasta
        {
            get; set;
        }
        // conserva producto como string para intercambiar este dato entre las capas
        public string Producto { get; set; } = string.Empty;
        // conserva categoria como string para intercambiar este dato entre las capas
        public string Categoria { get; set; } = string.Empty;
        // conserva marca como string para intercambiar este dato entre las capas
        public string Marca { get; set; } = string.Empty;
        // conserva cliente como string para intercambiar este dato entre las capas
        public string Cliente { get; set; } = string.Empty;
        // conserva envio como string para intercambiar este dato entre las capas
        public string Envio { get; set; } = string.Empty;
        // conserva ubicacion como string para intercambiar este dato entre las capas
        public string Ubicacion { get; set; } = string.Empty;
        // conserva monto minimo como decimal? para intercambiar este dato entre las capas
        public decimal? MontoMinimo
        {
            get; set;
        }
        // conserva monto maximo como decimal? para intercambiar este dato entre las capas
        public decimal? MontoMaximo
        {
            get; set;
        }
    }
}
