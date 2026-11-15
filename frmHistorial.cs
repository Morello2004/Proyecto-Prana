using Prana.negocio;
using System;
using System.Windows.Forms;

namespace Prana.Presentacion
{
    // agrupa los datos y operaciones de frm historial
    public partial class frmHistorial : Form
    {
        // accede al historial de actividades registrado por el sistema
        private readonly CN_LogActividad cnLogActividad = new CN_LogActividad();
        // prepara frm historial antes de que se utilice la instancia
        public frmHistorial()
        {
            InitializeComponent();
            pnlFiltros.Height = 175;
        }

        // configura y carga la grilla del historial
        private void ConfigurarHistorial()
        {
            // carga las ultimas diez actividades
            CargarActividades(DateTime.MinValue, DateTime.MaxValue, 10);
            // oculta identificadores tecnicos que no necesita ver el usuario dgvactividadescolumns[nameof(pranaentidadeslogactividadidlog)]visible: determina si el control ocupa una zona visible de la pantalla
            dgvActividades.Columns[nameof(Prana.Entidades.LogActividad.IdLog)].Visible = false;
            // dgvactividadescolumns[nameof(pranaentidadeslogactividadidusuario)]visible: determina si el control ocupa una zona visible de la pantalla
            dgvActividades.Columns[nameof(Prana.Entidades.LogActividad.IdUsuario)].Visible = false;
            // dgvactividadescolumns[nameof(pranaentidadeslogactividadidentidad)]visible: determina si el control ocupa una zona visible de la pantalla
            dgvActividades.Columns[nameof(Prana.Entidades.LogActividad.IdEntidad)].Visible = false;
            // ajusta los titulos y anchos de las columnas visibles dgvactividadescolumns[nameof(pranaentidadeslogactividadnombreusuario)]headertext: titulo de la columna; no es el nombre de la propiedad enlazada
            dgvActividades.Columns[nameof(Prana.Entidades.LogActividad.NombreUsuario)].HeaderText = "Usuario";
            dgvActividades.Columns[nameof(Prana.Entidades.LogActividad.NombreUsuario)].Width = 110;
            // dgvactividadescolumns[nameof(pranaentidadeslogactividadaccion)]headertext: titulo de la columna; no es el nombre de la propiedad enlazada
            dgvActividades.Columns[nameof(Prana.Entidades.LogActividad.Accion)].HeaderText = "Acción";
            dgvActividades.Columns[nameof(Prana.Entidades.LogActividad.Accion)].Width = 175;
            // dgvactividadescolumns[nameof(pranaentidadeslogactividadentidad)]headertext: titulo de la columna; no es el nombre de la propiedad enlazada
            dgvActividades.Columns[nameof(Prana.Entidades.LogActividad.Entidad)].HeaderText = "Entidad";
            dgvActividades.Columns[nameof(Prana.Entidades.LogActividad.Entidad)].Width = 120;
            // dgvactividadescolumns[nameof(pranaentidadeslogactividaddetalle)]headertext: titulo de la columna; no es el nombre de la propiedad enlazada
            dgvActividades.Columns[nameof(Prana.Entidades.LogActividad.Detalle)].HeaderText = "Detalle";
            dgvActividades.Columns[nameof(Prana.Entidades.LogActividad.Detalle)].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            // dgvactividadescolumns[nameof(pranaentidadeslogactividadfecha)]headertext: titulo de la columna; no es el nombre de la propiedad enlazada
            dgvActividades.Columns[nameof(Prana.Entidades.LogActividad.Fecha)].HeaderText = "Fecha";
            dgvActividades.Columns[nameof(Prana.Entidades.LogActividad.Fecha)].Width = 150;
            // dgvactividadescolumns[nameof(pranaentidadeslogactividadfecha)]defaultcellstyleformat: formato de presentacion, por ejemplo moneda o fecha; no modifica el valor original
            dgvActividades.Columns[nameof(Prana.Entidades.LogActividad.Fecha)].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
        }

        // carga actividades segun las fechas y el limite indicado
        private void CargarActividades(DateTime desde, DateTime hasta, int limite)
        {
            // actualiza la grilla con los registros filtrados
            int? remito = null;
            // distingue un dato vacio de un valor ingresado antes de continuar
            if (!string.IsNullOrWhiteSpace(txtRemito.Text))
            {
                if (!int.TryParse(txtRemito.Text.Trim(), out int numero) || numero <= 0)
                {
                    MessageBox.Show("Ingrese un numero de remito valido.", "Historial");
                    return;
                }

                remito = numero;
            }

            try
            {
                dgvActividades.DataSource = cnLogActividad.ObtenerFiltrado(desde, hasta, limite, remito, txtAccion.Text, txtUsuario.Text);
            }
            // atiende el error de esta operacion antes de informar o propagar el problema
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "Historial");
            }
        }

        // filtra las actividades por el rango seleccionado atiende la pulsacion de btnfiltrar y ejecuta la accion asociada el evento no significa que la operacion ya se haya guardado
        private void btnFiltrar_Click(object? sender, EventArgs e)
        {
            // valida que el rango sea correcto
            if (dtpDesde.Value.Date > dtpHasta.Value.Date)
            {
                MessageBox.Show("La fecha Desde no puede ser posterior a la fecha Hasta.", "Rango inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // incluye todo el dia seleccionado como fecha final
            CargarActividades(dtpDesde.Value.Date, dtpHasta.Value.Date.AddDays(1), 100);
        }

        // muestra nuevamente las ultimas diez actividades atiende la pulsacion de btnultimas y ejecuta la accion asociada el evento no significa que la operacion ya se haya guardado
        private void btnUltimas_Click(object? sender, EventArgs e)
        {
            // carga solo las diez actividades mas recientes
            CargarActividades(DateTime.MinValue, DateTime.MaxValue, 10);
        }

        // interpreta la tecla pulsada y ejecuta la accion prevista por el formulario
        private void Filtro_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                btnFiltrar_Click(this, EventArgs.Empty);
            }
        }

        // prepara la informacion inicial cuando se carga la ventana
        private void frmHistorial_Load(object? sender, EventArgs e) => ConfigurarHistorial();
    }
}
