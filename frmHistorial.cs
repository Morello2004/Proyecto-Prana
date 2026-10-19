using Prana.negocio;
using System;
using System.Windows.Forms;

namespace Prana.Presentacion
{
    public partial class frmHistorial : Form
    {
        // Accede al historial de actividades registrado por el sistema.
        private readonly CN_LogActividad cnLogActividad = new CN_LogActividad();

        public frmHistorial()
        {
            InitializeComponent();
            pnlFiltros.Height=175;

        }

        // Configura y carga la grilla del historial.
        private void ConfigurarHistorial()
        {
            // Carga las últimas diez actividades.
            CargarActividades(DateTime.MinValue, DateTime.MaxValue, 10);

            // Oculta identificadores técnicos que no necesita ver el usuario.
            dgvActividades.Columns[nameof(Prana.Entidades.LogActividad.IdLog)].Visible = false;
            dgvActividades.Columns[nameof(Prana.Entidades.LogActividad.IdUsuario)].Visible = false;
            dgvActividades.Columns[nameof(Prana.Entidades.LogActividad.IdEntidad)].Visible = false;

            // Ajusta los títulos y anchos de las columnas visibles.
            dgvActividades.Columns[nameof(Prana.Entidades.LogActividad.NombreUsuario)].HeaderText = "Usuario";
            dgvActividades.Columns[nameof(Prana.Entidades.LogActividad.NombreUsuario)].Width = 110;
            dgvActividades.Columns[nameof(Prana.Entidades.LogActividad.Accion)].HeaderText = "Acción";
            dgvActividades.Columns[nameof(Prana.Entidades.LogActividad.Accion)].Width = 175;
            dgvActividades.Columns[nameof(Prana.Entidades.LogActividad.Entidad)].HeaderText = "Entidad";
            dgvActividades.Columns[nameof(Prana.Entidades.LogActividad.Entidad)].Width = 120;
            dgvActividades.Columns[nameof(Prana.Entidades.LogActividad.Detalle)].HeaderText = "Detalle";
            dgvActividades.Columns[nameof(Prana.Entidades.LogActividad.Detalle)].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvActividades.Columns[nameof(Prana.Entidades.LogActividad.Fecha)].HeaderText = "Fecha";
            dgvActividades.Columns[nameof(Prana.Entidades.LogActividad.Fecha)].Width = 150;
            dgvActividades.Columns[nameof(Prana.Entidades.LogActividad.Fecha)].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";

        }

        // Carga actividades según las fechas y el límite indicado.
        private void CargarActividades(DateTime desde, DateTime hasta, int limite)
        {
            // Actualiza la grilla con los registros filtrados.
            int? remito=null;
            if(!string.IsNullOrWhiteSpace(txtRemito.Text))
            {
                if(!int.TryParse(txtRemito.Text.Trim(),out int numero) || numero<=0)
                { MessageBox.Show("Ingrese un numero de remito valido.","Historial");return; }
                remito=numero;
            }
            try { dgvActividades.DataSource = cnLogActividad.ObtenerFiltrado(desde, hasta, limite,remito,txtAccion.Text,txtUsuario.Text); }
            catch(Exception ex) { MessageBox.Show(ex.Message,"Historial"); }
        }

        // Filtra las actividades por el rango seleccionado.
        private void btnFiltrar_Click(object? sender, EventArgs e)
        {
            // Valida que el rango sea correcto.
            if (dtpDesde.Value.Date > dtpHasta.Value.Date)
            {
                MessageBox.Show("La fecha Desde no puede ser posterior a la fecha Hasta.", "Rango inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Incluye todo el día seleccionado como fecha final.
            CargarActividades(dtpDesde.Value.Date, dtpHasta.Value.Date.AddDays(1), 100);
        }

        // Muestra nuevamente las últimas diez actividades.
        private void btnUltimas_Click(object? sender, EventArgs e)
        {
            // Carga solo las diez actividades más recientes.
            CargarActividades(DateTime.MinValue, DateTime.MaxValue, 10);
        }

        private void Filtro_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; btnFiltrar_Click(this, EventArgs.Empty); }
        }

        private void frmHistorial_Load(object? sender, EventArgs e) => ConfigurarHistorial();

    }
}
