namespace Prana.Entidades
{
    // datos de consulta del panel; no modifica ventas ni pagos representa resumen ventas agrupa los datos que intercambian las consultas, las reglas de negocio y las pantallas
    public class ResumenVentas
    {
        // conserva total hoy como decimal para intercambiar este dato entre las capas
        public decimal TotalHoy
        {
            get; set;
        }
        // conserva cantidad hoy como int para intercambiar este dato entre las capas
        public int CantidadHoy
        {
            get; set;
        }
        // deuda que queda por cancelar segun los importes de ventas y pagos consultados
        public decimal SaldoPendiente
        {
            get; set;
        }
        // conserva ventas pendientes como int para intercambiar este dato entre las capas
        public int VentasPendientes
        {
            get; set;
        }
    }
}
