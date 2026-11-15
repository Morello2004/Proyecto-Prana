using Prana.negocio;
using Prana.Entidades;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Prana.Presentacion
{
    // agrupa los datos y operaciones de frm usuarios
    public partial class frmUsuarios : Form
    {
        // coordina la administracion de usuarios y acciones de cuenta
        private readonly CN_Usuario cnUsuario = new CN_Usuario();
        // conserva id seleccionado entre las operaciones de esta clase
        private int idSeleccionado;
        // inicializa los controles de administracion
        public frmUsuarios()
        {
            InitializeComponent();
            cmbRol.SelectedIndex = 1;
            txtNombreUsuario.MaxLength = 20;
            AutoScrollMinSize = new Size(560, 590);
        }

        // carga los usuarios y muestra errores de consulta
        private void frmUsuarios_Load(object? sender, EventArgs e)
        {
            try
            {
                Cargar();
            }
            // atiende el error de esta operacion antes de informar o propagar el problema
            catch (Exception error)
            {
                MessageBox.Show("No se pudieron cargar los usuarios: " + error.Message, "Usuarios", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // carga usuarios y prepara la opcion de crear una cuenta
        private void Cargar()
        {
            // muestra solo cuentas activas y conserva las bajas en la base de datos
            List<usuario> lista = cnUsuario.ListarUsuarios().Where(usuarioActual => usuarioActual.activo).ToList();
            lista.Insert(0, new usuario { nombreUsuario = "Crear un usuario nuevo" });
            selector.DataSource = lista;
            selector.SelectedIndex = 0;
            Seleccionar(null, EventArgs.Empty);
        }

        // actualiza los campos y el estado de bloqueo de la cuenta seleccionada
        private void Seleccionar(object? sender, EventArgs e)
        {
            if (selector.SelectedItem is not usuario u)
            {
                return;
            }

            idSeleccionado = u.idUsuario;
            if (idSeleccionado == 0)
            {
                txtNombreUsuario.Text = "";
            }
            else
            {
                txtNombreUsuario.Text = u.nombreUsuario;
            }
            txtContrasena.Clear();
            if (idSeleccionado == 0)
            {
                cmbRol.SelectedIndex = 1;
            }
            else
            {
                cmbRol.SelectedIndex = u.IdRolUsuario - 1;
            }
            if (idSeleccionado == 0)
            {
                lblSeccion.Text = "Crear usuario";
            }
            else
            {
                lblSeccion.Text = "Modificar usuario";
            }
            if (idSeleccionado == 0)
            {
                lblContrasena.Text = "Contraseña";
            }
            else
            {
                lblContrasena.Text = "Nueva contraseña (vacía conserva la actual)";
            }
            if (idSeleccionado == 0)
            {
                btnGuardar.Text = "Crear usuario";
            }
            else
            {
                btnGuardar.Text = "Guardar cambios";
            }
            btnDesbloquear.Enabled = idSeleccionado != 0 && u.EstaBloqueado;
            if (idSeleccionado == 0)
            {
                lblEstadoCuenta.Text = string.Empty;
            }
            else
            {
                if (u.activo)
                {
                    lblEstadoCuenta.Text = "Estado: activo";
                }
                else
                {
                    lblEstadoCuenta.Text = "Estado: dado de baja";
                }
            }
            btnDarDeBaja.Enabled = idSeleccionado != 0 && u.activo;
            // comprueba si se recibio el valor opcional antes de utilizarlo
            if (u.EstaBloqueado && u.BloqueadoHasta.HasValue)
            {
                lblEstadoBloqueo.Text = "Bloqueado hasta " + u.BloqueadoHasta.Value.ToString("dd/MM/yyyy HH:mm");
            }
            else
            {
                lblEstadoBloqueo.Text = string.Empty;
            }
        }

        // guarda los datos del usuario seleccionado o crea una cuenta nueva atiende la pulsacion de btnguardar y ejecuta la accion asociada el evento no significa que la operacion ya se haya guardado
        private void btnGuardar_Click(object? sender, EventArgs e)
        {
            try
            {
                bool editando = idSeleccionado != 0;
                if (editando)
                {
                    cnUsuario.ModificarUsuario(idSeleccionado, txtNombreUsuario.Text, txtContrasena.Text, cmbRol.SelectedIndex + 1);
                }
                else
                {
                    cnUsuario.CrearUsuario(txtNombreUsuario.Text.Trim(), txtContrasena.Text, cmbRol.SelectedIndex + 1);
                }

                txtContrasena.Clear();
                string valorSeleccionado;
                if (editando)
                {
                    valorSeleccionado = "Usuario actualizado correctamente.";
                }
                else
                {
                    valorSeleccionado = "Usuario creado correctamente.";
                }
                MessageBox.Show(valorSeleccionado, "Usuarios", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Cargar();
            }
            // atiende el error de esta operacion antes de informar o propagar el problema
            catch (Exception error)
            {
                MessageBox.Show("No se pudo guardar el usuario: " + error.Message, "Usuarios", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // desbloquea la cuenta seleccionada usando la sesion de administrador activa atiende la pulsacion de btndesbloquear y ejecuta la accion asociada el evento no significa que la operacion ya se haya guardado
        private void btnDesbloquear_Click(object? sender, EventArgs e)
        {
            if (idSeleccionado == 0)
            {
                return;
            }

            if (MessageBox.Show(this, "Se restableceran los intentos y el usuario podra volver a ingresar. Continuar?", "Desbloquear usuario", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            try
            {
                cnUsuario.DesbloquearUsuario(idSeleccionado);
                MessageBox.Show(this, "La cuenta fue desbloqueada correctamente.", "Usuarios", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Cargar();
            }
            // atiende el error de esta operacion antes de informar o propagar el problema
            catch (Exception error)
            {
                MessageBox.Show(this, "No se pudo desbloquear la cuenta: " + error.Message, "Usuarios", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // confirma la baja logica y actualiza la lista de usuarios atiende la pulsacion de btndardebaja y ejecuta la accion asociada el evento no significa que la operacion ya se haya guardado
        private void btnDarDeBaja_Click(object? sender, EventArgs e)
        {
            if (idSeleccionado == 0)
            {
                return;
            }

            if (MessageBox.Show(this, "El usuario perdera el acceso al sistema. Sus ventas y registros historicos se conservaran. Continuar?", "Dar de baja usuario", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
            {
                return;
            }

            try
            {
                cnUsuario.DarDeBajaUsuario(idSeleccionado);
                MessageBox.Show(this, "El usuario fue dado de baja. Sus registros historicos se conservaron.", "Usuarios", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Cargar();
            }
            // atiende el error de esta operacion antes de informar o propagar el problema
            catch (Exception error)
            {
                MessageBox.Show(this, "No se pudo dar de baja al usuario: " + error.Message, "Usuarios", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // conserva el evento vacio conectado al disenador atiende la pulsacion de label1 y ejecuta la accion asociada el evento no significa que la operacion ya se haya guardado
        private void label1_Click(object sender, EventArgs e)
        {
        }

        // selecciona la opcion para crear un usuario nuevo atiende la pulsacion de nuevo y ejecuta la accion asociada el evento no significa que la operacion ya se haya guardado
        private void nuevo_Click(object? sender, EventArgs e) => selector.SelectedIndex = 0;
    }
}
