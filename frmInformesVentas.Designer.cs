namespace Prana.Presentacion;
// parte visual de frminformesventas crea controles y define su distribucion, colores y eventos; visual studio puede regenerar este archivo al editar el disenador
partial class frmInformesVentas
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
        titulo = new Label();
        lbldesde = new Label();
        desde = new DateTimePicker();
        lblhasta = new Label();
        hasta = new DateTimePicker();
        lblproducto = new Label();
        producto = new ComboBox();
        lblcategoria = new Label();
        categoria = new ComboBox();
        lblmarca = new Label();
        marca = new ComboBox();
        lblcliente = new Label();
        cliente = new ComboBox();
        lblenvio = new Label();
        envio = new ComboBox();
        lblubicacion = new Label();
        ubicacion = new ComboBox();
        lblmontoMinimo = new Label();
        lblmontoMaximo = new Label();
        buscar = new Button();
        grilla = new DataGridView();
        nudMinimo = new NumericUpDown();
        nudMaximo = new NumericUpDown();
        // llama a ((systemcomponentmodelisupportinitialize)grilla)begininit para realizar este paso del flujo
        ((System.ComponentModel.ISupportInitialize)grilla).BeginInit();
        // llama a ((systemcomponentmodelisupportinitialize)nudminimo)begininit para realizar este paso del flujo
        ((System.ComponentModel.ISupportInitialize)nudMinimo).BeginInit();
        // llama a ((systemcomponentmodelisupportinitialize)nudmaximo)begininit para realizar este paso del flujo
        ((System.ComponentModel.ISupportInitialize)nudMaximo).BeginInit();
        SuspendLayout();
        // titulo titulofont: tipografia, tamano y estilo con los que se muestra el contenido
        titulo.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
        titulo.ForeColor = Color.White;
        titulo.Location = new Point(16, 16);
        titulo.Name = "titulo";
        titulo.Size = new Size(700, 42);
        titulo.TabIndex = 0;
        titulo.Text = "Informe de ventas";
        // lbldesde lbldesdeforecolor: color del texto cuando la celda o el control no esta seleccionado
        lbldesde.ForeColor = Color.White;
        lbldesde.Location = new Point(16, 72);
        lbldesde.Name = "lbldesde";
        lbldesde.Size = new Size(180, 23);
        lbldesde.TabIndex = 1;
        lbldesde.Text = "Desde";
        // desde desdeformat: formato de presentacion, por ejemplo moneda o fecha; no modifica el valor original
        desde.Format = DateTimePickerFormat.Short;
        desde.Location = new Point(16, 96);
        desde.Name = "desde";
        desde.Size = new Size(178, 23);
        desde.TabIndex = 2;
        // lblhasta lblhastaforecolor: color del texto cuando la celda o el control no esta seleccionado
        lblhasta.ForeColor = Color.White;
        lblhasta.Location = new Point(206, 72);
        lblhasta.Name = "lblhasta";
        lblhasta.Size = new Size(180, 23);
        lblhasta.TabIndex = 3;
        lblhasta.Text = "Hasta";
        // hasta hastaformat: formato de presentacion, por ejemplo moneda o fecha; no modifica el valor original
        hasta.Format = DateTimePickerFormat.Short;
        hasta.Location = new Point(206, 96);
        hasta.Name = "hasta";
        hasta.Size = new Size(178, 23);
        hasta.TabIndex = 4;
        // lblproducto lblproductoforecolor: color del texto cuando la celda o el control no esta seleccionado
        lblproducto.ForeColor = Color.White;
        lblproducto.Location = new Point(396, 72);
        lblproducto.Name = "lblproducto";
        lblproducto.Size = new Size(180, 23);
        lblproducto.TabIndex = 5;
        lblproducto.Text = "Producto";
        // producto productolocation: posicion horizontal y vertical respecto del contenedor
        producto.Location = new Point(396, 96);
        producto.Name = "producto";
        producto.Size = new Size(178, 23);
        producto.TabIndex = 6;
        // lblcategoria lblcategoriaforecolor: color del texto cuando la celda o el control no esta seleccionado
        lblcategoria.ForeColor = Color.White;
        lblcategoria.Location = new Point(586, 72);
        lblcategoria.Name = "lblcategoria";
        lblcategoria.Size = new Size(180, 23);
        lblcategoria.TabIndex = 7;
        lblcategoria.Text = "Categoría";
        // categoria categorialocation: posicion horizontal y vertical respecto del contenedor
        categoria.Location = new Point(586, 96);
        categoria.Name = "categoria";
        categoria.Size = new Size(178, 23);
        categoria.TabIndex = 8;
        // lblmarca lblmarcaforecolor: color del texto cuando la celda o el control no esta seleccionado
        lblmarca.ForeColor = Color.White;
        lblmarca.Location = new Point(776, 72);
        lblmarca.Name = "lblmarca";
        lblmarca.Size = new Size(180, 23);
        lblmarca.TabIndex = 9;
        lblmarca.Text = "Marca";
        // marca marcalocation: posicion horizontal y vertical respecto del contenedor
        marca.Location = new Point(776, 96);
        marca.Name = "marca";
        marca.Size = new Size(178, 23);
        marca.TabIndex = 10;
        // lblcliente lblclienteforecolor: color del texto cuando la celda o el control no esta seleccionado
        lblcliente.ForeColor = Color.White;
        lblcliente.Location = new Point(16, 137);
        lblcliente.Name = "lblcliente";
        lblcliente.Size = new Size(180, 23);
        lblcliente.TabIndex = 11;
        lblcliente.Text = "Cliente";
        // cliente clientelocation: posicion horizontal y vertical respecto del contenedor
        cliente.Location = new Point(16, 161);
        cliente.Name = "cliente";
        cliente.Size = new Size(178, 23);
        cliente.TabIndex = 12;
        // lblenvio lblenvioforecolor: color del texto cuando la celda o el control no esta seleccionado
        lblenvio.ForeColor = Color.White;
        lblenvio.Location = new Point(206, 137);
        lblenvio.Name = "lblenvio";
        lblenvio.Size = new Size(180, 23);
        lblenvio.TabIndex = 13;
        lblenvio.Text = "Envío";
        // envio enviolocation: posicion horizontal y vertical respecto del contenedor
        envio.Location = new Point(206, 161);
        envio.Name = "envio";
        envio.Size = new Size(178, 23);
        envio.TabIndex = 14;
        envio.SelectedIndexChanged += envio_SelectedIndexChanged;
        // lblubicacion lblubicacionforecolor: color del texto cuando la celda o el control no esta seleccionado
        lblubicacion.ForeColor = Color.White;
        lblubicacion.Location = new Point(396, 137);
        lblubicacion.Name = "lblubicacion";
        lblubicacion.Size = new Size(180, 23);
        lblubicacion.TabIndex = 15;
        lblubicacion.Text = "Localidad";
        // ubicacion ubicacionlocation: posicion horizontal y vertical respecto del contenedor
        ubicacion.Location = new Point(396, 161);
        ubicacion.Name = "ubicacion";
        ubicacion.Size = new Size(178, 23);
        ubicacion.TabIndex = 16;
        // lblmontominimo lblmontominimoforecolor: color del texto cuando la celda o el control no esta seleccionado
        lblmontoMinimo.ForeColor = Color.White;
        lblmontoMinimo.Location = new Point(586, 137);
        lblmontoMinimo.Name = "lblmontoMinimo";
        lblmontoMinimo.Size = new Size(180, 23);
        lblmontoMinimo.TabIndex = 17;
        lblmontoMinimo.Text = "Monto mínimo";
        // lblmontomaximo lblmontomaximoforecolor: color del texto cuando la celda o el control no esta seleccionado
        lblmontoMaximo.ForeColor = Color.White;
        lblmontoMaximo.Location = new Point(776, 137);
        lblmontoMaximo.Name = "lblmontoMaximo";
        lblmontoMaximo.Size = new Size(180, 23);
        lblmontoMaximo.TabIndex = 19;
        lblmontoMaximo.Text = "Monto máximo";
        // buscar buscarbackcolor: color del fondo del control o de la celda
        buscar.BackColor = Color.SteelBlue;
        buscar.FlatStyle = FlatStyle.Flat;
        buscar.ForeColor = Color.White;
        buscar.Location = new Point(16, 205);
        buscar.Name = "buscar";
        buscar.Size = new Size(130, 36);
        buscar.TabIndex = 21;
        buscar.Text = "Consultar";
        buscar.UseVisualStyleBackColor = false;
        buscar.Click += buscar_Click;
        // grilla grillalocation: posicion horizontal y vertical respecto del contenedor
        grilla.Location = new Point(16, 275);
        grilla.Name = "grilla";
        grilla.ForeColor = Color.Black;
        grilla.DefaultCellStyle.ForeColor = Color.Black;
        grilla.RowsDefaultCellStyle.ForeColor = Color.Black;
        grilla.AlternatingRowsDefaultCellStyle.ForeColor = Color.Black;
        grilla.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.Black;
        grilla.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(190, 220, 250);
        grilla.DefaultCellStyle.SelectionForeColor = Color.Black;
        grilla.DefaultCellStyle.SelectionBackColor = Color.FromArgb(190, 220, 250);
        grilla.AlternatingRowsDefaultCellStyle.SelectionForeColor = Color.Black;
        grilla.AlternatingRowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(190, 220, 250);
        grilla.Size = new Size(968, 400);
        grilla.TabIndex = 23;
        // nudminimo
        nudMinimo.DecimalPlaces = 2;
        nudMinimo.Location = new Point(586, 161);
        nudMinimo.Maximum = new decimal(new int[] { 100000000, 0, 0, 0 });
        nudMinimo.Name = "nudMinimo";
        nudMinimo.Size = new Size(178, 23);
        nudMinimo.TabIndex = 18;
        nudMinimo.ThousandsSeparator = true;
        // nudmaximo
        nudMaximo.DecimalPlaces = 2;
        nudMaximo.Location = new Point(776, 161);
        nudMaximo.Maximum = new decimal(new int[] { 100000000, 0, 0, 0 });
        nudMaximo.Name = "nudMaximo";
        nudMaximo.Size = new Size(178, 23);
        nudMaximo.TabIndex = 20;
        nudMaximo.ThousandsSeparator = true;
        // frminformesventas
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        AutoScroll = true;
        BackColor = Color.FromArgb(13, 19, 27);
        ClientSize = new Size(1000, 700);
        Controls.Add(titulo);
        Controls.Add(lbldesde);
        Controls.Add(desde);
        Controls.Add(lblhasta);
        Controls.Add(hasta);
        Controls.Add(lblproducto);
        Controls.Add(producto);
        Controls.Add(lblcategoria);
        Controls.Add(categoria);
        Controls.Add(lblmarca);
        Controls.Add(marca);
        Controls.Add(lblcliente);
        Controls.Add(cliente);
        Controls.Add(lblenvio);
        Controls.Add(envio);
        Controls.Add(lblubicacion);
        Controls.Add(ubicacion);
        Controls.Add(lblmontoMinimo);
        Controls.Add(nudMinimo);
        Controls.Add(lblmontoMaximo);
        Controls.Add(nudMaximo);
        Controls.Add(buscar);
        Controls.Add(grilla);
        ForeColor = Color.White;
        Name = "frmInformesVentas";
        Text = "Informe de ventas";
        Load += frmInformesVentas_Load;
        // llama a ((systemcomponentmodelisupportinitialize)grilla)endinit para realizar este paso del flujo
        ((System.ComponentModel.ISupportInitialize)grilla).EndInit();
        // llama a ((systemcomponentmodelisupportinitialize)nudminimo)endinit para realizar este paso del flujo
        ((System.ComponentModel.ISupportInitialize)nudMinimo).EndInit();
        // llama a ((systemcomponentmodelisupportinitialize)nudmaximo)endinit para realizar este paso del flujo
        ((System.ComponentModel.ISupportInitialize)nudMaximo).EndInit();
        ResumeLayout(false);
    }

    private Label titulo;
    private Label lbldesde;
    private DateTimePicker desde;
    private Label lblhasta;
    private DateTimePicker hasta;
    private Label lblproducto;
    private ComboBox producto;
    private Label lblcategoria;
    private ComboBox categoria;
    private Label lblmarca;
    private ComboBox marca;
    private Label lblcliente;
    private ComboBox cliente;
    private Label lblenvio;
    private ComboBox envio;
    private Label lblubicacion;
    private ComboBox ubicacion;
    private Label lblmontoMinimo;
    private NumericUpDown nudMinimo;
    private Label lblmontoMaximo;
    private NumericUpDown nudMaximo;
    private Button buscar;
    private DataGridView grilla;
}
