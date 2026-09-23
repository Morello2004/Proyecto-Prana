namespace Prana.Entidades
{
    // Representa un medio de pago y sus recargos por cuotas.
    public class MedioPago
    {
        // Identifica el medio de pago.
        public int IdMedioPago { get; set; }

        // Contiene el nombre del medio de pago.
        public string NombreMedioPago { get; set; } = string.Empty;

        // Conserva el recargo general del medio de pago.
        public decimal RecargoMedioPago { get; set; }

        // Conserva el recargo para tres cuotas.
        public decimal Recargo3 { get; set; }

        // Conserva el recargo para seis cuotas.
        public decimal Recargo6 { get; set; }

        // Conserva el recargo para nueve cuotas.
        public decimal Recargo9 { get; set; }

        // Conserva el recargo para doce cuotas.
        public decimal Recargo12 { get; set; }

        // Indica si el medio de pago está disponible.
        public bool Activo { get; set; }
    }
}
