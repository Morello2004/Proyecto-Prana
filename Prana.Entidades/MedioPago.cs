namespace Prana.Entidades
{
    // agrupa los datos y operaciones de medio pago
    public class MedioPago
    {
        // conserva la clasificacion fija del medio para los cierres de caja clasificacion del medio para agrupar los movimientos de caja
        public string TipoCaja { get; set; } = "Otros";
        // identifica el medio de pago
        public int IdMedioPago
        {
            get; set;
        }
        // contiene el nombre del medio de pago
        public string NombreMedioPago { get; set; } = string.Empty;
        // conserva el recargo general del medio de pago
        public decimal RecargoMedioPago
        {
            get; set;
        }
        // conserva el recargo para dos cuotas
        public decimal Recargo2
        {
            get; set;
        }
        // conserva el recargo para tres cuotas
        public decimal Recargo3
        {
            get; set;
        }
        // conserva el recargo para seis cuotas
        public decimal Recargo6
        {
            get; set;
        }
        // conserva el recargo para nueve cuotas
        public decimal Recargo9
        {
            get; set;
        }
        // conserva el recargo para doce cuotas
        public decimal Recargo12
        {
            get; set;
        }
        // indica si el medio de pago esta disponible marca si el registro esta habilitado para el uso habitual; no elimina su historial
        public bool Activo
        {
            get; set;
        }
    }
}
