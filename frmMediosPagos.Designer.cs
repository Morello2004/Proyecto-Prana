namespace Prana.Presentacion
{
    // parte visual de frmmediospagos crea controles y define su distribucion, colores y eventos; visual studio puede regenerar este archivo al editar el disenador
    partial class frmMediosPagos
    {
        private System.ComponentModel.IContainer components = null;
        // libera los recursos del formulario; disposing indica si tambien deben liberarse los componentes administrados
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        // construye la pantalla: instancia controles, asigna propiedades, agrega cada control a su contenedor y conecta los eventos no registra operaciones comerciales
        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            pnlListado = new Panel();
            dgvMediosPagos = new DataGridView();
            lblListado = new Label();
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
            flpCampos = new FlowLayoutPanel();
            pnlCampo1 = new Panel();
            pnlCampo2 = new Panel();
            pnlCampo3 = new Panel();
            pnlCampo4 = new Panel();
            pnlCampo5 = new Panel();
            pnlCampo6 = new Panel();
            pnlCampo7 = new Panel();
            flpAcciones = new FlowLayoutPanel();
            pnlPagina = new Panel();
            tlpSecciones = new TableLayoutPanel();
            pnlListado.SuspendLayout();
            // llama a ((systemcomponentmodelisupportinitialize)dgvmediospagos)begininit para realizar este paso del flujo
            ((System.ComponentModel.ISupportInitialize)dgvMediosPagos).BeginInit();
            pnlEncabezado.SuspendLayout();
            // llama a ((systemcomponentmodelisupportinitialize)nudrecargogeneral)begininit para realizar este paso del flujo
            ((System.ComponentModel.ISupportInitialize)nudRecargoGeneral).BeginInit();
            // llama a ((systemcomponentmodelisupportinitialize)nudrecargo2)begininit para realizar este paso del flujo
            ((System.ComponentModel.ISupportInitialize)nudRecargo2).BeginInit();
            // llama a ((systemcomponentmodelisupportinitialize)nudrecargo3)begininit para realizar este paso del flujo
            ((System.ComponentModel.ISupportInitialize)nudRecargo3).BeginInit();
            // llama a ((systemcomponentmodelisupportinitialize)nudrecargo6)begininit para realizar este paso del flujo
            ((System.ComponentModel.ISupportInitialize)nudRecargo6).BeginInit();
            // llama a ((systemcomponentmodelisupportinitialize)nudrecargo9)begininit para realizar este paso del flujo
            ((System.ComponentModel.ISupportInitialize)nudRecargo9).BeginInit();
            // llama a ((systemcomponentmodelisupportinitialize)nudrecargo12)begininit para realizar este paso del flujo
            ((System.ComponentModel.ISupportInitialize)nudRecargo12).BeginInit();
            flpCampos.SuspendLayout();
            pnlCampo1.SuspendLayout();
            pnlCampo2.SuspendLayout();
            pnlCampo3.SuspendLayout();
            pnlCampo4.SuspendLayout();
            pnlCampo5.SuspendLayout();
            pnlCampo6.SuspendLayout();
            pnlCampo7.SuspendLayout();
            flpAcciones.SuspendLayout();
            pnlPagina.SuspendLayout();
            tlpSecciones.SuspendLayout();
            SuspendLayout();
            // lbltipocaja lbltipocajadock: borde o zona del contenedor a la que se ajusta automaticamente el control
            // cbotipocaja cbotipocajaanchor: bordes que mantienen su distancia cuando cambia el tamano del contenedor
            // pnllistado inserta el control en su contenedor; esto determina su jerarquia visual y posicion relativa
            pnlListado.Controls.Add(dgvMediosPagos);
            pnlListado.Controls.Add(lblListado);
            pnlListado.Dock = DockStyle.Top;
            pnlListado.Location = new Point(0, 76);
            pnlListado.Margin = new Padding(0, 0, 0, 8);
            pnlListado.Name = "pnlListado";
            pnlListado.Padding = new Padding(16);
            pnlListado.Size = new Size(1100, 300);
            pnlListado.TabIndex = 4;
            // dgvmediospagos
            dgvMediosPagos.AllowUserToAddRows = false;
            dgvMediosPagos.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(236, 242, 249);
            dgvMediosPagos.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dgvMediosPagos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvMediosPagos.BackgroundColor = Color.FromArgb(13, 19, 27);
            dgvMediosPagos.BorderStyle = BorderStyle.None;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(37, 52, 70);
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = Color.White;
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(190, 220, 250);
            dataGridViewCellStyle2.SelectionForeColor = Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(190, 220, 250);
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dgvMediosPagos.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dgvMediosPagos.ColumnHeadersHeight = 38;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.White;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(21, 30, 41);
            dataGridViewCellStyle3.Padding = new Padding(5);
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(190, 220, 250);
            dataGridViewCellStyle3.SelectionForeColor = Color.Black;
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(190, 220, 250);
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            dgvMediosPagos.DefaultCellStyle = dataGridViewCellStyle3;
            dgvMediosPagos.Dock = DockStyle.Fill;
            dgvMediosPagos.EnableHeadersVisualStyles = false;
            dgvMediosPagos.Location = new Point(16, 56);
            dgvMediosPagos.MultiSelect = false;
            dgvMediosPagos.Name = "dgvMediosPagos";
            dgvMediosPagos.ForeColor = Color.Black;
            dgvMediosPagos.DefaultCellStyle.ForeColor = Color.Black;
            dgvMediosPagos.RowsDefaultCellStyle.ForeColor = Color.Black;
            dgvMediosPagos.AlternatingRowsDefaultCellStyle.ForeColor = Color.Black;
            dgvMediosPagos.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.Black;
            dgvMediosPagos.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(190, 220, 250);
            dgvMediosPagos.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvMediosPagos.DefaultCellStyle.SelectionBackColor = Color.FromArgb(190, 220, 250);
            dgvMediosPagos.AlternatingRowsDefaultCellStyle.SelectionForeColor = Color.Black;
            dgvMediosPagos.AlternatingRowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(190, 220, 250);
            dgvMediosPagos.ReadOnly = true;
            dgvMediosPagos.RowHeadersVisible = false;
            dgvMediosPagos.RowTemplate.Height = 32;
            dgvMediosPagos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvMediosPagos.Size = new Size(1068, 228);
            dgvMediosPagos.TabIndex = 9;
            dgvMediosPagos.CellClick += dgvMediosPagos_CellClick;
            // lbllistado lbllistadodock: borde o zona del contenedor a la que se ajusta automaticamente el control
            lblListado.Dock = DockStyle.Top;
            lblListado.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblListado.ForeColor = Color.White;
            lblListado.Location = new Point(16, 16);
            lblListado.Name = "lblListado";
            lblListado.Size = new Size(1068, 40);
            lblListado.TabIndex = 10;
            lblListado.Text = "Medios de pago activos. Seleccioná una fila para editarla.";
            // pnlencabezado pnlencabezadobackcolor: color del fondo del control o de la celda
            pnlEncabezado.BackColor = Color.FromArgb(21, 30, 41);
            pnlEncabezado.Controls.Add(lblTitulo);
            pnlEncabezado.Controls.Add(lblDescripcion);
            pnlEncabezado.Dock = DockStyle.Top;
            pnlEncabezado.Location = new Point(0, 0);
            pnlEncabezado.Margin = new Padding(0, 0, 0, 8);
            pnlEncabezado.Name = "pnlEncabezado";
            pnlEncabezado.Size = new Size(1100, 76);
            pnlEncabezado.TabIndex = 0;
            // lbltitulo
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.White;
            lblTitulo.Location = new Point(20, 8);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(198, 32);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Medios de pago";
            // lbldescripcion
            lblDescripcion.AutoSize = true;
            lblDescripcion.ForeColor = Color.FromArgb(145, 170, 210);
            lblDescripcion.Location = new Point(22, 45);
            lblDescripcion.Name = "lblDescripcion";
            lblDescripcion.Size = new Size(293, 15);
            lblDescripcion.TabIndex = 1;
            lblDescripcion.Text = "Editá los recargos de los medios de pago predefinidos.";
            // pnlformulario pnlformulariobackcolor: color del fondo del control o de la celda
            pnlFormulario.BackColor = Color.FromArgb(21, 30, 41);
            pnlFormulario.Dock = DockStyle.Top;
            pnlFormulario.Location = new Point(0, 68);
            pnlFormulario.Name = "pnlFormulario";
            pnlFormulario.Size = new Size(1100, 182);
            pnlFormulario.TabIndex = 0;
            pnlFormulario.Visible = false;
            // lblseccion lblsecciondock: borde o zona del contenedor a la que se ajusta automaticamente el control
            lblSeccion.Dock = DockStyle.Top;
            lblSeccion.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblSeccion.ForeColor = Color.White;
            lblSeccion.Location = new Point(8, 8);
            lblSeccion.Margin = new Padding(0, 0, 0, 8);
            lblSeccion.Name = "lblSeccion";
            lblSeccion.Size = new Size(1067, 28);
            lblSeccion.TabIndex = 1;
            lblSeccion.Text = "Selecciona un medio de pago para editar sus recargos";
            // lblnombre lblnombredock: borde o zona del contenedor a la que se ajusta automaticamente el control
            lblNombre.Dock = DockStyle.Top;
            lblNombre.ForeColor = Color.FromArgb(145, 170, 210);
            lblNombre.Location = new Point(0, 0);
            lblNombre.Name = "lblNombre";
            lblNombre.Size = new Size(190, 23);
            lblNombre.TabIndex = 2;
            lblNombre.Text = "Nombre del medio";
            // txtnombremediopago txtnombremediopagoanchor: bordes que mantienen su distancia cuando cambia el tamano del contenedor
            txtNombreMedioPago.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtNombreMedioPago.Font = new Font("Segoe UI", 10F);
            txtNombreMedioPago.Location = new Point(0, 26);
            txtNombreMedioPago.MaxLength = 20;
            txtNombreMedioPago.Name = "txtNombreMedioPago";
            txtNombreMedioPago.Size = new Size(190, 25);
            txtNombreMedioPago.TabIndex = 0;
            // lblgeneral lblgeneraldock: borde o zona del contenedor a la que se ajusta automaticamente el control
            lblGeneral.Dock = DockStyle.Top;
            lblGeneral.ForeColor = Color.FromArgb(145, 170, 210);
            lblGeneral.Location = new Point(0, 0);
            lblGeneral.Name = "lblGeneral";
            lblGeneral.Size = new Size(130, 23);
            lblGeneral.TabIndex = 3;
            lblGeneral.Text = "Recargo general (%)";
            // nudrecargogeneral nudrecargogeneralanchor: bordes que mantienen su distancia cuando cambia el tamano del contenedor
            nudRecargoGeneral.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            nudRecargoGeneral.DecimalPlaces = 2;
            nudRecargoGeneral.Font = new Font("Segoe UI", 10F);
            nudRecargoGeneral.Location = new Point(0, 26);
            nudRecargoGeneral.Maximum = new decimal(new int[] { 200, 0, 0, 0 });
            nudRecargoGeneral.Name = "nudRecargoGeneral";
            nudRecargoGeneral.Size = new Size(130, 25);
            nudRecargoGeneral.TabIndex = 1;
            // lblrecargo2 lblrecargo2dock: borde o zona del contenedor a la que se ajusta automaticamente el control
            lblRecargo2.Dock = DockStyle.Top;
            lblRecargo2.ForeColor = Color.FromArgb(145, 170, 210);
            lblRecargo2.Location = new Point(0, 0);
            lblRecargo2.Name = "lblRecargo2";
            lblRecargo2.Size = new Size(130, 23);
            lblRecargo2.TabIndex = 4;
            lblRecargo2.Text = "Tarjeta 2 cuotas (%)";
            // nudrecargo2 nudrecargo2anchor: bordes que mantienen su distancia cuando cambia el tamano del contenedor
            nudRecargo2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            nudRecargo2.DecimalPlaces = 2;
            nudRecargo2.Font = new Font("Segoe UI", 10F);
            nudRecargo2.Location = new Point(0, 26);
            nudRecargo2.Maximum = new decimal(new int[] { 200, 0, 0, 0 });
            nudRecargo2.Name = "nudRecargo2";
            nudRecargo2.Size = new Size(130, 25);
            nudRecargo2.TabIndex = 5;
            // lblrecargo3 lblrecargo3dock: borde o zona del contenedor a la que se ajusta automaticamente el control
            lblRecargo3.Dock = DockStyle.Top;
            lblRecargo3.ForeColor = Color.FromArgb(145, 170, 210);
            lblRecargo3.Location = new Point(0, 0);
            lblRecargo3.Name = "lblRecargo3";
            lblRecargo3.Size = new Size(130, 23);
            lblRecargo3.TabIndex = 6;
            lblRecargo3.Text = "Tarjeta 3 cuotas (%)";
            // nudrecargo3 nudrecargo3anchor: bordes que mantienen su distancia cuando cambia el tamano del contenedor
            nudRecargo3.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            nudRecargo3.DecimalPlaces = 2;
            nudRecargo3.Font = new Font("Segoe UI", 10F);
            nudRecargo3.Location = new Point(0, 26);
            nudRecargo3.Maximum = new decimal(new int[] { 200, 0, 0, 0 });
            nudRecargo3.Name = "nudRecargo3";
            nudRecargo3.Size = new Size(130, 25);
            nudRecargo3.TabIndex = 2;
            // lblrecargo6 lblrecargo6dock: borde o zona del contenedor a la que se ajusta automaticamente el control
            lblRecargo6.Dock = DockStyle.Top;
            lblRecargo6.ForeColor = Color.FromArgb(145, 170, 210);
            lblRecargo6.Location = new Point(0, 0);
            lblRecargo6.Name = "lblRecargo6";
            lblRecargo6.Size = new Size(130, 23);
            lblRecargo6.TabIndex = 7;
            lblRecargo6.Text = "Tarjeta 6 cuotas (%)";
            // nudrecargo6 nudrecargo6anchor: bordes que mantienen su distancia cuando cambia el tamano del contenedor
            nudRecargo6.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            nudRecargo6.DecimalPlaces = 2;
            nudRecargo6.Font = new Font("Segoe UI", 10F);
            nudRecargo6.Location = new Point(0, 26);
            nudRecargo6.Maximum = new decimal(new int[] { 200, 0, 0, 0 });
            nudRecargo6.Name = "nudRecargo6";
            nudRecargo6.Size = new Size(130, 25);
            nudRecargo6.TabIndex = 3;
            // lblrecargo9 lblrecargo9dock: borde o zona del contenedor a la que se ajusta automaticamente el control
            lblRecargo9.Dock = DockStyle.Top;
            lblRecargo9.ForeColor = Color.FromArgb(145, 170, 210);
            lblRecargo9.Location = new Point(0, 0);
            lblRecargo9.Name = "lblRecargo9";
            lblRecargo9.Size = new Size(130, 23);
            lblRecargo9.TabIndex = 8;
            lblRecargo9.Text = "Tarjeta 9 cuotas (%)";
            // nudrecargo9 nudrecargo9anchor: bordes que mantienen su distancia cuando cambia el tamano del contenedor
            nudRecargo9.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            nudRecargo9.DecimalPlaces = 2;
            nudRecargo9.Font = new Font("Segoe UI", 10F);
            nudRecargo9.Location = new Point(0, 26);
            nudRecargo9.Maximum = new decimal(new int[] { 200, 0, 0, 0 });
            nudRecargo9.Name = "nudRecargo9";
            nudRecargo9.Size = new Size(130, 25);
            nudRecargo9.TabIndex = 4;
            // lblrecargo12 lblrecargo12dock: borde o zona del contenedor a la que se ajusta automaticamente el control
            lblRecargo12.Dock = DockStyle.Top;
            lblRecargo12.ForeColor = Color.FromArgb(145, 170, 210);
            lblRecargo12.Location = new Point(0, 0);
            lblRecargo12.Name = "lblRecargo12";
            lblRecargo12.Size = new Size(130, 23);
            lblRecargo12.TabIndex = 9;
            lblRecargo12.Text = "Tarjeta 12 cuotas (%)";
            // nudrecargo12 nudrecargo12anchor: bordes que mantienen su distancia cuando cambia el tamano del contenedor
            nudRecargo12.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            nudRecargo12.DecimalPlaces = 2;
            nudRecargo12.Font = new Font("Segoe UI", 10F);
            nudRecargo12.Location = new Point(0, 26);
            nudRecargo12.Maximum = new decimal(new int[] { 200, 0, 0, 0 });
            nudRecargo12.Name = "nudRecargo12";
            nudRecargo12.Size = new Size(130, 25);
            nudRecargo12.TabIndex = 5;
            // btnguardar btnguardarbackcolor: color del fondo del control o de la celda
            btnGuardar.BackColor = Color.DodgerBlue;
            btnGuardar.FlatAppearance.BorderSize = 0;
            btnGuardar.FlatStyle = FlatStyle.Flat;
            btnGuardar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnGuardar.ForeColor = Color.White;
            btnGuardar.Location = new Point(12, 12);
            btnGuardar.Margin = new Padding(4, 4, 12, 4);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(110, 34);
            btnGuardar.TabIndex = 6;
            btnGuardar.Text = "Guardar";
            btnGuardar.UseVisualStyleBackColor = false;
            btnGuardar.Click += btnGuardar_Click;
            // flpcampos
            flpCampos.AutoSize = true;
            flpCampos.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flpCampos.Controls.Add(pnlCampo1);
            flpCampos.Controls.Add(pnlCampo2);
            flpCampos.Controls.Add(pnlCampo3);
            flpCampos.Controls.Add(pnlCampo4);
            flpCampos.Controls.Add(pnlCampo5);
            flpCampos.Controls.Add(pnlCampo6);
            flpCampos.Controls.Add(pnlCampo7);
            flpCampos.Dock = DockStyle.Top;
            flpCampos.Location = new Point(8, 44);
            flpCampos.Margin = new Padding(0, 0, 0, 8);
            flpCampos.Name = "flpCampos";
            flpCampos.Padding = new Padding(8);
            flpCampos.Size = new Size(1067, 148);
            flpCampos.TabIndex = 2;
            // pnlcampo1 inserta el control en su contenedor; esto determina su jerarquia visual y posicion relativa
            pnlCampo1.Controls.Add(txtNombreMedioPago);
            pnlCampo1.Controls.Add(lblNombre);
            pnlCampo1.Location = new Point(12, 12);
            pnlCampo1.Margin = new Padding(4, 4, 12, 4);
            pnlCampo1.Name = "pnlCampo1";
            pnlCampo1.Size = new Size(190, 58);
            pnlCampo1.TabIndex = 0;
            // pnlcampo2 inserta el control en su contenedor; esto determina su jerarquia visual y posicion relativa
            pnlCampo2.Controls.Add(nudRecargoGeneral);
            pnlCampo2.Controls.Add(lblGeneral);
            pnlCampo2.Location = new Point(218, 12);
            pnlCampo2.Margin = new Padding(4, 4, 12, 4);
            pnlCampo2.Name = "pnlCampo2";
            pnlCampo2.Size = new Size(130, 58);
            pnlCampo2.TabIndex = 1;
            // pnlcampo3 inserta el control en su contenedor; esto determina su jerarquia visual y posicion relativa
            pnlCampo3.Controls.Add(nudRecargo2);
            pnlCampo3.Controls.Add(lblRecargo2);
            pnlCampo3.Location = new Point(364, 12);
            pnlCampo3.Margin = new Padding(4, 4, 12, 4);
            pnlCampo3.Name = "pnlCampo3";
            pnlCampo3.Size = new Size(130, 58);
            pnlCampo3.TabIndex = 2;
            // pnlcampo4 inserta el control en su contenedor; esto determina su jerarquia visual y posicion relativa
            pnlCampo4.Controls.Add(nudRecargo3);
            pnlCampo4.Controls.Add(lblRecargo3);
            pnlCampo4.Location = new Point(510, 12);
            pnlCampo4.Margin = new Padding(4, 4, 12, 4);
            pnlCampo4.Name = "pnlCampo4";
            pnlCampo4.Size = new Size(130, 58);
            pnlCampo4.TabIndex = 3;
            // pnlcampo5 inserta el control en su contenedor; esto determina su jerarquia visual y posicion relativa
            pnlCampo5.Controls.Add(nudRecargo6);
            pnlCampo5.Controls.Add(lblRecargo6);
            pnlCampo5.Location = new Point(656, 12);
            pnlCampo5.Margin = new Padding(4, 4, 12, 4);
            pnlCampo5.Name = "pnlCampo5";
            pnlCampo5.Size = new Size(130, 58);
            pnlCampo5.TabIndex = 4;
            // pnlcampo6 inserta el control en su contenedor; esto determina su jerarquia visual y posicion relativa
            pnlCampo6.Controls.Add(nudRecargo9);
            pnlCampo6.Controls.Add(lblRecargo9);
            pnlCampo6.Location = new Point(802, 12);
            pnlCampo6.Margin = new Padding(4, 4, 12, 4);
            pnlCampo6.Name = "pnlCampo6";
            pnlCampo6.Size = new Size(130, 58);
            pnlCampo6.TabIndex = 5;
            // pnlcampo7 inserta el control en su contenedor; esto determina su jerarquia visual y posicion relativa
            pnlCampo7.Controls.Add(nudRecargo12);
            pnlCampo7.Controls.Add(lblRecargo12);
            pnlCampo7.Location = new Point(12, 78);
            pnlCampo7.Margin = new Padding(4, 4, 12, 4);
            pnlCampo7.Name = "pnlCampo7";
            pnlCampo7.Size = new Size(130, 58);
            pnlCampo7.TabIndex = 6;
            // pnlcampo8 inserta el control en su contenedor; esto determina su jerarquia visual y posicion relativa
            // flpacciones
            flpAcciones.AutoSize = true;
            flpAcciones.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flpAcciones.Controls.Add(btnGuardar);
            flpAcciones.Dock = DockStyle.Top;
            flpAcciones.Location = new Point(8, 200);
            flpAcciones.Margin = new Padding(0, 0, 0, 8);
            flpAcciones.Name = "flpAcciones";
            flpAcciones.Padding = new Padding(8);
            flpAcciones.Size = new Size(1067, 58);
            flpAcciones.TabIndex = 3;
            // pnlpagina
            pnlPagina.AutoScroll = true;
            pnlPagina.BackColor = Color.FromArgb(13, 19, 27);
            pnlPagina.Controls.Add(tlpSecciones);
            pnlPagina.Dock = DockStyle.Fill;
            pnlPagina.ForeColor = Color.White;
            pnlPagina.Location = new Point(0, 376);
            pnlPagina.Name = "pnlPagina";
            pnlPagina.Size = new Size(1100, 224);
            pnlPagina.TabIndex = 5;
            // tlpsecciones
            tlpSecciones.AutoSize = true;
            tlpSecciones.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tlpSecciones.ColumnCount = 1;
            tlpSecciones.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpSecciones.Controls.Add(lblSeccion, 0, 1);
            tlpSecciones.Controls.Add(flpCampos, 0, 2);
            tlpSecciones.Controls.Add(flpAcciones, 0, 3);
            tlpSecciones.Dock = DockStyle.Top;
            tlpSecciones.Location = new Point(0, 0);
            tlpSecciones.MinimumSize = new Size(760, 0);
            tlpSecciones.Name = "tlpSecciones";
            tlpSecciones.Padding = new Padding(8);
            tlpSecciones.RowCount = 5;
            tlpSecciones.RowStyles.Add(new RowStyle());
            tlpSecciones.RowStyles.Add(new RowStyle());
            tlpSecciones.RowStyles.Add(new RowStyle());
            tlpSecciones.RowStyles.Add(new RowStyle());
            tlpSecciones.RowStyles.Add(new RowStyle());
            tlpSecciones.Size = new Size(1083, 274);
            tlpSecciones.TabIndex = 0;
            // frmmediospagos
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            BackColor = Color.FromArgb(13, 19, 27);
            ClientSize = new Size(1100, 600);
            Controls.Add(pnlPagina);
            Controls.Add(pnlListado);
            Controls.Add(pnlEncabezado);
            ForeColor = Color.White;
            Name = "frmMediosPagos";
            Text = "Medios de pago";
            Load += frmMediosPagos_Load;
            pnlListado.ResumeLayout(false);
            // llama a ((systemcomponentmodelisupportinitialize)dgvmediospagos)endinit para realizar este paso del flujo
            ((System.ComponentModel.ISupportInitialize)dgvMediosPagos).EndInit();
            pnlEncabezado.ResumeLayout(false);
            pnlEncabezado.PerformLayout();
            // llama a ((systemcomponentmodelisupportinitialize)nudrecargogeneral)endinit para realizar este paso del flujo
            ((System.ComponentModel.ISupportInitialize)nudRecargoGeneral).EndInit();
            // llama a ((systemcomponentmodelisupportinitialize)nudrecargo2)endinit para realizar este paso del flujo
            ((System.ComponentModel.ISupportInitialize)nudRecargo2).EndInit();
            // llama a ((systemcomponentmodelisupportinitialize)nudrecargo3)endinit para realizar este paso del flujo
            ((System.ComponentModel.ISupportInitialize)nudRecargo3).EndInit();
            // llama a ((systemcomponentmodelisupportinitialize)nudrecargo6)endinit para realizar este paso del flujo
            ((System.ComponentModel.ISupportInitialize)nudRecargo6).EndInit();
            // llama a ((systemcomponentmodelisupportinitialize)nudrecargo9)endinit para realizar este paso del flujo
            ((System.ComponentModel.ISupportInitialize)nudRecargo9).EndInit();
            // llama a ((systemcomponentmodelisupportinitialize)nudrecargo12)endinit para realizar este paso del flujo
            ((System.ComponentModel.ISupportInitialize)nudRecargo12).EndInit();
            flpCampos.ResumeLayout(false);
            pnlCampo1.ResumeLayout(false);
            pnlCampo1.PerformLayout();
            pnlCampo2.ResumeLayout(false);
            pnlCampo3.ResumeLayout(false);
            pnlCampo4.ResumeLayout(false);
            pnlCampo5.ResumeLayout(false);
            pnlCampo6.ResumeLayout(false);
            pnlCampo7.ResumeLayout(false);
            flpAcciones.ResumeLayout(false);
            pnlPagina.ResumeLayout(false);
            pnlPagina.PerformLayout();
            tlpSecciones.ResumeLayout(false);
            tlpSecciones.PerformLayout();
            ResumeLayout(false);
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
        private FlowLayoutPanel flpCampos;
        private Panel pnlCampo1;
        private Panel pnlCampo2;
        private Panel pnlCampo3;
        private Panel pnlCampo4;
        private Panel pnlCampo5;
        private Panel pnlCampo6;
        private Panel pnlCampo7;
        private FlowLayoutPanel flpAcciones;
        private Panel pnlPagina;
        private TableLayoutPanel tlpSecciones;
    }
}
