using Prana.Entidades;
using Prana.negocio;
using System.Linq;
using System.Windows.Forms;

namespace Prana.Presentacion
{
    // agrupa los datos y operaciones de frm informes ventas
    public partial class frmInformesVentas : Form
    {
        // conserva negocio entre las operaciones de esta clase
        private readonly CN_InformeVentas negocio = new();
        // inicializa los filtros del informe
        public frmInformesVentas()
        {
            InitializeComponent();
            // procesa cada selector de new[] { producto, categoria, marca, cliente, envio, ubicacion } sin cambiar el orden de la coleccion
            foreach (ComboBox selector in new[]
            {
                producto,
                categoria,
                marca,
                cliente,
                envio,
                ubicacion
            }

            )
            {
                selector.DropDownStyle = ComboBoxStyle.DropDown;
                selector.AutoCompleteSource = AutoCompleteSource.ListItems;
                selector.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            }

            producto.MaxLength = 30;
            categoria.MaxLength = 30;
            marca.MaxLength = 50;
            cliente.MaxLength = 55;
            envio.MaxLength = 20;
            ubicacion.MaxLength = 50;
            producto.SelectionChangeCommitted += producto_SelectionChangeCommitted;
            producto.TextUpdate += producto_TextChanged;
            desde.Value = DateTime.Today.AddMonths(-1);
            hasta.Value = DateTime.Today;
            ConfigurarGrilla();
        }

        // carga las opciones y consulta el informe
        private void frmInformesVentas_Load(object? sender, EventArgs e)
        {
            CargarOpciones();
            Consultar();
        }

