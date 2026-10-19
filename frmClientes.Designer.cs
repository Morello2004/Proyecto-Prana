namespace Prana.Presentacion
{
    partial class frmClientes
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
            components = new System.ComponentModel.Container();
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            esperaBusqueda = new System.Windows.Forms.Timer(components);
            colVerDeuda = new DataGridViewButtonColumn();
            colEstadoCuenta = new DataGridViewTextBoxColumn();
            colDeudaPendiente = new DataGridViewTextBoxColumn();
            colCodigoPostal = new DataGridViewTextBoxColumn();
            sugerenciasClientes = new ListBox();
            lblCodigoPostal = new Label();
            txtCodigoPostal = new TextBox();
            panel1 = new Panel();
            PnlBusquedaCliente = new Panel();
            btnBuscarClientes = new FontAwesome.Sharp.IconButton();
            txtBuscarCliente = new TextBox();
            lblcodigo = new Label();
            label1 = new Label();
            panel2 = new Panel();
            btn_CrearCliente = new Button();
            Localidad = new Label();
            textBoxLocalidad = new TextBox();
            label5 = new Label();
            textBoxDireccion = new TextBox();
            dgv_listadoClientes = new DataGridView();
            colNombre = new DataGridViewTextBoxColumn();
            colApellido = new DataGridViewTextBoxColumn();
            colDNI = new DataGridViewTextBoxColumn();
            ColTelefono = new DataGridViewTextBoxColumn();
            colDireccion = new DataGridViewTextBoxColumn();
            colLocalidad = new DataGridViewTextBoxColumn();
            colAccion = new DataGridViewButtonColumn();
            colFechaRegistro = new DataGridViewTextBoxColumn();
            colSaldoFavor = new DataGridViewTextBoxColumn();
            label4 = new Label();
            btnCancelar = new FontAwesome.Sharp.IconButton();
            textBoxDNI = new TextBox();
            textBoxTelefono = new TextBox();
            label10 = new Label();
            label8 = new Label();
            textBoxApellido = new TextBox();
            label3 = new Label();
            lblVC = new Label();
            textBoxNombre = new TextBox();
            label2 = new Label();
            pnlEncabezadoClientes = new Panel();
            LBLtituloClientes = new Label();
            panel1.SuspendLayout();
            PnlBusquedaCliente.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgv_listadoClientes).BeginInit();
            pnlEncabezadoClientes.SuspendLayout();
            SuspendLayout();
            // 
            // esperaBusqueda
            // 
            esperaBusqueda.Interval = 250;
            esperaBusqueda.Tick += esperaBusqueda_Tick;
            // 
            // colVerDeuda
            // 
            colVerDeuda.HeaderText = "Pendientes";
            colVerDeuda.Name = "colVerDeuda";
            colVerDeuda.ReadOnly = true;
            colVerDeuda.Text = "Ver detalle";
            colVerDeuda.UseColumnTextForButtonValue = true;
            // 
            // colEstadoCuenta
            // 
            colEstadoCuenta.DataPropertyName = "EstadoCuenta";
            colEstadoCuenta.HeaderText = "Cuenta";
            colEstadoCuenta.Name = "colEstadoCuenta";
            colEstadoCuenta.ReadOnly = true;
            // 
            // colDeudaPendiente
            // 
            colDeudaPendiente.DataPropertyName = "DeudaPendiente";
            colDeudaPendiente.HeaderText = "Deuda pendiente";
            colDeudaPendiente.Name = "colDeudaPendiente";
            colDeudaPendiente.ReadOnly = true;
            colDeudaPendiente.Width = 125;
            // 
            // colCodigoPostal
            // 
            colCodigoPostal.DataPropertyName = "CodigoPostal";
            colCodigoPostal.HeaderText = "Código postal";
            colCodigoPostal.Name = "colCodigoPostal";
            // 
            // sugerenciasClientes
            // 
            sugerenciasClientes.HorizontalScrollbar = true;
            sugerenciasClientes.IntegralHeight = false;
            sugerenciasClientes.Location = new Point(0, 0);
            sugerenciasClientes.Name = "sugerenciasClientes";
            sugerenciasClientes.Size = new Size(350, 120);
            sugerenciasClientes.TabIndex = 1;
            sugerenciasClientes.Visible = false;
            sugerenciasClientes.Click += sugerenciasClientes_Click;
            sugerenciasClientes.KeyDown += Buscador_KeyDown;
            sugerenciasClientes.Leave += sugerenciasClientes_Leave;
            // 
            // lblCodigoPostal
            // 
            lblCodigoPostal.AutoSize = true;
            lblCodigoPostal.ForeColor = Color.White;
            lblCodigoPostal.Location = new Point(410, 147);
            lblCodigoPostal.Name = "lblCodigoPostal";
            lblCodigoPostal.Size = new Size(138, 15);
            lblCodigoPostal.TabIndex = 46;
            lblCodigoPostal.Text = "Código postal (opcional)";
            // 
            // txtCodigoPostal
            // 
            txtCodigoPostal.Location = new Point(410, 165);
            txtCodigoPostal.MaxLength = 12;
            txtCodigoPostal.Name = "txtCodigoPostal";
            txtCodigoPostal.Size = new Size(170, 23);
            txtCodigoPostal.TabIndex = 45;
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            panel1.AutoScroll = true;
            panel1.AutoSize = true;
            panel1.BackColor = Color.FromArgb(13, 19, 27);
            panel1.Controls.Add(PnlBusquedaCliente);
            panel1.Controls.Add(panel2);
            panel1.Controls.Add(pnlEncabezadoClientes);
            panel1.Location = new Point(2, 1);
            panel1.Name = "panel1";
            panel1.Size = new Size(1460, 715);
            panel1.TabIndex = 0;
            // 
            // PnlBusquedaCliente
            // 
            PnlBusquedaCliente.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            PnlBusquedaCliente.AutoSize = true;
            PnlBusquedaCliente.BackColor = Color.FromArgb(21, 30, 41);
            PnlBusquedaCliente.Controls.Add(btnBuscarClientes);
            PnlBusquedaCliente.Controls.Add(txtBuscarCliente);
            PnlBusquedaCliente.Controls.Add(lblcodigo);
            PnlBusquedaCliente.Controls.Add(label1);
            PnlBusquedaCliente.Location = new Point(2, 72);
            PnlBusquedaCliente.Name = "PnlBusquedaCliente";
            PnlBusquedaCliente.Size = new Size(1238, 145);
            PnlBusquedaCliente.TabIndex = 8;
            // 
            // btnBuscarClientes
            // 
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
            // 
            // txtBuscarCliente
            // 
            txtBuscarCliente.Font = new Font("Segoe UI", 10F);
            txtBuscarCliente.Location = new Point(15, 51);
            txtBuscarCliente.Name = "txtBuscarCliente";
            txtBuscarCliente.PlaceholderText = "Buscar por DNI, telefono o nombre";
            txtBuscarCliente.Size = new Size(378, 25);
            txtBuscarCliente.TabIndex = 5;
            txtBuscarCliente.TextChanged += txtBuscarCliente_TextChanged;
            txtBuscarCliente.KeyDown += Buscador_KeyDown;
            txtBuscarCliente.Leave += txtBuscarCliente_Leave;
            // 
            // lblcodigo
            // 
            lblcodigo.AutoSize = true;
            lblcodigo.ForeColor = Color.FromArgb(145, 170, 210);
            lblcodigo.Location = new Point(10, 33);
            lblcodigo.Name = "lblcodigo";
            lblcodigo.Size = new Size(42, 15);
            lblcodigo.TabIndex = 4;
            lblcodigo.Text = "Buscar";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            label1.ForeColor = Color.White;
            label1.Location = new Point(10, 0);
            label1.Name = "label1";
            label1.Size = new Size(85, 21);
            label1.TabIndex = 0;
            label1.Text = "Busqueda";
            // 
            // panel2
            // 
            panel2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            panel2.BackColor = Color.FromArgb(21, 30, 41);
            panel2.Controls.Add(btn_CrearCliente);
            panel2.Controls.Add(Localidad);
            panel2.Controls.Add(textBoxLocalidad);
            panel2.Controls.Add(label5);
            panel2.Controls.Add(textBoxDireccion);
            panel2.Controls.Add(dgv_listadoClientes);
            panel2.Controls.Add(label4);
            panel2.Controls.Add(btnCancelar);
            panel2.Controls.Add(textBoxDNI);
            panel2.Controls.Add(textBoxTelefono);
            panel2.Controls.Add(label10);
            panel2.Controls.Add(label8);
            panel2.Controls.Add(textBoxApellido);
            panel2.Controls.Add(label3);
            panel2.Controls.Add(lblVC);
            panel2.Controls.Add(textBoxNombre);
            panel2.Controls.Add(label2);
            panel2.Controls.Add(txtCodigoPostal);
            panel2.Controls.Add(lblCodigoPostal);
            panel2.Location = new Point(3, 225);
            panel2.Name = "panel2";
            panel2.Size = new Size(1439, 487);
            panel2.TabIndex = 8;
            // 
            // btn_CrearCliente
            // 
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
            // 
            // Localidad
            // 
            Localidad.AutoSize = true;
            Localidad.ForeColor = Color.FromArgb(145, 170, 210);
            Localidad.Location = new Point(203, 147);
            Localidad.Name = "Localidad";
            Localidad.Size = new Size(58, 15);
            Localidad.TabIndex = 43;
            Localidad.Text = "Localidad";
            // 
            // textBoxLocalidad
            // 
            textBoxLocalidad.Font = new Font("Segoe UI", 10F);
            textBoxLocalidad.Location = new Point(203, 165);
            textBoxLocalidad.Name = "textBoxLocalidad";
            textBoxLocalidad.Size = new Size(187, 25);
            textBoxLocalidad.TabIndex = 42;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.ForeColor = Color.FromArgb(145, 170, 210);
            label5.Location = new Point(2, 147);
            label5.Name = "label5";
            label5.Size = new Size(114, 15);
            label5.TabIndex = 41;
            label5.Text = "Dirección (opcional)";
            // 
            // textBoxDireccion
            // 
            textBoxDireccion.Font = new Font("Segoe UI", 10F);
            textBoxDireccion.Location = new Point(2, 165);
            textBoxDireccion.Name = "textBoxDireccion";
            textBoxDireccion.Size = new Size(168, 25);
            textBoxDireccion.TabIndex = 40;
            // 
            // dgv_listadoClientes
            // 
            dgv_listadoClientes.AllowUserToAddRows = false;
            dgv_listadoClientes.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(236, 242, 249);
            dgv_listadoClientes.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dgv_listadoClientes.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            dgv_listadoClientes.BackgroundColor = Color.FromArgb(13, 19, 27);
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(37, 52, 70);
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = Color.White;
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(37, 52, 70);
            dataGridViewCellStyle2.SelectionForeColor = Color.White;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dgv_listadoClientes.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dgv_listadoClientes.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgv_listadoClientes.Columns.AddRange(new DataGridViewColumn[] { colNombre, colApellido, colDNI, ColTelefono, colDireccion, colLocalidad, colAccion, colFechaRegistro, colSaldoFavor, colCodigoPostal, colDeudaPendiente, colEstadoCuenta, colVerDeuda });
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = Color.White;
            dataGridViewCellStyle5.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle5.ForeColor = Color.FromArgb(21, 30, 41);
            dataGridViewCellStyle5.Format = "c2";
            dataGridViewCellStyle5.SelectionBackColor = Color.FromArgb(35, 100, 166);
            dataGridViewCellStyle5.SelectionForeColor = Color.White;
            dataGridViewCellStyle5.WrapMode = DataGridViewTriState.False;
            dgv_listadoClientes.DefaultCellStyle = dataGridViewCellStyle5;
            dgv_listadoClientes.EnableHeadersVisualStyles = false;
            dgv_listadoClientes.Location = new Point(7, 288);
            dgv_listadoClientes.Name = "dgv_listadoClientes";
            dgv_listadoClientes.RowTemplate.Height = 30;
            dgv_listadoClientes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv_listadoClientes.Size = new Size(1429, 194);
            dgv_listadoClientes.TabIndex = 39;
            dgv_listadoClientes.CellClick += dgv_listadoClientes_CellClick;
            dgv_listadoClientes.CellFormatting += dgv_listadoClientes_CellFormatting;
            // 
            // colNombre
            // 
            colNombre.DataPropertyName = "Nombre";
            colNombre.HeaderText = "Nombre";
            colNombre.Name = "colNombre";
            // 
            // colApellido
            // 
            colApellido.DataPropertyName = "Apellido";
            colApellido.HeaderText = "Apellido";
            colApellido.Name = "colApellido";
            // 
            // colDNI
            // 
            colDNI.DataPropertyName = "DNI";
            colDNI.HeaderText = "DNI";
            colDNI.Name = "colDNI";
            // 
            // ColTelefono
            // 
            ColTelefono.DataPropertyName = "Telefono";
            ColTelefono.HeaderText = "Telefono";
            ColTelefono.Name = "ColTelefono";
            // 
            // colDireccion
            // 
            colDireccion.DataPropertyName = "Direccion";
            colDireccion.HeaderText = "Direccion";
            colDireccion.Name = "colDireccion";
            // 
            // colLocalidad
            // 
            colLocalidad.DataPropertyName = "NombreLocalidad";
            colLocalidad.HeaderText = "Localidad";
            colLocalidad.Name = "colLocalidad";
            // 
            // colAccion
            // 
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(21, 30, 41);
            dataGridViewCellStyle3.NullValue = "Modificar cliente";
            colAccion.DefaultCellStyle = dataGridViewCellStyle3;
            colAccion.HeaderText = "Acciones";
            colAccion.Name = "colAccion";
            colAccion.Resizable = DataGridViewTriState.True;
            colAccion.SortMode = DataGridViewColumnSortMode.Automatic;
            colAccion.Text = "Modificar cliente";
            colAccion.Width = 145;
            // 
            // colFechaRegistro
            // 
            colFechaRegistro.DataPropertyName = "FechaRegistro";
            dataGridViewCellStyle4.Format = "g";
            dataGridViewCellStyle4.NullValue = null;
            colFechaRegistro.DefaultCellStyle = dataGridViewCellStyle4;
            colFechaRegistro.HeaderText = "Fecha de registro";
            colFechaRegistro.Name = "colFechaRegistro";
            colFechaRegistro.ReadOnly = true;
            // 
            // colSaldoFavor
            // 
            colSaldoFavor.DataPropertyName = "SaldoFavor";
            colSaldoFavor.HeaderText = "Saldo a favor\n";
            colSaldoFavor.Name = "colSaldoFavor";
            colSaldoFavor.ReadOnly = true;
            colSaldoFavor.Resizable = DataGridViewTriState.False;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            label4.ForeColor = Color.White;
            label4.Location = new Point(9, 264);
            label4.Name = "label4";
            label4.Size = new Size(151, 21);
            label4.TabIndex = 38;
            label4.Text = "Listado de clientes";
            // 
            // btnCancelar
            // 
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
            // 
            // textBoxDNI
            // 
            textBoxDNI.Font = new Font("Segoe UI", 10F);
            textBoxDNI.Location = new Point(203, 107);
            textBoxDNI.Name = "textBoxDNI";
            textBoxDNI.Size = new Size(187, 25);
            textBoxDNI.TabIndex = 33;
            // 
            // textBoxTelefono
            // 
            textBoxTelefono.Font = new Font("Segoe UI", 10F);
            textBoxTelefono.Location = new Point(2, 107);
            textBoxTelefono.Name = "textBoxTelefono";
            textBoxTelefono.Size = new Size(168, 25);
            textBoxTelefono.TabIndex = 32;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.ForeColor = Color.FromArgb(145, 170, 210);
            label10.Location = new Point(203, 89);
            label10.Name = "label10";
            label10.Size = new Size(27, 15);
            label10.TabIndex = 31;
            label10.Text = "DNI";
            label10.Click += label10_Click;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.ForeColor = Color.FromArgb(145, 170, 210);
            label8.Location = new Point(203, 34);
            label8.Name = "label8";
            label8.Size = new Size(51, 15);
            label8.TabIndex = 29;
            label8.Text = "Apellido";
            // 
            // textBoxApellido
            // 
            textBoxApellido.Font = new Font("Segoe UI", 10F);
            textBoxApellido.Location = new Point(203, 52);
            textBoxApellido.Name = "textBoxApellido";
            textBoxApellido.Size = new Size(187, 25);
            textBoxApellido.TabIndex = 28;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.ForeColor = Color.FromArgb(145, 170, 210);
            label3.Location = new Point(0, 89);
            label3.Name = "label3";
            label3.Size = new Size(52, 15);
            label3.TabIndex = 27;
            label3.Text = "Telefono";
            // 
            // lblVC
            // 
            lblVC.AutoSize = true;
            lblVC.ForeColor = Color.FromArgb(145, 170, 210);
            lblVC.Location = new Point(2, 34);
            lblVC.Name = "lblVC";
            lblVC.Size = new Size(51, 15);
            lblVC.TabIndex = 25;
            lblVC.Text = "Nombre";
            // 
            // textBoxNombre
            // 
            textBoxNombre.Font = new Font("Segoe UI", 10F);
            textBoxNombre.Location = new Point(2, 52);
            textBoxNombre.Name = "textBoxNombre";
            textBoxNombre.Size = new Size(168, 25);
            textBoxNombre.TabIndex = 24;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            label2.ForeColor = Color.White;
            label2.Location = new Point(-3, 0);
            label2.Name = "label2";
            label2.Size = new Size(106, 21);
            label2.TabIndex = 1;
            label2.Text = "Crear cliente";
            label2.Click += label2_Click;
            // 
            // pnlEncabezadoClientes
            // 
            pnlEncabezadoClientes.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pnlEncabezadoClientes.AutoScroll = true;
            pnlEncabezadoClientes.AutoSize = true;
            pnlEncabezadoClientes.BackColor = Color.FromArgb(21, 30, 41);
            pnlEncabezadoClientes.Controls.Add(LBLtituloClientes);
            pnlEncabezadoClientes.Location = new Point(0, 0);
            pnlEncabezadoClientes.Name = "pnlEncabezadoClientes";
            pnlEncabezadoClientes.Size = new Size(1457, 65);
            pnlEncabezadoClientes.TabIndex = 7;
            // 
            // LBLtituloClientes
            // 
            LBLtituloClientes.AutoSize = true;
            LBLtituloClientes.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            LBLtituloClientes.ForeColor = Color.White;
            LBLtituloClientes.Location = new Point(15, 10);
            LBLtituloClientes.Name = "LBLtituloClientes";
            LBLtituloClientes.Size = new Size(104, 32);
            LBLtituloClientes.TabIndex = 0;
            LBLtituloClientes.Text = "Clientes";
            // 
            // frmClientes
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            AutoSize = true;
            ClientSize = new Size(1460, 534);
            Controls.Add(panel1);
            Controls.Add(sugerenciasClientes);
            Name = "frmClientes";
            Text = "frmClientes";
            Load += frmClientes_Load;
            Disposed += frmClientes_Disposed;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            PnlBusquedaCliente.ResumeLayout(false);
            PnlBusquedaCliente.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgv_listadoClientes).EndInit();
            pnlEncabezadoClientes.ResumeLayout(false);
            pnlEncabezadoClientes.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel panel1;
        private Panel pnlEncabezadoClientes;
        private Label LBLtituloClientes;
        private Panel PnlBusquedaCliente;
        private Label label1;
        private Label lblcodigo;
        private TextBox txtBuscarCliente;
        private Panel panel2;
        private Label label2;
        private FontAwesome.Sharp.IconButton btnBuscarClientes;
        private TextBox textBoxTelefono;
        private Label label10;
        private Label label8;
        private TextBox textBoxApellido;
        private Label label3;
        private Label lblVC;
        private TextBox textBoxNombre;
        private TextBox textBoxDNI;
        private FontAwesome.Sharp.IconButton btnCancelar;
        private DataGridView dgv_listadoClientes;
        private Label label4;
        private Label label5;
        private TextBox textBoxDireccion;
        private Label Localidad;
        private TextBox textBoxLocalidad;
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
        private TextBox txtCodigoPostal;

        private Label lblCodigoPostal;

        private ListBox sugerenciasClientes;

        private DataGridViewTextBoxColumn colCodigoPostal;

        private DataGridViewTextBoxColumn colDeudaPendiente;

        private DataGridViewTextBoxColumn colEstadoCuenta;

        private DataGridViewButtonColumn colVerDeuda;

        private System.Windows.Forms.Timer esperaBusqueda;

    }
}
