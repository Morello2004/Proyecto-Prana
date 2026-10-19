namespace Prana.Presentacion;

partial class frmMotivoAnulacion
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
        lblMotivo = new Label();
        txtMotivo = new TextBox();
        btnAceptar = new Button();
        btnCancelar = new Button();
        SuspendLayout();
        lblMotivo.Name = "lblMotivo";
        lblMotivo.Location = new Point(16, 12);
        lblMotivo.Size = new Size(405, 23);
        lblMotivo.Text = "Motivo de anulación";
        lblMotivo.ForeColor = Color.White;
        Controls.Add(lblMotivo);
        txtMotivo.Name = "txtMotivo";
        txtMotivo.Location = new Point(16, 38);
        txtMotivo.Size = new Size(405, 23);
        txtMotivo.MaxLength = 200;
        Controls.Add(txtMotivo);
        btnAceptar.Name = "btnAceptar";
        btnAceptar.Location = new Point(310, 85);
        btnAceptar.Size = new Size(110, 36);
        btnAceptar.Text = "Anular";
        btnAceptar.BackColor = Color.SteelBlue;
        btnAceptar.ForeColor = Color.White;
        btnAceptar.FlatStyle = FlatStyle.Flat;
        btnAceptar.Click += btnAceptar_Click;
        Controls.Add(btnAceptar);
        btnCancelar.Name = "btnCancelar";
        btnCancelar.Location = new Point(184, 85);
        btnCancelar.Size = new Size(110, 36);
        btnCancelar.Text = "Cancelar";
        btnCancelar.BackColor = Color.SteelBlue;
        btnCancelar.ForeColor = Color.White;
        btnCancelar.FlatStyle = FlatStyle.Flat;
        btnCancelar.Click += btnCancelar_Click;
        Controls.Add(btnCancelar);
        AutoScaleMode = AutoScaleMode.Font;
        AutoScroll = true;
        BackColor = Color.FromArgb(13, 19, 27);
        ForeColor = Color.White;
        ClientSize = new Size(440, 140);
        Text = "Motivo de anulación";
        Name = "frmMotivoAnulacion";
        StartPosition = FormStartPosition.CenterParent;
        AcceptButton = btnAceptar;
        CancelButton = btnCancelar;
        ResumeLayout(false);
        PerformLayout();
    }

    private Label lblMotivo;
    private TextBox txtMotivo;
    private Button btnAceptar;
    private Button btnCancelar;
}
