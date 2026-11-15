using Prana.Entidades;
using Prana.negocio;
using System.Globalization;

namespace Prana.Presentacion
{
    // agrupa los datos y operaciones de frm deuda cliente
    public partial class frmDeudaCliente : Form
    {
        // conserva cliente entre las operaciones de esta clase
        private readonly Cliente cliente;
        // prepara frm deuda cliente antes de que se utilice la instancia
        public frmDeudaCliente(Cliente cliente)
        {
            this.cliente = cliente;
            InitializeComponent();
            lblCliente.Text = $"{cliente.Nombre} {cliente.Apellido} · DNI {cliente.DNI}";
        }

        // prepara la informacion inicial cuando se carga la ventana
        private void frmDeudaCliente_Load(object? sender, EventArgs e)
        {
            try
            {
                // consulta actualizada por identificador, incluso si hay clientes homonimos
                List<venta> ventas = new CN_Venta().ObtenerDeudasCliente(cliente.IdCliente);
                dgvDeudas.DataSource = ventas;
                // comprueba si hay elementos para continuar con esta operacion
                if (ventas.Count == 0)
                {
                    lblEstado.Text = "El cliente no tiene ventas con saldo pendiente.";
                }
                else
                {
                    lblEstado.Text = $"{ventas.Count} ventas pendientes. El saldo corresponde a cada venta completa.";
                }
            }
            // atiende el error de esta operacion antes de informar o propagar el problema
            catch (Exception error)
            {
                lblEstado.Text = "No se pudo consultar la deuda del cliente.";
                MessageBox.Show(this, error.Message, "Consulta de deuda", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
