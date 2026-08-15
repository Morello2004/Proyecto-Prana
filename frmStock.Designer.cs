namespace Prana.Presentacion
{
    partial class frmStock
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
            Label label28;
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            label1 = new Label();
            pnlEncabezadostock = new Panel();
            lblstock = new Label();
            pnlbuscarproducto = new Panel();
            panel3 = new Panel();
            label30 = new Label();
            dataGridViewBusquedaProducto = new DataGridView();
            comboBoxCateogria = new ComboBox();
            label2 = new Label();
            comboBoxCondicion = new ComboBox();
            label7 = new Label();
            label6 = new Label();
            cboMarca = new ComboBox();
            BtnLimpiarFiltros = new FontAwesome.Sharp.IconButton();
            lblcodigo = new Label();
            btnBuscarProducto = new FontAwesome.Sharp.IconButton();
            txtBuscarProducto = new TextBox();
            panel1 = new Panel();
            panel5 = new Panel();
            label14 = new Label();
            PnlNuevoStock = new Panel();
            textBoxRtoProveedor = new TextBox();
            iconButton1 = new FontAwesome.Sharp.IconButton();
            label31 = new Label();
            ButtonRegistrarIngreso = new FontAwesome.Sharp.IconButton();
            labelCantidadIngresada = new Label();
            label26 = new Label();
            label25 = new Label();
            LblTotalIngresoDinero = new Label();
            dataGridViewDetalleIngreso = new DataGridView();
            colProducto = new DataGridViewTextBoxColumn();
            idVariante = new DataGridViewTextBoxColumn();
            colVarianteIngreso = new DataGridViewTextBoxColumn();
            colCantidad = new DataGridViewTextBoxColumn();
            colCosto = new DataGridViewTextBoxColumn();
            colTotal = new DataGridViewTextBoxColumn();
            label19 = new Label();
            label18 = new Label();
            dtpFechaIngresoStock = new DateTimePicker();
            comboBoxSeleccionaProveedor = new ComboBox();
            textBoxNuevoProvedor = new TextBox();
            label16 = new Label();
            panel2 = new Panel();
            label15 = new Label();
            textBoxModelo = new TextBox();
            panel4 = new Panel();
            label9 = new Label();
            label29 = new Label();
            textBoxMarca = new TextBox();
            btnCancelar = new FontAwesome.Sharp.IconButton();
            btnGuardarProducto = new FontAwesome.Sharp.IconButton();
            dgvVariantes = new DataGridView();
            CondicionProducto = new DataGridViewTextBoxColumn();
            colColor = new DataGridViewTextBoxColumn();
            Colmodelo2 = new DataGridViewTextBoxColumn();
            Precio = new DataGridViewTextBoxColumn();
            colStockFisico = new DataGridViewTextBoxColumn();
            ColComprometido = new DataGridViewTextBoxColumn();
            ColDisponible = new DataGridViewTextBoxColumn();
            NombreProducto = new DataGridViewTextBoxColumn();
            label13 = new Label();
            textBoxDescripcion = new TextBox();
            nudPrecio = new NumericUpDown();
            label11 = new Label();
            label10 = new Label();
            textBoxCategoria = new TextBox();
            textBoxColor = new TextBox();
            label8 = new Label();
            textBoxNombreProducto = new TextBox();
            label3 = new Label();
            comboBoxCondicion2 = new ComboBox();
            label4 = new Label();
            label5 = new Label();
            btnAgregarVariante = new FontAwesome.Sharp.IconButton();
            colIdVariante = new DataGridViewTextBoxColumn();
            colModelo = new DataGridViewTextBoxColumn();
            colNombre2 = new DataGridViewTextBoxColumn();
            colDescripcion = new DataGridViewTextBoxColumn();
            colColorb = new DataGridViewTextBoxColumn();
            colCondicion = new DataGridViewTextBoxColumn();
            colPrecio = new DataGridViewTextBoxColumn();
            colCosto2 = new DataGridViewTextBoxColumn();
            colStockFisicoBusqueda = new DataGridViewTextBoxColumn();
            colComprometidoBusqueda = new DataGridViewTextBoxColumn();
            colModificar = new DataGridViewButtonColumn();
            ColEliminar = new DataGridViewButtonColumn();
            label28 = new Label();
            pnlEncabezadostock.SuspendLayout();
            pnlbuscarproducto.SuspendLayout();
            panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewBusquedaProducto).BeginInit();
            panel1.SuspendLayout();
            panel5.SuspendLayout();
            PnlNuevoStock.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewDetalleIngreso).BeginInit();
            panel2.SuspendLayout();
            panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvVariantes).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudPrecio).BeginInit();
            SuspendLayout();
            // 
            // label28
            // 
            label28.AutoSize = true;
            label28.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            label28.ForeColor = Color.LightSkyBlue;
            label28.Location = new Point(12, 71);
            label28.Name = "label28";
            label28.Size = new Size(231, 20);
            label28.TabIndex = 38;
            label28.Text = "1. Datos generales del producto";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(60, 72);
            label1.Name = "label1";
            label1.Size = new Size(0, 15);
            label1.TabIndex = 0;
            // 
            // pnlEncabezadostock
            // 
            pnlEncabezadostock.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pnlEncabezadostock.AutoSize = true;
            pnlEncabezadostock.BackColor = Color.FromArgb(21, 30, 41);
            pnlEncabezadostock.Controls.Add(lblstock);
            pnlEncabezadostock.Location = new Point(6, 5);
            pnlEncabezadostock.Name = "pnlEncabezadostock";
            pnlEncabezadostock.Size = new Size(1478, 65);
            pnlEncabezadostock.TabIndex = 6;
            pnlEncabezadostock.Paint += pnlEncabezadostock_Paint;
            // 
            // lblstock
            // 
            lblstock.AutoSize = true;
            lblstock.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblstock.ForeColor = Color.White;
            lblstock.Location = new Point(15, 10);
            lblstock.Name = "lblstock";
            lblstock.Size = new Size(75, 32);
            lblstock.TabIndex = 0;
            lblstock.Text = "Stock";
            lblstock.Click += lblTituloNuevaVenta_Click;
            // 
            // pnlbuscarproducto
            // 
            pnlbuscarproducto.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pnlbuscarproducto.AutoSize = true;
            pnlbuscarproducto.BackColor = Color.FromArgb(21, 30, 41);
            pnlbuscarproducto.Controls.Add(panel3);
            pnlbuscarproducto.Controls.Add(dataGridViewBusquedaProducto);
            pnlbuscarproducto.Controls.Add(comboBoxCateogria);
            pnlbuscarproducto.Controls.Add(label2);
            pnlbuscarproducto.Controls.Add(comboBoxCondicion);
            pnlbuscarproducto.Controls.Add(label7);
            pnlbuscarproducto.Controls.Add(label6);
            pnlbuscarproducto.Controls.Add(cboMarca);
            pnlbuscarproducto.Controls.Add(BtnLimpiarFiltros);
            pnlbuscarproducto.Controls.Add(lblcodigo);
            pnlbuscarproducto.Controls.Add(btnBuscarProducto);
            pnlbuscarproducto.Controls.Add(txtBuscarProducto);
            pnlbuscarproducto.ForeColor = Color.White;
            pnlbuscarproducto.Location = new Point(9, 76);
            pnlbuscarproducto.Name = "pnlbuscarproducto";
            pnlbuscarproducto.Size = new Size(1503, 366);
            pnlbuscarproducto.TabIndex = 7;
            pnlbuscarproducto.Paint += pnlCliente_Paint;
            // 
            // panel3
            // 
            panel3.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            panel3.AutoSize = true;
            panel3.BackColor = Color.MidnightBlue;
            panel3.Controls.Add(label30);
            panel3.Location = new Point(3, 0);
            panel3.Name = "panel3";
            panel3.Size = new Size(1492, 65);
            panel3.TabIndex = 49;
            panel3.Paint += panel3_Paint;
            // 
            // label30
            // 
            label30.AutoSize = true;
            label30.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label30.ForeColor = Color.White;
            label30.Location = new Point(15, 10);
            label30.Name = "label30";
            label30.Size = new Size(214, 32);
            label30.TabIndex = 0;
            label30.Text = "Buscar productos";
            // 
            // dataGridViewBusquedaProducto
            // 
            dataGridViewBusquedaProducto.AllowUserToAddRows = false;
            dataGridViewBusquedaProducto.AllowUserToDeleteRows = false;
            dataGridViewBusquedaProducto.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            dataGridViewBusquedaProducto.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewBusquedaProducto.BackgroundColor = SystemColors.Control;
            dataGridViewBusquedaProducto.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewBusquedaProducto.Columns.AddRange(new DataGridViewColumn[] { colIdVariante, colModelo, colNombre2, colDescripcion, colColorb, colCondicion, colPrecio, colCosto2, colStockFisicoBusqueda, colComprometidoBusqueda, colModificar, ColEliminar });
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Window;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle1.ForeColor = Color.White;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.False;
            dataGridViewBusquedaProducto.DefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewBusquedaProducto.EditMode = DataGridViewEditMode.EditProgrammatically;
            dataGridViewBusquedaProducto.Location = new Point(8, 160);
            dataGridViewBusquedaProducto.MultiSelect = false;
            dataGridViewBusquedaProducto.Name = "dataGridViewBusquedaProducto";
            dataGridViewBusquedaProducto.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewBusquedaProducto.Size = new Size(1492, 150);
            dataGridViewBusquedaProducto.TabIndex = 48;
            dataGridViewBusquedaProducto.CellClick += dataGridViewBusquedaProducto_CellClick;
            dataGridViewBusquedaProducto.CellContentClick += dataGridViewBusquedaProducto_CellContentClick;
            // 
            // comboBoxCateogria
            // 
            comboBoxCateogria.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxCateogria.FormattingEnabled = true;
            comboBoxCateogria.Location = new Point(308, 106);
            comboBoxCateogria.Name = "comboBoxCateogria";
            comboBoxCateogria.Size = new Size(220, 23);
            comboBoxCateogria.TabIndex = 19;
            comboBoxCateogria.SelectedIndexChanged += comboBox4_SelectedIndexChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.ForeColor = Color.FromArgb(145, 170, 210);
            label2.Location = new Point(802, 88);
            label2.Name = "label2";
            label2.Size = new Size(62, 15);
            label2.TabIndex = 18;
            label2.Text = "Condicion";
            // 
            // comboBoxCondicion
            // 
            comboBoxCondicion.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxCondicion.FormattingEnabled = true;
            comboBoxCondicion.Items.AddRange(new object[] { "Primera", "Outlet" });
            comboBoxCondicion.Location = new Point(802, 106);
            comboBoxCondicion.Name = "comboBoxCondicion";
            comboBoxCondicion.Size = new Size(220, 23);
            comboBoxCondicion.TabIndex = 17;
            comboBoxCondicion.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.ForeColor = Color.FromArgb(145, 170, 210);
            label7.Location = new Point(556, 88);
            label7.Name = "label7";
            label7.Size = new Size(40, 15);
            label7.TabIndex = 16;
            label7.Text = "Marca";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.ForeColor = Color.FromArgb(145, 170, 210);
            label6.Location = new Point(308, 88);
            label6.Name = "label6";
            label6.Size = new Size(58, 15);
            label6.TabIndex = 15;
            label6.Text = "Categoria";
            // 
            // cboMarca
            // 
            cboMarca.DropDownStyle = ComboBoxStyle.DropDownList;
            cboMarca.FormattingEnabled = true;
            cboMarca.Location = new Point(556, 106);
            cboMarca.Name = "cboMarca";
            cboMarca.Size = new Size(220, 23);
            cboMarca.TabIndex = 14;
            cboMarca.SelectedIndexChanged += cboMarca_SelectedIndexChanged;
            // 
            // BtnLimpiarFiltros
            // 
            BtnLimpiarFiltros.BackColor = Color.DodgerBlue;
            BtnLimpiarFiltros.FlatAppearance.BorderSize = 0;
            BtnLimpiarFiltros.FlatStyle = FlatStyle.Flat;
            BtnLimpiarFiltros.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            BtnLimpiarFiltros.IconChar = FontAwesome.Sharp.IconChar.None;
            BtnLimpiarFiltros.IconColor = Color.Black;
            BtnLimpiarFiltros.IconFont = FontAwesome.Sharp.IconFont.Auto;
            BtnLimpiarFiltros.Location = new Point(1159, 104);
            BtnLimpiarFiltros.Name = "BtnLimpiarFiltros";
            BtnLimpiarFiltros.Size = new Size(100, 25);
            BtnLimpiarFiltros.TabIndex = 4;
            BtnLimpiarFiltros.Text = "Limpiar filtros";
            BtnLimpiarFiltros.UseVisualStyleBackColor = false;
            BtnLimpiarFiltros.Click += BtnLimpiarFiltros_Click;
            // 
            // lblcodigo
            // 
            lblcodigo.AutoSize = true;
            lblcodigo.ForeColor = Color.FromArgb(145, 170, 210);
            lblcodigo.Location = new Point(13, 88);
            lblcodigo.Name = "lblcodigo";
            lblcodigo.Size = new Size(167, 15);
            lblcodigo.TabIndex = 3;
            lblcodigo.Text = "Modelo / codigo del producto";
            // 
            // btnBuscarProducto
            // 
            btnBuscarProducto.BackColor = Color.DodgerBlue;
            btnBuscarProducto.FlatAppearance.BorderSize = 0;
            btnBuscarProducto.FlatStyle = FlatStyle.Flat;
            btnBuscarProducto.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnBuscarProducto.IconChar = FontAwesome.Sharp.IconChar.None;
            btnBuscarProducto.IconColor = Color.Black;
            btnBuscarProducto.IconFont = FontAwesome.Sharp.IconFont.Auto;
            btnBuscarProducto.Location = new Point(1043, 104);
            btnBuscarProducto.Name = "btnBuscarProducto";
            btnBuscarProducto.Size = new Size(100, 25);
            btnBuscarProducto.TabIndex = 2;
            btnBuscarProducto.Text = "Buscar";
            btnBuscarProducto.UseVisualStyleBackColor = false;
            btnBuscarProducto.Click += btnBuscarProducto_Click;
            // 
            // txtBuscarProducto
            // 
            txtBuscarProducto.Font = new Font("Segoe UI", 10F);
            txtBuscarProducto.Location = new Point(13, 106);
            txtBuscarProducto.Name = "txtBuscarProducto";
            txtBuscarProducto.PlaceholderText = "Buscar por codigo o nombre";
            txtBuscarProducto.Size = new Size(277, 25);
            txtBuscarProducto.TabIndex = 1;
            txtBuscarProducto.TextChanged += txtBuscarProducto_TextChanged;
            // 
            // panel1
            // 
            panel1.AutoScroll = true;
            panel1.AutoSize = true;
            panel1.BackColor = Color.FromArgb(13, 19, 27);
            panel1.Controls.Add(panel5);
            panel1.Controls.Add(PnlNuevoStock);
            panel1.Controls.Add(panel2);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(pnlbuscarproducto);
            panel1.Controls.Add(pnlEncabezadostock);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1516, 761);
            panel1.TabIndex = 8;
            panel1.Paint += panel1_Paint;
            // 
            // panel5
            // 
            panel5.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            panel5.AutoSize = true;
            panel5.BackColor = Color.MidnightBlue;
            panel5.Controls.Add(label14);
            panel5.Location = new Point(3, 1095);
            panel5.Name = "panel5";
            panel5.Size = new Size(1492, 65);
            panel5.TabIndex = 54;
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label14.ForeColor = Color.White;
            label14.Location = new Point(15, 10);
            label14.Name = "label14";
            label14.Size = new Size(203, 32);
            label14.TabIndex = 0;
            label14.Text = "Ingreso de stock";
            // 
            // PnlNuevoStock
            // 
            PnlNuevoStock.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            PnlNuevoStock.AutoScroll = true;
            PnlNuevoStock.AutoSize = true;
            PnlNuevoStock.BackColor = Color.FromArgb(21, 30, 41);
            PnlNuevoStock.Controls.Add(textBoxRtoProveedor);
            PnlNuevoStock.Controls.Add(iconButton1);
            PnlNuevoStock.Controls.Add(label31);
            PnlNuevoStock.Controls.Add(ButtonRegistrarIngreso);
            PnlNuevoStock.Controls.Add(labelCantidadIngresada);
            PnlNuevoStock.Controls.Add(label26);
            PnlNuevoStock.Controls.Add(label25);
            PnlNuevoStock.Controls.Add(LblTotalIngresoDinero);
            PnlNuevoStock.Controls.Add(dataGridViewDetalleIngreso);
            PnlNuevoStock.Controls.Add(label19);
            PnlNuevoStock.Controls.Add(label18);
            PnlNuevoStock.Controls.Add(dtpFechaIngresoStock);
            PnlNuevoStock.Controls.Add(comboBoxSeleccionaProveedor);
            PnlNuevoStock.Controls.Add(textBoxNuevoProvedor);
            PnlNuevoStock.Controls.Add(label16);
            PnlNuevoStock.Location = new Point(5, 1095);
            PnlNuevoStock.Name = "PnlNuevoStock";
            PnlNuevoStock.Size = new Size(1519, 517);
            PnlNuevoStock.TabIndex = 9;
            // 
            // textBoxRtoProveedor
            // 
            textBoxRtoProveedor.Font = new Font("Segoe UI", 10F);
            textBoxRtoProveedor.Location = new Point(602, 144);
            textBoxRtoProveedor.Name = "textBoxRtoProveedor";
            textBoxRtoProveedor.PlaceholderText = "Numero de doc de proveedor";
            textBoxRtoProveedor.Size = new Size(285, 25);
            textBoxRtoProveedor.TabIndex = 56;
            // 
            // iconButton1
            // 
            iconButton1.BackColor = Color.ForestGreen;
            iconButton1.FlatAppearance.BorderSize = 0;
            iconButton1.FlatStyle = FlatStyle.Flat;
            iconButton1.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            iconButton1.ForeColor = SystemColors.Control;
            iconButton1.IconChar = FontAwesome.Sharp.IconChar.None;
            iconButton1.IconColor = Color.Black;
            iconButton1.IconFont = FontAwesome.Sharp.IconFont.Auto;
            iconButton1.Location = new Point(24, 192);
            iconButton1.Name = "iconButton1";
            iconButton1.Size = new Size(285, 25);
            iconButton1.TabIndex = 55;
            iconButton1.Text = "Buscar producto";
            iconButton1.UseVisualStyleBackColor = false;
            iconButton1.Click += iconButton1_Click;
            // 
            // label31
            // 
            label31.AutoSize = true;
            label31.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            label31.ForeColor = Color.LightSkyBlue;
            label31.Location = new Point(10, 78);
            label31.Name = "label31";
            label31.Size = new Size(131, 20);
            label31.TabIndex = 54;
            label31.Text = "Datos del ingreso";
            // 
            // ButtonRegistrarIngreso
            // 
            ButtonRegistrarIngreso.BackColor = Color.DarkGreen;
            ButtonRegistrarIngreso.FlatAppearance.BorderSize = 0;
            ButtonRegistrarIngreso.FlatStyle = FlatStyle.Flat;
            ButtonRegistrarIngreso.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
            ButtonRegistrarIngreso.ForeColor = SystemColors.ButtonHighlight;
            ButtonRegistrarIngreso.IconChar = FontAwesome.Sharp.IconChar.None;
            ButtonRegistrarIngreso.IconColor = Color.White;
            ButtonRegistrarIngreso.IconFont = FontAwesome.Sharp.IconFont.Auto;
            ButtonRegistrarIngreso.Location = new Point(480, 447);
            ButtonRegistrarIngreso.Name = "ButtonRegistrarIngreso";
            ButtonRegistrarIngreso.Size = new Size(197, 42);
            ButtonRegistrarIngreso.TabIndex = 52;
            ButtonRegistrarIngreso.Text = "Registrar ingreso";
            ButtonRegistrarIngreso.UseVisualStyleBackColor = false;
            ButtonRegistrarIngreso.Click += ButtonRegistrarIngreso_Click;
            // 
            // labelCantidadIngresada
            // 
            labelCantidadIngresada.AutoSize = true;
            labelCantidadIngresada.Font = new Font("Segoe UI", 15F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelCantidadIngresada.ForeColor = Color.White;
            labelCantidadIngresada.Location = new Point(281, 447);
            labelCantidadIngresada.Name = "labelCantidadIngresada";
            labelCantidadIngresada.Size = new Size(24, 28);
            labelCantidadIngresada.TabIndex = 51;
            labelCantidadIngresada.Text = "0";
            // 
            // label26
            // 
            label26.AutoSize = true;
            label26.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label26.ForeColor = Color.FromArgb(145, 170, 210);
            label26.Location = new Point(229, 475);
            label26.Name = "label26";
            label26.Size = new Size(147, 19);
            label26.TabIndex = 50;
            label26.Text = "Unidades ingresadas";
            // 
            // label25
            // 
            label25.AutoSize = true;
            label25.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label25.ForeColor = Color.FromArgb(145, 170, 210);
            label25.Location = new Point(19, 475);
            label25.Name = "label25";
            label25.Size = new Size(96, 19);
            label25.TabIndex = 49;
            label25.Text = "Total ingreso";
            // 
            // LblTotalIngresoDinero
            // 
            LblTotalIngresoDinero.AutoSize = true;
            LblTotalIngresoDinero.Font = new Font("Segoe UI", 15F, FontStyle.Bold, GraphicsUnit.Point, 0);
            LblTotalIngresoDinero.ForeColor = Color.White;
            LblTotalIngresoDinero.Location = new Point(45, 447);
            LblTotalIngresoDinero.Name = "LblTotalIngresoDinero";
            LblTotalIngresoDinero.Size = new Size(36, 28);
            LblTotalIngresoDinero.TabIndex = 48;
            LblTotalIngresoDinero.Text = "$0";
            // 
            // dataGridViewDetalleIngreso
            // 
            dataGridViewDetalleIngreso.AllowUserToAddRows = false;
            dataGridViewDetalleIngreso.AllowUserToDeleteRows = false;
            dataGridViewDetalleIngreso.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            dataGridViewDetalleIngreso.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewDetalleIngreso.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewDetalleIngreso.Columns.AddRange(new DataGridViewColumn[] { colProducto, idVariante, colVarianteIngreso, colCantidad, colCosto, colTotal });
            dataGridViewDetalleIngreso.Location = new Point(10, 271);
            dataGridViewDetalleIngreso.Name = "dataGridViewDetalleIngreso";
            dataGridViewDetalleIngreso.Size = new Size(1492, 150);
            dataGridViewDetalleIngreso.TabIndex = 47;
            dataGridViewDetalleIngreso.CellContentClick += dataGridViewDetalleIngreso_CellContentClick;
            dataGridViewDetalleIngreso.CellEndEdit += dataGridViewDetalleIngreso_CellEndEdit;
            // 
            // colProducto
            // 
            colProducto.DataPropertyName = "NombreProducto";
            colProducto.HeaderText = "producto";
            colProducto.Name = "colProducto";
            // 
            // idVariante
            // 
            idVariante.DataPropertyName = "IdVariante";
            idVariante.HeaderText = "colId";
            idVariante.Name = "idVariante";
            idVariante.Visible = false;
            // 
            // colVarianteIngreso
            // 
            colVarianteIngreso.HeaderText = "Ingreso";
            colVarianteIngreso.Name = "colVarianteIngreso";
            // 
            // colCantidad
            // 
            colCantidad.HeaderText = "Cantidad";
            colCantidad.Name = "colCantidad";
            // 
            // colCosto
            // 
            colCosto.HeaderText = "Costo unitario";
            colCosto.Name = "colCosto";
            // 
            // colTotal
            // 
            dataGridViewCellStyle2.Format = "C2";
            dataGridViewCellStyle2.NullValue = null;
            colTotal.DefaultCellStyle = dataGridViewCellStyle2;
            colTotal.HeaderText = "Total";
            colTotal.Name = "colTotal";
            colTotal.ReadOnly = true;
            // 
            // label19
            // 
            label19.AutoSize = true;
            label19.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            label19.ForeColor = Color.FromArgb(145, 170, 210);
            label19.Location = new Point(337, 130);
            label19.Name = "label19";
            label19.Size = new Size(122, 13);
            label19.TabIndex = 33;
            label19.Text = "Seleccionar proveedor";
            // 
            // label18
            // 
            label18.AutoSize = true;
            label18.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            label18.ForeColor = Color.FromArgb(145, 170, 210);
            label18.Location = new Point(337, 178);
            label18.Name = "label18";
            label18.Size = new Size(95, 13);
            label18.TabIndex = 32;
            label18.Text = "Fecha de ingreso";
            // 
            // dtpFechaIngresoStock
            // 
            dtpFechaIngresoStock.Format = DateTimePickerFormat.Short;
            dtpFechaIngresoStock.Location = new Point(337, 194);
            dtpFechaIngresoStock.Name = "dtpFechaIngresoStock";
            dtpFechaIngresoStock.Size = new Size(231, 23);
            dtpFechaIngresoStock.TabIndex = 30;
            // 
            // comboBoxSeleccionaProveedor
            // 
            comboBoxSeleccionaProveedor.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxSeleccionaProveedor.FormattingEnabled = true;
            comboBoxSeleccionaProveedor.Location = new Point(337, 146);
            comboBoxSeleccionaProveedor.Name = "comboBoxSeleccionaProveedor";
            comboBoxSeleccionaProveedor.Size = new Size(231, 23);
            comboBoxSeleccionaProveedor.TabIndex = 28;
            comboBoxSeleccionaProveedor.Tag = "";
            // 
            // textBoxNuevoProvedor
            // 
            textBoxNuevoProvedor.Font = new Font("Segoe UI", 10F);
            textBoxNuevoProvedor.Location = new Point(24, 144);
            textBoxNuevoProvedor.Name = "textBoxNuevoProvedor";
            textBoxNuevoProvedor.PlaceholderText = "Si no esta cargada escribalo aqui";
            textBoxNuevoProvedor.Size = new Size(285, 25);
            textBoxNuevoProvedor.TabIndex = 27;
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            label16.ForeColor = Color.FromArgb(145, 170, 210);
            label16.Location = new Point(24, 128);
            label16.Name = "label16";
            label16.Size = new Size(61, 13);
            label16.TabIndex = 3;
            label16.Text = "Proveedor";
            // 
            // panel2
            // 
            panel2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            panel2.AutoScroll = true;
            panel2.AutoSize = true;
            panel2.BackColor = Color.FromArgb(21, 30, 41);
            panel2.Controls.Add(label15);
            panel2.Controls.Add(textBoxModelo);
            panel2.Controls.Add(panel4);
            panel2.Controls.Add(label29);
            panel2.Controls.Add(label28);
            panel2.Controls.Add(textBoxMarca);
            panel2.Controls.Add(btnCancelar);
            panel2.Controls.Add(btnGuardarProducto);
            panel2.Controls.Add(dgvVariantes);
            panel2.Controls.Add(label13);
            panel2.Controls.Add(textBoxDescripcion);
            panel2.Controls.Add(nudPrecio);
            panel2.Controls.Add(label11);
            panel2.Controls.Add(label10);
            panel2.Controls.Add(textBoxCategoria);
            panel2.Controls.Add(textBoxColor);
            panel2.Controls.Add(label8);
            panel2.Controls.Add(textBoxNombreProducto);
            panel2.Controls.Add(label3);
            panel2.Controls.Add(comboBoxCondicion2);
            panel2.Controls.Add(label4);
            panel2.Controls.Add(label5);
            panel2.Controls.Add(btnAgregarVariante);
            panel2.ForeColor = Color.White;
            panel2.Location = new Point(6, 448);
            panel2.Name = "panel2";
            panel2.Size = new Size(1478, 641);
            panel2.TabIndex = 8;
            panel2.Paint += panel2_Paint;
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.ForeColor = Color.FromArgb(145, 170, 210);
            label15.Location = new Point(255, 99);
            label15.Name = "label15";
            label15.Size = new Size(48, 15);
            label15.TabIndex = 52;
            label15.Text = "Modelo";
            // 
            // textBoxModelo
            // 
            textBoxModelo.Font = new Font("Segoe UI", 10F);
            textBoxModelo.Location = new Point(255, 119);
            textBoxModelo.Name = "textBoxModelo";
            textBoxModelo.PlaceholderText = "Ingrese modelo";
            textBoxModelo.Size = new Size(228, 25);
            textBoxModelo.TabIndex = 51;
            // 
            // panel4
            // 
            panel4.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            panel4.AutoSize = true;
            panel4.BackColor = Color.MidnightBlue;
            panel4.Controls.Add(label9);
            panel4.Location = new Point(0, 3);
            panel4.Name = "panel4";
            panel4.Size = new Size(1492, 65);
            panel4.TabIndex = 50;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label9.ForeColor = Color.White;
            label9.Location = new Point(15, 10);
            label9.Name = "label9";
            label9.Size = new Size(202, 32);
            label9.TabIndex = 0;
            label9.Text = "Nuevo producto";
            // 
            // label29
            // 
            label29.AutoSize = true;
            label29.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            label29.ForeColor = Color.LightSkyBlue;
            label29.Location = new Point(13, 228);
            label29.Name = "label29";
            label29.Size = new Size(184, 20);
            label29.TabIndex = 39;
            label29.Text = "2. Variantes del producto";
            // 
            // textBoxMarca
            // 
            textBoxMarca.Font = new Font("Segoe UI", 10F);
            textBoxMarca.Location = new Point(668, 119);
            textBoxMarca.Name = "textBoxMarca";
            textBoxMarca.PlaceholderText = "Marca";
            textBoxMarca.Size = new Size(151, 25);
            textBoxMarca.TabIndex = 37;
            textBoxMarca.TextChanged += textBoxMarca_TextChanged;
            // 
            // btnCancelar
            // 
            btnCancelar.BackColor = Color.SteelBlue;
            btnCancelar.FlatAppearance.BorderSize = 0;
            btnCancelar.FlatStyle = FlatStyle.Flat;
            btnCancelar.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnCancelar.IconChar = FontAwesome.Sharp.IconChar.None;
            btnCancelar.IconColor = Color.Black;
            btnCancelar.IconFont = FontAwesome.Sharp.IconFont.Auto;
            btnCancelar.Location = new Point(141, 507);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(124, 25);
            btnCancelar.TabIndex = 36;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = false;
            btnCancelar.Click += btnCancelar_Click;
            // 
            // btnGuardarProducto
            // 
            btnGuardarProducto.BackColor = Color.DarkGreen;
            btnGuardarProducto.FlatAppearance.BorderSize = 0;
            btnGuardarProducto.FlatStyle = FlatStyle.Flat;
            btnGuardarProducto.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnGuardarProducto.IconChar = FontAwesome.Sharp.IconChar.None;
            btnGuardarProducto.IconColor = Color.Black;
            btnGuardarProducto.IconFont = FontAwesome.Sharp.IconFont.Auto;
            btnGuardarProducto.Location = new Point(6, 507);
            btnGuardarProducto.Name = "btnGuardarProducto";
            btnGuardarProducto.Size = new Size(119, 25);
            btnGuardarProducto.TabIndex = 33;
            btnGuardarProducto.Text = "Guardar Producto";
            btnGuardarProducto.UseVisualStyleBackColor = false;
            btnGuardarProducto.Click += btnGuardarProducto_Click;
            // 
            // dgvVariantes
            // 
            dgvVariantes.AllowUserToAddRows = false;
            dgvVariantes.AllowUserToDeleteRows = false;
            dgvVariantes.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            dgvVariantes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvVariantes.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvVariantes.Columns.AddRange(new DataGridViewColumn[] { CondicionProducto, colColor, Colmodelo2, Precio, colStockFisico, ColComprometido, ColDisponible, NombreProducto });
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = SystemColors.Window;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle3.ForeColor = Color.White;
            dataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            dgvVariantes.DefaultCellStyle = dataGridViewCellStyle3;
            dgvVariantes.Location = new Point(6, 330);
            dgvVariantes.MultiSelect = false;
            dgvVariantes.Name = "dgvVariantes";
            dgvVariantes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvVariantes.Size = new Size(1360, 150);
            dgvVariantes.TabIndex = 32;
            dgvVariantes.CellContentClick += dgvVariantes_CellContentClick;
            // 
            // CondicionProducto
            // 
            CondicionProducto.DataPropertyName = "CondicionProducto";
            CondicionProducto.HeaderText = "Condicion";
            CondicionProducto.Name = "CondicionProducto";
            CondicionProducto.ReadOnly = true;
            // 
            // colColor
            // 
            colColor.DataPropertyName = "Color";
            colColor.HeaderText = "Color";
            colColor.Name = "colColor";
            colColor.ReadOnly = true;
            // 
            // Colmodelo2
            // 
            Colmodelo2.HeaderText = "Modelo";
            Colmodelo2.Name = "Colmodelo2";
            Colmodelo2.ReadOnly = true;
            // 
            // Precio
            // 
            Precio.DataPropertyName = "Precio";
            Precio.HeaderText = "Precio";
            Precio.Name = "Precio";
            Precio.ReadOnly = true;
            // 
            // colStockFisico
            // 
            colStockFisico.HeaderText = "Stock Fisico";
            colStockFisico.Name = "colStockFisico";
            colStockFisico.ReadOnly = true;
            // 
            // ColComprometido
            // 
            ColComprometido.HeaderText = "Comprometido";
            ColComprometido.Name = "ColComprometido";
            ColComprometido.ReadOnly = true;
            // 
            // ColDisponible
            // 
            ColDisponible.HeaderText = "Disponible";
            ColDisponible.Name = "ColDisponible";
            ColDisponible.ReadOnly = true;
            // 
            // NombreProducto
            // 
            NombreProducto.DataPropertyName = "NombreProducto";
            NombreProducto.HeaderText = "Nombre";
            NombreProducto.Name = "NombreProducto";
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.ForeColor = Color.FromArgb(145, 170, 210);
            label13.Location = new Point(9, 153);
            label13.Name = "label13";
            label13.Size = new Size(154, 15);
            label13.TabIndex = 30;
            label13.Text = "Descripción / características";
            // 
            // textBoxDescripcion
            // 
            textBoxDescripcion.Font = new Font("Segoe UI", 10F);
            textBoxDescripcion.Location = new Point(12, 171);
            textBoxDescripcion.Name = "textBoxDescripcion";
            textBoxDescripcion.PlaceholderText = "Caracteristicas generales del producto";
            textBoxDescripcion.Size = new Size(932, 25);
            textBoxDescripcion.TabIndex = 29;
            textBoxDescripcion.TextChanged += textBoxDescripcion_TextChanged;
            // 
            // nudPrecio
            // 
            nudPrecio.DecimalPlaces = 2;
            nudPrecio.Location = new Point(438, 287);
            nudPrecio.Maximum = new decimal(new int[] { 100000000, 0, 0, 0 });
            nudPrecio.Name = "nudPrecio";
            nudPrecio.Size = new Size(150, 23);
            nudPrecio.TabIndex = 28;
            nudPrecio.ThousandsSeparator = true;
            nudPrecio.ValueChanged += nudPrecio_ValueChanged;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.ForeColor = Color.FromArgb(145, 170, 210);
            label11.Location = new Point(438, 269);
            label11.Name = "label11";
            label11.Size = new Size(45, 15);
            label11.TabIndex = 25;
            label11.Text = "Precio*";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.ForeColor = Color.FromArgb(145, 170, 210);
            label10.Location = new Point(267, 268);
            label10.Name = "label10";
            label10.Size = new Size(41, 15);
            label10.TabIndex = 23;
            label10.Text = "Color*";
            // 
            // textBoxCategoria
            // 
            textBoxCategoria.Font = new Font("Segoe UI", 10F);
            textBoxCategoria.Location = new Point(497, 119);
            textBoxCategoria.Name = "textBoxCategoria";
            textBoxCategoria.PlaceholderText = "Categoria";
            textBoxCategoria.Size = new Size(163, 25);
            textBoxCategoria.TabIndex = 22;
            textBoxCategoria.TextChanged += textBoxCategoria_TextChanged;
            // 
            // textBoxColor
            // 
            textBoxColor.Font = new Font("Segoe UI", 10F);
            textBoxColor.Location = new Point(267, 286);
            textBoxColor.Name = "textBoxColor";
            textBoxColor.Size = new Size(164, 25);
            textBoxColor.TabIndex = 21;
            textBoxColor.TextChanged += textBoxColor_TextChanged;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.ForeColor = Color.FromArgb(145, 170, 210);
            label8.Location = new Point(13, 99);
            label8.Name = "label8";
            label8.Size = new Size(103, 15);
            label8.TabIndex = 20;
            label8.Text = "Nombre producto";
            // 
            // textBoxNombreProducto
            // 
            textBoxNombreProducto.Font = new Font("Segoe UI", 10F);
            textBoxNombreProducto.Location = new Point(18, 119);
            textBoxNombreProducto.Name = "textBoxNombreProducto";
            textBoxNombreProducto.PlaceholderText = "Ingrese codigo producto";
            textBoxNombreProducto.Size = new Size(228, 25);
            textBoxNombreProducto.TabIndex = 19;
            textBoxNombreProducto.TextChanged += textBoxNombreProducto_TextChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.ForeColor = Color.FromArgb(145, 170, 210);
            label3.Location = new Point(13, 268);
            label3.Name = "label3";
            label3.Size = new Size(67, 15);
            label3.TabIndex = 18;
            label3.Text = "Condicion*";
            // 
            // comboBoxCondicion2
            // 
            comboBoxCondicion2.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxCondicion2.FormattingEnabled = true;
            comboBoxCondicion2.Items.AddRange(new object[] { "Primera", "Outlet" });
            comboBoxCondicion2.Location = new Point(15, 286);
            comboBoxCondicion2.Name = "comboBoxCondicion2";
            comboBoxCondicion2.Size = new Size(231, 23);
            comboBoxCondicion2.TabIndex = 17;
            comboBoxCondicion2.SelectedIndexChanged += comboBox2_SelectedIndexChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.ForeColor = Color.FromArgb(145, 170, 210);
            label4.Location = new Point(668, 101);
            label4.Name = "label4";
            label4.Size = new Size(45, 15);
            label4.TabIndex = 16;
            label4.Text = "Marca*";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.ForeColor = Color.FromArgb(145, 170, 210);
            label5.Location = new Point(497, 99);
            label5.Name = "label5";
            label5.Size = new Size(63, 15);
            label5.TabIndex = 15;
            label5.Text = "Categoria*";
            // 
            // btnAgregarVariante
            // 
            btnAgregarVariante.BackColor = Color.DarkGreen;
            btnAgregarVariante.FlatAppearance.BorderSize = 0;
            btnAgregarVariante.FlatStyle = FlatStyle.Flat;
            btnAgregarVariante.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnAgregarVariante.IconChar = FontAwesome.Sharp.IconChar.None;
            btnAgregarVariante.IconColor = Color.Black;
            btnAgregarVariante.IconFont = FontAwesome.Sharp.IconFont.Auto;
            btnAgregarVariante.Location = new Point(825, 287);
            btnAgregarVariante.Name = "btnAgregarVariante";
            btnAgregarVariante.Size = new Size(119, 25);
            btnAgregarVariante.TabIndex = 2;
            btnAgregarVariante.Text = "Agregar variante";
            btnAgregarVariante.UseVisualStyleBackColor = false;
            btnAgregarVariante.Click += btnAgregarVariante_Click;
            // 
            // colIdVariante
            // 
            colIdVariante.HeaderText = "";
            colIdVariante.Name = "colIdVariante";
            colIdVariante.Visible = false;
            // 
            // colModelo
            // 
            colModelo.DataPropertyName = "Modelo";
            colModelo.HeaderText = "Modelo";
            colModelo.Name = "colModelo";
            // 
            // colNombre2
            // 
            colNombre2.HeaderText = "Producto";
            colNombre2.Name = "colNombre2";
            // 
            // colDescripcion
            // 
            colDescripcion.HeaderText = "Descripcion";
            colDescripcion.Name = "colDescripcion";
            // 
            // colColorb
            // 
            colColorb.DataPropertyName = "Color";
            colColorb.HeaderText = "Color";
            colColorb.Name = "colColorb";
            // 
            // colCondicion
            // 
            colCondicion.DataPropertyName = "Condicion";
            colCondicion.HeaderText = "Condicion";
            colCondicion.Name = "colCondicion";
            // 
            // colPrecio
            // 
            colPrecio.DataPropertyName = "Precio";
            colPrecio.HeaderText = "Precio";
            colPrecio.Name = "colPrecio";
            // 
            // colCosto2
            // 
            colCosto2.HeaderText = "Costo";
            colCosto2.Name = "colCosto2";
            // 
            // colStockFisicoBusqueda
            // 
            colStockFisicoBusqueda.DataPropertyName = "StockFisico";
            colStockFisicoBusqueda.HeaderText = "Stock fisico";
            colStockFisicoBusqueda.Name = "colStockFisicoBusqueda";
            // 
            // colComprometidoBusqueda
            // 
            colComprometidoBusqueda.DataPropertyName = "StockComprometido";
            colComprometidoBusqueda.HeaderText = "Comprometido";
            colComprometidoBusqueda.Name = "colComprometidoBusqueda";
            // 
            // colModificar
            // 
            colModificar.HeaderText = "Accion";
            colModificar.Name = "colModificar";
            colModificar.Resizable = DataGridViewTriState.True;
            colModificar.SortMode = DataGridViewColumnSortMode.Automatic;
            colModificar.Text = "Modificar";
            // 
            // ColEliminar
            // 
            ColEliminar.HeaderText = "Accion";
            ColEliminar.Name = "ColEliminar";
            ColEliminar.SortMode = DataGridViewColumnSortMode.Automatic;
            ColEliminar.Text = "Eliminar";
            ColEliminar.UseColumnTextForButtonValue = true;
            // 
            // frmStock
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            ClientSize = new Size(1516, 761);
            Controls.Add(panel1);
            Name = "frmStock";
            Text = "frmStock";
            pnlEncabezadostock.ResumeLayout(false);
            pnlEncabezadostock.PerformLayout();
            pnlbuscarproducto.ResumeLayout(false);
            pnlbuscarproducto.PerformLayout();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewBusquedaProducto).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel5.ResumeLayout(false);
            panel5.PerformLayout();
            PnlNuevoStock.ResumeLayout(false);
            PnlNuevoStock.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewDetalleIngreso).EndInit();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            panel4.ResumeLayout(false);
            panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvVariantes).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudPrecio).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Panel pnlEncabezadostock;
        private Label lblstock;
        private Panel pnlbuscarproducto;
        private FontAwesome.Sharp.IconButton BtnAgregarCliente;
        private Label lblcodigo;
        private FontAwesome.Sharp.IconButton btnBuscarProducto;
        private TextBox txtBuscarProducto;
        private Panel panel1;
        private ComboBox comboBoxCondicion;
        private Label label7;
        private Label label6;
        private ComboBox cboMarca;
        private Label label2;
        private Panel panel2;
        private Label label3;
        private ComboBox comboBoxCondicion2;
        private Label label4;
        private Label label5;
        private FontAwesome.Sharp.IconButton btnAgregarVariante;
        private Label label8;
        private TextBox textBoxNombreProducto;
        private Label label11;
        private Label label10;
        private TextBox textBoxCategoria;
        private TextBox textBoxColor;
        private NumericUpDown nudPrecio;
        private Label label13;
        private TextBox textBoxDescripcion;
        private DataGridView dgvVariantes;
        private FontAwesome.Sharp.IconButton btnCancelar;
        private FontAwesome.Sharp.IconButton btnGuardarProducto;
        private FontAwesome.Sharp.IconButton BtnLimpiarFiltros;
        private ComboBox comboBoxCateogria;
        private TextBox textBoxMarca;
        private Panel PnlNuevoStock;
        private Label label16;
        private ComboBox comboBoxSeleccionaProveedor;
        private TextBox textBoxNuevoProvedor;
        private Label label19;
        private Label label18;
        private DateTimePicker dtpFechaIngresoStock;
        private DateTimePicker dateTimePicker1;
        private TextBox textBoxRtoProveedor;
        private ComboBox comboBox1;
        private FontAwesome.Sharp.IconButton iconButton3;
        private FontAwesome.Sharp.IconButton iconButton2;
        private DataGridView dataGridViewDetalleIngreso;
        private FontAwesome.Sharp.IconButton ButtonRegistrarIngreso;
        private Label labelCantidadIngresada;
        private Label label26;
        private Label label25;
        private Label LblTotalIngresoDinero;
        private DataGridView dataGridViewBusquedaProducto;
        private Label label29;
        private Label label28;
        private Panel panel3;
        private Label label30;
        private Panel panel5;
        private Label label14;
        private Panel panel4;
        private Label label9;
        private Label label31;
        private Label label15;
        private TextBox textBoxModelo;
        private FontAwesome.Sharp.IconButton iconButton1;
        private DataGridViewTextBoxColumn CondicionProducto;
        private DataGridViewTextBoxColumn colColor;
        private DataGridViewTextBoxColumn Colmodelo2;
        private DataGridViewTextBoxColumn Precio;
        private DataGridViewTextBoxColumn colStockFisico;
        private DataGridViewTextBoxColumn ColComprometido;
        private DataGridViewTextBoxColumn ColDisponible;
        private DataGridViewTextBoxColumn NombreProducto;
        private DataGridViewTextBoxColumn colProducto;
        private DataGridViewTextBoxColumn idVariante;
        private DataGridViewTextBoxColumn colVarianteIngreso;
        private DataGridViewTextBoxColumn colCantidad;
        private DataGridViewTextBoxColumn colCosto;
        private DataGridViewTextBoxColumn colTotal;
        private DataGridViewTextBoxColumn colIdVariante;
        private DataGridViewTextBoxColumn colModelo;
        private DataGridViewTextBoxColumn colNombre2;
        private DataGridViewTextBoxColumn colDescripcion;
        private DataGridViewTextBoxColumn colColorb;
        private DataGridViewTextBoxColumn colCondicion;
        private DataGridViewTextBoxColumn colPrecio;
        private DataGridViewTextBoxColumn colCosto2;
        private DataGridViewTextBoxColumn colStockFisicoBusqueda;
        private DataGridViewTextBoxColumn colComprometidoBusqueda;
        private DataGridViewButtonColumn colModificar;
        private DataGridViewButtonColumn ColEliminar;
    }
}