// guia del archivo: esta pantalla combina consulta, alta del catalogo e ingreso de mercaderia producto agrupa nombre, modelo, marca, categoria y descripcion; cada variante aporta color, condicion y precio agregar variante prepara una fila; guardar producto confirma el articulo y sus variantes ingreso de stock es otro circuito: proveedor y documento identifican la recepcion, con cantidad y costo por unidad costo es lo que cuesta una unidad al comercio; precio es su valor de venta no intercambiar esos campos una modificacion de existencias no reemplaza una recepcion documentada ni corrige una venta historica
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
    // agrupa los datos y operaciones de frm stock
    public partial class frmStock : Form
    {
        // mantiene el datagridview actualizado automaticamente
        private readonly BindingList<VarianteProducto> variantesTemporales = new BindingList<VarianteProducto>();
        // lista separada para el detalle de ingreso (no enlazada a dgvvariantes)
        private readonly BindingList<VarianteProducto> ingresoTemporales = new BindingList<VarianteProducto>();
        // registra las operaciones exitosas realizadas sobre el stock
        private readonly CN_LogActividad cnLogActividad = new CN_LogActividad();
        // guarda el producto que actualmente se esta modificando
        private int idProductoEnEdicion = 0;
        // guarda que fila esta siendo editada
        private int filaEnEdicion = -1;
        // prepara frm stock antes de que se utilice la instancia
        public frmStock()
        {
            InitializeComponent();
            btnOpcionesStock.Visible = SesionActual.Usuario != null && Permisos.TienePermiso(Permisos.AdministrarOpcionesStock);
            // procesa cada selector de new[] { txt marca producto, txt categoria producto, combo box selecciona proveedor } sin cambiar el orden de la coleccion
            foreach (ComboBox selector in new[]
            {
                txtMarcaProducto,
                txtCategoriaProducto,
                txtColorVariante,
                comboBoxSeleccionaProveedor
            }

            )
            {
                selector.DropDownStyle = ComboBoxStyle.DropDown;
                selector.AutoCompleteSource = AutoCompleteSource.ListItems;
                selector.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            }

            // limita la entrada segun las columnas de la base de datos txtnombreproductomaxlength: limite de caracteres que puede escribir el usuario en este campo
            txtNombreProducto.MaxLength = 150;
            txtModeloProducto.MaxLength = 15;
            txtCategoriaProducto.MaxLength = 100;
            txtMarcaProducto.MaxLength = 100;
            txtDescripcionProducto.MaxLength = 500;
            txtColorVariante.MaxLength = 50;
        }

        // abre la administracion modal y refresca las listas conservando los textos que se estaban preparando
        private void btnOpcionesStock_Click(object? sender, EventArgs e)
        {
            try
            {
                Permisos.Exigir(Permisos.AdministrarOpcionesStock);
                using frmOpcionesStock formulario = new frmOpcionesStock();
                formulario.ShowDialog(this);
                CargarOpcionesProducto();
                string proveedorActual = comboBoxSeleccionaProveedor.Text;
                bool proveedorExistia = comboBoxSeleccionaProveedor.Items.Contains(proveedorActual);
                comboBoxSeleccionaProveedor.Items.Clear();
                comboBoxSeleccionaProveedor.Items.AddRange(new CN_Proveedor().buscarProveedor(string.Empty).Select(item => item.nombreProveedor).ToArray());
                // no conserva un proveedor que acaba de ser eliminado o renombrado para evitar recrearlo por error
                if (proveedorExistia && !comboBoxSeleccionaProveedor.Items.Contains(proveedorActual))
                {
                    comboBoxSeleccionaProveedor.Text = string.Empty;
                }
                else
                {
                    comboBoxSeleccionaProveedor.Text = proveedorActual;
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(this, error.Message, "Opciones de stock", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // prepara la informacion inicial cuando se carga la ventana
        private void frmStock_Load(object? sender, EventArgs e)
        {
            CargarOpcionesProducto();
            // crea el objeto de la capa negocio que permite consultar los proveedores
            CN_Proveedor negocioProveedor = new CN_Proveedor();
            // busca todos los proveedores guardados en la base de datos se envia un texto vacio para que la busqueda devuelva la lista completa
            List<Proveedor> proveedores = negocioProveedor.buscarProveedor(string.Empty);
            comboBoxSeleccionaProveedor.Items.Clear();
            comboBoxSeleccionaProveedor.Items.AddRange(proveedores.Select(proveedor => proveedor.nombreProveedor).ToArray());
            comboBoxSeleccionaProveedor.SelectedIndex = -1;
            // impide crear columnas automaticas dgvvariantesautogeneratecolumns: decide si el enlace de datos crea columnas automaticamente; desactivarlo conserva las columnas definidas
            dgvVariantes.AutoGenerateColumns = false;
            // vincula la lista con las columnas creadas en el disenador dgvvariantesdatasource: lista u objeto del que el control obtiene los registros que presenta
            dgvVariantes.DataSource = variantesTemporales;
            // el grid de detalle de ingreso usa una lista separada para no mezclar con las variantes del producto dgvdetalleingresoautogeneratecolumns: decide si el enlace de datos crea columnas automaticamente; desactivarlo conserva las columnas definidas
            dgvDetalleIngreso.AutoGenerateColumns = false;
            dgvDetalleIngreso.DataSource = ingresoTemporales;
            // configuracion de columnas del grid de ingreso: dejar la mayoria en modo lectura excepto cantidad y costo
            if (dgvDetalleIngreso.Columns.Contains("idVariante"))
            {
                // dgvdetalleingresocolumns["idvariante"]datapropertyname: propiedad del objeto enlazado cuyo valor muestra esta columna
                dgvDetalleIngreso.Columns["idVariante"].DataPropertyName = "IdVariante";
            }

            if (dgvDetalleIngreso.Columns.Contains("colProducto"))
            {
                // dgvdetalleingresocolumns["colproducto"]datapropertyname: propiedad del objeto enlazado cuyo valor muestra esta columna
                dgvDetalleIngreso.Columns["colProducto"].DataPropertyName = string.Empty;
                // dgvdetalleingresocolumns["colproducto"]readonly: impide editar el contenido desde este control sin impedir su consulta
                dgvDetalleIngreso.Columns["colProducto"].ReadOnly = true;
            }

            if (dgvDetalleIngreso.Columns.Contains("colVarianteIngreso"))
            {
                // dgvdetalleingresocolumns["colvarianteingreso"]datapropertyname: propiedad del objeto enlazado cuyo valor muestra esta columna
                dgvDetalleIngreso.Columns["colVarianteIngreso"].DataPropertyName = string.Empty;
                // dgvdetalleingresocolumns["colvarianteingreso"]readonly: impide editar el contenido desde este control sin impedir su consulta
                dgvDetalleIngreso.Columns["colVarianteIngreso"].ReadOnly = true;
            }

            if (!dgvDetalleIngreso.Columns.Contains("colCondicionIngreso"))
            {
                DataGridViewTextBoxColumn colCond = new DataGridViewTextBoxColumn();
                colCond.Name = "colCondicionIngreso";
                colCond.HeaderText = "Condición";
                colCond.ReadOnly = true;
                int insertIndex;
                if (dgvDetalleIngreso.Columns.Contains("colVarianteIngreso"))
                {
                    insertIndex = dgvDetalleIngreso.Columns["colVarianteIngreso"].Index + 1;
                }
                else
                {
                    insertIndex = dgvDetalleIngreso.Columns.Count;
                }
                dgvDetalleIngreso.Columns.Insert(insertIndex, colCond);
            }

            if (dgvDetalleIngreso.Columns.Contains("colCantidad"))
            {
                // dgvdetalleingresocolumns["colcantidad"]datapropertyname: propiedad del objeto enlazado cuyo valor muestra esta columna
                dgvDetalleIngreso.Columns["colCantidad"].DataPropertyName = string.Empty;
                // dgvdetalleingresocolumns["colcantidad"]readonly: impide editar el contenido desde este control sin impedir su consulta
                dgvDetalleIngreso.Columns["colCantidad"].ReadOnly = false;
                dgvDetalleIngreso.Columns["colCantidad"].DefaultCellStyle.NullValue = string.Empty;
            }

            if (dgvDetalleIngreso.Columns.Contains("colCosto"))
            {
                // dgvdetalleingresocolumns["colcosto"]datapropertyname: propiedad del objeto enlazado cuyo valor muestra esta columna
                dgvDetalleIngreso.Columns["colCosto"].DataPropertyName = string.Empty;
                // dgvdetalleingresocolumns["colcosto"]readonly: impide editar el contenido desde este control sin impedir su consulta
                dgvDetalleIngreso.Columns["colCosto"].ReadOnly = false;
                dgvDetalleIngreso.Columns["colCosto"].DefaultCellStyle.NullValue = string.Empty;
            }

            if (dgvDetalleIngreso.Columns.Contains("colTotal"))
            {
                // dgvdetalleingresocolumns["coltotal"]datapropertyname: propiedad del objeto enlazado cuyo valor muestra esta columna
                dgvDetalleIngreso.Columns["colTotal"].DataPropertyName = string.Empty;
                // dgvdetalleingresocolumns["coltotal"]readonly: impide editar el contenido desde este control sin impedir su consulta
                dgvDetalleIngreso.Columns["colTotal"].ReadOnly = true;
                dgvDetalleIngreso.Columns["colTotal"].DefaultCellStyle.NullValue = string.Empty;
            }

            // esta parte debajo hace que se carguen las categorias en el combobox de marca crea el objeto de la capa de negocio
            CN_Marca cnMarca = new CN_Marca();
            // obtiene todas las marcas registradas
            List<Marca> marcas = cnMarca.obtenerMarcas();
            // indica que propiedad se mostrara en el combobox
            cboMarca.DisplayMember = "NombreMarca";
            // indica que propiedad contiene el identificador
            cboMarca.ValueMember = "IdMarca";
            // carga las marcas en el combobox cbomarcadatasource: lista u objeto del que el control obtiene los registros que presenta
            cboMarca.DataSource = marcas;
            // evita que aparezca una marca seleccionada inicialmente cbomarcaselectedindex: indice de la opcion actual; menos uno significa que no hay una opcion seleccionada
            cboMarca.SelectedIndex = -1;
            // esta parte debajo hace que se carguen las categorias en el combobox de categoria crea el objeto de la capa de negocio
            CN_Categoria cnCategoria = new CN_Categoria();
            // obtiene todas las categorias registradas
            List<Categoria> categorias = cnCategoria.obtenerCategorias();
            // indica que propiedad se mostrara en el combobox
            comboBoxCateogria.DisplayMember = "NombreCategoria";
            // indica que propiedad se utilizara como identificador
            comboBoxCateogria.ValueMember = "IdCategoria";
            // carga las categorias en el combobox comboboxcateogriadatasource: lista u objeto del que el control obtiene los registros que presenta
            comboBoxCateogria.DataSource = categorias;
            // evita que aparezca una categoria seleccionada inicialmente comboboxcateogriaselectedindex: indice de la opcion actual; menos uno significa que no hay una opcion seleccionada
            comboBoxCateogria.SelectedIndex = -1;
        }

        // actualiza los totales cuando se modifica la cantidad o el costo atiende el evento cellendedit del control o formulario indicado por el nombre del metodo
        private void dgvDetalleIngreso_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            // evita ejecutar el codigo sobre encabezados
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                return;
            }

            // comprueba que se haya modificado cantidad o costo
            if (dgvDetalleIngreso.Columns[e.ColumnIndex].Name != "colCantidad" && dgvDetalleIngreso.Columns[e.ColumnIndex].Name != "colCosto")
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
            // recorre todas las filas del detalle recorre cada fila de la coleccion; el bloque se aplica a un elemento por vez
            foreach (DataGridViewRow fila in dgvDetalleIngreso.Rows)
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
                // muestra el total calculado en la fila filacells["coltotal"]value: valor actual del control numerico o de fecha
                fila.Cells["colTotal"].Value = totalFila;
                // suma la cantidad de la fila
                cantidadTotal += cantidad;
                // suma el importe de la fila
                importeTotal += totalFila;
            }

            // muestra la cantidad total de unidades labelcantidadingresadatext: texto visible en el control; en un campo de entrada tambien representa su contenido
            labelCantidadIngresada.Text = cantidadTotal.ToString();
            // muestra el importe total del ingreso lbltotalingresodinerotext: texto visible en el control; en un campo de entrada tambien representa su contenido
            LblTotalIngresoDinero.Text = importeTotal.ToString("C2");
        }

        // carga el producto completo en el sector de edicion
        private void CargarProductoParaModificar(producto productoSeleccionado)
        {
            // guarda el identificador del producto
            idProductoEnEdicion = productoSeleccionado.IdProducto;
            // carga los datos generales txtnombreproductotext: texto visible en el control; en un campo de entrada tambien representa su contenido
            txtNombreProducto.Text = productoSeleccionado.NombreProducto;
            txtCategoriaProducto.Text = productoSeleccionado.Categoria;
            txtMarcaProducto.Text = productoSeleccionado.Marca;
            txtDescripcionProducto.Text = productoSeleccionado.Descripcion;
            // limpia las variantes anteriores vacia la coleccion o campo indicado revisar si es una carga temporal antes de reutilizarlo
            variantesTemporales.Clear();
            // carga todas las variantes del producto recorre cada variante de la coleccion; el bloque se aplica a un elemento por vez
            foreach (VarianteProducto variante in productoSeleccionado.Variantes)
            {
                variantesTemporales.Add(variante);
            }

            // lleva el cursor al nombre
            txtNombreProducto.Focus();
        }

        // ejecuta la accion de btn agregar variante cuando el usuario pulsa el control
        private void btnAgregarVariante_Click(object sender, EventArgs e)
        {
            // distingue un dato vacio de un valor ingresado antes de continuar
            if (string.IsNullOrWhiteSpace(txtColorVariante.Text))
            {
                MessageBox.Show("Ingrese el color de la variante");
                txtColorVariante.Focus();
                return;
            }

            // distingue un dato vacio de un valor ingresado antes de continuar
            if (string.IsNullOrWhiteSpace(cboCondicionVariante.Text))
            {
                MessageBox.Show("Seleccione la condición de la variante");
                cboCondicionVariante.Focus();
                return;
            }

            if (nudPrecio.Value <= 0)
            {
                MessageBox.Show("Ingrese un precio mayor que cero");
                nudPrecio.Focus();
                return;
            }

            bool varianteRepetida = variantesTemporales.Any(variante => variante.Color.Equals(txtColorVariante.Text.Trim(), StringComparison.OrdinalIgnoreCase) && variante.CondicionProducto.Equals(cboCondicionVariante.Text.Trim(), StringComparison.OrdinalIgnoreCase));
            if (varianteRepetida)
            {
                MessageBox.Show("Ya agregaste una variante con ese color y condición");
                return;
            }

            VarianteProducto nuevaVariante = new VarianteProducto
            {
                Color = txtColorVariante.Text.Trim(),
                CondicionProducto = cboCondicionVariante.Text.Trim(),
                Precio = nudPrecio.Value

            };
            variantesTemporales.Add(nuevaVariante);
            // muestra el modelo del producto en la ultima fila agregada
            int ultimaFila = dgvVariantes.Rows.Count - 1;
            // muestra el nombre del producto dgvvariantesrows[ultimafila]cells["nombreproducto"]value: valor actual del control numerico o de fecha
            dgvVariantes.Rows[ultimaFila].Cells["NombreProducto"].Value = txtNombreProducto.Text.Trim();
            // dgvvariantesrows[ultimafila]cells["colmodelo2"]value: valor actual del control numerico o de fecha
            dgvVariantes.Rows[ultimaFila].Cells["Colmodelo2"].Value = txtModeloProducto.Text.Trim();
            LimpiarCamposVariante();
            txtColorVariante.Focus();
        }

        // guarda el producto junto con todas sus variantes atiende la pulsacion de btnguardarproducto y ejecuta la accion asociada el evento no significa que la operacion ya se haya guardado
        private void btnGuardarProducto_Click(object sender, EventArgs e)
        {
            // verifica que se haya ingresado el modelo o codigo
            if (string.IsNullOrWhiteSpace(txtNombreProducto.Text))
            {
                MessageBox.Show("Ingrese el modelo o código del producto", "Dato requerido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombreProducto.Focus();
                return;
            }

            // verifica que se haya ingresado la categoria
            if (string.IsNullOrWhiteSpace(txtCategoriaProducto.Text))
            {
                MessageBox.Show("Ingrese la categoría", "Dato requerido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCategoriaProducto.Focus();
                return;
            }

            // verifica que se haya ingresado la marca
            if (string.IsNullOrWhiteSpace(txtMarcaProducto.Text))
            {
                MessageBox.Show("Ingrese la marca", "Dato requerido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMarcaProducto.Focus();
                return;
            }

            // verifica que exista al menos una variante
            if (variantesTemporales.Count == 0)
            {
                MessageBox.Show("Agregue al menos una variante antes de guardar", "Variante requerida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtColorVariante.Focus();
                return;
            }

            // crea el producto con sus datos y variantes
            producto nuevoProducto = ObtenerProductoDelFormulario();
            // crea una instancia de la capa de negocio
            CN_Producto negocioProducto = new CN_Producto();
            // envia el producto para guardarlo
            bool guardado;
            try
            {
                guardado = negocioProducto.NuevoProducto(nuevoProducto);
            }
            // atiende el error de esta operacion antes de informar o propagar el problema
            catch (InvalidOperationException ex)
            {
                // presenta el motivo indicado por negocio y conserva los datos para corregirlos
                MessageBox.Show(this, ex.Message, "Revise los datos del producto", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // atiende el error de esta operacion antes de informar o propagar el problema
            catch (Exception)
            {
                // evita mostrar detalles internos de sql al usuario
                MessageBox.Show(this, "No se pudo guardar el producto. Verifique la conexión e intente nuevamente.", "Error al guardar", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // verifica el resultado
            if (guardado)
            {
                // registra la creacion exitosa del producto
                cnLogActividad.Registrar("CREAR_PRODUCTO", "Producto", null, "Producto creado: " + nuevoProducto.NombreProducto);
                MessageBox.Show("Producto y variantes guardados correctamente", "Producto guardado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // limpia completamente el formulario
                CargarOpcionesProducto();
                LimpiarFormulario();
            }
            else
            {
                MessageBox.Show("No se pudo guardar el producto", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // cancela la operacion y limpia el formulario atiende la pulsacion de btncancelar y ejecuta la accion asociada el evento no significa que la operacion ya se haya guardado
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult respuesta = MessageBox.Show("¿Desea cancelar la operación y borrar los datos ingresados?", "Cancelar operación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            // detiene la operacion si el usuario elige no
            if (respuesta == DialogResult.No)
            {
                return;
            }

            // limpia completamente el formulario
            LimpiarFormulario();
        }

        // limpia solamente los campos utilizados por una variante
        private void LimpiarCamposVariante()
        {
            txtColorVariante.SelectedIndex = -1;
            txtColorVariante.Text = string.Empty;
            cboCondicionVariante.SelectedIndex = -1;
            nudPrecio.Value = nudPrecio.Minimum;
        }

        // limpia todos los datos del formulario
        private void CargarOpcionesProducto()
        {
            // reutiliza los colores guardados y conserva la posibilidad de ingresar otro al crear una variante
            txtColorVariante.Items.Clear();
            txtColorVariante.Items.AddRange(new CN_Producto().ObtenerColores().ToArray());
            txtMarcaProducto.Items.Clear();
            txtMarcaProducto.Items.AddRange(new CN_Marca().obtenerMarcas().Select(marca => marca.NombreMarca).ToArray());
            txtCategoriaProducto.Items.Clear();
            txtCategoriaProducto.Items.AddRange(new CN_Categoria().obtenerCategorias().Select(categoria => categoria.NombreCategoria).ToArray());
        }

        // restablece formulario para preparar la siguiente operacion
        private void LimpiarFormulario()
        {
            // limpia los datos generales del producto vacia la coleccion o campo indicado revisar si es una carga temporal antes de reutilizarlo
            txtNombreProducto.Clear();
            txtModeloProducto.Clear();
            txtCategoriaProducto.Text = "";
            txtMarcaProducto.Text = "";
            txtDescripcionProducto.Clear();
            // limpia los campos de la variante
            LimpiarCamposVariante();
            // vacia la lista de variantes temporales vacia la coleccion o campo indicado revisar si es una carga temporal antes de reutilizarlo
            variantesTemporales.Clear();
            // coloca el cursor en el primer campo
            txtNombreProducto.Focus();
        }

        // manejador sin instrucciones no realiza ninguna accion adicional
        private void pnlCliente_Paint(object sender, PaintEventArgs e)
        {
        }

        // ejecuta la accion de lbl titulo nueva venta cuando el usuario pulsa el control
        private void lblTituloNuevaVenta_Click(object sender, EventArgs e)
        {
        }

        // ejecuta la accion de lbl titulo cliente cuando el usuario pulsa el control
        private void lblTituloCliente_Click(object sender, EventArgs e)
        {
        }

        // actualiza los controles relacionados cuando cambia la seleccion puede dispararse tambien al cargar los datos
        private void cboCategoria_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        // ejecuta la accion de label9 cuando el usuario pulsa el control
        private void label9_Click(object sender, EventArgs e)
        {
        }

        // manejador sin instrucciones no realiza ninguna accion adicional
        private void panel2_Paint(object sender, PaintEventArgs e)
        {
        }

        // actualiza los controles relacionados cuando cambia la seleccion puede dispararse tambien al cargar los datos
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        // ejecuta la accion de btn limpiar filtros cuando el usuario pulsa el control
        private void BtnLimpiarFiltros_Click(object sender, EventArgs e)
        {
            // limpia el nombre o modelo ingresado vacia la coleccion o campo indicado revisar si es una carga temporal antes de reutilizarlo
            txtBuscarProducto.Clear();
            // quita las selecciones de los filtros comboboxcateogriaselectedindex: indice de la opcion actual; menos uno significa que no hay una opcion seleccionada
            comboBoxCateogria.SelectedIndex = -1;
            cboMarca.SelectedIndex = -1;
            cboFiltroCondicion.SelectedIndex = -1;
            // elimina los resultados de la busqueda anterior vacia la coleccion o campo indicado revisar si es una carga temporal antes de reutilizarlo
            dgvBusquedaProductos.Rows.Clear();
            // deja el cursor en el campo de busqueda
            txtBuscarProducto.Focus();
        }

        // actualiza los controles relacionados cuando cambia la seleccion puede dispararse tambien al cargar los datos
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        // actualiza los controles relacionados cuando cambia la seleccion puede dispararse tambien al cargar los datos
        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        // manejador sin instrucciones no realiza ninguna accion adicional
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
        }

        // responde al cambio del texto; revisa el estado de busqueda o de seleccion antes de continuar
        private void textBoxCodigoProducto_TextChanged(object sender, EventArgs e)
        {
        }

        // responde al cambio del texto; revisa el estado de busqueda o de seleccion antes de continuar
        private void txtNombreProducto_TextChanged(object sender, EventArgs e)
        {
        }

        // responde al cambio del texto; revisa el estado de busqueda o de seleccion antes de continuar
        private void txtCategoriaProducto_TextChanged(object sender, EventArgs e)
        {
        }

        // responde al cambio del texto; revisa el estado de busqueda o de seleccion antes de continuar
        private void txtMarcaProducto_TextChanged(object sender, EventArgs e)
        {
        }

        // responde al cambio del texto; revisa el estado de busqueda o de seleccion antes de continuar
        private void txtColorVariante_TextChanged(object sender, EventArgs e)
        {
        }

        // manejador sin instrucciones no realiza ninguna accion adicional
        private void nudPrecio_ValueChanged(object sender, EventArgs e)
        {
        }

        // responde al cambio del texto; revisa el estado de busqueda o de seleccion antes de continuar
        private void textBoxOtroAtributo_TextChanged(object sender, EventArgs e)
        {
        }

        // responde al cambio del texto; revisa el estado de busqueda o de seleccion antes de continuar
        private void txtDescripcionProducto_TextChanged(object sender, EventArgs e)
        {
        }

        // responde al cambio del texto; revisa el estado de busqueda o de seleccion antes de continuar
        private void txtBuscarProducto_TextChanged(object sender, EventArgs e)
        {
        }

        // actualiza los controles relacionados cuando cambia la seleccion puede dispararse tambien al cargar los datos
        private void comboBoxElegirVariante_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        // ejecuta la accion de label12 cuando el usuario pulsa el control
        private void label12_Click(object sender, EventArgs e)
        {
        }

        // interpreta la columna y fila pulsadas para ejecutar la accion de esa celda; los encabezados no son registros
        private void dgvVariantes_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        // ejecuta la accion de btn buscar producto cuando el usuario pulsa el control
        private void btnBuscarProducto_Click(object sender, EventArgs e)
        {
            string nombreProducto = txtBuscarProducto.Text.Trim();
            string modelo = txtModeloProducto.Text.Trim();
            int? idCategoria;
            if (comboBoxCateogria.SelectedIndex == -1)
            {
                idCategoria = null;
            }
            else
            {
                idCategoria = Convert.ToInt32(comboBoxCateogria.SelectedValue);
            }
            int? idMarca;
            if (cboMarca.SelectedIndex == -1)
            {
                idMarca = null;
            }
            else
            {
                idMarca = Convert.ToInt32(cboMarca.SelectedValue);
            }
            string condicionProducto;
            if (cboFiltroCondicion.SelectedIndex == -1)
            {
                condicionProducto = string.Empty;
            }
            else
            {
                condicionProducto = cboFiltroCondicion.Text.Trim();
            }
            CN_Producto cnProducto = new CN_Producto();
            List<producto> productosEncontrados = cnProducto.buscarProducto(nombreProducto, idCategoria, idMarca, condicionProducto, modelo);
            // elimina los resultados anteriores vacia la coleccion o campo indicado revisar si es una carga temporal antes de reutilizarlo
            dgvBusquedaProductos.Rows.Clear();
            // recorre los productos encontrados recorre cada productoencontrado de la coleccion; el bloque se aplica a un elemento por vez
            foreach (producto productoEncontrado in productosEncontrados)
            {
                // recorre las variantes del producto recorre cada varianteencontrada de la coleccion; el bloque se aplica a un elemento por vez
                foreach (VarianteProducto varianteEncontrada in productoEncontrado.Variantes)
                {
                    // agrega la variante y guarda el indice de la nueva fila
                    int indiceFila = dgvBusquedaProductos.Rows.Add(varianteEncontrada.IdVariante, productoEncontrado.Modelo, productoEncontrado.NombreProducto, productoEncontrado.Descripcion, varianteEncontrada.Color, varianteEncontrada.CondicionProducto, varianteEncontrada.Precio, varianteEncontrada.Costo, varianteEncontrada.StockFisico, varianteEncontrada.StockComprometido);
                    // coloca modificar en el boton de la fila agregada dgvbusquedaproductosrows[indicefila]cells["colmodificar"]value: valor actual del control numerico o de fecha
                    dgvBusquedaProductos.Rows[indiceFila].Cells["colModificar"].Value = "Modificar";
                }
            }
        }

        // actualiza los controles relacionados cuando cambia la seleccion puede dispararse tambien al cargar los datos
        private void cboMarca_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        // interpreta la columna y fila pulsadas para ejecutar la accion de esa celda; los encabezados no son registros
        private void dgvBusquedaProductos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // evita clics en los encabezados
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                return;
            }

            // obtiene el nombre de la columna presionada
            string nombreColumna = dgvBusquedaProductos.Columns[e.ColumnIndex].Name;
            // desactiva la variante seleccionada
            if (nombreColumna == "ColEliminar")
            {
                // obtiene el identificador de la variante
                int idVariante = Convert.ToInt32(dgvBusquedaProductos.Rows[e.RowIndex].Cells["colIdVariante"].Value);
                // solicita confirmacion antes de desactivar
                DialogResult respuesta = MessageBox.Show("¿Desea desactivar esta variante?", "Confirmar desactivación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                // cancela la operacion si el usuario selecciona no
                if (respuesta == DialogResult.No)
                {
                    return;
                }

                try
                {
                    // envia la solicitud a la capa de negocio
                    CN_Producto cnProducto = new CN_Producto();
                    bool desactivada = cnProducto.DesactivarVariante(idVariante);
                    // informa si la variante no pudo desactivarse
                    if (!desactivada)
                    {
                        MessageBox.Show("La variante no pudo desactivarse.");
                        return;
                    }

                    // registra la desactivacion exitosa de la variante
                    cnLogActividad.Registrar("DESACTIVAR_VARIANTE", "VarianteProducto", idVariante, "Variante desactivada.");
                    // informa que la variante fue desactivada
                    MessageBox.Show("Variante desactivada correctamente.");
                    // vuelve a ejecutar la busqueda
                    btnBuscarProducto.PerformClick();
                }
                // atiende el error de esta operacion antes de informar o propagar el problema
                catch (Exception error)
                {
                    // muestra el error producido
                    MessageBox.Show("Error al desactivar la variante: " + error.Message);
                }

                return;
            }

            // solo continua si se presiono el boton modificar
            if (nombreColumna != "colModificar")
            {
                return;
            }

            // obtiene la fila seleccionada
            DataGridViewRow fila = dgvBusquedaProductos.Rows[e.RowIndex];
            // primer clic: comienza la modificacion
            if (filaEnEdicion == -1)
            {
                // guarda que fila se esta editando
                filaEnEdicion = e.RowIndex;
                // cambia el texto del boton mientras se modifica la fila filacells["colmodificar"]value: valor actual del control numerico o de fecha
                fila.Cells["colModificar"].Value = "Guardar modificación";
                // selecciona la celda del modelo
                dgvBusquedaProductos.CurrentCell = fila.Cells["colModelo"];
                // espera a que termine el clic y abre la edicion
                BeginInvoke(new Action(() =>
                {
                    dgvBusquedaProductos.BeginEdit(true);
                }));
                return;
            }

            // impide modificar simultaneamente otra fila
            if (filaEnEdicion != e.RowIndex)
            {
                MessageBox.Show("Primero guarde la fila que está modificando.");
                return;
            }

            // finaliza la edicion de la celda actual
            dgvBusquedaProductos.EndEdit();
            // obtiene el identificador de la variante
            int idVarianteModificar = Convert.ToInt32(fila.Cells["colIdVariante"].Value);
            // obtiene el modelo modificado
            string modelo = Convert.ToString(fila.Cells["colModelo"].Value)?.Trim() ?? string.Empty;
            // obtiene el nombre del producto
            string nombreProducto = Convert.ToString(fila.Cells["colNombre2"].Value)?.Trim() ?? string.Empty;
            // obtiene la descripcion
            string descripcion = Convert.ToString(fila.Cells["colDescripcion"].Value)?.Trim() ?? string.Empty;
            // obtiene el color
            string color = Convert.ToString(fila.Cells["colColorb"].Value)?.Trim() ?? string.Empty;
            // obtiene la condicion
            string condicion = Convert.ToString(fila.Cells["colCondicion"].Value)?.Trim() ?? string.Empty;
            // valida los campos obligatorios
            if (string.IsNullOrWhiteSpace(modelo) || string.IsNullOrWhiteSpace(nombreProducto) || string.IsNullOrWhiteSpace(color) || string.IsNullOrWhiteSpace(condicion))
            {
                MessageBox.Show("El modelo, el producto, el color y la condición son obligatorios.");
                return;
            }

            // valida el costo
            bool costoValido = decimal.TryParse(Convert.ToString(fila.Cells["colCosto2"].Value), out decimal costo);
            if (!costoValido || costo < 0)
            {
                MessageBox.Show("Ingrese un costo válido.");
                return;
            }

            // valida el precio
            bool precioValido = decimal.TryParse(Convert.ToString(fila.Cells["colPrecio"].Value), out decimal precio);
            if (!precioValido || precio < 0)
            {
                MessageBox.Show("Ingrese un precio válido.");
                return;
            }

            // valida el stock fisico
            bool stockFisicoValido = int.TryParse(Convert.ToString(fila.Cells["colStockFisicoBusqueda"].Value), out int stockFisico);
            if (!stockFisicoValido || stockFisico < 0)
            {
                MessageBox.Show("Ingrese un stock físico válido.");
                return;
            }

            try
            {
                // envia los cambios a la capa de negocio
                CN_Producto cnProducto = new CN_Producto();
                bool actualizado = cnProducto.ActualizarProductoDesdeStock(idVarianteModificar, modelo, nombreProducto, descripcion, color, condicion, costo, precio, stockFisico);
                // informa si no se pudo actualizar
                if (!actualizado)
                {
                    MessageBox.Show("No se pudo actualizar el producto.");
                    return;
                }

                // registra la modificacion exitosa del producto o precio
                cnLogActividad.Registrar("MODIFICAR_PRODUCTO", "VarianteProducto", idVarianteModificar, "Producto: " + nombreProducto + " | Precio nuevo: " + precio + " | Costo nuevo: " + costo);
                // vuelve a colocar el texto modificar filacells["colmodificar"]value: valor actual del control numerico o de fecha
                fila.Cells["colModificar"].Value = "Modificar";
                // indica que ya no hay ninguna fila en edicion
                filaEnEdicion = -1;
                // informa que la actualizacion termino
                MessageBox.Show("Producto actualizado correctamente.");
                // vuelve a cargar la busqueda desde la base
                btnBuscarProducto.PerformClick();
            }
            // atiende el error de esta operacion antes de informar o propagar el problema
            catch (Exception ex)
            {
                // muestra el error producido
                MessageBox.Show("Error al actualizar: " + ex.Message);
            }
        }

        // manejador sin instrucciones no realiza ninguna accion adicional
        private void pnlEncabezadostock_Paint(object sender, PaintEventArgs e)
        {
        }

        // manejador sin instrucciones no realiza ninguna accion adicional
        private void panel3_Paint(object sender, PaintEventArgs e)
        {
        }

        // identifica la fila y la columna pulsadas antes de ejecutar la accion de la celda
        private void dgvBusquedaProductos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // evita clics en los encabezados
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                return;
            }

            // solo permite editar la fila en modificacion
            if (e.RowIndex != filaEnEdicion)
            {
                return;
            }

            // obtiene el nombre de la columna seleccionada
            string nombreColumna = dgvBusquedaProductos.Columns[e.ColumnIndex].Name;
            // impide editar identificadores, modelo y columnas de acciones columnas que no pueden modificarse
            if (nombreColumna == "colIdVariante" || nombreColumna == "colComprometidoBusqueda" || nombreColumna == "colModificar" || nombreColumna == "ColEliminar")
            {
                return;
            }

            // obtiene la celda seleccionada
            DataGridViewCell celda = dgvBusquedaProductos.Rows[e.RowIndex].Cells[e.ColumnIndex];
            // no intenta editar una celda bloqueada
            if (celda.ReadOnly)
            {
                return;
            }

            // selecciona la celda
            dgvBusquedaProductos.CurrentCell = celda;
            // abre la edicion de la celda
            BeginInvoke(new Action(() =>
            {
                dgvBusquedaProductos.BeginEdit(true);
            }));
        }

        // ejecuta la accion de icon button1 cuando el usuario pulsa el control
        private void iconButton1_Click(object sender, EventArgs e)
        {
            // crea la instancia del formulario de seleccion
            buttonSeleccionarProducto formulario = new buttonSeleccionarProducto();
            // se suscribe para recibir la variante y el producto seleccionado
            formulario.OnVarianteSeleccionada += (variante, producto) =>
            {
                // comprueba el ingreso actual para impedir repetidos incluso si se vuelve a abrir el selector
                if (ingresoTemporales.Any(item => item.IdVariante == variante.IdVariante))
                {
                    MessageBox.Show(formulario, "Ese producto y variante ya están agregados al ingreso. Modifique la cantidad en la fila existente.",
                        "Ingreso de mercadería", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                // agrega la variante a la lista del ingreso
                ingresoTemporales.Add(variante);
                // rellena la ultima fila agregada con informacion (dejar cantidad y costo vacios)
                int ultimaFila = dgvDetalleIngreso.Rows.Count - 1;
                if (ultimaFila >= 0)
                {
                    if (dgvDetalleIngreso.Columns.Contains("colProducto"))
                    {
                        // dgvdetalleingresorows[ultimafila]cells["colproducto"]value: valor actual del control numerico o de fecha
                        dgvDetalleIngreso.Rows[ultimaFila].Cells["colProducto"].Value = producto.NombreProducto;
                    }

                    if (dgvDetalleIngreso.Columns.Contains("colVarianteIngreso"))
                    {
                        // dgvdetalleingresorows[ultimafila]cells["colvarianteingreso"]value: valor actual del control numerico o de fecha
                        dgvDetalleIngreso.Rows[ultimaFila].Cells["colVarianteIngreso"].Value = variante.Color;
                    }

                    if (dgvDetalleIngreso.Columns.Contains("colCondicionIngreso"))
                    {
                        // dgvdetalleingresorows[ultimafila]cells["colcondicioningreso"]value: valor actual del control numerico o de fecha
                        dgvDetalleIngreso.Rows[ultimaFila].Cells["colCondicionIngreso"].Value = variante.CondicionProducto;
                    }

                    if (dgvDetalleIngreso.Columns.Contains("colCosto"))
                    {
                        // dgvdetalleingresorows[ultimafila]cells["colcosto"]value: valor actual del control numerico o de fecha
                        dgvDetalleIngreso.Rows[ultimaFila].Cells["colCosto"].Value = DBNull.Value;
                    }

                    if (dgvDetalleIngreso.Columns.Contains("colCantidad"))
                    {
                        // dgvdetalleingresorows[ultimafila]cells["colcantidad"]value: valor actual del control numerico o de fecha
                        dgvDetalleIngreso.Rows[ultimaFila].Cells["colCantidad"].Value = DBNull.Value;
                    }

                    if (dgvDetalleIngreso.Columns.Contains("colTotal"))
                    {
                        // dgvdetalleingresorows[ultimafila]cells["coltotal"]value: valor actual del control numerico o de fecha
                        dgvDetalleIngreso.Rows[ultimaFila].Cells["colTotal"].Value = DBNull.Value;
                    }
                }
            };
            // abre con show() para que permanezca abierta de fondo y libre
            formulario.Show();
        }

        // quita la variante del ingreso en preparacion y recalcula el importe de las filas restantes
        private void dgvDetalleIngreso_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0 || dgvDetalleIngreso.Columns[e.ColumnIndex].Name != "colEliminarIngreso")
            {
                return;
            }

            if (dgvDetalleIngreso.Rows[e.RowIndex].DataBoundItem is VarianteProducto variante)
            {
                ingresoTemporales.Remove(variante);
                ActualizarTotalesIngreso();
            }
        }

        // ejecuta la accion de icon button agregar ingreso cuando el usuario pulsa el control
        private void iconButtonAgregarIngreso_Click(object sender, EventArgs e)
        {
        }

        // restablece ingreso para preparar la siguiente operacion
        private void LimpiarIngreso()
        {
            ingresoTemporales.Clear();
            comboBoxSeleccionaProveedor.SelectedIndex = -1;
            comboBoxSeleccionaProveedor.Text = "";
            txtDocumentoProveedor.Clear();
            dtpFechaIngresoStock.Value = DateTime.Today;
            ActualizarTotalesIngreso();
            comboBoxSeleccionaProveedor.Focus();
        }

        // ejecuta la accion de button registrar ingreso cuando el usuario pulsa el control
        private void ButtonRegistrarIngreso_Click(object sender, EventArgs e)
        {
            dgvDetalleIngreso.EndEdit();
            string proveedorSeleccionado = comboBoxSeleccionaProveedor.Text.Trim();
            // comprueba si hay elementos para continuar con esta operacion
            if (ingresoTemporales.Count == 0)
            {
                MessageBox.Show("No hay productos para ingresar.");
                return;
            }

            // distingue un dato vacio de un valor ingresado antes de continuar
            if (string.IsNullOrWhiteSpace(proveedorSeleccionado))
            {
                MessageBox.Show("Seleccione o escriba un proveedor.");
                return;
            }

            DateTime fecha = dtpFechaIngresoStock.Value;
            string remito = txtDocumentoProveedor.Text.Trim();
            // distingue un dato vacio de un valor ingresado antes de continuar
            if (string.IsNullOrWhiteSpace(remito))
            {
                MessageBox.Show("Ingrese el número o código del documento del proveedor (remito).", "Falta el documento del proveedor", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDocumentoProveedor.Focus();
                return;
            }

            List<(int idVariante, int cantidad, decimal costoUnitario)> detalles = new List<(int idVariante, int cantidad, decimal costoUnitario)>();
            List<string> productosIngresados = new List<string>();
            // procesa cada fila de dgv detalle ingresorows sin cambiar el orden de la coleccion
            foreach (DataGridViewRow fila in dgvDetalleIngreso.Rows)
            {
                if (fila.IsNewRow)
                {
                    continue;
                }

                object idVarObj = fila.Cells["idVariante"].Value;
                if (idVarObj == null)
                {
                    continue;
                }

                int idVariante = Convert.ToInt32(idVarObj);
                object cantObj = fila.Cells["colCantidad"].Value;
                // distingue un dato vacio de un valor ingresado antes de continuar
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
                object costoObj = fila.Cells["colCosto"].Value;
                // distingue un dato vacio de un valor ingresado antes de continuar
                if (costoObj != null && !string.IsNullOrWhiteSpace(costoObj.ToString()))
                {
                    if (!decimal.TryParse(costoObj.ToString(), out costo))
                    {
                        MessageBox.Show("Ingrese un costo válido.");
                        return;
                    }
                }

                detalles.Add((idVariante, cantidad, costo));
                string nombreProducto = Convert.ToString(fila.Cells["colProducto"].Value)?.Trim() ?? $"Variante {idVariante}";
                string color = Convert.ToString(fila.Cells["colVarianteIngreso"].Value)?.Trim() ?? "";
                string condicion = Convert.ToString(fila.Cells["colCondicionIngreso"].Value)?.Trim() ?? "";
                string variante = string.Join(", ", new[] { color, condicion }.Where(dato => !string.IsNullOrWhiteSpace(dato)));
                productosIngresados.Add($"{nombreProducto}{(variante.Length == 0 ? "" : $" ({variante})")} x {cantidad}");
            }

            CN_Producto negocio = new CN_Producto();
            bool ok;
            try
            {
                ok = negocio.RegistrarIngreso(proveedorSeleccionado, fecha, remito, detalles);
            }
            // atiende el error de esta operacion antes de informar o propagar el problema
            catch (Exception error)
            {
                MessageBox.Show("No se pudo registrar el ingreso: " + error.Message);
                return;
            }

            if (ok)
            {
                LimpiarIngreso();
                try
                {
                    comboBoxSeleccionaProveedor.Items.Clear();
                    comboBoxSeleccionaProveedor.Items.AddRange(new CN_Proveedor().buscarProveedor(string.Empty).Select(proveedor => proveedor.nombreProveedor).ToArray());
                    comboBoxSeleccionaProveedor.SelectedIndex = -1;
                    comboBoxSeleccionaProveedor.Text = "";
                    string detalleIngreso = $"Proveedor: {proveedorSeleccionado} | Doc.proveedor: {remito} | Productos: {string.Join("; ", productosIngresados)}";
                    cnLogActividad.Registrar("REGISTRAR_INGRESO_STOCK", "IngresoStock", null, detalleIngreso);
                    MessageBox.Show("Ingreso registrado correctamente.");
                }
                // atiende el error de esta operacion antes de informar o propagar el problema
                catch (Exception ex)
                {
                    MessageBox.Show("El ingreso se guardó, pero no se pudo actualizar la lista o el historial: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Error al registrar el ingreso.");
            }
        }

        // ejecuta la accion de lbl remito proveedor cuando el usuario pulsa el control
        private void lblRemitoProveedor_Click(object sender, EventArgs e)
        {
        }

        // lee los campos actuales y construye la solicitud; este metodo no guarda cambios ni modifica la seleccion
        private producto ObtenerProductoDelFormulario()
        {
            return new producto
            {
                NombreProducto = txtNombreProducto.Text.Trim(),
                Modelo = txtModeloProducto.Text.Trim(),
                Categoria = txtCategoriaProducto.Text.Trim(),
                Marca = txtMarcaProducto.Text.Trim(),
                Descripcion = txtDescripcionProducto.Text.Trim(),
                // envia una copia de la lista temporal
                Variantes = new List<VarianteProducto>(variantesTemporales)

            };
        }
    }
}
