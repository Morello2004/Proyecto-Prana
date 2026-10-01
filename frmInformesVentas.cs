using Prana.Entidades;
using Prana.negocio;
using System.Globalization;
using System.Text;
using System.Windows.Forms;

namespace Prana.Presentacion
{
    public class frmInformesVentas : Form
    {
        private readonly CN_InformeVentas negocio = new();
        private readonly DateTimePicker desde = new() { Value = DateTime.Today.AddMonths(-1), Format = DateTimePickerFormat.Short };
        private readonly DateTimePicker hasta = new() { Value = DateTime.Today, Format = DateTimePickerFormat.Short };
        private readonly ComboBox producto = CrearSelector();
        private readonly ComboBox categoria = CrearSelector();
        private readonly ComboBox marca = CrearSelector();
        private readonly ComboBox cliente = CrearSelector();
        private readonly ComboBox envio = CrearSelector();
        private readonly ComboBox ubicacion = CrearSelector();
        private readonly TextBox montoMinimo = new();
        private readonly TextBox montoMaximo = new();
        private readonly DataGridView grilla = new();
        private readonly Button exportar = new() { Text = "Exportar CSV", Enabled = false };
        private List<InformeVenta> datos = new();

        public frmInformesVentas()
        {
            Text = "Informe de ventas";
            BackColor = Color.FromArgb(13, 19, 27);
            ForeColor = Color.White;
            Padding = new Padding(16);
            var titulo = new Label { Text = "Informe de ventas", Font = new Font("Segoe UI", 18, FontStyle.Bold), AutoSize = true, Dock = DockStyle.Top, Height = 42 };
            var filtros = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 132, WrapContents = true, AutoScroll = true, Padding = new Padding(0, 6, 0, 6) };
            AgregarCampo(filtros, "Desde", desde, 100);
            AgregarCampo(filtros, "Hasta", hasta, 100);
            AgregarCampo(filtros, "Producto", producto, 150);
            AgregarCampo(filtros, "Categoría", categoria, 130);
            AgregarCampo(filtros, "Marca", marca, 130);
            AgregarCampo(filtros, "Cliente", cliente, 160);
            AgregarCampo(filtros, "Envío", envio, 130);
            AgregarCampo(filtros, "Ubicación", ubicacion, 150);
            AgregarCampo(filtros, "Monto mínimo", montoMinimo, 110);
            AgregarCampo(filtros, "Monto máximo", montoMaximo, 110);
            var buscar = new Button { Text = "Consultar", Width = 100, Height = 30, BackColor = Color.DodgerBlue, ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            buscar.Click += (_, _) => Consultar();
            exportar.Click += (_, _) => Exportar();
            filtros.Controls.Add(buscar);
            filtros.Controls.Add(exportar);
            ConfigurarGrilla();
            Controls.Add(grilla);
            Controls.Add(filtros);
            Controls.Add(titulo);
            CargarOpciones();
            Consultar();
        }

        private static ComboBox CrearSelector() => new()
        {
            DropDownStyle = ComboBoxStyle.DropDown,
            AutoCompleteMode = AutoCompleteMode.SuggestAppend,
            AutoCompleteSource = AutoCompleteSource.ListItems
        };

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

        private static void CargarOpciones(ComboBox selector, List<string> valores)
        {
            selector.Items.Clear();
            selector.Items.Add(string.Empty);
            selector.Items.AddRange(valores.Where(valor => !string.IsNullOrWhiteSpace(valor)).ToArray());
            selector.SelectedIndex = 0;
        }

        private static void AgregarCampo(FlowLayoutPanel panel, string etiqueta, Control control, int ancho)
        {
            var contenedor = new Panel { Width = ancho, Height = 56 };
            contenedor.Controls.Add(new Label { Text = etiqueta, ForeColor = Color.FromArgb(180, 200, 225), AutoSize = true, Location = new Point(0, 0) });
            control.Location = new Point(0, 22);
            control.Width = ancho - 6;
            contenedor.Controls.Add(control);
            panel.Controls.Add(contenedor);
        }

        private void ConfigurarGrilla()
        {
            grilla.Dock = DockStyle.Fill;
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

        private void Exportar()
        {
            using SaveFileDialog dialogo = new() { Filter = "CSV (*.csv)|*.csv", FileName = "informe-ventas.csv" };
            if (dialogo.ShowDialog() != DialogResult.OK) return;
            var csv = new StringBuilder();
            csv.AppendLine("Venta;Fecha;Cliente;Ubicacion;Productos;Categorias;Marcas;Total;Pagado;Saldo;Envio");
            foreach (var item in datos) csv.AppendLine(string.Join(";", item.IdVenta, item.Fecha.ToString("dd/MM/yyyy HH:mm"), Csv(item.Cliente), Csv(item.Ubicacion), Csv(item.Productos), Csv(item.Categorias), Csv(item.Marcas), item.Total.ToString("F2", CultureInfo.CurrentCulture), item.Pagado.ToString("F2", CultureInfo.CurrentCulture), item.Saldo.ToString("F2", CultureInfo.CurrentCulture), Csv(item.Envio)));
            File.WriteAllText(dialogo.FileName, csv.ToString(), Encoding.UTF8);
        }

        private static string Csv(string valor) => "\"" + valor.Replace("\"", "\"\"") + "\"";

        private static bool TryParseNullable(string texto, out decimal? valor)
        {
            if (string.IsNullOrWhiteSpace(texto)) { valor = null; return true; }
            if (decimal.TryParse(texto, NumberStyles.Number, CultureInfo.CurrentCulture, out decimal resultado)) { valor = resultado; return true; }
            valor = null;
            return false;
        }
    }
}
