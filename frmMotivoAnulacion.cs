namespace Prana.Presentacion;
// agrupa los datos y operaciones de frm motivo anulacion
public partial class frmMotivoAnulacion : Form
{
    // dato motivo de tipo string se obtiene al consultar la propiedad mediante la expresion siguiente
    public string Motivo => txtMotivo.Text;

    // prepara frm motivo anulacion antes de que se utilice la instancia
    public frmMotivoAnulacion()
    {
        InitializeComponent();
    }

    // ejecuta la accion de btn aceptar cuando el usuario pulsa el control
    private void btnAceptar_Click(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.OK;
    }

    // ejecuta la accion de btn cancelar cuando el usuario pulsa el control
    private void btnCancelar_Click(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
    }
}
