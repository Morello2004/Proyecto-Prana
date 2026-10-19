using Prana.Entidades;
using Prana.negocio;
using System.Windows.Forms;

namespace Prana.Presentacion
{
    public partial class frmAlertas : Form
    {
        private readonly CN_InformeVentas negocio = new();
        private readonly CN_Entrega negocioEntregas = new();

        public frmAlertas()
        {
            InitializeComponent();
            grilla.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
        }

        private async void frmAlertas_Load(object? sender, EventArgs e) => await CargarAsync();

        private async Task CargarAsync()
        {
            string etapa = "pagos pendientes";
            try
            {
                estado.Text = "Consultando pagos pendientes...";
                List<AlertaPagoPendiente> alertas = await Task.Run(negocio.ObtenerAlertas);
                estado.Text = alertas.Count == 0 ? "No hay pagos pendientes vencidos." : $"{alertas.Count} venta(s) requieren seguimiento.";

                etapa = "faltantes de stock";
                lblStock.Text = "Consultando faltantes de stock...";
                var alertasStock = await Task.Run(negocioEntregas.ObtenerAlertasStock);

                grillaStock.DataSource = alertasStock;

                grillaStock.Columns[nameof(AlertaStockPendiente.IdVariante)].Visible = false;
                grillaStock.Columns[nameof(AlertaStockPendiente.Producto)].HeaderText = "Producto / variante";
                grillaStock.Columns[nameof(AlertaStockPendiente.Pendientes)].HeaderText = "Pendientes de preparar";
                grillaStock.Columns[nameof(AlertaStockPendiente.Disponible)].HeaderText = "Stock disponible";
                grillaStock.Columns[nameof(AlertaStockPendiente.Faltantes)].HeaderText = "Unidades faltantes";

                lblStock.Text = alertasStock.Count == 0 ? "No hay faltantes para las ventas pendientes." : $"{alertasStock.Count} variante(s) necesitan ingreso de stock.";
                grilla.DataSource = alertas;
                if (grilla.Columns.Count == 0) return;
                grilla.Columns[nameof(AlertaPagoPendiente.IdVenta)].HeaderText = "Venta";
                grilla.Columns[nameof(AlertaPagoPendiente.Cliente)].HeaderText = "Cliente";
                grilla.Columns[nameof(AlertaPagoPendiente.Productos)].HeaderText = "Productos";
                grilla.Columns[nameof(AlertaPagoPendiente.PrimerPago)].HeaderText = "Primer pago";
                grilla.Columns[nameof(AlertaPagoPendiente.Total)].HeaderText = "Total";
                grilla.Columns[nameof(AlertaPagoPendiente.Pagado)].HeaderText = "Pagado";
                grilla.Columns[nameof(AlertaPagoPendiente.Saldo)].HeaderText = "Saldo";
                grilla.Columns[nameof(AlertaPagoPendiente.DiasVencida)].HeaderText = "Días vencida";
                grilla.Columns[nameof(AlertaPagoPendiente.PrimerPago)].DefaultCellStyle.Format = "dd/MM/yyyy";
                foreach (string nombre in new[] { nameof(AlertaPagoPendiente.Total), nameof(AlertaPagoPendiente.Pagado), nameof(AlertaPagoPendiente.Saldo) }) grilla.Columns[nombre].DefaultCellStyle.Format = "C2";
                grilla.Columns[nameof(AlertaPagoPendiente.Productos)].Width = 280;
            }
            catch (Exception ex)
            {
                if (etapa == "pagos pendientes")
                    estado.Text = "Error al consultar pagos pendientes.";
                else
                    lblStock.Text = "Error al consultar faltantes de stock.";
                MessageBox.Show(this, ex.Message, "Error al cargar alertas", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void grilla_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
