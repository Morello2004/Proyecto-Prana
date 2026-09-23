using Prana.negocio;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Prana.Presentacion
{
    public partial class frmAlertas : Form
    {
        // Accede al historial de actividades registrado por el sistema.
        private readonly CN_LogActividad cnLogActividad = new CN_LogActividad();

        // Muestra las actividades registradas.
        private readonly DataGridView dgvActividades = new DataGridView();

        // Permite seleccionar el inicio del período.
        private readonly DateTimePicker dtpDesde = new DateTimePicker();

        // Permite seleccionar el final del período.
        private readonly DateTimePicker dtpHasta = new DateTimePicker();

        // Contiene los controles de filtro.
        private readonly Panel pnlFiltros = new Panel();

        // Permite aplicar el rango de fechas.
        private readonly Button btnFiltrar = new Button();

        // Permite volver a mostrar las últimas diez actividades.
        private readonly Button btnUltimas = new Button();

        // Permite ocultar o mostrar el historial.
        private readonly Button btnOcultar = new Button();

        public frmAlertas()
        {
            Text = "Alertas";
            Controls.Add(new Label { Text = "Alertas", AutoSize = true, Location = new Point(20, 20) });
        }

        // Configura y carga la grilla del historial.
        private void ConfigurarHistorial()
        {
            // Configura la grilla para ocupar el formulario.
            dgvActividades.Dock = DockStyle.Fill;
            dgvActividades.AutoGenerateColumns = true;
            dgvActividades.ReadOnly = true;
            dgvActividades.AllowUserToAddRows = false;
            dgvActividades.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            dgvActividades.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvActividades.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvActividades.MultiSelect = false;
            dgvActividades.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

            // Configura el panel superior de filtros.
            pnlFiltros.Dock = DockStyle.Top;
            pnlFiltros.Height = 42;
            pnlFiltros.Padding = new Padding(8, 8, 8, 4);

            // Configura el selector de fecha inicial.
            dtpDesde.Format = DateTimePickerFormat.Short;
            dtpDesde.Value = DateTime.Today.AddMonths(-1);

            // Configura el selector de fecha final.
            dtpHasta.Format = DateTimePickerFormat.Short;
            dtpHasta.Value = DateTime.Today;

            // Configura el botón para filtrar por fechas.
            btnFiltrar.Text = "Filtrar";
            btnFiltrar.AutoSize = true;
            btnFiltrar.Click += btnFiltrar_Click;

            // Configura el botón para mostrar las últimas diez actividades.
            btnUltimas.Text = "Últimas 10";
            btnUltimas.AutoSize = true;
            btnUltimas.Click += btnUltimas_Click;

            // Configura el botón para ocultar o mostrar el historial.
            btnOcultar.Text = "Ocultar historial";
            btnOcultar.AutoSize = true;
            btnOcultar.Click += btnOcultar_Click;

            // Agrega los controles al panel de filtros.
            pnlFiltros.Controls.Add(new Label { Text = "Desde", AutoSize = true, Location = new Point(8, 12) });
            dtpDesde.Location = new Point(52, 8);
            pnlFiltros.Controls.Add(dtpDesde);
            pnlFiltros.Controls.Add(new Label { Text = "Hasta", AutoSize = true, Location = new Point(160, 12) });
            dtpHasta.Location = new Point(204, 8);
            pnlFiltros.Controls.Add(dtpHasta);
            btnFiltrar.Location = new Point(312, 7);
            pnlFiltros.Controls.Add(btnFiltrar);
            btnUltimas.Location = new Point(390, 7);
            pnlFiltros.Controls.Add(btnUltimas);
            btnOcultar.Location = new Point(490, 7);
            pnlFiltros.Controls.Add(btnOcultar);

            // Agrega el historial y los filtros al formulario.
            Controls.Add(dgvActividades);
            Controls.Add(pnlFiltros);
            dgvActividades.BringToFront();

            // Carga las últimas diez actividades.
            CargarActividades(DateTime.MinValue, DateTime.MaxValue, 10);

            // Oculta identificadores técnicos que no necesita ver el usuario.
            dgvActividades.Columns[nameof(Prana.Entidades.LogActividad.IdLog)].Visible = false;
            dgvActividades.Columns[nameof(Prana.Entidades.LogActividad.IdUsuario)].Visible = false;
            dgvActividades.Columns[nameof(Prana.Entidades.LogActividad.IdEntidad)].Visible = false;

            // Ajusta los títulos y anchos de las columnas visibles.
            dgvActividades.Columns[nameof(Prana.Entidades.LogActividad.NombreUsuario)].HeaderText = "Usuario";
            dgvActividades.Columns[nameof(Prana.Entidades.LogActividad.NombreUsuario)].Width = 140;
            dgvActividades.Columns[nameof(Prana.Entidades.LogActividad.Accion)].HeaderText = "Acción";
            dgvActividades.Columns[nameof(Prana.Entidades.LogActividad.Accion)].Width = 190;
            dgvActividades.Columns[nameof(Prana.Entidades.LogActividad.Entidad)].HeaderText = "Entidad";
            dgvActividades.Columns[nameof(Prana.Entidades.LogActividad.Entidad)].Width = 150;
            dgvActividades.Columns[nameof(Prana.Entidades.LogActividad.Detalle)].HeaderText = "Detalle";
            dgvActividades.Columns[nameof(Prana.Entidades.LogActividad.Detalle)].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvActividades.Columns[nameof(Prana.Entidades.LogActividad.Fecha)].HeaderText = "Fecha";
            dgvActividades.Columns[nameof(Prana.Entidades.LogActividad.Fecha)].Width = 150;

        }

        // Carga actividades según las fechas y el límite indicado.
        private void CargarActividades(DateTime desde, DateTime hasta, int limite)
        {
            // Actualiza la grilla con los registros filtrados.
            dgvActividades.DataSource = cnLogActividad.ObtenerFiltrado(desde, hasta, limite);
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

        // Oculta o muestra la grilla del historial.
        private void btnOcultar_Click(object? sender, EventArgs e)
        {
            // Invierte la visibilidad del historial.
            dgvActividades.Visible = !dgvActividades.Visible;
            btnOcultar.Text = dgvActividades.Visible ? "Ocultar historial" : "Mostrar historial";
        }
    }
}
