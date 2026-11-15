namespace Prana.Presentacion
{
    // parte visual de frmnuevaventa crea controles y define su distribucion, colores y eventos; visual studio puede regenerar este archivo al editar el disenador
    partial class frmNuevaVenta
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
            DataGridViewCellStyle dataGridViewCellStyle19 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle20 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle21 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle22 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle23 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle24 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle25 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle26 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle27 = new DataGridViewCellStyle();
            lblCobroPrevisto = new Label();
            usarDireccionRegistrada = new CheckBox();
            txtCuentaDestino = new TextBox();
            lblCuentaDestino = new Label();
            cboCliente = new ComboBox();
            cboBuscarProducto = new ComboBox();
            label1 = new Label();
            pnlContenidoVenta = new TableLayoutPanel();
            tlpNuevaVenta = new Panel();
            pnlDocumentos = new Panel();
            lblAclaracionDocumentos = new Label();
            lblTituloDocumentos = new Label();
            btnRegistrarVenta = new Button();
            pnlFormaEntrega = new Panel();
            lblUsarDireccionRegistrada = new Label();
            txtSegundoTelefono = new TextBox();
            label3 = new Label();
            nudCostoEnvio = new NumericUpDown();
            lblCostoEnvio = new Label();
            rdoEnvioDomicilio = new RadioButton();
            rdoRetiroPosterior = new RadioButton();
            cboTipoLogistica = new ComboBox();
            lblTipoLogistica = new Label();
            txtHorarioSugerido = new TextBox();
            lblHorarioSugerido = new Label();
            txtObservacionesEntrega = new TextBox();
            lblObservacionesEntrega = new Label();
            dtpFechaEntrega = new DateTimePicker();
            lblFechaEntrega = new Label();
            txtDomicilioEntrega = new TextBox();
            lblDomicilioEntrega = new Label();
            lblTituloFormaEntrega = new Label();
            rdoRetiroLocal = new RadioButton();
            pnlEncabezadoVenta = new Panel();
            lblNumeroVenta = new Label();
            lblTituloNuevaVenta = new Label();
            pnlCliente = new Panel();
            dgvClienteSeleccionado = new DataGridView();
            lblClienteSeleccionado = new Label();
            lblTituloCliente = new Label();
            pnlProductos = new Panel();
            dgvDetalleVenta = new DataGridView();
            colProducto = new DataGridViewTextBoxColumn();
            colVariante = new DataGridViewTextBoxColumn();
            colCantidadVenta = new DataGridViewTextBoxColumn();
            colCantidadVenta.Name = "colCantidadVenta";
            colCantidadVenta.HeaderText = "Cantidad";
            colCantidadVenta.ReadOnly = true;
            colCantidadVenta.FillWeight = 45;
            colPrecioUnitario = new DataGridViewTextBoxColumn();
            colSubtotal = new DataGridViewTextBoxColumn();
            colEliminar = new DataGridViewButtonColumn();
            colStockFisico = new DataGridViewTextBoxColumn();
            colStockComprometido = new DataGridViewTextBoxColumn();
            btnAgregarProducto = new Button();
            nudCantidad = new NumericUpDown();
            lblTituloProductos = new Label();
            pnlDescuentoTotales = new Panel();
            lblTotal = new Label();
            lblTotalTexto = new Label();
            lblDescuentoAplicado = new Label();
            lblDescuentoAplicadoTexto = new Label();
            lblSubtotal = new Label();
            lblSubtotalTexto = new Label();
            lblEnvio = new Label();
            lblEnvioTexto = new Label();
            lblPorcentaje = new Label();
            nudDescuento = new NumericUpDown();
            lblDescuento = new Label();
            lblTituloDescuento = new Label();
            pnlPago = new Panel();
            cboCantCuotas = new ComboBox();
            lblCuotasPago = new Label();
            lblTotalPagoActual = new Label();
            txtObservacionPago = new TextBox();
            lblObservacionPago = new Label();
            dtpFechaPago = new DateTimePicker();
            lblFechaPago = new Label();
            cboCondicionPago = new ComboBox();
            lblCondicionPago = new Label();
            dgvPagos = new DataGridView();
            colMedioPago = new DataGridViewTextBoxColumn();
            colMontoPago = new DataGridViewTextBoxColumn();
            colRecargoPago = new DataGridViewTextBoxColumn();
            colTotalPago = new DataGridViewTextBoxColumn();
            colQuitarPago = new DataGridViewButtonColumn();
            btnAgregarPago = new Button();
            nudMontoPago = new NumericUpDown();
            lblMontoPago = new Label();
            cboMedioPago = new ComboBox();
            lblTituloPago = new Label();
            lblMedioPago = new Label();
            pnlPagina = new Panel();
            tlpSecciones = new TableLayoutPanel();
            chkIncluirSinComprasRecientes = new CheckBox();
            pnlContenidoVenta.SuspendLayout();
            pnlDocumentos.SuspendLayout();
            pnlFormaEntrega.SuspendLayout();
            // llama a ((systemcomponentmodelisupportinitialize)nudcostoenvio)begininit para realizar este paso del flujo
            ((System.ComponentModel.ISupportInitialize)nudCostoEnvio).BeginInit();
            pnlEncabezadoVenta.SuspendLayout();
            pnlCliente.SuspendLayout();
            // llama a ((systemcomponentmodelisupportinitialize)dgvclienteseleccionado)begininit para realizar este paso del flujo
            ((System.ComponentModel.ISupportInitialize)dgvClienteSeleccionado).BeginInit();
            pnlProductos.SuspendLayout();
            // llama a ((systemcomponentmodelisupportinitialize)dgvdetalleventa)begininit para realizar este paso del flujo
            ((System.ComponentModel.ISupportInitialize)dgvDetalleVenta).BeginInit();
            // llama a ((systemcomponentmodelisupportinitialize)nudcantidad)begininit para realizar este paso del flujo
            ((System.ComponentModel.ISupportInitialize)nudCantidad).BeginInit();
            pnlDescuentoTotales.SuspendLayout();
            // llama a ((systemcomponentmodelisupportinitialize)nuddescuento)begininit para realizar este paso del flujo
            ((System.ComponentModel.ISupportInitialize)nudDescuento).BeginInit();
            pnlPago.SuspendLayout();
            // llama a ((systemcomponentmodelisupportinitialize)dgvpagos)begininit para realizar este paso del flujo
            ((System.ComponentModel.ISupportInitialize)dgvPagos).BeginInit();
            // llama a ((systemcomponentmodelisupportinitialize)nudmontopago)begininit para realizar este paso del flujo
            ((System.ComponentModel.ISupportInitialize)nudMontoPago).BeginInit();
            pnlPagina.SuspendLayout();
            tlpSecciones.SuspendLayout();
            SuspendLayout();
            // lblcobroprevisto lblcobroprevistoanchor: bordes que mantienen su distancia cuando cambia el tamano del contenedor
            lblCobroPrevisto.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblCobroPrevisto.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblCobroPrevisto.ForeColor = Color.White;
            lblCobroPrevisto.Location = new Point(20, 245);
            lblCobroPrevisto.Name = "lblCobroPrevisto";
            lblCobroPrevisto.Size = new Size(797, 26);
            lblCobroPrevisto.TabIndex = 20;
            lblCobroPrevisto.Click += lblCobroPrevisto_Click;
            // usardireccionregistrada
            usarDireccionRegistrada.AutoSize = true;
            usarDireccionRegistrada.ForeColor = SystemColors.ButtonHighlight;
            usarDireccionRegistrada.Location = new Point(520, 56);
            usarDireccionRegistrada.Name = "usarDireccionRegistrada";
            usarDireccionRegistrada.Size = new Size(15, 14);
            usarDireccionRegistrada.TabIndex = 16;
            usarDireccionRegistrada.CheckedChanged += usarDireccionRegistrada_CheckedChanged;
            // txtcuentadestino txtcuentadestinolocation: posicion horizontal y vertical respecto del contenedor
            txtCuentaDestino.Location = new Point(280, 200);
            txtCuentaDestino.MaxLength = 30;
            txtCuentaDestino.Name = "txtCuentaDestino";
            txtCuentaDestino.Size = new Size(250, 23);
            txtCuentaDestino.TabIndex = 21;
            // lblcuentadestino
            lblCuentaDestino.AutoSize = true;
            lblCuentaDestino.ForeColor = Color.White;
            lblCuentaDestino.Location = new Point(280, 182);
            lblCuentaDestino.Name = "lblCuentaDestino";
            lblCuentaDestino.Size = new Size(150, 15);
            lblCuentaDestino.TabIndex = 22;
            lblCuentaDestino.Text = "Cuenta destino / referencia";
            // cbocliente cboclienteautocompletemode: comportamiento nativo de sugerencias al escribir en la lista
            cboCliente.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboCliente.AutoCompleteSource = AutoCompleteSource.ListItems;
            cboCliente.Font = new Font("Segoe UI", 10F);
            cboCliente.Location = new Point(20, 36);
            cboCliente.MaxLength = 201;
            cboCliente.Name = "cboCliente";
            cboCliente.Size = new Size(460, 25);
            cboCliente.TabIndex = 1;
            cboCliente.SelectionChangeCommitted += cboCliente_SelectionChangeCommitted;
            cboCliente.TextUpdate += cboCliente_TextUpdate;
            // cbobuscarproducto cbobuscarproductodropdownstyle: establece si la lista permite escribir o solo elegir elementos
            cboBuscarProducto.DropDownStyle = ComboBoxStyle.DropDown;
            cboBuscarProducto.Font = new Font("Segoe UI", 10F);
            cboBuscarProducto.Location = new Point(20, 50);
            cboBuscarProducto.MaxDropDownItems = 12;
            cboBuscarProducto.IntegralHeight = false;
            cboBuscarProducto.DropDownHeight = 240;
            cboBuscarProducto.MaxLength = 32767;
            cboBuscarProducto.Name = "cboBuscarProducto";
            cboBuscarProducto.Size = new Size(420, 25);
            cboBuscarProducto.TabIndex = 1;
            cboBuscarProducto.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboBuscarProducto.AutoCompleteSource = AutoCompleteSource.ListItems;
            cboBuscarProducto.TextUpdate += cboBuscarProducto_TextUpdate;
            cboBuscarProducto.SelectionChangeCommitted += cboBuscarProducto_SelectionChangeCommitted;
            // label1
            label1.AutoSize = true;
            label1.Location = new Point(109, 55);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(126, 25);
            label1.TabIndex = 0;
            label1.Text = "NUEVA VENTA";
            label1.Visible = false;
            // pnlcontenidoventa pnlcontenidoventaanchor: bordes que mantienen su distancia cuando cambia el tamano del contenedor
            pnlContenidoVenta.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pnlContenidoVenta.AutoScroll = true;
            pnlContenidoVenta.AutoSize = true;
            pnlContenidoVenta.BackColor = Color.FromArgb(13, 19, 27);
            pnlContenidoVenta.ColumnCount = 1;
            pnlContenidoVenta.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            pnlContenidoVenta.Controls.Add(tlpNuevaVenta, 0, 1);
            pnlContenidoVenta.Location = new Point(0, 0);
            pnlContenidoVenta.Margin = new Padding(0);
            pnlContenidoVenta.Name = "pnlContenidoVenta";
            pnlContenidoVenta.RowCount = 3;
            pnlContenidoVenta.RowStyles.Add(new RowStyle(SizeType.Absolute, 108F));
            pnlContenidoVenta.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            pnlContenidoVenta.RowStyles.Add(new RowStyle(SizeType.Absolute, 125F));
            pnlContenidoVenta.Size = new Size(1624, 3239);
            pnlContenidoVenta.TabIndex = 1;
            pnlContenidoVenta.Visible = false;
            pnlContenidoVenta.Paint += pnlContenidoVenta_Paint;
            // tlpnuevaventa
            tlpNuevaVenta.AutoScroll = true;
            tlpNuevaVenta.AutoSize = true;
            tlpNuevaVenta.BackColor = Color.FromArgb(13, 19, 27);
            tlpNuevaVenta.Dock = DockStyle.Fill;
            tlpNuevaVenta.ForeColor = Color.White;
            tlpNuevaVenta.Location = new Point(4, 113);
            tlpNuevaVenta.Margin = new Padding(4, 5, 4, 5);
            tlpNuevaVenta.Name = "tlpNuevaVenta";
            tlpNuevaVenta.Padding = new Padding(0, 17, 0, 17);
            tlpNuevaVenta.Size = new Size(1616, 2996);
            tlpNuevaVenta.TabIndex = 1;
            tlpNuevaVenta.Paint += panel1_Paint;
            // pnldocumentos pnldocumentosbackcolor: color del fondo del control o de la celda
            pnlDocumentos.BackColor = Color.FromArgb(21, 30, 41);
            pnlDocumentos.Controls.Add(lblAclaracionDocumentos);
            pnlDocumentos.Controls.Add(lblTituloDocumentos);
            pnlDocumentos.Controls.Add(btnRegistrarVenta);
            pnlDocumentos.Dock = DockStyle.Top;
            pnlDocumentos.Location = new Point(6, 1618);
            pnlDocumentos.Margin = new Padding(0, 0, 0, 5);
            pnlDocumentos.Name = "pnlDocumentos";
            pnlDocumentos.Size = new Size(1318, 124);
            pnlDocumentos.TabIndex = 6;
            // lblaclaraciondocumentos
            lblAclaracionDocumentos.AutoSize = true;
            lblAclaracionDocumentos.ForeColor = Color.FromArgb(145, 170, 210);
            lblAclaracionDocumentos.Location = new Point(20, 100);
            lblAclaracionDocumentos.Name = "lblAclaracionDocumentos";
            lblAclaracionDocumentos.Size = new Size(355, 15);
            lblAclaracionDocumentos.TabIndex = 3;
            lblAclaracionDocumentos.Text = "La garantía y el remito se generan con el mismo número de venta.";
            // lbltitulodocumentos
            lblTituloDocumentos.AutoSize = true;
            lblTituloDocumentos.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTituloDocumentos.ForeColor = Color.White;
            lblTituloDocumentos.Location = new Point(20, 15);
            lblTituloDocumentos.Name = "lblTituloDocumentos";
            lblTituloDocumentos.Size = new Size(124, 21);
            lblTituloDocumentos.TabIndex = 0;
            lblTituloDocumentos.Text = "6. Documentos";
            // btnregistrarventa btnregistrarventabackcolor: color del fondo del control o de la celda
            btnRegistrarVenta.BackColor = Color.FromArgb(35, 135, 190);
            btnRegistrarVenta.FlatAppearance.BorderSize = 0;
            btnRegistrarVenta.FlatStyle = FlatStyle.Flat;
            btnRegistrarVenta.ForeColor = Color.White;
            btnRegistrarVenta.Location = new Point(20, 53);
            btnRegistrarVenta.Name = "btnRegistrarVenta";
            btnRegistrarVenta.Size = new Size(185, 30);
            btnRegistrarVenta.TabIndex = 16;
            btnRegistrarVenta.Text = "Registrar venta";
            btnRegistrarVenta.UseVisualStyleBackColor = false;
            btnRegistrarVenta.Click += btnRegistrarVenta_Click;
            // pnlformaentrega pnlformaentregabackcolor: color del fondo del control o de la celda
            pnlFormaEntrega.BackColor = Color.FromArgb(21, 30, 41);
            pnlFormaEntrega.Controls.Add(lblUsarDireccionRegistrada);
            pnlFormaEntrega.Controls.Add(txtSegundoTelefono);
            pnlFormaEntrega.Controls.Add(label3);
            pnlFormaEntrega.Controls.Add(nudCostoEnvio);
            pnlFormaEntrega.Controls.Add(lblCostoEnvio);
            pnlFormaEntrega.Controls.Add(rdoEnvioDomicilio);
            pnlFormaEntrega.Controls.Add(rdoRetiroPosterior);
            pnlFormaEntrega.Controls.Add(cboTipoLogistica);
            pnlFormaEntrega.Controls.Add(lblTipoLogistica);
            pnlFormaEntrega.Controls.Add(txtHorarioSugerido);
            pnlFormaEntrega.Controls.Add(lblHorarioSugerido);
            pnlFormaEntrega.Controls.Add(txtObservacionesEntrega);
            pnlFormaEntrega.Controls.Add(lblObservacionesEntrega);
            pnlFormaEntrega.Controls.Add(dtpFechaEntrega);
            pnlFormaEntrega.Controls.Add(lblFechaEntrega);
            pnlFormaEntrega.Controls.Add(txtDomicilioEntrega);
            pnlFormaEntrega.Controls.Add(lblDomicilioEntrega);
            pnlFormaEntrega.Controls.Add(lblTituloFormaEntrega);
            pnlFormaEntrega.Controls.Add(rdoRetiroLocal);
            pnlFormaEntrega.Controls.Add(usarDireccionRegistrada);
            pnlFormaEntrega.Dock = DockStyle.Top;
            pnlFormaEntrega.Location = new Point(6, 678);
            pnlFormaEntrega.Margin = new Padding(0, 0, 0, 5);
            pnlFormaEntrega.Name = "pnlFormaEntrega";
            pnlFormaEntrega.Size = new Size(1318, 269);
            pnlFormaEntrega.TabIndex = 3;
            pnlFormaEntrega.Paint += pnlFormaEntrega_Paint;
            // lblusardireccionregistrada
            lblUsarDireccionRegistrada.AutoSize = true;
            lblUsarDireccionRegistrada.ForeColor = Color.White;
            lblUsarDireccionRegistrada.Location = new Point(541, 55);
            lblUsarDireccionRegistrada.Name = "lblUsarDireccionRegistrada";
            lblUsarDireccionRegistrada.Size = new Size(137, 15);
            lblUsarDireccionRegistrada.TabIndex = 19;
            lblUsarDireccionRegistrada.Text = "Usar direccion registrada";
            // txtsegundotelefono txtsegundotelefonolocation: posicion horizontal y vertical respecto del contenedor
            txtSegundoTelefono.Location = new Point(390, 189);
            txtSegundoTelefono.MaxLength = 10;
            txtSegundoTelefono.Name = "txtSegundoTelefono";
            txtSegundoTelefono.Size = new Size(143, 23);
            txtSegundoTelefono.TabIndex = 18;
            // label3
            label3.AutoSize = true;
            label3.Location = new Point(280, 192);
            label3.Name = "label3";
            label3.Size = new Size(101, 15);
            label3.TabIndex = 17;
            label3.Text = "Segundo telefono";
            label3.Click += label3_Click;
            // nudcostoenvio
            nudCostoEnvio.DecimalPlaces = 2;
            nudCostoEnvio.Enabled = false;
            nudCostoEnvio.Location = new Point(121, 229);
            nudCostoEnvio.Maximum = new decimal(new int[] { 1410065407, 2, 0, 131072 });
            nudCostoEnvio.Name = "nudCostoEnvio";
            nudCostoEnvio.Size = new Size(180, 23);
            nudCostoEnvio.TabIndex = 15;
            nudCostoEnvio.ThousandsSeparator = true;
            nudCostoEnvio.ValueChanged += nudCostoEnvio_ValueChanged;
            // lblcostoenvio
            lblCostoEnvio.AutoSize = true;
            lblCostoEnvio.Location = new Point(20, 231);
            lblCostoEnvio.Name = "lblCostoEnvio";
            lblCostoEnvio.Size = new Size(89, 15);
            lblCostoEnvio.TabIndex = 14;
            lblCostoEnvio.Text = "Costo del envio";
            // rdoenviodomicilio
            rdoEnvioDomicilio.AutoSize = true;
            rdoEnvioDomicilio.ForeColor = Color.White;
            rdoEnvioDomicilio.Location = new Point(368, 55);
            rdoEnvioDomicilio.Name = "rdoEnvioDomicilio";
            rdoEnvioDomicilio.Size = new Size(116, 19);
            rdoEnvioDomicilio.TabIndex = 13;
            rdoEnvioDomicilio.TabStop = true;
            rdoEnvioDomicilio.Text = "Envio a domicilio";
            rdoEnvioDomicilio.TextAlign = ContentAlignment.MiddleCenter;
            rdoEnvioDomicilio.UseVisualStyleBackColor = true;
            rdoEnvioDomicilio.CheckedChanged += rdoEnvioDomicilio_CheckedChanged;
            // rdoretiroposterior
            rdoRetiroPosterior.AutoSize = true;
            rdoRetiroPosterior.ForeColor = Color.White;
            rdoRetiroPosterior.Location = new Point(160, 55);
            rdoRetiroPosterior.Name = "rdoRetiroPosterior";
            rdoRetiroPosterior.Size = new Size(162, 19);
            rdoRetiroPosterior.TabIndex = 12;
            rdoRetiroPosterior.TabStop = true;
            rdoRetiroPosterior.Text = "Retiro posterior en el local";
            rdoRetiroPosterior.TextAlign = ContentAlignment.MiddleCenter;
            rdoRetiroPosterior.UseVisualStyleBackColor = true;
            rdoRetiroPosterior.CheckedChanged += rdoRetiroPosterior_CheckedChanged;
            // cbotipologistica cbotipologisticadropdownstyle: establece si la lista permite escribir o solo elegir elementos
            cboTipoLogistica.DropDownStyle = ComboBoxStyle.DropDownList;
            cboTipoLogistica.FormattingEnabled = true;
            cboTipoLogistica.Items.AddRange(new object[] { "Logística propia", "Empresa externa" });
            cboTipoLogistica.Location = new Point(120, 189);
            cboTipoLogistica.Name = "cboTipoLogistica";
            cboTipoLogistica.Size = new Size(121, 23);
            cboTipoLogistica.TabIndex = 11;
            cboTipoLogistica.SelectedIndexChanged += cboTipoLogistica_SelectedIndexChanged;
            // lbltipologistica
            lblTipoLogistica.AutoSize = true;
            lblTipoLogistica.Location = new Point(20, 192);
            lblTipoLogistica.Name = "lblTipoLogistica";
            lblTipoLogistica.Size = new Size(93, 15);
            lblTipoLogistica.TabIndex = 10;
            lblTipoLogistica.Text = "Tipo de logística";
            // txthorariosugerido
            txtHorarioSugerido.CharacterCasing = CharacterCasing.Upper;
            txtHorarioSugerido.Location = new Point(399, 118);
            txtHorarioSugerido.MaxLength = 150;
            txtHorarioSugerido.Name = "txtHorarioSugerido";
            txtHorarioSugerido.PlaceholderText = "Ej: tarde, de 14 a 18 hs";
            txtHorarioSugerido.Size = new Size(136, 23);
            txtHorarioSugerido.TabIndex = 9;
            // lblhorariosugerido
            lblHorarioSugerido.AutoSize = true;
            lblHorarioSugerido.ForeColor = Color.White;
            lblHorarioSugerido.Location = new Point(399, 95);
            lblHorarioSugerido.Name = "lblHorarioSugerido";
            lblHorarioSugerido.Size = new Size(96, 15);
            lblHorarioSugerido.TabIndex = 8;
            lblHorarioSugerido.Text = "Horario sugerido";
            // txtobservacionesentrega txtobservacionesentregaanchor: bordes que mantienen su distancia cuando cambia el tamano del contenedor
            txtObservacionesEntrega.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtObservacionesEntrega.CharacterCasing = CharacterCasing.Upper;
            txtObservacionesEntrega.Location = new Point(120, 152);
            txtObservacionesEntrega.MaxLength = 150;
            txtObservacionesEntrega.Name = "txtObservacionesEntrega";
            txtObservacionesEntrega.Size = new Size(1052, 23);
            txtObservacionesEntrega.TabIndex = 7;
            // lblobservacionesentrega
            lblObservacionesEntrega.AutoSize = true;
            lblObservacionesEntrega.ForeColor = Color.White;
            lblObservacionesEntrega.Location = new Point(20, 155);
            lblObservacionesEntrega.Name = "lblObservacionesEntrega";
            lblObservacionesEntrega.Size = new Size(84, 15);
            lblObservacionesEntrega.TabIndex = 6;
            lblObservacionesEntrega.Text = "Observaciones";
            // dtpfechaentrega dtpfechaentregaenabled: determina si el usuario puede interactuar con el control en este estado
            dtpFechaEntrega.Enabled = false;
            dtpFechaEntrega.Format = DateTimePickerFormat.Short;
            dtpFechaEntrega.Location = new Point(256, 118);
            dtpFechaEntrega.Name = "dtpFechaEntrega";
            dtpFechaEntrega.Size = new Size(131, 23);
            dtpFechaEntrega.TabIndex = 5;
            // lblfechaentrega
            lblFechaEntrega.AutoSize = true;
            lblFechaEntrega.ForeColor = Color.White;
            lblFechaEntrega.Location = new Point(256, 95);
            lblFechaEntrega.Name = "lblFechaEntrega";
            lblFechaEntrega.Size = new Size(89, 15);
            lblFechaEntrega.TabIndex = 4;
            lblFechaEntrega.Text = "Fecha estimada";
            // txtdomicilioentrega
            txtDomicilioEntrega.CharacterCasing = CharacterCasing.Upper;
            txtDomicilioEntrega.Enabled = false;
            txtDomicilioEntrega.Location = new Point(20, 118);
            txtDomicilioEntrega.MaxLength = 200;
            txtDomicilioEntrega.Name = "txtDomicilioEntrega";
            txtDomicilioEntrega.Size = new Size(232, 23);
            txtDomicilioEntrega.TabIndex = 3;
            // lbldomicilioentrega
            lblDomicilioEntrega.AutoSize = true;
            lblDomicilioEntrega.ForeColor = Color.White;
            lblDomicilioEntrega.Location = new Point(20, 95);
            lblDomicilioEntrega.Name = "lblDomicilioEntrega";
            lblDomicilioEntrega.Size = new Size(117, 15);
            lblDomicilioEntrega.TabIndex = 2;
            lblDomicilioEntrega.Text = "Domicilio de entrega";
            // lbltituloformaentrega
            lblTituloFormaEntrega.AutoSize = true;
            lblTituloFormaEntrega.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTituloFormaEntrega.ForeColor = Color.White;
            lblTituloFormaEntrega.Location = new Point(20, 15);
            lblTituloFormaEntrega.Name = "lblTituloFormaEntrega";
            lblTituloFormaEntrega.Size = new Size(161, 21);
            lblTituloFormaEntrega.TabIndex = 1;
            lblTituloFormaEntrega.Text = "3. Forma de entrega";
            // rdoretirolocal
            rdoRetiroLocal.AutoSize = true;
            rdoRetiroLocal.ForeColor = Color.White;
            rdoRetiroLocal.Location = new Point(20, 55);
            rdoRetiroLocal.Name = "rdoRetiroLocal";
            rdoRetiroLocal.Size = new Size(112, 19);
            rdoRetiroLocal.TabIndex = 0;
            rdoRetiroLocal.TabStop = true;
            rdoRetiroLocal.Text = "Retiro en el local";
            rdoRetiroLocal.TextAlign = ContentAlignment.MiddleCenter;
            rdoRetiroLocal.UseVisualStyleBackColor = true;
            rdoRetiroLocal.CheckedChanged += rdoRetiroLocal_CheckedChanged;
            // pnlencabezadoventa pnlencabezadoventabackcolor: color del fondo del control o de la celda
            pnlEncabezadoVenta.BackColor = Color.FromArgb(21, 30, 41);
            pnlEncabezadoVenta.Controls.Add(lblNumeroVenta);
            pnlEncabezadoVenta.Controls.Add(lblTituloNuevaVenta);
            pnlEncabezadoVenta.Dock = DockStyle.Top;
            pnlEncabezadoVenta.Location = new Point(6, 5);
            pnlEncabezadoVenta.Margin = new Padding(0, 0, 0, 5);
            pnlEncabezadoVenta.Name = "pnlEncabezadoVenta";
            pnlEncabezadoVenta.Size = new Size(1318, 65);
            pnlEncabezadoVenta.TabIndex = 0;
            // lblnumeroventa
            lblNumeroVenta.AutoSize = true;
            lblNumeroVenta.ForeColor = Color.FromArgb(145, 170, 210);
            lblNumeroVenta.Location = new Point(17, 42);
            lblNumeroVenta.Name = "lblNumeroVenta";
            lblNumeroVenta.Size = new Size(56, 15);
            lblNumeroVenta.TabIndex = 1;
            lblNumeroVenta.Text = "Venta N.º";
            // lbltitulonuevaventa
            lblTituloNuevaVenta.AutoSize = true;
            lblTituloNuevaVenta.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTituloNuevaVenta.ForeColor = Color.White;
            lblTituloNuevaVenta.Location = new Point(15, 10);
            lblTituloNuevaVenta.Name = "lblTituloNuevaVenta";
            lblTituloNuevaVenta.Size = new Size(158, 32);
            lblTituloNuevaVenta.TabIndex = 0;
            lblTituloNuevaVenta.Text = "Nueva Venta";
            // pnlcliente pnlclientebackcolor: color del fondo del control o de la celda
            pnlCliente.BackColor = Color.FromArgb(21, 30, 41);
            pnlCliente.Controls.Add(chkIncluirSinComprasRecientes);
            pnlCliente.Controls.Add(dgvClienteSeleccionado);
            pnlCliente.Controls.Add(lblClienteSeleccionado);
            pnlCliente.Controls.Add(cboCliente);
            pnlCliente.Controls.Add(lblTituloCliente);
            pnlCliente.Dock = DockStyle.Top;
            pnlCliente.ForeColor = Color.White;
            pnlCliente.Location = new Point(6, 75);
            pnlCliente.Margin = new Padding(0, 0, 0, 5);
            pnlCliente.Name = "pnlCliente";
            pnlCliente.Size = new Size(1318, 267);
            pnlCliente.TabIndex = 1;
            pnlCliente.Paint += pnlCliente_Paint;
            // dgvclienteseleccionado
            dgvClienteSeleccionado.AllowUserToAddRows = false;
            dgvClienteSeleccionado.AllowUserToDeleteRows = false;
            dataGridViewCellStyle19.BackColor = Color.FromArgb(236, 242, 249);
            dgvClienteSeleccionado.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle19;
            dgvClienteSeleccionado.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            dgvClienteSeleccionado.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvClienteSeleccionado.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvClienteSeleccionado.BackgroundColor = Color.FromArgb(13, 19, 27);
            dgvClienteSeleccionado.BorderStyle = BorderStyle.None;
            dataGridViewCellStyle20.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle20.BackColor = Color.FromArgb(37, 52, 70);
            dataGridViewCellStyle20.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle20.ForeColor = Color.White;
            dataGridViewCellStyle20.SelectionBackColor = Color.FromArgb(190, 220, 250);
            dataGridViewCellStyle20.SelectionForeColor = Color.Black;
            dataGridViewCellStyle20.SelectionBackColor = Color.FromArgb(190, 220, 250);
            dataGridViewCellStyle20.WrapMode = DataGridViewTriState.True;
            dgvClienteSeleccionado.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle20;
            dgvClienteSeleccionado.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle21.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle21.BackColor = Color.White;
            dataGridViewCellStyle21.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle21.ForeColor = Color.White;
            dataGridViewCellStyle21.Padding = new Padding(4);
            dataGridViewCellStyle21.SelectionBackColor = Color.FromArgb(190, 220, 250);
            dataGridViewCellStyle21.SelectionForeColor = Color.Black;
            dataGridViewCellStyle21.SelectionBackColor = Color.FromArgb(190, 220, 250);
            dataGridViewCellStyle21.WrapMode = DataGridViewTriState.True;
            dgvClienteSeleccionado.DefaultCellStyle = dataGridViewCellStyle21;
            dgvClienteSeleccionado.EnableHeadersVisualStyles = false;
            dgvClienteSeleccionado.Location = new Point(17, 93);
            dgvClienteSeleccionado.MultiSelect = false;
            dgvClienteSeleccionado.Name = "dgvClienteSeleccionado";
            dgvClienteSeleccionado.ForeColor = Color.Black;
            dgvClienteSeleccionado.DefaultCellStyle.ForeColor = Color.Black;
            dgvClienteSeleccionado.RowsDefaultCellStyle.ForeColor = Color.Black;
            dgvClienteSeleccionado.AlternatingRowsDefaultCellStyle.ForeColor = Color.Black;
            dgvClienteSeleccionado.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.Black;
            dgvClienteSeleccionado.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(190, 220, 250);
            dgvClienteSeleccionado.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvClienteSeleccionado.DefaultCellStyle.SelectionBackColor = Color.FromArgb(190, 220, 250);
            dgvClienteSeleccionado.AlternatingRowsDefaultCellStyle.SelectionForeColor = Color.Black;
            dgvClienteSeleccionado.AlternatingRowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(190, 220, 250);
            dgvClienteSeleccionado.ReadOnly = true;
            dgvClienteSeleccionado.RowHeadersVisible = false;
            dgvClienteSeleccionado.RowHeadersWidth = 62;
            dgvClienteSeleccionado.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvClienteSeleccionado.Size = new Size(797, 87);
            dgvClienteSeleccionado.TabIndex = 5;
            // lblclienteseleccionado
            lblClienteSeleccionado.AutoSize = true;
            lblClienteSeleccionado.ForeColor = Color.FromArgb(145, 170, 210);
            lblClienteSeleccionado.Location = new Point(20, 64);
            lblClienteSeleccionado.Name = "lblClienteSeleccionado";
            lblClienteSeleccionado.Size = new Size(157, 15);
            lblClienteSeleccionado.TabIndex = 3;
            lblClienteSeleccionado.Text = "Ningún cliente seleccionado";
            // lbltitulocliente
            lblTituloCliente.AutoSize = true;
            lblTituloCliente.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTituloCliente.ForeColor = Color.White;
            lblTituloCliente.Location = new Point(15, 12);
            lblTituloCliente.Name = "lblTituloCliente";
            lblTituloCliente.Size = new Size(81, 21);
            lblTituloCliente.TabIndex = 0;
            lblTituloCliente.Text = "1. Cliente";
            // pnlproductos pnlproductosbackcolor: color del fondo del control o de la celda
            pnlProductos.BackColor = Color.FromArgb(21, 30, 41);
            pnlProductos.Controls.Add(dgvDetalleVenta);
            pnlProductos.Controls.Add(btnAgregarProducto);
            pnlProductos.Controls.Add(nudCantidad);
            pnlProductos.Controls.Add(cboBuscarProducto);
            pnlProductos.Controls.Add(lblTituloProductos);
            pnlProductos.Dock = DockStyle.Top;
            pnlProductos.Location = new Point(6, 347);
            pnlProductos.Margin = new Padding(0, 0, 0, 5);
            pnlProductos.Name = "pnlProductos";
            pnlProductos.Size = new Size(1318, 326);
            pnlProductos.TabIndex = 2;
            pnlProductos.Paint += pnlProductos_Paint;
            // dgvdetalleventa
            dgvDetalleVenta.AllowUserToAddRows = false;
            dgvDetalleVenta.AllowUserToDeleteRows = false;
            dataGridViewCellStyle22.BackColor = Color.FromArgb(236, 242, 249);
            dgvDetalleVenta.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle22;
            dgvDetalleVenta.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            dgvDetalleVenta.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvDetalleVenta.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvDetalleVenta.BackgroundColor = Color.FromArgb(13, 19, 27);
            dgvDetalleVenta.BorderStyle = BorderStyle.None;
            dataGridViewCellStyle23.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle23.BackColor = Color.FromArgb(37, 52, 70);
            dataGridViewCellStyle23.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle23.ForeColor = Color.White;
            dataGridViewCellStyle23.SelectionBackColor = Color.FromArgb(190, 220, 250);
            dataGridViewCellStyle23.SelectionForeColor = Color.Black;
            dataGridViewCellStyle23.SelectionBackColor = Color.FromArgb(190, 220, 250);
            dataGridViewCellStyle23.WrapMode = DataGridViewTriState.True;
            dgvDetalleVenta.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle23;
            dgvDetalleVenta.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvDetalleVenta.Columns.AddRange(new DataGridViewColumn[] { colProducto, colVariante, colCantidadVenta, colPrecioUnitario, colSubtotal, colEliminar, colStockFisico, colStockComprometido });
            dataGridViewCellStyle24.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle24.BackColor = Color.White;
            dataGridViewCellStyle24.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle24.ForeColor = Color.White;
            dataGridViewCellStyle24.Padding = new Padding(4);
            dataGridViewCellStyle24.SelectionBackColor = Color.FromArgb(190, 220, 250);
            dataGridViewCellStyle24.SelectionForeColor = Color.Black;
            dataGridViewCellStyle24.SelectionBackColor = Color.FromArgb(190, 220, 250);
            dataGridViewCellStyle24.WrapMode = DataGridViewTriState.True;
            dgvDetalleVenta.DefaultCellStyle = dataGridViewCellStyle24;
            dgvDetalleVenta.EnableHeadersVisualStyles = false;
            dgvDetalleVenta.Location = new Point(20, 123);
            dgvDetalleVenta.MultiSelect = false;
            dgvDetalleVenta.Name = "dgvDetalleVenta";
            dgvDetalleVenta.ForeColor = Color.Black;
            dgvDetalleVenta.DefaultCellStyle.ForeColor = Color.Black;
            dgvDetalleVenta.RowsDefaultCellStyle.ForeColor = Color.Black;
            dgvDetalleVenta.AlternatingRowsDefaultCellStyle.ForeColor = Color.Black;
            dgvDetalleVenta.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.Black;
            dgvDetalleVenta.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(190, 220, 250);
            dgvDetalleVenta.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvDetalleVenta.DefaultCellStyle.SelectionBackColor = Color.FromArgb(190, 220, 250);
            dgvDetalleVenta.AlternatingRowsDefaultCellStyle.SelectionForeColor = Color.Black;
            dgvDetalleVenta.AlternatingRowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(190, 220, 250);
            dgvDetalleVenta.ReadOnly = true;
            dgvDetalleVenta.RowHeadersVisible = false;
            dgvDetalleVenta.RowHeadersWidth = 62;
            dgvDetalleVenta.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDetalleVenta.Size = new Size(1158, 138);
            dgvDetalleVenta.TabIndex = 5;
            dgvDetalleVenta.CellContentClick += dgvDetalleVenta_CellContentClick;
            // colproducto colproductoheadertext: titulo de la columna; no es el nombre de la propiedad enlazada
            colProducto.HeaderText = "Producto";
            colProducto.MinimumWidth = 8;
            colProducto.Name = "colProducto";
            colProducto.ReadOnly = true;
            // colvariante colvarianteheadertext: titulo de la columna; no es el nombre de la propiedad enlazada
            colVariante.HeaderText = "Variante";
            colVariante.MinimumWidth = 8;
            colVariante.Name = "colVariante";
            colVariante.ReadOnly = true;
            // colpreciounitario colpreciounitarioheadertext: titulo de la columna; no es el nombre de la propiedad enlazada
            colPrecioUnitario.HeaderText = "Precio unitario";
            colPrecioUnitario.MinimumWidth = 8;
            colPrecioUnitario.Name = "colPrecioUnitario";
            colPrecioUnitario.ReadOnly = true;
            // colsubtotal colsubtotalheadertext: titulo de la columna; no es el nombre de la propiedad enlazada
            colSubtotal.HeaderText = "Subtotal";
            colSubtotal.MinimumWidth = 8;
            colSubtotal.Name = "colSubtotal";
            colSubtotal.ReadOnly = true;
            // coleliminar
            colEliminar.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            colEliminar.HeaderText = "Eliminar";
            colEliminar.MinimumWidth = 8;
            colEliminar.Name = "colEliminar";
            colEliminar.DefaultCellStyle.ForeColor = Color.Black;
            colEliminar.DefaultCellStyle.SelectionForeColor = Color.Black;
            colEliminar.DefaultCellStyle.BackColor = Color.Gainsboro;
            colEliminar.DefaultCellStyle.SelectionBackColor = Color.Gainsboro;
            colEliminar.ReadOnly = true;
            colEliminar.Resizable = DataGridViewTriState.True;
            colEliminar.SortMode = DataGridViewColumnSortMode.Automatic;
            colEliminar.Width = 80;
            // colstockfisico colstockfisicoheadertext: titulo de la columna; no es el nombre de la propiedad enlazada
            colStockFisico.HeaderText = "Stock físico";
            colStockFisico.MinimumWidth = 8;
            colStockFisico.Name = "colStockFisico";
            colStockFisico.ReadOnly = true;
            // colstockcomprometido colstockcomprometidoheadertext: titulo de la columna; no es el nombre de la propiedad enlazada
            colStockComprometido.HeaderText = "Stock comprometido";
            colStockComprometido.MinimumWidth = 8;
            colStockComprometido.Name = "colStockComprometido";
            colStockComprometido.ReadOnly = true;
            // btnagregarproducto btnagregarproductobackcolor: color del fondo del control o de la celda
            btnAgregarProducto.BackColor = Color.FromArgb(35, 135, 190);
            btnAgregarProducto.FlatAppearance.BorderColor = Color.FromArgb(35, 135, 190);
            btnAgregarProducto.FlatAppearance.BorderSize = 0;
            btnAgregarProducto.FlatStyle = FlatStyle.Flat;
            btnAgregarProducto.ForeColor = SystemColors.Window;
            btnAgregarProducto.Location = new Point(572, 46);
            btnAgregarProducto.Name = "btnAgregarProducto";
            btnAgregarProducto.Size = new Size(85, 29);
            btnAgregarProducto.TabIndex = 4;
            btnAgregarProducto.Text = "Agregar";
            btnAgregarProducto.UseVisualStyleBackColor = false;
            btnAgregarProducto.Click += btnAgregarProducto_Click;
            // nudcantidad nudcantidadlocation: posicion horizontal y vertical respecto del contenedor
            nudCantidad.Location = new Point(476, 50);
            nudCantidad.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            nudCantidad.Name = "nudCantidad";
            nudCantidad.Size = new Size(70, 23);
            nudCantidad.TabIndex = 3;
            nudCantidad.TextAlign = HorizontalAlignment.Center;
            nudCantidad.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // lbltituloproductos
            lblTituloProductos.AutoSize = true;
            lblTituloProductos.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTituloProductos.ForeColor = Color.White;
            lblTituloProductos.Location = new Point(20, 15);
            lblTituloProductos.Name = "lblTituloProductos";
            lblTituloProductos.Size = new Size(104, 21);
            lblTituloProductos.TabIndex = 0;
            lblTituloProductos.Text = "2. Productos";
            // pnldescuentototales pnldescuentototalesbackcolor: color del fondo del control o de la celda
            pnlDescuentoTotales.BackColor = Color.FromArgb(21, 30, 41);
            pnlDescuentoTotales.Controls.Add(lblTotal);
            pnlDescuentoTotales.Controls.Add(lblTotalTexto);
            pnlDescuentoTotales.Controls.Add(lblDescuentoAplicado);
            pnlDescuentoTotales.Controls.Add(lblDescuentoAplicadoTexto);
            pnlDescuentoTotales.Controls.Add(lblSubtotal);
            pnlDescuentoTotales.Controls.Add(lblSubtotalTexto);
            pnlDescuentoTotales.Controls.Add(lblEnvio);
            pnlDescuentoTotales.Controls.Add(lblEnvioTexto);
            pnlDescuentoTotales.Controls.Add(lblPorcentaje);
            pnlDescuentoTotales.Controls.Add(nudDescuento);
            pnlDescuentoTotales.Controls.Add(lblDescuento);
            pnlDescuentoTotales.Controls.Add(lblTituloDescuento);
            pnlDescuentoTotales.Dock = DockStyle.Top;
            pnlDescuentoTotales.Location = new Point(6, 952);
            pnlDescuentoTotales.Margin = new Padding(0, 0, 0, 5);
            pnlDescuentoTotales.Name = "pnlDescuentoTotales";
            pnlDescuentoTotales.Size = new Size(1318, 165);
            pnlDescuentoTotales.TabIndex = 4;
            pnlDescuentoTotales.Paint += panel1_Paint_2;
            // lbltotal
            lblTotal.AutoSize = true;
            lblTotal.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTotal.ForeColor = Color.White;
            lblTotal.Location = new Point(371, 115);
            lblTotal.Name = "lblTotal";
            lblTotal.Size = new Size(61, 25);
            lblTotal.TabIndex = 9;
            lblTotal.Text = "$0,00";
            // lbltotaltexto
            lblTotalTexto.AutoSize = true;
            lblTotalTexto.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTotalTexto.ForeColor = Color.White;
            lblTotalTexto.Location = new Point(245, 118);
            lblTotalTexto.Name = "lblTotalTexto";
            lblTotalTexto.Size = new Size(60, 21);
            lblTotalTexto.TabIndex = 8;
            lblTotalTexto.Text = "TOTAL:";
            // lbldescuentoaplicado
            lblDescuentoAplicado.AutoSize = true;
            lblDescuentoAplicado.ForeColor = Color.White;
            lblDescuentoAplicado.Location = new Point(371, 58);
            lblDescuentoAplicado.Name = "lblDescuentoAplicado";
            lblDescuentoAplicado.Size = new Size(42, 15);
            lblDescuentoAplicado.TabIndex = 7;
            lblDescuentoAplicado.Text = "- $0,00";
            // lbldescuentoaplicadotexto
            lblDescuentoAplicadoTexto.AutoSize = true;
            lblDescuentoAplicadoTexto.ForeColor = Color.White;
            lblDescuentoAplicadoTexto.Location = new Point(245, 58);
            lblDescuentoAplicadoTexto.Name = "lblDescuentoAplicadoTexto";
            lblDescuentoAplicadoTexto.Size = new Size(66, 15);
            lblDescuentoAplicadoTexto.TabIndex = 6;
            lblDescuentoAplicadoTexto.Text = "Descuento:";
            // lblsubtotal
            lblSubtotal.AutoSize = true;
            lblSubtotal.ForeColor = Color.White;
            lblSubtotal.Location = new Point(371, 25);
            lblSubtotal.Name = "lblSubtotal";
            lblSubtotal.Size = new Size(34, 15);
            lblSubtotal.TabIndex = 5;
            lblSubtotal.Text = "$0,00";
            // lblsubtotaltexto
            lblSubtotalTexto.AutoSize = true;
            lblSubtotalTexto.ForeColor = Color.White;
            lblSubtotalTexto.Location = new Point(245, 25);
            lblSubtotalTexto.Name = "lblSubtotalTexto";
            lblSubtotalTexto.Size = new Size(54, 15);
            lblSubtotalTexto.TabIndex = 4;
            lblSubtotalTexto.Text = "Subtotal:";
            // lblenvio
            lblEnvio.AutoSize = true;
            lblEnvio.ForeColor = Color.White;
            lblEnvio.Location = new Point(371, 83);
            lblEnvio.Name = "lblEnvio";
            lblEnvio.Size = new Size(34, 15);
            lblEnvio.TabIndex = 10;
            lblEnvio.Text = "$0,00";
            // lblenviotexto
            lblEnvioTexto.AutoSize = true;
            lblEnvioTexto.ForeColor = Color.White;
            lblEnvioTexto.Location = new Point(245, 83);
            lblEnvioTexto.Name = "lblEnvioTexto";
            lblEnvioTexto.Size = new Size(39, 15);
            lblEnvioTexto.TabIndex = 11;
            lblEnvioTexto.Text = "Envío:";
            // lblporcentaje
            lblPorcentaje.AutoSize = true;
            lblPorcentaje.ForeColor = Color.White;
            lblPorcentaje.Location = new Point(90, 65);
            lblPorcentaje.Name = "lblPorcentaje";
            lblPorcentaje.Size = new Size(13, 15);
            lblPorcentaje.TabIndex = 3;
            lblPorcentaje.Text = "$";
            // nuddescuento
            nudDescuento.DecimalPlaces = 2;
            nudDescuento.Location = new Point(105, 61);
            nudDescuento.Maximum = new decimal(new int[] { 1410065407, 2, 0, 131072 });
            nudDescuento.Name = "nudDescuento";
            nudDescuento.Size = new Size(75, 23);
            nudDescuento.TabIndex = 2;
            nudDescuento.ThousandsSeparator = true;
            nudDescuento.ValueChanged += nudDescuento_ValueChanged;
            // lbldescuento
            lblDescuento.AutoSize = true;
            lblDescuento.ForeColor = Color.White;
            lblDescuento.Location = new Point(20, 65);
            lblDescuento.Name = "lblDescuento";
            lblDescuento.Size = new Size(66, 15);
            lblDescuento.TabIndex = 1;
            lblDescuento.Text = "Descuento:\n";
            // lbltitulodescuento
            lblTituloDescuento.AutoSize = true;
            lblTituloDescuento.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTituloDescuento.ForeColor = Color.White;
            lblTituloDescuento.Location = new Point(20, 15);
            lblTituloDescuento.Name = "lblTituloDescuento";
            lblTituloDescuento.Size = new Size(177, 21);
            lblTituloDescuento.TabIndex = 0;
            lblTituloDescuento.Text = "4. Descuento y totales";
            // pnlpago pnlpagobackcolor: color del fondo del control o de la celda
            pnlPago.BackColor = Color.FromArgb(21, 30, 41);
            pnlPago.Controls.Add(cboCantCuotas);
            pnlPago.Controls.Add(lblCuotasPago);
            pnlPago.Controls.Add(lblTotalPagoActual);
            pnlPago.Controls.Add(txtObservacionPago);
            pnlPago.Controls.Add(lblObservacionPago);
            pnlPago.Controls.Add(dtpFechaPago);
            pnlPago.Controls.Add(lblFechaPago);
            pnlPago.Controls.Add(cboCondicionPago);
            pnlPago.Controls.Add(lblCondicionPago);
            pnlPago.Controls.Add(dgvPagos);
            pnlPago.Controls.Add(btnAgregarPago);
            pnlPago.Controls.Add(nudMontoPago);
            pnlPago.Controls.Add(lblMontoPago);
            pnlPago.Controls.Add(cboMedioPago);
            pnlPago.Controls.Add(lblTituloPago);
            pnlPago.Controls.Add(lblMedioPago);
            pnlPago.Controls.Add(lblCobroPrevisto);
            pnlPago.Controls.Add(txtCuentaDestino);
            pnlPago.Controls.Add(lblCuentaDestino);
            pnlPago.Dock = DockStyle.Top;
            pnlPago.Location = new Point(6, 1122);
            pnlPago.Margin = new Padding(0, 0, 0, 5);
            pnlPago.Name = "pnlPago";
            pnlPago.Size = new Size(1318, 491);
            pnlPago.TabIndex = 5;
            // cbocantcuotas cbocantcuotasdropdownstyle: establece si la lista permite escribir o solo elegir elementos
            cboCantCuotas.DropDownStyle = ComboBoxStyle.DropDownList;
            cboCantCuotas.FormattingEnabled = true;
            cboCantCuotas.Location = new Point(26, 200);
            cboCantCuotas.Name = "cboCantCuotas";
            cboCantCuotas.Size = new Size(230, 23);
            cboCantCuotas.TabIndex = 19;
            cboCantCuotas.SelectedIndexChanged += cboCantCuotas_SelectedIndexChanged;
            // lblcuotaspago
            lblCuotasPago.AutoSize = true;
            lblCuotasPago.ForeColor = Color.White;
            lblCuotasPago.Location = new Point(20, 182);
            lblCuotasPago.Name = "lblCuotasPago";
            lblCuotasPago.Size = new Size(44, 15);
            lblCuotasPago.TabIndex = 18;
            lblCuotasPago.Text = "Cuotas";
            // lbltotalpagoactual lbltotalpagoactualanchor: bordes que mantienen su distancia cuando cambia el tamano del contenedor
            lblTotalPagoActual.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblTotalPagoActual.BackColor = Color.Transparent;
            lblTotalPagoActual.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTotalPagoActual.ForeColor = Color.White;
            lblTotalPagoActual.Location = new Point(15, 269);
            lblTotalPagoActual.Name = "lblTotalPagoActual";
            lblTotalPagoActual.Size = new Size(195, 26);
            lblTotalPagoActual.TabIndex = 14;
            lblTotalPagoActual.Text = "Total del pago actual: $ 0";
            // txtobservacionpago txtobservacionpagoanchor: bordes que mantienen su distancia cuando cambia el tamano del contenedor
            txtObservacionPago.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtObservacionPago.CharacterCasing = CharacterCasing.Upper;
            txtObservacionPago.Location = new Point(460, 78);
            txtObservacionPago.MaxLength = 150;
            txtObservacionPago.Name = "txtObservacionPago";
            txtObservacionPago.Size = new Size(798, 23);
            txtObservacionPago.TabIndex = 13;
            // lblobservacionpago
            lblObservacionPago.AutoSize = true;
            lblObservacionPago.Location = new Point(460, 55);
            lblObservacionPago.Name = "lblObservacionPago";
            lblObservacionPago.Size = new Size(73, 15);
            lblObservacionPago.TabIndex = 12;
            lblObservacionPago.Text = "Observación";
            // dtpfechapago dtpfechapagoformat: formato de presentacion, por ejemplo moneda o fecha; no modifica el valor original
            dtpFechaPago.Format = DateTimePickerFormat.Short;
            dtpFechaPago.Location = new Point(260, 78);
            dtpFechaPago.Name = "dtpFechaPago";
            dtpFechaPago.Size = new Size(180, 23);
            dtpFechaPago.TabIndex = 11;
            // lblfechapago
            lblFechaPago.AutoSize = true;
            lblFechaPago.Location = new Point(260, 55);
            lblFechaPago.Name = "lblFechaPago";
            lblFechaPago.Size = new Size(87, 15);
            lblFechaPago.TabIndex = 10;
            lblFechaPago.Text = "Fecha del pago";
            // cbocondicionpago cbocondicionpagodropdownstyle: establece si la lista permite escribir o solo elegir elementos
            cboCondicionPago.DropDownStyle = ComboBoxStyle.DropDownList;
            cboCondicionPago.FormattingEnabled = true;
            cboCondicionPago.Items.AddRange(new object[] { "Pago parcial", "Pago total" });
            cboCondicionPago.Location = new Point(20, 78);
            cboCondicionPago.Name = "cboCondicionPago";
            cboCondicionPago.Size = new Size(220, 23);
            cboCondicionPago.TabIndex = 9;
            cboCondicionPago.SelectedIndexChanged += cboCondicionPago_SelectedIndexChanged;
            // lblcondicionpago
            lblCondicionPago.AutoSize = true;
            lblCondicionPago.Location = new Point(20, 55);
            lblCondicionPago.Name = "lblCondicionPago";
            lblCondicionPago.Size = new Size(145, 15);
            lblCondicionPago.TabIndex = 8;
            lblCondicionPago.Text = "Condición del pago inicial";
            // dgvpagos
            dgvPagos.AllowUserToAddRows = false;
            dgvPagos.AllowUserToDeleteRows = false;
            dataGridViewCellStyle25.BackColor = Color.FromArgb(236, 242, 249);
            dgvPagos.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle25;
            dgvPagos.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            dgvPagos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPagos.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvPagos.BackgroundColor = Color.FromArgb(13, 19, 27);
            dgvPagos.BorderStyle = BorderStyle.None;
            dataGridViewCellStyle26.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle26.BackColor = Color.FromArgb(37, 52, 70);
            dataGridViewCellStyle26.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle26.ForeColor = Color.White;
            dataGridViewCellStyle26.SelectionBackColor = Color.FromArgb(190, 220, 250);
            dataGridViewCellStyle26.SelectionForeColor = Color.Black;
            dataGridViewCellStyle26.SelectionBackColor = Color.FromArgb(190, 220, 250);
            dataGridViewCellStyle26.WrapMode = DataGridViewTriState.True;
            dgvPagos.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle26;
            dgvPagos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvPagos.Columns.AddRange(new DataGridViewColumn[] { colMedioPago, colMontoPago, colRecargoPago, colTotalPago, colQuitarPago });
            dataGridViewCellStyle27.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle27.BackColor = Color.White;
            dataGridViewCellStyle27.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle27.ForeColor = Color.White;
            dataGridViewCellStyle27.Padding = new Padding(4);
            dataGridViewCellStyle27.SelectionBackColor = Color.FromArgb(190, 220, 250);
            dataGridViewCellStyle27.SelectionForeColor = Color.Black;
            dataGridViewCellStyle27.SelectionBackColor = Color.FromArgb(190, 220, 250);
            dataGridViewCellStyle27.WrapMode = DataGridViewTriState.True;
            dgvPagos.DefaultCellStyle = dataGridViewCellStyle27;
            dgvPagos.EnableHeadersVisualStyles = false;
            dgvPagos.Location = new Point(26, 298);
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
            dgvPagos.RowHeadersWidth = 62;
            dgvPagos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPagos.Size = new Size(1141, 108);
            dgvPagos.TabIndex = 7;
            dgvPagos.CellContentClick += dgvPagos_CellContentClick;
            // colmediopago colmediopagoheadertext: titulo de la columna; no es el nombre de la propiedad enlazada
            colMedioPago.HeaderText = "Medio de pago";
            colMedioPago.MinimumWidth = 8;
            colMedioPago.Name = "colMedioPago";
            colMedioPago.ReadOnly = true;
            // colmontopago colmontopagoheadertext: titulo de la columna; no es el nombre de la propiedad enlazada
            colMontoPago.HeaderText = "Monto";
            colMontoPago.MinimumWidth = 8;
            colMontoPago.Name = "colMontoPago";
            colMontoPago.ReadOnly = true;
            // colrecargopago colrecargopagoheadertext: titulo de la columna; no es el nombre de la propiedad enlazada
            colRecargoPago.HeaderText = "Recargo";
            colRecargoPago.MinimumWidth = 8;
            colRecargoPago.Name = "colRecargoPago";
            colRecargoPago.ReadOnly = true;
            // coltotalpago coltotalpagoheadertext: titulo de la columna; no es el nombre de la propiedad enlazada
            colTotalPago.HeaderText = "Total";
            colTotalPago.MinimumWidth = 8;
            colTotalPago.Name = "colTotalPago";
            colTotalPago.ReadOnly = true;
            // colquitarpago
            colQuitarPago.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            colQuitarPago.FlatStyle = FlatStyle.Flat;
            colQuitarPago.HeaderText = "Quitar";
            colQuitarPago.Text = "Quitar";
            colQuitarPago.MinimumWidth = 8;
            colQuitarPago.Name = "colQuitarPago";
            colQuitarPago.DefaultCellStyle.ForeColor = Color.Black;
            colQuitarPago.DefaultCellStyle.SelectionForeColor = Color.Black;
            colQuitarPago.DefaultCellStyle.BackColor = Color.Gainsboro;
            colQuitarPago.DefaultCellStyle.SelectionBackColor = Color.Gainsboro;
            colQuitarPago.ReadOnly = true;
            colQuitarPago.UseColumnTextForButtonValue = true;
            colQuitarPago.Width = 80;
            // btnagregarpago btnagregarpagobackcolor: color del fondo del control o de la celda
            btnAgregarPago.BackColor = Color.FromArgb(35, 135, 190);
            btnAgregarPago.FlatAppearance.BorderSize = 0;
            btnAgregarPago.FlatStyle = FlatStyle.Flat;
            btnAgregarPago.ForeColor = Color.White;
            btnAgregarPago.Location = new Point(505, 135);
            btnAgregarPago.Name = "btnAgregarPago";
            btnAgregarPago.Size = new Size(152, 30);
            btnAgregarPago.TabIndex = 6;
            btnAgregarPago.Text = "Agregar medio al pago";
            btnAgregarPago.UseVisualStyleBackColor = false;
            btnAgregarPago.Click += btnAgregarPago_Click;
            // nudmontopago
            nudMontoPago.DecimalPlaces = 2;
            nudMontoPago.Location = new Point(279, 140);
            nudMontoPago.Maximum = new decimal(new int[] { 100000000, 0, 0, 0 });
            nudMontoPago.Name = "nudMontoPago";
            nudMontoPago.Size = new Size(150, 23);
            nudMontoPago.TabIndex = 5;
            nudMontoPago.ThousandsSeparator = true;
            nudMontoPago.ValueChanged += nudMontoPago_ValueChanged;
            // lblmontopago
            lblMontoPago.AutoSize = true;
            lblMontoPago.ForeColor = Color.White;
            lblMontoPago.Location = new Point(279, 119);
            lblMontoPago.Name = "lblMontoPago";
            lblMontoPago.Size = new Size(43, 15);
            lblMontoPago.TabIndex = 4;
            lblMontoPago.Text = "Monto";
            // cbomediopago cbomediopagodropdownstyle: establece si la lista permite escribir o solo elegir elementos
            cboMedioPago.DropDownStyle = ComboBoxStyle.DropDownList;
            cboMedioPago.FormattingEnabled = true;
            cboMedioPago.Location = new Point(26, 139);
            cboMedioPago.Name = "cboMedioPago";
            cboMedioPago.Size = new Size(230, 23);
            cboMedioPago.TabIndex = 3;
            cboMedioPago.SelectedIndexChanged += cboMedioPago_SelectedIndexChanged;
            // lbltitulopago
            lblTituloPago.AutoSize = true;
            lblTituloPago.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTituloPago.ForeColor = Color.White;
            lblTituloPago.Location = new Point(20, 15);
            lblTituloPago.Name = "lblTituloPago";
            lblTituloPago.Size = new Size(66, 21);
            lblTituloPago.TabIndex = 0;
            lblTituloPago.Text = "5. Pago";
            // lblmediopago
            lblMedioPago.AutoSize = true;
            lblMedioPago.ForeColor = Color.White;
            lblMedioPago.Location = new Point(20, 119);
            lblMedioPago.Name = "lblMedioPago";
            lblMedioPago.Size = new Size(87, 15);
            lblMedioPago.TabIndex = 2;
            lblMedioPago.Text = "Medio de pago";
            // pnlpagina
            pnlPagina.AutoScroll = true;
            pnlPagina.BackColor = Color.FromArgb(13, 19, 27);
            pnlPagina.Controls.Add(tlpSecciones);
            pnlPagina.Dock = DockStyle.Fill;
            pnlPagina.ForeColor = Color.White;
            pnlPagina.Location = new Point(0, 0);
            pnlPagina.Margin = new Padding(2);
            pnlPagina.Name = "pnlPagina";
            pnlPagina.Size = new Size(1347, 594);
            pnlPagina.TabIndex = 0;
            // tlpsecciones
            tlpSecciones.AutoSize = true;
            tlpSecciones.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tlpSecciones.ColumnCount = 1;
            tlpSecciones.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpSecciones.Controls.Add(pnlEncabezadoVenta, 0, 0);
            tlpSecciones.Controls.Add(pnlCliente, 0, 1);
            tlpSecciones.Controls.Add(pnlProductos, 0, 2);
            tlpSecciones.Controls.Add(pnlFormaEntrega, 0, 3);
            tlpSecciones.Controls.Add(pnlDescuentoTotales, 0, 4);
            tlpSecciones.Controls.Add(pnlPago, 0, 5);
            tlpSecciones.Controls.Add(pnlDocumentos, 0, 6);
            tlpSecciones.Dock = DockStyle.Top;
            tlpSecciones.Location = new Point(0, 0);
            tlpSecciones.Margin = new Padding(2);
            tlpSecciones.MinimumSize = new Size(532, 0);
            tlpSecciones.Name = "tlpSecciones";
            tlpSecciones.Padding = new Padding(6, 5, 6, 5);
            tlpSecciones.RowCount = 7;
            tlpSecciones.RowStyles.Add(new RowStyle());
            tlpSecciones.RowStyles.Add(new RowStyle());
            tlpSecciones.RowStyles.Add(new RowStyle());
            tlpSecciones.RowStyles.Add(new RowStyle());
            tlpSecciones.RowStyles.Add(new RowStyle());
            tlpSecciones.RowStyles.Add(new RowStyle());
            tlpSecciones.RowStyles.Add(new RowStyle());
            tlpSecciones.Size = new Size(1330, 1752);
            tlpSecciones.TabIndex = 0;
            // chkincluirsincomprasrecientes
            chkIncluirSinComprasRecientes.AutoSize = true;
            chkIncluirSinComprasRecientes.Location = new Point(520, 40);
            chkIncluirSinComprasRecientes.Name = "chkIncluirSinComprasRecientes";
            chkIncluirSinComprasRecientes.Size = new Size(219, 19);
            chkIncluirSinComprasRecientes.TabIndex = 6;
            chkIncluirSinComprasRecientes.Text = "Incluir clientes sin compras recientes";
            chkIncluirSinComprasRecientes.UseVisualStyleBackColor = true;
            chkIncluirSinComprasRecientes.CheckedChanged += chkIncluirSinComprasRecientes_CheckedChanged;
            // frmnuevaventa
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(13, 19, 27);
            ClientSize = new Size(1347, 594);
            Controls.Add(pnlPagina);
            ForeColor = Color.White;
            Name = "frmNuevaVenta";
            Text = "frmNuevaVenta";
            Load += frmNuevaVenta_Load;
            pnlContenidoVenta.ResumeLayout(false);
            pnlContenidoVenta.PerformLayout();
            pnlDocumentos.ResumeLayout(false);
            pnlDocumentos.PerformLayout();
            pnlFormaEntrega.ResumeLayout(false);
            pnlFormaEntrega.PerformLayout();
            // llama a ((systemcomponentmodelisupportinitialize)nudcostoenvio)endinit para realizar este paso del flujo
            ((System.ComponentModel.ISupportInitialize)nudCostoEnvio).EndInit();
            pnlEncabezadoVenta.ResumeLayout(false);
            pnlEncabezadoVenta.PerformLayout();
            pnlCliente.ResumeLayout(false);
            pnlCliente.PerformLayout();
            // llama a ((systemcomponentmodelisupportinitialize)dgvclienteseleccionado)endinit para realizar este paso del flujo
            ((System.ComponentModel.ISupportInitialize)dgvClienteSeleccionado).EndInit();
            pnlProductos.ResumeLayout(false);
            pnlProductos.PerformLayout();
            // llama a ((systemcomponentmodelisupportinitialize)dgvdetalleventa)endinit para realizar este paso del flujo
            ((System.ComponentModel.ISupportInitialize)dgvDetalleVenta).EndInit();
            // llama a ((systemcomponentmodelisupportinitialize)nudcantidad)endinit para realizar este paso del flujo
            ((System.ComponentModel.ISupportInitialize)nudCantidad).EndInit();
            pnlDescuentoTotales.ResumeLayout(false);
            pnlDescuentoTotales.PerformLayout();
            // llama a ((systemcomponentmodelisupportinitialize)nuddescuento)endinit para realizar este paso del flujo
            ((System.ComponentModel.ISupportInitialize)nudDescuento).EndInit();
            pnlPago.ResumeLayout(false);
            pnlPago.PerformLayout();
            // llama a ((systemcomponentmodelisupportinitialize)dgvpagos)endinit para realizar este paso del flujo
            ((System.ComponentModel.ISupportInitialize)dgvPagos).EndInit();
            // llama a ((systemcomponentmodelisupportinitialize)nudmontopago)endinit para realizar este paso del flujo
            ((System.ComponentModel.ISupportInitialize)nudMontoPago).EndInit();
            pnlPagina.ResumeLayout(false);
            pnlPagina.PerformLayout();
            tlpSecciones.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private Label label1;
        private TableLayoutPanel pnlContenidoVenta;
        private Panel tlpNuevaVenta;
        private Panel pnlDocumentos;
        private Label lblTituloDocumentos;
        private Panel pnlFormaEntrega;
        private TextBox txtObservacionesEntrega;
        private Label lblObservacionesEntrega;
        private DateTimePicker dtpFechaEntrega;
        private Label lblFechaEntrega;
        private TextBox txtDomicilioEntrega;
        private Label lblDomicilioEntrega;
        private Label lblTituloFormaEntrega;
        private RadioButton rdoRetiroLocal;
        private Panel pnlEncabezadoVenta;
        private Label lblNumeroVenta;
        private Label lblTituloNuevaVenta;
        private Panel pnlCliente;
        private Label lblClienteSeleccionado;
        private ComboBox cboCliente;
        private Label lblTituloCliente;
        private Panel pnlProductos;
        private DataGridView dgvDetalleVenta;
        private DataGridViewTextBoxColumn colProducto;
        private DataGridViewTextBoxColumn colVariante;
        private DataGridViewTextBoxColumn colCantidadVenta;
        private DataGridViewTextBoxColumn colPrecioUnitario;
        private DataGridViewTextBoxColumn colSubtotal;
        private DataGridViewButtonColumn colEliminar;
        private DataGridViewTextBoxColumn colStockFisico;
        private DataGridViewTextBoxColumn colStockComprometido;
        private Button btnAgregarProducto;
        private NumericUpDown nudCantidad;
        private Button btnBuscarProducto;
        private ComboBox cboBuscarProducto;
        private Label lblTituloProductos;
        private Panel pnlDescuentoTotales;
        private Label lblTotal;
        private Label lblTotalTexto;
        private Label lblDescuentoAplicado;
        private Label lblDescuentoAplicadoTexto;
        private Label lblSubtotal;
        private Label lblSubtotalTexto;
        private Label lblEnvio;
        private Label lblEnvioTexto;
        private Label lblPorcentaje;
        private NumericUpDown nudDescuento;
        private Label lblDescuento;
        private Label lblTituloDescuento;
        private Panel pnlPago;
        private DataGridView dgvPagos;
        private DataGridViewTextBoxColumn colMedioPago;
        private DataGridViewTextBoxColumn colMontoPago;
        private DataGridViewTextBoxColumn colRecargoPago;
        private DataGridViewTextBoxColumn colTotalPago;
        private DataGridViewButtonColumn colQuitarPago;
        private Button btnAgregarPago;
        private NumericUpDown nudMontoPago;
        private Label lblMontoPago;
        private ComboBox cboMedioPago;
        private Label lblTituloPago;
        private Label lblMedioPago;
        private Label lblAclaracionDocumentos;
        private ComboBox cboTipoLogistica;
        private Label lblTipoLogistica;
        private TextBox txtHorarioSugerido;
        private Label lblHorarioSugerido;
        private RadioButton rdoEnvioDomicilio;
        private RadioButton rdoRetiroPosterior;
        private NumericUpDown nudCostoEnvio;
        private Label lblCostoEnvio;
        private Label lblFechaPago;
        private ComboBox cboCondicionPago;
        private Label lblCondicionPago;
        private TextBox txtObservacionPago;
        private Label lblObservacionPago;
        private DateTimePicker dtpFechaPago;
        private Label lblTotalPagoActual;
        private DataGridView dataGridView1;
        private Label lblPagosRegistrados;
        private Button btnRegistrarVenta;
        private DataGridView dgvClienteSeleccionado;
        private ComboBox cboCantCuotas;
        private Label lblCuotasPago;
        private Label lblCobroPrevisto;
        private CheckBox usarDireccionRegistrada;
        private TextBox txtCuentaDestino;
        private Label lblCuentaDestino;
        private Label label3;
        private TextBox txtSegundoTelefono;
        private Panel pnlPagina;
        private TableLayoutPanel tlpSecciones;
        private Label lblUsarDireccionRegistrada;
        private CheckBox chkIncluirSinComprasRecientes;
    }
}