        // ejecuta la consulta con los filtros actuales atiende la pulsacion de buscar y ejecuta la accion asociada el evento no significa que la operacion ya se haya guardado
        private void buscar_Click(object? sender, EventArgs e) => Consultar();
        // recibe la opcion que el usuario confirmo en el desplegable; no confundirla con escribir texto sin elegir
        private void producto_SelectionChangeCommitted(object? sender, EventArgs e)
        {
            if (producto.SelectedIndex < 0)
            {
                return;
            }

            categoria.Text = string.Empty;
            marca.Text = string.Empty;
            categoria.Enabled = true;
            marca.Enabled = true;
            string nombreProducto = producto.Text.Trim();
            // distingue un dato vacio de un valor ingresado antes de continuar
            if (string.IsNullOrWhiteSpace(nombreProducto))
            {
                return;
            }

            try
            {
                (string Categoria, string Marca)? datosProducto = negocio.ObtenerCategoriaMarcaProducto(nombreProducto);
                // comprueba si se recibio el valor opcional antes de utilizarlo
                if (!datosProducto.HasValue)
                {
                    return;
                }

                categoria.Text = datosProducto.Value.Categoria;
                marca.Text = datosProducto.Value.Marca;
                categoria.Enabled = false;
                marca.Enabled = false;
            }
            // atiende el error de esta operacion antes de informar o propagar el problema
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "Informe", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // responde al cambio del texto; revisa el estado de busqueda o de seleccion antes de continuar
        private void producto_TextChanged(object? sender, EventArgs e)
        {
            categoria.Text = string.Empty;
            marca.Text = string.Empty;
            categoria.Enabled = true;
            marca.Enabled = true;
        }

        // carga las opciones disponibles para los filtros del informe
        private void CargarOpciones()
        {
            try
            {
                Dictionary<string, List<string>> opciones = negocio.ObtenerOpciones();
                producto.Items.Clear();
                producto.Items.Add(string.Empty);
                producto.Items.AddRange(opciones["Producto"].ToArray());
                categoria.Items.Clear();
                categoria.Items.Add(string.Empty);
                categoria.Items.AddRange(opciones["Categoria"].ToArray());
                marca.Items.Clear();
                marca.Items.Add(string.Empty);
                marca.Items.AddRange(opciones["Marca"].ToArray());
                cliente.Items.Clear();
                cliente.Items.Add(string.Empty);
                cliente.Items.AddRange(opciones["Cliente"].ToArray());
                envio.Items.Clear();
                envio.Items.Add(string.Empty);
                envio.Items.AddRange(opciones["Envio"].ToArray());
                ubicacion.Items.Clear();
                ubicacion.Items.Add(string.Empty);
                ubicacion.Items.AddRange(opciones["Ubicacion"].ToArray());
            }
            // atiende el error de esta operacion antes de informar o propagar el problema
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "Informe", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // aplica estilos y comportamiento de solo lectura a la grilla
        private void ConfigurarGrilla()
        {
            grilla.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            grilla.ReadOnly = true;
            grilla.AllowUserToAddRows = false;
            grilla.AutoGenerateColumns = true;
            grilla.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            grilla.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            grilla.RowHeadersVisible = false;
            grilla.BackgroundColor = Color.FromArgb(13, 19, 27);
            grilla.DefaultCellStyle.BackColor = Color.White;
            grilla.DefaultCellStyle.ForeColor = Color.FromArgb(21, 30, 41);
            grilla.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(236, 242, 249);
            grilla.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
        }

        // valida los filtros y consulta las ventas
        private void Consultar()
        {
            if (desde.Value.Date > hasta.Value.Date)
            {
                MessageBox.Show("La fecha Desde no puede ser posterior a la fecha Hasta.", "Informe", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            decimal? minimo;
            if (nudMinimo.Value > 0)
            {
                minimo = nudMinimo.Value;
            }
            else
            {
                minimo = null;
            }
            decimal? maximo;
            if (nudMaximo.Value > 0)
            {
                maximo = nudMaximo.Value;
            }
            else
            {
                maximo = null;
            }
            // comprueba si se recibio el valor opcional antes de utilizarlo
            if (minimo.HasValue && maximo.HasValue && minimo > maximo)
            {
                MessageBox.Show("El monto mínimo no puede superar al máximo.", "Informe", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                grilla.DataSource = negocio.Buscar(new FiltroInformeVentas
                {
                    Desde = desde.Value.Date,
                    Hasta = hasta.Value.Date.AddDays(1),
                    Producto = producto.Text,
                    Categoria = categoria.Text,
                    Marca = marca.Text,
                    Cliente = cliente.Text,
                    Envio = envio.Text,
                    Ubicacion = ubicacion.Text,
                    MontoMinimo = minimo,
                    MontoMaximo = maximo

                });
                ConfigurarColumnas();
            }
            // atiende el error de esta operacion antes de informar o propagar el problema
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "Informe", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // configura los titulos y formatos de las columnas del informe
        private void ConfigurarColumnas()
        {
            // comprueba si hay elementos para continuar con esta operacion
            if (grilla.Columns.Count == 0)
            {
                return;
            }

            // grillacolumns[nameof(informeventaidventa)]headertext: titulo de la columna; no es el nombre de la propiedad enlazada
            grilla.Columns[nameof(InformeVenta.IdVenta)].HeaderText = "Venta";
            // grillacolumns[nameof(informeventafecha)]headertext: titulo de la columna; no es el nombre de la propiedad enlazada
            grilla.Columns[nameof(InformeVenta.Fecha)].HeaderText = "Fecha";
            // grillacolumns[nameof(informeventacliente)]headertext: titulo de la columna; no es el nombre de la propiedad enlazada
            grilla.Columns[nameof(InformeVenta.Cliente)].HeaderText = "Cliente";
            // grillacolumns[nameof(informeventaubicacion)]headertext: titulo de la columna; no es el nombre de la propiedad enlazada
            grilla.Columns[nameof(InformeVenta.Ubicacion)].HeaderText = "Ubicación";
            // grillacolumns[nameof(informeventaproductos)]headertext: titulo de la columna; no es el nombre de la propiedad enlazada
            grilla.Columns[nameof(InformeVenta.Productos)].HeaderText = "Productos";
            // grillacolumns[nameof(informeventacategorias)]headertext: titulo de la columna; no es el nombre de la propiedad enlazada
            grilla.Columns[nameof(InformeVenta.Categorias)].HeaderText = "Categorías";
            // grillacolumns[nameof(informeventamarcas)]headertext: titulo de la columna; no es el nombre de la propiedad enlazada
            grilla.Columns[nameof(InformeVenta.Marcas)].HeaderText = "Marcas";
            // grillacolumns[nameof(informeventatotal)]headertext: titulo de la columna; no es el nombre de la propiedad enlazada
            grilla.Columns[nameof(InformeVenta.Total)].HeaderText = "Total";
            // grillacolumns[nameof(informeventapagado)]headertext: titulo de la columna; no es el nombre de la propiedad enlazada
            grilla.Columns[nameof(InformeVenta.Pagado)].HeaderText = "Pagado";
            // grillacolumns[nameof(informeventasaldo)]headertext: titulo de la columna; no es el nombre de la propiedad enlazada
            grilla.Columns[nameof(InformeVenta.Saldo)].HeaderText = "Saldo";
            // grillacolumns[nameof(informeventaenvio)]headertext: titulo de la columna; no es el nombre de la propiedad enlazada
            grilla.Columns[nameof(InformeVenta.Envio)].HeaderText = "Envío";
            // grillacolumns[nameof(informeventafecha)]defaultcellstyleformat: formato de presentacion, por ejemplo moneda o fecha; no modifica el valor original
            grilla.Columns[nameof(InformeVenta.Fecha)].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
            // procesa cada nombre de new[] { nameof(informe ventatotal), nameof(informe ventapagado), nameof(informe ventasaldo) } sin cambiar el orden de la coleccion
            foreach (string nombre in new[]
            {
                nameof(InformeVenta.Total),
                nameof(InformeVenta.Pagado),
                nameof(InformeVenta.Saldo)
            }

            )
            {
                // grillacolumns[nombre]defaultcellstyleformat: formato de presentacion, por ejemplo moneda o fecha; no modifica el valor original
                grilla.Columns[nombre].DefaultCellStyle.Format = "C2";
            }

            grilla.Columns[nameof(InformeVenta.Productos)].Width = 250;
            grilla.Columns[nameof(InformeVenta.Cliente)].Width = 170;
        }

        // actualiza los controles relacionados cuando cambia la seleccion puede dispararse tambien al cargar los datos
        private void envio_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
    }
}
