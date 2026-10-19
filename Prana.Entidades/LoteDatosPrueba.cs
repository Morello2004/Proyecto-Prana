namespace Prana.Entidades
{
    // Resume un lote de datos de prueba y el periodo asignado a sus operaciones.
    public class LoteDatosPrueba
    {
        // Identifica el lote de forma unica.
        public Guid Id { get; set; }

        // Identifica visualmente los registros generados.
        public string Token { get; set; } = string.Empty;

        // Conserva la fecha de creacion del lote.
        public DateTime Creado { get; set; }

        // Conserva la fecha inicial de los cierres generados.
        public DateTime? FechaDesde { get; set; }

        // Conserva la fecha final de los cierres generados.
        public DateTime? FechaHasta { get; set; }

        // Indica cuantos clientes se generan en cada lote.
        public int Clientes { get; set; } = 100;

        // Indica cuantos productos se generan en cada lote.
        public int Productos { get; set; } = 100;

        // Indica cuantas ventas se generan en cada lote.
        public int Ventas { get; set; } = 100;

        // Indica cuantos pagos se generan en cada lote.
        public int Pagos { get; set; } = 100;

        // Indica cuantas entregas se generan en cada lote.
        public int Entregas { get; set; } = 100;

        // Indica cuantos cierres se generan en cada lote.
        public int Cierres { get; set; } = 100;
    }
}
