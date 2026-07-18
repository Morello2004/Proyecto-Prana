using Prana.negocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Prana.Presentacion
{
    public partial class login : Form
    {
        public login()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void login_Load(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void iconButton2_Click(object sender, EventArgs e)
        {
            //cierra la instancia actual de la clase cerrando el formulario de login
            this.Close();
        }

        private void iconButton1_Click(object sender, EventArgs e)
        {
            try
            {   // Se crea un objeto de la capa de Negocio
                CN_Usuario N_Usuario = new CN_Usuario();

                // Se manda a la capa de negocio los datos escritos en el formulario y devuelve el usuario correcto o null
                var usuario = N_Usuario.IniciarSesion(txtusuario.Text, txtclave.Text);

                if (usuario == null)
                {
                    MessageBox.Show(
                        "Nombre de usuario o contraseña incorrectos.",
                        "Inicio de sesión",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );

                    // Borra solamente la contraseña para volver a intentarlo
                    txtclave.Clear();

                    return;
                }
                // abre un nuevo formulario el cual es el inicio
                Interfaz form = new Interfaz();

                //muestra y abre en pantalla el formulario creado
                form.Show();

                //oculta el formulario de logueo una vez ingresado
                this.Hide();

                //Cuando el formulario form se esté cerrando, ejecutá el método frm_closing
                form.FormClosing += frm_closing;


            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Ocurrió un error al iniciar sesión: " + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }
        


        //este metodo hace que cuando se cierre la interfaz de inicio se muestre la de logueo( recibe el objeto que produce el evento que es 
        //el formulario que se esta cerrando y e Windows Forms lo crea automáticamente antes de cerrar el formulario tiene información sobre el cierre y permite controlarlo
        private void frm_closing(object? sender, FormClosingEventArgs e)
        {
            //pone en blanco campos de usuario y clave
            txtclave.Text = "";
            txtusuario.Text = "";

            this.Show();


        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
