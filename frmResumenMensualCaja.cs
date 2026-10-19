using Prana.Entidades;
using Prana.negocio;

namespace Prana.Presentacion;

public partial class frmResumenMensualCaja : Form
{
    private readonly CN_Caja negocio = new();
    public frmResumenMensualCaja() : this(DateTime.Today) { }
    public frmResumenMensualCaja(DateTime fecha)
    {
        InitializeComponent();
        mes.Value = new DateTime(fecha.Year, fecha.Month, 1);
        foreach (var grilla in new[] { totales, porDia, porMedio, porTipo })
        {
            EstiloGestion.Grilla(grilla);
            EstiloGestion.Columna(grilla, "Concepto", "Concepto", 160);
            EstiloGestion.Columna(grilla, "Moneda", "Moneda", 60);
            EstiloGestion.Columna(grilla, "Cantidad", "Movimientos", 80);
            EstiloGestion.Columna(grilla, "Ingresos", "Ingresos", 110, "N2");
            EstiloGestion.Columna(grilla, "Gastos", "Gastos", 110, "N2");
            EstiloGestion.Columna(grilla, "Resultado", "Resultado", 110, "N2");
        }
        EstiloGestion.Grilla(detalle);
        foreach (var (prop, texto, ancho) in new[] { ("Fecha", "Fecha y hora", 120), ("Venta", "Remito", 70),
            ("Tipo", "Tipo", 90), ("Concepto", "Cliente / motivo", 180), ("Medio", "Medio", 90),
            ("Moneda", "Moneda", 60), ("Importe", "Importe", 100), ("Cuenta", "Cuenta / referencia", 120), ("Usuario", "Usuario (ID)", 70) })
            EstiloGestion.Columna(detalle, prop, texto, ancho, prop == "Importe" ? "N2" : prop == "Fecha" ? "dd/MM/yyyy HH:mm" : "");
    }

    private void frmResumenMensualCaja_Load(object? sender, EventArgs e) => Cargar();
    private void consultar_Click(object? sender, EventArgs e) => Cargar();
    private void Cargar()
    {
        try { Mostrar(negocio.ConsultarMes(mes.Value)); }
        catch (Exception ex) { MessageBox.Show(this, ex.Message, "Resumen del mes", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
    }
    private void Mostrar(ResumenMensualCaja resumen)
    {
        totales.DataSource = resumen.Totales;
        porDia.DataSource = resumen.PorDia;
        porMedio.DataSource = resumen.PorMedio;
        porTipo.DataSource = resumen.PorTipo;
        detalle.DataSource = resumen.Movimientos;
        estado.Text = $"{resumen.Mes:MMMM yyyy}: {resumen.Movimientos.Count} movimientos. Resultado = ingresos menos gastos. " +
            "Se incluyen cobros por fecha de pago y movimientos vigentes, también de días sin cerrar. ARS y USD se muestran separados. " +
            "Los saldos iniciales no son ingresos. Esta consulta no cierra la caja.";
    }
    private void detalle_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        if (detalle.Columns[e.ColumnIndex].DataPropertyName == "Venta" && e.Value is int numero)
        { e.Value = $"RTO {numero}"; e.FormattingApplied = true; }
    }
}
