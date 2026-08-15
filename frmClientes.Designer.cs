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
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            panel1 = new Panel();
            panel2 = new Panel();
            btn_CrearCliente = new Button();
            Localidad = new Label();
            textBoxLocalidad = new TextBox();
            label5 = new Label();
            textBoxDireccion = new TextBox();
            dgv_listadoClientes = new DataGridView();
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
            PnlBusquedaCliente = new Panel();
            btnBuscarProducto = new FontAwesome.Sharp.IconButton();
            txtBuscarProducto = new TextBox();
            lblcodigo = new Label();
            label1 = new Label();
            colNombre = new DataGridViewTextBoxColumn();
            colApellido = new DataGridViewTextBoxColumn();
            colDNI = new DataGridViewTextBoxColumn();
            ColTelefono = new DataGridViewTextBoxColumn();
            colDireccion = new DataGridViewTextBoxColumn();
            colLocalidad = new DataGridViewTextBoxColumn();
            colAccion = new DataGridViewButtonColumn();
            colFechaRegistro = new DataGridViewTextBoxColumn();
            colSaldoFavor = new DataGridViewTextBoxColumn();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgv_listadoClientes).BeginInit();
            pnlEncabezadoClientes.SuspendLayout();
            PnlBusquedaCliente.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            panel1.AutoScroll = true;
            panel1.AutoSize = true;
            panel1.BackColor = Color.FromArgb(13, 19, 27);
            panel1.Controls.Add(panel2);
            panel1.Controls.Add(pnlEncabezadoClientes);
            panel1.Location = new Point(2, 1);
            panel1.Name = "panel1";
            panel1.Size = new Size(1460, 536);
            panel1.TabIndex = 0;
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
            panel2.Location = new Point(3, 225);
            panel2.Name = "panel2";
            panel2.Size = new Size(1439, 308);
            panel2.TabIndex = 8;
            // 
            // btn_CrearCliente
            // 
            btn_CrearCliente.Location = new Point(42, 220);
            btn_CrearCliente.Name = "btn_CrearCliente";
            btn_CrearCliente.Size = new Size(109, 23);
            btn_CrearCliente.TabIndex = 44;
            btn_CrearCliente.Text = "Crear cliente";
            btn_CrearCliente.UseVisualStyleBackColor = true;
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
            label5.Size = new Size(57, 15);
            label5.TabIndex = 41;
            label5.Text = "Direccion";
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
            dgv_listadoClientes.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            dgv_listadoClientes.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgv_listadoClientes.Columns.AddRange(new DataGridViewColumn[] { colNombre, colApellido, colDNI, ColTelefono, colDireccion, colLocalidad, colAccion, colFechaRegistro, colSaldoFavor });
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = SystemColors.Window;
            dataGridViewCellStyle4.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle4.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle4.Format = "c2";
            dataGridViewCellStyle4.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.False;
            dgv_listadoClientes.DefaultCellStyle = dataGridViewCellStyle4;
            dgv_listadoClientes.Location = new Point(423, 31);
            dgv_listadoClientes.Name = "dgv_listadoClientes";
            dgv_listadoClientes.Size = new Size(1009, 194);
            dgv_listadoClientes.TabIndex = 39;
            dgv_listadoClientes.CellClick += dgv_listadoClientes_CellClick;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            label4.ForeColor = Color.White;
            label4.Location = new Point(422, 0);
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
            // PnlBusquedaCliente
            // 
            PnlBusquedaCliente.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            PnlBusquedaCliente.AutoSize = true;
            PnlBusquedaCliente.BackColor = Color.FromArgb(21, 30, 41);
            PnlBusquedaCliente.Controls.Add(btnBuscarProducto);
            PnlBusquedaCliente.Controls.Add(txtBuscarProducto);
            PnlBusquedaCliente.Controls.Add(lblcodigo);
            PnlBusquedaCliente.Controls.Add(label1);
            PnlBusquedaCliente.Location = new Point(2, 72);
            PnlBusquedaCliente.Name = "PnlBusquedaCliente";
            PnlBusquedaCliente.Size = new Size(1238, 145);
            PnlBusquedaCliente.TabIndex = 8;
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
            btnBuscarProducto.Location = new Point(399, 51);
            btnBuscarProducto.Name = "btnBuscarProducto";
            btnBuscarProducto.Size = new Size(100, 25);
            btnBuscarProducto.TabIndex = 6;
            btnBuscarProducto.Text = "Buscar";
            btnBuscarProducto.UseVisualStyleBackColor = false;
            btnBuscarProducto.Click += btnBuscarProducto_Click;
            // 
            // txtBuscarProducto
            // 
            txtBuscarProducto.Font = new Font("Segoe UI", 10F);
            txtBuscarProducto.Location = new Point(15, 51);
            txtBuscarProducto.Name = "txtBuscarProducto";
            txtBuscarProducto.PlaceholderText = "Buscar por DNI, telefono o nombre";
            txtBuscarProducto.Size = new Size(378, 25);
            txtBuscarProducto.TabIndex = 5;
            txtBuscarProducto.TextChanged += txtBuscarProducto_TextChanged;
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
            colAccion.HeaderText = "Accion";
            colAccion.Name = "colAccion";
            colAccion.Resizable = DataGridViewTriState.True;
            colAccion.SortMode = DataGridViewColumnSortMode.Automatic;
            colAccion.Text = "Modificar";
            // 
            // colFechaRegistro
            // 
            colFechaRegistro.DataPropertyName = "FechaRegistro";
            dataGridViewCellStyle3.Format = "g";
            dataGridViewCellStyle3.NullValue = null;
            colFechaRegistro.DefaultCellStyle = dataGridViewCellStyle3;
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
            // frmClientes
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            AutoSize = true;
            ClientSize = new Size(1460, 534);
            Controls.Add(PnlBusquedaCliente);
            Controls.Add(panel1);
            Name = "frmClientes";
            Text = "frmClientes";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgv_listadoClientes).EndInit();
            pnlEncabezadoClientes.ResumeLayout(false);
            pnlEncabezadoClientes.PerformLayout();
            PnlBusquedaCliente.ResumeLayout(false);
            PnlBusquedaCliente.PerformLayout();
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
        private TextBox txtBuscarProducto;
        private Panel panel2;
        private Label label2;
        private FontAwesome.Sharp.IconButton btnBuscarProducto;
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
    }
}