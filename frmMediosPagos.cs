using Prana.Entidades;
using Prana.negocio;
using System;
using System.Windows.Forms;

namespace Prana.Presentacion
{
    // Permite administrar medios de pago y sus recargos.
    public partial class frmMediosPagos : Form
    {
        // Coordina el ABM de medios de pago.
        private readonly CN_MedioPago cnMedioPago = new CN_MedioPago();

        // Conserva el medio seleccionado para modificarlo.
        private int idMedioPagoSeleccionado;
        private readonly ComboBox cboTipoCaja = new() { DropDownStyle = ComboBoxStyle.DropDownList };

        // Inicializa el formulario y carga los medios activos.
        public frmMediosPagos()
        {
            InitializeComponent();
            cboTipoCaja.Items.AddRange(TiposCaja.Todos);
            cboTipoCaja.SelectedItem = "Otros";
            ConfigurarDistribucion();
            CargarMedios();
        }

        // Carga los medios activos en la grilla.
        private void CargarMedios()
        {
            dgvMediosPagos.DataSource = null;
            dgvMediosPagos.DataSource = cnMedioPago.ObtenerTodos();
            dgvMediosPagos.Columns[nameof(MedioPago.IdMedioPago)].Visible = false;
            dgvMediosPagos.Columns[nameof(MedioPago.Activo)].Visible = false;
            dgvMediosPagos.Columns[nameof(MedioPago.TipoCaja)].HeaderText = "Tipo en caja";
            dgvMediosPagos.Columns[nameof(MedioPago.NombreMedioPago)].HeaderText = "Medio de pago";
            dgvMediosPagos.Columns[nameof(MedioPago.RecargoMedioPago)].HeaderText = "Recargo general %";
            dgvMediosPagos.Columns[nameof(MedioPago.Recargo3)].HeaderText = "3 cuotas %";
            dgvMediosPagos.Columns[nameof(MedioPago.Recargo6)].HeaderText = "6 cuotas %";
            dgvMediosPagos.Columns[nameof(MedioPago.Recargo9)].HeaderText = "9 cuotas %";
            dgvMediosPagos.Columns[nameof(MedioPago.Recargo12)].HeaderText = "12 cuotas %";
        }

        // Guarda un medio nuevo o modifica el seleccionado.
        private void btnGuardar_Click(object? sender, EventArgs e)
        {
            try
            {
                MedioPago medio = ObtenerMedioDesdeFormulario();
                if (idMedioPagoSeleccionado == 0) cnMedioPago.Crear(medio); else { medio.IdMedioPago = idMedioPagoSeleccionado; cnMedioPago.Modificar(medio); }
                MessageBox.Show("Medio de pago guardado correctamente.", "Medios de pago", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimpiarFormulario();
                CargarMedios();
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo guardar el medio de pago: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Desactiva el medio seleccionado sin borrar pagos históricos.
        private void btnDesactivar_Click(object? sender, EventArgs e)
        {
            if (idMedioPagoSeleccionado == 0) return;
            cnMedioPago.Desactivar(idMedioPagoSeleccionado);
            MessageBox.Show("Medio de pago desactivado correctamente.", "Medios de pago", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LimpiarFormulario();
            CargarMedios();
        }

        // Carga en el formulario el medio seleccionado.
        private void dgvMediosPagos_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            MedioPago medio = (MedioPago)dgvMediosPagos.Rows[e.RowIndex].DataBoundItem;
            idMedioPagoSeleccionado = medio.IdMedioPago;
            cboTipoCaja.SelectedItem = medio.TipoCaja;
            txtNombreMedioPago.Text = medio.NombreMedioPago;
            nudRecargoGeneral.Value = medio.RecargoMedioPago;
            nudRecargo3.Value = medio.Recargo3;
            nudRecargo6.Value = medio.Recargo6;
            nudRecargo9.Value = medio.Recargo9;
            nudRecargo12.Value = medio.Recargo12;
            btnDesactivar.Enabled = true;
            btnNuevo.Text = "Cancelar edición";
        }

        // Convierte los controles del formulario en una entidad.
        private MedioPago ObtenerMedioDesdeFormulario()
        {
            return new MedioPago { TipoCaja = cboTipoCaja.Text, NombreMedioPago = txtNombreMedioPago.Text.Trim(), RecargoMedioPago = nudRecargoGeneral.Value, Recargo3 = nudRecargo3.Value, Recargo6 = nudRecargo6.Value, Recargo9 = nudRecargo9.Value, Recargo12 = nudRecargo12.Value, Activo = true };
        }

        // Limpia el formulario y prepara una nueva alta.
        private void LimpiarFormulario()
        {
            idMedioPagoSeleccionado = 0;
            cboTipoCaja.SelectedItem = "Otros";
            txtNombreMedioPago.Clear();
            nudRecargoGeneral.Value = 0;
            nudRecargo3.Value = 0;
            nudRecargo6.Value = 0;
            nudRecargo9.Value = 0;
            nudRecargo12.Value = 0;
            btnDesactivar.Enabled = false;
            btnNuevo.Text = "Nuevo";
            txtNombreMedioPago.Focus();
        }

        // Limpia los campos sin guardar cambios.
        private void btnNuevo_Click(object? sender, EventArgs e)
        {
            LimpiarFormulario();
        }
    }
}
