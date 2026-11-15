namespace Prana.Presentacion;
// parte visual de frmalertas crea controles y define su distribucion, colores y eventos; visual studio puede regenerar este archivo al editar el disenador
partial class frmAlertas
{
    private System.ComponentModel.IContainer? components = null;
    // libera los recursos del formulario; disposing indica si tambien deben liberarse los componentes administrados
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            components?.Dispose();
        }

        base.Dispose(disposing);
    }

    // construye la pantalla: instancia controles, asigna propiedades, agrega cada control a su contenedor y conecta los eventos no registra operaciones comerciales
    private void InitializeComponent()
    {
        DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
        titulo = new Label();
        estado = new Label();
        grilla = new DataGridView();
        lblStock = new Label();
        grillaStock = new DataGridView();
        // llama a ((systemcomponentmodelisupportinitialize)grilla)begininit para realizar este paso del flujo
        ((System.ComponentModel.ISupportInitialize)grilla).BeginInit();
        // llama a ((systemcomponentmodelisupportinitialize)grillastock)begininit para realizar este paso del flujo
        ((System.ComponentModel.ISupportInitialize)grillaStock).BeginInit();
        SuspendLayout();
        // titulo titulofont: tipografia, tamano y estilo con los que se muestra el contenido
        titulo.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
        titulo.ForeColor = Color.White;
        titulo.Location = new Point(16, 16);
        titulo.Name = "titulo";
        titulo.Size = new Size(700, 42);
        titulo.TabIndex = 0;
        titulo.Text = "Alertas de pagos pendientes";
        // estado estadoforecolor: color del texto cuando la celda o el control no esta seleccionado
        estado.ForeColor = Color.White;
        estado.Location = new Point(16, 66);
        estado.Name = "estado";
        estado.Size = new Size(900, 30);
        estado.TabIndex = 1;
        estado.Text = "Ventas con saldo pendiente luego de un mes desde el primer pago";
        // grilla
        grilla.AllowUserToAddRows = false;
        grilla.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        grilla.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        grilla.BackgroundColor = Color.FromArgb(13, 19, 27);
        dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle1.BackColor = Color.White;
        dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F);
        dataGridViewCellStyle1.ForeColor = Color.White;
        dataGridViewCellStyle1.SelectionBackColor = Color.FromArgb(190, 220, 250);
        dataGridViewCellStyle1.SelectionForeColor = Color.Black;
        dataGridViewCellStyle1.SelectionBackColor = Color.FromArgb(190, 220, 250);
        dataGridViewCellStyle1.WrapMode = DataGridViewTriState.False;
        grilla.DefaultCellStyle = dataGridViewCellStyle1;
        grilla.Location = new Point(16, 106);
        grilla.Name = "grilla";
        grilla.ForeColor = Color.Black;
        grilla.DefaultCellStyle.ForeColor = Color.Black;
        grilla.RowsDefaultCellStyle.ForeColor = Color.Black;
        grilla.AlternatingRowsDefaultCellStyle.ForeColor = Color.Black;
        grilla.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.Black;
        grilla.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(190, 220, 250);
        grilla.DefaultCellStyle.SelectionForeColor = Color.Black;
        grilla.DefaultCellStyle.SelectionBackColor = Color.FromArgb(190, 220, 250);
        grilla.AlternatingRowsDefaultCellStyle.SelectionForeColor = Color.Black;
        grilla.AlternatingRowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(190, 220, 250);
        grilla.ReadOnly = true;
        grilla.RowHeadersVisible = false;
        grilla.Size = new Size(968, 324);
        grilla.TabIndex = 2;
        grilla.CellContentClick += grilla_CellContentClick;
        // lblstock
        lblStock.AutoSize = true;
        lblStock.Location = new Point(12, 443);
        lblStock.Name = "lblStock";
        lblStock.Size = new Size(218, 15);
        lblStock.TabIndex = 3;
        lblStock.Text = "Productos pendientes con falta de stock";
        // grillastock
        grillaStock.AllowUserToAddRows = false;
        grillaStock.AllowUserToDeleteRows = false;
        grillaStock.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        grillaStock.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        grillaStock.Location = new Point(12, 476);
        grillaStock.Name = "grillaStock";
        grillaStock.ForeColor = Color.Black;
        grillaStock.DefaultCellStyle.ForeColor = Color.Black;
        grillaStock.RowsDefaultCellStyle.ForeColor = Color.Black;
        grillaStock.AlternatingRowsDefaultCellStyle.ForeColor = Color.Black;
        grillaStock.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.Black;
        grillaStock.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(190, 220, 250);
        grillaStock.DefaultCellStyle.SelectionForeColor = Color.Black;
        grillaStock.DefaultCellStyle.SelectionBackColor = Color.FromArgb(190, 220, 250);
        grillaStock.AlternatingRowsDefaultCellStyle.SelectionForeColor = Color.Black;
        grillaStock.AlternatingRowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(190, 220, 250);
        grillaStock.ReadOnly = true;
        grillaStock.Size = new Size(976, 187);
        grillaStock.TabIndex = 4;
        // frmalertas
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        AutoScroll = true;
        BackColor = Color.FromArgb(13, 19, 27);
        ClientSize = new Size(1000, 700);
        Controls.Add(grillaStock);
        Controls.Add(lblStock);
        Controls.Add(titulo);
        Controls.Add(estado);
        Controls.Add(grilla);
        ForeColor = Color.White;
        Name = "frmAlertas";
        Text = "Alertas";
        Load += frmAlertas_Load;
        // llama a ((systemcomponentmodelisupportinitialize)grilla)endinit para realizar este paso del flujo
        ((System.ComponentModel.ISupportInitialize)grilla).EndInit();
        // llama a ((systemcomponentmodelisupportinitialize)grillastock)endinit para realizar este paso del flujo
        ((System.ComponentModel.ISupportInitialize)grillaStock).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    private Label titulo;
    private Label estado;
    private DataGridView grilla;
    private Label lblStock;
    private DataGridView grillaStock;
}
