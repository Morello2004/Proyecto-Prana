using Prana.Entidades;
using Prana.negocio;

namespace Prana.Presentacion;
// agrupa los datos y operaciones de frm resumen mensual caja
public partial class frmResumenMensualCaja : Form
{
    // conserva negocio entre las operaciones de esta clase
    private readonly CN_Caja negocio = new();
    // prepara frm resumen mensual caja antes de que se utilice la instancia
    public frmResumenMensualCaja() : this(DateTime.Today)
    {
    }

    // prepara frm resumen mensual caja antes de que se utilice la instancia
    public frmResumenMensualCaja(DateTime fecha)
    {
        InitializeComponent();
        mes.Value = new DateTime(fecha.Year, fecha.Month, 1);
    }

    // prepara la informacion inicial cuando se carga la ventana
    private void frmResumenMensualCaja_Load(object? sender, EventArgs e) => Cargar();
    // ejecuta la accion de consultar cuando el usuario pulsa el control
    private void consultar_Click(object? sender, EventArgs e) => Cargar();
    // obtiene y presenta para actualizar los controles de la pantalla
    private void Cargar()
    {
        try
        {
            Mostrar(negocio.ConsultarMes(mes.Value));
        }
        // atiende el error de esta operacion antes de informar o propagar el problema
        catch (Exception error)
        {
            MessageBox.Show(this, error.Message, "Resumen del mes", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    // resuelve mostrar con los datos recibidos y entrega el resultado al codigo que lo llama
    private void Mostrar(ResumenMensualCaja resumen)
    {
        totales.DataSource = resumen.Totales;
        porDia.DataSource = resumen.PorDia;
        porMedio.DataSource = resumen.PorMedio;
        porTipo.DataSource = resumen.PorTipo;
        detalle.DataSource = resumen.Movimientos;
        estado.Text = $"{resumen.Mes:MMMM yyyy}: {resumen.Movimientos.Count} movimientos. Resultado = ingresos menos gastos. " + "Se incluyen cobros por fecha de pago y movimientos vigentes, también de días sin cerrar. ARS y USD se muestran separados. " + "Esta consulta no cierra la caja.";
    }

    // ajusta la presentacion de una celda sin convertir ese formato en una escritura a la base
    private void detalle_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        if (detalle.Columns[e.ColumnIndex].DataPropertyName == "Venta" && e.Value is int numero)
        {
            e.Value = $"RTO {numero}";
            e.FormattingApplied = true;
        }
    }
}
