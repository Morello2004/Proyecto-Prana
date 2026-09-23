namespace Prana.Presentacion
{
    partial class frmDeudaCliente
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing)
        {
            if (disposing) components?.Dispose();
            base.Dispose(disposing);
        }
        private void InitializeComponent()
        {
            lblCliente = new Label();
            lblEstado = new Label();
            dgvDeudas = new DataGridView();
            btnCerrar = new Button();
            SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvDeudas).BeginInit();
            lblCliente.Dock = DockStyle.Top;
            lblCliente.Height = 65;
            lblCliente.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblCliente.Padding = new Padding(10, 14, 0, 0);
            lblEstado.Dock = DockStyle.Top;
            lblEstado.Height = 40;
            lblEstado.Padding = new Padding(10, 0, 0, 0);
            lblEstado.Text = "Consultando ventas pendientes…";
            lblEstado.ForeColor = Color.FromArgb(145, 170, 210);
            dgvDeudas.Dock = DockStyle.Fill;
            dgvDeudas.AutoGenerateColumns = false;
            dgvDeudas.ReadOnly = true;
            dgvDeudas.AllowUserToAddRows = false;
            dgvDeudas.AllowUserToDeleteRows = false;
            dgvDeudas.RowHeadersVisible = false;
            dgvDeudas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvDeudas.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvDeudas.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgvDeudas.DefaultCellStyle.ForeColor = Color.Black;
            dgvDeudas.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvDeudas.BackgroundColor = Color.FromArgb(13, 19, 27);
            dgvDeudas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDeudas.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "N.º venta", DataPropertyName = "IdVenta", FillWeight = 55 });
            dgvDeudas.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Fecha", DataPropertyName = "Fecha", DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy" } });
            dgvDeudas.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Productos de la venta", DataPropertyName = "Productos", FillWeight = 280 });
            dgvDeudas.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Total", DataPropertyName = "Total", DefaultCellStyle = new DataGridViewCellStyle { Format = "C2", FormatProvider = System.Globalization.CultureInfo.GetCultureInfo("es-AR") } });
            dgvDeudas.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Pagado", DataPropertyName = "Pagado", DefaultCellStyle = new DataGridViewCellStyle { Format = "C2", FormatProvider = System.Globalization.CultureInfo.GetCultureInfo("es-AR") } });
            dgvDeudas.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Saldo pendiente", DataPropertyName = "SaldoPendiente", DefaultCellStyle = new DataGridViewCellStyle { Format = "C2", FormatProvider = System.Globalization.CultureInfo.GetCultureInfo("es-AR"), ForeColor = Color.FromArgb(165, 72, 0) } });
            dgvDeudas.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Estado", DataPropertyName = "Estado" });
            btnCerrar.Text = "Cerrar";
            btnCerrar.Dock = DockStyle.Bottom;
            btnCerrar.Height = 36;
            btnCerrar.BackColor = Color.SteelBlue;
            btnCerrar.FlatStyle = FlatStyle.Flat;
            btnCerrar.DialogResult = DialogResult.Cancel;
            CancelButton = btnCerrar;
            Controls.Add(dgvDeudas);
            Controls.Add(lblEstado);
            Controls.Add(lblCliente);
            Controls.Add(btnCerrar);
            BackColor = Color.FromArgb(21, 30, 41);
            ForeColor = Color.White;
            ClientSize = new Size(1100, 480);
            MinimumSize = new Size(800, 380);
            StartPosition = FormStartPosition.CenterParent;
            Text = "Ventas pendientes del cliente";
            Load += frmDeudaCliente_Load;
            ((System.ComponentModel.ISupportInitialize)dgvDeudas).EndInit();
            ResumeLayout(false);
        }
        private Label lblCliente, lblEstado;
        private DataGridView dgvDeudas;
        private Button btnCerrar;
    }
}
