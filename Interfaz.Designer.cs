namespace Prana.Presentacion
{
    partial class Interfaz
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Interfaz));
            pnlMenuLateral = new Panel();
            panel2 = new Panel();
            btnUsuarios = new FontAwesome.Sharp.IconButton();
            btnAlerta = new FontAwesome.Sharp.IconButton();
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
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // pnlMenuLateral
            // 
            pnlMenuLateral.BackColor = Color.FromArgb(20, 24, 30);
            pnlMenuLateral.Controls.Add(panel2);
            pnlMenuLateral.Controls.Add(btnUsuarios);
            pnlMenuLateral.Controls.Add(btnAlerta);
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
            // 
            // panel2
            // 
            panel2.Location = new Point(228, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(842, 560);
            panel2.TabIndex = 11;
            // 
            // btnUsuarios
            // 
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
            // 
            // btnAlerta
            // 
            btnAlerta.FlatStyle = FlatStyle.Flat;
            btnAlerta.Font = new Font("Arial Black", 11F, FontStyle.Bold);
            btnAlerta.IconChar = FontAwesome.Sharp.IconChar.None;
            btnAlerta.IconColor = Color.Black;
            btnAlerta.IconFont = FontAwesome.Sharp.IconFont.Auto;
            btnAlerta.Location = new Point(12, 449);
            btnAlerta.Name = "btnAlerta";
            btnAlerta.Size = new Size(210, 46);
            btnAlerta.TabIndex = 9;
            btnAlerta.Text = "ALERTA";
            btnAlerta.UseVisualStyleBackColor = true;
            btnAlerta.Click += btnAlerta_Click;
            // 
            // btnEntregas
            // 
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
            // 
            // btnMediosPagos
            // 
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
            // 
            // btnPagos
            // 
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
            // 
            // btnStock
            // 
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
            // 
            // btnClientes
            // 
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
            // 
            // btnNuevaVenta
            // 
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
            // 
            // btnPanelPrincipal
            // 
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
            // 
            // panel1
            // 
            panel1.Controls.Add(pictureBox1);
            panel1.Location = new Point(52, 18);
            panel1.Name = "panel1";
            panel1.Size = new Size(129, 49);
            panel1.TabIndex = 0;
            // 
            // pictureBox1
            // 
            pictureBox1.Dock = DockStyle.Fill;
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(0, 0);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(129, 49);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            pictureBox1.Click += pictureBox1_Click;
            // 
            // pnlContenido
            // 
            pnlContenido.Dock = DockStyle.Left;
            pnlContenido.Location = new Point(230, 0);
            pnlContenido.Name = "pnlContenido";
            pnlContenido.Size = new Size(850, 558);
            pnlContenido.TabIndex = 1;
            // 
            // Interfaz
            // 
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
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlMenuLateral;
        private Panel panel1;
        private PictureBox pictureBox1;
        private FontAwesome.Sharp.IconButton btnPanelPrincipal;
        private FontAwesome.Sharp.IconButton btnNuevaVenta;
        private FontAwesome.Sharp.IconButton btnAlerta;
        private FontAwesome.Sharp.IconButton btnEntregas;
        private FontAwesome.Sharp.IconButton btnMediosPagos;
        private FontAwesome.Sharp.IconButton btnPagos;
        private FontAwesome.Sharp.IconButton btnStock;
        private FontAwesome.Sharp.IconButton btnClientes;
        private FontAwesome.Sharp.IconButton btnUsuarios;
        private Panel panel2;
        private Panel pnlContenido;
    }
}