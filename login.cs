using Prana.negocio;
using Prana.Entidades;
using System;
using System.Windows.Forms;

namespace Prana.Presentacion
{
    // agrupa los datos y operaciones de login
    public partial class login : Form
    {
        // inicializa la pantalla de autenticacion
        public login()
        {
            InitializeComponent();
        }

        // conserva el evento vacio conectado al disenador atiende la pulsacion de picturebox1 y ejecuta la accion asociada el evento no significa que la operacion ya se haya guardado
        private void pictureBox1_Click(object sender, EventArgs e)
        {
        }

        // conserva el evento vacio conectado al disenador atiende la pulsacion de label2 y ejecuta la accion asociada el evento no significa que la operacion ya se haya guardado
        private void label2_Click(object sender, EventArgs e)
        {
        }

        // conserva el evento vacio conectado al disenador
        private void login_Load(object sender, EventArgs e)
        {
        }

        // conserva el evento vacio conectado al disenador atiende la pulsacion de label3 y ejecuta la accion asociada el evento no significa que la operacion ya se haya guardado
        private void label3_Click(object sender, EventArgs e)
        {
        }

        // cierra el formulario de autenticacion atiende la pulsacion de iconbutton2 y ejecuta la accion asociada el evento no significa que la operacion ya se haya guardado
        private void iconButton2_Click(object sender, EventArgs e)
        {
            Close();
        }

        // autentica al usuario y presenta los intentos disponibles o el bloqueo atiende la pulsacion de iconbutton1 y ejecuta la accion asociada el evento no significa que la operacion ya se haya guardado
        private void iconButton1_Click(object sender, EventArgs e)
        {
            try
            {
                CN_Usuario negocioUsuario = new CN_Usuario();
                ResultadoInicioSesion resultado = negocioUsuario.IniciarSesion(txtusuario.Text, txtclave.Text);
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
            // atiende el error de esta operacion antes de informar o propagar el problema
            catch (Exception error)
            {
                MessageBox.Show("Ocurrio un error al iniciar sesion: " + error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // cierra la sesion y vuelve a mostrar el formulario de autenticacion atiende el evento closing del control o formulario indicado por el nombre del metodo
        private void frm_closing(object? sender, FormClosingEventArgs e)
        {
            SesionActual.CerrarSesion();
            txtclave.Clear();
            txtusuario.Clear();
            this.Show();
        }
        // manejador sin instrucciones no realiza ninguna accion adicional
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }
    }
}
