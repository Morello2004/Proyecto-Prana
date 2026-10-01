using Prana.Entidades;
using Prana.negocio;
using System.Globalization;

namespace Prana.Presentacion
{
    public partial class frmPanelPrincipal : Form
    {
        private readonly CN_Venta negocioVentas = new();
        private readonly CultureInfo moneda = CultureInfo.GetCultureInfo("es-AR");
        public frmPanelPrincipal()
        {
            InitializeComponent();
            ConfigurarAccesosCaja();
            foreach (var columna in new[] { colTotal, colPagado, colSaldo })
            {
                columna.DefaultCellStyle.FormatProvider = moneda;
                columna.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
        }
        private void ConfigurarAccesosCaja()
        {
            pnlEncabezado.Height = 120;
            lblTitulo.Top = 8;
            var caja = new Button { Text="Caja y gastos",Width=145,Height=34,BackColor=Color.SteelBlue,FlatStyle=FlatStyle.Flat };
            caja.Visible = Permisos.TienePermiso(Permisos.GestionarCaja);
            caja.Click += (_,_) => { if(Parent?.FindForm() is Interfaz interfaz) interfaz.AbrirFormulario(new frmCaja()); };
            var documentos = new Button { Text="Garantía / remito",Width=160,Height=34,BackColor=Color.SteelBlue,FlatStyle=FlatStyle.Flat };
            documentos.Click += (_,_) => {
                if(dgvUltimasVentas.CurrentRow?.DataBoundItem is not venta seleccionada) { MessageBox.Show("Seleccione una venta del listado."); return; }
                try { DocumentosPrana.MostrarVenta(this,seleccionada.IdVenta); }
                catch(Exception ex) { MessageBox.Show(this,ex.Message,"Comprobantes",MessageBoxButtons.OK,MessageBoxIcon.Warning); }
            };
            var acciones=DistribucionAdaptable.Fila(btnNuevaVenta,btnStock,caja,documentos);
            acciones.Dock=DockStyle.Bottom; acciones.Height=60;
            pnlEncabezado.Controls.Add(acciones);
        }
        private void frmPanelPrincipal_Load(object? sender, EventArgs e) => CargarVentas();

        // Presentación solo consulta negocio y enlaza resultados; no calcula deudas.
        private void CargarVentas()
        {
            try
            {
                UseWaitCursor = true;
                var ventas = negocioVentas.BuscarVentas(txtBuscarVenta.Text);
                dgvUltimasVentas.DataSource = ventas;
                lblResultados.Text = ventas.Count == 0 ? "No se encontraron ventas" :
                    string.IsNullOrWhiteSpace(txtBuscarVenta.Text) ? $"Últimas 10 ventas · {ventas.Count} registradas" : $"Resultados de búsqueda · {ventas.Count} ventas";
                var resumen = negocioVentas.ObtenerResumen();
                lblTotalHoy.Text = resumen.TotalHoy.ToString("C2", moneda);
                lblCantidadHoy.Text = $"{resumen.CantidadHoy} operaciones registradas hoy";
                lblTotalPendiente.Text = resumen.SaldoPendiente.ToString("C2", moneda);
                lblCantidadPendientes.Text = resumen.VentasPendientes.ToString();
            }
            catch (Exception ex)
            {
                dgvUltimasVentas.DataSource = null;
                lblResultados.Text = "No se pudieron cargar las ventas. Presione Actualizar para reintentar.";
                lblTotalHoy.Text = lblTotalPendiente.Text = lblCantidadPendientes.Text = "—";
                lblCantidadHoy.Text = "Sin datos disponibles";
                MessageBox.Show(this, ex.Message, "Consulta de ventas", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally { UseWaitCursor = false; }
        }
        private void btnBuscar_Click(object? sender, EventArgs e) => CargarVentas();
        private void btnUltimas_Click(object? sender, EventArgs e)
        {
            txtBuscarVenta.Clear();
            CargarVentas();
        }
        private void txtBuscarVenta_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            e.SuppressKeyPress = true;
            CargarVentas();
        }
        private void btnNuevaVenta_Click(object? sender, EventArgs e)
        {
            if (Parent?.FindForm() is Interfaz interfaz) interfaz.AbrirFormulario(new frmNuevaVenta());
        }
        private void btnStock_Click(object? sender, EventArgs e)
        {
            if (Parent?.FindForm() is Interfaz interfaz) interfaz.AbrirFormulario(new frmStock());
        }
        private void dgvUltimasVentas_SelectionChanged(object? sender, EventArgs e)
        {
            lblDetalle.Text = dgvUltimasVentas.CurrentRow?.DataBoundItem is venta seleccionada
                ? $"Venta #{seleccionada.IdVenta} · Productos: {seleccionada.Productos}"
                : "Seleccione una venta para ver sus productos.";
        }
        private void dgvUltimasVentas_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == colSaldo.Index && e.Value is decimal saldo && saldo > 0)
                e.CellStyle!.ForeColor = Color.FromArgb(165, 72, 0);
        }
    }
}
