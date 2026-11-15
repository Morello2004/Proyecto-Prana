using System;

namespace Prana.Entidades
{
    // agrupa los datos y operaciones de alerta pago pendiente
    public class AlertaPagoPendiente
    {
        // identificador de la venta relaciona sus productos, cobros, entrega y comprobantes
        public int IdVenta
        {
            get; set;
        }
        // conserva cliente como string para intercambiar este dato entre las capas
        public string Cliente { get; set; } = string.Empty;
        // conserva productos como string para intercambiar este dato entre las capas
        public string Productos { get; set; } = string.Empty;
        // conserva primer pago como date time para intercambiar este dato entre las capas
        public DateTime PrimerPago
        {
            get; set;
        }
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
        // conserva dias vencida como int para intercambiar este dato entre las capas
        public int DiasVencida
        {
            get; set;
        }
    }
}
