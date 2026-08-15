using Prana.Entidades;
using Prana.negocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Prana.Presentacion
{
    public partial class frmClientes : Form
    {
        public frmClientes()
        {
            InitializeComponent();

            // evita que se creen columnas adicionales automaticamente
            dgv_listadoClientes.AutoGenerateColumns = false;
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
                fila.Cells["colAccion"].Value = "Modificar";
            }
        }






        private void dgv_listadoClientes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // evita ejecutar el codigo al presionar los encabezados
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
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
            string textoBoton = filaSeleccionada.Cells["colAccion"].Value?.ToString() ?? "Modificar";

            // comprueba si se debe habilitar la modificacion
            if (textoBoton == "Modificar")
            {
                // habilita solamente los datos que pueden modificarse
                filaSeleccionada.Cells["colNombre"].ReadOnly = false;
                filaSeleccionada.Cells["colApellido"].ReadOnly = false;
                filaSeleccionada.Cells["colDNI"].ReadOnly = false;
                filaSeleccionada.Cells["ColTelefono"].ReadOnly = false;
                filaSeleccionada.Cells["colDireccion"].ReadOnly = false;
                filaSeleccionada.Cells["colLocalidad"].ReadOnly = false;

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
        private void btnBuscarProducto_Click(object sender, EventArgs e)
        {
            try
            {
                // obtiene el texto ingresado
                string busqueda = txtBuscarProducto.Text.Trim();

                // crea el objeto de negocio
                CN_Cliente negocioCliente = new CN_Cliente();

                // carga los resultados encontrados
                dgv_listadoClientes.DataSource = null;
                dgv_listadoClientes.DataSource = negocioCliente.buscarCliente(busqueda);

                // coloca modificar en cada boton
                foreach (DataGridViewRow fila in dgv_listadoClientes.Rows)
                {
                    fila.Cells["colAccion"].Value = "Modificar";
                }

                // informa cuando no se encontraron clientes
                if (dgv_listadoClientes.Rows.Count == 0)
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

        private void txtBuscarProducto_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
