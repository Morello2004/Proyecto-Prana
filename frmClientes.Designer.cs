namespace Prana.Presentacion
{
    // parte visual de frmclientes crea controles y define su distribucion, colores y eventos; visual studio puede regenerar este archivo al editar el disenador
    partial class frmClientes
    {
        // /// required designer variable
        private System.ComponentModel.IContainer components = null;
        // /// clean up any resources being used true if managed resources should be disposed; otherwise, false libera los recursos del formulario; disposing indica si tambien deben liberarse los componentes administrados
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        // /// required method for designer support - do not modify the contents of this method with the code editor construye la pantalla: instancia controles, asigna propiedades, agrega cada control a su contenedor y conecta los eventos no registra operaciones comerciales
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle7 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle10 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle8 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle9 = new DataGridViewCellStyle();
            esperaBusqueda = new System.Windows.Forms.Timer(components);
            colVerDeuda = new DataGridViewButtonColumn();
            colEstadoCuenta = new DataGridViewTextBoxColumn();
            colDeudaPendiente = new DataGridViewTextBoxColumn();
            pnlFiltroEstadoCliente = new Panel();
            cboFiltroEstadoCliente = new ComboBox();
            btnCambiarEstadoCliente = new Button();
            panel1 = new Panel();
            PnlBusquedaCliente = new Panel();
            btnBuscarClientes = new FontAwesome.Sharp.IconButton();
            cboBuscarCliente = new ComboBox();
            lblcodigo = new Label();
            label1 = new Label();
            panel2 = new Panel();
            btn_CrearCliente = new Button();
            btnAdministrarLocalidades = new Button();
            Localidad = new Label();
            cboLocalidadCliente = new ComboBox();
            label5 = new Label();
            txtDireccionCliente = new TextBox();
            dgv_listadoClientes = new DataGridView();
            colNombre = new DataGridViewTextBoxColumn();
            colApellido = new DataGridViewTextBoxColumn();
            colTipoDocumento = new DataGridViewComboBoxColumn();
            colDNI = new DataGridViewTextBoxColumn();
            ColTelefono = new DataGridViewTextBoxColumn();
            colDireccion = new DataGridViewTextBoxColumn();
            colLocalidad = new DataGridViewTextBoxColumn();
            colAccion = new DataGridViewButtonColumn();
            colFechaRegistro = new DataGridViewTextBoxColumn();
            colSaldoFavor = new DataGridViewTextBoxColumn();
            label4 = new Label();
            btnCancelar = new FontAwesome.Sharp.IconButton();
            txtDocumentoCliente = new TextBox();
            cboTipoDocumento = new ComboBox();
            txtTelefonoCliente = new TextBox();
            txtAreaCliente = new TextBox();
            lblNumeroCliente = new Label();
            label10 = new Label();
            label8 = new Label();
            txtApellidoCliente = new TextBox();
            label3 = new Label();
            lblVC = new Label();
            txtNombreCliente = new TextBox();
            label2 = new Label();
            pnlEncabezadoClientes = new Panel();
            LBLtituloClientes = new Label();
            pnlListadoDiseno = new Panel();
            pnlPagina = new Panel();
            tlpSecciones = new TableLayoutPanel();
            PnlBusquedaCliente.SuspendLayout();
            panel2.SuspendLayout();
            // llama a ((systemcomponentmodelisupportinitialize)dgv_listadoclientes)begininit para realizar este paso del flujo
            ((System.ComponentModel.ISupportInitialize)dgv_listadoClientes).BeginInit();
            pnlFiltroEstadoCliente.SuspendLayout();
            pnlEncabezadoClientes.SuspendLayout();
            pnlListadoDiseno.SuspendLayout();
            pnlPagina.SuspendLayout();
            tlpSecciones.SuspendLayout();
            SuspendLayout();
            // esperabusqueda esperabusquedainterval: tiempo entre activaciones del temporizador, en milisegundos
            esperaBusqueda.Interval = 250;
            esperaBusqueda.Tick += esperaBusqueda_Tick;
            // colverdeuda colverdeudaheadertext: titulo de la columna; no es el nombre de la propiedad enlazada
            colVerDeuda.HeaderText = "Pendientes";
            colVerDeuda.Name = "colVerDeuda";
            colVerDeuda.DefaultCellStyle.ForeColor = Color.Black;
            colVerDeuda.DefaultCellStyle.SelectionForeColor = Color.Black;
            colVerDeuda.DefaultCellStyle.BackColor = Color.Gainsboro;
            colVerDeuda.DefaultCellStyle.SelectionBackColor = Color.Gainsboro;
            colVerDeuda.ReadOnly = true;
            colVerDeuda.Text = "Ver detalle";
            colVerDeuda.UseColumnTextForButtonValue = true;
            // colestadocuenta colestadocuentadatapropertyname: propiedad del objeto enlazado cuyo valor muestra esta columna
            colEstadoCuenta.DataPropertyName = "EstadoCuenta";
            colEstadoCuenta.HeaderText = "Cuenta";
            colEstadoCuenta.Name = "colEstadoCuenta";
            colEstadoCuenta.ReadOnly = true;
            // coldeudapendiente coldeudapendientedatapropertyname: propiedad del objeto enlazado cuyo valor muestra esta columna
            colDeudaPendiente.DataPropertyName = "DeudaPendiente";
            colDeudaPendiente.HeaderText = "Deuda pendiente";
            colDeudaPendiente.Name = "colDeudaPendiente";
            colDeudaPendiente.ReadOnly = true;
            colDeudaPendiente.Width = 125;
            // panel1 panel1anchor: bordes que mantienen su distancia cuando cambia el tamano del contenedor
            panel1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            panel1.AutoScroll = true;
            panel1.AutoSize = true;
            panel1.BackColor = Color.FromArgb(13, 19, 27);
            panel1.Location = new Point(2, 1);
            panel1.Name = "panel1";
            panel1.Size = new Size(1460, 715);
            panel1.TabIndex = 0;
            panel1.Visible = false;
            // pnlbusquedacliente
            PnlBusquedaCliente.AutoSize = true;
            PnlBusquedaCliente.BackColor = Color.FromArgb(21, 30, 41);
            PnlBusquedaCliente.Controls.Add(btnBuscarClientes);
            PnlBusquedaCliente.Controls.Add(cboBuscarCliente);
            PnlBusquedaCliente.Controls.Add(lblcodigo);
            PnlBusquedaCliente.Controls.Add(label1);
            PnlBusquedaCliente.Dock = DockStyle.Top;
            PnlBusquedaCliente.Location = new Point(8, 58);
            PnlBusquedaCliente.Margin = new Padding(0, 0, 0, 8);
            PnlBusquedaCliente.Name = "PnlBusquedaCliente";
            PnlBusquedaCliente.Size = new Size(1427, 79);
            PnlBusquedaCliente.TabIndex = 1;
            // btnbuscarclientes btnbuscarclientesbackcolor: color del fondo del control o de la celda
            btnBuscarClientes.BackColor = Color.DodgerBlue;
            btnBuscarClientes.FlatAppearance.BorderSize = 0;
            btnBuscarClientes.FlatStyle = FlatStyle.Flat;
            btnBuscarClientes.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnBuscarClientes.IconChar = FontAwesome.Sharp.IconChar.None;
            btnBuscarClientes.IconColor = Color.Black;
            btnBuscarClientes.IconFont = FontAwesome.Sharp.IconFont.Auto;
            btnBuscarClientes.Location = new Point(399, 51);
            btnBuscarClientes.Name = "btnBuscarClientes";
            btnBuscarClientes.Size = new Size(100, 25);
            btnBuscarClientes.TabIndex = 6;
            btnBuscarClientes.Text = "Buscar";
            btnBuscarClientes.UseVisualStyleBackColor = false;
            btnBuscarClientes.Click += btnBuscarClientes_Click;
            // cbobuscarcliente cbobuscarclientefont: tipografia, tamano y estilo con los que se muestra el contenido
            cboBuscarCliente.Font = new Font("Segoe UI", 10F);
            cboBuscarCliente.Location = new Point(15, 51);
            cboBuscarCliente.MaxLength = 201;
            cboBuscarCliente.Name = "cboBuscarCliente";
            cboBuscarCliente.Size = new Size(378, 25);
            cboBuscarCliente.TabIndex = 5;
            cboBuscarCliente.TextUpdate += cboBuscarCliente_TextUpdate;
            cboBuscarCliente.SelectionChangeCommitted += cboBuscarCliente_SelectionChangeCommitted;
            cboBuscarCliente.Enter += cboBuscarCliente_Enter;
            cboBuscarCliente.KeyDown += Buscador_KeyDown;
            // lblcodigo
            lblcodigo.AutoSize = true;
            lblcodigo.ForeColor = Color.FromArgb(145, 170, 210);
            lblcodigo.Location = new Point(10, 33);
            lblcodigo.Name = "lblcodigo";
            lblcodigo.Size = new Size(42, 15);
            lblcodigo.TabIndex = 4;
            lblcodigo.Text = "Buscar";
            // label1
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            label1.ForeColor = Color.White;
            label1.Location = new Point(10, 0);
            label1.Name = "label1";
            label1.Size = new Size(85, 21);
            label1.TabIndex = 0;
            label1.Text = "Busqueda";
            // panel2 panel2backcolor: color del fondo del control o de la celda
            panel2.BackColor = Color.FromArgb(21, 30, 41);
            panel2.Controls.Add(btn_CrearCliente);
            panel2.Controls.Add(btnAdministrarLocalidades);
            panel2.Controls.Add(Localidad);
            panel2.Controls.Add(cboLocalidadCliente);
            panel2.Controls.Add(label5);
            panel2.Controls.Add(txtDireccionCliente);
            panel2.Controls.Add(btnCancelar);
            panel2.Controls.Add(txtDocumentoCliente);
            panel2.Controls.Add(cboTipoDocumento);
            panel2.Controls.Add(txtTelefonoCliente);
            panel2.Controls.Add(txtAreaCliente);
            panel2.Controls.Add(lblNumeroCliente);
            txtAreaCliente.Location = new Point(2, 107);
            txtAreaCliente.Size = new Size(55, 25);
            txtAreaCliente.Font = new Font("Segoe UI", 10F);
            txtAreaCliente.MaxLength = 4;
            txtAreaCliente.PlaceholderText = "2323";
            txtAreaCliente.Name = "txtAreaCliente";
            txtAreaCliente.TabIndex = 32;
            lblNumeroCliente.Location = new Point(65, 89);
            lblNumeroCliente.AutoSize = true;
            lblNumeroCliente.ForeColor = Color.FromArgb(145, 170, 210);
            lblNumeroCliente.Text = "Número";
            lblNumeroCliente.Name = "lblNumeroCliente";
            panel2.Controls.Add(label10);
            panel2.Controls.Add(label8);
            panel2.Controls.Add(txtApellidoCliente);
            panel2.Controls.Add(label3);
            panel2.Controls.Add(lblVC);
            panel2.Controls.Add(txtNombreCliente);
            panel2.Controls.Add(label2);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(8, 145);
            panel2.Margin = new Padding(0, 0, 0, 8);
            panel2.Name = "panel2";
            panel2.Size = new Size(1427, 270);
            panel2.TabIndex = 2;
            // btn_crearcliente btn_crearclientebackcolor: color del fondo del control o de la celda
            btn_CrearCliente.BackColor = Color.DodgerBlue;
            btn_CrearCliente.FlatAppearance.BorderSize = 0;
            btn_CrearCliente.FlatStyle = FlatStyle.Flat;
            btn_CrearCliente.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btn_CrearCliente.ForeColor = Color.White;
            btn_CrearCliente.Location = new Point(42, 220);
            btn_CrearCliente.Name = "btn_CrearCliente";
            btn_CrearCliente.Size = new Size(145, 34);
            btn_CrearCliente.TabIndex = 44;
            btn_CrearCliente.Text = "Crear cliente";
            btn_CrearCliente.UseVisualStyleBackColor = false;
            btn_CrearCliente.Click += btn_CrearCliente_Click;
            // localidad
            Localidad.AutoSize = true;
            Localidad.ForeColor = Color.FromArgb(145, 170, 210);
            Localidad.Location = new Point(203, 147);
            Localidad.Name = "Localidad";
            Localidad.Size = new Size(58, 15);
            Localidad.TabIndex = 43;
            Localidad.Text = "Localidad / ciudad (opcional)";
            // cb local editable, con administracion de opciones al lado
            cboLocalidadCliente.DropDownStyle = ComboBoxStyle.DropDown;
            cboLocalidadCliente.Font = new Font("Segoe UI", 10F);
            cboLocalidadCliente.Location = new Point(203, 165);
            cboLocalidadCliente.MaxLength = 25;
            cboLocalidadCliente.Name = "cboLocalidadCliente";
            cboLocalidadCliente.Size = new Size(187, 25);
            cboLocalidadCliente.TabIndex = 42;
            cboLocalidadCliente.TextChanged += cboLocalidadCliente_TextChanged;
            btnAdministrarLocalidades.BackColor = Color.Gainsboro;
            btnAdministrarLocalidades.FlatAppearance.BorderSize = 0;
            btnAdministrarLocalidades.FlatStyle = FlatStyle.Flat;
            btnAdministrarLocalidades.ForeColor = Color.Black;
            btnAdministrarLocalidades.Location = new Point(398, 164);
            btnAdministrarLocalidades.Name = "btnAdministrarLocalidades";
            btnAdministrarLocalidades.Size = new Size(155, 27);
            btnAdministrarLocalidades.TabIndex = 43;
            btnAdministrarLocalidades.Text = "Administrar localidades";
            btnAdministrarLocalidades.UseVisualStyleBackColor = false;
            btnAdministrarLocalidades.Click += btnAdministrarLocalidades_Click;
            // label5
            label5.AutoSize = true;
            label5.ForeColor = Color.FromArgb(145, 170, 210);
            label5.Location = new Point(2, 147);
            label5.Name = "label5";
            label5.Size = new Size(114, 15);
            label5.TabIndex = 41;
            label5.Text = "Dirección (opcional)";
            // txtdireccioncliente
            txtDireccionCliente.CharacterCasing = CharacterCasing.Upper;
            txtDireccionCliente.Font = new Font("Segoe UI", 10F);
            txtDireccionCliente.Location = new Point(2, 165);
            txtDireccionCliente.MaxLength = 200;
            txtDireccionCliente.Name = "txtDireccionCliente";
            txtDireccionCliente.PlaceholderText = "CALLE 123 O CALLE S/N";
            txtDireccionCliente.Size = new Size(168, 25);
            txtDireccionCliente.TabIndex = 40;
            // pnlfiltroestadocliente pnlfiltroestadoclientebackcolor: color del fondo del control o de la celda
            pnlFiltroEstadoCliente.BackColor = Color.FromArgb(21, 30, 41);
            pnlFiltroEstadoCliente.Controls.Add(btnCambiarEstadoCliente);
            pnlFiltroEstadoCliente.Controls.Add(cboFiltroEstadoCliente);
            pnlFiltroEstadoCliente.Dock = DockStyle.Top;
            pnlFiltroEstadoCliente.Location = new Point(0, 32);
            pnlFiltroEstadoCliente.Name = "pnlFiltroEstadoCliente";
            pnlFiltroEstadoCliente.Size = new Size(1427, 42);
            pnlFiltroEstadoCliente.TabIndex = 40;
            // cbofiltroestadocliente cbofiltroestadoclientedropdownstyle: establece si la lista permite escribir o solo elegir elementos
            cboFiltroEstadoCliente.DropDownStyle = ComboBoxStyle.DropDownList;
            cboFiltroEstadoCliente.Items.AddRange(new object[] { "Activos", "Inactivos", "Dados de baja", "Todos" });
            cboFiltroEstadoCliente.Location = new Point(8, 8);
            cboFiltroEstadoCliente.Name = "cboFiltroEstadoCliente";
            cboFiltroEstadoCliente.Size = new Size(150, 23);
            cboFiltroEstadoCliente.TabIndex = 0;
            cboFiltroEstadoCliente.SelectedIndex = 0;
            cboFiltroEstadoCliente.SelectedIndexChanged += cboFiltroEstadoCliente_SelectedIndexChanged;
            // btncambiarestadocliente btncambiarestadoclientebackcolor: color del fondo del control o de la celda
            btnCambiarEstadoCliente.BackColor = Color.DodgerBlue;
            btnCambiarEstadoCliente.FlatAppearance.BorderSize = 0;
            btnCambiarEstadoCliente.FlatStyle = FlatStyle.Flat;
            btnCambiarEstadoCliente.ForeColor = Color.White;
            btnCambiarEstadoCliente.Location = new Point(170, 7);
            btnCambiarEstadoCliente.Name = "btnCambiarEstadoCliente";
            btnCambiarEstadoCliente.Size = new Size(130, 26);
            btnCambiarEstadoCliente.TabIndex = 1;
            btnCambiarEstadoCliente.Text = "Dar de baja";
            btnCambiarEstadoCliente.UseVisualStyleBackColor = false;
            btnCambiarEstadoCliente.Click += btnCambiarEstadoCliente_Click;
            // dgv_listadoclientes
            dgv_listadoClientes.AllowUserToAddRows = false;
            dgv_listadoClientes.AllowUserToDeleteRows = false;
            dataGridViewCellStyle6.BackColor = Color.FromArgb(236, 242, 249);
            dgv_listadoClientes.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle6;
            dgv_listadoClientes.BackgroundColor = Color.FromArgb(13, 19, 27);
            dataGridViewCellStyle7.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = Color.FromArgb(37, 52, 70);
            dataGridViewCellStyle7.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dataGridViewCellStyle7.ForeColor = Color.White;
            dataGridViewCellStyle7.SelectionBackColor = Color.FromArgb(190, 220, 250);
            dataGridViewCellStyle7.SelectionForeColor = Color.Black;
            dataGridViewCellStyle7.SelectionBackColor = Color.FromArgb(190, 220, 250);
            dataGridViewCellStyle7.WrapMode = DataGridViewTriState.True;
            dgv_listadoClientes.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            dgv_listadoClientes.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgv_listadoClientes.EditingControlShowing += dgv_listadoClientes_EditingControlShowing;
            dgv_listadoClientes.Columns.AddRange(new DataGridViewColumn[] { colNombre, colApellido, colTipoDocumento, colDNI, ColTelefono, colDireccion, colLocalidad, colAccion, colFechaRegistro, colDeudaPendiente, colEstadoCuenta, colVerDeuda });
            dataGridViewCellStyle10.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle10.BackColor = Color.White;
            dataGridViewCellStyle10.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle10.ForeColor = Color.White;
            dataGridViewCellStyle10.Format = "c2";
            dataGridViewCellStyle10.SelectionBackColor = Color.FromArgb(190, 220, 250);
            dataGridViewCellStyle10.SelectionForeColor = Color.Black;
            dataGridViewCellStyle10.SelectionBackColor = Color.FromArgb(190, 220, 250);
            dataGridViewCellStyle10.WrapMode = DataGridViewTriState.False;
            dgv_listadoClientes.DefaultCellStyle = dataGridViewCellStyle10;
            dgv_listadoClientes.Dock = DockStyle.Fill;
            dgv_listadoClientes.EnableHeadersVisualStyles = false;
            dgv_listadoClientes.Location = new Point(0, 32);
            dgv_listadoClientes.Name = "dgv_listadoClientes";
            dgv_listadoClientes.ForeColor = Color.Black;
            dgv_listadoClientes.DefaultCellStyle.ForeColor = Color.Black;
            dgv_listadoClientes.RowsDefaultCellStyle.ForeColor = Color.Black;
            dgv_listadoClientes.AlternatingRowsDefaultCellStyle.ForeColor = Color.Black;
            dgv_listadoClientes.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.Black;
            dgv_listadoClientes.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(190, 220, 250);
            dgv_listadoClientes.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgv_listadoClientes.DefaultCellStyle.SelectionBackColor = Color.FromArgb(190, 220, 250);
            dgv_listadoClientes.AlternatingRowsDefaultCellStyle.SelectionForeColor = Color.Black;
            dgv_listadoClientes.AlternatingRowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(190, 220, 250);
            dgv_listadoClientes.RowTemplate.Height = 30;
            dgv_listadoClientes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv_listadoClientes.Size = new Size(1427, 258);
            dgv_listadoClientes.TabIndex = 39;
            dgv_listadoClientes.CellClick += dgv_listadoClientes_CellClick;
            dgv_listadoClientes.CellFormatting += dgv_listadoClientes_CellFormatting;
            dgv_listadoClientes.SelectionChanged += dgv_listadoClientes_SelectionChanged;
            // colnombre colnombredatapropertyname: propiedad del objeto enlazado cuyo valor muestra esta columna
            colNombre.DataPropertyName = "Nombre";
            colNombre.MaxInputLength = 100;
            colNombre.HeaderText = "Nombre";
            colNombre.Name = "colNombre";
            // colapellido colapellidodatapropertyname: propiedad del objeto enlazado cuyo valor muestra esta columna
            colApellido.DataPropertyName = "Apellido";
            colApellido.MaxInputLength = 100;
            colApellido.HeaderText = "Apellido";
            colApellido.Name = "colApellido";
            // coltipodocumento coltipodocumentodatapropertyname: propiedad del objeto enlazado cuyo valor muestra esta columna
            colTipoDocumento.DataPropertyName = "TipoDocumento";
            colTipoDocumento.HeaderText = "Tipo documento";
            colTipoDocumento.Items.AddRange(new object[] { "DNI", "CUIT" });
            colTipoDocumento.Name = "colTipoDocumento";
            colTipoDocumento.ReadOnly = true;
            // coldni coldnidatapropertyname: propiedad del objeto enlazado cuyo valor muestra esta columna
            colDNI.DataPropertyName = "DNI";
            colDNI.HeaderText = "Numero documento";
            colDNI.MaxInputLength = 11;
            colDNI.Name = "colDNI";
            // coltelefono coltelefonodatapropertyname: propiedad del objeto enlazado cuyo valor muestra esta columna
            ColTelefono.DataPropertyName = "Telefono";
            ColTelefono.HeaderText = "Telefono";
            ColTelefono.Name = "ColTelefono";
            ColTelefono.MaxInputLength = 10;
            ColTelefono.ToolTipText = "Área y número concatenados: 10 dígitos";
            // coldireccion coldirecciondatapropertyname: propiedad del objeto enlazado cuyo valor muestra esta columna
            colDireccion.DataPropertyName = "Direccion";
            colDireccion.HeaderText = "Direccion";
            colDireccion.Name = "colDireccion";
            colDireccion.MaxInputLength = 200;
            // collocalidad collocalidaddatapropertyname: propiedad del objeto enlazado cuyo valor muestra esta columna
            colLocalidad.DataPropertyName = "NombreLocalidad";
            colLocalidad.HeaderText = "Localidad";
            colLocalidad.Name = "colLocalidad";
            colLocalidad.MaxInputLength = 25;
            // colaccion
            dataGridViewCellStyle8.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle8.ForeColor = Color.FromArgb(21, 30, 41);
            dataGridViewCellStyle8.NullValue = "Modificar cliente";
            colAccion.DefaultCellStyle = dataGridViewCellStyle8;
            colAccion.HeaderText = "Acciones";
            colAccion.Name = "colAccion";
            colAccion.DefaultCellStyle.ForeColor = Color.Black;
            colAccion.DefaultCellStyle.SelectionForeColor = Color.Black;
            colAccion.DefaultCellStyle.BackColor = Color.Gainsboro;
            colAccion.DefaultCellStyle.SelectionBackColor = Color.Gainsboro;
            colAccion.Resizable = DataGridViewTriState.True;
            colAccion.SortMode = DataGridViewColumnSortMode.Automatic;
            colAccion.Text = "Modificar cliente";
            colAccion.Width = 145;
            // colfecharegistro colfecharegistrodatapropertyname: propiedad del objeto enlazado cuyo valor muestra esta columna
            colFechaRegistro.DataPropertyName = "FechaRegistro";
            dataGridViewCellStyle9.Format = "g";
            dataGridViewCellStyle9.NullValue = null;
            colFechaRegistro.DefaultCellStyle = dataGridViewCellStyle9;
            colFechaRegistro.HeaderText = "Fecha de registro";
            colFechaRegistro.Name = "colFechaRegistro";
            colFechaRegistro.ReadOnly = true;
            // colsaldofavor colsaldofavordatapropertyname: propiedad del objeto enlazado cuyo valor muestra esta columna
            colSaldoFavor.DataPropertyName = "SaldoFavor";
            colSaldoFavor.HeaderText = "Saldo a favor\n";
            colSaldoFavor.Name = "colSaldoFavor";
            colSaldoFavor.ReadOnly = true;
            colSaldoFavor.Resizable = DataGridViewTriState.False;
            // label4 label4dock: borde o zona del contenedor a la que se ajusta automaticamente el control
            label4.Dock = DockStyle.Top;
            label4.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            label4.ForeColor = Color.White;
            label4.Location = new Point(0, 0);
            label4.Name = "label4";
            label4.Size = new Size(1427, 32);
            label4.TabIndex = 38;
            label4.Text = "Listado de clientes";
            // btncancelar btncancelarbackcolor: color del fondo del control o de la celda
            btnCancelar.BackColor = Color.SteelBlue;
            btnCancelar.FlatAppearance.BorderSize = 0;
            btnCancelar.FlatStyle = FlatStyle.Flat;
            btnCancelar.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnCancelar.ForeColor = SystemColors.ButtonHighlight;
            btnCancelar.IconChar = FontAwesome.Sharp.IconChar.None;
            btnCancelar.IconColor = Color.Black;
            btnCancelar.IconFont = FontAwesome.Sharp.IconFont.Auto;
            btnCancelar.Location = new Point(236, 218);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(124, 25);
            btnCancelar.TabIndex = 37;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = false;
            // txtdocumentocliente txtdocumentoclientefont: tipografia, tamano y estilo con los que se muestra el contenido
            txtDocumentoCliente.Font = new Font("Segoe UI", 10F);
            txtDocumentoCliente.Location = new Point(203, 107);
            txtDocumentoCliente.MaxLength = 8;
            txtDocumentoCliente.Name = "txtDocumentoCliente";
            txtDocumentoCliente.Size = new Size(187, 25);
            txtDocumentoCliente.TabIndex = 33;
            // cbotipodocumento cbotipodocumentodropdownstyle: establece si la lista permite escribir o solo elegir elementos
            cboTipoDocumento.DropDownStyle = ComboBoxStyle.DropDownList;
            cboTipoDocumento.Items.AddRange(new object[] { "DNI", "CUIT" });
            cboTipoDocumento.Location = new Point(410, 107);
            cboTipoDocumento.Name = "cboTipoDocumento";
            cboTipoDocumento.Size = new Size(110, 23);
            cboTipoDocumento.TabIndex = 45;
            cboTipoDocumento.SelectedIndexChanged += cboTipoDocumento_SelectedIndexChanged;
            // txttelefonocliente txttelefonoclientefont: tipografia, tamano y estilo con los que se muestra el contenido
            txtTelefonoCliente.Font = new Font("Segoe UI", 10F);
            txtTelefonoCliente.Location = new Point(65, 107);
            txtTelefonoCliente.MaxLength = 8;
            txtTelefonoCliente.Name = "txtTelefonoCliente";
            txtTelefonoCliente.Size = new Size(105, 25);
            txtTelefonoCliente.TabIndex = 33;
            // label10
            label10.AutoSize = true;
            label10.ForeColor = Color.FromArgb(145, 170, 210);
            label10.Location = new Point(203, 89);
            label10.Name = "label10";
            label10.Size = new Size(168, 15);
            label10.TabIndex = 31;
            label10.Text = "Documento (tipo a la derecha)";
            label10.Click += label10_Click;
            // label8
            label8.AutoSize = true;
            label8.ForeColor = Color.FromArgb(145, 170, 210);
            label8.Location = new Point(203, 34);
            label8.Name = "label8";
            label8.Size = new Size(51, 15);
            label8.TabIndex = 29;
            label8.Text = "Apellido";
            // txtapellidocliente
            txtApellidoCliente.CharacterCasing = CharacterCasing.Upper;
            txtApellidoCliente.Font = new Font("Segoe UI", 10F);
            txtApellidoCliente.Location = new Point(203, 52);
            txtApellidoCliente.MaxLength = 100;
            txtApellidoCliente.Name = "txtApellidoCliente";
            txtApellidoCliente.Size = new Size(187, 25);
            txtApellidoCliente.TabIndex = 28;
            // label3
            label3.AutoSize = true;
            label3.ForeColor = Color.FromArgb(145, 170, 210);
            label3.Location = new Point(0, 89);
            label3.Name = "label3";
            label3.Size = new Size(52, 15);
            label3.TabIndex = 27;
            label3.Text = "Área";
            // lblvc
            lblVC.AutoSize = true;
            lblVC.ForeColor = Color.FromArgb(145, 170, 210);
            lblVC.Location = new Point(2, 34);
            lblVC.Name = "lblVC";
            lblVC.Size = new Size(51, 15);
            lblVC.TabIndex = 25;
            lblVC.Text = "Nombre";
            // txtnombrecliente
            txtNombreCliente.CharacterCasing = CharacterCasing.Upper;
            txtNombreCliente.Font = new Font("Segoe UI", 10F);
            txtNombreCliente.Location = new Point(2, 52);
            txtNombreCliente.MaxLength = 100;
            txtNombreCliente.Name = "txtNombreCliente";
            txtNombreCliente.Size = new Size(168, 25);
            txtNombreCliente.TabIndex = 24;
            // label2
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            label2.ForeColor = Color.White;
            label2.Location = new Point(-3, 0);
            label2.Name = "label2";
            label2.Size = new Size(106, 21);
            label2.TabIndex = 1;
            label2.Text = "Crear cliente";
            label2.Click += label2_Click;
            // pnlencabezadoclientes
            pnlEncabezadoClientes.AutoScroll = true;
            pnlEncabezadoClientes.AutoSize = true;
            pnlEncabezadoClientes.BackColor = Color.FromArgb(21, 30, 41);
            pnlEncabezadoClientes.Controls.Add(LBLtituloClientes);
            pnlEncabezadoClientes.Dock = DockStyle.Top;
            pnlEncabezadoClientes.Location = new Point(8, 8);
            pnlEncabezadoClientes.Margin = new Padding(0, 0, 0, 8);
            pnlEncabezadoClientes.Name = "pnlEncabezadoClientes";
            pnlEncabezadoClientes.Size = new Size(1427, 42);
            pnlEncabezadoClientes.TabIndex = 0;
            // lbltituloclientes
            LBLtituloClientes.AutoSize = true;
            LBLtituloClientes.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            LBLtituloClientes.ForeColor = Color.White;
            LBLtituloClientes.Location = new Point(15, 10);
            LBLtituloClientes.Name = "LBLtituloClientes";
            LBLtituloClientes.Size = new Size(104, 32);
            LBLtituloClientes.TabIndex = 0;
            LBLtituloClientes.Text = "Clientes";
            // pnllistadodiseno inserta el control en su contenedor; esto determina su jerarquia visual y posicion relativa
            pnlListadoDiseno.Controls.Add(dgv_listadoClientes);
            pnlListadoDiseno.Controls.Add(pnlFiltroEstadoCliente);
            pnlListadoDiseno.Controls.Add(label4);
            pnlListadoDiseno.Dock = DockStyle.Top;
            pnlListadoDiseno.Location = new Point(8, 423);
            pnlListadoDiseno.Margin = new Padding(0, 0, 0, 8);
            pnlListadoDiseno.Name = "pnlListadoDiseno";
            pnlListadoDiseno.Size = new Size(1427, 290);
            pnlListadoDiseno.TabIndex = 3;
            // pnlpagina
            pnlPagina.AutoScroll = true;
            pnlPagina.BackColor = Color.FromArgb(13, 19, 27);
            pnlPagina.Controls.Add(tlpSecciones);
            pnlPagina.Dock = DockStyle.Fill;
            pnlPagina.ForeColor = Color.White;
            pnlPagina.Location = new Point(0, 0);
            pnlPagina.Name = "pnlPagina";
            pnlPagina.Size = new Size(1460, 534);
            pnlPagina.TabIndex = 2;
            // tlpsecciones
            tlpSecciones.AutoSize = true;
            tlpSecciones.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tlpSecciones.ColumnCount = 1;
            tlpSecciones.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpSecciones.Controls.Add(pnlEncabezadoClientes, 0, 0);
            tlpSecciones.Controls.Add(PnlBusquedaCliente, 0, 1);
            tlpSecciones.Controls.Add(panel2, 0, 2);
            tlpSecciones.Controls.Add(pnlListadoDiseno, 0, 3);
            tlpSecciones.Dock = DockStyle.Top;
            tlpSecciones.Location = new Point(0, 0);
            tlpSecciones.MinimumSize = new Size(760, 0);
            tlpSecciones.Name = "tlpSecciones";
            tlpSecciones.Padding = new Padding(8);
            tlpSecciones.RowCount = 4;
            tlpSecciones.RowStyles.Add(new RowStyle());
            tlpSecciones.RowStyles.Add(new RowStyle());
            tlpSecciones.RowStyles.Add(new RowStyle());
            tlpSecciones.RowStyles.Add(new RowStyle());
            tlpSecciones.Size = new Size(1443, 729);
            tlpSecciones.TabIndex = 0;
            // frmclientes
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            BackColor = Color.FromArgb(13, 19, 27);
            ClientSize = new Size(1460, 534);
            Controls.Add(pnlPagina);
            ForeColor = Color.White;
            Name = "frmClientes";
            Text = "frmClientes";
            Load += frmClientes_Load;
            Disposed += frmClientes_Disposed;
            PnlBusquedaCliente.ResumeLayout(false);
            PnlBusquedaCliente.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            // llama a ((systemcomponentmodelisupportinitialize)dgv_listadoclientes)endinit para realizar este paso del flujo
            ((System.ComponentModel.ISupportInitialize)dgv_listadoClientes).EndInit();
            pnlFiltroEstadoCliente.ResumeLayout(false);
            pnlEncabezadoClientes.ResumeLayout(false);
            pnlEncabezadoClientes.PerformLayout();
            pnlListadoDiseno.ResumeLayout(false);
            pnlPagina.ResumeLayout(false);
            pnlPagina.PerformLayout();
            tlpSecciones.ResumeLayout(false);
            tlpSecciones.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private Panel panel1;
        private Panel pnlEncabezadoClientes;
        private Label LBLtituloClientes;
        private Panel PnlBusquedaCliente;
        private Label label1;
        private Label lblcodigo;
        private ComboBox cboBuscarCliente;
        private Panel panel2;
        private Label label2;
        private FontAwesome.Sharp.IconButton btnBuscarClientes;
        private TextBox txtTelefonoCliente;
        private TextBox txtAreaCliente;
        private Label lblNumeroCliente;
        private Label label10;
        private Label label8;
        private TextBox txtApellidoCliente;
        private Label label3;
        private Label lblVC;
        private TextBox txtNombreCliente;
        private TextBox txtDocumentoCliente;
        private ComboBox cboTipoDocumento;
        private DataGridViewComboBoxColumn colTipoDocumento;
        private FontAwesome.Sharp.IconButton btnCancelar;
        private DataGridView dgv_listadoClientes;
        private Label label4;
        private Label label5;
        private TextBox txtDireccionCliente;
        private Label Localidad;
        private ComboBox cboLocalidadCliente;
        private Button btnAdministrarLocalidades;
        private Button btn_CrearCliente;
        private DataGridViewTextBoxColumn colNombre;
        private DataGridViewTextBoxColumn colApellido;
        private DataGridViewTextBoxColumn colDNI;
        private DataGridViewTextBoxColumn ColTelefono;
        private DataGridViewTextBoxColumn colDireccion;
        private DataGridViewTextBoxColumn colLocalidad;
        private DataGridViewButtonColumn colAccion;
        private DataGridViewTextBoxColumn colFechaRegistro;
        private DataGridViewTextBoxColumn colSaldoFavor;
        private DataGridViewTextBoxColumn colDeudaPendiente;
        private DataGridViewTextBoxColumn colEstadoCuenta;
        private DataGridViewButtonColumn colVerDeuda;
        private System.Windows.Forms.Timer esperaBusqueda;
        private Panel pnlFiltroEstadoCliente;
        private ComboBox cboFiltroEstadoCliente;
        private Button btnCambiarEstadoCliente;
        private Panel pnlListadoDiseno;
        private Panel pnlPagina;
        private TableLayoutPanel tlpSecciones;
    }
}
