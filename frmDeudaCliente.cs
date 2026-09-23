using Prana.Entidades;
using Prana.negocio;
using System.Globalization;

namespace Prana.Presentacion
{
    public partial class frmDeudaCliente : Form
    {
        private readonly Cliente cliente;
        public frmDeudaCliente(Cliente cliente)
        {
            this.cliente = cliente;
            InitializeComponent();
            lblCliente.Text = $"{cliente.Nombre} {cliente.Apellido} · DNI {cliente.DNI}";
        }
        private void frmDeudaCliente_Load(object? sender, EventArgs e)
        {
            try
            {
                // Consulta actualizada por identificador, incluso si hay clientes homónimos.
                var ventas = new CN_Venta().ObtenerDeudasCliente(cliente.IdCliente);
                dgvDeudas.DataSource = ventas;
                lblEstado.Text = ventas.Count == 0 ? "El cliente no tiene ventas con saldo pendiente." :
                    $"{ventas.Count} ventas pendientes. El saldo corresponde a cada venta completa.";
            }
            catch (Exception ex)
            {
                lblEstado.Text = "No se pudo consultar la deuda del cliente.";
                MessageBox.Show(this, ex.Message, "Consulta de deuda", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
