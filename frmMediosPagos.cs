using Prana.Entidades;
using Prana.negocio;
using System;
using System.Windows.Forms;

namespace Prana.Presentacion
{
    // Permite editar los recargos de los medios de pago fijos
    public partial class frmMediosPagos : Form
    {
        // Coordina la consulta y edicion de recargos
        private readonly CN_MedioPago cnMedioPago = new CN_MedioPago();

        // Conserva el medio seleccionado para modificar sus recargos
        private int idMedioPagoSeleccionado;

        // Inicializa el formulario y bloquea los datos que no se pueden editar
        public frmMediosPagos()
        {
            InitializeComponent();
            cboTipoCaja.Items.AddRange(TiposCaja.Todos);
            cboTipoCaja.SelectedIndex = -1;
            cboTipoCaja.Enabled = false;
            txtNombreMedioPago.ReadOnly = true;
            btnGuardar.Enabled = false;
            ConfigurarCuotas(false);
            lblSeccion.Text = "Selecciona un medio de pago para editar sus recargos";
            ConfigurarDistribucion();
        }

        // Carga los medios de pago fijos cuando se abre la pantalla
        private void frmMediosPagos_Load(object? sender, EventArgs e)
        {
            CargarMedios();
        }

        // Carga los cuatro medios de pago fijos en la grilla
        private void CargarMedios()
        {
            dgvMediosPagos.DataSource = null;
            dgvMediosPagos.DataSource = cnMedioPago.ObtenerTodos();
            dgvMediosPagos.Columns[nameof(MedioPago.IdMedioPago)].Visible = false;
            dgvMediosPagos.Columns[nameof(MedioPago.Activo)].Visible = false;
            dgvMediosPagos.Columns[nameof(MedioPago.TipoCaja)].HeaderText = "Tipo en caja";
            dgvMediosPagos.Columns[nameof(MedioPago.NombreMedioPago)].HeaderText = "Medio de pago";
            dgvMediosPagos.Columns[nameof(MedioPago.RecargoMedioPago)].HeaderText = "Recargo general %";
            dgvMediosPagos.Columns[nameof(MedioPago.Recargo2)].HeaderText = "2 cuotas %";
            dgvMediosPagos.Columns[nameof(MedioPago.Recargo3)].HeaderText = "3 cuotas %";
            dgvMediosPagos.Columns[nameof(MedioPago.Recargo6)].HeaderText = "6 cuotas %";
            dgvMediosPagos.Columns[nameof(MedioPago.Recargo9)].HeaderText = "9 cuotas %";
            dgvMediosPagos.Columns[nameof(MedioPago.Recargo12)].HeaderText = "12 cuotas %";
            dgvMediosPagos.ClearSelection();
            LimpiarSeleccion();
        }

        // Guarda los recargos del medio seleccionado
        private void btnGuardar_Click(object? sender, EventArgs e)
        {
            try
            {
                if (idMedioPagoSeleccionado == 0) throw new InvalidOperationException("Seleccione un medio de pago para editar sus recargos.");
                MedioPago medio = ObtenerMedioDesdeFormulario();
                medio.IdMedioPago = idMedioPagoSeleccionado;
                cnMedioPago.Modificar(medio);
                MessageBox.Show("Recargos guardados correctamente.", "Medios de pago", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarMedios();
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudieron guardar los recargos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Carga en el formulario el medio seleccionado
        private void dgvMediosPagos_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || dgvMediosPagos.Rows[e.RowIndex].DataBoundItem is not MedioPago medio) return;
            idMedioPagoSeleccionado = medio.IdMedioPago;
            cboTipoCaja.SelectedItem = medio.TipoCaja;
            txtNombreMedioPago.Text = medio.NombreMedioPago;
            nudRecargoGeneral.Value = medio.RecargoMedioPago;
            bool permiteCuotas = medio.TipoCaja == "Crédito";
            ConfigurarCuotas(permiteCuotas);
            nudRecargo2.Value = permiteCuotas ? medio.Recargo2 : 0;
            nudRecargo3.Value = permiteCuotas ? medio.Recargo3 : 0;
            nudRecargo6.Value = permiteCuotas ? medio.Recargo6 : 0;
            nudRecargo9.Value = permiteCuotas ? medio.Recargo9 : 0;
            nudRecargo12.Value = permiteCuotas ? medio.Recargo12 : 0;
            btnGuardar.Enabled = true;
        }

        // Convierte los recargos editables en una entidad
        private MedioPago ObtenerMedioDesdeFormulario()
        {
            return new MedioPago
            {
                TipoCaja = cboTipoCaja.Text,
                NombreMedioPago = txtNombreMedioPago.Text,
                RecargoMedioPago = nudRecargoGeneral.Value,
                Recargo2 = nudRecargo2.Value,
                Recargo3 = nudRecargo3.Value,
                Recargo6 = nudRecargo6.Value,
                Recargo9 = nudRecargo9.Value,
                Recargo12 = nudRecargo12.Value,
                Activo = true
            };
        }

        // Habilita las cuotas unicamente para el medio de credito
        private void ConfigurarCuotas(bool habilitadas)
        {
            lblRecargo2.Enabled = habilitadas;
            nudRecargo2.Enabled = habilitadas;
            lblRecargo3.Enabled = habilitadas;
            nudRecargo3.Enabled = habilitadas;
            lblRecargo6.Enabled = habilitadas;
            nudRecargo6.Enabled = habilitadas;
            lblRecargo9.Enabled = habilitadas;
            nudRecargo9.Enabled = habilitadas;
            lblRecargo12.Enabled = habilitadas;
            nudRecargo12.Enabled = habilitadas;
        }

        // Limpia la seleccion y bloquea la edicion hasta elegir un medio
        private void LimpiarSeleccion()
        {
            idMedioPagoSeleccionado = 0;
            cboTipoCaja.SelectedIndex = -1;
            txtNombreMedioPago.Clear();
            nudRecargoGeneral.Value = 0;
            nudRecargo2.Value = 0;
            nudRecargo3.Value = 0;
            nudRecargo6.Value = 0;
            nudRecargo9.Value = 0;
            nudRecargo12.Value = 0;
            ConfigurarCuotas(false);
            btnGuardar.Enabled = false;
        }
    }
}
