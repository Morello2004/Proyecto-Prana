namespace Prana.Presentacion;

partial class frmAlertas
{
    private System.ComponentModel.IContainer? components = null;
    protected override void Dispose(bool disposing)
    {
        if (disposing) components?.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
        titulo = new Label();
        estado = new Label();
        grilla = new DataGridView();
        lblStock = new Label();
        grillaStock = new DataGridView();
        ((System.ComponentModel.ISupportInitialize)grilla).BeginInit();
        ((System.ComponentModel.ISupportInitialize)grillaStock).BeginInit();
        SuspendLayout();
        // 
        // titulo
        // 
        titulo.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
        titulo.ForeColor = Color.White;
        titulo.Location = new Point(16, 16);
        titulo.Name = "titulo";
        titulo.Size = new Size(700, 42);
        titulo.TabIndex = 0;
        titulo.Text = "Alertas de pagos pendientes";
        // 
        // estado
        // 
        estado.ForeColor = Color.White;
        estado.Location = new Point(16, 66);
        estado.Name = "estado";
        estado.Size = new Size(900, 30);
        estado.TabIndex = 1;
        estado.Text = "Ventas con saldo pendiente luego de un mes desde el primer pago";
        // 
        // grilla
        // 
        grilla.AllowUserToAddRows = false;
        grilla.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        grilla.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        grilla.BackgroundColor = Color.FromArgb(13, 19, 27);
        dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle1.BackColor = Color.White;
        dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F);
        dataGridViewCellStyle1.ForeColor = Color.White;
        dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
        dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
        dataGridViewCellStyle1.WrapMode = DataGridViewTriState.False;
        grilla.DefaultCellStyle = dataGridViewCellStyle1;
        grilla.Location = new Point(16, 106);
        grilla.Name = "grilla";
        grilla.ReadOnly = true;
        grilla.RowHeadersVisible = false;
        grilla.Size = new Size(968, 324);
        grilla.TabIndex = 2;
        grilla.CellContentClick += grilla_CellContentClick;
        // 
        // lblStock
        // 
        lblStock.AutoSize = true;
        lblStock.Location = new Point(12, 443);
        lblStock.Name = "lblStock";
        lblStock.Size = new Size(218, 15);
        lblStock.TabIndex = 3;
        lblStock.Text = "Productos pendientes con falta de stock";
        // 
        // grillaStock
        // 
        grillaStock.AllowUserToAddRows = false;
        grillaStock.AllowUserToDeleteRows = false;
        grillaStock.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        grillaStock.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        grillaStock.Location = new Point(12, 476);
        grillaStock.Name = "grillaStock";
        grillaStock.ReadOnly = true;
        grillaStock.Size = new Size(976, 187);
        grillaStock.TabIndex = 4;
        // 
        // frmAlertas
        // 
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
        ((System.ComponentModel.ISupportInitialize)grilla).EndInit();
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
