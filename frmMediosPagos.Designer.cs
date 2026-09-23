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
            lblRecargo3 = new Label();
            nudRecargo3 = new NumericUpDown();
            lblRecargo6 = new Label();
            nudRecargo6 = new NumericUpDown();
            lblRecargo9 = new Label();
            nudRecargo9 = new NumericUpDown();
            lblRecargo12 = new Label();
            nudRecargo12 = new NumericUpDown();
            btnGuardar = new Button();
            btnNuevo = new Button();
            btnDesactivar = new Button();
            lblListado = new Label();
            dgvMediosPagos = new DataGridView();
            pnlEncabezado.SuspendLayout();
            pnlFormulario.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudRecargoGeneral).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudRecargo3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudRecargo6).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudRecargo9).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudRecargo12).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvMediosPagos).BeginInit();
            SuspendLayout();
            // Encabezado principal.
            pnlEncabezado.BackColor = Color.FromArgb(21, 30, 41);
            pnlEncabezado.Controls.Add(lblTitulo);
            pnlEncabezado.Controls.Add(lblDescripcion);
            pnlEncabezado.Dock = DockStyle.Top;
            pnlEncabezado.Location = new Point(0, 0);
            pnlEncabezado.Name = "pnlEncabezado";
            pnlEncabezado.Size = new Size(1100, 68);
            // Título del formulario.
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.White;
            lblTitulo.Location = new Point(16, 8);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Text = "Medios de pago";
            // Explica qué se configura en esta pantalla.
            lblDescripcion.AutoSize = true;
            lblDescripcion.ForeColor = Color.Gainsboro;
            lblDescripcion.Location = new Point(19, 42);
            lblDescripcion.Name = "lblDescripcion";
            lblDescripcion.Text = "Configurá los recargos para efectivo, transferencia y tarjetas.";
            // Panel de alta y edición.
            pnlFormulario.BackColor = Color.FromArgb(245, 245, 245);
            pnlFormulario.Controls.Add(lblSeccion);
            pnlFormulario.Controls.Add(lblNombre);
            pnlFormulario.Controls.Add(txtNombreMedioPago);
            pnlFormulario.Controls.Add(lblGeneral);
            pnlFormulario.Controls.Add(nudRecargoGeneral);
            pnlFormulario.Controls.Add(lblRecargo3);
            pnlFormulario.Controls.Add(nudRecargo3);
            pnlFormulario.Controls.Add(lblRecargo6);
            pnlFormulario.Controls.Add(nudRecargo6);
            pnlFormulario.Controls.Add(lblRecargo9);
            pnlFormulario.Controls.Add(nudRecargo9);
            pnlFormulario.Controls.Add(lblRecargo12);
            pnlFormulario.Controls.Add(nudRecargo12);
            pnlFormulario.Controls.Add(btnGuardar);
            pnlFormulario.Controls.Add(btnNuevo);
            pnlFormulario.Controls.Add(btnDesactivar);
            pnlFormulario.Dock = DockStyle.Top;
            pnlFormulario.Location = new Point(0, 68);
            pnlFormulario.Name = "pnlFormulario";
            pnlFormulario.Size = new Size(1100, 170);
            // Título de la sección de edición.
            lblSeccion.AutoSize = true;
            lblSeccion.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblSeccion.Location = new Point(16, 10);
            lblSeccion.Name = "lblSeccion";
            lblSeccion.Text = "Nuevo medio de pago o edición del seleccionado";
            // Label del nombre.
            lblNombre.AutoSize = true;
            lblNombre.Location = new Point(16, 48);
            lblNombre.Name = "lblNombre";
            lblNombre.Text = "Nombre del medio";
            // Campo del nombre.
            txtNombreMedioPago.Location = new Point(16, 68);
            txtNombreMedioPago.Name = "txtNombreMedioPago";
            txtNombreMedioPago.Size = new Size(180, 23);
            // Label del recargo general.
            lblGeneral.AutoSize = true;
            lblGeneral.Location = new Point(216, 48);
            lblGeneral.Name = "lblGeneral";
            lblGeneral.Text = "Recargo general (%)";
            // Campo del recargo general.
            nudRecargoGeneral.DecimalPlaces = 2;
            nudRecargoGeneral.Maximum = 1000;
            nudRecargoGeneral.Location = new Point(216, 68);
            nudRecargoGeneral.Name = "nudRecargoGeneral";
            nudRecargoGeneral.Size = new Size(120, 23);
            // Label del recargo de tres cuotas.
            lblRecargo3.AutoSize = true;
            lblRecargo3.Location = new Point(356, 48);
            lblRecargo3.Name = "lblRecargo3";
            lblRecargo3.Text = "Tarjeta 3 cuotas (%)";
            // Campo del recargo de tres cuotas.
            nudRecargo3.DecimalPlaces = 2;
            nudRecargo3.Maximum = 1000;
            nudRecargo3.Location = new Point(356, 68);
            nudRecargo3.Name = "nudRecargo3";
            nudRecargo3.Size = new Size(120, 23);
            // Label del recargo de seis cuotas.
            lblRecargo6.AutoSize = true;
            lblRecargo6.Location = new Point(496, 48);
            lblRecargo6.Name = "lblRecargo6";
            lblRecargo6.Text = "Tarjeta 6 cuotas (%)";
            // Campo del recargo de seis cuotas.
            nudRecargo6.DecimalPlaces = 2;
            nudRecargo6.Maximum = 1000;
            nudRecargo6.Location = new Point(496, 68);
            nudRecargo6.Name = "nudRecargo6";
            nudRecargo6.Size = new Size(120, 23);
            // Label del recargo de nueve cuotas.
            lblRecargo9.AutoSize = true;
            lblRecargo9.Location = new Point(636, 48);
            lblRecargo9.Name = "lblRecargo9";
            lblRecargo9.Text = "Tarjeta 9 cuotas (%)";
            // Campo del recargo de nueve cuotas.
            nudRecargo9.DecimalPlaces = 2;
            nudRecargo9.Maximum = 1000;
            nudRecargo9.Location = new Point(636, 68);
            nudRecargo9.Name = "nudRecargo9";
            nudRecargo9.Size = new Size(120, 23);
            // Label del recargo de doce cuotas.
            lblRecargo12.AutoSize = true;
            lblRecargo12.Location = new Point(776, 48);
            lblRecargo12.Name = "lblRecargo12";
            lblRecargo12.Text = "Tarjeta 12 cuotas (%)";
            // Campo del recargo de doce cuotas.
            nudRecargo12.DecimalPlaces = 2;
            nudRecargo12.Maximum = 1000;
            nudRecargo12.Location = new Point(776, 68);
            nudRecargo12.Name = "nudRecargo12";
            nudRecargo12.Size = new Size(120, 23);
            // Guarda el alta o la edición.
            btnGuardar.Location = new Point(16, 112);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(110, 30);
            btnGuardar.Text = "Guardar";
            btnGuardar.UseVisualStyleBackColor = true;
            btnGuardar.Click += btnGuardar_Click;
            // Limpia el formulario para iniciar una alta.
            btnNuevo.Location = new Point(136, 112);
            btnNuevo.Name = "btnNuevo";
            btnNuevo.Size = new Size(145, 30);
            btnNuevo.Text = "Nuevo";
            btnNuevo.UseVisualStyleBackColor = true;
            btnNuevo.Click += btnNuevo_Click;
            // Desactiva el medio seleccionado.
            btnDesactivar.Enabled = false;
            btnDesactivar.Location = new Point(291, 112);
            btnDesactivar.Name = "btnDesactivar";
            btnDesactivar.Size = new Size(110, 30);
            btnDesactivar.Text = "Desactivar";
            btnDesactivar.UseVisualStyleBackColor = true;
            btnDesactivar.Click += btnDesactivar_Click;
            // Título de la lista.
            lblListado.AutoSize = true;
            lblListado.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblListado.Location = new Point(16, 250);
            lblListado.Name = "lblListado";
            lblListado.Text = "Medios de pago activos. Seleccioná una fila para editarla.";
            // Grilla editable desde el diseñador.
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
            // Configuración general del formulario.
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
            btnNuevo.TabIndex = 7;
            btnDesactivar.TabIndex = 8;
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
            btnNuevo.BackColor = Color.SteelBlue;
            btnNuevo.ForeColor = Color.White;
            btnNuevo.FlatStyle = FlatStyle.Flat;
            btnNuevo.FlatAppearance.BorderSize = 0;
            btnNuevo.UseVisualStyleBackColor = false;
            btnNuevo.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnNuevo.Height = 34;
            btnDesactivar.BackColor = Color.FromArgb(153, 55, 65);
            btnDesactivar.ForeColor = Color.White;
            btnDesactivar.FlatStyle = FlatStyle.Flat;
            btnDesactivar.FlatAppearance.BorderSize = 0;
            btnDesactivar.UseVisualStyleBackColor = false;
            btnDesactivar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnDesactivar.Height = 34;
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
            ((System.ComponentModel.ISupportInitialize)nudRecargo3).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudRecargo6).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudRecargo9).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudRecargo12).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvMediosPagos).EndInit();
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
        private Label lblRecargo3;
        private NumericUpDown nudRecargo3;
        private Label lblRecargo6;
        private NumericUpDown nudRecargo6;
        private Label lblRecargo9;
        private NumericUpDown nudRecargo9;
        private Label lblRecargo12;
        private NumericUpDown nudRecargo12;
        private Button btnGuardar;
        private Button btnNuevo;
        private Button btnDesactivar;
        private Label lblListado;
        private DataGridView dgvMediosPagos;
    }
}
