namespace Prana.Presentacion;

public partial class frmComprobantes : Form
{
    private string carpeta = "";
    private int numero;
    public frmComprobantes() { InitializeComponent(); }
    public frmComprobantes(int numero, string carpeta) : this()
    {
        this.numero = numero;
        this.carpeta = carpeta;
        Text = $"Venta N.º {numero} - comprobantes";
        lblInformacion.Text = $"Garantía y remito N.º {numero} generados.\nAbra cada documento para imprimirlo o guardarlo como PDF.";
    }
    private void btnGarantia_Click(object? sender, EventArgs e) => Abrir("Garantia");
    private void btnRemito_Click(object? sender, EventArgs e) => Abrir("Remito");
    private void Abrir(string tipo)
    {
        try { DocumentosPrana.Abrir(Path.Combine(carpeta, $"{tipo}-{numero}.html")); }
        catch (Exception ex) { MessageBox.Show(this, ex.Message, "Abrir comprobante"); }
    }
}
