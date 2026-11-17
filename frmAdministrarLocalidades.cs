using Prana.Entidades;
using Prana.negocio;

namespace Prana.Presentacion;

public sealed class frmAdministrarLocalidades : Form
{
    // comparte las operaciones de localidad con la capa de negocio
    private readonly CN_Localidad negocio = new();
    private readonly DataGridView localidades = new();
    private readonly TextBox nombre = new() { CharacterCasing = CharacterCasing.Upper, MaxLength = 25 };

    // prepara la lista de localidades y las acciones para administrarlas
    public frmAdministrarLocalidades()
    {
        Text = "Administrar localidades";
        Size = new Size(620, 480);
        MinimumSize = new Size(520, 380);
        StartPosition = FormStartPosition.CenterParent;
        Font = new Font("Segoe UI", 10);
        BackColor = Color.FromArgb(21, 30, 41);
        ForeColor = Color.White;

        // distribuye la descripcion la lista el campo de nombre y las acciones
        TableLayoutPanel panel = new() { Dock = DockStyle.Fill, Padding = new Padding(14), ColumnCount = 1, RowCount = 4 };
        panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 32));
        panel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 34));
        panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 44));
        panel.Controls.Add(new Label { Text = "Localidades guardadas (se muestran en mayúsculas). Las inactivas no aparecen al cargar clientes.", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft }, 0, 0);

        // presenta todas las localidades sin permitir editar filas directamente
        localidades.Dock = DockStyle.Fill;
        localidades.AutoGenerateColumns = false;
        localidades.AllowUserToAddRows = false;
        localidades.AllowUserToDeleteRows = false;
        localidades.ReadOnly = true;
        localidades.MultiSelect = false;
        localidades.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        localidades.DefaultCellStyle.ForeColor = Color.Black;
        localidades.DefaultCellStyle.SelectionForeColor = Color.Black;
        localidades.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Localidad.Nombre), HeaderText = "Localidad", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
        localidades.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Localidad.Activa), HeaderText = "Activa", Width = 75 });
        localidades.SelectionChanged += (_, _) => Seleccionar();
        panel.Controls.Add(localidades, 0, 1);

        nombre.Dock = DockStyle.Fill;
        nombre.ForeColor = Color.Black;
        panel.Controls.Add(nombre, 0, 2);

        // agrupa las acciones protegidas de alta renombrado y baja logica
        FlowLayoutPanel acciones = new() { Dock = DockStyle.Fill, FlowDirection = FlowDirection.LeftToRight };
        acciones.Controls.Add(Boton("Agregar", Agregar));
        acciones.Controls.Add(Boton("Renombrar", Renombrar));
        acciones.Controls.Add(Boton("Desactivar / Reactivar", CambiarEstado));
        acciones.Controls.Add(Boton("Cerrar", Close));
        panel.Controls.Add(acciones, 0, 3);
        Controls.Add(panel);
        Load += (_, _) => Ejecutar(() => Cargar());
    }

    private Button Boton(string texto, Action accion)
    {
        Button boton = new() { Text = texto, AutoSize = true, Height = 32, ForeColor = Color.Black, BackColor = Color.Gainsboro };
        boton.Click += (_, _) => Ejecutar(accion);
        return boton;
    }

    // informa los errores de administracion sin ocultarlos
    private void Ejecutar(Action accion)
    {
        try
        {
            accion();
        }
        catch (Exception error)
        {
            MessageBox.Show(this, error.Message, "Administrar localidades", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    // actualiza la lista y conserva la seleccion cuando sigue disponible
    private void Cargar(string? seleccionar = null)
    {
        localidades.DataSource = negocio.ObtenerTodas();
        if (seleccionar != null)
        {
            foreach (DataGridViewRow fila in localidades.Rows)
            {
                if (fila.DataBoundItem is Localidad opcion && opcion.Nombre == seleccionar)
                {
                    fila.Selected = true;
                    localidades.CurrentCell = fila.Cells[0];
                    break;
                }
            }
        }
        Seleccionar();
    }

    // copia el nombre de la fila seleccionada al campo de edicion
    private void Seleccionar()
    {
        if (localidades.CurrentRow?.DataBoundItem is Localidad opcion)
            nombre.Text = opcion.Nombre;
        else
            nombre.Clear();
    }

    // crea una localidad con el nombre escrito en mayusculas
    private void Agregar()
    {
        string nuevo = nombre.Text;
        negocio.Agregar(nuevo);
        Cargar(nuevo.Trim().ToUpperInvariant());
    }

    // confirma el cambio de nombre antes de actualizar la localidad
    private void Renombrar()
    {
        string actual = Seleccionada().Nombre;
        string nuevo = nombre.Text.Trim().ToUpperInvariant();
        if (MessageBox.Show(this, $"¿Cambiar {actual} por {nuevo} en clientes y entregas?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
        negocio.Renombrar(actual, nuevo);
        Cargar(nuevo);
    }

    // solicita confirmacion antes de activar o desactivar la localidad
    private void CambiarEstado()
    {
        Localidad opcion = Seleccionada();
        bool activar = !opcion.Activa;
        string accion = activar ? "activar" : "desactivar";
        if (MessageBox.Show(this, $"¿Confirma {accion} {opcion.Nombre}? Los datos históricos se conservarán.", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
        negocio.CambiarEstado(opcion.Nombre, activar);
        Cargar(opcion.Nombre);
    }

    // requiere una fila valida para ejecutar una accion administrativa
    private Localidad Seleccionada() => localidades.CurrentRow?.DataBoundItem is Localidad fila
        ? fila
        : throw new InvalidOperationException("Seleccione una localidad de la lista.");
}
