using System;
using System.Windows.Forms;
using Prana.negocio;

namespace Prana.Presentacion
{
    // agrupa los datos y operaciones de interfaz
    public partial class Interfaz : Form
    {
        // variable que va a guardar la referencia al formulario mostrado actualmente al principio vale null porque todavia no se abrio ninguna seccion
        private Form? formularioActivo;
        // prepara interfaz antes de que se utilice la instancia
        public Interfaz()
        {
            InitializeComponent();
            btnAlerta.Visible = Permisos.TienePermiso(Permisos.ConsultarAlertas);
            btnHistorial.Visible = Permisos.TienePermiso(Permisos.ConsultarHistorial);
            btnInformes.Visible = Permisos.TienePermiso(Permisos.ConsultarInformes);
            btnUsuarios.Visible = Permisos.TienePermiso(Permisos.GestionarUsuarios);
            btnMediosPagos.Visible = Permisos.TienePermiso(Permisos.ModificarMediosPago);
        }

        // abre un formulario dentro del panel derecho
        public void AbrirFormulario(Form formulario)
        {
            // cierra la seccion anterior, si existe
            formularioActivo?.Close();
            // guarda la nueva seccion como activa
            formularioActivo = formulario;
            // permite insertar el formulario dentro del panel
            formulario.TopLevel = false;
            // elimina los bordes y la barra superior
            formulario.FormBorderStyle = FormBorderStyle.None;
            // hace que ocupe todo el espacio disponible formulariodock: borde o zona del contenedor a la que se ajusta automaticamente el control
            formulario.Dock = DockStyle.Fill;
            // elimina la seccion mostrada anteriormente vacia la coleccion o campo indicado revisar si es una carga temporal antes de reutilizarlo
            pnlContenido.Controls.Clear();
            // agrega la nueva seccion al panel derecho inserta el control en su contenedor; esto determina su jerarquia visual y posicion relativa
            pnlContenido.Controls.Add(formulario);
            // muestra la nueva seccion
            formulario.Show();
        }

        // prepara la informacion inicial cuando se carga la ventana
        private void Interfaz_Load(object sender, EventArgs e)
        {
            // muestra el panel principal al abrir la interfaz
            AbrirFormulario(new frmPanelPrincipal());
        }

        // ejecuta la accion de icon button2 cuando el usuario pulsa el control
        private void iconButton2_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new frmNuevaVenta());
        }

        // manejador sin instrucciones no realiza ninguna accion adicional
        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
        }

        // manejador sin instrucciones no realiza ninguna accion adicional
        private void menuStrip2_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
        }

        // ejecuta la accion de label1 cuando el usuario pulsa el control
        private void label1_Click(object sender, EventArgs e)
        {
        }

        // ejecuta la accion de icon menu item1 cuando el usuario pulsa el control
        private void iconMenuItem1_Click(object sender, EventArgs e)
        {
        }

        // ejecuta la accion de icon menu item12 cuando el usuario pulsa el control
        private void iconMenuItem12_Click(object sender, EventArgs e)
        {
        }

        // ejecuta la accion de picture box1 cuando el usuario pulsa el control
        private void pictureBox1_Click(object sender, EventArgs e)
        {
        }

        // ejecuta la accion de icon button1 cuando el usuario pulsa el control
        private void iconButton1_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new frmPanelPrincipal());
        }

        // ejecuta la accion de btn clientes cuando el usuario pulsa el control
        private void btnClientes_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new frmClientes());
        }

        // ejecuta la accion de btn stock cuando el usuario pulsa el control
        private void btnStock_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new frmStock());
        }

        // ejecuta la accion de btn pagos cuando el usuario pulsa el control
        private void btnPagos_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new frmPagos());
        }

        // ejecuta la accion de btn medios pagos cuando el usuario pulsa el control
        private void btnMediosPagos_Click(object sender, EventArgs e)
        {
            // detiene el evento si el usuario no puede modificar medios de pago
            if (!Permisos.TienePermiso(Permisos.ModificarMediosPago))
            {
                return;
            }

            // abre el abm de medios de pago
            AbrirFormulario(new frmMediosPagos());
        }

        // ejecuta la accion de btn entregas cuando el usuario pulsa el control
        private void btnEntregas_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new frmEntregas());
        }

        // ejecuta la accion de btn alerta cuando el usuario pulsa el control
        private void btnAlerta_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new frmAlertas());
        }

        // ejecuta la accion de btn informes cuando el usuario pulsa el control
        private void btnInformes_Click(object sender, EventArgs e)
        {
                AbrirFormulario(new frmInformesVentas());
            
        }

        // ejecuta la accion de btn historial cuando el usuario pulsa el control
        private void btnHistorial_Click(object sender, EventArgs e)
        {
                AbrirFormulario(new frmHistorial());
        
        }

        // ejecuta la accion de btn usuarios cuando el usuario pulsa el control
        private void btnUsuarios_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new frmUsuarios());
        }

        // manejador sin instrucciones no realiza ninguna accion adicional
        private void pnlContenido_Paint(object sender, PaintEventArgs e)
        {
        }
    }
}
