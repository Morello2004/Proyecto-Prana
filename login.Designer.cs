namespace Prana.Presentacion
{
    partial class login
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(login));
            label1 = new Label();
            label2 = new Label();
            pictureBox1 = new PictureBox();
            iconPictureBox1 = new FontAwesome.Sharp.IconPictureBox();
            txtusuario = new TextBox();
            txtclave = new TextBox();
            label3 = new Label();
            label4 = new Label();
            iconButton1 = new FontAwesome.Sharp.IconButton();
            iconButton2 = new FontAwesome.Sharp.IconButton();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)iconPictureBox1).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.BackColor = Color.White;
            label1.Dock = DockStyle.Left;
            label1.Location = new Point(0, 0);
            label1.Name = "label1";
            label1.Size = new Size(321, 450);
            label1.TabIndex = 0;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.White;
            label2.Font = new Font("Segoe UI", 20.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.ForeColor = Color.Red;
            label2.Location = new Point(32, 354);
            label2.Name = "label2";
            label2.Size = new Size(265, 37);
            label2.TabIndex = 1;
            label2.Text = "SISTEMA DE VENTA";
            label2.Click += label2_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.White;
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(32, 38);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(265, 110);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 2;
            pictureBox1.TabStop = false;
            pictureBox1.Click += pictureBox1_Click;
            // 
            // iconPictureBox1
            // 
            iconPictureBox1.BackColor = SystemColors.HighlightText;
            iconPictureBox1.ForeColor = Color.FromArgb(236, 49, 61);
            iconPictureBox1.IconChar = FontAwesome.Sharp.IconChar.Store;
            iconPictureBox1.IconColor = Color.FromArgb(236, 49, 61);
            iconPictureBox1.IconFont = FontAwesome.Sharp.IconFont.Auto;
            iconPictureBox1.IconSize = 197;
            iconPictureBox1.Location = new Point(68, 154);
            iconPictureBox1.Name = "iconPictureBox1";
            iconPictureBox1.Size = new Size(198, 197);
            iconPictureBox1.TabIndex = 3;
            iconPictureBox1.TabStop = false;
            // 
            // txtusuario
            // 
            txtusuario.Location = new Point(403, 108);
            txtusuario.Name = "txtusuario";
            txtusuario.Size = new Size(351, 23);
            txtusuario.TabIndex = 4;
            txtusuario.TextChanged += textBox1_TextChanged;
            // 
            // txtclave
            // 
            txtclave.Location = new Point(403, 166);
            txtclave.Name = "txtclave";
            txtclave.PasswordChar = '*';
            txtclave.Size = new Size(351, 23);
            txtclave.TabIndex = 5;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(549, 90);
            label3.Name = "label3";
            label3.Size = new Size(47, 15);
            label3.TabIndex = 6;
            label3.Text = "Usuario";
            label3.Click += label3_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(539, 148);
            label4.Name = "label4";
            label4.Size = new Size(67, 15);
            label4.TabIndex = 7;
            label4.Text = "Contraseña";
            // 
            // iconButton1
            // 
            iconButton1.FlatStyle = FlatStyle.Flat;
            iconButton1.IconChar = FontAwesome.Sharp.IconChar.None;
            iconButton1.IconColor = Color.Black;
            iconButton1.IconFont = FontAwesome.Sharp.IconFont.Auto;
            iconButton1.Location = new Point(403, 222);
            iconButton1.Name = "iconButton1";
            iconButton1.Size = new Size(119, 46);
            iconButton1.TabIndex = 10;
            iconButton1.Text = "Ingresar";
            iconButton1.UseVisualStyleBackColor = true;
            iconButton1.Click += iconButton1_Click;
            // 
            // iconButton2
            // 
            iconButton2.FlatStyle = FlatStyle.Flat;
            iconButton2.IconChar = FontAwesome.Sharp.IconChar.None;
            iconButton2.IconColor = Color.Black;
            iconButton2.IconFont = FontAwesome.Sharp.IconFont.Auto;
            iconButton2.Location = new Point(635, 222);
            iconButton2.Name = "iconButton2";
            iconButton2.Size = new Size(119, 46);
            iconButton2.TabIndex = 11;
            iconButton2.Text = "Cancelar";
            iconButton2.UseVisualStyleBackColor = true;
            iconButton2.Click += iconButton2_Click;
            // 
            // login
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(iconButton2);
            Controls.Add(iconButton1);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(txtclave);
            Controls.Add(txtusuario);
            Controls.Add(iconPictureBox1);
            Controls.Add(pictureBox1);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "login";
            Text = "login";
            Load += login_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)iconPictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private PictureBox pictureBox1;
        private FontAwesome.Sharp.IconPictureBox iconPictureBox1;
        private TextBox txtusuario;
        private TextBox txtclave;
        private Label label3;
        private Label label4;
        private FontAwesome.Sharp.IconButton iconButton1;
        private FontAwesome.Sharp.IconButton iconButton2;
    }
}