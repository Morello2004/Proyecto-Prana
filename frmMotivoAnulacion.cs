namespace Prana.Presentacion;

public partial class frmMotivoAnulacion : Form
{
    public string Motivo => txtMotivo.Text;
    public frmMotivoAnulacion() { InitializeComponent(); }
    private void btnAceptar_Click(object? sender, EventArgs e) { DialogResult = DialogResult.OK; }
    private void btnCancelar_Click(object? sender, EventArgs e) { DialogResult = DialogResult.Cancel; }
}
