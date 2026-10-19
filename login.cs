using Prana.negocio;
using Prana.Entidades;
using System;
using System.Windows.Forms;

namespace Prana.Presentacion
{
    public partial class login : Form
    {
        // Inicializa la pantalla de autenticacion.
        public login()
        {
            InitializeComponent();
        }

        // Conserva el evento vacio conectado al disenador
        private void pictureBox1_Click(object sender, EventArgs e)
        {
        }

        // Conserva el evento vacio conectado al disenador
        private void label2_Click(object sender, EventArgs e)
        {
        }

        // Conserva el evento vacio conectado al disenador
        private void login_Load(object sender, EventArgs e)
        {
        }

        // Conserva el evento vacio conectado al disenador
        private void label3_Click(object sender, EventArgs e)
        {
        }

        // Cierra el formulario de autenticacion.
        private void iconButton2_Click(object sender, EventArgs e)
        {
            Close();
        }

        // Autentica al usuario y presenta los intentos disponibles o el bloqueo.
        private void iconButton1_Click(object sender, EventArgs e)
        {
            try
            {
                CN_Usuario negocioUsuario = new CN_Usuario();
                var resultado = negocioUsuario.IniciarSesion(txtusuario.Text, txtclave.Text);
                if (!resultado.Autenticado)
                {
                    MessageBox.Show(resultado.Mensaje, "Inicio de sesion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtclave.Clear();
                    return;
                }

                usuario usuarioAutenticado = resultado.Usuario ?? throw new InvalidOperationException("La autenticacion no devolvio el usuario.");
                SesionActual.IniciarSesion(usuarioAutenticado);
                Interfaz form = new Interfaz();
                form.Show();
                this.Hide();
                form.FormClosing += frm_closing;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrio un error al iniciar sesion: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Cierra la sesion y vuelve a mostrar el formulario de autenticacion.
        private void frm_closing(object? sender, FormClosingEventArgs e)
        {
            SesionActual.CerrarSesion();
            txtclave.Clear();
            txtusuario.Clear();
            this.Show();
        }

        // Conserva el evento vacio conectado al disenador
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }
    }
}
