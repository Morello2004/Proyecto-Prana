using Prana.Entidades;
using Prana.negocio;
using System;
using System.Windows.Forms;

namespace Prana.Presentacion
{
    // permite editar los recargos de los medios de pago fijos
    public partial class frmMediosPagos : Form
    {
        // coordina la consulta y edicion de recargos
        private readonly CN_MedioPago cnMedioPago = new CN_MedioPago();
        // conserva el medio seleccionado para modificar sus recargos
        private int idMedioPagoSeleccionado;
        // conserva el tipo original para validar los recargos sin mostrar un campo redundante al usuario
        private string tipoCajaSeleccionado = string.Empty;
        // inicializa el formulario y bloquea los datos que no se pueden editar
        public frmMediosPagos()
        {
            InitializeComponent();
            txtNombreMedioPago.ReadOnly = true;
            btnGuardar.Enabled = false;
            ConfigurarCuotas(false);
            lblSeccion.Text = "Selecciona un medio de pago para editar sus recargos";
        }

        // consulta los medios de pago fijos al abrir la pantalla
        private void frmMediosPagos_Load(object? sender, EventArgs e)
        {
            CargarMedios();
        }

        // carga los cuatro medios de pago fijos en la grilla
        private void CargarMedios()
        {
            dgvMediosPagos.DataSource = null;
            dgvMediosPagos.DataSource = cnMedioPago.ObtenerTodos();
            // dgvmediospagoscolumns[nameof(mediopagoidmediopago)]visible: determina si el control ocupa una zona visible de la pantalla
            dgvMediosPagos.Columns[nameof(MedioPago.IdMedioPago)].Visible = false;
            // dgvmediospagoscolumns[nameof(mediopagoactivo)]visible: determina si el control ocupa una zona visible de la pantalla
            dgvMediosPagos.Columns[nameof(MedioPago.Activo)].Visible = false;
            dgvMediosPagos.Columns[nameof(MedioPago.TipoCaja)].Visible = false;
            // dgvmediospagoscolumns[nameof(mediopagonombremediopago)]headertext: titulo de la columna; no es el nombre de la propiedad enlazada
            dgvMediosPagos.Columns[nameof(MedioPago.NombreMedioPago)].HeaderText = "Medio de pago";
            // dgvmediospagoscolumns[nameof(mediopagorecargomediopago)]headertext: titulo de la columna; no es el nombre de la propiedad enlazada
            dgvMediosPagos.Columns[nameof(MedioPago.RecargoMedioPago)].HeaderText = "Recargo general %";
            // dgvmediospagoscolumns[nameof(mediopagorecargo2)]headertext: titulo de la columna; no es el nombre de la propiedad enlazada
            dgvMediosPagos.Columns[nameof(MedioPago.Recargo2)].HeaderText = "2 cuotas %";
            // dgvmediospagoscolumns[nameof(mediopagorecargo3)]headertext: titulo de la columna; no es el nombre de la propiedad enlazada
            dgvMediosPagos.Columns[nameof(MedioPago.Recargo3)].HeaderText = "3 cuotas %";
            // dgvmediospagoscolumns[nameof(mediopagorecargo6)]headertext: titulo de la columna; no es el nombre de la propiedad enlazada
            dgvMediosPagos.Columns[nameof(MedioPago.Recargo6)].HeaderText = "6 cuotas %";
            // dgvmediospagoscolumns[nameof(mediopagorecargo9)]headertext: titulo de la columna; no es el nombre de la propiedad enlazada
            dgvMediosPagos.Columns[nameof(MedioPago.Recargo9)].HeaderText = "9 cuotas %";
            // dgvmediospagoscolumns[nameof(mediopagorecargo12)]headertext: titulo de la columna; no es el nombre de la propiedad enlazada
            dgvMediosPagos.Columns[nameof(MedioPago.Recargo12)].HeaderText = "12 cuotas %";
            dgvMediosPagos.ClearSelection();
            LimpiarSeleccion();
        }

        // guarda los recargos del medio seleccionado atiende la pulsacion de btnguardar y ejecuta la accion asociada el evento no significa que la operacion ya se haya guardado
        private void btnGuardar_Click(object? sender, EventArgs e)
        {
            try
            {
                if (idMedioPagoSeleccionado == 0)
                {
                    // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                    throw new InvalidOperationException("Seleccione un medio de pago para editar sus recargos.");
                }

                MedioPago medio = ObtenerMedioDesdeFormulario();
                medio.IdMedioPago = idMedioPagoSeleccionado;
                cnMedioPago.Modificar(medio);
                MessageBox.Show("Recargos guardados correctamente.", "Medios de pago", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarMedios();
            }
            // atiende el error de esta operacion antes de informar o propagar el problema
            catch (Exception error)
            {
                MessageBox.Show("No se pudieron guardar los recargos: " + error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // carga en el formulario el medio seleccionado atiende el evento cellclick del control o formulario indicado por el nombre del metodo
        private void dgvMediosPagos_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            // comprueba que el clic corresponda a una fila de datos y no al encabezado
            if (e.RowIndex < 0 || dgvMediosPagos.Rows[e.RowIndex].DataBoundItem is not MedioPago medio)
            {
                return;
            }

            idMedioPagoSeleccionado = medio.IdMedioPago;
            tipoCajaSeleccionado = medio.TipoCaja;
            txtNombreMedioPago.Text = medio.NombreMedioPago;
            nudRecargoGeneral.Value = medio.RecargoMedioPago;
            bool permiteCuotas = medio.TipoCaja == "Crédito";
            ConfigurarCuotas(permiteCuotas);
            if (permiteCuotas)
            {
                nudRecargo2.Value = medio.Recargo2;
            }
            else
            {
                nudRecargo2.Value = 0;
            }
            if (permiteCuotas)
            {
                nudRecargo3.Value = medio.Recargo3;
            }
            else
            {
                nudRecargo3.Value = 0;
            }
            if (permiteCuotas)
            {
                nudRecargo6.Value = medio.Recargo6;
            }
            else
            {
                nudRecargo6.Value = 0;
            }
            if (permiteCuotas)
            {
                nudRecargo9.Value = medio.Recargo9;
            }
            else
            {
                nudRecargo9.Value = 0;
            }
            if (permiteCuotas)
            {
                nudRecargo12.Value = medio.Recargo12;
            }
            else
            {
                nudRecargo12.Value = 0;
            }
            btnGuardar.Enabled = true;
        }

        // convierte los recargos editables en una entidad
        private MedioPago ObtenerMedioDesdeFormulario()
        {
            return new MedioPago
            {
                TipoCaja = tipoCajaSeleccionado,
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

        // habilita las cuotas unicamente para el medio de credito
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

        // limpia la seleccion y bloquea la edicion hasta elegir un medio
        private void LimpiarSeleccion()
        {
            idMedioPagoSeleccionado = 0;
            tipoCajaSeleccionado = string.Empty;
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
