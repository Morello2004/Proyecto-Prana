using System;

namespace Prana.Entidades
{
    // agrupa los datos y operaciones de informe venta
    public class InformeVenta
    {
        // identificador de la venta relaciona sus productos, cobros, entrega y comprobantes
        public int IdVenta
        {
            get; set;
        }
        // conserva fecha como date time para intercambiar este dato entre las capas
        public DateTime Fecha
        {
            get; set;
        }
        // conserva cliente como string para intercambiar este dato entre las capas
        public string Cliente { get; set; } = string.Empty;
        // conserva ubicacion como string para intercambiar este dato entre las capas
        public string Ubicacion { get; set; } = string.Empty;
        // conserva productos como string para intercambiar este dato entre las capas
        public string Productos { get; set; } = string.Empty;
        // conserva categorias como string para intercambiar este dato entre las capas
        public string Categorias { get; set; } = string.Empty;
        // conserva marcas como string para intercambiar este dato entre las capas
        public string Marcas { get; set; } = string.Empty;
        // conserva total como decimal para intercambiar este dato entre las capas
        public decimal Total
        {
            get; set;
        }
        // total abonado que utiliza esta consulta o comprobante; revisar si incluye recargos en su origen
        public decimal Pagado
        {
            get; set;
        }
        // conserva saldo como decimal para intercambiar este dato entre las capas
        public decimal Saldo
        {
            get; set;
        }
        // conserva envio como string para intercambiar este dato entre las capas
        public string Envio { get; set; } = string.Empty;
    }
}
