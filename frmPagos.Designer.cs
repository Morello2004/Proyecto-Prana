namespace Prana.Presentacion
{
    partial class frmPagos
    {
        private System.ComponentModel.IContainer components=null;
        protected override void Dispose(bool disposing){if(disposing)components?.Dispose();base.Dispose(disposing);}
        private void InitializeComponent()
        {
            pnlTitulo=new Panel(); lblTitulo=new Label(); pnlFiltros=new Panel(); txtBuscar=new TextBox(); btnBuscar=new Button(); chkPendientes=new CheckBox();
            contenido=new TableLayoutPanel(); pnlVentas=new Panel(); lblVentas=new Label(); dgvVentas=new DataGridView();
            pnlCobro=new Panel(); lblSeleccion=new Label(); lblImporte=new Label(); nudImporte=new NumericUpDown(); lblMedio=new Label(); cboMedio=new ComboBox();
            lblCuotas=new Label(); cboCuotas=new ComboBox(); lblCobro=new Label(); btnRegistrar=new Button(); pnlPagos=new Panel(); lblPagos=new Label(); dgvPagos=new DataGridView();
            SuspendLayout();
            pnlTitulo.Dock=DockStyle.Top; pnlTitulo.Height=76; pnlTitulo.BackColor=Color.FromArgb(21,30,41); pnlTitulo.Controls.Add(lblTitulo);
            lblTitulo.Text="Pagos"; lblTitulo.Font=new Font("Segoe UI",18F,FontStyle.Bold); lblTitulo.AutoSize=true; lblTitulo.Location=new Point(20,20);
            pnlFiltros.Dock=DockStyle.Top; pnlFiltros.Height=70; pnlFiltros.BackColor=Color.FromArgb(21,30,41);
            pnlFiltros.Controls.AddRange(new Control[]{txtBuscar,btnBuscar,chkPendientes});
            txtBuscar.Location=new Point(20,18); txtBuscar.Size=new Size(420,27); txtBuscar.Font=new Font("Segoe UI",10F); txtBuscar.MaxLength=200;
            txtBuscar.PlaceholderText="Buscar por venta, nombre completo o DNI"; txtBuscar.TabIndex=0;
            btnBuscar.Location=new Point(455,15); btnBuscar.Size=new Size(135,34); btnBuscar.Text="Buscar / Actualizar"; btnBuscar.BackColor=Color.DodgerBlue;
            btnBuscar.ForeColor=Color.White; btnBuscar.FlatStyle=FlatStyle.Flat; btnBuscar.FlatAppearance.BorderSize=0; btnBuscar.TabIndex=1;
            chkPendientes.Text="Solo con saldo pendiente"; chkPendientes.Checked=true; chkPendientes.AutoSize=true; chkPendientes.Location=new Point(610,23); chkPendientes.TabIndex=2;
            contenido.Dock=DockStyle.Fill; contenido.Padding=new Padding(16); contenido.ColumnCount=1; contenido.RowCount=3;
            contenido.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,100)); contenido.RowStyles.Add(new RowStyle(SizeType.Percent,55));
            contenido.RowStyles.Add(new RowStyle(SizeType.Absolute,165)); contenido.RowStyles.Add(new RowStyle(SizeType.Percent,45));
            contenido.Controls.Add(pnlVentas,0,0); contenido.Controls.Add(pnlCobro,0,1); contenido.Controls.Add(pnlPagos,0,2);
            pnlVentas.Dock=DockStyle.Fill; pnlVentas.Controls.Add(dgvVentas); pnlVentas.Controls.Add(lblVentas);
            lblVentas.Dock=DockStyle.Top; lblVentas.Height=34; lblVentas.Text="Ventas"; lblVentas.Font=new Font("Segoe UI",12F,FontStyle.Bold); dgvVentas.Dock=DockStyle.Fill;
            pnlCobro.Dock=DockStyle.Fill; pnlCobro.BackColor=Color.FromArgb(21,30,41);
            pnlCobro.Controls.AddRange(new Control[]{lblSeleccion,lblImporte,nudImporte,lblMedio,cboMedio,lblCuotas,cboCuotas,lblCobro,btnRegistrar});
            lblSeleccion.Location=new Point(12,10); lblSeleccion.Size=new Size(840,24); lblSeleccion.Text="Seleccione una venta"; lblSeleccion.Font=new Font("Segoe UI",11F,FontStyle.Bold);
            lblImporte.Text="Importe a cancelar (sin recargo)"; lblImporte.AutoSize=true; lblImporte.Location=new Point(12,46); lblImporte.ForeColor=Color.FromArgb(145,170,210);
            nudImporte.Location=new Point(12,69); nudImporte.Size=new Size(215,27); nudImporte.DecimalPlaces=2; nudImporte.Maximum=99999999; nudImporte.ThousandsSeparator=true; nudImporte.TabIndex=0;
            lblMedio.Text="Medio de pago"; lblMedio.AutoSize=true; lblMedio.Location=new Point(245,46); lblMedio.ForeColor=Color.FromArgb(145,170,210);
            cboMedio.Location=new Point(245,69); cboMedio.Size=new Size(220,27); cboMedio.DropDownStyle=ComboBoxStyle.DropDownList; cboMedio.TabIndex=1;
            lblCuotas.Text="Cuotas"; lblCuotas.AutoSize=true; lblCuotas.Location=new Point(485,46); lblCuotas.ForeColor=Color.FromArgb(145,170,210);
            cboCuotas.Location=new Point(485,69); cboCuotas.Size=new Size(90,27); cboCuotas.DropDownStyle=ComboBoxStyle.DropDownList; cboCuotas.Items.AddRange(new object[]{1,3,6,9,12}); cboCuotas.TabIndex=2;
            btnRegistrar.Text="Registrar pago"; btnRegistrar.Location=new Point(600,63); btnRegistrar.Size=new Size(170,35); btnRegistrar.BackColor=Color.DodgerBlue;
            btnRegistrar.ForeColor=Color.White; btnRegistrar.FlatStyle=FlatStyle.Flat; btnRegistrar.FlatAppearance.BorderSize=0; btnRegistrar.Enabled=false; btnRegistrar.TabIndex=3;
            lblCobro.Location=new Point(12,115); lblCobro.Size=new Size(800,30); lblCobro.ForeColor=Color.FromArgb(255,195,95);
            pnlPagos.Dock=DockStyle.Fill; pnlPagos.Controls.Add(dgvPagos); pnlPagos.Controls.Add(lblPagos);
            lblPagos.Dock=DockStyle.Top; lblPagos.Height=34; lblPagos.Text="Pagos registrados"; lblPagos.Font=new Font("Segoe UI",12F,FontStyle.Bold); dgvPagos.Dock=DockStyle.Fill;
            AutoScaleDimensions=new SizeF(7F,15F); AutoScaleMode=AutoScaleMode.Font; ClientSize=new Size(1200,820);
            BackColor=Color.FromArgb(13,19,27); ForeColor=Color.White; AutoScroll=true; AutoScrollMinSize=new Size(880,740);
            Controls.Add(contenido); Controls.Add(pnlFiltros); Controls.Add(pnlTitulo); Name="frmPagos"; Text="Pagos";
            ResumeLayout(false);
        }
        private Panel pnlTitulo,pnlFiltros,pnlVentas,pnlCobro,pnlPagos;
        private Label lblTitulo,lblVentas,lblSeleccion,lblImporte,lblMedio,lblCuotas,lblCobro,lblPagos;
        private TextBox txtBuscar;
        private CheckBox chkPendientes;
        private Button btnBuscar,btnRegistrar;
        private TableLayoutPanel contenido;
        private DataGridView dgvVentas,dgvPagos;
        private NumericUpDown nudImporte;
        private ComboBox cboMedio,cboCuotas;
    }
}
