namespace Prana.Presentacion
{
    partial class buttonSeleccionarProducto
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
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            buttonBuscarProducto = new Button();
            textBox1 = new TextBox();
            dgvProductos = new DataGridView();
            colIdVariante = new DataGridViewTextBoxColumn();
            colProducto = new DataGridViewTextBoxColumn();
            colModelo = new DataGridViewTextBoxColumn();
            colColor = new DataGridViewTextBoxColumn();
            colCondicion = new DataGridViewTextBoxColumn();
            colStock = new DataGridViewTextBoxColumn();
            colCosto = new DataGridViewTextBoxColumn();
            colAgregar = new DataGridViewButtonColumn();
            btnAgregarProducto = new Button();
            buttonCancelar = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvProductos).BeginInit();
            SuspendLayout();
            // 
            // buttonBuscarProducto
            // 
            buttonBuscarProducto.Location = new Point(374, 12);
            buttonBuscarProducto.Name = "buttonBuscarProducto";
            buttonBuscarProducto.Size = new Size(75, 23);
            buttonBuscarProducto.TabIndex = 0;
            buttonBuscarProducto.Text = "Buscar";
            buttonBuscarProducto.UseVisualStyleBackColor = true;
            buttonBuscarProducto.Click += buttonBuscarProducto_Click;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(36, 12);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(298, 23);
            textBox1.TabIndex = 3;
            textBox1.TextChanged += textBox1_TextChanged;
            // 
            // dgvProductos
            // 
            dgvProductos.AllowUserToAddRows = false;
            dgvProductos.AllowUserToDeleteRows = false;
            dgvProductos.AllowUserToResizeRows = false;
            dgvProductos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvProductos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvProductos.Columns.AddRange(new DataGridViewColumn[] { colIdVariante, colProducto, colModelo, colColor, colCondicion, colStock, colCosto, colAgregar });
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = SystemColors.Window;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle2.Format = "C2";
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dgvProductos.DefaultCellStyle = dataGridViewCellStyle2;
            dgvProductos.Location = new Point(12, 71);
            dgvProductos.MultiSelect = false;
            dgvProductos.Name = "dgvProductos";
            dgvProductos.ReadOnly = true;
            dgvProductos.RowHeadersVisible = false;
            dgvProductos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProductos.Size = new Size(587, 217);
            dgvProductos.TabIndex = 4;
            dgvProductos.CellClick += dgvProductos_CellClick;
            dgvProductos.CellContentClick += dgvProductos_CellContentClick;
            // 
            // colIdVariante
            // 
            colIdVariante.DataPropertyName = "IdVariante";
            colIdVariante.HeaderText = "Id variante";
            colIdVariante.Name = "colIdVariante";
            colIdVariante.ReadOnly = true;
            colIdVariante.Visible = false;
            // 
            // colProducto
            // 
            colProducto.DataPropertyName = "NombreProducto";
            colProducto.HeaderText = "Producto";
            colProducto.Name = "colProducto";
            colProducto.ReadOnly = true;
            // 
            // colModelo
            // 
            colModelo.DataPropertyName = "Modelo";
            colModelo.HeaderText = "Modelo";
            colModelo.Name = "colModelo";
            colModelo.ReadOnly = true;
            // 
            // colColor
            // 
            colColor.DataPropertyName = "Color";
            colColor.HeaderText = "Color";
            colColor.Name = "colColor";
            colColor.ReadOnly = true;
            // 
            // colCondicion
            // 
            colCondicion.DataPropertyName = "CondicionProducto";
            colCondicion.HeaderText = "Condición";
            colCondicion.Name = "colCondicion";
            colCondicion.ReadOnly = true;
            // 
            // colStock
            // 
            colStock.DataPropertyName = "StockFisico";
            dataGridViewCellStyle1.Format = "N0";
            dataGridViewCellStyle1.NullValue = null;
            colStock.DefaultCellStyle = dataGridViewCellStyle1;
            colStock.HeaderText = "Stock";
            colStock.Name = "colStock";
            colStock.ReadOnly = true;
            // 
            // colCosto
            // 
            colCosto.DataPropertyName = "Costo";
            colCosto.HeaderText = "Costo actual";
            colCosto.Name = "colCosto";
            colCosto.ReadOnly = true;
            // 
            // colAgregar
            // 
            colAgregar.HeaderText = "Accion";
            colAgregar.Name = "colAgregar";
            colAgregar.ReadOnly = true;
            colAgregar.Text = "Agregar";
            colAgregar.UseColumnTextForButtonValue = true;
            // 
            // btnAgregarProducto
            // 
            btnAgregarProducto.Location = new Point(12, 307);
            btnAgregarProducto.Name = "btnAgregarProducto";
            btnAgregarProducto.Size = new Size(129, 23);
            btnAgregarProducto.TabIndex = 5;
            btnAgregarProducto.Text = "Seleccionar producto";
            btnAgregarProducto.UseVisualStyleBackColor = true;
            // 
            // buttonCancelar
            // 
            buttonCancelar.Location = new Point(162, 307);
            buttonCancelar.Name = "buttonCancelar";
            buttonCancelar.Size = new Size(129, 23);
            buttonCancelar.TabIndex = 6;
            buttonCancelar.Text = "Cancelar";
            buttonCancelar.UseVisualStyleBackColor = true;
            buttonCancelar.Click += buttonCancelar_Click;
            // 
            // buttonSeleccionarProducto
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(buttonCancelar);
            Controls.Add(btnAgregarProducto);
            Controls.Add(dgvProductos);
            Controls.Add(textBox1);
            Controls.Add(buttonBuscarProducto);
            Name = "buttonSeleccionarProducto";
            Text = "frmSeleccionarProducto";
            Load += frmSeleccionarProducto_Load;
            ((System.ComponentModel.ISupportInitialize)dgvProductos).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button buttonBuscarProducto;
        private TextBox textBox1;
        private DataGridView dgvProductos;
        private Button btnAgregarProducto;
        private Button buttonCancelar;
        private DataGridViewTextBoxColumn colIdVariante;
        private DataGridViewTextBoxColumn colProducto;
        private DataGridViewTextBoxColumn colModelo;
        private DataGridViewTextBoxColumn colColor;
        private DataGridViewTextBoxColumn colCondicion;
        private DataGridViewTextBoxColumn colStock;
        private DataGridViewTextBoxColumn colCosto;
        private DataGridViewButtonColumn colAgregar;
    }
}