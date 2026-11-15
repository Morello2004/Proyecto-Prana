namespace Prana.Presentacion;
// parte visual de frmcaja crea controles y define su distribucion, colores y eventos; visual studio puede regenerar este archivo al editar el disenador
partial class frmCaja
{
    private System.ComponentModel.IContainer? components = null;
    // libera los recursos del formulario; disposing indica si tambien deben liberarse los componentes administrados
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            components?.Dispose();
        }

        base.Dispose(disposing);
    }

    // construye la pantalla: instancia controles, asigna propiedades, agrega cada control a su contenedor y conecta los eventos no registra operaciones comerciales
    private void InitializeComponent()
    {
        DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
        titulo = new Label();
        lblFecha = new Label();
        fecha = new DateTimePicker();
        actualizar = new Button();
        resumenMes = new Button();
        estado = new Label();
        lblRegistro = new Label();
        lbltipo = new Label();
        tipo = new ComboBox();
        lblmotivo = new Label();
        motivo = new TextBox();
        lblmedio = new Label();
        medio = new ComboBox();
        lblmoneda = new Label();
        moneda = new ComboBox();
        lblimporte = new Label();
        importe = new NumericUpDown();
        lblcuenta = new Label();
        cuenta = new TextBox();
        registrar = new Button();
        lblMovimientos = new Label();
        movimientos = new DataGridView();
        anular = new Button();
        lblCierre = new Label();
        lblinicio = new Label();
        inicio = new DateTimePicker();
        lblsaldo = new Label();
        saldo = new NumericUpDown();
        lblsaldoUsd = new Label();
        saldoUsd = new NumericUpDown();
        resumen = new Label();
        lblObservaciones = new Label();
        observaciones = new TextBox();
        cerrar = new Button();
        imprimir = new Button();
        ((System.ComponentModel.ISupportInitialize)importe).BeginInit();
        ((System.ComponentModel.ISupportInitialize)movimientos).BeginInit();
        ((System.ComponentModel.ISupportInitialize)saldo).BeginInit();
        ((System.ComponentModel.ISupportInitialize)saldoUsd).BeginInit();
        SuspendLayout();
        // 
        // titulo
        // 
        titulo.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
        titulo.ForeColor = Color.White;
        titulo.Location = new Point(16, 12);
        titulo.Name = "titulo";
        titulo.Size = new Size(700, 42);
        titulo.TabIndex = 0;
        titulo.Text = "Caja y gastos";
        // 
        // lblfecha
        // 
        lblFecha.ForeColor = Color.White;
        lblFecha.Location = new Point(16, 65);
        lblFecha.Name = "lblFecha";
        lblFecha.Size = new Size(200, 23);
        lblFecha.TabIndex = 1;
        lblFecha.Text = "Fecha";
        // 
        // fecha
        // 
        fecha.Format = DateTimePickerFormat.Short;
        fecha.Location = new Point(16, 92);
        fecha.Name = "fecha";
        fecha.Size = new Size(150, 23);
        fecha.TabIndex = 2;
        fecha.ValueChanged += fecha_ValueChanged;
        // 
        // actualizar
        // 
        actualizar.BackColor = Color.SteelBlue;
        actualizar.FlatStyle = FlatStyle.Flat;
        actualizar.ForeColor = Color.White;
        actualizar.Location = new Point(190, 86);
        actualizar.Name = "actualizar";
        actualizar.Size = new Size(210, 36);
        actualizar.TabIndex = 3;
        actualizar.Text = "Consultar / actualizar";
        actualizar.UseVisualStyleBackColor = false;
        actualizar.Click += actualizar_Click;
        // 
        // resumenmes
        // 
        resumenMes.BackColor = Color.SteelBlue;
        resumenMes.FlatStyle = FlatStyle.Flat;
        resumenMes.ForeColor = Color.White;
        resumenMes.Location = new Point(420, 86);
        resumenMes.Name = "resumenMes";
        resumenMes.Size = new Size(210, 36);
        resumenMes.TabIndex = 4;
        resumenMes.Text = "Resumen del mes";
        resumenMes.UseVisualStyleBackColor = false;
        resumenMes.Click += resumenMes_Click;
        // 
        // estado
        // 
        estado.ForeColor = Color.White;
        estado.Location = new Point(16, 135);
        estado.Name = "estado";
        estado.Size = new Size(940, 40);
        estado.TabIndex = 5;
        estado.Text = "Caja sin cerrar";
        // 
        // lblregistro
        // 
        lblRegistro.ForeColor = Color.White;
        lblRegistro.Location = new Point(16, 183);
        lblRegistro.Name = "lblRegistro";
        lblRegistro.Size = new Size(940, 28);
        lblRegistro.TabIndex = 6;
        lblRegistro.Text = "Registrar gasto u otro ingreso (hoy)";
        // 
        // lbltipo
        // 
        lbltipo.ForeColor = Color.White;
        lbltipo.Location = new Point(16, 220);
        lbltipo.Name = "lbltipo";
        lbltipo.Size = new Size(170, 23);
        lbltipo.TabIndex = 7;
        lbltipo.Text = "Tipo";
        // 
        // tipo
        // 
        tipo.DropDownStyle = ComboBoxStyle.DropDownList;
        tipo.Items.AddRange(new object[] { "Gasto", "Ingreso extra", "Café" });
        tipo.Location = new Point(16, 245);
        tipo.Name = "tipo";
        tipo.Size = new Size(170, 23);
        tipo.TabIndex = 8;
        // 
        // lblmotivo
        // 
        lblmotivo.ForeColor = Color.White;
        lblmotivo.Location = new Point(210, 220);
        lblmotivo.Name = "lblmotivo";
        lblmotivo.Size = new Size(310, 23);
        lblmotivo.TabIndex = 9;
        lblmotivo.Text = "Motivo";
        // 
        // motivo
        // 
        motivo.Location = new Point(210, 245);
        motivo.MaxLength = 200;
        motivo.Name = "motivo";
        motivo.Size = new Size(310, 23);
        motivo.TabIndex = 10;
        // 
        // lblmedio
        // 
        lblmedio.ForeColor = Color.White;
        lblmedio.Location = new Point(545, 220);
        lblmedio.Name = "lblmedio";
        lblmedio.Size = new Size(170, 23);
        lblmedio.TabIndex = 11;
        lblmedio.Text = "Medio";
        // 
        // medio
        // 
        medio.DropDownStyle = ComboBoxStyle.DropDownList;
        medio.Items.AddRange(new object[] { "Efectivo", "Transferencia", "Débito", "Crédito", "Otros" });
        medio.Location = new Point(545, 245);
        medio.Name = "medio";
        medio.Size = new Size(170, 23);
        medio.TabIndex = 12;
        // 
        // lblmoneda
        // 
        lblmoneda.ForeColor = Color.White;
        lblmoneda.Location = new Point(740, 220);
        lblmoneda.Name = "lblmoneda";
        lblmoneda.Size = new Size(90, 23);
        lblmoneda.TabIndex = 13;
        lblmoneda.Text = "Moneda";
        // 
        // moneda
        // 
        moneda.DropDownStyle = ComboBoxStyle.DropDownList;
        moneda.Items.AddRange(new object[] { "ARS", "USD" });
        moneda.Location = new Point(740, 245);
        moneda.Name = "moneda";
        moneda.Size = new Size(90, 23);
        moneda.TabIndex = 14;
        // 
        // lblimporte
        // 
        lblimporte.ForeColor = Color.White;
        lblimporte.Location = new Point(16, 290);
        lblimporte.Name = "lblimporte";
        lblimporte.Size = new Size(160, 23);
        lblimporte.TabIndex = 15;
        lblimporte.Text = "Importe";
        // 
        // importe
        // 
        importe.DecimalPlaces = 2;
        importe.Location = new Point(16, 315);
        importe.Maximum = new decimal(new int[] { 90000000, 0, 0, 0 });
        importe.Name = "importe";
        importe.Size = new Size(160, 23);
        importe.TabIndex = 16;
        importe.ThousandsSeparator = true;
        // 
        // lblcuenta
        // 
        lblcuenta.ForeColor = Color.White;
        lblcuenta.Location = new Point(210, 290);
        lblcuenta.Name = "lblcuenta";
        lblcuenta.Size = new Size(300, 23);
        lblcuenta.TabIndex = 17;
        lblcuenta.Text = "Cuenta / referencia";
        // 
        // cuenta
        // 
        cuenta.Location = new Point(210, 315);
        cuenta.MaxLength = 30;
        cuenta.Name = "cuenta";
        cuenta.Size = new Size(300, 23);
        cuenta.TabIndex = 18;
        // 
        // registrar
        // 
        registrar.BackColor = Color.SteelBlue;
        registrar.FlatStyle = FlatStyle.Flat;
        registrar.ForeColor = Color.White;
        registrar.Location = new Point(545, 311);
        registrar.Name = "registrar";
        registrar.Size = new Size(230, 36);
        registrar.TabIndex = 19;
        registrar.Text = "Registrar movimiento";
        registrar.UseVisualStyleBackColor = false;
        registrar.Click += registrar_Click;
        // 
        // lblmovimientos
        // 
        lblMovimientos.ForeColor = Color.White;
        lblMovimientos.Location = new Point(16, 360);
        lblMovimientos.Name = "lblMovimientos";
        lblMovimientos.Size = new Size(940, 28);
        lblMovimientos.TabIndex = 20;
        lblMovimientos.Text = "Cobros y movimientos del día";
        // 
        // movimientos
        // 
        movimientos.AllowUserToAddRows = false;
        movimientos.AllowUserToDeleteRows = false;
        dataGridViewCellStyle1.BackColor = Color.FromArgb(236, 242, 249);
        dataGridViewCellStyle1.ForeColor = Color.Black;
        dataGridViewCellStyle1.SelectionBackColor = Color.FromArgb(190, 220, 250);
        dataGridViewCellStyle1.SelectionForeColor = Color.Black;
        movimientos.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
        movimientos.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        movimientos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        movimientos.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        movimientos.BackgroundColor = Color.FromArgb(13, 19, 27);
        movimientos.BorderStyle = BorderStyle.None;
        dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle2.BackColor = Color.FromArgb(37, 52, 70);
        dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
        dataGridViewCellStyle2.ForeColor = Color.White;
        dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(190, 220, 250);
        dataGridViewCellStyle2.SelectionForeColor = Color.Black;
        dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
        movimientos.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
        movimientos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle3.BackColor = Color.White;
        dataGridViewCellStyle3.Font = new Font("Segoe UI", 9F);
        dataGridViewCellStyle3.ForeColor = Color.White;
        dataGridViewCellStyle3.Padding = new Padding(4);
        dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(35, 100, 166);
        dataGridViewCellStyle3.SelectionForeColor = Color.White;
        dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
        movimientos.DefaultCellStyle = dataGridViewCellStyle3;
        movimientos.EnableHeadersVisualStyles = false;
        movimientos.Location = new Point(16, 397);
        movimientos.MultiSelect = false;
        movimientos.Name = "movimientos";
        movimientos.ReadOnly = true;
        movimientos.RowHeadersVisible = false;
        dataGridViewCellStyle4.ForeColor = Color.Black;
        movimientos.RowsDefaultCellStyle = dataGridViewCellStyle4;
        movimientos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        movimientos.Size = new Size(916, 240);
        movimientos.TabIndex = 21;
        movimientos.CellFormatting += movimientos_CellFormatting;
        // 
        // anular
        // 
        anular.BackColor = Color.SteelBlue;
        anular.FlatStyle = FlatStyle.Flat;
        anular.ForeColor = Color.White;
        anular.Location = new Point(16, 650);
        anular.Name = "anular";
        anular.Size = new Size(260, 36);
        anular.TabIndex = 22;
        anular.Text = "Anular movimiento seleccionado";
        anular.UseVisualStyleBackColor = false;
        anular.Click += anular_Click;
        // 
        // lblcierre
        // 
        lblCierre.ForeColor = Color.White;
        lblCierre.Location = new Point(16, 702);
        lblCierre.Name = "lblCierre";
        lblCierre.Size = new Size(940, 28);
        lblCierre.TabIndex = 23;
        lblCierre.Text = "Resumen y cierre";
        // 
        // lblinicio
        // 
        lblinicio.ForeColor = Color.White;
        lblinicio.Location = new Point(16, 740);
        lblinicio.Name = "lblinicio";
        lblinicio.Size = new Size(170, 23);
        lblinicio.TabIndex = 24;
        lblinicio.Text = "Hora de inicio";
        // 
        // inicio
        // 
        inicio.Format = DateTimePickerFormat.Time;
        inicio.Location = new Point(16, 765);
        inicio.Name = "inicio";
        inicio.ShowUpDown = true;
        inicio.Size = new Size(170, 23);
        inicio.TabIndex = 25;
        // 
        // lblsaldo
        // 
        lblsaldo.ForeColor = Color.White;
        lblsaldo.Location = new Point(210, 740);
        lblsaldo.Name = "lblsaldo";
        lblsaldo.Size = new Size(170, 23);
        lblsaldo.TabIndex = 26;
        lblsaldo.Text = "Saldo inicial ARS";
        // 
        // saldo
        // 
        saldo.DecimalPlaces = 2;
        saldo.Location = new Point(210, 765);
        saldo.Maximum = new decimal(new int[] { 999999999, 0, 0, 0 });
        saldo.Name = "saldo";
        saldo.Size = new Size(170, 23);
        saldo.TabIndex = 27;
        saldo.ThousandsSeparator = true;
        saldo.ValueChanged += saldo_ValueChanged;
        // 
        // lblsaldousd
        // 
        lblsaldoUsd.ForeColor = Color.White;
        lblsaldoUsd.Location = new Point(405, 740);
        lblsaldoUsd.Name = "lblsaldoUsd";
        lblsaldoUsd.Size = new Size(170, 23);
        lblsaldoUsd.TabIndex = 28;
        lblsaldoUsd.Text = "Saldo inicial USD";
        // 
        // saldousd
        // 
        saldoUsd.DecimalPlaces = 2;
        saldoUsd.Location = new Point(405, 765);
        saldoUsd.Maximum = new decimal(new int[] { 999999999, 0, 0, 0 });
        saldoUsd.Name = "saldoUsd";
        saldoUsd.Size = new Size(170, 23);
        saldoUsd.TabIndex = 29;
        saldoUsd.ThousandsSeparator = true;
        saldoUsd.ValueChanged += saldoUsd_ValueChanged;
        // 
        // resumen
        // 
        resumen.ForeColor = Color.White;
        resumen.Location = new Point(16, 808);
        resumen.Name = "resumen";
        resumen.Size = new Size(950, 110);
        resumen.TabIndex = 30;
        resumen.Text = "Resumen de caja";
        // 
        // lblobservaciones
        // 
        lblObservaciones.ForeColor = Color.White;
        lblObservaciones.Location = new Point(16, 930);
        lblObservaciones.Name = "lblObservaciones";
        lblObservaciones.Size = new Size(940, 23);
        lblObservaciones.TabIndex = 31;
        lblObservaciones.Text = "Observaciones del cierre";
        // 
        // observaciones
        // 
        observaciones.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        observaciones.Location = new Point(16, 960);
        observaciones.MaxLength = 1000;
        observaciones.Multiline = true;
        observaciones.Name = "observaciones";
        observaciones.Size = new Size(916, 60);
        observaciones.TabIndex = 32;
        // 
        // cerrar
        // 
        cerrar.BackColor = Color.SteelBlue;
        cerrar.FlatStyle = FlatStyle.Flat;
        cerrar.ForeColor = Color.White;
        cerrar.Location = new Point(16, 1038);
        cerrar.Name = "cerrar";
        cerrar.Size = new Size(200, 36);
        cerrar.TabIndex = 33;
        cerrar.Text = "Cerrar caja del día";
        cerrar.UseVisualStyleBackColor = false;
        cerrar.Click += cerrar_Click;
        // 
        // imprimir
        // 
        imprimir.BackColor = Color.SteelBlue;
        imprimir.FlatStyle = FlatStyle.Flat;
        imprimir.ForeColor = Color.White;
        imprimir.Location = new Point(240, 1038);
        imprimir.Name = "imprimir";
        imprimir.Size = new Size(210, 36);
        imprimir.TabIndex = 34;
        imprimir.Text = "Ver / imprimir cierre";
        imprimir.UseVisualStyleBackColor = false;
        imprimir.Click += imprimir_Click;
        // 
        // frmcaja
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        AutoScroll = true;
        BackColor = Color.FromArgb(13, 19, 27);
        ClientSize = new Size(1000, 700);
        Controls.Add(titulo);
        Controls.Add(lblFecha);
        Controls.Add(fecha);
        Controls.Add(actualizar);
        Controls.Add(resumenMes);
        Controls.Add(estado);
        Controls.Add(lblRegistro);
        Controls.Add(lbltipo);
        Controls.Add(tipo);
        Controls.Add(lblmotivo);
        Controls.Add(motivo);
        Controls.Add(lblmedio);
        Controls.Add(medio);
        Controls.Add(lblmoneda);
        Controls.Add(moneda);
        Controls.Add(lblimporte);
        Controls.Add(importe);
        Controls.Add(lblcuenta);
        Controls.Add(cuenta);
        Controls.Add(registrar);
        Controls.Add(lblMovimientos);
        Controls.Add(movimientos);
        Controls.Add(anular);
        Controls.Add(lblCierre);
        Controls.Add(lblinicio);
        Controls.Add(inicio);
        Controls.Add(lblsaldo);
        Controls.Add(saldo);
        Controls.Add(lblsaldoUsd);
        Controls.Add(saldoUsd);
        Controls.Add(resumen);
        Controls.Add(lblObservaciones);
        Controls.Add(observaciones);
        Controls.Add(cerrar);
        Controls.Add(imprimir);
        ForeColor = Color.White;
        Name = "frmCaja";
        Text = "Caja y gastos";
        Load += frmCaja_Load;
        ((System.ComponentModel.ISupportInitialize)importe).EndInit();
        ((System.ComponentModel.ISupportInitialize)movimientos).EndInit();
        ((System.ComponentModel.ISupportInitialize)saldo).EndInit();
        ((System.ComponentModel.ISupportInitialize)saldoUsd).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    private Label titulo;
    private Label lblFecha;
    private DateTimePicker fecha;
    private Button actualizar;
    private Button resumenMes;
    private Label estado;
    private Label lblRegistro;
    private Label lbltipo;
    private ComboBox tipo;
    private Label lblmotivo;
    private TextBox motivo;
    private Label lblmedio;
    private ComboBox medio;
    private Label lblmoneda;
    private ComboBox moneda;
    private Label lblimporte;
    private NumericUpDown importe;
    private Label lblcuenta;
    private TextBox cuenta;
    private Button registrar;
    private Label lblMovimientos;
    private DataGridView movimientos;
    private Button anular;
    private Label lblCierre;
    private Label lblinicio;
    private DateTimePicker inicio;
    private Label lblsaldo;
    private NumericUpDown saldo;
    private Label lblsaldoUsd;
    private NumericUpDown saldoUsd;
    private Label resumen;
    private Label lblObservaciones;
    private TextBox observaciones;
    private Button cerrar;
    private Button imprimir;
    private DataGridViewTextBoxColumn colDiseno57;
    private DataGridViewTextBoxColumn colDiseno58;
    private DataGridViewTextBoxColumn colDiseno59;
    private DataGridViewTextBoxColumn colDiseno60;
    private DataGridViewTextBoxColumn colDiseno61;
    private DataGridViewTextBoxColumn colDiseno62;
    private DataGridViewTextBoxColumn colDiseno63;
    private DataGridViewTextBoxColumn colDiseno64;
}
