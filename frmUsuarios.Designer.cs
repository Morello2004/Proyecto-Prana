namespace Prana.Presentacion
{
    // parte visual de frmusuarios crea controles y define su distribucion, colores y eventos; visual studio puede regenerar este archivo al editar el disenador
    partial class frmUsuarios
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
            nuevo = new Button();
            selector = new ComboBox();
            label1 = new Label();
            pnlEncabezado = new Panel();
            pnlFormulario = new Panel();
            lblSeccion = new Label();
            lblNombreUsuario = new Label();
            txtNombreUsuario = new TextBox();
            lblContrasena = new Label();
            txtContrasena = new TextBox();
            lblRol = new Label();
            cmbRol = new ComboBox();
            btnGuardar = new Button();
            lblEstadoBloqueo = new Label();
            btnDesbloquear = new Button();
            lblEstadoCuenta = new Label();
            btnDarDeBaja = new Button();
            pnlEncabezado.SuspendLayout();
            pnlFormulario.SuspendLayout();
            SuspendLayout();
            // nuevo nuevolocation: posicion horizontal y vertical respecto del contenedor
            nuevo.Location = new Point(220, 380);
            nuevo.Name = "nuevo";
            nuevo.Size = new Size(180, 34);
            nuevo.TabIndex = 9;
            nuevo.Text = "Nuevo usuario";
            nuevo.Click += nuevo_Click;
            // selector
            selector.DisplayMember = "NombreParaMostrar";
            selector.DropDownStyle = ComboBoxStyle.DropDownList;
            selector.Location = new Point(20, 55);
            selector.Name = "selector";
            selector.Size = new Size(470, 23);
            selector.TabIndex = 8;
            selector.SelectedIndexChanged += Seleccionar;
            // label1
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            label1.ForeColor = Color.White;
            label1.Location = new Point(20, 20);
            label1.Name = "label1";
            label1.Size = new Size(113, 32);
            label1.TabIndex = 0;
            label1.Text = "Usuarios";
            label1.Click += label1_Click;
            // pnlencabezado pnlencabezadobackcolor: color del fondo del control o de la celda
            pnlEncabezado.BackColor = Color.FromArgb(21, 30, 41);
            pnlEncabezado.Controls.Add(label1);
            pnlEncabezado.Dock = DockStyle.Top;
            pnlEncabezado.Location = new Point(0, 0);
            pnlEncabezado.Name = "pnlEncabezado";
            pnlEncabezado.Size = new Size(783, 76);
            pnlEncabezado.TabIndex = 1;
            // pnlformulario pnlformulariobackcolor: color del fondo del control o de la celda
            pnlFormulario.BackColor = Color.FromArgb(21, 30, 41);
            pnlFormulario.Controls.Add(lblSeccion);
            pnlFormulario.Controls.Add(lblNombreUsuario);
            pnlFormulario.Controls.Add(txtNombreUsuario);
            pnlFormulario.Controls.Add(lblContrasena);
            pnlFormulario.Controls.Add(txtContrasena);
            pnlFormulario.Controls.Add(lblRol);
            pnlFormulario.Controls.Add(cmbRol);
            pnlFormulario.Controls.Add(btnGuardar);
            pnlFormulario.Controls.Add(lblEstadoBloqueo);
            pnlFormulario.Controls.Add(btnDesbloquear);
            pnlFormulario.Controls.Add(lblEstadoCuenta);
            pnlFormulario.Controls.Add(btnDarDeBaja);
            pnlFormulario.Controls.Add(selector);
            pnlFormulario.Controls.Add(nuevo);
            pnlFormulario.Location = new Point(16, 92);
            pnlFormulario.Name = "pnlFormulario";
            pnlFormulario.Size = new Size(520, 610);
            pnlFormulario.TabIndex = 0;
            // lblseccion
            lblSeccion.AutoSize = true;
            lblSeccion.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblSeccion.Location = new Point(20, 18);
            lblSeccion.Name = "lblSeccion";
            lblSeccion.Size = new Size(111, 21);
            lblSeccion.TabIndex = 0;
            lblSeccion.Text = "Crear usuario";
            // lblnombreusuario
            lblNombreUsuario.AutoSize = true;
            lblNombreUsuario.ForeColor = Color.FromArgb(145, 170, 210);
            lblNombreUsuario.Location = new Point(20, 135);
            lblNombreUsuario.Name = "lblNombreUsuario";
            lblNombreUsuario.Size = new Size(109, 15);
            lblNombreUsuario.TabIndex = 1;
            lblNombreUsuario.Text = "Nombre de usuario";
            // txtnombreusuario txtnombreusuariofont: tipografia, tamano y estilo con los que se muestra el contenido
            txtNombreUsuario.Font = new Font("Segoe UI", 10F);
            txtNombreUsuario.Location = new Point(20, 157);
            txtNombreUsuario.MaxLength = 20;
            txtNombreUsuario.Name = "txtNombreUsuario";
            txtNombreUsuario.Size = new Size(470, 25);
            txtNombreUsuario.TabIndex = 0;
            // lblcontrasena
            lblContrasena.AutoSize = true;
            lblContrasena.ForeColor = Color.FromArgb(145, 170, 210);
            lblContrasena.Location = new Point(20, 207);
            lblContrasena.Name = "lblContrasena";
            lblContrasena.Size = new Size(67, 15);
            lblContrasena.TabIndex = 2;
            lblContrasena.Text = "Contraseña";
            // txtcontrasena txtcontrasenafont: tipografia, tamano y estilo con los que se muestra el contenido
            txtContrasena.Font = new Font("Segoe UI", 10F);
            txtContrasena.Location = new Point(20, 229);
            txtContrasena.MaxLength = 30;
            txtContrasena.Name = "txtContrasena";
            txtContrasena.Size = new Size(470, 25);
            txtContrasena.TabIndex = 1;
            txtContrasena.UseSystemPasswordChar = true;
            // lblrol
            lblRol.AutoSize = true;
            lblRol.ForeColor = Color.FromArgb(145, 170, 210);
            lblRol.Location = new Point(20, 279);
            lblRol.Name = "lblRol";
            lblRol.Size = new Size(24, 15);
            lblRol.TabIndex = 3;
            lblRol.Text = "Rol";
            // cmbrol cmbroldropdownstyle: establece si la lista permite escribir o solo elegir elementos
            cmbRol.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbRol.Font = new Font("Segoe UI", 10F);
            cmbRol.Items.AddRange(new object[] { "Administrador", "Vendedor" });
            cmbRol.Location = new Point(20, 301);
            cmbRol.Name = "cmbRol";
            cmbRol.Size = new Size(470, 25);
            cmbRol.TabIndex = 2;
            // btnguardar btnguardarbackcolor: color del fondo del control o de la celda
            btnGuardar.BackColor = Color.DodgerBlue;
            btnGuardar.FlatAppearance.BorderSize = 0;
            btnGuardar.FlatStyle = FlatStyle.Flat;
            btnGuardar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnGuardar.ForeColor = Color.White;
            btnGuardar.Location = new Point(20, 380);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(180, 34);
            btnGuardar.TabIndex = 3;
            btnGuardar.Text = "Guardar usuario";
            btnGuardar.UseVisualStyleBackColor = false;
            btnGuardar.Click += btnGuardar_Click;
            // lblestadobloqueo
            lblEstadoBloqueo.AutoSize = true;
            lblEstadoBloqueo.ForeColor = Color.Orange;
            lblEstadoBloqueo.Location = new Point(20, 426);
            lblEstadoBloqueo.Name = "lblEstadoBloqueo";
            lblEstadoBloqueo.Size = new Size(0, 15);
            lblEstadoBloqueo.TabIndex = 4;
            // btndesbloquear btndesbloquearbackcolor: color del fondo del control o de la celda
            btnDesbloquear.BackColor = Color.DarkOrange;
            btnDesbloquear.Enabled = false;
            btnDesbloquear.FlatAppearance.BorderSize = 0;
            btnDesbloquear.FlatStyle = FlatStyle.Flat;
            btnDesbloquear.ForeColor = Color.White;
            btnDesbloquear.Location = new Point(20, 450);
            btnDesbloquear.Name = "btnDesbloquear";
            btnDesbloquear.Size = new Size(200, 34);
            btnDesbloquear.TabIndex = 5;
            btnDesbloquear.Text = "Desbloquear usuario";
            btnDesbloquear.UseVisualStyleBackColor = false;
            btnDesbloquear.Click += btnDesbloquear_Click;
            // lblestadocuenta
            lblEstadoCuenta.AutoSize = true;
            lblEstadoCuenta.ForeColor = Color.LightGray;
            lblEstadoCuenta.Location = new Point(20, 494);
            lblEstadoCuenta.Name = "lblEstadoCuenta";
            lblEstadoCuenta.Size = new Size(0, 15);
            lblEstadoCuenta.TabIndex = 6;
            // btndardebaja btndardebajabackcolor: color del fondo del control o de la celda
            btnDarDeBaja.BackColor = Color.Firebrick;
            btnDarDeBaja.Enabled = false;
            btnDarDeBaja.FlatAppearance.BorderSize = 0;
            btnDarDeBaja.FlatStyle = FlatStyle.Flat;
            btnDarDeBaja.ForeColor = Color.White;
            btnDarDeBaja.Location = new Point(20, 520);
            btnDarDeBaja.Name = "btnDarDeBaja";
            btnDarDeBaja.Size = new Size(200, 34);
            btnDarDeBaja.TabIndex = 7;
            btnDarDeBaja.Text = "Dar de baja usuario";
            btnDarDeBaja.UseVisualStyleBackColor = false;
            btnDarDeBaja.Click += btnDarDeBaja_Click;
            // frmusuarios
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            AutoScrollMinSize = new Size(560, 650);
            BackColor = Color.FromArgb(13, 19, 27);
            ClientSize = new Size(800, 450);
            Controls.Add(pnlFormulario);
            Controls.Add(pnlEncabezado);
            ForeColor = Color.White;
            Name = "frmUsuarios";
            Text = "frmUsuarios";
            Load += frmUsuarios_Load;
            pnlEncabezado.ResumeLayout(false);
            pnlEncabezado.PerformLayout();
            pnlFormulario.ResumeLayout(false);
            pnlFormulario.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private Label label1;
        private Panel pnlEncabezado, pnlFormulario;
        private Label lblSeccion, lblNombreUsuario, lblContrasena, lblRol;
        private TextBox txtNombreUsuario, txtContrasena;
        private ComboBox cmbRol;
        private Button btnGuardar;
        private Label lblEstadoBloqueo;
        private Button btnDesbloquear;
        private Label lblEstadoCuenta;
        private Button btnDarDeBaja;
        private ComboBox selector;
        private Button nuevo;
    }
}
