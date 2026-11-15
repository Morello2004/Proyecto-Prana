namespace Prana.Presentacion
{
    // parte visual de buttonseleccionarproducto crea controles y define su distribucion, colores y eventos; visual studio puede regenerar este archivo al editar el disenador
    partial class buttonSeleccionarProducto
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
            // llama a ((systemcomponentmodelisupportinitialize)dgvproductos)begininit para realizar este paso del flujo
            ((System.ComponentModel.ISupportInitialize)dgvProductos).BeginInit();
            SuspendLayout();
            // buttonbuscarproducto buttonbuscarproductolocation: posicion horizontal y vertical respecto del contenedor
            buttonBuscarProducto.Location = new Point(374, 12);
            buttonBuscarProducto.Name = "buttonBuscarProducto";
            buttonBuscarProducto.Size = new Size(75, 23);
            buttonBuscarProducto.TabIndex = 0;
            buttonBuscarProducto.Text = "Buscar";
            buttonBuscarProducto.UseVisualStyleBackColor = true;
            buttonBuscarProducto.Click += buttonBuscarProducto_Click;
            // textbox1 textbox1location: posicion horizontal y vertical respecto del contenedor
            textBox1.Location = new Point(36, 12);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(298, 23);
            textBox1.TabIndex = 3;
            textBox1.TextChanged += textBox1_TextChanged;
            // dgvproductos
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
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(190, 220, 250);
            dataGridViewCellStyle2.SelectionForeColor = Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(190, 220, 250);
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dgvProductos.DefaultCellStyle = dataGridViewCellStyle2;
            dgvProductos.Location = new Point(12, 71);
            dgvProductos.MultiSelect = false;
            dgvProductos.Name = "dgvProductos";
            dgvProductos.ForeColor = Color.Black;
            dgvProductos.DefaultCellStyle.ForeColor = Color.Black;
            dgvProductos.RowsDefaultCellStyle.ForeColor = Color.Black;
            dgvProductos.AlternatingRowsDefaultCellStyle.ForeColor = Color.Black;
            dgvProductos.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.Black;
            dgvProductos.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(190, 220, 250);
            dgvProductos.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvProductos.DefaultCellStyle.SelectionBackColor = Color.FromArgb(190, 220, 250);
            dgvProductos.AlternatingRowsDefaultCellStyle.SelectionForeColor = Color.Black;
            dgvProductos.AlternatingRowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(190, 220, 250);
            dgvProductos.ReadOnly = true;
            dgvProductos.RowHeadersVisible = false;
            dgvProductos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProductos.Size = new Size(587, 217);
            dgvProductos.TabIndex = 4;
            dgvProductos.CellClick += dgvProductos_CellClick;
            dgvProductos.CellContentClick += dgvProductos_CellContentClick;
            // colidvariante colidvariantedatapropertyname: propiedad del objeto enlazado cuyo valor muestra esta columna
            colIdVariante.DataPropertyName = "IdVariante";
            colIdVariante.HeaderText = "Id variante";
            colIdVariante.Name = "colIdVariante";
            colIdVariante.ReadOnly = true;
            colIdVariante.Visible = false;
            // colproducto colproductodatapropertyname: propiedad del objeto enlazado cuyo valor muestra esta columna
            colProducto.DataPropertyName = "NombreProducto";
            colProducto.HeaderText = "Producto";
            colProducto.Name = "colProducto";
            colProducto.ReadOnly = true;
            // colmodelo colmodelodatapropertyname: propiedad del objeto enlazado cuyo valor muestra esta columna
            colModelo.DataPropertyName = "Modelo";
            colModelo.HeaderText = "Modelo";
            colModelo.Name = "colModelo";
            colModelo.ReadOnly = true;
            // colcolor colcolordatapropertyname: propiedad del objeto enlazado cuyo valor muestra esta columna
            colColor.DataPropertyName = "Color";
            colColor.HeaderText = "Color";
            colColor.Name = "colColor";
            colColor.ReadOnly = true;
            // colcondicion colcondiciondatapropertyname: propiedad del objeto enlazado cuyo valor muestra esta columna
            colCondicion.DataPropertyName = "CondicionProducto";
            colCondicion.HeaderText = "Condición";
            colCondicion.Name = "colCondicion";
            colCondicion.ReadOnly = true;
            // colstock colstockdatapropertyname: propiedad del objeto enlazado cuyo valor muestra esta columna
            colStock.DataPropertyName = "StockFisico";
            dataGridViewCellStyle1.Format = "N0";
            dataGridViewCellStyle1.NullValue = null;
            colStock.DefaultCellStyle = dataGridViewCellStyle1;
            colStock.HeaderText = "Stock";
            colStock.Name = "colStock";
            colStock.ReadOnly = true;
            // colcosto colcostodatapropertyname: propiedad del objeto enlazado cuyo valor muestra esta columna
            colCosto.DataPropertyName = "Costo";
            colCosto.HeaderText = "Costo actual";
            colCosto.Name = "colCosto";
            colCosto.ReadOnly = true;
            // colagregar colagregarheadertext: titulo de la columna; no es el nombre de la propiedad enlazada
            colAgregar.HeaderText = "Accion";
            colAgregar.Name = "colAgregar";
            colAgregar.DefaultCellStyle.ForeColor = Color.Black;
            colAgregar.DefaultCellStyle.SelectionForeColor = Color.Black;
            colAgregar.DefaultCellStyle.BackColor = Color.Gainsboro;
            colAgregar.DefaultCellStyle.SelectionBackColor = Color.Gainsboro;
            colAgregar.ReadOnly = true;
            colAgregar.Text = "Agregar";
            colAgregar.UseColumnTextForButtonValue = true;
            // btnagregarproducto btnagregarproductolocation: posicion horizontal y vertical respecto del contenedor
            btnAgregarProducto.Location = new Point(12, 307);
            btnAgregarProducto.Name = "btnAgregarProducto";
            btnAgregarProducto.Size = new Size(129, 23);
            btnAgregarProducto.TabIndex = 5;
            btnAgregarProducto.Text = "Seleccionar producto";
            btnAgregarProducto.UseVisualStyleBackColor = true;
            // buttoncancelar buttoncancelarlocation: posicion horizontal y vertical respecto del contenedor
            buttonCancelar.Location = new Point(162, 307);
            buttonCancelar.Name = "buttonCancelar";
            buttonCancelar.Size = new Size(129, 23);
            buttonCancelar.TabIndex = 6;
            buttonCancelar.Text = "Cancelar";
            buttonCancelar.UseVisualStyleBackColor = true;
            buttonCancelar.Click += buttonCancelar_Click;
            // buttonseleccionarproducto
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
            // llama a ((systemcomponentmodelisupportinitialize)dgvproductos)endinit para realizar este paso del flujo
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
