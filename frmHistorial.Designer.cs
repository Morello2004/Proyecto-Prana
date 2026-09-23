namespace Prana.Presentacion
{
    partial class frmHistorial
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
            pnlEncabezado = new Panel();
            lblTitulo = new Label();
            pnlListado = new Panel();
            lblListado = new Label();
            pnlFiltros = new Panel();
            lblDesde = new Label();
            dtpDesde = new DateTimePicker();
            lblHasta = new Label();
            dtpHasta = new DateTimePicker();
            btnFiltrar = new Button();
            btnUltimas = new Button();
            dgvActividades = new DataGridView();
            pnlFiltros.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvActividades).BeginInit();
            SuspendLayout();
            // 
            // pnlFiltros
            // 
            pnlFiltros.Controls.Add(lblDesde);
            pnlFiltros.Controls.Add(dtpDesde);
            pnlFiltros.Controls.Add(lblHasta);
            pnlFiltros.Controls.Add(dtpHasta);
            pnlFiltros.Controls.Add(btnFiltrar);
            pnlFiltros.Controls.Add(btnUltimas);
            pnlFiltros.Dock = DockStyle.Top;
            pnlFiltros.Location = new Point(0, 0);
            pnlFiltros.Name = "pnlFiltros";
            pnlFiltros.Size = new Size(800, 42);
            pnlFiltros.TabIndex = 0;
            // 
            // lblDesde
            // 
            lblDesde.AutoSize = true;
            lblDesde.Location = new Point(8, 13);
            lblDesde.Name = "lblDesde";
            lblDesde.Size = new Size(36, 15);
            lblDesde.TabIndex = 1;
            lblDesde.Text = "Desde";
            // 
            // dtpDesde
            // 
            dtpDesde.Format = DateTimePickerFormat.Short;
            dtpDesde.Location = new Point(50, 8);
            dtpDesde.Name = "dtpDesde";
            dtpDesde.Size = new Size(100, 23);
            dtpDesde.TabIndex = 2;
            dtpDesde.Value = DateTime.Today.AddMonths(-1);
            // 
            // lblHasta
            // 
            lblHasta.AutoSize = true;
            lblHasta.Location = new Point(160, 13);
            lblHasta.Name = "lblHasta";
            lblHasta.Size = new Size(36, 15);
            lblHasta.TabIndex = 3;
            lblHasta.Text = "Hasta";
            // 
            // dtpHasta
            // 
            dtpHasta.Format = DateTimePickerFormat.Short;
            dtpHasta.Location = new Point(202, 8);
            dtpHasta.Name = "dtpHasta";
            dtpHasta.Size = new Size(100, 23);
            dtpHasta.TabIndex = 4;
            dtpHasta.Value = DateTime.Today;
            // 
            // btnFiltrar
            // 
            btnFiltrar.Location = new Point(312, 7);
            btnFiltrar.Name = "btnFiltrar";
            btnFiltrar.Size = new Size(70, 27);
            btnFiltrar.TabIndex = 5;
            btnFiltrar.Text = "Filtrar";
            btnFiltrar.UseVisualStyleBackColor = true;
            btnFiltrar.Click += btnFiltrar_Click;
            // 
            // btnUltimas
            // 
            btnUltimas.Location = new Point(390, 7);
            btnUltimas.Name = "btnUltimas";
            btnUltimas.Size = new Size(90, 27);
            btnUltimas.TabIndex = 6;
            btnUltimas.Text = "Últimas 10";
            btnUltimas.UseVisualStyleBackColor = true;
            btnUltimas.Click += btnUltimas_Click;
            // 
            // dgvActividades
            // 
            dgvActividades.AllowUserToAddRows = false;
            dgvActividades.AutoGenerateColumns = true;
            dgvActividades.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            dgvActividades.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvActividades.Dock = DockStyle.Fill;
            dgvActividades.Location = new Point(0, 42);
            dgvActividades.MultiSelect = false;
            dgvActividades.Name = "dgvActividades";
            dgvActividades.ReadOnly = true;
            dgvActividades.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvActividades.Size = new Size(800, 408);
            dgvActividades.TabIndex = 8;
            dgvActividades.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            // 
            // frmHistorial
            // 
            BackColor = Color.FromArgb(13, 19, 27);
            ForeColor = Color.White;
            AutoScroll = true;
            AutoScrollMinSize = new Size(780, 540);
            pnlEncabezado.Dock = DockStyle.Top;
            pnlEncabezado.Height = 76;
            pnlEncabezado.BackColor = Color.FromArgb(21, 30, 41);
            pnlEncabezado.Controls.Add(lblTitulo);
            lblTitulo.Text = "Historial";
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTitulo.Location = new Point(20, 20);
            pnlFiltros.Height = 100;
            pnlFiltros.BackColor = Color.FromArgb(21, 30, 41);
            lblDesde.Location = new Point(20, 15);
            lblDesde.ForeColor = Color.FromArgb(145, 170, 210);
            dtpDesde.Location = new Point(20, 40);
            dtpDesde.Size = new Size(155, 27);
            dtpDesde.Font = new Font("Segoe UI", 10F);
            lblHasta.Location = new Point(195, 15);
            lblHasta.ForeColor = Color.FromArgb(145, 170, 210);
            dtpHasta.Location = new Point(195, 40);
            dtpHasta.Size = new Size(155, 27);
            dtpHasta.Font = new Font("Segoe UI", 10F);
            btnFiltrar.Location = new Point(374, 36);
            btnFiltrar.Width = 110;
            btnUltimas.Location = new Point(496, 36);
            btnUltimas.Width = 130;
            pnlListado.Dock = DockStyle.Fill;
            pnlListado.Padding = new Padding(16);
            pnlListado.Controls.Add(dgvActividades);
            pnlListado.Controls.Add(lblListado);
            lblListado.Text = "Actividades registradas";
            lblListado.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblListado.Dock = DockStyle.Top;
            lblListado.Height = 40;
            btnFiltrar.BackColor = Color.DodgerBlue;
            btnFiltrar.ForeColor = Color.White;
            btnFiltrar.FlatStyle = FlatStyle.Flat;
            btnFiltrar.FlatAppearance.BorderSize = 0;
            btnFiltrar.UseVisualStyleBackColor = false;
            btnFiltrar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnFiltrar.Height = 34;
            btnUltimas.BackColor = Color.SteelBlue;
            btnUltimas.ForeColor = Color.White;
            btnUltimas.FlatStyle = FlatStyle.Flat;
            btnUltimas.FlatAppearance.BorderSize = 0;
            btnUltimas.UseVisualStyleBackColor = false;
            btnUltimas.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnUltimas.Height = 34;
            dgvActividades.BackgroundColor = Color.FromArgb(13, 19, 27);
            dgvActividades.BorderStyle = BorderStyle.None;
            dgvActividades.RowHeadersVisible = false;
            dgvActividades.AllowUserToDeleteRows = false;
            dgvActividades.EnableHeadersVisualStyles = false;
            dgvActividades.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(37, 52, 70);
            dgvActividades.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvActividades.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgvActividades.ColumnHeadersHeight = 38;
            dgvActividades.RowTemplate.Height = 32;
            dgvActividades.DefaultCellStyle.Padding = new Padding(5);
            dgvActividades.DefaultCellStyle.BackColor = Color.White;
            dgvActividades.DefaultCellStyle.ForeColor = Color.FromArgb(21, 30, 41);
            dgvActividades.DefaultCellStyle.SelectionBackColor = Color.FromArgb(35, 100, 166);
            dgvActividades.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvActividades.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(236, 242, 249);

            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(pnlListado);
            Controls.Add(pnlFiltros);
            Controls.Add(pnlEncabezado);
            Name = "frmHistorial";
            Text = "frmHistorial";
            pnlFiltros.ResumeLayout(false);
            pnlFiltros.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvActividades).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlEncabezado, pnlListado;
        private Label lblTitulo, lblListado;
        private Panel pnlFiltros;
        private Label lblDesde;
        private DateTimePicker dtpDesde;
        private Label lblHasta;
        private DateTimePicker dtpHasta;
        private Button btnFiltrar;
        private Button btnUltimas;
        private DataGridView dgvActividades;
    }
}