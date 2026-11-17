using Prana.Entidades;
using Prana.negocio;

namespace Prana.Presentacion;

// administra nombres compartidos desde una ventana modal exclusiva del administrador
public class frmOpcionesStock : Form
{
    private readonly CN_OpcionStock negocio = new CN_OpcionStock();
    private readonly ComboBox cboTipo = new ComboBox();
    private readonly DataGridView dgvOpciones = new DataGridView();
    private readonly TextBox txtNombre = new TextBox { CharacterCasing = CharacterCasing.Upper };
    private readonly ComboBox cboDestino = new ComboBox();
    private readonly Label lblEstado = new Label();

    public frmOpcionesStock()
    {
        Text = "Administrar opciones de stock";
        Size = new Size(820, 600);
        MinimumSize = new Size(700, 540);
        StartPosition = FormStartPosition.CenterParent;
        Font = new Font("Segoe UI", 10);
        BackColor = Color.FromArgb(21, 30, 41);
        ForeColor = Color.White;
        TableLayoutPanel panel = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(14), ColumnCount = 1, RowCount = 7 };
        panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 38));
        panel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
        panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));
        panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
        panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 42));
        panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 36));
        cboTipo.DropDownStyle = ComboBoxStyle.DropDownList;
        cboTipo.ForeColor = Color.Black;
        cboTipo.DataSource = Enum.GetValues<TipoOpcionStock>();
        cboTipo.Width = 240;
        panel.Controls.Add(cboTipo, 0, 0);
        // presenta solamente el nombre y los registros vinculados sin permitir editar directamente las filas
        dgvOpciones.Dock = DockStyle.Fill;
        dgvOpciones.AutoGenerateColumns = false;
        dgvOpciones.AllowUserToAddRows = false;
        dgvOpciones.AllowUserToDeleteRows = false;
        dgvOpciones.ReadOnly = true;
        dgvOpciones.MultiSelect = false;
        dgvOpciones.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvOpciones.DefaultCellStyle.ForeColor = Color.Black;
        dgvOpciones.DefaultCellStyle.BackColor = Color.White;
        dgvOpciones.DefaultCellStyle.SelectionForeColor = Color.Black;
        dgvOpciones.DefaultCellStyle.SelectionBackColor = Color.FromArgb(190, 220, 250);
        dgvOpciones.EnableHeadersVisualStyles = false;
        dgvOpciones.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
        dgvOpciones.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(220, 230, 240);
        dgvOpciones.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Nombre", HeaderText = "Nombre", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
        dgvOpciones.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Usos", HeaderText = "Registros vinculados", Width = 170 });
        panel.Controls.Add(dgvOpciones, 0, 1);
        panel.Controls.Add(new Label { Text = "Corregir el nombre de la opción seleccionada", AutoSize = true }, 0, 2);
        FlowLayoutPanel correccion = new FlowLayoutPanel { Dock = DockStyle.Fill };
        txtNombre.Width = 330;
        txtNombre.ForeColor = Color.Black;
        correccion.Controls.Add(txtNombre);
        correccion.Controls.Add(Boton("Guardar nombre", () => Ejecutar(false, false)));
        correccion.Controls.Add(Boton("Eliminar", () => Ejecutar(true, false)));
        panel.Controls.Add(correccion, 0, 3);
        panel.Controls.Add(new Label { Text = "Unificar: pasar los registros vinculados a otra opción existente", AutoSize = true }, 0, 4);
        FlowLayoutPanel unificacion = new FlowLayoutPanel { Dock = DockStyle.Fill };
        cboDestino.DropDownStyle = ComboBoxStyle.DropDownList;
        cboDestino.ForeColor = Color.Black;
        cboDestino.Width = 330;
        cboDestino.DisplayMember = "Nombre";
        unificacion.Controls.Add(cboDestino);
        unificacion.Controls.Add(Boton("Unificar", () => Ejecutar(false, true)));
        unificacion.Controls.Add(Boton("Cerrar", Close));
        panel.Controls.Add(unificacion, 0, 5);
        lblEstado.Dock = DockStyle.Fill;
        panel.Controls.Add(lblEstado, 0, 6);
        Controls.Add(panel);
        cboTipo.SelectedIndexChanged += (sender, e) => Cargar();
        dgvOpciones.SelectionChanged += (sender, e) => Seleccionar();
        Load += (sender, e) => Cargar();
    }

    // conecta botones nativos con las operaciones del formulario
    private Button Boton(string texto, Action accion)
    {
        Button boton = new Button { Text = texto, AutoSize = true, Height = 30, ForeColor = Color.Black, BackColor = Color.Gainsboro };
        boton.Click += (sender, e) => accion();
        return boton;
    }

    private TipoOpcionStock Tipo => (TipoOpcionStock)cboTipo.SelectedItem!;

    // vuelve a consultar despues de cada cambio para no conservar nombres eliminados
    private void Cargar()
    {
        try
        {
            List<OpcionStock> opciones = negocio.Obtener(Tipo);
            dgvOpciones.DataSource = opciones;
            txtNombre.MaxLength = 100;
            if (Tipo == TipoOpcionStock.Color) txtNombre.MaxLength = 50;
            Seleccionar();
            lblEstado.Text = "Eliminar solo admite opciones sin uso; corregir conserva sus relaciones.";
        }
        catch (Exception error) { MessageBox.Show(this, error.Message, "Opciones de stock", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
    }

    // excluye la opcion de origen del destino para no unificarla consigo misma
    private void Seleccionar()
    {
        if (dgvOpciones.CurrentRow?.DataBoundItem is OpcionStock opcion)
        {
            txtNombre.Text = opcion.Nombre;
            List<OpcionStock> opciones = (List<OpcionStock>)dgvOpciones.DataSource;
            cboDestino.DataSource = opciones.Where(item => !ReferenceEquals(item, opcion)).ToList();
            cboDestino.SelectedIndex = -1;
        }
        else { txtNombre.Clear(); cboDestino.DataSource = null; }
    }

    // solicita confirmacion sobre la opcion concreta antes de cambiar sus referencias
    private void Ejecutar(bool eliminar, bool unificar)
    {
        try
        {
            Permisos.Exigir(Permisos.AdministrarOpcionesStock);
            if (dgvOpciones.CurrentRow?.DataBoundItem is not OpcionStock opcion) throw new InvalidOperationException("Seleccione una opción del listado.");
            int? destino = null;
            string nombre = txtNombre.Text;
            string mensaje = "¿Guardar el nombre de " + opcion.Nombre + " como " + nombre + "?";
            if (eliminar) mensaje = "¿Eliminar " + opcion.Nombre + "? Solo se permite si no tiene registros vinculados.";
            if (unificar)
            {
                if (cboDestino.SelectedItem is not OpcionStock elegida) throw new InvalidOperationException("Seleccione la opción destino para unificar.");
                nombre = elegida.Nombre;
                if (Tipo != TipoOpcionStock.Color) destino = elegida.Id;
                mensaje = "¿Unificar " + opcion.Nombre + " con " + nombre + "? Los registros vinculados pasarán al destino y el nombre anterior dejará de estar disponible.";
            }
            if (MessageBox.Show(this, mensaje, "Confirmar cambio", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
            negocio.Guardar(Tipo, opcion, nombre, destino, eliminar);
            Cargar();
            lblEstado.Text = "Cambio guardado correctamente.";
        }
        catch (Exception error) { MessageBox.Show(this, error.Message, "Opciones de stock", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
    }
}
