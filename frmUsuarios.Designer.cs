namespace Prana.Presentacion
{
    partial class frmUsuarios
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            nuevo = new Button();

            selector = new ComboBox();

            label1 = new Label();
            pnlEncabezado = new Panel();
            pnlFormulario = new Panel();
            lblSeccion = new Label();
            lblNombreUsuario = new Label();
            lblContrasena = new Label();
            lblRol = new Label();
            txtNombreUsuario = new TextBox();
            txtContrasena = new TextBox();
            cmbRol = new ComboBox();
            btnGuardar = new Button();
            lblEstadoBloqueo = new Label();
            btnDesbloquear = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(191, 58);
            label1.Name = "label1";
            label1.Size = new Size(52, 15);
            label1.TabIndex = 0;
            label1.Text = "Usuarios";
            label1.Click += label1_Click;
            // 
            // frmUsuarios
            // 
            // Encabezado y formulario con la paleta del sistema.
            BackColor = Color.FromArgb(13, 19, 27);
            ForeColor = Color.White;
            AutoScroll = true;
            AutoScrollMinSize = new Size(560, 510);
            pnlEncabezado.Dock = DockStyle.Top;
            pnlEncabezado.Height = 76;
            pnlEncabezado.BackColor = Color.FromArgb(21, 30, 41);
            pnlEncabezado.Controls.Add(label1);
            label1.Location = new Point(20, 20);
            label1.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            label1.ForeColor = Color.White;
            pnlFormulario.Location = new Point(16, 92);
            pnlFormulario.Size = new Size(520, 470);
            pnlFormulario.BackColor = Color.FromArgb(21, 30, 41);
            pnlFormulario.Controls.AddRange(new Control[] { lblSeccion, lblNombreUsuario, txtNombreUsuario, lblContrasena, txtContrasena, lblRol, cmbRol, btnGuardar, lblEstadoBloqueo, btnDesbloquear });
            lblSeccion.Text = "Crear usuario";
            lblSeccion.AutoSize = true;
            lblSeccion.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblSeccion.Location = new Point(20, 18);
            lblNombreUsuario.Text = "Nombre de usuario";
            lblNombreUsuario.AutoSize = true;
            lblNombreUsuario.ForeColor = Color.FromArgb(145, 170, 210);
            lblNombreUsuario.Location = new Point(20, 135);
            txtNombreUsuario.Location = new Point(20, 157);
            txtNombreUsuario.Size = new Size(470, 27);
            txtNombreUsuario.Font = new Font("Segoe UI", 10F);
            txtNombreUsuario.TabIndex = 0;
            lblContrasena.Text = "Contraseña";
            lblContrasena.AutoSize = true;
            lblContrasena.ForeColor = Color.FromArgb(145, 170, 210);
            lblContrasena.Location = new Point(20, 207);
            txtContrasena.Location = new Point(20, 229);
            txtContrasena.Size = new Size(470, 27);
            txtContrasena.Font = new Font("Segoe UI", 10F);
            txtContrasena.UseSystemPasswordChar = true;
            txtContrasena.TabIndex = 1;
            lblRol.Text = "Rol";
            lblRol.AutoSize = true;
            lblRol.ForeColor = Color.FromArgb(145, 170, 210);
            lblRol.Location = new Point(20, 279);
            cmbRol.Location = new Point(20, 301);
            cmbRol.Size = new Size(470, 27);
            cmbRol.Font = new Font("Segoe UI", 10F);
            cmbRol.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbRol.Items.AddRange(new object[] { "Administrador", "Vendedor" });
            cmbRol.TabIndex = 2;
            btnGuardar.Text = "Guardar usuario";
            btnGuardar.Location = new Point(20, 380);
            btnGuardar.Size = new Size(180, 36);
            btnGuardar.TabIndex = 3;
            btnGuardar.Click += btnGuardar_Click;
            btnGuardar.BackColor = Color.DodgerBlue;
            btnGuardar.ForeColor = Color.White;
            btnGuardar.FlatStyle = FlatStyle.Flat;
            btnGuardar.FlatAppearance.BorderSize = 0;
            btnGuardar.UseVisualStyleBackColor = false;
            btnGuardar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnGuardar.Height = 34;
            // Muestra hasta cuando queda bloqueada la cuenta seleccionada.
            lblEstadoBloqueo.AutoSize = true;
            lblEstadoBloqueo.ForeColor = Color.Orange;
            lblEstadoBloqueo.Location = new Point(20, 426);
            lblEstadoBloqueo.Name = "lblEstadoBloqueo";
            lblEstadoBloqueo.Text = "";
            // Permite al administrador actual restablecer la cuenta seleccionada.
            btnDesbloquear.Text = "Desbloquear usuario";
            btnDesbloquear.Location = new Point(20, 450);
            btnDesbloquear.Size = new Size(200, 34);
            btnDesbloquear.Enabled = false;
            btnDesbloquear.BackColor = Color.DarkOrange;
            btnDesbloquear.ForeColor = Color.White;
            btnDesbloquear.FlatStyle = FlatStyle.Flat;
            btnDesbloquear.FlatAppearance.BorderSize = 0;
            btnDesbloquear.Click += btnDesbloquear_Click;

            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(pnlFormulario);
            Controls.Add(pnlEncabezado);
            Name = "frmUsuarios";
            Text = "frmUsuarios";
            Load += frmUsuarios_Load;
            selector.Name = "selector";
            selector.Location = new Point(20, 55);
            selector.Size = new Size(470, 27);
            selector.DropDownStyle = ComboBoxStyle.DropDownList;
            selector.DisplayMember = "nombreUsuario";
            pnlFormulario.Controls.Add(selector);
            selector.SelectedIndexChanged += Seleccionar;
            nuevo.Name = "nuevo";
            nuevo.Text = "Nuevo usuario";
            nuevo.Location = new Point(220, 380);
            nuevo.Size = new Size(180, 34);
            pnlFormulario.Controls.Add(nuevo);
            nuevo.Click += nuevo_Click;
            ResumeLayout(false);
            PerformLayout();
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
        private ComboBox selector;

        private Button nuevo;

    }
}