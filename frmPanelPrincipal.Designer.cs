namespace Prana.Presentacion
{
    // parte visual de frmpanelprincipal crea controles y define su distribucion, colores y eventos; visual studio puede regenerar este archivo al editar el disenador
    partial class frmPanelPrincipal
    {
        private System.ComponentModel.IContainer components = null;
        // libera los recursos del formulario; disposing indica si tambien deben liberarse los componentes administrados
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                components?.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        // construye la pantalla: instancia controles, asigna propiedades, agrega cada control a su contenedor y conecta los eventos no registra operaciones comerciales
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPanelPrincipal));
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            ayuda = new Label();
            documentos = new Button();
            caja = new Button();
            pnlEncabezado = new Panel();
            lblTitulo = new Label();
            flpAccesos = new FlowLayoutPanel();
            btnNuevaVenta = new Button();
            btnStock = new Button();
            tlpContenido = new TableLayoutPanel();
            tlpResumen = new TableLayoutPanel();
            pnlHoy = new Panel();
            lblHoy = new Label();
            lblTotalHoy = new Label();
            lblCantidadHoy = new Label();
            pnlSaldo = new Panel();
            lblSaldo = new Label();
            lblTotalPendiente = new Label();
            lblSaldoAyuda = new Label();
            pnlPendientes = new Panel();
            lblPendientes = new Label();
            lblCantidadPendientes = new Label();
            lblPendientesAyuda = new Label();
            pnlBusqueda = new Panel();
            lblBuscar = new Label();
            txtBuscarVenta = new TextBox();
            btnBuscar = new Button();
            btnUltimas = new Button();
            pnlVentas = new Panel();
            dgvUltimasVentas = new DataGridView();
            colNumeroVenta = new DataGridViewTextBoxColumn();
            colFecha = new DataGridViewTextBoxColumn();
            colCliente = new DataGridViewTextBoxColumn();
            colTotal = new DataGridViewTextBoxColumn();
            lblDetalle = new Label();
            lblResultados = new Label();
            pnlEncabezado.SuspendLayout();
            flpAccesos.SuspendLayout();
            tlpContenido.SuspendLayout();
            tlpResumen.SuspendLayout();
            pnlHoy.SuspendLayout();
            pnlSaldo.SuspendLayout();
            pnlPendientes.SuspendLayout();
            pnlBusqueda.SuspendLayout();
            pnlVentas.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvUltimasVentas).BeginInit();
            SuspendLayout();
            // 
            // ayuda
            // 
            ayuda.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            ayuda.Font = new Font("Segoe UI", 9F);
            ayuda.ForeColor = Color.FromArgb(145, 170, 210);
            ayuda.Location = new Point(16, 78);
            ayuda.Name = "ayuda";
            ayuda.Size = new Size(1810, 80);
            ayuda.TabIndex = 3;
            ayuda.Text = resources.GetString("ayuda.Text");
            // 
            // documentos
            // 
            documentos.BackColor = Color.SteelBlue;
            documentos.FlatStyle = FlatStyle.Flat;
            documentos.Location = new Point(465, 12);
            documentos.Margin = new Padding(4, 4, 12, 4);
            documentos.Name = "documentos";
            documentos.Size = new Size(160, 34);
            documentos.TabIndex = 4;
            documentos.Text = "Garantía / remito";
            documentos.UseVisualStyleBackColor = false;
            documentos.Click += documentos_Click;
            // 
            // caja
            // 
            caja.BackColor = Color.SteelBlue;
            caja.FlatStyle = FlatStyle.Flat;
            caja.Location = new Point(304, 12);
            caja.Margin = new Padding(4, 4, 12, 4);
            caja.Name = "caja";
            caja.Size = new Size(145, 34);
            caja.TabIndex = 3;
            caja.Text = "Caja y gastos";
            caja.UseVisualStyleBackColor = false;
            caja.Click += caja_Click;
            // 
            // pnlencabezado
            // 
            pnlEncabezado.BackColor = Color.FromArgb(21, 30, 41);
            pnlEncabezado.Controls.Add(lblTitulo);
            pnlEncabezado.Controls.Add(flpAccesos);
            pnlEncabezado.Dock = DockStyle.Top;
            pnlEncabezado.Location = new Point(0, 0);
            pnlEncabezado.Name = "pnlEncabezado";
            pnlEncabezado.Size = new Size(1190, 120);
            pnlEncabezado.TabIndex = 1;
            // 
            // lbltitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTitulo.Location = new Point(20, 8);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(184, 32);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Panel principal";
            // 
            // flpaccesos
            // 
            flpAccesos.Controls.Add(btnNuevaVenta);
            flpAccesos.Controls.Add(btnStock);
            flpAccesos.Controls.Add(caja);
            flpAccesos.Controls.Add(documentos);
            flpAccesos.Dock = DockStyle.Bottom;
            flpAccesos.Location = new Point(0, 60);
            flpAccesos.Name = "flpAccesos";
            flpAccesos.Padding = new Padding(8);
            flpAccesos.Size = new Size(1190, 60);
            flpAccesos.TabIndex = 1;
            // 
            // btnnuevaventa
            // 
            btnNuevaVenta.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnNuevaVenta.BackColor = Color.DodgerBlue;
            btnNuevaVenta.FlatAppearance.BorderSize = 0;
            btnNuevaVenta.FlatStyle = FlatStyle.Flat;
            btnNuevaVenta.Location = new Point(12, 12);
            btnNuevaVenta.Margin = new Padding(4, 4, 12, 4);
            btnNuevaVenta.Name = "btnNuevaVenta";
            btnNuevaVenta.Size = new Size(130, 34);
            btnNuevaVenta.TabIndex = 1;
            btnNuevaVenta.Text = "Nueva venta";
            btnNuevaVenta.UseVisualStyleBackColor = false;
            btnNuevaVenta.Click += btnNuevaVenta_Click;
            // 
            // btnstock
            // 
            btnStock.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnStock.BackColor = Color.SteelBlue;
            btnStock.FlatAppearance.BorderSize = 0;
            btnStock.FlatStyle = FlatStyle.Flat;
            btnStock.Location = new Point(158, 12);
            btnStock.Margin = new Padding(4, 4, 12, 4);
            btnStock.Name = "btnStock";
            btnStock.Size = new Size(130, 34);
            btnStock.TabIndex = 2;
            btnStock.Text = "Ver stock";
            btnStock.UseVisualStyleBackColor = false;
            btnStock.Click += btnStock_Click;
            // 
            // tlpcontenido
            // 
            tlpContenido.ColumnCount = 1;
            tlpContenido.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpContenido.Controls.Add(tlpResumen, 0, 0);
            tlpContenido.Controls.Add(pnlBusqueda, 0, 1);
            tlpContenido.Controls.Add(pnlVentas, 0, 2);
            tlpContenido.Dock = DockStyle.Fill;
            tlpContenido.Location = new Point(0, 120);
            tlpContenido.Name = "tlpContenido";
            tlpContenido.Padding = new Padding(12);
            tlpContenido.RowCount = 3;
            tlpContenido.RowStyles.Add(new RowStyle(SizeType.Absolute, 128F));
            tlpContenido.RowStyles.Add(new RowStyle(SizeType.Absolute, 96F));
            tlpContenido.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpContenido.Size = new Size(1190, 620);
            tlpContenido.TabIndex = 0;
            // 
            // tlpresumen
            // 
            tlpResumen.ColumnCount = 3;
            tlpResumen.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            tlpResumen.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            tlpResumen.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.34F));
            tlpResumen.Controls.Add(pnlHoy, 0, 0);
            tlpResumen.Controls.Add(pnlSaldo, 1, 0);
            tlpResumen.Controls.Add(pnlPendientes, 2, 0);
            tlpResumen.Dock = DockStyle.Fill;
            tlpResumen.Location = new Point(15, 15);
            tlpResumen.Name = "tlpResumen";
            tlpResumen.RowCount = 1;
            tlpResumen.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpResumen.Size = new Size(1160, 122);
            tlpResumen.TabIndex = 0;
            // 
            // pnlhoy
            // 
            pnlHoy.BackColor = Color.FromArgb(21, 30, 41);
            pnlHoy.Controls.Add(lblHoy);
            pnlHoy.Controls.Add(lblTotalHoy);
            pnlHoy.Controls.Add(lblCantidadHoy);
            pnlHoy.Dock = DockStyle.Fill;
            pnlHoy.Location = new Point(3, 3);
            pnlHoy.Name = "pnlHoy";
            pnlHoy.Size = new Size(380, 116);
            pnlHoy.TabIndex = 0;
            // 
            // lblhoy
            // 
            lblHoy.AutoSize = true;
            lblHoy.Location = new Point(16, 12);
            lblHoy.Name = "lblHoy";
            lblHoy.Size = new Size(80, 15);
            lblHoy.TabIndex = 0;
            lblHoy.Text = "Ventas de hoy";
            // 
            // lbltotalhoy
            // 
            lblTotalHoy.AutoSize = true;
            lblTotalHoy.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
            lblTotalHoy.Location = new Point(12, 34);
            lblTotalHoy.Name = "lblTotalHoy";
            lblTotalHoy.Size = new Size(48, 41);
            lblTotalHoy.TabIndex = 1;
            lblTotalHoy.Text = "—";
            lblTotalHoy.Click += lblTotalHoy_Click;
            // 
            // lblcantidadhoy
            // 
            lblCantidadHoy.AutoSize = true;
            lblCantidadHoy.ForeColor = Color.FromArgb(145, 170, 210);
            lblCantidadHoy.Location = new Point(16, 85);
            lblCantidadHoy.Name = "lblCantidadHoy";
            lblCantidadHoy.Size = new Size(135, 15);
            lblCantidadHoy.TabIndex = 2;
            lblCantidadHoy.Text = "Cargando operaciones…";
            // 
            // pnlsaldo
            // 
            pnlSaldo.BackColor = Color.FromArgb(21, 30, 41);
            pnlSaldo.Controls.Add(lblSaldo);
            pnlSaldo.Controls.Add(lblTotalPendiente);
            pnlSaldo.Controls.Add(lblSaldoAyuda);
            pnlSaldo.Dock = DockStyle.Fill;
            pnlSaldo.Location = new Point(389, 3);
            pnlSaldo.Name = "pnlSaldo";
            pnlSaldo.Size = new Size(380, 116);
            pnlSaldo.TabIndex = 1;
            // 
            // lblsaldo
            // 
            lblSaldo.AutoSize = true;
            lblSaldo.Location = new Point(16, 12);
            lblSaldo.Name = "lblSaldo";
            lblSaldo.Size = new Size(142, 15);
            lblSaldo.TabIndex = 0;
            lblSaldo.Text = "Saldo pendiente de cobro";
            // 
            // lbltotalpendiente
            // 
            lblTotalPendiente.AutoSize = true;
            lblTotalPendiente.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
            lblTotalPendiente.ForeColor = Color.FromArgb(255, 195, 95);
            lblTotalPendiente.Location = new Point(12, 34);
            lblTotalPendiente.Name = "lblTotalPendiente";
            lblTotalPendiente.Size = new Size(48, 41);
            lblTotalPendiente.TabIndex = 1;
            lblTotalPendiente.Text = "—";
            // 
            // lblsaldoayuda
            // 
            lblSaldoAyuda.AutoSize = true;
            lblSaldoAyuda.ForeColor = Color.FromArgb(145, 170, 210);
            lblSaldoAyuda.Location = new Point(16, 85);
            lblSaldoAyuda.Name = "lblSaldoAyuda";
            lblSaldoAyuda.Size = new Size(139, 15);
            lblSaldoAyuda.TabIndex = 2;
            lblSaldoAyuda.Text = "Total de ventas con saldo";
            // 
            // pnlpendientes
            // 
            pnlPendientes.BackColor = Color.FromArgb(21, 30, 41);
            pnlPendientes.Controls.Add(lblPendientes);
            pnlPendientes.Controls.Add(lblCantidadPendientes);
            pnlPendientes.Controls.Add(lblPendientesAyuda);
            pnlPendientes.Dock = DockStyle.Fill;
            pnlPendientes.Location = new Point(775, 3);
            pnlPendientes.Name = "pnlPendientes";
            pnlPendientes.Size = new Size(382, 116);
            pnlPendientes.TabIndex = 2;
            // 
            // lblpendientes
            // 
            lblPendientes.AutoSize = true;
            lblPendientes.Location = new Point(16, 12);
            lblPendientes.Name = "lblPendientes";
            lblPendientes.Size = new Size(99, 15);
            lblPendientes.TabIndex = 0;
            lblPendientes.Text = "Ventas por cobrar";
            // 
            // lblcantidadpendientes
            // 
            lblCantidadPendientes.AutoSize = true;
            lblCantidadPendientes.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
            lblCantidadPendientes.Location = new Point(12, 34);
            lblCantidadPendientes.Name = "lblCantidadPendientes";
            lblCantidadPendientes.Size = new Size(48, 41);
            lblCantidadPendientes.TabIndex = 1;
            lblCantidadPendientes.Text = "—";
            // 
            // lblpendientesayuda
            // 
            lblPendientesAyuda.AutoSize = true;
            lblPendientesAyuda.ForeColor = Color.FromArgb(145, 170, 210);
            lblPendientesAyuda.Location = new Point(16, 85);
            lblPendientesAyuda.Name = "lblPendientesAyuda";
            lblPendientesAyuda.Size = new Size(176, 15);
            lblPendientesAyuda.TabIndex = 2;
            lblPendientesAyuda.Text = "Con pagos parciales o sin pagos";
            // 
            // pnlbusqueda
            // 
            pnlBusqueda.BackColor = Color.FromArgb(21, 30, 41);
            pnlBusqueda.Controls.Add(lblBuscar);
            pnlBusqueda.Controls.Add(txtBuscarVenta);
            pnlBusqueda.Controls.Add(btnBuscar);
            pnlBusqueda.Controls.Add(btnUltimas);
            pnlBusqueda.Controls.Add(ayuda);
            pnlBusqueda.Dock = DockStyle.Fill;
            pnlBusqueda.Location = new Point(15, 143);
            pnlBusqueda.Name = "pnlBusqueda";
            pnlBusqueda.Size = new Size(1160, 90);
            pnlBusqueda.TabIndex = 1;
            // 
            // lblbuscar
            // 
            lblBuscar.AutoSize = true;
            lblBuscar.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblBuscar.Location = new Point(14, 9);
            lblBuscar.Name = "lblBuscar";
            lblBuscar.Size = new Size(106, 20);
            lblBuscar.TabIndex = 0;
            lblBuscar.Text = "Buscar ventas";
            // 
            // txtbuscarventa
            // 
            txtBuscarVenta.Location = new Point(16, 41);
            txtBuscarVenta.MaxLength = 50;
            txtBuscarVenta.Name = "txtBuscarVenta";
            txtBuscarVenta.PlaceholderText = "Nº de venta, cliente, DNI o estado";
            txtBuscarVenta.Size = new Size(460, 23);
            txtBuscarVenta.TabIndex = 0;
           
            // 
            // btnbuscar
            // 
            btnBuscar.BackColor = Color.DodgerBlue;
            btnBuscar.FlatAppearance.BorderSize = 0;
            btnBuscar.FlatStyle = FlatStyle.Flat;
            btnBuscar.Location = new Point(488, 39);
            btnBuscar.Name = "btnBuscar";
            btnBuscar.Size = new Size(100, 31);
            btnBuscar.TabIndex = 1;
            btnBuscar.Text = "Buscar";
            btnBuscar.UseVisualStyleBackColor = false;
            btnBuscar.Click += btnBuscar_Click;
            // 
            // btnultimas
            // 
            btnUltimas.BackColor = Color.SteelBlue;
            btnUltimas.FlatAppearance.BorderSize = 0;
            btnUltimas.FlatStyle = FlatStyle.Flat;
            btnUltimas.Location = new Point(600, 39);
            btnUltimas.Name = "btnUltimas";
            btnUltimas.Size = new Size(195, 31);
            btnUltimas.TabIndex = 2;
            btnUltimas.Text = "Últimas 10 / Actualizar";
            btnUltimas.UseVisualStyleBackColor = false;
            btnUltimas.Click += btnUltimas_Click;
            // 
            // pnlventas
            // 
            pnlVentas.BackColor = Color.FromArgb(21, 30, 41);
            pnlVentas.Controls.Add(dgvUltimasVentas);
            pnlVentas.Controls.Add(lblDetalle);
            pnlVentas.Controls.Add(lblResultados);
            pnlVentas.Dock = DockStyle.Fill;
            pnlVentas.Location = new Point(15, 239);
            pnlVentas.Name = "pnlVentas";
            pnlVentas.Padding = new Padding(12);
            pnlVentas.Size = new Size(1160, 366);
            pnlVentas.TabIndex = 2;
            // 
            // dgvultimasventas
            // 
            dgvUltimasVentas.AllowUserToAddRows = false;
            dgvUltimasVentas.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(236, 242, 249);
            dataGridViewCellStyle1.ForeColor = Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = Color.FromArgb(190, 220, 250);
            dataGridViewCellStyle1.SelectionForeColor = Color.Black;
            dgvUltimasVentas.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dgvUltimasVentas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvUltimasVentas.BackgroundColor = Color.FromArgb(13, 19, 27);
            dgvUltimasVentas.BorderStyle = BorderStyle.None;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = SystemColors.Control;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(190, 220, 250);
            dataGridViewCellStyle2.SelectionForeColor = Color.Black;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dgvUltimasVentas.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dgvUltimasVentas.ColumnHeadersHeight = 38;
            dgvUltimasVentas.Columns.AddRange(new DataGridViewColumn[] { colNumeroVenta, colFecha, colCliente, colTotal });
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = Color.White;
            dataGridViewCellStyle5.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle5.ForeColor = Color.White;
            dataGridViewCellStyle5.SelectionBackColor = Color.FromArgb(190, 220, 250);
            dataGridViewCellStyle5.SelectionForeColor = Color.Black;
            dataGridViewCellStyle5.WrapMode = DataGridViewTriState.False;
            dgvUltimasVentas.DefaultCellStyle = dataGridViewCellStyle5;
            dgvUltimasVentas.Dock = DockStyle.Fill;
            dgvUltimasVentas.Location = new Point(12, 48);
            dgvUltimasVentas.MultiSelect = false;
            dgvUltimasVentas.Name = "dgvUltimasVentas";
            dgvUltimasVentas.ReadOnly = true;
            dgvUltimasVentas.RowHeadersVisible = false;
            dataGridViewCellStyle6.ForeColor = Color.Black;
            dgvUltimasVentas.RowsDefaultCellStyle = dataGridViewCellStyle6;
            dgvUltimasVentas.RowTemplate.Height = 32;
            dgvUltimasVentas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvUltimasVentas.Size = new Size(1136, 242);
            dgvUltimasVentas.TabIndex = 0;
            dgvUltimasVentas.CellFormatting += dgvUltimasVentas_CellFormatting;
            dgvUltimasVentas.SelectionChanged += dgvUltimasVentas_SelectionChanged;
            // 
            // colnumeroventa
            // 
            colNumeroVenta.DataPropertyName = "IdVenta";
            colNumeroVenta.FillWeight = 55F;
            colNumeroVenta.HeaderText = "N.º venta";
            colNumeroVenta.Name = "colNumeroVenta";
            colNumeroVenta.ReadOnly = true;
            // 
            // colfecha
            // 
            colFecha.DataPropertyName = "Fecha";
            dataGridViewCellStyle3.Format = "dd/MM/yyyy HH:mm";
            colFecha.DefaultCellStyle = dataGridViewCellStyle3;
            colFecha.FillWeight = 120F;
            colFecha.HeaderText = "Fecha";
            colFecha.Name = "colFecha";
            colFecha.ReadOnly = true;
            // 
            // colcliente
            // 
            colCliente.DataPropertyName = "NombreCompletoCliente";
            colCliente.FillWeight = 170F;
            colCliente.HeaderText = "Cliente";
            colCliente.Name = "colCliente";
            colCliente.ReadOnly = true;
            // 
            // coltotal
            // 
            colTotal.DataPropertyName = "Total";
            dataGridViewCellStyle4.Format = "C2";
            colTotal.DefaultCellStyle = dataGridViewCellStyle4;
            colTotal.HeaderText = "Total";
            colTotal.Name = "colTotal";
            colTotal.ReadOnly = true;
            // 
            // 
            // 
            // 
            // 
            // lbldetalle
            // 
            lblDetalle.AutoEllipsis = true;
            lblDetalle.Dock = DockStyle.Bottom;
            lblDetalle.ForeColor = Color.FromArgb(145, 170, 210);
            lblDetalle.Location = new Point(12, 290);
            lblDetalle.Name = "lblDetalle";
            lblDetalle.Padding = new Padding(0, 8, 0, 0);
            lblDetalle.Size = new Size(1136, 64);
            lblDetalle.TabIndex = 1;
            lblDetalle.Text = "Seleccione una venta para ver sus productos.";
            // 
            // lblresultados
            // 
            lblResultados.Dock = DockStyle.Top;
            lblResultados.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblResultados.Location = new Point(12, 12);
            lblResultados.Name = "lblResultados";
            lblResultados.Size = new Size(1136, 36);
            lblResultados.TabIndex = 2;
            lblResultados.Text = "Últimas 10 ventas";
            // 
            // frmpanelprincipal
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            AutoScrollMinSize = new Size(850, 650);
            BackColor = Color.FromArgb(13, 19, 27);
            ClientSize = new Size(1190, 740);
            Controls.Add(tlpContenido);
            Controls.Add(pnlEncabezado);
            Font = new Font("Segoe UI", 9F);
            ForeColor = Color.White;
            Name = "frmPanelPrincipal";
            Text = "Panel principal";
            Load += frmPanelPrincipal_Load;
            pnlEncabezado.ResumeLayout(false);
            pnlEncabezado.PerformLayout();
            flpAccesos.ResumeLayout(false);
            tlpContenido.ResumeLayout(false);
            tlpResumen.ResumeLayout(false);
            pnlHoy.ResumeLayout(false);
            pnlHoy.PerformLayout();
            pnlSaldo.ResumeLayout(false);
            pnlSaldo.PerformLayout();
            pnlPendientes.ResumeLayout(false);
            pnlPendientes.PerformLayout();
            pnlBusqueda.ResumeLayout(false);
            pnlBusqueda.PerformLayout();
            pnlVentas.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvUltimasVentas).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private Panel pnlEncabezado, pnlHoy, pnlSaldo, pnlPendientes, pnlBusqueda, pnlVentas;
        private TableLayoutPanel tlpContenido, tlpResumen;
        private Label lblTitulo, lblHoy, lblTotalHoy, lblCantidadHoy, lblSaldo, lblTotalPendiente, lblSaldoAyuda;
        private Label lblPendientes, lblCantidadPendientes, lblPendientesAyuda, lblBuscar, lblResultados, lblDetalle;
        private Button btnNuevaVenta, btnStock, btnBuscar, btnUltimas;
        private TextBox txtBuscarVenta;
        private DataGridView dgvUltimasVentas;
        private DataGridViewTextBoxColumn colNumeroVenta, colCliente, colFecha, colTotal;
        private Button caja;
        private Button documentos;
        private Label ayuda;
        private FlowLayoutPanel flpAccesos;
    }
}
