using Prana.Datos;
using Prana.Entidades;
using Prana.negocio;
using Prana.Negocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace Prana.Presentacion
{
    public partial class frmStock : Form
    {
        // Mantiene el DataGridView actualizado automáticamente
        private readonly BindingList<VarianteProducto> variantesTemporales = new BindingList<VarianteProducto>();
        // Lista separada para el detalle de ingreso (no enlazada a dgvVariantes)
        private readonly BindingList<VarianteProducto> ingresoTemporales = new BindingList<VarianteProducto>();

        // Guarda el producto que actualmente se está modificando
        private int idProductoEnEdicion = 0;

        // Guarda qué fila está siendo editada
        private int filaEnEdicion = -1;


        public frmStock()
        {
            InitializeComponent();

           
        
           

            // Crea el objeto de la capa Negocio que permite consultar los proveedores.
            CN_Proveedor negocioProveedor = new CN_Proveedor();

            // Busca todos los proveedores guardados en la base de datos.
            // Se envía un texto vacío para que la búsqueda devuelva la lista completa.
            List<Proveedor> proveedores =
                negocioProveedor.buscarProveedor(string.Empty);

            // Indica qué propiedad del proveedor se mostrará en el ComboBox.
            comboBoxSeleccionaProveedor.DisplayMember = "nombreProveedor";

            // Indica qué propiedad se utilizará como valor o identificador.
            comboBoxSeleccionaProveedor.ValueMember = "idProveedor";

            // Carga en el ComboBox todos los proveedores encontrados.
            comboBoxSeleccionaProveedor.DataSource = proveedores;

            // Deja el ComboBox sin ningún proveedor seleccionado inicialmente.
            comboBoxSeleccionaProveedor.SelectedIndex = -1;
        

        // Impide crear columnas automáticas
        dgvVariantes.AutoGenerateColumns = false;

            // Vincula la lista con las columnas creadas en el diseñador
            dgvVariantes.DataSource = variantesTemporales;

            // El grid de detalle de ingreso usa una lista separada para no mezclar con las variantes del producto
            dataGridViewDetalleIngreso.AutoGenerateColumns = false;
            dataGridViewDetalleIngreso.DataSource = ingresoTemporales;

            // Configuración de columnas del grid de ingreso: dejar la mayoría en modo lectura excepto cantidad y costo
            if (dataGridViewDetalleIngreso.Columns.Contains("idVariante"))
                dataGridViewDetalleIngreso.Columns["idVariante"].DataPropertyName = "IdVariante";

            if (dataGridViewDetalleIngreso.Columns.Contains("colProducto"))
            {
                dataGridViewDetalleIngreso.Columns["colProducto"].DataPropertyName = string.Empty;
                dataGridViewDetalleIngreso.Columns["colProducto"].ReadOnly = true;
            }



            if (dataGridViewDetalleIngreso.Columns.Contains("colVarianteIngreso"))
            {
                dataGridViewDetalleIngreso.Columns["colVarianteIngreso"].DataPropertyName = string.Empty;
                dataGridViewDetalleIngreso.Columns["colVarianteIngreso"].ReadOnly = true;
            }

            if (!dataGridViewDetalleIngreso.Columns.Contains("colCondicionIngreso"))
            {
                var colCond = new DataGridViewTextBoxColumn();
                colCond.Name = "colCondicionIngreso";
                colCond.HeaderText = "Condición";
                colCond.ReadOnly = true;
                int insertIndex = dataGridViewDetalleIngreso.Columns.Contains("colVarianteIngreso") ? dataGridViewDetalleIngreso.Columns["colVarianteIngreso"].Index + 1 : dataGridViewDetalleIngreso.Columns.Count;
                dataGridViewDetalleIngreso.Columns.Insert(insertIndex, colCond);
            }

            if (dataGridViewDetalleIngreso.Columns.Contains("colCantidad"))
            {
                dataGridViewDetalleIngreso.Columns["colCantidad"].DataPropertyName = string.Empty;
                dataGridViewDetalleIngreso.Columns["colCantidad"].ReadOnly = false;
                dataGridViewDetalleIngreso.Columns["colCantidad"].DefaultCellStyle.NullValue = string.Empty;
            }

            if (dataGridViewDetalleIngreso.Columns.Contains("colCosto"))
            {
                dataGridViewDetalleIngreso.Columns["colCosto"].DataPropertyName = string.Empty;
                dataGridViewDetalleIngreso.Columns["colCosto"].ReadOnly = false;
                dataGridViewDetalleIngreso.Columns["colCosto"].DefaultCellStyle.NullValue = string.Empty;
            }

            if (dataGridViewDetalleIngreso.Columns.Contains("colTotal"))
            {
                dataGridViewDetalleIngreso.Columns["colTotal"].DataPropertyName = string.Empty;
                dataGridViewDetalleIngreso.Columns["colTotal"].ReadOnly = true;
                dataGridViewDetalleIngreso.Columns["colTotal"].DefaultCellStyle.NullValue = string.Empty;
            }

            // Suscribimos el botón registrar ingreso si existe
            if (ButtonRegistrarIngreso != null)
            {
                ButtonRegistrarIngreso.Click -= ButtonRegistrarIngreso_Click;
                ButtonRegistrarIngreso.Click += ButtonRegistrarIngreso_Click;
            }


            //esta parte debajo hace que se carguen las categorias en el combobox de Marca

            // Crea el objeto de la capa de Negocio
            CN_Marca cnMarca = new CN_Marca();

            // Obtiene todas las marcas registradas
            List<Marca> marcas = cnMarca.obtenerMarcas();

            // Indica qué propiedad se mostrará en el ComboBox
            cboMarca.DisplayMember = "NombreMarca";

            // Indica qué propiedad contiene el identificador
            cboMarca.ValueMember = "IdMarca";

            // Carga las marcas en el ComboBox
            cboMarca.DataSource = marcas;

            // Evita que aparezca una marca seleccionada inicialmente
            cboMarca.SelectedIndex = -1;


            //esta parte debajo hace que se carguen las categorias en el combobox de categoria



            // Crea el objeto de la capa de Negocio
            CN_Categoria cnCategoria = new CN_Categoria();

            // Obtiene todas las categorías registradas
            List<Categoria> categorias = cnCategoria.obtenerCategorias();

            // Indica qué propiedad se mostrará en el ComboBox
            comboBoxCateogria.DisplayMember = "NombreCategoria";

            // Indica qué propiedad se utilizará como identificador
            comboBoxCateogria.ValueMember = "IdCategoria";

            // Carga las categorías en el ComboBox
            comboBoxCateogria.DataSource = categorias;

            // Evita que aparezca una categoría seleccionada inicialmente
            comboBoxCateogria.SelectedIndex = -1;





        }

        // actualiza los totales cuando se modifica la cantidad o el costo
        private void dataGridViewDetalleIngreso_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            // evita ejecutar el codigo sobre encabezados
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                return;
            }

            // comprueba que se haya modificado cantidad o costo
            if (dataGridViewDetalleIngreso.Columns[e.ColumnIndex].Name != "colCantidad" &&
                dataGridViewDetalleIngreso.Columns[e.ColumnIndex].Name != "colCosto")
            {
                return;
            }

            // actualiza los totales mostrados
            ActualizarTotalesIngreso();
        }

        // calcula los totales de todas las filas del ingreso
        private void ActualizarTotalesIngreso()
        {
            // inicia los acumuladores
            int cantidadTotal = 0;
            decimal importeTotal = 0;

            // recorre todas las filas del detalle
            foreach (DataGridViewRow fila in dataGridViewDetalleIngreso.Rows)
            {
                // ignora la fila vacia utilizada para agregar registros
                if (fila.IsNewRow)
                {
                    continue;
                }

                // intenta obtener la cantidad ingresada
                int.TryParse(fila.Cells["colCantidad"].Value?.ToString(), out int cantidad);

                // intenta obtener el costo unitario ingresado
                decimal.TryParse(fila.Cells["colCosto"].Value?.ToString(), out decimal costoUnitario);

                // evita utilizar numeros negativos
                if (cantidad < 0)
                {
                    cantidad = 0;
                }

                // evita utilizar costos negativos
                if (costoUnitario < 0)
                {
                    costoUnitario = 0;
                }

                // calcula el total de la fila
                decimal totalFila = cantidad * costoUnitario;

                // muestra el total calculado en la fila
                fila.Cells["colTotal"].Value = totalFila;

                // suma la cantidad de la fila
                cantidadTotal += cantidad;

                // suma el importe de la fila
                importeTotal += totalFila;
            }

            // muestra la cantidad total de unidades
            labelCantidadIngresada.Text = cantidadTotal.ToString();

            // muestra el importe total del ingreso
            LblTotalIngresoDinero.Text = importeTotal.ToString("C2");
        }

        // Carga el producto completo en el sector de edición
        private void CargarProductoParaModificar(
            producto productoSeleccionado)
        {
            // Guarda el identificador del producto
            idProductoEnEdicion =
                productoSeleccionado.IdProducto;

            // Carga los datos generales
            textBoxNombreProducto.Text =
                productoSeleccionado.NombreProducto;

            textBoxCategoria.Text =
                productoSeleccionado.Categoria;

            textBoxMarca.Text =
                productoSeleccionado.Marca;

            textBoxDescripcion.Text =
                productoSeleccionado.Descripcion;

            // Limpia las variantes anteriores
            variantesTemporales.Clear();

            // Carga todas las variantes del producto
            foreach (VarianteProducto variante
                in productoSeleccionado.Variantes)
            {
                variantesTemporales.Add(variante);
            }


            // Lleva el cursor al nombre
            textBoxNombreProducto.Focus();
        }

      

        private void btnAgregarVariante_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxColor.Text))
            {
                MessageBox.Show("Ingrese el color de la variante");
                textBoxColor.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(comboBoxCondicion2.Text))
            {
                MessageBox.Show("Seleccione la condición de la variante");
                comboBoxCondicion2.Focus();
                return;
            }

            if (nudPrecio.Value <= 0)
            {
                MessageBox.Show("Ingrese un precio mayor que cero");
                nudPrecio.Focus();
                return;
            }

            bool varianteRepetida = variantesTemporales.Any(
                variante =>
                    variante.Color.Equals(
                        textBoxColor.Text.Trim(),
                        StringComparison.OrdinalIgnoreCase
                    )
                    &&
                    variante.CondicionProducto.Equals(
                        comboBoxCondicion2.Text.Trim(),
                        StringComparison.OrdinalIgnoreCase
                    )
            );

            if (varianteRepetida)
            {
                MessageBox.Show(
                    "Ya agregaste una variante con ese color y condición"
                );
                return;
            }

            VarianteProducto nuevaVariante =
                new VarianteProducto
                {
                    Color = textBoxColor.Text.Trim(),
                    CondicionProducto = comboBoxCondicion2.Text.Trim(),
                    Precio = nudPrecio.Value
                };

            variantesTemporales.Add(nuevaVariante);

            // Muestra el modelo del producto en la última fila agregada
            int ultimaFila = dgvVariantes.Rows.Count - 1;

            // Muestra el nombre del producto
            dgvVariantes.Rows[ultimaFila].Cells["NombreProducto"].Value = textBoxNombreProducto.Text.Trim();

            dgvVariantes.Rows[ultimaFila].Cells["Colmodelo2"].Value = textBoxModelo.Text.Trim();

            LimpiarCamposVariante();

            textBoxColor.Focus();
        }


        // Guarda el producto junto con todas sus variantes
        private void btnGuardarProducto_Click(object sender, EventArgs e)
        {
            // Verifica que se haya ingresado el modelo o código
            if (string.IsNullOrWhiteSpace(textBoxNombreProducto.Text))
            {
                MessageBox.Show(
                    "Ingrese el modelo o código del producto",
                    "Dato requerido",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                textBoxNombreProducto.Focus();
                return;
            }

            // Verifica que se haya ingresado la categoría
            if (string.IsNullOrWhiteSpace(textBoxCategoria.Text))
            {
                MessageBox.Show(
                    "Ingrese la categoría",
                    "Dato requerido",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                textBoxCategoria.Focus();
                return;
            }

            // Verifica que se haya ingresado la marca
            if (string.IsNullOrWhiteSpace(textBoxMarca.Text))
            {
                MessageBox.Show(
                    "Ingrese la marca",
                    "Dato requerido",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                textBoxMarca.Focus();
                return;
            }

            // Verifica que exista al menos una variante
            if (variantesTemporales.Count == 0)
            {
                MessageBox.Show(
                    "Agregue al menos una variante antes de guardar",
                    "Variante requerida",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                textBoxColor.Focus();
                return;
            }

            // Crea el producto con sus datos y variantes
            producto nuevoProducto = new producto
            {
                NombreProducto = textBoxNombreProducto.Text.Trim(),
                Modelo = textBoxModelo.Text.Trim(),
                Categoria = textBoxCategoria.Text.Trim(),
                Marca = textBoxMarca.Text.Trim(),
                Descripcion = textBoxDescripcion.Text.Trim(),

                // Envía una copia de la lista temporal
                Variantes =
                    new List<VarianteProducto>(variantesTemporales)
            };

            // Crea una instancia de la capa de Negocio
            CN_Producto negocioProducto = new CN_Producto();

            // Envía el producto para guardarlo
            bool guardado =
                negocioProducto.NuevoProducto(nuevoProducto);

            // Verifica el resultado
            if (guardado)
            {
                MessageBox.Show(
                    "Producto y variantes guardados correctamente",
                    "Producto guardado",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                // Limpia completamente el formulario
                LimpiarFormulario();
            }
            else
            {
                MessageBox.Show(
                    "No se pudo guardar el producto",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        // Cancela la operación y limpia el formulario
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult respuesta = MessageBox.Show(
                "¿Desea cancelar la operación y borrar los datos ingresados?",
                "Cancelar operación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            // Detiene la operación si el usuario elige No
            if (respuesta == DialogResult.No)
            {
                return;
            }

            // Limpia completamente el formulario
            LimpiarFormulario();
        }

        // Limpia solamente los campos utilizados por una variante
        private void LimpiarCamposVariante()
        {
            textBoxColor.Clear();
            comboBoxCondicion2.SelectedIndex = -1;
            nudPrecio.Value = nudPrecio.Minimum;
        }

        // Limpia todos los datos del formulario
        private void LimpiarFormulario()
        {
            // Limpia los datos generales del producto
            textBoxNombreProducto.Clear();
            textBoxCategoria.Clear();
            textBoxMarca.Clear();
            textBoxDescripcion.Clear();

            // Limpia los campos de la variante
            LimpiarCamposVariante();

            // Vacía la lista de variantes temporales
            variantesTemporales.Clear();


            // Coloca el cursor en el primer campo
            textBoxNombreProducto.Focus();
        }

        private void pnlCliente_Paint(
            object sender,
            PaintEventArgs e)
        {
        }

        private void lblTituloNuevaVenta_Click(
            object sender,
            EventArgs e)
        {
        }

        private void lblTituloCliente_Click(
            object sender,
            EventArgs e)
        {
        }

        private void cboCategoria_SelectedIndexChanged(
            object sender,
            EventArgs e)
        {
        }

        private void label9_Click(
            object sender,
            EventArgs e)
        {
        }

        private void panel2_Paint(
            object sender,
            PaintEventArgs e)
        {
        }

        private void comboBox2_SelectedIndexChanged(
            object sender,
            EventArgs e)
        {
        }

        private void BtnLimpiarFiltros_Click(object sender,EventArgs e)
        {
            // Limpia el nombre o modelo ingresado
            txtBuscarProducto.Clear();

            // Quita las selecciones de los filtros
            comboBoxCateogria.SelectedIndex = -1;
            cboMarca.SelectedIndex = -1;
            comboBoxCondicion.SelectedIndex = -1;

            // Elimina los resultados de la búsqueda anterior
            dataGridViewBusquedaProducto.Rows.Clear();

            // Deja el cursor en el campo de búsqueda
            txtBuscarProducto.Focus();
        }

        private void comboBox1_SelectedIndexChanged(
            object sender,
            EventArgs e)
        {
        }

        private void comboBox4_SelectedIndexChanged(
            object sender,
            EventArgs e)
        {

        }

        private void panel1_Paint(
            object sender,
            PaintEventArgs e)
        {
        }

        private void textBoxCodigoProducto_TextChanged(
            object sender,
            EventArgs e)
        {
        }

        private void textBoxNombreProducto_TextChanged(
            object sender,
            EventArgs e)
        {
        }

        private void textBoxCategoria_TextChanged(
            object sender,
            EventArgs e)
        {
        }

        private void textBoxMarca_TextChanged(
            object sender,
            EventArgs e)
        {
        }

        private void textBoxColor_TextChanged(
            object sender,
            EventArgs e)
        {
        }

        private void nudPrecio_ValueChanged(
            object sender,
            EventArgs e)
        {
        }

        private void textBoxOtroAtributo_TextChanged(
            object sender,
            EventArgs e)
        {
        }

        private void textBoxDescripcion_TextChanged(
            object sender,
            EventArgs e)
        {
        }

        private void txtBuscarProducto_TextChanged(
            object sender,
            EventArgs e)
        {
        }

        private void comboBoxElegirVariante_SelectedIndexChanged(
            object sender,
            EventArgs e)
        {
        }

        private void label12_Click(
            object sender,
            EventArgs e)
        {
        }

        private void dgvVariantes_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnBuscarProducto_Click(object sender, EventArgs e)
        {
            string nombreProducto = txtBuscarProducto.Text.Trim();
            string modelo = textBoxModelo.Text.Trim();

            int? idCategoria = comboBoxCateogria.SelectedIndex == -1 ? null : Convert.ToInt32(comboBoxCateogria.SelectedValue);
            int? idMarca = cboMarca.SelectedIndex == -1 ? null : Convert.ToInt32(cboMarca.SelectedValue);

            string condicionProducto = comboBoxCondicion.SelectedIndex == -1 ? string.Empty : comboBoxCondicion.Text.Trim();

            CN_Producto cnProducto = new CN_Producto();

            List<producto> productosEncontrados = cnProducto.buscarProducto(nombreProducto, idCategoria, idMarca, condicionProducto, modelo);

            // elimina los resultados anteriores
            dataGridViewBusquedaProducto.Rows.Clear();

            // recorre los productos encontrados
            foreach (producto productoEncontrado in productosEncontrados)
            {
                // recorre las variantes del producto
                foreach (VarianteProducto varianteEncontrada in productoEncontrado.Variantes)
                {
                    // agrega la variante y guarda el indice de la nueva fila
                    int indiceFila = dataGridViewBusquedaProducto.Rows.Add(
                        varianteEncontrada.IdVariante,
                        productoEncontrado.Modelo,
                        productoEncontrado.NombreProducto,
                        productoEncontrado.Descripcion,
                        varianteEncontrada.Color,
                        varianteEncontrada.CondicionProducto,
                        varianteEncontrada.Precio,
                        varianteEncontrada.Costo,
                        varianteEncontrada.StockFisico,
                        varianteEncontrada.StockComprometido
                    );

                    // coloca modificar en el boton de la fila agregada
                    dataGridViewBusquedaProducto.Rows[indiceFila].Cells["colModificar"].Value = "Modificar";
                }
            }
        }

        private void cboMarca_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dataGridViewBusquedaProducto_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Evita clics en los encabezados
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                return;
            }

            // Obtiene el nombre de la columna presionada
            string nombreColumna = dataGridViewBusquedaProducto.Columns[e.ColumnIndex].Name;

            // Desactiva la variante seleccionada
            if (nombreColumna == "ColEliminar")
            {
                // Obtiene el identificador de la variante
                int idVariante = Convert.ToInt32(dataGridViewBusquedaProducto.Rows[e.RowIndex].Cells["colIdVariante"].Value);

                // Solicita confirmación antes de desactivar
                DialogResult respuesta = MessageBox.Show("¿Desea desactivar esta variante?", "Confirmar desactivación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                // Cancela la operación si el usuario selecciona No
                if (respuesta == DialogResult.No)
                {
                    return;
                }

                try
                {
                    // Envía la solicitud a la capa de Negocio
                    CN_Producto cnProducto = new CN_Producto();

                    bool desactivada = cnProducto.DesactivarVariante(idVariante);

                    // Informa si la variante no pudo desactivarse
                    if (!desactivada)
                    {
                        MessageBox.Show(
                            "La variante no pudo desactivarse."
                        );

                        return;
                    }

                    // Informa que la variante fue desactivada
                    MessageBox.Show(
                        "Variante desactivada correctamente."
                    );

                    // Vuelve a ejecutar la búsqueda
                    btnBuscarProducto.PerformClick();
                }
                catch (Exception ex)
                {
                    // Muestra el error producido
                    MessageBox.Show(
                        "Error al desactivar la variante: "
                        + ex.Message
                    );
                }

                return;
            }

            // Solo continúa si se presionó el botón Modificar
            if (nombreColumna != "colModificar")
            {
                return;
            }

            // Obtiene la fila seleccionada
            DataGridViewRow fila =
                dataGridViewBusquedaProducto.Rows[e.RowIndex];

            // Primer clic: comienza la modificación
            if (filaEnEdicion == -1)
            {
                // Guarda qué fila se está editando
                filaEnEdicion = e.RowIndex;

                // cambia el texto del boton mientras se modifica la fila
                fila.Cells["colModificar"].Value = "Guardar modificación";

                // Selecciona la celda del modelo
                dataGridViewBusquedaProducto.CurrentCell = fila.Cells["colModelo"];

                // Espera a que termine el clic y abre la edición
                BeginInvoke(new Action(() =>
                {
                    dataGridViewBusquedaProducto.BeginEdit(true);
                }));

                return;
            }

            // Impide modificar simultáneamente otra fila
            if (filaEnEdicion != e.RowIndex)
            {
                MessageBox.Show(
                    "Primero guarde la fila que está modificando."
                );

                return;
            }

            // Finaliza la edición de la celda actual
            dataGridViewBusquedaProducto.EndEdit();

            // Obtiene el identificador de la variante
            int idVarianteModificar = Convert.ToInt32(fila.Cells["colIdVariante"].Value);

            // Obtiene el modelo modificado
            string modelo = Convert.ToString(fila.Cells["colModelo"].Value)?.Trim() ?? string.Empty;



            // Obtiene el nombre del producto
            string nombreProducto = Convert.ToString(fila.Cells["colNombre2"].Value)?.Trim() ?? string.Empty;

            // Obtiene la descripción
            string descripcion = Convert.ToString(fila.Cells["colDescripcion"].Value)?.Trim() ?? string.Empty;

            // Obtiene el color
            string color = Convert.ToString(fila.Cells["colColorb"].Value)?.Trim() ?? string.Empty;

            // Obtiene la condición
            string condicion = Convert.ToString(fila.Cells["colCondicion"].Value)?.Trim() ?? string.Empty;

            // Valida los campos obligatorios
            if (string.IsNullOrWhiteSpace(modelo) || string.IsNullOrWhiteSpace(nombreProducto) || string.IsNullOrWhiteSpace(color) || string.IsNullOrWhiteSpace(condicion))
            {
                MessageBox.Show(
                    "El modelo, el producto, el color y la condición son obligatorios."
                );

                return;
            }

            // Valida el costo
            bool costoValido = decimal.TryParse(
                Convert.ToString(
                    fila.Cells["colCosto2"].Value
                ),
                out decimal costo
            );

            if (!costoValido || costo < 0)
            {
                MessageBox.Show("Ingrese un costo válido.");
                return;
            }

            // Valida el precio
            bool precioValido = decimal.TryParse(
                Convert.ToString(
                    fila.Cells["colPrecio"].Value
                ),
                out decimal precio
            );

            if (!precioValido || precio < 0)
            {
                MessageBox.Show("Ingrese un precio válido.");
                return;
            }

            // Valida el stock físico
            bool stockFisicoValido = int.TryParse(
                Convert.ToString(
                    fila.Cells["colStockFisicoBusqueda"].Value
                ),
                out int stockFisico
            );

            if (!stockFisicoValido || stockFisico < 0)
            {
                MessageBox.Show(
                    "Ingrese un stock físico válido."
                );

                return;
            }

            // Obtiene el stock comprometido
            int stockComprometido = Convert.ToInt32(
                fila.Cells["colComprometidoBusqueda"].Value
            );

            // Impide tener menos stock físico que comprometido
            if (stockFisico < stockComprometido)
            {
                MessageBox.Show(
                    "El stock físico no puede ser menor al stock comprometido."
                );

                return;
            }

            // Calcula el stock realmente disponible
            int stockDisponible =
                stockFisico - stockComprometido;

            try
            {
                // Envía los cambios a la capa de Negocio
                CN_Producto cnProducto = new CN_Producto();

                bool actualizado = cnProducto.ActualizarProductoDesdeStock(idVarianteModificar, modelo,nombreProducto, descripcion, color, condicion,costo,precio,stockDisponible);

                // Informa si no se pudo actualizar
                if (!actualizado)
                {
                    MessageBox.Show(
                        "No se pudo actualizar el producto."
                    );

                    return;
                }

                // Vuelve a colocar el texto Modificar
                fila.Cells["colModificar"].Value = "Modificar";

                // Indica que ya no hay ninguna fila en edición
                filaEnEdicion = -1;

                // Informa que la actualización terminó
                MessageBox.Show(
                    "Producto actualizado correctamente."
                );

                // Vuelve a cargar la búsqueda desde la base
                btnBuscarProducto.PerformClick();
            }
            catch (Exception ex)
            {
                // Muestra el error producido
                MessageBox.Show(
                    "Error al actualizar: " + ex.Message
                );
            }
        }


        private void pnlEncabezadostock_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dataGridViewBusquedaProducto_CellClick(object sender,DataGridViewCellEventArgs e)
        {
            // Evita clics en los encabezados
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                return;
            }

            // Solo permite editar la fila en modificación
            if (e.RowIndex != filaEnEdicion)
            {
                return;
            }

            // Obtiene el nombre de la columna seleccionada
            string nombreColumna = dataGridViewBusquedaProducto.Columns[e.ColumnIndex].Name;

            // Impide editar identificadores, modelo y columnas de acciones
            // Columnas que no pueden modificarse
            if (nombreColumna == "colIdVariante"|| nombreColumna == "colComprometidoBusqueda" || nombreColumna == "colModificar" || nombreColumna == "ColEliminar")
            {
                return;
            }

            // Obtiene la celda seleccionada
            DataGridViewCell celda = dataGridViewBusquedaProducto.Rows[e.RowIndex].Cells[e.ColumnIndex];

            // No intenta editar una celda bloqueada
            if (celda.ReadOnly)
            {
                return;
            }

            // Selecciona la celda
            dataGridViewBusquedaProducto.CurrentCell = celda;

            // Abre la edicion de la celda
            BeginInvoke(new Action(() =>
            {
                dataGridViewBusquedaProducto.BeginEdit(true);
            }));
        }

        private void iconButton1_Click(object sender, EventArgs e)
        {
            //Crea la instancia del formulario de selección
            buttonSeleccionarProducto formulario = new buttonSeleccionarProducto();

            //Se suscribe para recibir la variante y el producto seleccionado
            formulario.OnVarianteSeleccionada += (variante, producto) =>
            {
                // Agrega la variante a la lista del ingreso
                ingresoTemporales.Add(variante);

                // Rellena la última fila agregada con información (dejar cantidad y costo vacíos)
                int ultimaFila = dataGridViewDetalleIngreso.Rows.Count - 1;
                if (ultimaFila >= 0)
                {
                    if (dataGridViewDetalleIngreso.Columns.Contains("colProducto"))
                        dataGridViewDetalleIngreso.Rows[ultimaFila].Cells["colProducto"].Value = producto.NombreProducto;

                    if (dataGridViewDetalleIngreso.Columns.Contains("colVarianteIngreso"))
                        dataGridViewDetalleIngreso.Rows[ultimaFila].Cells["colVarianteIngreso"].Value = variante.Color;

                    if (dataGridViewDetalleIngreso.Columns.Contains("colCondicionIngreso"))
                        dataGridViewDetalleIngreso.Rows[ultimaFila].Cells["colCondicionIngreso"].Value = variante.CondicionProducto;

                    if (dataGridViewDetalleIngreso.Columns.Contains("colCosto"))
                        dataGridViewDetalleIngreso.Rows[ultimaFila].Cells["colCosto"].Value = DBNull.Value;

                    if (dataGridViewDetalleIngreso.Columns.Contains("colCantidad"))
                        dataGridViewDetalleIngreso.Rows[ultimaFila].Cells["colCantidad"].Value = DBNull.Value;

                    if (dataGridViewDetalleIngreso.Columns.Contains("colTotal"))
                        dataGridViewDetalleIngreso.Rows[ultimaFila].Cells["colTotal"].Value = DBNull.Value;
                }
            };

            // abre con .Show() para que permanezca abierta de fondo y libre
            formulario.Show();
        }

        private void dataGridViewDetalleIngreso_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void iconButtonAgregarIngreso_Click(object sender, EventArgs e)
        {

        }

        private void ButtonRegistrarIngreso_Click(object sender, EventArgs e)
        {
            //declara la instancia de la capa de negocio para poder utilizar sus metodos
            CN_Proveedor negocioProveedor = new CN_Proveedor();

            //obtiene el nombre del proveedor ingresado en el textbox
            string proveedorSeleccionado = textBoxNuevoProvedor.Text.Trim();

            //crea la lista de proveedores encontrados con el nombre ingresado
            List<Proveedor> proveedoresEncontrados = negocioProveedor.buscarProveedor(proveedorSeleccionado);

            if (!string.IsNullOrWhiteSpace(proveedorSeleccionado) && proveedoresEncontrados.Count > 0)
            {
                MessageBox.Show("El proveedor ya existe seleccionelo de la lista.");
                return;
            }

            //si el proveedor no existe lo crea y lo agrega al combobox
            if (!string.IsNullOrWhiteSpace(proveedorSeleccionado) && proveedoresEncontrados.Count == 0)
            {
                Proveedor nuevoProveedor = new Proveedor
                {
                    nombreProveedor = proveedorSeleccionado
                };

                //crea la variable proveedorCreado y le asigna el resultado del metodo NuevoProveedor de la capa de negocio
                bool proveedorCreado = negocioProveedor.NuevoProveedor(nuevoProveedor);

                if (proveedorCreado != null)
                {
                    MessageBox.Show("Proveedor creado correctamente.");

                    // Actualiza el ComboBox de proveedores
                    List<Proveedor> proveedores = negocioProveedor.buscarProveedor(string.Empty);

                    // Limpia y desvincula cualquier dato que el ComboBox tuviera cargado previamente
                    comboBoxSeleccionaProveedor.DataSource = null;

                    //indica al programa que solo muestre el texto contenido en nombreProveedor dentro de la lista desplegable
                    comboBoxSeleccionaProveedor.DisplayMember = "nombreProveedor";

                    //Define el valor la clave primaria que se va a guardar detrás de la opción seleccionada
                    comboBoxSeleccionaProveedor.ValueMember = "idProveedor";

                    //Asigna la lista al componente para que se llene
                    comboBoxSeleccionaProveedor.DataSource = proveedores;
                }
                else
                {
                    MessageBox.Show("Error al crear el proveedor.");
                    return;
                }
            }





            if (ingresoTemporales.Count == 0)
            {
                MessageBox.Show("No hay items para ingresar.");
                return;
            }

            if (comboBoxSeleccionaProveedor.SelectedIndex == -1)
            {
                MessageBox.Show("Seleccione un proveedor.");
                return;
            }

            int idProveedor = Convert.ToInt32(comboBoxSeleccionaProveedor.SelectedValue);
            DateTime fecha = dtpFechaIngresoStock.Value;
            string remito = textBoxRtoProveedor.Text.Trim();

            var detalles = new List<(int idVariante, int cantidad, decimal costoUnitario)>();

            foreach (DataGridViewRow row in dataGridViewDetalleIngreso.Rows)
            {
                if (row.IsNewRow) continue;

                object idVarObj = row.Cells["idVariante"].Value;
                if (idVarObj == null) continue;

                int idVariante = Convert.ToInt32(idVarObj);

                object cantObj = row.Cells["colCantidad"].Value;
                if (cantObj == null || string.IsNullOrWhiteSpace(cantObj.ToString()))
                {
                    MessageBox.Show("Complete la cantidad para todos los items antes de registrar.");
                    return;
                }

                if (!int.TryParse(cantObj.ToString(), out int cantidad) || cantidad <= 0)
                {
                    MessageBox.Show("Ingrese cantidades válidas (>0).");
                    return;
                }

                decimal costo = 0m;
                object costoObj = row.Cells["colCosto"].Value;
                if (costoObj != null && !string.IsNullOrWhiteSpace(costoObj.ToString()))
                {
                    if (!decimal.TryParse(costoObj.ToString(), out costo))
                    {
                        MessageBox.Show("Ingrese un costo válido.");
                        return;
                    }
                }

                detalles.Add((idVariante, cantidad, costo));
            }

            CN_Producto negocio = new CN_Producto();
            bool ok = negocio.RegistrarIngreso(idProveedor, fecha, remito, detalles);

            if (ok)
            {
                MessageBox.Show("Ingreso registrado correctamente.");
                ingresoTemporales.Clear();
                dataGridViewDetalleIngreso.Refresh();
            }
            else
            {
                MessageBox.Show("Error al registrar el ingreso.");
            }
        }
    }
}
    
