namespace Prana.Entidades
{
    // agrupa los datos y operaciones de variante producto
    public class VarianteProducto
    {
        // identificador de la presentacion concreta del producto; distingue color y condicion
        public int IdVariante
        {
            get; set;
        }
        // color de la variante elegida
        public string Color { get; set; } = string.Empty;
        // condicion comercial de la variante, por ejemplo primera u outlet
        public string CondicionProducto { get; set; } = string.Empty;
        // conserva precio como decimal para intercambiar este dato entre las capas
        public decimal Precio
        {
            get; set;
        }
        // conserva costo como decimal para intercambiar este dato entre las capas
        public decimal Costo
        {
            get; set;
        }
        // unidades fisicas registradas para esta variante
        public int StockFisico
        {
            get; set;
        }
        // unidades vendidas que todavia esperan preparacion, segun la consulta de existencias
        public int StockComprometido
        {
            get; set;
        }
    }
}
