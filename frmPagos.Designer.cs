namespace Prana.Presentacion
{
    // parte visual de frmpagos crea controles y define su distribucion, colores y eventos; visual studio puede regenerar este archivo al editar el disenador
    partial class frmPagos
    {
        private System.ComponentModel.IContainer components = null;
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
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            lblCuentaDestino = new Label();
            txtCuentaDestino = new TextBox();
            pnlTitulo = new Panel();
            lblTitulo = new Label();
            pnlFiltros = new Panel();
            txtBuscar = new TextBox();
            btnBuscar = new Button();
            btnGarantia = new Button();
            chkPendientes = new CheckBox();
            contenido = new TableLayoutPanel();
            pnlVentas = new Panel();
            dgvVentas = new DataGridView();
            lblVentas = new Label();
            pnlCobro = new Panel();
            lblSeleccion = new Label();
            lblImporte = new Label();
            nudImporte = new NumericUpDown();
            lblMedio = new Label();
            cboMedio = new ComboBox();
            lblCuotas = new Label();
            cboCuotas = new ComboBox();
            lblCobro = new Label();
            btnRegistrar = new Button();
            pnlPagos = new Panel();
            dgvPagos = new DataGridView();
            lblPagos = new Label();
            flpFiltros = new FlowLayoutPanel();
            flpCobro = new FlowLayoutPanel();
            pnlCampo27 = new Panel();
            pnlCampo28 = new Panel();
            pnlCampo29 = new Panel();
            pnlCampo30 = new Panel();
            pnlPagina = new Panel();
            tlpSecciones = new TableLayoutPanel();
            pnlTitulo.SuspendLayout();
            contenido.SuspendLayout();
            pnlVentas.SuspendLayout();
            // llama a ((systemcomponentmodelisupportinitialize)dgvventas)begininit para realizar este paso del flujo
            ((System.ComponentModel.ISupportInitialize)dgvVentas).BeginInit();
            // llama a ((systemcomponentmodelisupportinitialize)nudimporte)begininit para realizar este paso del flujo
            ((System.ComponentModel.ISupportInitialize)nudImporte).BeginInit();
            pnlPagos.SuspendLayout();
            // llama a ((systemcomponentmodelisupportinitialize)dgvpagos)begininit para realizar este paso del flujo
            ((System.ComponentModel.ISupportInitialize)dgvPagos).BeginInit();
            flpFiltros.SuspendLayout();
            flpCobro.SuspendLayout();
            pnlCampo27.SuspendLayout();
            pnlCampo28.SuspendLayout();
            pnlCampo29.SuspendLayout();
            pnlPagina.SuspendLayout();
            tlpSecciones.SuspendLayout();
            SuspendLayout();
            // lblcuentadestino lblcuentadestinodock: borde o zona del contenedor a la que se ajusta automaticamente el control
            lblCuentaDestino.Dock = DockStyle.Top;
            lblCuentaDestino.ForeColor = Color.White;
            lblCuentaDestino.Location = new Point(0, 0);
            lblCuentaDestino.Name = "lblCuentaDestino";
            lblCuentaDestino.Size = new Size(1200, 23);
            lblCuentaDestino.TabIndex = 1;
            lblCuentaDestino.Text = "Cuenta destino / referencia";
            // txtcuentadestino txtcuentadestinoanchor: bordes que mantienen su distancia cuando cambia el tamano del contenedor
            txtCuentaDestino.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtCuentaDestino.Location = new Point(0, 26);
            txtCuentaDestino.MaxLength = 100;
            txtCuentaDestino.Name = "txtCuentaDestino";
            txtCuentaDestino.Size = new Size(220, 23);
            txtCuentaDestino.TabIndex = 0;
            // pnltitulo pnltitulobackcolor: color del fondo del control o de la celda
            pnlTitulo.BackColor = Color.FromArgb(21, 30, 41);
            pnlTitulo.Controls.Add(lblTitulo);
            pnlTitulo.Dock = DockStyle.Top;
            pnlTitulo.Location = new Point(0, 23);
            pnlTitulo.Margin = new Padding(0, 0, 0, 8);
            pnlTitulo.Name = "pnlTitulo";
            pnlTitulo.Size = new Size(1200, 76);
            pnlTitulo.TabIndex = 0;
            // lbltitulo
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTitulo.Location = new Point(20, 20);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(82, 32);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Pagos";
            // pnlfiltros pnlfiltrosbackcolor: color del fondo del control o de la celda
            pnlFiltros.BackColor = Color.FromArgb(21, 30, 41);
            pnlFiltros.Dock = DockStyle.Top;
            pnlFiltros.Location = new Point(0, 99);
            pnlFiltros.Name = "pnlFiltros";
            pnlFiltros.Size = new Size(1200, 70);
            pnlFiltros.TabIndex = 1;
            pnlFiltros.Visible = false;
            // txtbuscar txtbuscarfont: tipografia, tamano y estilo con los que se muestra el contenido
            txtBuscar.Font = new Font("Segoe UI", 10F);
            txtBuscar.Location = new Point(12, 12);
            txtBuscar.Margin = new Padding(4, 4, 12, 4);
            txtBuscar.MaxLength = 30;
            txtBuscar.Name = "txtBuscar";
            txtBuscar.PlaceholderText = "Buscar por venta, nombre completo, DNI o CUIT";
            txtBuscar.Size = new Size(420, 25);
            txtBuscar.TabIndex = 0;
            txtBuscar.KeyDown += txtBuscar_KeyDown;
            // btnbuscar btnbuscarbackcolor: color del fondo del control o de la celda
            btnBuscar.BackColor = Color.DodgerBlue;
            btnBuscar.FlatAppearance.BorderSize = 0;
            btnBuscar.FlatStyle = FlatStyle.Flat;
            btnBuscar.ForeColor = Color.White;
            btnBuscar.Location = new Point(448, 12);
            btnBuscar.Margin = new Padding(4, 4, 12, 4);
            btnBuscar.Name = "btnBuscar";
            btnBuscar.Size = new Size(135, 34);
            btnBuscar.TabIndex = 1;
            btnBuscar.Text = "Buscar / Actualizar";
            btnBuscar.UseVisualStyleBackColor = false;
            btnBuscar.Click += btnBuscar_Click;
            // btngarantia btngarantianame: nombre con el que el formulario identifica este control en el codigo
            btnGarantia.Name = "btnGarantia";
            btnGarantia.Text = "Emitir garantía actualizada";
            btnGarantia.Size = new Size(200, 34);
            btnGarantia.Margin = new Padding(4, 4, 12, 4);
            btnGarantia.BackColor = Color.DodgerBlue;
            btnGarantia.ForeColor = Color.White;
            btnGarantia.FlatStyle = FlatStyle.Flat;
            btnGarantia.Enabled = false;
            btnGarantia.Click += btnGarantia_Click;
            // chkpendientes
            chkPendientes.AutoSize = true;
            chkPendientes.Checked = true;
            chkPendientes.CheckState = CheckState.Checked;
            chkPendientes.Location = new Point(599, 12);
            chkPendientes.Margin = new Padding(4, 4, 12, 4);
            chkPendientes.Name = "chkPendientes";
            chkPendientes.Size = new Size(159, 19);
            chkPendientes.TabIndex = 2;
            chkPendientes.Text = "Solo con saldo pendiente";
            // contenido
            contenido.ColumnCount = 1;
            contenido.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            contenido.Controls.Add(pnlCobro, 0, 1);
            contenido.Dock = DockStyle.Fill;
            contenido.Location = new Point(0, 169);
            contenido.Name = "contenido";
            contenido.Padding = new Padding(16);
            contenido.RowCount = 3;
            contenido.RowStyles.Add(new RowStyle(SizeType.Percent, 55F));
            contenido.RowStyles.Add(new RowStyle(SizeType.Absolute, 165F));
            contenido.RowStyles.Add(new RowStyle(SizeType.Percent, 45F));
            contenido.Size = new Size(1200, 651);
            contenido.TabIndex = 0;
            contenido.Visible = false;
            // pnlventas inserta el control en su contenedor; esto determina su jerarquia visual y posicion relativa
            pnlVentas.Controls.Add(dgvVentas);
            pnlVentas.Controls.Add(lblVentas);
            pnlVentas.Dock = DockStyle.Top;
            pnlVentas.Location = new Point(8, 74);
            pnlVentas.Margin = new Padding(0, 0, 0, 8);
            pnlVentas.Name = "pnlVentas";
            pnlVentas.Size = new Size(1167, 230);
            pnlVentas.TabIndex = 2;
            // dgvventas
            dgvVentas.AllowUserToAddRows = false;
            dgvVentas.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(236, 242, 249);
            dgvVentas.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dgvVentas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvVentas.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvVentas.BackgroundColor = Color.FromArgb(13, 19, 27);
            dgvVentas.BorderStyle = BorderStyle.None;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(37, 52, 70);
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = Color.White;
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(190, 220, 250);
            dataGridViewCellStyle2.SelectionForeColor = Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(190, 220, 250);
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dgvVentas.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dgvVentas.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.White;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(21, 30, 41);
            dataGridViewCellStyle3.Padding = new Padding(4);
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(190, 220, 250);
            dataGridViewCellStyle3.SelectionForeColor = Color.Black;
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(190, 220, 250);
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
            dgvVentas.DefaultCellStyle = dataGridViewCellStyle3;
            dgvVentas.Dock = DockStyle.Fill;
            dgvVentas.EnableHeadersVisualStyles = false;
            dgvVentas.Location = new Point(0, 34);
            dgvVentas.MultiSelect = false;
            dgvVentas.Name = "dgvVentas";
            dgvVentas.ForeColor = Color.Black;
            dgvVentas.DefaultCellStyle.ForeColor = Color.Black;
            dgvVentas.RowsDefaultCellStyle.ForeColor = Color.Black;
            dgvVentas.AlternatingRowsDefaultCellStyle.ForeColor = Color.Black;
            dgvVentas.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.Black;
            dgvVentas.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(190, 220, 250);
            dgvVentas.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvVentas.DefaultCellStyle.SelectionBackColor = Color.FromArgb(190, 220, 250);
            dgvVentas.AlternatingRowsDefaultCellStyle.SelectionForeColor = Color.Black;
            dgvVentas.AlternatingRowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(190, 220, 250);
            dgvVentas.ReadOnly = true;
            dgvVentas.RowHeadersVisible = false;
            dgvVentas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvVentas.Size = new Size(1167, 196);
            dgvVentas.TabIndex = 0;
            dgvVentas.SelectionChanged += dgvVentas_SelectionChanged;
            // lblventas lblventasdock: borde o zona del contenedor a la que se ajusta automaticamente el control
            lblVentas.Dock = DockStyle.Top;
            lblVentas.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblVentas.Location = new Point(0, 0);
            lblVentas.Name = "lblVentas";
            lblVentas.Size = new Size(1167, 34);
            lblVentas.TabIndex = 1;
            lblVentas.Text = "Ventas";
            // pnlcobro pnlcobrobackcolor: color del fondo del control o de la celda
            pnlCobro.BackColor = Color.FromArgb(21, 30, 41);
            pnlCobro.Dock = DockStyle.Fill;
            pnlCobro.Location = new Point(19, 268);
            pnlCobro.Name = "pnlCobro";
            pnlCobro.Size = new Size(1162, 159);
            pnlCobro.TabIndex = 3;
            // lblseleccion lblselecciondock: borde o zona del contenedor a la que se ajusta automaticamente el control
            lblSeleccion.Dock = DockStyle.Top;
            lblSeleccion.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblSeleccion.Location = new Point(8, 312);
            lblSeleccion.Margin = new Padding(0, 0, 0, 8);
            lblSeleccion.Name = "lblSeleccion";
            lblSeleccion.Size = new Size(1167, 32);
            lblSeleccion.TabIndex = 3;
            lblSeleccion.Text = "Seleccione una venta";
            // lblimporte lblimportedock: borde o zona del contenedor a la que se ajusta automaticamente el control
            lblImporte.Dock = DockStyle.Top;
            lblImporte.ForeColor = Color.FromArgb(145, 170, 210);
            lblImporte.Location = new Point(0, 0);
            lblImporte.Name = "lblImporte";
            lblImporte.Size = new Size(215, 23);
            lblImporte.TabIndex = 4;
            lblImporte.Text = "Importe a cancelar (sin recargo)";
            // nudimporte nudimporteanchor: bordes que mantienen su distancia cuando cambia el tamano del contenedor
            nudImporte.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            nudImporte.DecimalPlaces = 2;
            nudImporte.Location = new Point(0, 26);
            nudImporte.Maximum = new decimal(new int[] { 99999999, 0, 0, 0 });
            nudImporte.Name = "nudImporte";
            nudImporte.Size = new Size(177, 23);
            nudImporte.TabIndex = 0;
            nudImporte.ThousandsSeparator = true;
            nudImporte.ValueChanged += nudImporte_ValueChanged;
            // lblmedio lblmediodock: borde o zona del contenedor a la que se ajusta automaticamente el control
            lblMedio.Dock = DockStyle.Top;
            lblMedio.ForeColor = Color.FromArgb(145, 170, 210);
            lblMedio.Location = new Point(0, 0);
            lblMedio.Name = "lblMedio";
            lblMedio.Size = new Size(220, 23);
            lblMedio.TabIndex = 5;
            lblMedio.Text = "Medio de pago";
            // cbomedio cbomedioanchor: bordes que mantienen su distancia cuando cambia el tamano del contenedor
            cboMedio.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            cboMedio.DropDownStyle = ComboBoxStyle.DropDownList;
            cboMedio.Location = new Point(0, 26);
            cboMedio.Name = "cboMedio";
            cboMedio.Size = new Size(182, 23);
            cboMedio.TabIndex = 1;
            cboMedio.SelectedIndexChanged += cboMedio_SelectedIndexChanged;
            // lblcuotas lblcuotasdock: borde o zona del contenedor a la que se ajusta automaticamente el control
            lblCuotas.Dock = DockStyle.Top;
            lblCuotas.ForeColor = Color.FromArgb(145, 170, 210);
            lblCuotas.Location = new Point(0, 0);
            lblCuotas.Name = "lblCuotas";
            lblCuotas.Size = new Size(90, 23);
            lblCuotas.TabIndex = 6;
            lblCuotas.Text = "Cuotas";
            // cbocuotas cbocuotasanchor: bordes que mantienen su distancia cuando cambia el tamano del contenedor
            cboCuotas.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            cboCuotas.DropDownStyle = ComboBoxStyle.DropDownList;
            cboCuotas.Items.AddRange(new object[] { 1, 2, 3, 6, 9, 12 });
            cboCuotas.Location = new Point(0, 26);
            cboCuotas.Name = "cboCuotas";
            cboCuotas.Size = new Size(52, 23);
            cboCuotas.TabIndex = 2;
            cboCuotas.SelectedIndexChanged += cboCuotas_SelectedIndexChanged;
            // lblcobro lblcobrodock: borde o zona del contenedor a la que se ajusta automaticamente el control
            lblCobro.Dock = DockStyle.Top;
            lblCobro.ForeColor = Color.FromArgb(255, 195, 95);
            lblCobro.Location = new Point(8, 442);
            lblCobro.Margin = new Padding(0, 0, 0, 8);
            lblCobro.Name = "lblCobro";
            lblCobro.AutoSize = true;
            lblCobro.MinimumSize = new Size(0, 70);
            lblCobro.Size = new Size(1167, 70);
            lblCobro.TabIndex = 5;
            // btnregistrar btnregistrarbackcolor: color del fondo del control o de la celda
            btnRegistrar.BackColor = Color.DodgerBlue;
            btnRegistrar.Enabled = false;
            btnRegistrar.FlatAppearance.BorderSize = 0;
            btnRegistrar.FlatStyle = FlatStyle.Flat;
            btnRegistrar.ForeColor = Color.White;
            btnRegistrar.Location = new Point(821, 12);
            btnRegistrar.Margin = new Padding(4, 4, 12, 4);
            btnRegistrar.Name = "btnRegistrar";
            btnRegistrar.Size = new Size(170, 35);
            btnRegistrar.TabIndex = 3;
            btnRegistrar.Text = "Registrar pago";
            btnRegistrar.UseVisualStyleBackColor = false;
            btnRegistrar.Click += Registrar;
            // pnlpagos inserta el control en su contenedor; esto determina su jerarquia visual y posicion relativa
            pnlPagos.Controls.Add(dgvPagos);
            pnlPagos.Controls.Add(lblPagos);
            pnlPagos.Dock = DockStyle.Top;
            pnlPagos.Location = new Point(8, 492);
            pnlPagos.Margin = new Padding(0, 0, 0, 8);
            pnlPagos.Name = "pnlPagos";
            pnlPagos.Size = new Size(1167, 220);
            pnlPagos.TabIndex = 6;
            // dgvpagos
            dgvPagos.AllowUserToAddRows = false;
            dgvPagos.AllowUserToDeleteRows = false;
            dataGridViewCellStyle4.BackColor = Color.FromArgb(236, 242, 249);
            dgvPagos.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle4;
            dgvPagos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPagos.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvPagos.BackgroundColor = Color.FromArgb(13, 19, 27);
            dgvPagos.BorderStyle = BorderStyle.None;
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = Color.FromArgb(37, 52, 70);
            dataGridViewCellStyle5.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle5.ForeColor = Color.White;
            dataGridViewCellStyle5.SelectionBackColor = Color.FromArgb(190, 220, 250);
            dataGridViewCellStyle5.SelectionForeColor = Color.Black;
            dataGridViewCellStyle5.SelectionBackColor = Color.FromArgb(190, 220, 250);
            dataGridViewCellStyle5.WrapMode = DataGridViewTriState.True;
            dgvPagos.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            dgvPagos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = Color.White;
            dataGridViewCellStyle6.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle6.ForeColor = Color.FromArgb(21, 30, 41);
            dataGridViewCellStyle6.Padding = new Padding(4);
            dataGridViewCellStyle6.SelectionBackColor = Color.FromArgb(190, 220, 250);
            dataGridViewCellStyle6.SelectionForeColor = Color.Black;
            dataGridViewCellStyle6.SelectionBackColor = Color.FromArgb(190, 220, 250);
            dataGridViewCellStyle6.WrapMode = DataGridViewTriState.True;
            dgvPagos.DefaultCellStyle = dataGridViewCellStyle6;
            dgvPagos.Dock = DockStyle.Fill;
            dgvPagos.EnableHeadersVisualStyles = false;
            dgvPagos.Location = new Point(0, 34);
            dgvPagos.MultiSelect = false;
            dgvPagos.Name = "dgvPagos";
            dgvPagos.ForeColor = Color.Black;
            dgvPagos.DefaultCellStyle.ForeColor = Color.Black;
            dgvPagos.RowsDefaultCellStyle.ForeColor = Color.Black;
            dgvPagos.AlternatingRowsDefaultCellStyle.ForeColor = Color.Black;
            dgvPagos.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.Black;
            dgvPagos.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(190, 220, 250);
            dgvPagos.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvPagos.DefaultCellStyle.SelectionBackColor = Color.FromArgb(190, 220, 250);
            dgvPagos.AlternatingRowsDefaultCellStyle.SelectionForeColor = Color.Black;
            dgvPagos.AlternatingRowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(190, 220, 250);
            dgvPagos.ReadOnly = true;
            dgvPagos.RowHeadersVisible = false;
            dgvPagos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPagos.Size = new Size(1167, 186);
            dgvPagos.TabIndex = 0;
            // lblpagos lblpagosdock: borde o zona del contenedor a la que se ajusta automaticamente el control
            lblPagos.Dock = DockStyle.Top;
            lblPagos.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblPagos.Location = new Point(0, 0);
            lblPagos.Name = "lblPagos";
            lblPagos.Size = new Size(1167, 34);
            lblPagos.TabIndex = 1;
            lblPagos.Text = "Pagos registrados";
            // flpfiltros
            flpFiltros.AutoSize = true;
            flpFiltros.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flpFiltros.Controls.Add(txtBuscar);
            flpFiltros.Controls.Add(btnBuscar);
            flpFiltros.Controls.Add(chkPendientes);
            flpFiltros.Controls.Add(btnGarantia);
            flpFiltros.Dock = DockStyle.Top;
            flpFiltros.Location = new Point(8, 8);
            flpFiltros.Margin = new Padding(0, 0, 0, 8);
            flpFiltros.Name = "flpFiltros";
            flpFiltros.Padding = new Padding(8);
            flpFiltros.Size = new Size(1167, 58);
            flpFiltros.TabIndex = 1;
            // flpcobro
            flpCobro.AutoSize = true;
            flpCobro.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flpCobro.Controls.Add(pnlCampo27);
            flpCobro.Controls.Add(pnlCampo28);
            flpCobro.Controls.Add(pnlCampo29);
            flpCobro.Controls.Add(pnlCampo30);
            flpCobro.Controls.Add(btnRegistrar);
            flpCobro.Dock = DockStyle.Top;
            flpCobro.Location = new Point(8, 352);
            flpCobro.Margin = new Padding(0, 0, 0, 8);
            flpCobro.Name = "flpCobro";
            flpCobro.Padding = new Padding(8);
            flpCobro.Size = new Size(1167, 82);
            flpCobro.TabIndex = 4;
            // pnlcampo27 inserta el control en su contenedor; esto determina su jerarquia visual y posicion relativa
            pnlCampo27.Controls.Add(nudImporte);
            pnlCampo27.Controls.Add(lblImporte);
            pnlCampo27.Location = new Point(12, 12);
            pnlCampo27.Margin = new Padding(4, 4, 12, 4);
            pnlCampo27.Name = "pnlCampo27";
            pnlCampo27.Size = new Size(215, 58);
            pnlCampo27.TabIndex = 0;
            // pnlcampo28 inserta el control en su contenedor; esto determina su jerarquia visual y posicion relativa
            pnlCampo28.Controls.Add(cboMedio);
            pnlCampo28.Controls.Add(lblMedio);
            pnlCampo28.Location = new Point(243, 12);
            pnlCampo28.Margin = new Padding(4, 4, 12, 4);
            pnlCampo28.Name = "pnlCampo28";
            pnlCampo28.Size = new Size(220, 58);
            pnlCampo28.TabIndex = 1;
            // pnlcampo29 inserta el control en su contenedor; esto determina su jerarquia visual y posicion relativa
            pnlCampo29.Controls.Add(cboCuotas);
            pnlCampo29.Controls.Add(lblCuotas);
            pnlCampo29.Location = new Point(479, 12);
            pnlCampo29.Margin = new Padding(4, 4, 12, 4);
            pnlCampo29.Name = "pnlCampo29";
            pnlCampo29.Size = new Size(90, 58);
            pnlCampo29.TabIndex = 2;
            // pnlcampo30 pnlcampo30location: posicion horizontal y vertical respecto del contenedor
            pnlCampo30.Location = new Point(585, 12);
            pnlCampo30.Margin = new Padding(4, 4, 12, 4);
            pnlCampo30.Name = "pnlCampo30";
            pnlCampo30.Size = new Size(220, 58);
            pnlCampo30.TabIndex = 3;
            // pnlpagina
            pnlPagina.AutoScroll = true;
            pnlPagina.BackColor = Color.FromArgb(13, 19, 27);
            pnlPagina.Controls.Add(tlpSecciones);
            pnlPagina.Dock = DockStyle.Fill;
            pnlPagina.ForeColor = Color.White;
            pnlPagina.Location = new Point(0, 169);
            pnlPagina.Name = "pnlPagina";
            pnlPagina.Size = new Size(1200, 651);
            pnlPagina.TabIndex = 2;
            // tlpsecciones
            tlpSecciones.AutoSize = true;
            tlpSecciones.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tlpSecciones.ColumnCount = 1;
            tlpSecciones.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpSecciones.Controls.Add(flpFiltros, 0, 1);
            tlpSecciones.Controls.Add(pnlVentas, 0, 2);
            tlpSecciones.Controls.Add(lblSeleccion, 0, 3);
            tlpSecciones.Controls.Add(flpCobro, 0, 4);
            tlpSecciones.Controls.Add(lblCobro, 0, 5);
            tlpSecciones.Controls.Add(pnlPagos, 0, 6);
            tlpSecciones.Dock = DockStyle.Top;
            tlpSecciones.Location = new Point(0, 0);
            tlpSecciones.MinimumSize = new Size(760, 0);
            tlpSecciones.Name = "tlpSecciones";
            tlpSecciones.Padding = new Padding(8);
            tlpSecciones.RowCount = 7;
            tlpSecciones.RowStyles.Add(new RowStyle());
            tlpSecciones.RowStyles.Add(new RowStyle());
            tlpSecciones.RowStyles.Add(new RowStyle());
            tlpSecciones.RowStyles.Add(new RowStyle());
            tlpSecciones.RowStyles.Add(new RowStyle());
            tlpSecciones.RowStyles.Add(new RowStyle());
            tlpSecciones.RowStyles.Add(new RowStyle());
            tlpSecciones.Size = new Size(1183, 728);
            tlpSecciones.TabIndex = 0;
            // frmpagos
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            BackColor = Color.FromArgb(13, 19, 27);
            ClientSize = new Size(1200, 820);
            Controls.Add(pnlPagina);
            Controls.Add(contenido);
            Controls.Add(pnlFiltros);
            Controls.Add(pnlTitulo);
            Controls.Add(txtCuentaDestino);
            Controls.Add(lblCuentaDestino);
            ForeColor = Color.White;
            Name = "frmPagos";
            Text = "Pagos";
            Load += frmPagos_Load;
            pnlTitulo.ResumeLayout(false);
            pnlTitulo.PerformLayout();
            contenido.ResumeLayout(false);
            pnlVentas.ResumeLayout(false);
            // llama a ((systemcomponentmodelisupportinitialize)dgvventas)endinit para realizar este paso del flujo
            ((System.ComponentModel.ISupportInitialize)dgvVentas).EndInit();
            // llama a ((systemcomponentmodelisupportinitialize)nudimporte)endinit para realizar este paso del flujo
            ((System.ComponentModel.ISupportInitialize)nudImporte).EndInit();
            pnlPagos.ResumeLayout(false);
            // llama a ((systemcomponentmodelisupportinitialize)dgvpagos)endinit para realizar este paso del flujo
            ((System.ComponentModel.ISupportInitialize)dgvPagos).EndInit();
            flpFiltros.ResumeLayout(false);
            flpFiltros.PerformLayout();
            flpCobro.ResumeLayout(false);
            pnlCampo27.ResumeLayout(false);
            pnlCampo28.ResumeLayout(false);
            pnlCampo29.ResumeLayout(false);
            pnlPagina.ResumeLayout(false);
            pnlPagina.PerformLayout();
            tlpSecciones.ResumeLayout(false);
            tlpSecciones.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        private Panel pnlTitulo, pnlFiltros, pnlVentas, pnlCobro, pnlPagos;
        private Label lblTitulo, lblVentas, lblSeleccion, lblImporte, lblMedio, lblCuotas, lblCobro, lblPagos;
        private TextBox txtBuscar;
        private CheckBox chkPendientes;
        private Button btnBuscar, btnRegistrar;
        private TableLayoutPanel contenido;
        private DataGridView dgvVentas, dgvPagos;
        private NumericUpDown nudImporte;
        private ComboBox cboMedio, cboCuotas;
        private TextBox txtCuentaDestino;
        private Label lblCuentaDestino;
        private FlowLayoutPanel flpFiltros;
        private FlowLayoutPanel flpCobro;
        private Panel pnlCampo27;
        private Panel pnlCampo28;
        private Panel pnlCampo29;
        private Panel pnlCampo30;
        private Panel pnlPagina;
        private TableLayoutPanel tlpSecciones;
        private DataGridViewTextBoxColumn colDiseno45;
        private DataGridViewTextBoxColumn colDiseno46;
        private DataGridViewTextBoxColumn colDiseno47;
        private DataGridViewTextBoxColumn colDiseno48;
        private DataGridViewTextBoxColumn colDiseno49;
        private DataGridViewTextBoxColumn colDiseno50;
        private DataGridViewTextBoxColumn colDiseno51;
        private DataGridViewTextBoxColumn colDiseno52;
        private DataGridViewTextBoxColumn colDiseno53;
        private DataGridViewTextBoxColumn colDiseno54;
        private DataGridViewTextBoxColumn colDiseno55;
        private DataGridViewTextBoxColumn colDiseno56;
    }
}
