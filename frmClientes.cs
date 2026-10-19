using Prana.Entidades;
using Prana.negocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Prana.Presentacion
{
    public partial class frmClientes : Form
    {
        private bool seleccionandoCliente;

        public frmClientes()
        {
            InitializeComponent();
            ConfigurarDistribucion();
            Localidad.Text="Ciudad (opcional)";
            colLocalidad.HeaderText="Ciudad";

            // evita que se creen columnas adicionales automaticamente
            dgv_listadoClientes.AutoGenerateColumns = false;
            txtBuscarCliente.MaxLength = 200;
            // El saldo llega desde negocio y nunca se edita como dato del cliente.
            colDeudaPendiente.DisplayIndex = 3;
            dgv_listadoClientes.Columns["colEstadoCuenta"]!.DisplayIndex = 4;
            dgv_listadoClientes.Columns["colVerDeuda"]!.DisplayIndex = 5;

            sugerenciasClientes.Font = txtBuscarCliente.Font;
            sugerenciasClientes.Visible = false;
            sugerenciasClientes.IntegralHeight = false;
            sugerenciasClientes.HorizontalScrollbar = true;
            txtBuscarCliente.PlaceholderText = "Buscar por nombre completo, DNI o teléfono";

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        //viejo eliminar
        private void btnAgregarVariante_Click(object sender, EventArgs e)
        {
            CN_Cliente negocioCliente = new CN_Cliente();

        }

        private void iconButton1_Click(object sender, EventArgs e)
        {

        }

        private void btn_CrearCliente_Click(object sender, EventArgs e)
        {
            CN_Cliente negocioCliente = new CN_Cliente();

            try
            {
                // crea el cliente con los datos ingresados
                Cliente nuevoCliente = new Cliente
                {
                    Nombre = textBoxNombre.Text.Trim(),
                    Apellido = textBoxApellido.Text.Trim(),
                    DNI = textBoxDNI.Text.Trim(),
                    Telefono = textBoxTelefono.Text.Trim(),
                    Direccion = textBoxDireccion.Text.Trim(),
                    CodigoPostal = txtCodigoPostal.Text.Trim(),

                    Localidad = new Localidad
                    {
                        NombreLocalidad = textBoxLocalidad.Text.Trim()
                    },

                    FechaRegistro = DateTime.Now,
                    SaldoFavor = 0
                };

                // intenta registrar el cliente
                if (!negocioCliente.NuevoCliente(nuevoCliente))
                {
                    MessageBox.Show("No se pudo crear el cliente");
                    return;
                }

                // informa que el cliente fue creado
                MessageBox.Show("Cliente creado correctamente");

                // limpia los datos ingresados
                textBoxNombre.Clear();
                textBoxApellido.Clear();
                textBoxDNI.Clear();
                textBoxTelefono.Clear();
                textBoxDireccion.Clear();
                textBoxLocalidad.Clear();
                txtCodigoPostal.Clear();

                // actualiza el listado
                CargarClientes();
            }
            catch (Exception ex)
            {
                // muestra el error enviado por la capa de negocio
                MessageBox.Show(ex.Message);
            }
        }

        // carga los clientes y coloca modificar en los botones
        private void CargarClientes()
        {
            // crea el objeto de la capa de negocio
            CN_Cliente negocioCliente = new CN_Cliente();

            // elimina los datos anteriores del grid
            dgv_listadoClientes.DataSource = null;

            // carga todos los clientes desde la base de datos
            dgv_listadoClientes.DataSource = negocioCliente.buscarCliente(null);

            // coloca modificar en el boton de cada fila
            foreach (DataGridViewRow fila in dgv_listadoClientes.Rows)
            {
                fila.Cells["colAccion"].Value = "Modificar cliente";
            }
        }

        private void dgv_listadoClientes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // evita ejecutar el codigo al presionar los encabezados
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                return;
            }

            if (dgv_listadoClientes.Columns[e.ColumnIndex].Name == "colVerDeuda")
            {
                if (dgv_listadoClientes.Rows[e.RowIndex].DataBoundItem is Cliente cliente)
                {
                    using var detalle = new frmDeudaCliente(cliente);
                    detalle.ShowDialog(this);
                }
                return;
            }

            // comprueba que se haya presionado la columna accion
            if (dgv_listadoClientes.Columns[e.ColumnIndex].Name != "colAccion")
            {
                return;
            }

            // obtiene la fila donde se presiono el boton
            DataGridViewRow filaSeleccionada = dgv_listadoClientes.Rows[e.RowIndex];

            // obtiene el cliente relacionado con la fila
            Cliente? clienteSeleccionado = filaSeleccionada.DataBoundItem as Cliente;

            // comprueba que la fila tenga un cliente
            if (clienteSeleccionado == null)
            {
                MessageBox.Show("No se pudo obtener el cliente seleccionado");
                return;
            }

            // obtiene el texto actual del boton
            string textoBoton = filaSeleccionada.Cells["colAccion"].Value?.ToString() ?? "Modificar cliente";

            // comprueba si se debe habilitar la modificacion
            if (textoBoton == "Modificar cliente")
            {
                // habilita solamente los datos que pueden modificarse
                filaSeleccionada.Cells["colNombre"].ReadOnly = false;
                filaSeleccionada.Cells["colApellido"].ReadOnly = false;
                filaSeleccionada.Cells["colDNI"].ReadOnly = false;
                filaSeleccionada.Cells["ColTelefono"].ReadOnly = false;
                filaSeleccionada.Cells["colDireccion"].ReadOnly = false;
                filaSeleccionada.Cells["colLocalidad"].ReadOnly = false;
                filaSeleccionada.Cells["colCodigoPostal"].ReadOnly = false;

                // cambia el texto del boton de esta fila
                filaSeleccionada.Cells["colAccion"].Value = "Guardar cambios";

                // coloca el cursor en el nombre
                dgv_listadoClientes.CurrentCell = filaSeleccionada.Cells["colNombre"];

                // permite comenzar a escribir
                dgv_listadoClientes.BeginEdit(true);

                return;
            }

            try
            {
                // confirma el ultimo valor escrito
                dgv_listadoClientes.EndEdit();

                // quita el foco para terminar cualquier edicion pendiente
                dgv_listadoClientes.CurrentCell = null;

                // copia los datos de la fila al cliente
                clienteSeleccionado.Nombre = filaSeleccionada.Cells["colNombre"].Value?.ToString()?.Trim() ?? string.Empty;
                clienteSeleccionado.Apellido = filaSeleccionada.Cells["colApellido"].Value?.ToString()?.Trim() ?? string.Empty;
                clienteSeleccionado.DNI = filaSeleccionada.Cells["colDNI"].Value?.ToString()?.Trim() ?? string.Empty;
                clienteSeleccionado.Telefono = filaSeleccionada.Cells["ColTelefono"].Value?.ToString()?.Trim() ?? string.Empty;
                clienteSeleccionado.Direccion = filaSeleccionada.Cells["colDireccion"].Value?.ToString()?.Trim() ?? string.Empty;

                clienteSeleccionado.CodigoPostal = filaSeleccionada.Cells["colCodigoPostal"].Value?.ToString()?.Trim() ?? string.Empty;
                // guarda la localidad escrita en la fila
                clienteSeleccionado.Localidad = new Localidad
                {
                    NombreLocalidad = filaSeleccionada.Cells["colLocalidad"].Value?.ToString()?.Trim() ?? string.Empty
                };

                // crea el objeto de la capa de negocio
                CN_Cliente negocioCliente = new CN_Cliente();

                // valida y guarda la modificacion
                if (!negocioCliente.ModificarCliente(clienteSeleccionado))
                {
                    MessageBox.Show("No se pudo modificar el cliente");
                    return;
                }

                // vuelve a cargar los datos guardados
                CargarClientes();

                // informa que la modificacion se realizo
                MessageBox.Show("Cliente modificado correctamente");
            }
            catch (Exception ex)
            {
                // muestra el error y mantiene la fila en edicion
                MessageBox.Show(ex.Message);
            }
        }

        // busca clientes por nombre apellido dni o telefono
        private void btnBuscarClientes_Click(object sender, EventArgs e)
        {
            OcultarSugerencias();
            try
            {
                // obtiene el texto ingresado
                string busqueda = txtBuscarCliente.Text.Trim();

                // crea el objeto de negocio
                // carga los resultados encontrados
                dgv_listadoClientes.DataSource = null;
                var encontrados = BuscarClientes(busqueda);
                dgv_listadoClientes.DataSource = encontrados;

                // coloca modificar en cada boton
                foreach (DataGridViewRow fila in dgv_listadoClientes.Rows)
                {
                    fila.Cells["colAccion"].Value = "Modificar cliente";
                }

                // informa cuando no se encontraron clientes
                if (encontrados.Count == 0)
                {
                    MessageBox.Show("No se encontraron clientes");
                }
            }
            catch (Exception ex)
            {
                // muestra el error producido
                MessageBox.Show(ex.Message);
            }
        }

        private void txtBuscarCliente_TextChanged(object sender, EventArgs e)
        {
            OcultarSugerencias();
            if (!seleccionandoCliente && txtBuscarCliente.Text.Trim().Length >= 2)
                esperaBusqueda.Start();
        }

        private static List<Cliente> BuscarClientes(string texto)
        {
            // La búsqueda completa y la consulta de deuda pertenecen a las capas inferiores.
            return new CN_Cliente().buscarCliente(texto);
        }

        private void OcultarSugerencias()
        {
            esperaBusqueda.Stop();
            sugerenciasClientes.Visible = false;
        }

        private void ActualizarSugerencias()
        {
            esperaBusqueda.Stop();
            if (!txtBuscarCliente.Focused) return;
            try
            {
                sugerenciasClientes.Items.Clear();
                foreach (var cliente in BuscarClientes(txtBuscarCliente.Text).Take(15))
                    sugerenciasClientes.Items.Add(new OpcionCliente(cliente));
                sugerenciasClientes.Location = PointToClient(txtBuscarCliente.PointToScreen(new Point(0, txtBuscarCliente.Height + 2)));
                sugerenciasClientes.Size = new Size(txtBuscarCliente.Width,
                    Math.Min(120, sugerenciasClientes.Items.Count * sugerenciasClientes.ItemHeight + 4));
                sugerenciasClientes.Visible = sugerenciasClientes.Items.Count > 0;
                sugerenciasClientes.BringToFront();
            }
            catch (Exception ex)
            {
                OcultarSugerencias();
                MessageBox.Show(ex.Message);
            }
        }

        private void ElegirSugerencia()
        {
            if (sugerenciasClientes.SelectedItem is not OpcionCliente opcion) return;
            OcultarSugerencias();
            seleccionandoCliente = true;
            txtBuscarCliente.Text = $"{opcion.Cliente.Nombre} {opcion.Cliente.Apellido}";
            seleccionandoCliente = false;
            dgv_listadoClientes.DataSource = new List<Cliente> { opcion.Cliente };
            foreach (DataGridViewRow fila in dgv_listadoClientes.Rows)
                fila.Cells["colAccion"].Value = "Modificar cliente";
            txtBuscarCliente.Focus();
            txtBuscarCliente.SelectionStart = txtBuscarCliente.TextLength;
        }

        private void Buscador_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                OcultarSugerencias();
            else if (sugerenciasClientes.Visible && e.KeyCode == Keys.Down)
                sugerenciasClientes.SelectedIndex = Math.Min(sugerenciasClientes.SelectedIndex + 1, sugerenciasClientes.Items.Count - 1);
            else if (sugerenciasClientes.Visible && e.KeyCode == Keys.Up)
                sugerenciasClientes.SelectedIndex = Math.Max(sugerenciasClientes.SelectedIndex - 1, 0);
            else if (e.KeyCode == Keys.Enter)
            {
                if (sugerenciasClientes.Visible && sugerenciasClientes.SelectedIndex >= 0) ElegirSugerencia();
                else btnBuscarClientes_Click(sender!, EventArgs.Empty);
            }
            else return;
            e.SuppressKeyPress = true;
        }

        private sealed class OpcionCliente
        {
            public Cliente Cliente { get; }
            public OpcionCliente(Cliente cliente) => Cliente = cliente;
            public override string ToString() => $"{Cliente.Nombre} {Cliente.Apellido} — DNI: {Cliente.DNI} — {Cliente.EstadoCuenta}";
        }
        private void dgv_listadoClientes_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e) {
                if (e.ColumnIndex == colDeudaPendiente.Index && e.Value is decimal saldo && saldo > 0)
                    e.CellStyle!.ForeColor = Color.FromArgb(165, 72, 0);
            }

        private void frmClientes_Load(object? sender, EventArgs e) {
                try { CargarClientes(); }
                catch (Exception ex) { MessageBox.Show(ex.Message, "Consulta de clientes"); }
            }

        private void esperaBusqueda_Tick(object? sender, EventArgs e) { ActualizarSugerencias(); }

        private void sugerenciasClientes_Click(object? sender, EventArgs e) { ElegirSugerencia(); }

        private void txtBuscarCliente_Leave(object? sender, EventArgs e) { BeginInvoke(new Action(() =>
            {
                if (!sugerenciasClientes.ContainsFocus) OcultarSugerencias();
            })); }

        private void sugerenciasClientes_Leave(object? sender, EventArgs e) { OcultarSugerencias(); }

        private void frmClientes_Disposed(object? sender, EventArgs e) { esperaBusqueda.Dispose(); }

    }
}
