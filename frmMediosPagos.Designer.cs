namespace Prana.Presentacion
{
    partial class frmMediosPagos
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            lblTipoCaja = new Label();

            cboTipoCaja = new ComboBox();

            pnlListado = new Panel();
            pnlEncabezado = new Panel();
            lblTitulo = new Label();
            lblDescripcion = new Label();
            pnlFormulario = new Panel();
            lblSeccion = new Label();
            lblNombre = new Label();
            txtNombreMedioPago = new TextBox();
            lblGeneral = new Label();
            nudRecargoGeneral = new NumericUpDown();
            lblRecargo2 = new Label();
            nudRecargo2 = new NumericUpDown();
            lblRecargo3 = new Label();
            nudRecargo3 = new NumericUpDown();
            lblRecargo6 = new Label();
            nudRecargo6 = new NumericUpDown();
            lblRecargo9 = new Label();
            nudRecargo9 = new NumericUpDown();
            lblRecargo12 = new Label();
            nudRecargo12 = new NumericUpDown();
            btnGuardar = new Button();
            lblListado = new Label();
            dgvMediosPagos = new DataGridView();
            pnlEncabezado.SuspendLayout();
            pnlFormulario.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudRecargoGeneral).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudRecargo2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudRecargo3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudRecargo6).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudRecargo9).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudRecargo12).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvMediosPagos).BeginInit();
            SuspendLayout();
            // Configura el encabezado principal
            pnlEncabezado.BackColor = Color.FromArgb(21, 30, 41);
            pnlEncabezado.Controls.Add(lblTitulo);
            pnlEncabezado.Controls.Add(lblDescripcion);
            pnlEncabezado.Dock = DockStyle.Top;
            pnlEncabezado.Location = new Point(0, 0);
            pnlEncabezado.Name = "pnlEncabezado";
            pnlEncabezado.Size = new Size(1100, 68);
            // Configura el titulo del formulario
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.White;
            lblTitulo.Location = new Point(16, 8);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Text = "Medios de pago";
            // Explica que se configura en esta pantalla
            lblDescripcion.AutoSize = true;
            lblDescripcion.ForeColor = Color.Gainsboro;
            lblDescripcion.Location = new Point(19, 42);
            lblDescripcion.Name = "lblDescripcion";
            lblDescripcion.Text = "Editá los recargos de los medios de pago predefinidos.";
            // Configura el panel de edicion
            pnlFormulario.BackColor = Color.FromArgb(245, 245, 245);
            pnlFormulario.Controls.Add(lblSeccion);
            pnlFormulario.Controls.Add(lblNombre);
            pnlFormulario.Controls.Add(txtNombreMedioPago);
            pnlFormulario.Controls.Add(lblGeneral);
            pnlFormulario.Controls.Add(nudRecargoGeneral);
            pnlFormulario.Controls.Add(lblRecargo2);
            pnlFormulario.Controls.Add(nudRecargo2);
            pnlFormulario.Controls.Add(lblRecargo3);
            pnlFormulario.Controls.Add(nudRecargo3);
            pnlFormulario.Controls.Add(lblRecargo6);
            pnlFormulario.Controls.Add(nudRecargo6);
            pnlFormulario.Controls.Add(lblRecargo9);
            pnlFormulario.Controls.Add(nudRecargo9);
            pnlFormulario.Controls.Add(lblRecargo12);
            pnlFormulario.Controls.Add(nudRecargo12);
            pnlFormulario.Controls.Add(btnGuardar);
            pnlFormulario.Dock = DockStyle.Top;
            pnlFormulario.Location = new Point(0, 68);
            pnlFormulario.Name = "pnlFormulario";
            pnlFormulario.Size = new Size(1100, 170);
            // Configura el titulo de la seccion de edicion
            lblSeccion.AutoSize = true;
            lblSeccion.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblSeccion.Location = new Point(16, 10);
            lblSeccion.Name = "lblSeccion";
            lblSeccion.Text = "Selecciona un medio de pago para editar sus recargos";
            // Configura la etiqueta del nombre
            lblNombre.AutoSize = true;
            lblNombre.Location = new Point(16, 48);
            lblNombre.Name = "lblNombre";
            lblNombre.Text = "Nombre del medio";
            // Configura el campo del nombre
            txtNombreMedioPago.Location = new Point(16, 68);
            txtNombreMedioPago.Name = "txtNombreMedioPago";
            txtNombreMedioPago.Size = new Size(180, 23);
            // Configura la etiqueta del recargo general
            lblGeneral.AutoSize = true;
            lblGeneral.Location = new Point(216, 48);
            lblGeneral.Name = "lblGeneral";
            lblGeneral.Text = "Recargo general (%)";
            // Configura el campo del recargo general
            nudRecargoGeneral.DecimalPlaces = 2;
            nudRecargoGeneral.Maximum = 999.99M;
            nudRecargoGeneral.Location = new Point(216, 68);
            nudRecargoGeneral.Name = "nudRecargoGeneral";
            nudRecargoGeneral.Size = new Size(120, 23);
            lblRecargo2.AutoSize = true;
            lblRecargo2.Location = new Point(356, 48);
            lblRecargo2.Name = "lblRecargo2";
            lblRecargo2.Text = "Tarjeta 2 cuotas (%)";
            lblRecargo2.ForeColor = Color.FromArgb(145, 170, 210);
            nudRecargo2.DecimalPlaces = 2;
            nudRecargo2.Maximum = 999.99M;
            nudRecargo2.Location = new Point(356, 68);
            nudRecargo2.Name = "nudRecargo2";
            nudRecargo2.Size = new Size(120, 23);
            nudRecargo2.Font = new Font("Segoe UI", 10F);
            // Configura la etiqueta del recargo de tres cuotas
            lblRecargo3.AutoSize = true;
            lblRecargo3.Location = new Point(496, 48);
            lblRecargo3.Name = "lblRecargo3";
            lblRecargo3.Text = "Tarjeta 3 cuotas (%)";
            // Configura el campo del recargo de tres cuotas
            nudRecargo3.DecimalPlaces = 2;
            nudRecargo3.Maximum = 999.99M;
            nudRecargo3.Location = new Point(496, 68);
            nudRecargo3.Name = "nudRecargo3";
            nudRecargo3.Size = new Size(120, 23);
            // Configura la etiqueta del recargo de seis cuotas
            lblRecargo6.AutoSize = true;
            lblRecargo6.Location = new Point(636, 48);
            lblRecargo6.Name = "lblRecargo6";
            lblRecargo6.Text = "Tarjeta 6 cuotas (%)";
            // Configura el campo del recargo de seis cuotas
            nudRecargo6.DecimalPlaces = 2;
            nudRecargo6.Maximum = 999.99M;
            nudRecargo6.Location = new Point(636, 68);
            nudRecargo6.Name = "nudRecargo6";
            nudRecargo6.Size = new Size(120, 23);
            // Configura la etiqueta del recargo de nueve cuotas
            lblRecargo9.AutoSize = true;
            lblRecargo9.Location = new Point(776, 48);
            lblRecargo9.Name = "lblRecargo9";
            lblRecargo9.Text = "Tarjeta 9 cuotas (%)";
            // Configura el campo del recargo de nueve cuotas
            nudRecargo9.DecimalPlaces = 2;
            nudRecargo9.Maximum = 999.99M;
            nudRecargo9.Location = new Point(776, 68);
            nudRecargo9.Name = "nudRecargo9";
            nudRecargo9.Size = new Size(120, 23);
            // Configura la etiqueta del recargo de doce cuotas
            lblRecargo12.AutoSize = true;
            lblRecargo12.Location = new Point(916, 48);
            lblRecargo12.Name = "lblRecargo12";
            lblRecargo12.Text = "Tarjeta 12 cuotas (%)";
            // Configura el campo del recargo de doce cuotas
            nudRecargo12.DecimalPlaces = 2;
            nudRecargo12.Maximum = 999.99M;
            nudRecargo12.Location = new Point(916, 68);
            nudRecargo12.Name = "nudRecargo12";
            nudRecargo12.Size = new Size(120, 23);
            // Guarda los recargos del medio seleccionado
            btnGuardar.Location = new Point(16, 112);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(110, 30);
            btnGuardar.Text = "Guardar";
            btnGuardar.UseVisualStyleBackColor = true;
            btnGuardar.Click += btnGuardar_Click;
            // Configura el titulo de la lista
            lblListado.AutoSize = true;
            lblListado.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblListado.Location = new Point(16, 250);
            lblListado.Name = "lblListado";
            lblListado.Text = "Medios de pago activos. Seleccioná una fila para editarla.";
            // Configura la grilla de medios de pago
            dgvMediosPagos.AllowUserToAddRows = false;
            dgvMediosPagos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvMediosPagos.Dock = DockStyle.Bottom;
            dgvMediosPagos.Location = new Point(0, 280);
            dgvMediosPagos.MultiSelect = false;
            dgvMediosPagos.Name = "dgvMediosPagos";
            dgvMediosPagos.ReadOnly = true;
            dgvMediosPagos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvMediosPagos.Size = new Size(1100, 320);
            dgvMediosPagos.TabIndex = 9;
            dgvMediosPagos.CellClick += dgvMediosPagos_CellClick;
            // Configura el aspecto general del formulario
            BackColor = Color.FromArgb(13, 19, 27);
            ForeColor = Color.White;
            AutoScroll = true;
            AutoScrollMinSize = new Size(940, 590);
            pnlEncabezado.Height = 76;
            lblTitulo.Location = new Point(20, 8);
            lblDescripcion.Location = new Point(22, 45);
            lblDescripcion.ForeColor = Color.FromArgb(145, 170, 210);
            pnlFormulario.BackColor = Color.FromArgb(21, 30, 41);
            pnlFormulario.Height = 182;
            lblSeccion.ForeColor = Color.White;
            lblNombre.ForeColor = Color.FromArgb(145, 170, 210);
            lblGeneral.ForeColor = Color.FromArgb(145, 170, 210);
            lblRecargo3.ForeColor = Color.FromArgb(145, 170, 210);
            lblRecargo6.ForeColor = Color.FromArgb(145, 170, 210);
            lblRecargo9.ForeColor = Color.FromArgb(145, 170, 210);
            lblRecargo12.ForeColor = Color.FromArgb(145, 170, 210);
            txtNombreMedioPago.Font = new Font("Segoe UI", 10F);
            nudRecargoGeneral.Font = new Font("Segoe UI", 10F);
            nudRecargo3.Font = new Font("Segoe UI", 10F);
            nudRecargo6.Font = new Font("Segoe UI", 10F);
            nudRecargo9.Font = new Font("Segoe UI", 10F);
            nudRecargo12.Font = new Font("Segoe UI", 10F);
            txtNombreMedioPago.TabIndex = 0;
            nudRecargoGeneral.TabIndex = 1;
            nudRecargo3.TabIndex = 2;
            nudRecargo6.TabIndex = 3;
            nudRecargo9.TabIndex = 4;
            nudRecargo12.TabIndex = 5;
            btnGuardar.TabIndex = 6;
            pnlListado.Dock = DockStyle.Fill;
            pnlListado.Padding = new Padding(16);
            pnlListado.Controls.Add(dgvMediosPagos);
            pnlListado.Controls.Add(lblListado);
            lblListado.AutoSize = false;
            lblListado.Dock = DockStyle.Top;
            lblListado.Height = 40;
            lblListado.ForeColor = Color.White;
            dgvMediosPagos.Dock = DockStyle.Fill;
            btnGuardar.BackColor = Color.DodgerBlue;
            btnGuardar.ForeColor = Color.White;
            btnGuardar.FlatStyle = FlatStyle.Flat;
            btnGuardar.FlatAppearance.BorderSize = 0;
            btnGuardar.UseVisualStyleBackColor = false;
            btnGuardar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnGuardar.Height = 34;
            dgvMediosPagos.BackgroundColor = Color.FromArgb(13, 19, 27);
            dgvMediosPagos.BorderStyle = BorderStyle.None;
            dgvMediosPagos.RowHeadersVisible = false;
            dgvMediosPagos.AllowUserToDeleteRows = false;
            dgvMediosPagos.EnableHeadersVisualStyles = false;
            dgvMediosPagos.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(37, 52, 70);
            dgvMediosPagos.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvMediosPagos.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgvMediosPagos.ColumnHeadersHeight = 38;
            dgvMediosPagos.RowTemplate.Height = 32;
            dgvMediosPagos.DefaultCellStyle.Padding = new Padding(5);
            dgvMediosPagos.DefaultCellStyle.BackColor = Color.White;
            dgvMediosPagos.DefaultCellStyle.ForeColor = Color.FromArgb(21, 30, 41);
            dgvMediosPagos.DefaultCellStyle.SelectionBackColor = Color.FromArgb(35, 100, 166);
            dgvMediosPagos.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvMediosPagos.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(236, 242, 249);

            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1100, 600);
            Controls.Add(pnlListado);

            Controls.Add(pnlFormulario);
            Controls.Add(pnlEncabezado);
            Name = "frmMediosPagos";
            Text = "Medios de pago";
            pnlEncabezado.ResumeLayout(false);
            pnlEncabezado.PerformLayout();
            pnlFormulario.ResumeLayout(false);
            pnlFormulario.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudRecargoGeneral).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudRecargo2).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudRecargo3).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudRecargo6).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudRecargo9).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudRecargo12).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvMediosPagos).EndInit();
            cboTipoCaja.Name = "cboTipoCaja";
            cboTipoCaja.DropDownStyle = ComboBoxStyle.DropDownList;
            cboTipoCaja.Location = new Point(20, 155);
            cboTipoCaja.Size = new Size(160, 25);
            pnlFormulario.Controls.Add(cboTipoCaja);

            lblTipoCaja.Name = "lblTipoCaja";
            lblTipoCaja.Text = "Tipo en caja";
            lblTipoCaja.Location = new Point(20, 130);
            lblTipoCaja.Size = new Size(160, 23);
            lblTipoCaja.ForeColor = Color.White;
            pnlFormulario.Controls.Add(lblTipoCaja);

            Load += frmMediosPagos_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        private Panel pnlListado;
        private Panel pnlEncabezado;
        private Label lblTitulo;
        private Label lblDescripcion;
        private Panel pnlFormulario;
        private Label lblSeccion;
        private Label lblNombre;
        private TextBox txtNombreMedioPago;
        private Label lblGeneral;
        private NumericUpDown nudRecargoGeneral;
        private Label lblRecargo2;
        private NumericUpDown nudRecargo2;
        private Label lblRecargo3;
        private NumericUpDown nudRecargo3;
        private Label lblRecargo6;
        private NumericUpDown nudRecargo6;
        private Label lblRecargo9;
        private NumericUpDown nudRecargo9;
        private Label lblRecargo12;
        private NumericUpDown nudRecargo12;
        private Button btnGuardar;
        private Label lblListado;
        private DataGridView dgvMediosPagos;
        private ComboBox cboTipoCaja;

        private Label lblTipoCaja;

    }
}
