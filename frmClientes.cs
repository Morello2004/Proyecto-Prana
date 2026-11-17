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
    // agrupa los datos y operaciones de frm clientes
    public partial class frmClientes : Form
    {
        // conserva seleccionando cliente entre las operaciones de esta clase
        private bool seleccionandoCliente;
        // prepara frm clientes antes de que se utilice la instancia
        public frmClientes()
        {
            InitializeComponent();
            // evita que se creen columnas adicionales automaticamente dgv_listadoclientesautogeneratecolumns: decide si el enlace de datos crea columnas automaticamente; desactivarlo conserva las columnas definidas
            dgv_listadoClientes.AutoGenerateColumns = false;
            cboBuscarCliente.MaxLength = 201;
            cboBuscarCliente.DropDownStyle = ComboBoxStyle.DropDown;
            cboBuscarCliente.AutoCompleteMode = AutoCompleteMode.None;
            cboBuscarCliente.AutoCompleteSource = AutoCompleteSource.None;
            // habilita sugerencias y autocompletado para las localidades activas
            cboLocalidadCliente.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboLocalidadCliente.AutoCompleteSource = AutoCompleteSource.ListItems;
            // muestra la administracion solo a usuarios con permiso
            btnAdministrarLocalidades.Visible = SesionActual.Usuario != null && Permisos.TienePermiso(Permisos.AdministrarLocalidades);
            // el saldo llega desde negocio y nunca se edita como dato del cliente
            colDeudaPendiente.DisplayIndex = 3;
            dgv_listadoClientes.Columns["colEstadoCuenta"]!.DisplayIndex = 4;
            dgv_listadoClientes.Columns["colVerDeuda"]!.DisplayIndex = 5;
        }

        private void dgv_listadoClientes_EditingControlShowing(object? sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control is TextBox texto)
                texto.CharacterCasing = CharacterCasing.Upper;
        }

        // limita el ingreso segun el tipo sin recortar un documento ya escrito actualiza los controles relacionados cuando cambia la seleccion puede dispararse tambien al cargar los datos
        private void cboTipoDocumento_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (cboTipoDocumento.Text == "CUIT")
            {
                txtDocumentoCliente.MaxLength = 11;
            }
            else
            {
                txtDocumentoCliente.MaxLength = 8;
            }
        }

        // ejecuta la accion de label10 cuando el usuario pulsa el control
        private void label10_Click(object sender, EventArgs e)
        {
        }

        // ejecuta la accion de label2 cuando el usuario pulsa el control
        private void label2_Click(object sender, EventArgs e)
        {
        }

        // viejo eliminar atiende la pulsacion de btnagregarvariante y ejecuta la accion asociada el evento no significa que la operacion ya se haya guardado
        private void btnAgregarVariante_Click(object sender, EventArgs e)
        {
            CN_Cliente negocioCliente = new CN_Cliente();
        }

        // ejecuta la accion de icon button1 cuando el usuario pulsa el control
        private void iconButton1_Click(object sender, EventArgs e)
        {
        }

        // ejecuta la accion de btn crear cliente cuando el usuario pulsa el control
        private void btn_CrearCliente_Click(object sender, EventArgs e)
        {
            CN_Cliente negocioCliente = new CN_Cliente();
            try
            {
                // crea el cliente con los datos ingresados
                Cliente nuevoCliente = ObtenerClienteDelFormulario();
                // intenta registrar el cliente
                if (!negocioCliente.NuevoCliente(nuevoCliente))
                {
                    MessageBox.Show("No se pudo crear el cliente");
                    return;
                }

                // informa que el cliente fue creado
                MessageBox.Show("Cliente creado correctamente");
                // limpia los datos ingresados vacia la coleccion o campo indicado revisar si es una carga temporal antes de reutilizarlo
                txtNombreCliente.Clear();
                txtApellidoCliente.Clear();
                txtDocumentoCliente.Clear();
                txtTelefonoCliente.Clear();
                txtAreaCliente.Clear();
                txtDireccionCliente.Clear();
                cboLocalidadCliente.SelectedIndex = -1;
                cboLocalidadCliente.Text = string.Empty;
                CargarOpcionesLocalidad();
                // actualiza el listado
                CargarClientes();
            }
            // atiende el error de esta operacion antes de informar o propagar el problema
            catch (Exception error)
            {
                // muestra el error enviado por la capa de negocio
                MessageBox.Show(error.Message);
            }
        }

        // carga el listado aplicando el filtro seleccionado en el disenador
        private void CargarClientes(string? busqueda = null)
        {
            CN_Cliente negocioCliente = new CN_Cliente();
            dgv_listadoClientes.DataSource = null;
            dgv_listadoClientes.DataSource = negocioCliente.buscarCliente(busqueda, Math.Max(cboFiltroEstadoCliente.SelectedIndex, 0));
            // procesa cada fila de dgv listado clientesrows sin cambiar el orden de la coleccion
            foreach (DataGridViewRow fila in dgv_listadoClientes.Rows)
            {
                // filacells["colaccion"]value: valor actual del control numerico o de fecha
                fila.Cells["colAccion"].Value = "Modificar cliente";
            }

            ActualizarBotonEstadoCliente();
        }

        // resuelve actualizar boton estado cliente con los datos recibidos y entrega el resultado al codigo que lo llama
        private void ActualizarBotonEstadoCliente()
        {
            Cliente? cliente = dgv_listadoClientes.CurrentRow?.DataBoundItem as Cliente;
            bool puedeDarDeBaja = SesionActual.Usuario != null && Permisos.TienePermiso(Permisos.DarDeBajaClientes);
            btnCambiarEstadoCliente.Enabled = cliente != null && (cliente.Eliminado || !cliente.Activo || puedeDarDeBaja);
            if (cliente == null)
            {
                btnCambiarEstadoCliente.Text = "Cambiar estado";
            }
            else
            {
                if (cliente.Eliminado)
                {
                    btnCambiarEstadoCliente.Text = "Restaurar";
                }
                else
                {
                    if (cliente.Activo)
                    {
                        btnCambiarEstadoCliente.Text = "Dar de baja";
                    }
                    else
                    {
                        btnCambiarEstadoCliente.Text = "Activar";
                    }
                }
            }
        }

        // actualiza los controles relacionados cuando cambia la seleccion puede dispararse tambien al cargar los datos
        private void cboFiltroEstadoCliente_SelectedIndexChanged(object? sender, EventArgs e)
        {
            try
            {
                CargarClientes(cboBuscarCliente.Text.Trim());
            }
            // atiende el error de esta operacion antes de informar o propagar el problema
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "Consulta de clientes", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // actualiza los controles relacionados cuando cambia la seleccion puede dispararse tambien al cargar los datos
        private void dgv_listadoClientes_SelectionChanged(object? sender, EventArgs e) => ActualizarBotonEstadoCliente();
        // ejecuta la accion de btn cambiar estado cliente cuando el usuario pulsa el control
        private void btnCambiarEstadoCliente_Click(object? sender, EventArgs e)
        {
            if (dgv_listadoClientes.CurrentRow?.DataBoundItem is not Cliente cliente)
            {
                return;
            }

            string accion;
            if (cliente.Eliminado)
            {
                accion = "restaurar";
            }
            else
            {
                if (!cliente.Activo)
                {
                    accion = "activar";
                }
                else
                {
                    accion = "dar de baja";
                }
            }
            if (MessageBox.Show(this, $"¿Confirma {accion} a {cliente.Nombre} {cliente.Apellido}?", "Estado del cliente", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            try
            {
                CN_Cliente negocioCliente = new CN_Cliente();
                if (cliente.Eliminado)
                {
                    negocioCliente.RestaurarCliente(cliente.IdCliente);
                }
                else
                    if (!cliente.Activo)
                    {
                        negocioCliente.ActivarCliente(cliente.IdCliente);
                    }
                    else
                    {
                        negocioCliente.DarDeBajaCliente(cliente.IdCliente);
                    }

                CargarClientes(cboBuscarCliente.Text.Trim());
            }
            // atiende el error de esta operacion antes de informar o propagar el problema
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "Estado del cliente", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // identifica la fila y la columna pulsadas antes de ejecutar la accion de la celda
        private void dgv_listadoClientes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // evita ejecutar el codigo al presionar los encabezados
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                return;
            }

            if (dgv_listadoClientes.Columns[e.ColumnIndex].Name == "colVerDeuda")
            {
                // comprueba que el clic corresponda a una fila de datos y no al encabezado
                if (dgv_listadoClientes.Rows[e.RowIndex].DataBoundItem is Cliente cliente)
                {
                    using frmDeudaCliente detalle = new frmDeudaCliente(cliente);
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

            if (clienteSeleccionado.Eliminado)
            {
                MessageBox.Show("Restaure el cliente antes de modificar sus datos.");
                return;
            }

            // obtiene el texto actual del boton
            string textoBoton = filaSeleccionada.Cells["colAccion"].Value?.ToString() ?? "Modificar cliente";
            // comprueba si se debe habilitar la modificacion
            if (textoBoton == "Modificar cliente")
            {
                // habilita solamente los datos que pueden modificarse filaseleccionadacells["colnombre"]readonly: impide editar el contenido desde este control sin impedir su consulta
                filaSeleccionada.Cells["colNombre"].ReadOnly = false;
                // filaseleccionadacells["colapellido"]readonly: impide editar el contenido desde este control sin impedir su consulta
                filaSeleccionada.Cells["colApellido"].ReadOnly = false;
                // filaseleccionadacells["coldni"]readonly: impide editar el contenido desde este control sin impedir su consulta
                filaSeleccionada.Cells["colDNI"].ReadOnly = false;
                // filaseleccionadacells["coltipodocumento"]readonly: impide editar el contenido desde este control sin impedir su consulta
                filaSeleccionada.Cells["colTipoDocumento"].ReadOnly = false;
                // filaseleccionadacells["coltelefono"]readonly: impide editar el contenido desde este control sin impedir su consulta
                filaSeleccionada.Cells["ColTelefono"].ReadOnly = false;
                // filaseleccionadacells["coldireccion"]readonly: impide editar el contenido desde este control sin impedir su consulta
                filaSeleccionada.Cells["colDireccion"].ReadOnly = false;
                // filaseleccionadacells["collocalidad"]readonly: impide editar el contenido desde este control sin impedir su consulta
                filaSeleccionada.Cells["colLocalidad"].ReadOnly = false;
                // cambia el texto del boton de esta fila filaseleccionadacells["colaccion"]value: valor actual del control numerico o de fecha
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
                clienteSeleccionado.TipoDocumento = filaSeleccionada.Cells["colTipoDocumento"].Value?.ToString() ?? "";
                clienteSeleccionado.DNI = filaSeleccionada.Cells["colDNI"].Value?.ToString()?.Trim() ?? string.Empty;
                clienteSeleccionado.Telefono = filaSeleccionada.Cells["ColTelefono"].Value?.ToString()?.Trim() ?? string.Empty;
                clienteSeleccionado.Direccion = filaSeleccionada.Cells["colDireccion"].Value?.ToString()?.Trim() ?? string.Empty;
                // guarda la localidad escrita en la fila
                clienteSeleccionado.NombreLocalidad = filaSeleccionada.Cells["colLocalidad"].Value?.ToString()?.Trim() ?? string.Empty;
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
            // atiende el error de esta operacion antes de informar o propagar el problema
            catch (Exception error)
            {
                // muestra el error y mantiene la fila en edicion
                MessageBox.Show(error.Message);
            }
        }

        // busca clientes por nombre apellido dni o telefono atiende la pulsacion de btnbuscarclientes y ejecuta la accion asociada el evento no significa que la operacion ya se haya guardado
        private void btnBuscarClientes_Click(object sender, EventArgs e)
        {
            OcultarSugerencias();
            try
            {
                // obtiene el texto ingresado
                string busqueda = cboBuscarCliente.Text.Trim();
                List<Cliente> encontrados = BuscarClientes(busqueda, Math.Max(cboFiltroEstadoCliente.SelectedIndex, 0));
                MostrarClientes(encontrados);
                // informa cuando no se encontraron clientes
                if (encontrados.Count == 0)
                {
                    MessageBox.Show("No se encontraron clientes");
                }
            }
            // atiende el error de esta operacion antes de informar o propagar el problema
            catch (Exception error)
            {
                // muestra el error producido
                MessageBox.Show(error.Message);
            }
        }

        // responde al cambio del texto; revisa el estado de busqueda o de seleccion antes de continuar
        private void cboBuscarCliente_TextUpdate(object sender, EventArgs e)
        {
            OcultarSugerencias();
            if (!seleccionandoCliente)
            {
                esperaBusqueda.Start();
            }
        }

        // resuelve buscar clientes con los datos recibidos y entrega el resultado al codigo que lo llama
        private static List<Cliente> BuscarClientes(string texto, int filtro)
        {
            return new CN_Cliente().buscarCliente(texto, filtro, buscarPorPrefijo: true);
        }

        // resuelve mostrar clientes con los datos recibidos y entrega el resultado al codigo que lo llama
        private void MostrarClientes(List<Cliente> clientes)
        {
            dgv_listadoClientes.DataSource = null;
            dgv_listadoClientes.DataSource = clientes;
            // procesa cada fila de dgv listado clientesrows sin cambiar el orden de la coleccion
            foreach (DataGridViewRow fila in dgv_listadoClientes.Rows)
            {
                // filacells["colaccion"]value: valor actual del control numerico o de fecha
                fila.Cells["colAccion"].Value = "Modificar cliente";
            }

            ActualizarBotonEstadoCliente();
        }

        // resuelve ocultar sugerencias con los datos recibidos y entrega el resultado al codigo que lo llama
        private void OcultarSugerencias()
        {
            esperaBusqueda.Stop();
            cboBuscarCliente.DroppedDown = false;
        }

        // mismo buscador que nueva venta: consulta por prefijo y conserva lo escrito
        private void ActualizarSugerencias()
        {
            esperaBusqueda.Stop();
            string texto = cboBuscarCliente.Text;
            try
            {
                seleccionandoCliente = true;
                cboBuscarCliente.Items.Clear();
                // procesa cada cliente de buscar clientes(texto, mathmax(cbo filtro estado clienteselected index, 0)) sin cambiar el orden de la coleccion
                foreach (Cliente cliente in BuscarClientes(texto, Math.Max(cboFiltroEstadoCliente.SelectedIndex, 0)))
                {
                    cboBuscarCliente.Items.Add(new OpcionCliente(cliente));
                }

                cboBuscarCliente.SelectedIndex = -1;
                cboBuscarCliente.Text = texto;
                cboBuscarCliente.SelectionStart = texto.Length;
                // comprueba si hay elementos para continuar con esta operacion
                if (cboBuscarCliente.Focused && cboBuscarCliente.Items.Count > 0)
                {
                    cboBuscarCliente.DroppedDown = true;
                }
            }
            // atiende el error de esta operacion antes de informar o propagar el problema
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
            // realiza la limpieza de salida tanto si la operacion termina bien como si falla
            finally
            {
                seleccionandoCliente = false;
            }
        }

        // resuelve cbo buscar cliente enter con los datos recibidos y entrega el resultado al codigo que lo llama
        private void cboBuscarCliente_Enter(object? sender, EventArgs e)
        {
            ActualizarSugerencias();
        }

        // recibe la opcion que el usuario confirmo en el desplegable; no confundirla con escribir texto sin elegir
        private void cboBuscarCliente_SelectionChangeCommitted(object? sender, EventArgs e)
        {
            esperaBusqueda.Stop();
            if (cboBuscarCliente.SelectedItem is OpcionCliente opcion)
            {
                MostrarClientes(new List<Cliente> { opcion.Cliente });
            }
        }

        // interpreta la tecla pulsada y ejecuta la accion prevista por el formulario
        private void Buscador_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (cboBuscarCliente.SelectedItem is OpcionCliente)
                {
                    cboBuscarCliente_SelectionChangeCommitted(sender, EventArgs.Empty);
                }
                else
                {
                    btnBuscarClientes_Click(sender!, EventArgs.Empty);
                }

                e.SuppressKeyPress = true;
            }
        }

        // agrupa los datos y operaciones de opcion cliente
        private sealed class OpcionCliente
        {
            // conserva cliente como cliente para intercambiar este dato entre las capas
            public Cliente Cliente
            {
                get;
            }

            // prepara opcion cliente antes de que se utilice la instancia
            public OpcionCliente(Cliente cliente) => Cliente = cliente;
            // resuelve to string con los datos recibidos y entrega el resultado al codigo que lo llama
            public override string ToString() => $"{Cliente.Nombre} {Cliente.Apellido} — DNI: {Cliente.DNI} — {Cliente.EstadoCuenta}";
        }

        // ajusta la presentacion de una celda sin convertir ese formato en una escritura a la base
        private void dgv_listadoClientes_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == colDeudaPendiente.Index && e.Value is decimal saldo && saldo > 0)
            {
                // ecellstyle!forecolor: color del texto cuando la celda o el control no esta seleccionado
                e.CellStyle!.ForeColor = Color.FromArgb(165, 72, 0);
            }
        }

        // prepara la informacion inicial cuando se carga la ventana
        private void frmClientes_Load(object? sender, EventArgs e)
        {
            try
            {
                CargarOpcionesLocalidad();
                CargarClientes();
            }
            // atiende el error de esta operacion antes de informar o propagar el problema
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "Consulta de clientes");
            }
        }

        // ejecuta la accion pendiente del temporizador, por ejemplo una consulta despues de una pausa al escribir
        private void esperaBusqueda_Tick(object? sender, EventArgs e)
        {
            ActualizarSugerencias();
        }

        // resuelve frm clientes disposed con los datos recibidos y entrega el resultado al codigo que lo llama
        private void frmClientes_Disposed(object? sender, EventArgs e)
        {
            esperaBusqueda.Dispose();
        }

        // carga las opciones activas y conserva el texto que estaba escribiendo el usuario
        private void CargarOpcionesLocalidad(string? conservarTexto = null)
        {
            string texto = conservarTexto ?? cboLocalidadCliente.Text;
            List<string> localidades = new CN_Localidad().Obtener();
            cboLocalidadCliente.BeginUpdate();
            try
            {
                cboLocalidadCliente.Items.Clear();
                cboLocalidadCliente.Items.AddRange(localidades.Cast<object>().ToArray());
                cboLocalidadCliente.SelectedIndex = -1;
                cboLocalidadCliente.Text = texto;
                cboLocalidadCliente.SelectionStart = cboLocalidadCliente.Text.Length;
            }
            finally
            {
                cboLocalidadCliente.EndUpdate();
            }
        }

        // mantiene en mayusculas el texto elegido o escrito en la localidad
        private void cboLocalidadCliente_TextChanged(object? sender, EventArgs e)
        {
            string texto = cboLocalidadCliente.Text;
            string mayusculas = texto.ToUpper(CultureInfo.CurrentCulture);
            if (texto == mayusculas) return;
            int inicio = cboLocalidadCliente.SelectionStart;
            cboLocalidadCliente.Text = mayusculas;
            cboLocalidadCliente.SelectionStart = Math.Min(inicio, mayusculas.Length);
        }

        // abre la administracion y vuelve a cargar las opciones al cerrar la ventana
        private void btnAdministrarLocalidades_Click(object? sender, EventArgs e)
        {
            string texto = cboLocalidadCliente.Text;
            using frmAdministrarLocalidades formulario = new frmAdministrarLocalidades();
            formulario.ShowDialog(this);
            CargarOpcionesLocalidad(texto);
        }

        // construye el cliente con los valores actuales del formulario
        private Cliente ObtenerClienteDelFormulario()
        {
            return new Cliente
            {
                Nombre = txtNombreCliente.Text.Trim(),
                Apellido = txtApellidoCliente.Text.Trim(),
                DNI = txtDocumentoCliente.Text.Trim(),
                TipoDocumento = cboTipoDocumento.Text,
                Telefono = CN_Cliente.UnirTelefono(txtAreaCliente.Text, txtTelefonoCliente.Text),
                Direccion = txtDireccionCliente.Text.Trim(),
                // envia el nombre elegido o escrito para resolver su identificador al guardar
                NombreLocalidad = cboLocalidadCliente.Text.Trim(),
                FechaRegistro = DateTime.Now,
                SaldoFavor = 0

            };
        }
    }
}
