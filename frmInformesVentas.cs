using Prana.Entidades;
using Prana.negocio;
using System.Globalization;
using System.Text;
using System.Windows.Forms;

namespace Prana.Presentacion
{
    public partial class frmInformesVentas : Form
    {
        private readonly CN_InformeVentas negocio = new();
        private readonly CN_DatosPrueba negocioDatosPrueba = new();
        private List<InformeVenta> datos = new();

        // Inicializa los filtros y configura el acceso al lote de pruebas
        public frmInformesVentas()
        {
            InitializeComponent();
            desde.Value = DateTime.Today.AddMonths(-1);
            hasta.Value = DateTime.Today;
            ConfigurarGrilla();
            bool esAdministrador = SesionActual.Usuario?.IdRolUsuario == 1;
            pruebaAdmin.Visible = esAdministrador;
            eliminarPrueba.Visible = esAdministrador;
        }

        // Carga las opciones del informe y el estado del lote de pruebas
        private void frmInformesVentas_Load(object? sender, EventArgs e)
        {
            CargarOpciones();
            Consultar();
            ActualizarEstadoPrueba();
        }

        // Ejecuta la consulta con los filtros actuales
        private void buscar_Click(object? sender, EventArgs e) => Consultar();

        // Exporta los resultados visibles a un archivo CSV
        private void exportar_Click(object? sender, EventArgs e) => Exportar();

