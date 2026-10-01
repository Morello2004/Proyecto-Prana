using System;
using System.Windows.Forms;
using Prana.negocio;

namespace Prana.Presentacion
{
    public partial class Interfaz : Form
    {
        // Variable que va a guardar la referencia al formulario mostrado actualmente
        // Al principio vale null porque todavía no se abrió ninguna sección
        private Form? formularioActivo;

        public Interfaz()
        {
            InitializeComponent();
            btnAlerta.Visible = Permisos.TienePermiso(Permisos.ConsultarAlertas);
            btnHistorial.Visible = Permisos.TienePermiso(Permisos.ConsultarHistorial);
            btnInformes.Visible = Permisos.TienePermiso(Permisos.ConsultarInformes);
            btnUsuarios.Visible = Permisos.TienePermiso(Permisos.GestionarUsuarios);
            btnMediosPagos.Visible = Permisos.TienePermiso(Permisos.ModificarMediosPago);
            ConfigurarMenuCompacto();
        }

        private void ConfigurarMenuCompacto()
        {
            WindowState = FormWindowState.Maximized;
            pnlContenido.BackColor = EstiloGestion.Fondo;
            pnlMenuLateral.Width = 168;
            panel2.Visible = false;
            panel1.SetBounds(22, 8, 124, 48);
            pnlMenuLateral.AutoScroll = true;
            int y = 64;
            foreach (var boton in new[] { btnPanelPrincipal, btnNuevaVenta, btnClientes,
                btnStock, btnPagos, btnMediosPagos, btnEntregas, btnInformes, btnAlerta, btnHistorial, btnUsuarios })
            {
                // Visible depende también del formulario padre, aún sin mostrar.
                bool permitido = boton != btnUsuarios || Permisos.TienePermiso(Permisos.GestionarUsuarios);
                permitido &= boton != btnMediosPagos || Permisos.TienePermiso(Permisos.ModificarMediosPago);
                permitido &= boton != btnAlerta || Permisos.TienePermiso(Permisos.ConsultarAlertas);
                permitido &= boton != btnInformes || Permisos.TienePermiso(Permisos.ConsultarInformes);
                permitido &= boton != btnHistorial || Permisos.TienePermiso(Permisos.ConsultarHistorial);
                boton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
                boton.SetBounds(6, y, 156, 36);
                if (permitido) y += 40;
            }
        }

        // Abre un formulario dentro del panel derecho
        public void AbrirFormulario(Form formulario)
        {
            // Cierra la sección anterior, si existe
            formularioActivo?.Close();

            // Guarda la nueva sección como activa
            formularioActivo = formulario;

            // Permite insertar el formulario dentro del panel
            formulario.TopLevel = false;

            // Elimina los bordes y la barra superior
            formulario.FormBorderStyle = FormBorderStyle.None;

            // Hace que ocupe todo el espacio disponible
            formulario.Dock = DockStyle.Fill;

            // Elimina la sección mostrada anteriormente
            pnlContenido.Controls.Clear();

            // Agrega la nueva sección al panel derecho
            pnlContenido.Controls.Add(formulario);

            // Muestra la nueva sección
            formulario.Show();
        }

        private void Interfaz_Load(object sender, EventArgs e)
        {
            // Muestra el panel principal al abrir la interfaz
            AbrirFormulario(new frmPanelPrincipal());
        }

        private void iconButton2_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new frmNuevaVenta());
        }

        private void menuStrip1_ItemClicked(
            object sender,
            ToolStripItemClickedEventArgs e)
        {
        }

        private void menuStrip2_ItemClicked(
            object sender,
            ToolStripItemClickedEventArgs e)
        {
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void iconMenuItem1_Click(object sender, EventArgs e)
        {
        }

        private void iconMenuItem12_Click(object sender, EventArgs e)
        {
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void iconButton1_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new frmPanelPrincipal());
        }

        private void btnClientes_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new frmClientes());
        }

        private void btnStock_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new frmStock());
        }

        private void btnPagos_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new frmPagos());
        }

        private void btnMediosPagos_Click(object sender, EventArgs e)
        {
            // Detiene el evento si el usuario no puede modificar medios de pago.
            if (!Permisos.TienePermiso(Permisos.ModificarMediosPago)) return;

            // Abre el ABM de medios de pago.
            AbrirFormulario(new frmMediosPagos());
        }

        private void btnEntregas_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new frmEntregas());
        }

        private void btnAlerta_Click(object sender, EventArgs e)
        {
            // Comprueba si el usuario puede consultar el historial.
            if (!Permisos.TienePermiso(Permisos.ConsultarAlertas))
            {
                // Informa que el usuario no tiene autorización.
                MessageBox.Show("No tiene permisos para consultar alertas.", "Acceso denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Abre el historial de actividades.
            AbrirFormulario(new frmAlertas());
        }

        private void btnInformes_Click(object sender, EventArgs e)
        {
            if (Permisos.TienePermiso(Permisos.ConsultarInformes)) AbrirFormulario(new frmInformesVentas());
        }

        private void btnHistorial_Click(object sender, EventArgs e)
        {
            if (Permisos.TienePermiso(Permisos.ConsultarHistorial)) AbrirFormulario(new frmHistorial());
        }

        private void btnUsuarios_Click(object sender, EventArgs e)
        {
            // Detiene el evento si el usuario no puede gestionar usuarios.
            if (!Permisos.TienePermiso(Permisos.GestionarUsuarios))
            {
                // Detiene la apertura del formulario.
                return;
            }

            // Abre el formulario de gestión de usuarios.
            AbrirFormulario(new frmUsuarios());
        }

        private void pnlContenido_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
