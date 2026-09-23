using Prana.negocio;
using System;
using System.Windows.Forms;

namespace Prana.Presentacion
{
    // Permite crear usuarios nuevos desde la aplicación.
    public partial class frmUsuarios : Form
    {
        // Servicio de usuarios de la capa de negocio.
        private readonly CN_Usuario cnUsuario = new CN_Usuario();
        // Ejecuta acciones cuando se carga el formulario.
        private void frmUsuarios_Load(object? sender, EventArgs e)
        {
        }

        // Inicializa el formulario y sus controles.
        public frmUsuarios()
        {
            InitializeComponent();
            cmbRol.SelectedIndex = 1;
        }

        // Valida los datos y crea el usuario.
        private void btnGuardar_Click(object? sender, EventArgs e)
        {
            try
            {
                // Obtiene el rol seleccionado: 1 Administrador o 2 Vendedor.
                int idRolUsuario = cmbRol.SelectedIndex + 1;

                // Crea el usuario y genera su hash BCrypt.
                cnUsuario.CrearUsuario(txtNombreUsuario.Text.Trim(), txtContrasena.Text, idRolUsuario);

                // Informa que el usuario fue creado correctamente.
                MessageBox.Show("Usuario creado correctamente.", "Usuarios", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Limpia los campos para crear otro usuario.
                txtNombreUsuario.Clear();
                txtContrasena.Clear();
                cmbRol.SelectedIndex = 1;
            }
            catch (Exception ex)
            {
                // Muestra el error ocurrido durante la creación.
                MessageBox.Show("No se pudo crear el usuario: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Conserva el evento existente del diseñador.
        private void label1_Click(object sender, EventArgs e)
        {
        }
    }
}