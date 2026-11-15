namespace Prana.Presentacion;
// agrupa los datos y operaciones de frm comprobantes
public partial class frmComprobantes : Form
{
    // conserva carpeta entre las operaciones de esta clase
    private string carpeta = "";
    // conserva numero entre las operaciones de esta clase
    private int numero;
    // prepara frm comprobantes antes de que se utilice la instancia
    public frmComprobantes()
    {
        InitializeComponent();
    }

    // prepara frm comprobantes antes de que se utilice la instancia
    public frmComprobantes(int numero, string carpeta) : this()
    {
        this.numero = numero;
        this.carpeta = carpeta;
        Text = $"Venta N.º {numero} - comprobantes";
        lblInformacion.Text = $"Garantía y remito N.º {numero} generados.\nAbra cada documento para imprimirlo o guardarlo como PDF.";
    }

    // ejecuta la accion de btn garantia cuando el usuario pulsa el control
    private void btnGarantia_Click(object? sender, EventArgs e) => Abrir("Garantia");
    // ejecuta la accion de btn remito cuando el usuario pulsa el control
    private void btnRemito_Click(object? sender, EventArgs e) => Abrir("Remito");
    // resuelve abrir con los datos recibidos y entrega el resultado al codigo que lo llama
    private void Abrir(string tipo)
    {
        try
        {
            DocumentosPrana.Abrir(Path.Combine(carpeta, $"{tipo}-{numero}.html"));
        }
        // atiende el error de esta operacion antes de informar o propagar el problema
        catch (Exception error)
        {
            MessageBox.Show(this, error.Message, "Abrir comprobante");
        }
    }
}