        // Carga opciones de catalogo para los filtros del informe
        private void CargarOpciones()
        {
            try
            {
                var opciones = negocio.ObtenerOpciones();
                CargarOpciones(producto, opciones["Producto"]);
                CargarOpciones(categoria, opciones["Categoria"]);
                CargarOpciones(marca, opciones["Marca"]);
                CargarOpciones(cliente, opciones["Cliente"]);
                CargarOpciones(envio, opciones["Envio"]);
                CargarOpciones(ubicacion, opciones["Ubicacion"]);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Informe", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        // Llena un selector con una opcion vacia y los valores disponibles
        private static void CargarOpciones(ComboBox selector, List<string> valores)
        {
            selector.Items.Clear();
            selector.Items.Add(string.Empty);
            selector.Items.AddRange(valores.Where(valor => !string.IsNullOrWhiteSpace(valor)).ToArray());
            selector.SelectedIndex = 0;
        }

        // Aplica estilos y comportamiento de solo lectura a la grilla
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

        // Valida los filtros y consulta las ventas
        private void Consultar()
        {
            if (desde.Value.Date > hasta.Value.Date)
            {
                MessageBox.Show("La fecha Desde no puede ser posterior a la fecha Hasta.", "Informe", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!TryParseNullable(montoMinimo.Text, out decimal? minimo) || !TryParseNullable(montoMaximo.Text, out decimal? maximo))
            {
                MessageBox.Show("Los montos deben ser números válidos.", "Informe", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (minimo.HasValue && maximo.HasValue && minimo > maximo)
            {
                MessageBox.Show("El monto mínimo no puede superar al máximo.", "Informe", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                datos = negocio.Buscar(new FiltroInformeVentas { Desde = desde.Value.Date, Hasta = hasta.Value.Date.AddDays(1), Producto = producto.Text, Categoria = categoria.Text, Marca = marca.Text, Cliente = cliente.Text, Envio = envio.Text, Ubicacion = ubicacion.Text, MontoMinimo = minimo, MontoMaximo = maximo });
                grilla.DataSource = datos;
                ConfigurarColumnas();
                exportar.Enabled = datos.Count > 0;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Informe", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        // Configura los titulos y formatos de las columnas del informe
        private void ConfigurarColumnas()
        {
            if (grilla.Columns.Count == 0) return;
            grilla.Columns[nameof(InformeVenta.IdVenta)].HeaderText = "Venta";
            grilla.Columns[nameof(InformeVenta.Fecha)].HeaderText = "Fecha";
            grilla.Columns[nameof(InformeVenta.Cliente)].HeaderText = "Cliente";
            grilla.Columns[nameof(InformeVenta.Ubicacion)].HeaderText = "Ubicación";
            grilla.Columns[nameof(InformeVenta.Productos)].HeaderText = "Productos";
            grilla.Columns[nameof(InformeVenta.Categorias)].HeaderText = "Categorías";
            grilla.Columns[nameof(InformeVenta.Marcas)].HeaderText = "Marcas";
            grilla.Columns[nameof(InformeVenta.Total)].HeaderText = "Total";
            grilla.Columns[nameof(InformeVenta.Pagado)].HeaderText = "Pagado";
            grilla.Columns[nameof(InformeVenta.Saldo)].HeaderText = "Saldo";
            grilla.Columns[nameof(InformeVenta.Envio)].HeaderText = "Envío";
            grilla.Columns[nameof(InformeVenta.Fecha)].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
            foreach (string nombre in new[] { nameof(InformeVenta.Total), nameof(InformeVenta.Pagado), nameof(InformeVenta.Saldo) }) grilla.Columns[nombre].DefaultCellStyle.Format = "C2";
            grilla.Columns[nameof(InformeVenta.Productos)].Width = 250;
            grilla.Columns[nameof(InformeVenta.Cliente)].Width = 170;
        }

        // Guarda los resultados actuales en formato CSV
        private void Exportar()
        {
            using SaveFileDialog dialogo = new() { Filter = "CSV (*.csv)|*.csv", FileName = "informe-ventas.csv" };
            if (dialogo.ShowDialog() != DialogResult.OK) return;
            var csv = new StringBuilder();
            csv.AppendLine("Venta;Fecha;Cliente;Ubicacion;Productos;Categorias;Marcas;Total;Pagado;Saldo;Envio");
            foreach (var item in datos) csv.AppendLine(string.Join(";", item.IdVenta, item.Fecha.ToString("dd/MM/yyyy HH:mm"), Csv(item.Cliente), Csv(item.Ubicacion), Csv(item.Productos), Csv(item.Categorias), Csv(item.Marcas), item.Total.ToString("F2", CultureInfo.CurrentCulture), item.Pagado.ToString("F2", CultureInfo.CurrentCulture), item.Saldo.ToString("F2", CultureInfo.CurrentCulture), Csv(item.Envio)));
            File.WriteAllText(dialogo.FileName, csv.ToString(), Encoding.UTF8);
        }

        // Escapa un campo para el formato CSV
        private static string Csv(string valor) => "\"" + valor.Replace("\"", "\"\"") + "\"";

        // Convierte un filtro numerico opcional respetando la cultura actual
        private static bool TryParseNullable(string texto, out decimal? valor)
        {
            if (string.IsNullOrWhiteSpace(texto)) { valor = null; return true; }
            if (decimal.TryParse(texto, NumberStyles.Number, CultureInfo.CurrentCulture, out decimal resultado)) { valor = resultado; return true; }
            valor = null;
            return false;
        }

        // Carga registros sinteticos para ejercitar ventas y cierres sin fechas operativas recientes
        private void pruebaAdmin_Click(object? sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Se agregaran 100 clientes, 100 productos, 100 ventas, 100 pagos, 100 entregas y 100 cierres en fechas historicas. Los datos quedaran identificados para poder eliminarlos. Continuar?",
                "Cargar datos de prueba", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
            try
            {
                LoteDatosPrueba lote = negocioDatosPrueba.Cargar();
                if (!lote.FechaDesde.HasValue || !lote.FechaHasta.HasValue)
                    throw new InvalidOperationException("El lote se creo sin fechas para mostrar sus operaciones.");
                CargarOpciones();
                desde.Value = lote.FechaDesde.Value;
                hasta.Value = lote.FechaHasta.Value;
                Consultar();
                ActualizarEstadoPrueba();
                MessageBox.Show(this, $"Lote {lote.Token} creado: {lote.Clientes} clientes, {lote.Productos} productos, {lote.Ventas} ventas, {lote.Pagos} pagos, {lote.Entregas} entregas y {lote.Cierres} cierres. Fechas: {lote.FechaDesde:dd/MM/yyyy} a {lote.FechaHasta:dd/MM/yyyy}.",
                    "Datos de prueba", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "No se pudieron cargar los datos de prueba: " + ex.Message, "Datos de prueba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Elimina exclusivamente las filas que pertenecen al lote registrado
        private void eliminarPrueba_Click(object? sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Se eliminaran solo los registros del lote de prueba activo. Si algun registro de prueba ya se uso en datos ajenos al lote, la operacion se cancelara. Continuar?",
                "Eliminar datos de prueba", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
            try
            {
                negocioDatosPrueba.EliminarActivo();
                CargarOpciones();
                desde.Value = DateTime.Today.AddMonths(-1);
                hasta.Value = DateTime.Today;
                Consultar();
                ActualizarEstadoPrueba();
                MessageBox.Show(this, "Se elimino el lote de prueba activo.", "Datos de prueba", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "No se pudo eliminar el lote de prueba: " + ex.Message, "Datos de prueba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Habilita la carga o la eliminacion segun el estado persistido del lote
        private void ActualizarEstadoPrueba()
        {
            bool esAdministrador = SesionActual.Usuario?.IdRolUsuario == 1;
            pruebaAdmin.Visible = esAdministrador;
            eliminarPrueba.Visible = esAdministrador;
            if (!esAdministrador) return;
            try
            {
                LoteDatosPrueba? lote = negocioDatosPrueba.ObtenerActivo();
                pruebaAdmin.Enabled = lote == null;
                eliminarPrueba.Enabled = lote != null;
                estadoPrueba.Text = lote == null
                    ? "No hay un lote de prueba activo"
                    : $"Lote activo {lote.Token} | Creado {lote.Creado:dd/MM/yyyy HH:mm} | {lote.Cierres} cierres";
            }
            catch (Exception ex)
            {
                pruebaAdmin.Enabled = false;
                eliminarPrueba.Enabled = false;
                estadoPrueba.Text = "No se pudo consultar el estado del lote de prueba";
                MessageBox.Show(this, "No se pudo consultar el estado de los datos de prueba: " + ex.Message, "Datos de prueba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
