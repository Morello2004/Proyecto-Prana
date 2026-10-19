using Prana.negocio;
using Prana.Entidades;
using System;
using System.Drawing;
using System.Windows.Forms;
namespace Prana.Presentacion
{
    public partial class frmUsuarios : Form
    {
        // Coordina la administracion de usuarios y desbloqueos
        private readonly CN_Usuario cnUsuario = new CN_Usuario();
        private int idSeleccionado;

        // Inicializa los controles de administracion
        public frmUsuarios()
        {
            InitializeComponent();
            cmbRol.SelectedIndex = 1;
            txtNombreUsuario.MaxLength = 100;
            AutoScrollMinSize = new Size(560, 590);

        }
        // Carga los usuarios y muestra errores de consulta
        private void frmUsuarios_Load(object? sender, EventArgs e)
        {
            try { Cargar(); }
            catch (Exception ex) { MessageBox.Show("No se pudieron cargar los usuarios: " + ex.Message, "Usuarios", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
        // Carga usuarios y prepara la opcion de crear una cuenta
        private void Cargar()
        {
            var lista = cnUsuario.ListarUsuarios();
            lista.Insert(0, new usuario { nombreUsuario = "Crear un usuario nuevo" });
            selector.DataSource = lista;
            selector.SelectedIndex = 0;
            Seleccionar(null, EventArgs.Empty);
        }
        // Actualiza los campos y el estado de bloqueo de la cuenta seleccionada
        private void Seleccionar(object? sender, EventArgs e)
        {
            if (selector.SelectedItem is not usuario u) return;
            idSeleccionado = u.idUsuario;
            txtNombreUsuario.Text = idSeleccionado == 0 ? "" : u.nombreUsuario;
            txtContrasena.Clear();
            cmbRol.SelectedIndex = idSeleccionado == 0 ? 1 : u.IdRolUsuario - 1;
            lblSeccion.Text = idSeleccionado == 0 ? "Crear usuario" : "Modificar usuario";
            lblContrasena.Text = idSeleccionado == 0 ? "Contraseña" : "Nueva contraseña (vacía conserva la actual)";
            btnGuardar.Text = idSeleccionado == 0 ? "Crear usuario" : "Guardar cambios";
            btnDesbloquear.Enabled = idSeleccionado != 0 && u.EstaBloqueado;
            lblEstadoBloqueo.Text = u.EstaBloqueado && u.BloqueadoHasta.HasValue
                ? "Bloqueado hasta " + u.BloqueadoHasta.Value.ToString("dd/MM/yyyy HH:mm")
                : string.Empty;
        }

        // Guarda los datos del usuario seleccionado o crea una cuenta nueva
        private void btnGuardar_Click(object? sender, EventArgs e)
        {
            try
            {
                bool editando = idSeleccionado != 0;
                if (editando) cnUsuario.ModificarUsuario(idSeleccionado, txtNombreUsuario.Text, txtContrasena.Text, cmbRol.SelectedIndex + 1);
                else cnUsuario.CrearUsuario(txtNombreUsuario.Text.Trim(), txtContrasena.Text, cmbRol.SelectedIndex + 1);
                txtContrasena.Clear();
                MessageBox.Show(editando ? "Usuario actualizado correctamente." : "Usuario creado correctamente.", "Usuarios", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Cargar();
            }
            catch (Exception ex) { MessageBox.Show("No se pudo guardar el usuario: " + ex.Message, "Usuarios", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        // Desbloquea la cuenta seleccionada usando la sesion de administrador activa
        private void btnDesbloquear_Click(object? sender, EventArgs e)
        {
            if (idSeleccionado == 0) return;
            if (MessageBox.Show(this, "Se restableceran los intentos y el usuario podra volver a ingresar. Continuar?",
                "Desbloquear usuario", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            try
            {
                cnUsuario.DesbloquearUsuario(idSeleccionado);
                MessageBox.Show(this, "La cuenta fue desbloqueada correctamente.", "Usuarios", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Cargar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "No se pudo desbloquear la cuenta: " + ex.Message, "Usuarios", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Conserva el evento vacio conectado al disenador
        private void label1_Click(object sender, EventArgs e) { }

        // Selecciona la opcion para crear un usuario nuevo
        private void nuevo_Click(object? sender, EventArgs e) => selector.SelectedIndex = 0;

    }
}
