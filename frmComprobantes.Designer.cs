namespace Prana.Presentacion;

partial class frmComprobantes
{
    private System.ComponentModel.IContainer? components = null;
    protected override void Dispose(bool disposing)
    {
        if (disposing) components?.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        lblInformacion = new Label();
        btnGarantia = new Button();
        btnRemito = new Button();
        SuspendLayout();
        lblInformacion.Name = "lblInformacion";
        lblInformacion.Location = new Point(20, 20);
        lblInformacion.Size = new Size(480, 55);
        lblInformacion.Text = "Abra cada documento para imprimirlo o guardarlo como PDF.";
        lblInformacion.ForeColor = Color.White;
        Controls.Add(lblInformacion);
        btnGarantia.Name = "btnGarantia";
        btnGarantia.Location = new Point(20, 90);
        btnGarantia.Size = new Size(225, 36);
        btnGarantia.Text = "Ver / imprimir garantía";
        btnGarantia.BackColor = Color.SteelBlue;
        btnGarantia.ForeColor = Color.White;
        btnGarantia.FlatStyle = FlatStyle.Flat;
        btnGarantia.Click += btnGarantia_Click;
        Controls.Add(btnGarantia);
        btnRemito.Name = "btnRemito";
        btnRemito.Location = new Point(265, 90);
        btnRemito.Size = new Size(225, 36);
        btnRemito.Text = "Ver / imprimir remito";
        btnRemito.BackColor = Color.SteelBlue;
        btnRemito.ForeColor = Color.White;
        btnRemito.FlatStyle = FlatStyle.Flat;
        btnRemito.Click += btnRemito_Click;
        Controls.Add(btnRemito);
        AutoScaleMode = AutoScaleMode.Font;
        AutoScroll = true;
        BackColor = Color.FromArgb(13, 19, 27);
        ForeColor = Color.White;
        ClientSize = new Size(520, 180);
        Text = "Comprobantes";
        Name = "frmComprobantes";
        StartPosition = FormStartPosition.CenterParent;
        MaximizeBox = false;
        MinimizeBox = false;
        ResumeLayout(false);
        PerformLayout();
    }

    private Label lblInformacion;
    private Button btnGarantia;
    private Button btnRemito;
}
