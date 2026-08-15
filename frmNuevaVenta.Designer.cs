namespace Prana.Presentacion
{
    partial class frmNuevaVenta
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            pnlContenidoVenta = new TableLayoutPanel();
            tlpNuevaVenta = new Panel();
            pnlDocumentos = new Panel();
            lblAclaracionDocumentos = new Label();
            chkGenerarRemito = new CheckBox();
            chkGenerarRecibo = new CheckBox();
            lblTituloDocumentos = new Label();
            pnlFormaEntrega = new Panel();
            numericUpDown1 = new NumericUpDown();
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
            textBox4 = new TextBox();
            textBox3 = new TextBox();
            textBox2 = new TextBox();
            textBox1 = new TextBox();
            label5 = new Label();
            label4 = new Label();
            label3 = new Label();
            label2 = new Label();
            BtnAgregarCliente = new FontAwesome.Sharp.IconButton();
            lblClienteSeleccionado = new Label();
            btnBuscarCliente = new FontAwesome.Sharp.IconButton();
            txtBuscarCliente = new TextBox();
            lblTituloCliente = new Label();
            pnlProductos = new Panel();
            dgvDetalleVenta = new DataGridView();
            colProducto = new DataGridViewTextBoxColumn();
            colVariante = new DataGridViewTextBoxColumn();
            colPrecioUnitario = new DataGridViewTextBoxColumn();
            colSubtotal = new DataGridViewTextBoxColumn();
            colEliminar = new DataGridViewButtonColumn();
            btnAgregarProducto = new Button();
            nudCantidad = new NumericUpDown();
            txtBuscarProducto = new TextBox();
            lblTituloProductos = new Label();
            pnlDescuentoTotales = new Panel();
            lblTotal = new Label();
            lblTotalTexto = new Label();
            lblDescuentoAplicado = new Label();
            lblDescuentoAplicadoTexto = new Label();
            lblSubtotal = new Label();
            lblSubtotalTexto = new Label();
            lblPorcentaje = new Label();
            nudDescuento = new NumericUpDown();
            lblDescuento = new Label();
            lblTituloDescuento = new Label();
            pnlPago = new Panel();
            numericUpDown2 = new NumericUpDown();
            btnConfirmarPagoActual = new Button();
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
            pnlContenidoVenta.SuspendLayout();
            tlpNuevaVenta.SuspendLayout();
            pnlDocumentos.SuspendLayout();
            pnlFormaEntrega.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).BeginInit();
            pnlEncabezadoVenta.SuspendLayout();
            pnlCliente.SuspendLayout();
            pnlProductos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvDetalleVenta).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudCantidad).BeginInit();
            pnlDescuentoTotales.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudDescuento).BeginInit();
            pnlPago.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvPagos).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudMontoPago).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(76, 33);
            label1.Name = "label1";
            label1.Size = new Size(82, 15);
            label1.TabIndex = 0;
            label1.Text = "NUEVA VENTA";
            // 
            // pnlContenidoVenta
            // 
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
            pnlContenidoVenta.RowStyles.Add(new RowStyle(SizeType.Absolute, 65F));
            pnlContenidoVenta.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            pnlContenidoVenta.RowStyles.Add(new RowStyle(SizeType.Absolute, 75F));
            pnlContenidoVenta.Size = new Size(1292, 1924);
            pnlContenidoVenta.TabIndex = 1;
            pnlContenidoVenta.Paint += pnlContenidoVenta_Paint;
            // 
            // tlpNuevaVenta
            // 
            tlpNuevaVenta.AutoScroll = true;
            tlpNuevaVenta.AutoSize = true;
            tlpNuevaVenta.BackColor = Color.FromArgb(13, 19, 27);
            tlpNuevaVenta.Controls.Add(pnlDocumentos);
            tlpNuevaVenta.Controls.Add(pnlFormaEntrega);
            tlpNuevaVenta.Controls.Add(pnlEncabezadoVenta);
            tlpNuevaVenta.Controls.Add(pnlCliente);
            tlpNuevaVenta.Controls.Add(pnlProductos);
            tlpNuevaVenta.Controls.Add(pnlDescuentoTotales);
            tlpNuevaVenta.Controls.Add(pnlPago);
            tlpNuevaVenta.Dock = DockStyle.Fill;
            tlpNuevaVenta.ForeColor = Color.White;
            tlpNuevaVenta.Location = new Point(3, 68);
            tlpNuevaVenta.Name = "tlpNuevaVenta";
            tlpNuevaVenta.Padding = new Padding(0, 10, 0, 10);
            tlpNuevaVenta.Size = new Size(1286, 1778);
            tlpNuevaVenta.TabIndex = 1;
            tlpNuevaVenta.Paint += panel1_Paint;
            // 
            // pnlDocumentos
            // 
            pnlDocumentos.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pnlDocumentos.BackColor = Color.FromArgb(21, 30, 41);
            pnlDocumentos.Controls.Add(lblAclaracionDocumentos);
            pnlDocumentos.Controls.Add(chkGenerarRemito);
            pnlDocumentos.Controls.Add(chkGenerarRecibo);
            pnlDocumentos.Controls.Add(lblTituloDocumentos);
            pnlDocumentos.Location = new Point(9, 1641);
            pnlDocumentos.Name = "pnlDocumentos";
            pnlDocumentos.Size = new Size(1683, 124);
            pnlDocumentos.TabIndex = 8;
            // 
            // lblAclaracionDocumentos
            // 
            lblAclaracionDocumentos.AutoSize = true;
            lblAclaracionDocumentos.ForeColor = Color.FromArgb(145, 170, 210);
            lblAclaracionDocumentos.Location = new Point(20, 100);
            lblAclaracionDocumentos.Name = "lblAclaracionDocumentos";
            lblAclaracionDocumentos.Size = new Size(359, 15);
            lblAclaracionDocumentos.TabIndex = 3;
            lblAclaracionDocumentos.Text = " Los documentos seleccionados se generarán al confirmar la venta.";
            // 
            // chkGenerarRemito
            // 
            chkGenerarRemito.AutoSize = true;
            chkGenerarRemito.Checked = true;
            chkGenerarRemito.CheckState = CheckState.Checked;
            chkGenerarRemito.Location = new Point(29, 58);
            chkGenerarRemito.Name = "chkGenerarRemito";
            chkGenerarRemito.Size = new Size(103, 19);
            chkGenerarRemito.TabIndex = 2;
            chkGenerarRemito.Text = "Generar recibo";
            chkGenerarRemito.UseVisualStyleBackColor = true;
            // 
            // chkGenerarRecibo
            // 
            chkGenerarRecibo.AutoSize = true;
            chkGenerarRecibo.Checked = true;
            chkGenerarRecibo.CheckState = CheckState.Checked;
            chkGenerarRecibo.Location = new Point(147, 58);
            chkGenerarRecibo.Name = "chkGenerarRecibo";
            chkGenerarRecibo.Size = new Size(103, 19);
            chkGenerarRecibo.TabIndex = 1;
            chkGenerarRecibo.Text = "Generar recibo";
            chkGenerarRecibo.UseVisualStyleBackColor = true;
            // 
            // lblTituloDocumentos
            // 
            lblTituloDocumentos.AutoSize = true;
            lblTituloDocumentos.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTituloDocumentos.ForeColor = Color.White;
            lblTituloDocumentos.Location = new Point(20, 15);
            lblTituloDocumentos.Name = "lblTituloDocumentos";
            lblTituloDocumentos.Size = new Size(124, 21);
            lblTituloDocumentos.TabIndex = 0;
            lblTituloDocumentos.Text = "6. Documentos";
            // 
            // pnlFormaEntrega
            // 
            pnlFormaEntrega.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pnlFormaEntrega.BackColor = Color.FromArgb(21, 30, 41);
            pnlFormaEntrega.Controls.Add(numericUpDown1);
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
            pnlFormaEntrega.Location = new Point(9, 1357);
            pnlFormaEntrega.Name = "pnlFormaEntrega";
            pnlFormaEntrega.Size = new Size(1683, 269);
            pnlFormaEntrega.TabIndex = 7;
            // 
            // numericUpDown1
            // 
            numericUpDown1.DecimalPlaces = 2;
            numericUpDown1.Enabled = false;
            numericUpDown1.Location = new Point(121, 229);
            numericUpDown1.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            numericUpDown1.Name = "numericUpDown1";
            numericUpDown1.Size = new Size(180, 23);
            numericUpDown1.TabIndex = 15;
            numericUpDown1.ThousandsSeparator = true;
            // 
            // lblCostoEnvio
            // 
            lblCostoEnvio.AutoSize = true;
            lblCostoEnvio.Location = new Point(20, 231);
            lblCostoEnvio.Name = "lblCostoEnvio";
            lblCostoEnvio.Size = new Size(89, 15);
            lblCostoEnvio.TabIndex = 14;
            lblCostoEnvio.Text = "Costo del envio";
            // 
            // rdoEnvioDomicilio
            // 
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
            // 
            // rdoRetiroPosterior
            // 
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
            rdoRetiroPosterior.CheckedChanged += radioButton1_CheckedChanged;
            // 
            // cboTipoLogistica
            // 
            cboTipoLogistica.DropDownStyle = ComboBoxStyle.DropDownList;
            cboTipoLogistica.FormattingEnabled = true;
            cboTipoLogistica.Items.AddRange(new object[] { "Logística propia", "Empresa externa" });
            cboTipoLogistica.Location = new Point(120, 189);
            cboTipoLogistica.Name = "cboTipoLogistica";
            cboTipoLogistica.Size = new Size(121, 23);
            cboTipoLogistica.TabIndex = 11;
            // 
            // lblTipoLogistica
            // 
            lblTipoLogistica.AutoSize = true;
            lblTipoLogistica.Location = new Point(20, 192);
            lblTipoLogistica.Name = "lblTipoLogistica";
            lblTipoLogistica.Size = new Size(93, 15);
            lblTipoLogistica.TabIndex = 10;
            lblTipoLogistica.Text = "Tipo de logística";
            // 
            // txtHorarioSugerido
            // 
            txtHorarioSugerido.Location = new Point(761, 118);
            txtHorarioSugerido.Name = "txtHorarioSugerido";
            txtHorarioSugerido.PlaceholderText = "Ej: tarde, de 14 a 18 hs";
            txtHorarioSugerido.Size = new Size(171, 23);
            txtHorarioSugerido.TabIndex = 9;
            // 
            // lblHorarioSugerido
            // 
            lblHorarioSugerido.AutoSize = true;
            lblHorarioSugerido.Location = new Point(761, 95);
            lblHorarioSugerido.Name = "lblHorarioSugerido";
            lblHorarioSugerido.Size = new Size(96, 15);
            lblHorarioSugerido.TabIndex = 8;
            lblHorarioSugerido.Text = "Horario sugerido";
            // 
            // txtObservacionesEntrega
            // 
            txtObservacionesEntrega.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtObservacionesEntrega.Location = new Point(120, 152);
            txtObservacionesEntrega.Name = "txtObservacionesEntrega";
            txtObservacionesEntrega.Size = new Size(1417, 23);
            txtObservacionesEntrega.TabIndex = 7;
            // 
            // lblObservacionesEntrega
            // 
            lblObservacionesEntrega.AutoSize = true;
            lblObservacionesEntrega.ForeColor = Color.White;
            lblObservacionesEntrega.Location = new Point(20, 155);
            lblObservacionesEntrega.Name = "lblObservacionesEntrega";
            lblObservacionesEntrega.Size = new Size(84, 15);
            lblObservacionesEntrega.TabIndex = 6;
            lblObservacionesEntrega.Text = "Observaciones";
            // 
            // dtpFechaEntrega
            // 
            dtpFechaEntrega.Enabled = false;
            dtpFechaEntrega.Format = DateTimePickerFormat.Short;
            dtpFechaEntrega.Location = new Point(490, 118);
            dtpFechaEntrega.Name = "dtpFechaEntrega";
            dtpFechaEntrega.Size = new Size(245, 23);
            dtpFechaEntrega.TabIndex = 5;
            // 
            // lblFechaEntrega
            // 
            lblFechaEntrega.AutoSize = true;
            lblFechaEntrega.ForeColor = Color.White;
            lblFechaEntrega.Location = new Point(490, 95);
            lblFechaEntrega.Name = "lblFechaEntrega";
            lblFechaEntrega.Size = new Size(89, 15);
            lblFechaEntrega.TabIndex = 4;
            lblFechaEntrega.Text = "Fecha estimada";
            // 
            // txtDomicilioEntrega
            // 
            txtDomicilioEntrega.Enabled = false;
            txtDomicilioEntrega.Location = new Point(20, 118);
            txtDomicilioEntrega.Name = "txtDomicilioEntrega";
            txtDomicilioEntrega.Size = new Size(450, 23);
            txtDomicilioEntrega.TabIndex = 3;
            // 
            // lblDomicilioEntrega
            // 
            lblDomicilioEntrega.AutoSize = true;
            lblDomicilioEntrega.ForeColor = Color.White;
            lblDomicilioEntrega.Location = new Point(20, 95);
            lblDomicilioEntrega.Name = "lblDomicilioEntrega";
            lblDomicilioEntrega.Size = new Size(117, 15);
            lblDomicilioEntrega.TabIndex = 2;
            lblDomicilioEntrega.Text = "Domicilio de entrega";
            // 
            // lblTituloFormaEntrega
            // 
            lblTituloFormaEntrega.AutoSize = true;
            lblTituloFormaEntrega.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTituloFormaEntrega.ForeColor = Color.White;
            lblTituloFormaEntrega.Location = new Point(20, 15);
            lblTituloFormaEntrega.Name = "lblTituloFormaEntrega";
            lblTituloFormaEntrega.Size = new Size(161, 21);
            lblTituloFormaEntrega.TabIndex = 1;
            lblTituloFormaEntrega.Text = "5. Forma de entrega";
            // 
            // rdoRetiroLocal
            // 
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
            // 
            // pnlEncabezadoVenta
            // 
            pnlEncabezadoVenta.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pnlEncabezadoVenta.BackColor = Color.FromArgb(21, 30, 41);
            pnlEncabezadoVenta.Controls.Add(lblNumeroVenta);
            pnlEncabezadoVenta.Controls.Add(lblTituloNuevaVenta);
            pnlEncabezadoVenta.Location = new Point(3, 0);
            pnlEncabezadoVenta.Name = "pnlEncabezadoVenta";
            pnlEncabezadoVenta.Size = new Size(1683, 65);
            pnlEncabezadoVenta.TabIndex = 0;
            // 
            // lblNumeroVenta
            // 
            lblNumeroVenta.AutoSize = true;
            lblNumeroVenta.ForeColor = Color.FromArgb(145, 170, 210);
            lblNumeroVenta.Location = new Point(17, 42);
            lblNumeroVenta.Name = "lblNumeroVenta";
            lblNumeroVenta.Size = new Size(56, 15);
            lblNumeroVenta.TabIndex = 1;
            lblNumeroVenta.Text = "Venta N.º";
            // 
            // lblTituloNuevaVenta
            // 
            lblTituloNuevaVenta.AutoSize = true;
            lblTituloNuevaVenta.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTituloNuevaVenta.ForeColor = Color.White;
            lblTituloNuevaVenta.Location = new Point(15, 10);
            lblTituloNuevaVenta.Name = "lblTituloNuevaVenta";
            lblTituloNuevaVenta.Size = new Size(158, 32);
            lblTituloNuevaVenta.TabIndex = 0;
            lblTituloNuevaVenta.Text = "Nueva Venta";
            // 
            // pnlCliente
            // 
            pnlCliente.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pnlCliente.BackColor = Color.FromArgb(21, 30, 41);
            pnlCliente.Controls.Add(textBox4);
            pnlCliente.Controls.Add(textBox3);
            pnlCliente.Controls.Add(textBox2);
            pnlCliente.Controls.Add(textBox1);
            pnlCliente.Controls.Add(label5);
            pnlCliente.Controls.Add(label4);
            pnlCliente.Controls.Add(label3);
            pnlCliente.Controls.Add(label2);
            pnlCliente.Controls.Add(BtnAgregarCliente);
            pnlCliente.Controls.Add(lblClienteSeleccionado);
            pnlCliente.Controls.Add(btnBuscarCliente);
            pnlCliente.Controls.Add(txtBuscarCliente);
            pnlCliente.Controls.Add(lblTituloCliente);
            pnlCliente.ForeColor = Color.White;
            pnlCliente.Location = new Point(3, 71);
            pnlCliente.Name = "pnlCliente";
            pnlCliente.Size = new Size(1683, 256);
            pnlCliente.TabIndex = 0;
            // 
            // textBox4
            // 
            textBox4.Font = new Font("Segoe UI", 10F);
            textBox4.Location = new Point(263, 196);
            textBox4.Name = "textBox4";
            textBox4.Size = new Size(207, 25);
            textBox4.TabIndex = 12;
            // 
            // textBox3
            // 
            textBox3.Font = new Font("Segoe UI", 10F);
            textBox3.Location = new Point(20, 196);
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(207, 25);
            textBox3.TabIndex = 11;
            // 
            // textBox2
            // 
            textBox2.Font = new Font("Segoe UI", 10F);
            textBox2.Location = new Point(263, 121);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(207, 25);
            textBox2.TabIndex = 10;
            // 
            // textBox1
            // 
            textBox1.Font = new Font("Segoe UI", 10F);
            textBox1.Location = new Point(20, 121);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(207, 25);
            textBox1.TabIndex = 9;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(270, 103);
            label5.Name = "label5";
            label5.Size = new Size(27, 15);
            label5.TabIndex = 8;
            label5.Text = "DNI";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(20, 178);
            label4.Name = "label4";
            label4.Size = new Size(57, 15);
            label4.TabIndex = 7;
            label4.Text = "Direccion";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(270, 178);
            label3.Name = "label3";
            label3.Size = new Size(52, 15);
            label3.TabIndex = 6;
            label3.Text = "Telefono";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(15, 103);
            label2.Name = "label2";
            label2.Size = new Size(107, 15);
            label2.TabIndex = 5;
            label2.Text = "Nombre y Apellido";
            // 
            // BtnAgregarCliente
            // 
            BtnAgregarCliente.BackColor = Color.DodgerBlue;
            BtnAgregarCliente.FlatAppearance.BorderSize = 0;
            BtnAgregarCliente.FlatStyle = FlatStyle.Flat;
            BtnAgregarCliente.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            BtnAgregarCliente.IconChar = FontAwesome.Sharp.IconChar.None;
            BtnAgregarCliente.IconColor = Color.Black;
            BtnAgregarCliente.IconFont = FontAwesome.Sharp.IconFont.Auto;
            BtnAgregarCliente.Location = new Point(603, 36);
            BtnAgregarCliente.Name = "BtnAgregarCliente";
            BtnAgregarCliente.Size = new Size(100, 25);
            BtnAgregarCliente.TabIndex = 4;
            BtnAgregarCliente.Text = "Agregar";
            BtnAgregarCliente.UseVisualStyleBackColor = false;
            // 
            // lblClienteSeleccionado
            // 
            lblClienteSeleccionado.AutoSize = true;
            lblClienteSeleccionado.ForeColor = Color.FromArgb(145, 170, 210);
            lblClienteSeleccionado.Location = new Point(20, 64);
            lblClienteSeleccionado.Name = "lblClienteSeleccionado";
            lblClienteSeleccionado.Size = new Size(157, 15);
            lblClienteSeleccionado.TabIndex = 3;
            lblClienteSeleccionado.Text = "Ningún cliente seleccionado";
            // 
            // btnBuscarCliente
            // 
            btnBuscarCliente.BackColor = Color.FromArgb(35, 135, 190);
            btnBuscarCliente.FlatAppearance.BorderSize = 0;
            btnBuscarCliente.FlatStyle = FlatStyle.Flat;
            btnBuscarCliente.IconChar = FontAwesome.Sharp.IconChar.None;
            btnBuscarCliente.IconColor = Color.Black;
            btnBuscarCliente.IconFont = FontAwesome.Sharp.IconFont.Auto;
            btnBuscarCliente.Location = new Point(486, 36);
            btnBuscarCliente.Name = "btnBuscarCliente";
            btnBuscarCliente.Size = new Size(100, 25);
            btnBuscarCliente.TabIndex = 2;
            btnBuscarCliente.Text = "Buscar";
            btnBuscarCliente.UseVisualStyleBackColor = false;
            // 
            // txtBuscarCliente
            // 
            txtBuscarCliente.Font = new Font("Segoe UI", 10F);
            txtBuscarCliente.Location = new Point(20, 36);
            txtBuscarCliente.Name = "txtBuscarCliente";
            txtBuscarCliente.PlaceholderText = "Buscar por DNI, nombre o apellido...";
            txtBuscarCliente.Size = new Size(460, 25);
            txtBuscarCliente.TabIndex = 1;
            // 
            // lblTituloCliente
            // 
            lblTituloCliente.AutoSize = true;
            lblTituloCliente.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTituloCliente.ForeColor = Color.White;
            lblTituloCliente.Location = new Point(15, 12);
            lblTituloCliente.Name = "lblTituloCliente";
            lblTituloCliente.Size = new Size(81, 21);
            lblTituloCliente.TabIndex = 0;
            lblTituloCliente.Text = "1. Cliente";
            // 
            // pnlProductos
            // 
            pnlProductos.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pnlProductos.BackColor = Color.FromArgb(21, 30, 41);
            pnlProductos.Controls.Add(dgvDetalleVenta);
            pnlProductos.Controls.Add(btnAgregarProducto);
            pnlProductos.Controls.Add(nudCantidad);
            pnlProductos.Controls.Add(txtBuscarProducto);
            pnlProductos.Controls.Add(lblTituloProductos);
            pnlProductos.Location = new Point(3, 354);
            pnlProductos.Name = "pnlProductos";
            pnlProductos.Size = new Size(1683, 273);
            pnlProductos.TabIndex = 1;
            // 
            // dgvDetalleVenta
            // 
            dgvDetalleVenta.AllowUserToAddRows = false;
            dgvDetalleVenta.AllowUserToDeleteRows = false;
            dgvDetalleVenta.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvDetalleVenta.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvDetalleVenta.BackgroundColor = SystemColors.WindowFrame;
            dgvDetalleVenta.BorderStyle = BorderStyle.None;
            dgvDetalleVenta.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvDetalleVenta.Columns.AddRange(new DataGridViewColumn[] { colProducto, colVariante, colPrecioUnitario, colSubtotal, colEliminar });
            dgvDetalleVenta.Location = new Point(20, 123);
            dgvDetalleVenta.MultiSelect = false;
            dgvDetalleVenta.Name = "dgvDetalleVenta";
            dgvDetalleVenta.ReadOnly = true;
            dgvDetalleVenta.RowHeadersVisible = false;
            dgvDetalleVenta.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDetalleVenta.Size = new Size(1523, 138);
            dgvDetalleVenta.TabIndex = 5;
            dgvDetalleVenta.CellContentClick += dgvDetalleVenta_CellContentClick;
            // 
            // colProducto
            // 
            colProducto.HeaderText = "Producto";
            colProducto.Name = "colProducto";
            colProducto.ReadOnly = true;
            // 
            // colVariante
            // 
            colVariante.HeaderText = "Variante";
            colVariante.Name = "colVariante";
            colVariante.ReadOnly = true;
            // 
            // colPrecioUnitario
            // 
            colPrecioUnitario.HeaderText = "Precio unitario";
            colPrecioUnitario.Name = "colPrecioUnitario";
            colPrecioUnitario.ReadOnly = true;
            // 
            // colSubtotal
            // 
            colSubtotal.HeaderText = "Subtotal";
            colSubtotal.Name = "colSubtotal";
            colSubtotal.ReadOnly = true;
            // 
            // colEliminar
            // 
            colEliminar.HeaderText = "Eliminar";
            colEliminar.Name = "colEliminar";
            colEliminar.ReadOnly = true;
            colEliminar.Resizable = DataGridViewTriState.True;
            colEliminar.SortMode = DataGridViewColumnSortMode.Automatic;
            // 
            // btnAgregarProducto
            // 
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
            // 
            // nudCantidad
            // 
            nudCantidad.Location = new Point(476, 50);
            nudCantidad.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            nudCantidad.Name = "nudCantidad";
            nudCantidad.Size = new Size(70, 23);
            nudCantidad.TabIndex = 3;
            nudCantidad.TextAlign = HorizontalAlignment.Center;
            nudCantidad.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // txtBuscarProducto
            // 
            txtBuscarProducto.Font = new Font("Segoe UI", 10F);
            txtBuscarProducto.Location = new Point(20, 50);
            txtBuscarProducto.Name = "txtBuscarProducto";
            txtBuscarProducto.PlaceholderText = " Buscar por código, producto o marca...";
            txtBuscarProducto.Size = new Size(420, 25);
            txtBuscarProducto.TabIndex = 1;
            // 
            // lblTituloProductos
            // 
            lblTituloProductos.AutoSize = true;
            lblTituloProductos.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTituloProductos.ForeColor = Color.White;
            lblTituloProductos.Location = new Point(20, 15);
            lblTituloProductos.Name = "lblTituloProductos";
            lblTituloProductos.Size = new Size(104, 21);
            lblTituloProductos.TabIndex = 0;
            lblTituloProductos.Text = "2. Productos";
            // 
            // pnlDescuentoTotales
            // 
            pnlDescuentoTotales.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pnlDescuentoTotales.BackColor = Color.FromArgb(21, 30, 41);
            pnlDescuentoTotales.Controls.Add(lblTotal);
            pnlDescuentoTotales.Controls.Add(lblTotalTexto);
            pnlDescuentoTotales.Controls.Add(lblDescuentoAplicado);
            pnlDescuentoTotales.Controls.Add(lblDescuentoAplicadoTexto);
            pnlDescuentoTotales.Controls.Add(lblSubtotal);
            pnlDescuentoTotales.Controls.Add(lblSubtotalTexto);
            pnlDescuentoTotales.Controls.Add(lblPorcentaje);
            pnlDescuentoTotales.Controls.Add(nudDescuento);
            pnlDescuentoTotales.Controls.Add(lblDescuento);
            pnlDescuentoTotales.Controls.Add(lblTituloDescuento);
            pnlDescuentoTotales.Location = new Point(3, 646);
            pnlDescuentoTotales.Name = "pnlDescuentoTotales";
            pnlDescuentoTotales.Size = new Size(1683, 146);
            pnlDescuentoTotales.TabIndex = 0;
            pnlDescuentoTotales.Paint += panel1_Paint_2;
            // 
            // lblTotal
            // 
            lblTotal.AutoSize = true;
            lblTotal.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTotal.ForeColor = Color.White;
            lblTotal.Location = new Point(665, 95);
            lblTotal.Name = "lblTotal";
            lblTotal.Size = new Size(61, 25);
            lblTotal.TabIndex = 9;
            lblTotal.Text = "$0,00";
            // 
            // lblTotalTexto
            // 
            lblTotalTexto.AutoSize = true;
            lblTotalTexto.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTotalTexto.ForeColor = Color.White;
            lblTotalTexto.Location = new Point(505, 98);
            lblTotalTexto.Name = "lblTotalTexto";
            lblTotalTexto.Size = new Size(60, 21);
            lblTotalTexto.TabIndex = 8;
            lblTotalTexto.Text = "TOTAL:";
            // 
            // lblDescuentoAplicado
            // 
            lblDescuentoAplicado.AutoSize = true;
            lblDescuentoAplicado.ForeColor = Color.White;
            lblDescuentoAplicado.Location = new Point(680, 58);
            lblDescuentoAplicado.Name = "lblDescuentoAplicado";
            lblDescuentoAplicado.Size = new Size(42, 15);
            lblDescuentoAplicado.TabIndex = 7;
            lblDescuentoAplicado.Text = "- $0,00";
            // 
            // lblDescuentoAplicadoTexto
            // 
            lblDescuentoAplicadoTexto.AutoSize = true;
            lblDescuentoAplicadoTexto.ForeColor = Color.White;
            lblDescuentoAplicadoTexto.Location = new Point(505, 58);
            lblDescuentoAplicadoTexto.Name = "lblDescuentoAplicadoTexto";
            lblDescuentoAplicadoTexto.Size = new Size(66, 15);
            lblDescuentoAplicadoTexto.TabIndex = 6;
            lblDescuentoAplicadoTexto.Text = "Descuento:";
            // 
            // lblSubtotal
            // 
            lblSubtotal.AutoSize = true;
            lblSubtotal.ForeColor = Color.White;
            lblSubtotal.Location = new Point(680, 25);
            lblSubtotal.Name = "lblSubtotal";
            lblSubtotal.Size = new Size(34, 15);
            lblSubtotal.TabIndex = 5;
            lblSubtotal.Text = "$0,00";
            // 
            // lblSubtotalTexto
            // 
            lblSubtotalTexto.AutoSize = true;
            lblSubtotalTexto.ForeColor = Color.White;
            lblSubtotalTexto.Location = new Point(505, 25);
            lblSubtotalTexto.Name = "lblSubtotalTexto";
            lblSubtotalTexto.Size = new Size(54, 15);
            lblSubtotalTexto.TabIndex = 4;
            lblSubtotalTexto.Text = "Subtotal:";
            // 
            // lblPorcentaje
            // 
            lblPorcentaje.AutoSize = true;
            lblPorcentaje.ForeColor = Color.White;
            lblPorcentaje.Location = new Point(90, 65);
            lblPorcentaje.Name = "lblPorcentaje";
            lblPorcentaje.Size = new Size(13, 15);
            lblPorcentaje.TabIndex = 3;
            lblPorcentaje.Text = "$";
            // 
            // nudDescuento
            // 
            nudDescuento.DecimalPlaces = 2;
            nudDescuento.Location = new Point(105, 61);
            nudDescuento.Maximum = new decimal(new int[] { 100000000, 0, 0, 0 });
            nudDescuento.Name = "nudDescuento";
            nudDescuento.Size = new Size(75, 23);
            nudDescuento.TabIndex = 2;
            nudDescuento.ThousandsSeparator = true;
            // 
            // lblDescuento
            // 
            lblDescuento.AutoSize = true;
            lblDescuento.ForeColor = Color.White;
            lblDescuento.Location = new Point(20, 65);
            lblDescuento.Name = "lblDescuento";
            lblDescuento.Size = new Size(66, 15);
            lblDescuento.TabIndex = 1;
            lblDescuento.Text = "Descuento:\n";
            // 
            // lblTituloDescuento
            // 
            lblTituloDescuento.AutoSize = true;
            lblTituloDescuento.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTituloDescuento.ForeColor = Color.White;
            lblTituloDescuento.Location = new Point(20, 15);
            lblTituloDescuento.Name = "lblTituloDescuento";
            lblTituloDescuento.Size = new Size(177, 21);
            lblTituloDescuento.TabIndex = 0;
            lblTituloDescuento.Text = "3. Descuento y totales";
            // 
            // pnlPago
            // 
            pnlPago.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pnlPago.BackColor = Color.FromArgb(21, 30, 41);
            pnlPago.Controls.Add(numericUpDown2);
            pnlPago.Controls.Add(btnConfirmarPagoActual);
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
            pnlPago.Location = new Point(3, 802);
            pnlPago.Name = "pnlPago";
            pnlPago.Size = new Size(1683, 546);
            pnlPago.TabIndex = 6;
            // 
            // numericUpDown2
            // 
            numericUpDown2.Location = new Point(445, 140);
            numericUpDown2.Maximum = new decimal(new int[] { 12, 0, 0, 0 });
            numericUpDown2.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numericUpDown2.Name = "numericUpDown2";
            numericUpDown2.Size = new Size(45, 23);
            numericUpDown2.TabIndex = 17;
            numericUpDown2.ThousandsSeparator = true;
            numericUpDown2.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // btnConfirmarPagoActual
            // 
            btnConfirmarPagoActual.BackColor = Color.FromArgb(35, 135, 190);
            btnConfirmarPagoActual.FlatAppearance.BorderSize = 0;
            btnConfirmarPagoActual.FlatStyle = FlatStyle.Flat;
            btnConfirmarPagoActual.ForeColor = Color.White;
            btnConfirmarPagoActual.Location = new Point(255, 272);
            btnConfirmarPagoActual.Name = "btnConfirmarPagoActual";
            btnConfirmarPagoActual.Size = new Size(185, 30);
            btnConfirmarPagoActual.TabIndex = 16;
            btnConfirmarPagoActual.Text = "Confirmar pago actual";
            btnConfirmarPagoActual.UseVisualStyleBackColor = false;
            // 
            // lblTotalPagoActual
            // 
            lblTotalPagoActual.AutoSize = true;
            lblTotalPagoActual.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTotalPagoActual.Location = new Point(36, 276);
            lblTotalPagoActual.Name = "lblTotalPagoActual";
            lblTotalPagoActual.Size = new Size(184, 20);
            lblTotalPagoActual.TabIndex = 14;
            lblTotalPagoActual.Text = "Total del pago actual: $ 0";
            // 
            // txtObservacionPago
            // 
            txtObservacionPago.Location = new Point(460, 78);
            txtObservacionPago.Name = "txtObservacionPago";
            txtObservacionPago.Size = new Size(258, 23);
            txtObservacionPago.TabIndex = 13;
            // 
            // lblObservacionPago
            // 
            lblObservacionPago.AutoSize = true;
            lblObservacionPago.Location = new Point(460, 55);
            lblObservacionPago.Name = "lblObservacionPago";
            lblObservacionPago.Size = new Size(73, 15);
            lblObservacionPago.TabIndex = 12;
            lblObservacionPago.Text = "Observación";
            // 
            // dtpFechaPago
            // 
            dtpFechaPago.Format = DateTimePickerFormat.Short;
            dtpFechaPago.Location = new Point(260, 78);
            dtpFechaPago.Name = "dtpFechaPago";
            dtpFechaPago.Size = new Size(180, 23);
            dtpFechaPago.TabIndex = 11;
            // 
            // lblFechaPago
            // 
            lblFechaPago.AutoSize = true;
            lblFechaPago.Location = new Point(260, 55);
            lblFechaPago.Name = "lblFechaPago";
            lblFechaPago.Size = new Size(87, 15);
            lblFechaPago.TabIndex = 10;
            lblFechaPago.Text = "Fecha del pago";
            // 
            // cboCondicionPago
            // 
            cboCondicionPago.DropDownStyle = ComboBoxStyle.DropDownList;
            cboCondicionPago.FormattingEnabled = true;
            cboCondicionPago.Items.AddRange(new object[] { "Seña para reserva", "Pago parcial", "Pago total" });
            cboCondicionPago.Location = new Point(20, 78);
            cboCondicionPago.Name = "cboCondicionPago";
            cboCondicionPago.Size = new Size(220, 23);
            cboCondicionPago.TabIndex = 9;
            cboCondicionPago.SelectedIndexChanged += cboCondicionPago_SelectedIndexChanged;
            // 
            // lblCondicionPago
            // 
            lblCondicionPago.AutoSize = true;
            lblCondicionPago.Location = new Point(20, 55);
            lblCondicionPago.Name = "lblCondicionPago";
            lblCondicionPago.Size = new Size(145, 15);
            lblCondicionPago.TabIndex = 8;
            lblCondicionPago.Text = "Condición del pago inicial";
            // 
            // dgvPagos
            // 
            dgvPagos.AllowUserToAddRows = false;
            dgvPagos.AllowUserToDeleteRows = false;
            dgvPagos.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            dgvPagos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPagos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvPagos.Columns.AddRange(new DataGridViewColumn[] { colMedioPago, colMontoPago, colRecargoPago, colTotalPago, colQuitarPago });
            dgvPagos.Location = new Point(20, 180);
            dgvPagos.Name = "dgvPagos";
            dgvPagos.ReadOnly = true;
            dgvPagos.RowHeadersVisible = false;
            dgvPagos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPagos.Size = new Size(1506, 74);
            dgvPagos.TabIndex = 7;
            // 
            // colMedioPago
            // 
            colMedioPago.HeaderText = "Medio de pago";
            colMedioPago.Name = "colMedioPago";
            colMedioPago.ReadOnly = true;
            // 
            // colMontoPago
            // 
            colMontoPago.HeaderText = "Monto";
            colMontoPago.Name = "colMontoPago";
            colMontoPago.ReadOnly = true;
            // 
            // colRecargoPago
            // 
            colRecargoPago.HeaderText = "Recargo";
            colRecargoPago.Name = "colRecargoPago";
            colRecargoPago.ReadOnly = true;
            // 
            // colTotalPago
            // 
            colTotalPago.HeaderText = "Total";
            colTotalPago.Name = "colTotalPago";
            colTotalPago.ReadOnly = true;
            // 
            // colQuitarPago
            // 
            colQuitarPago.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            colQuitarPago.FlatStyle = FlatStyle.Flat;
            colQuitarPago.HeaderText = "Quitar";
            colQuitarPago.Name = "colQuitarPago";
            colQuitarPago.ReadOnly = true;
            colQuitarPago.UseColumnTextForButtonValue = true;
            colQuitarPago.Width = 70;
            // 
            // btnAgregarPago
            // 
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
            // 
            // nudMontoPago
            // 
            nudMontoPago.DecimalPlaces = 2;
            nudMontoPago.Location = new Point(279, 140);
            nudMontoPago.Maximum = new decimal(new int[] { 100000000, 0, 0, 0 });
            nudMontoPago.Name = "nudMontoPago";
            nudMontoPago.Size = new Size(150, 23);
            nudMontoPago.TabIndex = 5;
            nudMontoPago.ThousandsSeparator = true;
            // 
            // lblMontoPago
            // 
            lblMontoPago.AutoSize = true;
            lblMontoPago.ForeColor = Color.White;
            lblMontoPago.Location = new Point(279, 119);
            lblMontoPago.Name = "lblMontoPago";
            lblMontoPago.Size = new Size(43, 15);
            lblMontoPago.TabIndex = 4;
            lblMontoPago.Text = "Monto";
            // 
            // cboMedioPago
            // 
            cboMedioPago.DropDownStyle = ComboBoxStyle.DropDownList;
            cboMedioPago.FormattingEnabled = true;
            cboMedioPago.Location = new Point(20, 140);
            cboMedioPago.Name = "cboMedioPago";
            cboMedioPago.Size = new Size(230, 23);
            cboMedioPago.TabIndex = 3;
            // 
            // lblTituloPago
            // 
            lblTituloPago.AutoSize = true;
            lblTituloPago.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTituloPago.ForeColor = Color.White;
            lblTituloPago.Location = new Point(20, 15);
            lblTituloPago.Name = "lblTituloPago";
            lblTituloPago.Size = new Size(66, 21);
            lblTituloPago.TabIndex = 0;
            lblTituloPago.Text = "4. Pago";
            // 
            // lblMedioPago
            // 
            lblMedioPago.AutoSize = true;
            lblMedioPago.ForeColor = Color.White;
            lblMedioPago.Location = new Point(20, 119);
            lblMedioPago.Name = "lblMedioPago";
            lblMedioPago.Size = new Size(87, 15);
            lblMedioPago.TabIndex = 2;
            lblMedioPago.Text = "Medio de pago";
            // 
            // frmNuevaVenta
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            ClientSize = new Size(1377, 594);
            Controls.Add(pnlContenidoVenta);
            Controls.Add(label1);
            Name = "frmNuevaVenta";
            Text = "frmNuevaVenta";
            Load += frmNuevaVenta_Load;
            pnlContenidoVenta.ResumeLayout(false);
            pnlContenidoVenta.PerformLayout();
            tlpNuevaVenta.ResumeLayout(false);
            pnlDocumentos.ResumeLayout(false);
            pnlDocumentos.PerformLayout();
            pnlFormaEntrega.ResumeLayout(false);
            pnlFormaEntrega.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).EndInit();
            pnlEncabezadoVenta.ResumeLayout(false);
            pnlEncabezadoVenta.PerformLayout();
            pnlCliente.ResumeLayout(false);
            pnlCliente.PerformLayout();
            pnlProductos.ResumeLayout(false);
            pnlProductos.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvDetalleVenta).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudCantidad).EndInit();
            pnlDescuentoTotales.ResumeLayout(false);
            pnlDescuentoTotales.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudDescuento).EndInit();
            pnlPago.ResumeLayout(false);
            pnlPago.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown2).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvPagos).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudMontoPago).EndInit();
            ResumeLayout(false);
            PerformLayout();
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
        private FontAwesome.Sharp.IconButton btnBuscarCliente;
        private TextBox txtBuscarCliente;
        private Label lblTituloCliente;
        private Panel pnlProductos;
        private DataGridView dgvDetalleVenta;
        private DataGridViewTextBoxColumn colProducto;
        private DataGridViewTextBoxColumn colVariante;
        private DataGridViewTextBoxColumn colPrecioUnitario;
        private DataGridViewTextBoxColumn colSubtotal;
        private DataGridViewButtonColumn colEliminar;
        private Button btnAgregarProducto;
        private NumericUpDown nudCantidad;
        private Button btnBuscarProducto;
        private TextBox txtBuscarProducto;
        private Label lblTituloProductos;
        private Panel pnlDescuentoTotales;
        private Label lblTotal;
        private Label lblTotalTexto;
        private Label lblDescuentoAplicado;
        private Label lblDescuentoAplicadoTexto;
        private Label lblSubtotal;
        private Label lblSubtotalTexto;
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
        private CheckBox chkGenerarRecibo;
        private CheckBox chkGenerarRemito;
        private Label lblAclaracionDocumentos;
        private ComboBox cboTipoLogistica;
        private Label lblTipoLogistica;
        private TextBox txtHorarioSugerido;
        private Label lblHorarioSugerido;
        private RadioButton rdoEnvioDomicilio;
        private RadioButton rdoRetiroPosterior;
        private NumericUpDown numericUpDown1;
        private Label lblCostoEnvio;
        private FontAwesome.Sharp.IconButton BtnAgregarCliente;
        private Label label5;
        private Label label4;
        private Label label3;
        private Label label2;
        private TextBox textBox4;
        private TextBox textBox3;
        private TextBox textBox2;
        private TextBox textBox1;
        private Label lblFechaPago;
        private ComboBox cboCondicionPago;
        private Label lblCondicionPago;
        private TextBox txtObservacionPago;
        private Label lblObservacionPago;
        private DateTimePicker dtpFechaPago;
        private Label lblTotalPagoActual;
        private DataGridView dataGridView1;
        private Label lblPagosRegistrados;
        private Button btnConfirmarPagoActual;
        private NumericUpDown numericUpDown2;
        
    }
}