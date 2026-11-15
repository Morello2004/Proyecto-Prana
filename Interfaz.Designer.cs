namespace Prana.Presentacion
{
    // parte visual de interfaz crea controles y define su distribucion, colores y eventos; visual studio puede regenerar este archivo al editar el disenador
    partial class Interfaz
    {
        // /// required designer variable
        private System.ComponentModel.IContainer components = null;
        // /// clean up any resources being used true if managed resources should be disposed; otherwise, false libera los recursos del formulario; disposing indica si tambien deben liberarse los componentes administrados
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        // /// required method for designer support - do not modify the contents of this method with the code editor construye la pantalla: instancia controles, asigna propiedades, agrega cada control a su contenedor y conecta los eventos no registra operaciones comerciales
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Interfaz));
            pnlMenuLateral = new Panel();
            panel2 = new Panel();
            btnUsuarios = new FontAwesome.Sharp.IconButton();
            btnAlerta = new FontAwesome.Sharp.IconButton();
            btnInformes = new FontAwesome.Sharp.IconButton();
            btnHistorial = new FontAwesome.Sharp.IconButton();
            btnEntregas = new FontAwesome.Sharp.IconButton();
            btnMediosPagos = new FontAwesome.Sharp.IconButton();
            btnPagos = new FontAwesome.Sharp.IconButton();
            btnStock = new FontAwesome.Sharp.IconButton();
            btnClientes = new FontAwesome.Sharp.IconButton();
            btnNuevaVenta = new FontAwesome.Sharp.IconButton();
            btnPanelPrincipal = new FontAwesome.Sharp.IconButton();
            panel1 = new Panel();
            pictureBox1 = new PictureBox();
            pnlContenido = new Panel();
            pnlMenuLateral.SuspendLayout();
            panel1.SuspendLayout();
            // llama a ((systemcomponentmodelisupportinitialize)picturebox1)begininit para realizar este paso del flujo
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // pnlmenulateral pnlmenulateralbackcolor: color del fondo del control o de la celda
            pnlMenuLateral.BackColor = Color.FromArgb(20, 24, 30);
            pnlMenuLateral.Controls.Add(panel2);
            pnlMenuLateral.Controls.Add(btnUsuarios);
            pnlMenuLateral.Controls.Add(btnAlerta);
            pnlMenuLateral.Controls.Add(btnInformes);
            pnlMenuLateral.Controls.Add(btnHistorial);
            pnlMenuLateral.Controls.Add(btnEntregas);
            pnlMenuLateral.Controls.Add(btnMediosPagos);
            pnlMenuLateral.Controls.Add(btnPagos);
            pnlMenuLateral.Controls.Add(btnStock);
            pnlMenuLateral.Controls.Add(btnClientes);
            pnlMenuLateral.Controls.Add(btnNuevaVenta);
            pnlMenuLateral.Controls.Add(btnPanelPrincipal);
            pnlMenuLateral.Controls.Add(panel1);
            pnlMenuLateral.Dock = DockStyle.Left;
            pnlMenuLateral.Location = new Point(0, 0);
            pnlMenuLateral.Name = "pnlMenuLateral";
            pnlMenuLateral.Size = new Size(230, 558);
            pnlMenuLateral.TabIndex = 0;
            // panel2 panel2location: posicion horizontal y vertical respecto del contenedor
            panel2.Location = new Point(228, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(842, 560);
            panel2.TabIndex = 11;
            // btnusuarios
            btnUsuarios.FlatStyle = FlatStyle.Flat;
            btnUsuarios.Font = new Font("Arial Black", 11F, FontStyle.Bold);
            btnUsuarios.IconChar = FontAwesome.Sharp.IconChar.None;
            btnUsuarios.IconColor = Color.Black;
            btnUsuarios.IconFont = FontAwesome.Sharp.IconFont.Auto;
            btnUsuarios.Location = new Point(12, 501);
            btnUsuarios.Name = "btnUsuarios";
            btnUsuarios.Size = new Size(210, 46);
            btnUsuarios.TabIndex = 10;
            btnUsuarios.Text = "USUARIOS";
            btnUsuarios.UseVisualStyleBackColor = true;
            btnUsuarios.Click += btnUsuarios_Click;
            // btnalerta
            btnAlerta.FlatStyle = FlatStyle.Flat;
            btnAlerta.Font = new Font("Arial Black", 11F, FontStyle.Bold);
            btnAlerta.IconChar = FontAwesome.Sharp.IconChar.None;
            btnAlerta.IconColor = Color.Black;
            btnAlerta.IconFont = FontAwesome.Sharp.IconFont.Auto;
            btnAlerta.Location = new Point(12, 449);
            btnAlerta.Name = "btnAlerta";
            btnAlerta.Size = new Size(210, 46);
            btnAlerta.TabIndex = 9;
            btnAlerta.Text = "ALERTAS";
            btnAlerta.UseVisualStyleBackColor = true;
            btnAlerta.Click += btnAlerta_Click;
            // btninformes
            btnInformes.FlatStyle = FlatStyle.Flat;
            btnInformes.Font = new Font("Arial Black", 11F, FontStyle.Bold);
            btnInformes.IconChar = FontAwesome.Sharp.IconChar.None;
            btnInformes.IconColor = Color.Black;
            btnInformes.IconFont = FontAwesome.Sharp.IconFont.Auto;
            btnInformes.Location = new Point(12, 397);
            btnInformes.Name = "btnInformes";
            btnInformes.Size = new Size(210, 46);
            btnInformes.TabIndex = 8;
            btnInformes.Text = "INFORMES";
            btnInformes.UseVisualStyleBackColor = true;
            btnInformes.Click += btnInformes_Click;
            // btnhistorial
            btnHistorial.FlatStyle = FlatStyle.Flat;
            btnHistorial.Font = new Font("Arial Black", 11F, FontStyle.Bold);
            btnHistorial.IconChar = FontAwesome.Sharp.IconChar.None;
            btnHistorial.IconColor = Color.Black;
            btnHistorial.IconFont = FontAwesome.Sharp.IconFont.Auto;
            btnHistorial.Location = new Point(12, 501);
            btnHistorial.Name = "btnHistorial";
            btnHistorial.Size = new Size(210, 46);
            btnHistorial.TabIndex = 11;
            btnHistorial.Text = "HISTORIAL";
            btnHistorial.UseVisualStyleBackColor = true;
            btnHistorial.Click += btnHistorial_Click;
            // btnentregas
            btnEntregas.FlatStyle = FlatStyle.Flat;
            btnEntregas.Font = new Font("Arial Black", 11F, FontStyle.Bold);
            btnEntregas.IconChar = FontAwesome.Sharp.IconChar.None;
            btnEntregas.IconColor = Color.Black;
            btnEntregas.IconFont = FontAwesome.Sharp.IconFont.Auto;
            btnEntregas.Location = new Point(12, 397);
            btnEntregas.Name = "btnEntregas";
            btnEntregas.Size = new Size(210, 46);
            btnEntregas.TabIndex = 7;
            btnEntregas.Text = "ENTREGAS";
            btnEntregas.UseVisualStyleBackColor = true;
            btnEntregas.Click += btnEntregas_Click;
            // btnmediospagos
            btnMediosPagos.FlatStyle = FlatStyle.Flat;
            btnMediosPagos.Font = new Font("Arial Black", 11F, FontStyle.Bold);
            btnMediosPagos.IconChar = FontAwesome.Sharp.IconChar.None;
            btnMediosPagos.IconColor = Color.Black;
            btnMediosPagos.IconFont = FontAwesome.Sharp.IconFont.Auto;
            btnMediosPagos.Location = new Point(12, 345);
            btnMediosPagos.Name = "btnMediosPagos";
            btnMediosPagos.Size = new Size(210, 46);
            btnMediosPagos.TabIndex = 6;
            btnMediosPagos.Text = "MEDIOS PAGOS";
            btnMediosPagos.UseVisualStyleBackColor = true;
            btnMediosPagos.Click += btnMediosPagos_Click;
            // btnpagos
            btnPagos.FlatStyle = FlatStyle.Flat;
            btnPagos.Font = new Font("Arial Black", 11F, FontStyle.Bold);
            btnPagos.IconChar = FontAwesome.Sharp.IconChar.None;
            btnPagos.IconColor = Color.Black;
            btnPagos.IconFont = FontAwesome.Sharp.IconFont.Auto;
            btnPagos.Location = new Point(12, 293);
            btnPagos.Name = "btnPagos";
            btnPagos.Size = new Size(210, 46);
            btnPagos.TabIndex = 5;
            btnPagos.Text = "PAGOS";
            btnPagos.UseVisualStyleBackColor = true;
            btnPagos.Click += btnPagos_Click;
            // btnstock
            btnStock.FlatStyle = FlatStyle.Flat;
            btnStock.Font = new Font("Arial Black", 11F, FontStyle.Bold);
            btnStock.IconChar = FontAwesome.Sharp.IconChar.None;
            btnStock.IconColor = Color.Black;
            btnStock.IconFont = FontAwesome.Sharp.IconFont.Auto;
            btnStock.Location = new Point(12, 241);
            btnStock.Name = "btnStock";
            btnStock.Size = new Size(210, 46);
            btnStock.TabIndex = 4;
            btnStock.Text = "STOCK";
            btnStock.UseVisualStyleBackColor = true;
            btnStock.Click += btnStock_Click;
            // btnclientes
            btnClientes.FlatStyle = FlatStyle.Flat;
            btnClientes.Font = new Font("Arial Black", 11F, FontStyle.Bold);
            btnClientes.IconChar = FontAwesome.Sharp.IconChar.None;
            btnClientes.IconColor = Color.Black;
            btnClientes.IconFont = FontAwesome.Sharp.IconFont.Auto;
            btnClientes.Location = new Point(12, 189);
            btnClientes.Name = "btnClientes";
            btnClientes.Size = new Size(210, 46);
            btnClientes.TabIndex = 3;
            btnClientes.Text = "CLIENTES";
            btnClientes.UseVisualStyleBackColor = true;
            btnClientes.Click += btnClientes_Click;
            // btnnuevaventa
            btnNuevaVenta.FlatStyle = FlatStyle.Flat;
            btnNuevaVenta.Font = new Font("Arial Black", 11F, FontStyle.Bold);
            btnNuevaVenta.IconChar = FontAwesome.Sharp.IconChar.None;
            btnNuevaVenta.IconColor = Color.Black;
            btnNuevaVenta.IconFont = FontAwesome.Sharp.IconFont.Auto;
            btnNuevaVenta.Location = new Point(12, 137);
            btnNuevaVenta.Name = "btnNuevaVenta";
            btnNuevaVenta.Size = new Size(210, 46);
            btnNuevaVenta.TabIndex = 2;
            btnNuevaVenta.Text = "NUEVA VENTA";
            btnNuevaVenta.UseVisualStyleBackColor = true;
            btnNuevaVenta.Click += iconButton2_Click;
            // btnpanelprincipal
            btnPanelPrincipal.FlatStyle = FlatStyle.Flat;
            btnPanelPrincipal.Font = new Font("Arial Black", 11F, FontStyle.Bold);
            btnPanelPrincipal.IconChar = FontAwesome.Sharp.IconChar.None;
            btnPanelPrincipal.IconColor = Color.Black;
            btnPanelPrincipal.IconFont = FontAwesome.Sharp.IconFont.Auto;
            btnPanelPrincipal.Location = new Point(12, 85);
            btnPanelPrincipal.Name = "btnPanelPrincipal";
            btnPanelPrincipal.Size = new Size(210, 46);
            btnPanelPrincipal.TabIndex = 1;
            btnPanelPrincipal.Text = "PANEL PRINCIPAL";
            btnPanelPrincipal.UseVisualStyleBackColor = true;
            btnPanelPrincipal.Click += iconButton1_Click;
            // panel1 inserta el control en su contenedor; esto determina su jerarquia visual y posicion relativa
            panel1.Controls.Add(pictureBox1);
            panel1.Location = new Point(52, 18);
            panel1.Name = "panel1";
            panel1.Size = new Size(129, 49);
            panel1.TabIndex = 0;
            // picturebox1 picturebox1dock: borde o zona del contenedor a la que se ajusta automaticamente el control
            pictureBox1.Dock = DockStyle.Fill;
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(0, 0);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(129, 49);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            pictureBox1.Click += pictureBox1_Click;
            // pnlcontenido pnlcontenidodock: borde o zona del contenedor a la que se ajusta automaticamente el control
            pnlContenido.Dock = DockStyle.Fill;
            pnlContenido.Location = new Point(230, 0);
            pnlContenido.Name = "pnlContenido";
            pnlContenido.Size = new Size(850, 558);
            pnlContenido.TabIndex = 1;
            pnlContenido.Paint += pnlContenido_Paint;
            // interfaz
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1080, 558);
            Controls.Add(pnlContenido);
            Controls.Add(pnlMenuLateral);
            ForeColor = SystemColors.ButtonHighlight;
            Name = "Interfaz";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Inicio";
            Load += Interfaz_Load;
            pnlMenuLateral.ResumeLayout(false);
            panel1.ResumeLayout(false);
            // llama a ((systemcomponentmodelisupportinitialize)picturebox1)endinit para realizar este paso del flujo
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            // distribucion y estilos editables desde el disenador
            flpMenu = new FlowLayoutPanel();
            flpMenu.Name = "flpMenu";
            WindowState = FormWindowState.Maximized;
            pnlContenido.BackColor = Color.FromArgb(13, 19, 27);
            pnlMenuLateral.Width = 168;
            panel2.Visible = false;
            panel1.Location = new Point(22, 8);
            panel1.Size = new Size(124, 48);
            flpMenu.Location = new Point(0, 64);
            flpMenu.Size = new Size(168, 620);
            flpMenu.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            flpMenu.FlowDirection = FlowDirection.TopDown;
            flpMenu.WrapContents = false;
            flpMenu.AutoScroll = true;
            flpMenu.Padding = new Padding(2, 0, 0, 0);
            btnPanelPrincipal.Dock = DockStyle.None;
            btnPanelPrincipal.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            btnPanelPrincipal.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnPanelPrincipal.Size = new Size(156, 36);
            btnPanelPrincipal.Margin = new Padding(4, 0, 4, 4);
            flpMenu.Controls.Add(btnPanelPrincipal);
            btnNuevaVenta.Dock = DockStyle.None;
            btnNuevaVenta.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            btnNuevaVenta.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnNuevaVenta.Size = new Size(156, 36);
            btnNuevaVenta.Margin = new Padding(4, 0, 4, 4);
            flpMenu.Controls.Add(btnNuevaVenta);
            btnClientes.Dock = DockStyle.None;
            btnClientes.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            btnClientes.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnClientes.Size = new Size(156, 36);
            btnClientes.Margin = new Padding(4, 0, 4, 4);
            flpMenu.Controls.Add(btnClientes);
            btnStock.Dock = DockStyle.None;
            btnStock.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            btnStock.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnStock.Size = new Size(156, 36);
            btnStock.Margin = new Padding(4, 0, 4, 4);
            flpMenu.Controls.Add(btnStock);
            btnPagos.Dock = DockStyle.None;
            btnPagos.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            btnPagos.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnPagos.Size = new Size(156, 36);
            btnPagos.Margin = new Padding(4, 0, 4, 4);
            flpMenu.Controls.Add(btnPagos);
            btnMediosPagos.Dock = DockStyle.None;
            btnMediosPagos.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            btnMediosPagos.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnMediosPagos.Size = new Size(156, 36);
            btnMediosPagos.Margin = new Padding(4, 0, 4, 4);
            flpMenu.Controls.Add(btnMediosPagos);
            btnEntregas.Dock = DockStyle.None;
            btnEntregas.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            btnEntregas.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnEntregas.Size = new Size(156, 36);
            btnEntregas.Margin = new Padding(4, 0, 4, 4);
            flpMenu.Controls.Add(btnEntregas);
            btnInformes.Dock = DockStyle.None;
            btnInformes.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            btnInformes.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnInformes.Size = new Size(156, 36);
            btnInformes.Margin = new Padding(4, 0, 4, 4);
            flpMenu.Controls.Add(btnInformes);
            btnAlerta.Dock = DockStyle.None;
            btnAlerta.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            btnAlerta.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnAlerta.Size = new Size(156, 36);
            btnAlerta.Margin = new Padding(4, 0, 4, 4);
            flpMenu.Controls.Add(btnAlerta);
            btnHistorial.Dock = DockStyle.None;
            btnHistorial.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            btnHistorial.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnHistorial.Size = new Size(156, 36);
            btnHistorial.Margin = new Padding(4, 0, 4, 4);
            flpMenu.Controls.Add(btnHistorial);
            btnUsuarios.Dock = DockStyle.None;
            btnUsuarios.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            btnUsuarios.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnUsuarios.Size = new Size(156, 36);
            btnUsuarios.Margin = new Padding(4, 0, 4, 4);
            flpMenu.Controls.Add(btnUsuarios);
            pnlMenuLateral.Controls.Add(flpMenu);
            pnlMenuLateral.AutoScroll = false;
        }

        #endregion
        private Panel pnlMenuLateral;
        private Panel panel1;
        private PictureBox pictureBox1;
        private FontAwesome.Sharp.IconButton btnPanelPrincipal;
        private FontAwesome.Sharp.IconButton btnNuevaVenta;
        private FontAwesome.Sharp.IconButton btnAlerta;
        private FontAwesome.Sharp.IconButton btnInformes;
        private FontAwesome.Sharp.IconButton btnHistorial;
        private FontAwesome.Sharp.IconButton btnEntregas;
        private FontAwesome.Sharp.IconButton btnMediosPagos;
        private FontAwesome.Sharp.IconButton btnPagos;
        private FontAwesome.Sharp.IconButton btnStock;
        private FontAwesome.Sharp.IconButton btnClientes;
        private FontAwesome.Sharp.IconButton btnUsuarios;
        private Panel panel2;
        private Panel pnlContenido;
        private FlowLayoutPanel flpMenu;
    }
}
