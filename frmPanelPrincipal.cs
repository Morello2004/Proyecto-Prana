using System;
using System.Windows.Forms;
using Prana.negocio;

namespace Prana.Presentacion
{
    public partial class frmPanelPrincipal : Form
    {
        private readonly CN_Venta cnVenta = new CN_Venta();
        public frmPanelPrincipal()
        {
            InitializeComponent();
        }

        private void frmPanelPrincipal_Load(object sender, EventArgs e)
        {
            // Evita que el DataGridView cree columnas automáticamente
            dgvUltimasVentas.AutoGenerateColumns = false;

            colNumeroVenta.DataPropertyName = "IdVenta";
            colFecha.DataPropertyName = "Fecha";
            colTotal.DataPropertyName = "Total";
            colEstado.DataPropertyName = "Estado";
            colCliente.DataPropertyName = "NombreCompletoCliente";

            // Carga en el DataGridView las ventas obtenidas desde la capa de negocio
            dgvUltimasVentas.DataSource = cnVenta.ObtenerUltimasVentas();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void flpAlertas_Click(object sender, EventArgs e)
        {

        }

        private void iconButton2_Click(object sender, EventArgs e)
        {
            // Busca la ventana principal que contiene este formulario
            if (Parent?.FindForm() is Interfaz interfaz)
            {
                // Reemplaza el panel principal por el formulario Nueva Venta
                interfaz.AbrirFormulario(new frmNuevaVenta());
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}