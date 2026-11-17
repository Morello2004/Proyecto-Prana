using Prana.Entidades;
using Prana.negocio;

namespace Prana.Presentacion
{
    // agrupa los datos y operaciones de frm entregas
    public partial class frmEntregas : Form
    {
        // conserva negocio entre las operaciones de esta clase
        private readonly CN_Entrega negocio = new();
        // dato producto de tipo productoporentregar? se obtiene al consultar la propiedad mediante la expresion siguiente
        private ProductoPorEntregar? Producto => dgvPendientes.CurrentRow?.DataBoundItem as ProductoPorEntregar;

        // prepara frm entregas antes de que se utilice la instancia
        public frmEntregas()
        {
            InitializeComponent();
        }

        // resuelve buscar con los datos recibidos y entrega el resultado al codigo que lo llama
        private void Buscar()
        {
            try
            {
                List<ProductoPorEntregar> pendientes = negocio.Pendientes(txtBuscar.Text);
                List<EntregaRegistrada> entregas = negocio.Listar(txtBuscar.Text, chkPendientes.Checked);
                dgvPendientes.DataSource = pendientes;
                dgvEntregas.DataSource = entregas;
                lblPendientes.Text = $"Productos por preparar · {pendientes.Count}";
                lblEntregas.Text = $"Entregas preparadas · {entregas.Count}";
            }
            // atiende el error de esta operacion antes de informar o propagar el problema
            catch (Exception error)
            {
                dgvPendientes.DataSource = null;
                dgvEntregas.DataSource = null;
                Error(error);
            }

            Seleccionar();
            ActualizarConfirmacion();
        }

        // resuelve seleccionar con los datos recibidos y entrega el resultado al codigo que lo llama
        private void Seleccionar()
        {
            ProductoPorEntregar producto = Producto;
            if (producto == null)
            {
                lblSeleccion.Text = "Seleccione un producto para preparar su entrega";
            }
            else
            {
                lblSeleccion.Text = $"Venta #{producto.IdVenta} · {producto.Cliente} · {producto.Producto}";
            }
            nudCantidad.Value = 1;
            txtDireccion.Text = producto?.DomicilioSugerido ?? "";
            txtLocalidad.Text = producto?.LocalidadSugerida ?? "";
            bool retiro = producto?.Modalidad.StartsWith("Retiro", StringComparison.OrdinalIgnoreCase) == true;
            if (retiro)
            {
                txtDireccion.Text = "Retiro en local";
                txtLocalidad.Clear();
            }

            txtDireccion.ReadOnly = retiro || producto == null;
            txtLocalidad.Enabled = !retiro && producto != null;
            btnPreparar.Enabled = producto != null && PuedeGestionar();
        }

        // resuelve puede gestionar con los datos recibidos y entrega el resultado al codigo que lo llama
        private bool PuedeGestionar() => SesionActual.Usuario != null && Permisos.TienePermiso(Permisos.GestionarEntregas);
        // resuelve actualizar confirmacion con los datos recibidos y entrega el resultado al codigo que lo llama
        private void ActualizarConfirmacion() => btnConfirmar.Enabled = dgvEntregas.CurrentRow?.DataBoundItem is EntregaRegistrada entrega && entrega.Estado == "Pendiente" && PuedeGestionar();
        // resuelve preparar con los datos recibidos y entrega el resultado al codigo que lo llama
        private void Preparar(object? sender, EventArgs e)
        {
            if (Producto == null)
            {
                return;
            }

            try
            {
                btnPreparar.Enabled = false;
                // negocio valida; datos guarda remito, envio, stock y actividad en una transaccion
                negocio.Preparar(Producto, (int)nudCantidad.Value, txtDireccion.Text, txtLocalidad.Text);
                Buscar();
                MessageBox.Show(this, "Entrega preparada y stock reservado. Selecciónela abajo para confirmar cuando se entregue.", "Entregas");
            }
            // atiende el error de esta operacion antes de informar o propagar el problema
            catch (Exception error)
            {
                Error(error);
                btnPreparar.Enabled = PuedeGestionar();
            }
        }

        // resuelve confirmar con los datos recibidos y entrega el resultado al codigo que lo llama
        private void Confirmar(object? sender, EventArgs e)
        {
            if (dgvEntregas.CurrentRow?.DataBoundItem is not EntregaRegistrada entrega)
            {
                return;
            }

            try
            {
                btnConfirmar.Enabled = false;
                negocio.Confirmar(entrega.IdEnvio);
                Buscar();
                MessageBox.Show(this, "Entrega confirmada correctamente.", "Entregas");
            }
            // atiende el error de esta operacion antes de informar o propagar el problema
            catch (Exception error)
            {
                Error(error);
                ActualizarConfirmacion();
            }
        }

        // resuelve error con los datos recibidos y entrega el resultado al codigo que lo llama
        private void Error(Exception ex) => MessageBox.Show(this, ex.Message, "Entregas", MessageBoxButtons.OK, MessageBoxIcon.Error);
        // ejecuta la accion de btn buscar cuando el usuario pulsa el control
        private void btnBuscar_Click(object? sender, EventArgs e)
        {
            Buscar();
        }

        // interpreta la tecla pulsada y ejecuta la accion prevista por el formulario
        private void txtBuscar_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                Buscar();
            }
        }

        // actualiza los controles relacionados cuando cambia la seleccion puede dispararse tambien al cargar los datos
        private void dgvPendientes_SelectionChanged(object? sender, EventArgs e)
        {
            Seleccionar();
        }

        // actualiza los controles relacionados cuando cambia la seleccion puede dispararse tambien al cargar los datos
        private void dgvEntregas_SelectionChanged(object? sender, EventArgs e)
        {
            ActualizarConfirmacion();
        }

        // prepara la informacion inicial cuando se carga la ventana
        private void frmEntregas_Load(object? sender, EventArgs e) => Buscar();
    }
}
