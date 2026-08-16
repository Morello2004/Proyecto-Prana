namespace Prana.Entidades
{
    public class VarianteProducto
    {
        public int IdVariante { get; set; }

        public string Color { get; set; } = string.Empty;

        public string CondicionProducto { get; set; } = string.Empty;

        public decimal Precio { get; set; }

        public decimal Costo { get; set; }

        public int StockFisico { get; set; }

        public int StockComprometido { get; set; }

        
    }
}