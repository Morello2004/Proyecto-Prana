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
            btnAlerta.Visible = Permisos.TienePermiso(Permisos.ConsultarHistorial);
            btnUsuarios.Visible = Permisos.TienePermiso(Permisos.GestionarUsuarios);
            btnMediosPagos.Visible = Permisos.TienePermiso(Permisos.ModificarMediosPago);
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
            if (!Permisos.TienePermiso(Permisos.ConsultarHistorial))
            {
                // Informa que el usuario no tiene autorización.
                MessageBox.Show("No tiene permisos para consultar el historial.", "Acceso denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Abre el historial de actividades.
            AbrirFormulario(new frmHistorial());
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
