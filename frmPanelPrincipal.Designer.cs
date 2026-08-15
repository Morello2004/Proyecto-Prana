namespace Prana.Presentacion
{
    partial class frmPanelPrincipal
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
            pnlEncabezado = new Panel();
            btnCierreCaja = new FontAwesome.Sharp.IconButton();
            btnIngresoStock = new FontAwesome.Sharp.IconButton();
            btnNuevaVenta = new FontAwesome.Sharp.IconButton();
            label2 = new Label();
            panel2 = new Panel();
            tableLayoutPanel1 = new TableLayoutPanel();
            pnlEnviosPendientes = new Panel();
            label14 = new Label();
            label13 = new Label();
            label12 = new Label();
            pnlStockBajo = new Panel();
            label11 = new Label();
            label10 = new Label();
            label9 = new Label();
            pnlPagosPendientes = new Panel();
            label8 = new Label();
            label7 = new Label();
            label6 = new Label();
            pnlVentasDia = new Panel();
            label5 = new Label();
            label4 = new Label();
            label3 = new Label();
            pnlUltimasVentas = new Panel();
            dgvUltimasVentas = new DataGridView();
            colNumeroVenta = new DataGridViewTextBoxColumn();
            colCliente = new DataGridViewTextBoxColumn();
            colFecha = new DataGridViewTextBoxColumn();
            colTotal = new DataGridViewTextBoxColumn();
            colEstado = new DataGridViewTextBoxColumn();
            label15 = new Label();
            pnlAlertas = new Panel();
            label16 = new Label();
            flowLayoutPanel1 = new FlowLayoutPanel();
            flpAlertas = new Label();
            pnlEncabezado.SuspendLayout();
            panel2.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            pnlEnviosPendientes.SuspendLayout();
            pnlStockBajo.SuspendLayout();
            pnlPagosPendientes.SuspendLayout();
            pnlVentasDia.SuspendLayout();
            pnlUltimasVentas.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvUltimasVentas).BeginInit();
            pnlAlertas.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 15F);
            label1.Location = new Point(6, 0);
            label1.Name = "label1";
            label1.Size = new Size(166, 28);
            label1.TabIndex = 0;
            label1.Text = "PANEL PRINCIPAL";
            // 
            // pnlEncabezado
            // 
            pnlEncabezado.Controls.Add(btnCierreCaja);
            pnlEncabezado.Controls.Add(btnIngresoStock);
            pnlEncabezado.Controls.Add(btnNuevaVenta);
            pnlEncabezado.Controls.Add(label2);
            pnlEncabezado.Controls.Add(label1);
            pnlEncabezado.Dock = DockStyle.Top;
            pnlEncabezado.Location = new Point(0, 0);
            pnlEncabezado.Name = "pnlEncabezado";
            pnlEncabezado.Size = new Size(800, 61);
            pnlEncabezado.TabIndex = 1;
            // 
            // btnCierreCaja
            // 
            btnCierreCaja.IconChar = FontAwesome.Sharp.IconChar.None;
            btnCierreCaja.IconColor = Color.Black;
            btnCierreCaja.IconFont = FontAwesome.Sharp.IconFont.Auto;
            btnCierreCaja.Location = new Point(700, 19);
            btnCierreCaja.Name = "btnCierreCaja";
            btnCierreCaja.Size = new Size(88, 32);
            btnCierreCaja.TabIndex = 6;
            btnCierreCaja.Text = "Cierre caja";
            btnCierreCaja.UseVisualStyleBackColor = true;
            // 
            // btnIngresoStock
            // 
            btnIngresoStock.IconChar = FontAwesome.Sharp.IconChar.None;
            btnIngresoStock.IconColor = Color.Black;
            btnIngresoStock.IconFont = FontAwesome.Sharp.IconFont.Auto;
            btnIngresoStock.Location = new Point(592, 19);
            btnIngresoStock.Name = "btnIngresoStock";
            btnIngresoStock.Size = new Size(88, 32);
            btnIngresoStock.TabIndex = 5;
            btnIngresoStock.Text = "Ingreso stock";
            btnIngresoStock.UseVisualStyleBackColor = true;
            // 
            // btnNuevaVenta
            // 
            btnNuevaVenta.IconChar = FontAwesome.Sharp.IconChar.None;
            btnNuevaVenta.IconColor = Color.Black;
            btnNuevaVenta.IconFont = FontAwesome.Sharp.IconFont.Auto;
            btnNuevaVenta.Location = new Point(488, 19);
            btnNuevaVenta.Name = "btnNuevaVenta";
            btnNuevaVenta.Size = new Size(88, 32);
            btnNuevaVenta.TabIndex = 4;
            btnNuevaVenta.Text = "Nueva venta";
            btnNuevaVenta.UseVisualStyleBackColor = true;
            btnNuevaVenta.Click += iconButton2_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 36);
            label2.Name = "label2";
            label2.Size = new Size(147, 15);
            label2.TabIndex = 2;
            label2.Text = "Resumen operativo del dia";
            // 
            // panel2
            // 
            panel2.Controls.Add(tableLayoutPanel1);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(0, 61);
            panel2.Name = "panel2";
            panel2.Size = new Size(800, 390);
            panel2.TabIndex = 2;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 4;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.Controls.Add(pnlEnviosPendientes, 3, 0);
            tableLayoutPanel1.Controls.Add(pnlStockBajo, 2, 0);
            tableLayoutPanel1.Controls.Add(pnlPagosPendientes, 1, 0);
            tableLayoutPanel1.Controls.Add(pnlVentasDia, 0, 0);
            tableLayoutPanel1.Controls.Add(pnlUltimasVentas, 0, 1);
            tableLayoutPanel1.Controls.Add(pnlAlertas, 3, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 30F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 70F));
            tableLayoutPanel1.Size = new Size(800, 390);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // pnlEnviosPendientes
            // 
            pnlEnviosPendientes.Controls.Add(label14);
            pnlEnviosPendientes.Controls.Add(label13);
            pnlEnviosPendientes.Controls.Add(label12);
            pnlEnviosPendientes.Dock = DockStyle.Fill;
            pnlEnviosPendientes.Location = new Point(603, 3);
            pnlEnviosPendientes.Name = "pnlEnviosPendientes";
            pnlEnviosPendientes.Size = new Size(194, 111);
            pnlEnviosPendientes.TabIndex = 3;
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Location = new Point(39, 68);
            label14.Name = "label14";
            label14.Size = new Size(44, 15);
            label14.TabIndex = 2;
            label14.Text = "label14";
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Location = new Point(30, 38);
            label13.Name = "label13";
            label13.Size = new Size(44, 15);
            label13.TabIndex = 1;
            label13.Text = "label13";
            // 
            // label12
            // 
            label12.Dock = DockStyle.Fill;
            label12.Location = new Point(0, 0);
            label12.Name = "label12";
            label12.Size = new Size(194, 111);
            label12.TabIndex = 0;
            label12.Text = "Envios pendientes";
            // 
            // pnlStockBajo
            // 
            pnlStockBajo.Controls.Add(label11);
            pnlStockBajo.Controls.Add(label10);
            pnlStockBajo.Controls.Add(label9);
            pnlStockBajo.Dock = DockStyle.Fill;
            pnlStockBajo.Location = new Point(403, 3);
            pnlStockBajo.Name = "pnlStockBajo";
            pnlStockBajo.Size = new Size(194, 111);
            pnlStockBajo.TabIndex = 2;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(54, 85);
            label11.Name = "label11";
            label11.Size = new Size(44, 15);
            label11.TabIndex = 2;
            label11.Text = "label11";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(54, 61);
            label10.Name = "label10";
            label10.Size = new Size(44, 15);
            label10.TabIndex = 1;
            label10.Text = "label10";
            // 
            // label9
            // 
            label9.Dock = DockStyle.Fill;
            label9.Location = new Point(0, 0);
            label9.Name = "label9";
            label9.Size = new Size(194, 111);
            label9.TabIndex = 0;
            label9.Text = "Stock bajo";
            // 
            // pnlPagosPendientes
            // 
            pnlPagosPendientes.Controls.Add(label8);
            pnlPagosPendientes.Controls.Add(label7);
            pnlPagosPendientes.Controls.Add(label6);
            pnlPagosPendientes.Dock = DockStyle.Fill;
            pnlPagosPendientes.Location = new Point(203, 3);
            pnlPagosPendientes.Name = "pnlPagosPendientes";
            pnlPagosPendientes.Size = new Size(194, 111);
            pnlPagosPendientes.TabIndex = 1;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(24, 63);
            label8.Name = "label8";
            label8.Size = new Size(38, 15);
            label8.TabIndex = 2;
            label8.Text = "label8";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(14, 41);
            label7.Name = "label7";
            label7.Size = new Size(38, 15);
            label7.TabIndex = 1;
            label7.Text = "label7";
            // 
            // label6
            // 
            label6.Dock = DockStyle.Fill;
            label6.Location = new Point(0, 0);
            label6.Name = "label6";
            label6.Size = new Size(194, 111);
            label6.TabIndex = 0;
            label6.Text = "Pagos pendientes";
            // 
            // pnlVentasDia
            // 
            pnlVentasDia.AutoSize = true;
            pnlVentasDia.Controls.Add(label5);
            pnlVentasDia.Controls.Add(label4);
            pnlVentasDia.Controls.Add(label3);
            pnlVentasDia.Dock = DockStyle.Fill;
            pnlVentasDia.Location = new Point(3, 3);
            pnlVentasDia.Name = "pnlVentasDia";
            pnlVentasDia.Size = new Size(194, 111);
            pnlVentasDia.TabIndex = 0;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(29, 71);
            label5.Name = "label5";
            label5.Size = new Size(140, 15);
            label5.TabIndex = 2;
            label5.Text = "0 operaciones registradas";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(29, 45);
            label4.Name = "label4";
            label4.Size = new Size(19, 15);
            label4.TabIndex = 1;
            label4.Text = "$0";
            label4.Click += label4_Click;
            // 
            // label3
            // 
            label3.Dock = DockStyle.Fill;
            label3.Location = new Point(0, 0);
            label3.Name = "label3";
            label3.Size = new Size(194, 111);
            label3.TabIndex = 0;
            label3.Text = "Ventas del dia";
            // 
            // pnlUltimasVentas
            // 
            tableLayoutPanel1.SetColumnSpan(pnlUltimasVentas, 3);
            pnlUltimasVentas.Controls.Add(dgvUltimasVentas);
            pnlUltimasVentas.Controls.Add(label15);
            pnlUltimasVentas.Dock = DockStyle.Fill;
            pnlUltimasVentas.Location = new Point(3, 120);
            pnlUltimasVentas.Name = "pnlUltimasVentas";
            pnlUltimasVentas.Size = new Size(594, 267);
            pnlUltimasVentas.TabIndex = 4;
            // 
            // dgvUltimasVentas
            // 
            dgvUltimasVentas.AllowUserToAddRows = false;
            dgvUltimasVentas.AllowUserToDeleteRows = false;
            dgvUltimasVentas.AllowUserToResizeColumns = false;
            dgvUltimasVentas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvUltimasVentas.BackgroundColor = Color.FromArgb(13, 19, 27);
            dgvUltimasVentas.BorderStyle = BorderStyle.None;
            dgvUltimasVentas.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvUltimasVentas.Columns.AddRange(new DataGridViewColumn[] { colNumeroVenta, colCliente, colFecha, colTotal, colEstado });
            dgvUltimasVentas.Dock = DockStyle.Fill;
            dgvUltimasVentas.Location = new Point(0, 35);
            dgvUltimasVentas.Name = "dgvUltimasVentas";
            dgvUltimasVentas.ReadOnly = true;
            dgvUltimasVentas.RowHeadersVisible = false;
            dgvUltimasVentas.ScrollBars = ScrollBars.Vertical;
            dgvUltimasVentas.Size = new Size(594, 232);
            dgvUltimasVentas.TabIndex = 1;
            dgvUltimasVentas.CellContentClick += dataGridView1_CellContentClick;
            // 
            // colNumeroVenta
            // 
            colNumeroVenta.HeaderText = "N.º venta";
            colNumeroVenta.Name = "colNumeroVenta";
            colNumeroVenta.ReadOnly = true;
            // 
            // colCliente
            // 
            colCliente.HeaderText = "Cliente";
            colCliente.Name = "colCliente";
            colCliente.ReadOnly = true;
            // 
            // colFecha
            // 
            colFecha.HeaderText = "Fecha";
            colFecha.Name = "colFecha";
            colFecha.ReadOnly = true;
            // 
            // colTotal
            // 
            colTotal.HeaderText = "Total";
            colTotal.Name = "colTotal";
            colTotal.ReadOnly = true;
            // 
            // colEstado
            // 
            colEstado.HeaderText = "Estado";
            colEstado.Name = "colEstado";
            colEstado.ReadOnly = true;
            // 
            // label15
            // 
            label15.Dock = DockStyle.Top;
            label15.Location = new Point(0, 0);
            label15.Name = "label15";
            label15.Size = new Size(594, 35);
            label15.TabIndex = 0;
            label15.Text = " Ultimas ventas registradas";
            // 
            // pnlAlertas
            // 
            pnlAlertas.Controls.Add(label16);
            pnlAlertas.Controls.Add(flowLayoutPanel1);
            pnlAlertas.Controls.Add(flpAlertas);
            pnlAlertas.Dock = DockStyle.Fill;
            pnlAlertas.Location = new Point(603, 120);
            pnlAlertas.Name = "pnlAlertas";
            pnlAlertas.Size = new Size(194, 267);
            pnlAlertas.TabIndex = 5;
            // 
            // label16
            // 
            label16.ForeColor = SystemColors.ButtonHighlight;
            label16.Location = new Point(0, 44);
            label16.Name = "label16";
            label16.Size = new Size(197, 50);
            label16.TabIndex = 3;
            label16.Text = "label16";
            label16.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.AutoScroll = true;
            flowLayoutPanel1.Dock = DockStyle.Fill;
            flowLayoutPanel1.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanel1.Location = new Point(0, 267);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(194, 0);
            flowLayoutPanel1.TabIndex = 2;
            flowLayoutPanel1.WrapContents = false;
            // 
            // flpAlertas
            // 
            flpAlertas.Dock = DockStyle.Top;
            flpAlertas.Location = new Point(0, 0);
            flpAlertas.Name = "flpAlertas";
            flpAlertas.Size = new Size(194, 267);
            flpAlertas.TabIndex = 1;
            flpAlertas.Text = "Alertas";
            flpAlertas.Click += flpAlertas_Click;
            // 
            // frmPanelPrincipal
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(panel2);
            Controls.Add(pnlEncabezado);
            Name = "frmPanelPrincipal";
            Text = "frmPanelPrincipal";
            Load += frmPanelPrincipal_Load;
            pnlEncabezado.ResumeLayout(false);
            pnlEncabezado.PerformLayout();
            panel2.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            pnlEnviosPendientes.ResumeLayout(false);
            pnlEnviosPendientes.PerformLayout();
            pnlStockBajo.ResumeLayout(false);
            pnlStockBajo.PerformLayout();
            pnlPagosPendientes.ResumeLayout(false);
            pnlPagosPendientes.PerformLayout();
            pnlVentasDia.ResumeLayout(false);
            pnlVentasDia.PerformLayout();
            pnlUltimasVentas.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvUltimasVentas).EndInit();
            pnlAlertas.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Label label1;
        private Panel pnlEncabezado;
        private Label label2;
        private FontAwesome.Sharp.IconButton btnCierreCaja;
        private FontAwesome.Sharp.IconButton btnIngresoStock;
        private FontAwesome.Sharp.IconButton btnNuevaVenta;
        private Panel panel2;
        private TableLayoutPanel tableLayoutPanel1;
        private Panel pnlEnviosPendientes;
        private Label label14;
        private Label label13;
        private Label label12;
        private Panel pnlStockBajo;
        private Label label11;
        private Label label10;
        private Label label9;
        private Panel pnlPagosPendientes;
        private Label label8;
        private Label label7;
        private Label label6;
        private Panel pnlVentasDia;
        private Label label5;
        private Label label4;
        private Label label3;
        private Panel pnlUltimasVentas;
        private Label label15;
        private Panel pnlAlertas;
        private Label flpAlertas;
        private DataGridView dgvUltimasVentas;
        private DataGridViewTextBoxColumn colNumeroVenta;
        private DataGridViewTextBoxColumn colCliente;
        private DataGridViewTextBoxColumn colFecha;
        private DataGridViewTextBoxColumn colTotal;
        private DataGridViewTextBoxColumn colEstado;
        private FlowLayoutPanel flowLayoutPanel1;
        private Label label16;
    }
}