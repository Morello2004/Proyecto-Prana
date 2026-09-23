namespace Prana.Presentacion
{
    partial class frmEntregas
    {
        private System.ComponentModel.IContainer components=null;
        protected override void Dispose(bool disposing){if(disposing)components?.Dispose();base.Dispose(disposing);}
        private void InitializeComponent()
        {
            pnlTitulo=new Panel();lblTitulo=new Label();pnlFiltros=new Panel();txtBuscar=new TextBox();btnBuscar=new Button();chkPendientes=new CheckBox();
            contenido=new TableLayoutPanel();pnlPendientes=new Panel();lblPendientes=new Label();dgvPendientes=new DataGridView();
            pnlPreparar=new Panel();lblSeleccion=new Label();lblCantidad=new Label();nudCantidad=new NumericUpDown();lblDireccion=new Label();txtDireccion=new TextBox();
            lblHorario=new Label();txtHorario=new TextBox();btnPreparar=new Button();lblAyuda=new Label();pnlEntregas=new Panel();lblEntregas=new Label();dgvEntregas=new DataGridView();btnConfirmar=new Button();
            SuspendLayout();
            pnlTitulo.Dock=DockStyle.Top;pnlTitulo.Height=76;pnlTitulo.BackColor=Color.FromArgb(21,30,41);pnlTitulo.Controls.Add(lblTitulo);
            lblTitulo.Text="Entregas";lblTitulo.Font=new Font("Segoe UI",18F,FontStyle.Bold);lblTitulo.AutoSize=true;lblTitulo.Location=new Point(20,20);
            pnlFiltros.Dock=DockStyle.Top;pnlFiltros.Height=70;pnlFiltros.BackColor=Color.FromArgb(21,30,41);pnlFiltros.Controls.AddRange(new Control[]{txtBuscar,btnBuscar,chkPendientes});
            txtBuscar.Location=new Point(20,18);txtBuscar.Size=new Size(420,27);txtBuscar.Font=new Font("Segoe UI",10F);txtBuscar.MaxLength=200;txtBuscar.PlaceholderText="Buscar por venta, nombre completo o DNI";txtBuscar.TabIndex=0;
            btnBuscar.Location=new Point(455,15);btnBuscar.Size=new Size(135,34);btnBuscar.Text="Buscar / Actualizar";btnBuscar.BackColor=Color.DodgerBlue;btnBuscar.ForeColor=Color.White;btnBuscar.FlatStyle=FlatStyle.Flat;btnBuscar.FlatAppearance.BorderSize=0;btnBuscar.TabIndex=1;
            chkPendientes.Text="Solo entregas sin confirmar";chkPendientes.Checked=true;chkPendientes.AutoSize=true;chkPendientes.Location=new Point(610,23);chkPendientes.TabIndex=2;
            contenido.Dock=DockStyle.Fill;contenido.Padding=new Padding(16);contenido.ColumnCount=1;contenido.RowCount=3;contenido.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,100));
            contenido.RowStyles.Add(new RowStyle(SizeType.Percent,50));contenido.RowStyles.Add(new RowStyle(SizeType.Absolute,160));contenido.RowStyles.Add(new RowStyle(SizeType.Percent,50));
            contenido.Controls.Add(pnlPendientes,0,0);contenido.Controls.Add(pnlPreparar,0,1);contenido.Controls.Add(pnlEntregas,0,2);
            pnlPendientes.Dock=DockStyle.Fill;pnlPendientes.Controls.Add(dgvPendientes);pnlPendientes.Controls.Add(lblPendientes);
            lblPendientes.Dock=DockStyle.Top;lblPendientes.Height=34;lblPendientes.Text="Productos por preparar";lblPendientes.Font=new Font("Segoe UI",12F,FontStyle.Bold);dgvPendientes.Dock=DockStyle.Fill;
            pnlPreparar.Dock=DockStyle.Fill;pnlPreparar.BackColor=Color.FromArgb(21,30,41);pnlPreparar.Controls.AddRange(new Control[]{lblSeleccion,lblCantidad,nudCantidad,lblDireccion,txtDireccion,lblHorario,txtHorario,btnPreparar,lblAyuda});
            lblSeleccion.Location=new Point(12,10);lblSeleccion.Size=new Size(850,24);lblSeleccion.AutoEllipsis=true;lblSeleccion.Text="Seleccione un producto";lblSeleccion.Font=new Font("Segoe UI",11F,FontStyle.Bold);
            lblCantidad.Text="Cantidad";lblCantidad.AutoSize=true;lblCantidad.Location=new Point(12,46);lblCantidad.ForeColor=Color.FromArgb(145,170,210);
            nudCantidad.Location=new Point(12,69);nudCantidad.Size=new Size(80,27);nudCantidad.Minimum=1;nudCantidad.Maximum=1000000;nudCantidad.Value=1;nudCantidad.TabIndex=0;
            lblDireccion.Text="Dirección / Retiro";lblDireccion.AutoSize=true;lblDireccion.Location=new Point(110,46);lblDireccion.ForeColor=Color.FromArgb(145,170,210);
            txtDireccion.Location=new Point(110,69);txtDireccion.Size=new Size(320,27);txtDireccion.MaxLength=200;txtDireccion.TabIndex=1;
            lblHorario.Text="Horario sugerido";lblHorario.AutoSize=true;lblHorario.Location=new Point(445,46);lblHorario.ForeColor=Color.FromArgb(145,170,210);
            txtHorario.Location=new Point(445,69);txtHorario.Size=new Size(145,27);txtHorario.MaxLength=50;txtHorario.TabIndex=2;
            btnPreparar.Text="Preparar entrega";btnPreparar.Location=new Point(610,63);btnPreparar.Size=new Size(170,35);btnPreparar.BackColor=Color.DodgerBlue;btnPreparar.ForeColor=Color.White;btnPreparar.FlatStyle=FlatStyle.Flat;btnPreparar.FlatAppearance.BorderSize=0;btnPreparar.Enabled=false;btnPreparar.TabIndex=3;
            lblAyuda.Text="Preparar reserva el stock. Confirmar registra que el cliente recibió los productos.";lblAyuda.Location=new Point(12,115);lblAyuda.Size=new Size(820,30);lblAyuda.ForeColor=Color.FromArgb(145,170,210);
            pnlEntregas.Dock=DockStyle.Fill;pnlEntregas.Controls.Add(dgvEntregas);pnlEntregas.Controls.Add(lblEntregas);pnlEntregas.Controls.Add(btnConfirmar);
            lblEntregas.Dock=DockStyle.Top;lblEntregas.Height=34;lblEntregas.Text="Entregas preparadas";lblEntregas.Font=new Font("Segoe UI",12F,FontStyle.Bold);dgvEntregas.Dock=DockStyle.Fill;
            btnConfirmar.Text="Confirmar entrega seleccionada";btnConfirmar.Dock=DockStyle.Bottom;btnConfirmar.Height=36;btnConfirmar.BackColor=Color.SteelBlue;btnConfirmar.ForeColor=Color.White;btnConfirmar.FlatStyle=FlatStyle.Flat;btnConfirmar.FlatAppearance.BorderSize=0;btnConfirmar.Enabled=false;
            AutoScaleDimensions=new SizeF(7F,15F);AutoScaleMode=AutoScaleMode.Font;ClientSize=new Size(1200,840);BackColor=Color.FromArgb(13,19,27);ForeColor=Color.White;AutoScroll=true;AutoScrollMinSize=new Size(900,780);
            Controls.Add(contenido);Controls.Add(pnlFiltros);Controls.Add(pnlTitulo);Name="frmEntregas";Text="Entregas";
            ResumeLayout(false);
        }
        private Panel pnlTitulo,pnlFiltros,pnlPendientes,pnlPreparar,pnlEntregas;
        private Label lblTitulo,lblPendientes,lblSeleccion,lblCantidad,lblDireccion,lblHorario,lblAyuda,lblEntregas;
        private TextBox txtBuscar,txtDireccion,txtHorario;
        private CheckBox chkPendientes;
        private Button btnBuscar,btnPreparar,btnConfirmar;
        private TableLayoutPanel contenido;
        private DataGridView dgvPendientes,dgvEntregas;
        private NumericUpDown nudCantidad;
    }
}
