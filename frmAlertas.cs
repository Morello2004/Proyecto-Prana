using Prana.Entidades;
using Prana.negocio;
using System.Windows.Forms;

namespace Prana.Presentacion
{
    public class frmAlertas : Form
    {
        private readonly CN_InformeVentas negocio = new();
        private readonly DataGridView grilla = new();
        private readonly Label estado = new();

        public frmAlertas()
        {
            Text = "Alertas";
            BackColor = Color.FromArgb(13, 19, 27);
            ForeColor = Color.White;
            Padding = new Padding(16);
            var titulo = new Label { Text = "Alertas de pagos pendientes", Font = new Font("Segoe UI", 18, FontStyle.Bold), AutoSize = true, Dock = DockStyle.Top, Height = 42 };
            estado.Text = "Ventas con saldo pendiente luego de un mes desde el primer pago";
            estado.Dock = DockStyle.Top;
            estado.Height = 30;
            estado.ForeColor = Color.FromArgb(180, 200, 225);
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
            Controls.Add(grilla);
            Controls.Add(estado);
            Controls.Add(titulo);
            Cargar();
        }

        private void Cargar()
        {
            try
            {
                List<AlertaPagoPendiente> alertas = negocio.ObtenerAlertas();
                grilla.DataSource = alertas;
                estado.Text = alertas.Count == 0 ? "No hay pagos pendientes vencidos." : $"{alertas.Count} venta(s) requieren seguimiento.";
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
            catch (Exception ex) { MessageBox.Show(ex.Message, "Alertas", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
    }
}
