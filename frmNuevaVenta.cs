// guia del archivo: el formulario mantiene cliente, productos y pagos en preparacion elegir una opcion no la guarda en una venta: agregarproducto crea una fila temporal detallesventa conserva cantidades y precios; pagosventa conserva base y recargo de cada cobro totalbase corresponde a productos menos descuento mas envio; totalconrecargos agrega los recargos ya incorporados en este formulario importe cobrado representa dinero recibido con recargo incluido registrarventa envia la carga completa al servicio; solo tras obtener el identificador se considera guardada abrir o imprimir documentos es posterior al guardado: si falla, no se debe repetir la venta los handlers con sufijo de evento son llamados por los controles; no ejecutarlos por duplicado al conectarlos
using Prana.Entidades;
using Prana.Negocio;
using Prana.negocio;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Prana.Presentacion
{
    // agrupa los datos y operaciones de frm nueva venta
    public partial class frmNuevaVenta : Form
    {
        // servicios y estado temporal de la venta
        private readonly CN_Cliente clientesVenta = new();
        // conserva productos venta entre las operaciones de esta clase
        private readonly CN_Producto productosVenta = new();
        // conserva medios venta entre las operaciones de esta clase
        private readonly CN_MedioPago mediosVenta = new();
        // conserva registro ventas entre las operaciones de esta clase
        private readonly CN_RegistroVenta registroVentas = new();
        // agrupa las teclas rapidas y consulta una vez, 250 ms despues de que el usuario deja de escribir
        private readonly System.Windows.Forms.Timer esperaClientes = new()
        {
            Interval = 250
        };
        // evita que restaurar el texto del combo dispare otra busqueda como si el usuario estuviera escribiendo
        private bool actualizandoListaClientes;
        // conserva cliente elegido entre las operaciones de esta clase
        private Cliente? clienteElegido;
        // conserva producto elegido entre las operaciones de esta clase
        private OpcionProducto? productoElegido;
        // conserva detalles venta entre las operaciones de esta clase
        private readonly List<DetalleNuevaVenta> detallesVenta = new();
        // conserva pagos venta entre las operaciones de esta clase
        private readonly List<Pago> pagosVenta = new();
        // conserva venta registrada entre las operaciones de esta clase
        private int ventaRegistrada;
        // conserva monto pago editado entre las operaciones de esta clase
        private bool montoPagoEditado;
        // conserva actualizando monto sugerido entre las operaciones de esta clase
        private bool actualizandoMontoSugerido;
        // inicializa el disenador y conecta los controles con la logica de venta
        public frmNuevaVenta()
        {
            InitializeComponent();
            lblAclaracionDocumentos.Text = "La garantía y el remito se generan con el mismo número de venta.";
            btnRegistrarVenta.Text = "Registrar venta";
            dtpFechaPago.Enabled = false;
            colMontoPago.Visible = colRecargoPago.Visible = false;
            cboTipoLogistica.SelectedIndex = 0;
            // al entrar al combo, abre las opciones que ya se cargaron al iniciar el formulario
            cboCliente.Enter += cboCliente_Enter;
            // conecta el temporizador que ejecuta la busqueda despues de una pausa al escribir
            esperaClientes.Tick += esperaClientes_Tick;
            // libera el temporizador al cerrar el formulario
            Disposed += (_, _) => esperaClientes.Dispose();
            // desactiva las sugerencias locales: las coincidencias se consultan en la base de datos cboclienteautocompletemode: comportamiento nativo de sugerencias al escribir en la lista
            cboCliente.AutoCompleteMode = AutoCompleteMode.None;
            cboCliente.AutoCompleteSource = AutoCompleteSource.None;
        }

        // resuelve actualizar monto sugerido con los datos recibidos y entrega el resultado al codigo que lo llama
        private void ActualizarMontoSugerido()
        {
            decimal saldoBase = Math.Max(0, TotalBase - pagosVenta.Sum(pagoActual => pagoActual.Monto));
            decimal recargo = registroVentas.CalcularRecargo(saldoBase, PorcentajeRecargoSeleccionado());
            decimal sugerido = Math.Clamp(saldoBase + recargo, nudMontoPago.Minimum, nudMontoPago.Maximum);
            actualizandoMontoSugerido = true;
            try
            {
                nudMontoPago.Value = sugerido;
            }
            // realiza la limpieza de salida tanto si la operacion termina bien como si falla
            finally
            {
                actualizandoMontoSugerido = false;
            }
        }

        // resuelve chk incluir sin compras recientes checked changed con los datos recibidos y entrega el resultado al codigo que lo llama
        private void chkIncluirSinComprasRecientes_CheckedChanged(object? sender, EventArgs e)
        {
            // repite la busqueda actual con filtro 0 (activos) o 4 (todos menos eliminados)
            CargarClientes(cboCliente.Text);
        }

        // resuelve cbo cliente enter con los datos recibidos y entrega el resultado al codigo que lo llama
        private void cboCliente_Enter(object? sender, EventArgs e)
        {
            // no hace otra consulta; despliega los resultados ya cargados al iniciar o cambiar el check
            if (cboCliente.Items.Count > 0)
            {
                cboCliente.DroppedDown = true;
            }
        }

        // conserva el cliente elegido
        private void ElegirCliente()
        {
            if (cboCliente.SelectedItem is not OpcionCliente opcion)
            {
                return;
            }

            esperaClientes.Stop();
            clienteElegido = opcion.Cliente;
            lblClienteSeleccionado.Text = opcion.ToString();
            dgvClienteSeleccionado.Rows.Clear();
            // anade una fila visible; el orden de los valores debe coincidir con el de las columnas
            dgvClienteSeleccionado.Rows.Add($"{opcion.Cliente.TipoDocumento}: {opcion.Cliente.DNI}", opcion.Cliente.Nombre, opcion.Cliente.Apellido, opcion.Cliente.Telefono, opcion.Cliente.Direccion, opcion.Cliente.NombreLocalidad, opcion.Cliente.CodigoPostal);
            usarDireccionRegistrada.Checked = false;
            txtDomicilioEntrega.Clear();
            ActualizarEntrega();
        }

        // obtiene y presenta clientes para actualizar los controles de la pantalla
        private void CargarClientes(string? busqueda = null)
        {
            // usa el texto enviado por el temporizador o, al iniciar, el texto que ya tenga el combo
            string texto = busqueda ?? cboCliente.Text;
            try
            {
                actualizandoListaClientes = true;
                cboCliente.Items.Clear();
                int filtroEstadoCliente;
                if (chkIncluirSinComprasRecientes.Checked)
                {
                    filtroEstadoCliente = 4;
                }
                else
                {
                    filtroEstadoCliente = 0;
                }
                // el check marcado usa filtro 4 (activos e inactivos no eliminados); desmarcado usa 0 (solo activos) buscarporprefijo hace que escribir "40" busque valores que comienzan con 40
                cboCliente.Items.AddRange(clientesVenta.buscarCliente(texto, filtroEstadoCliente, buscarPorPrefijo: true).Select(cliente => new OpcionCliente(cliente)).ToArray());
                // quita la seleccion anterior, pero conserva el texto que el usuario esta buscando cboclienteselectedindex: indice de la opcion actual; menos uno significa que no hay una opcion seleccionada
                cboCliente.SelectedIndex = -1;
                cboCliente.Text = texto;
                cboCliente.SelectionStart = cboCliente.Text.Length;
            }
            // atiende el error de esta operacion antes de informar o propagar el problema
            catch (Exception error)
            {
                ErrorVenta(error);
            }
            // realiza la limpieza de salida tanto si la operacion termina bien como si falla
            finally
            {
                actualizandoListaClientes = false;
            }

            // muestra los resultados recien consultados sin esperar a que el usuario vuelva a abrir la lista
            if (cboCliente.Focused && cboCliente.Items.Count > 0)
            {
                cboCliente.DroppedDown = true;
            }
        }

        // igual que localidad en informes: carga las opciones y windows sugiere al escribir
        private void CargarProductos()
        {
            try
            {
                cboBuscarProducto.Items.Clear();
                // procesa cada item de productos ventabuscar producto("", null, null, "", "") sin cambiar el orden de la coleccion
                foreach (producto item in productosVenta.buscarProducto("", null, null, "", ""))
                {
                    // procesa cada variante de itemvariantes sin cambiar el orden de la coleccion
                    foreach (VarianteProducto variante in item.Variantes)
                    {
                        cboBuscarProducto.Items.Add(new OpcionProducto(item, variante));
                    }
                }

                cboBuscarProducto.SelectedIndex = -1;
            }
            // atiende el error de esta operacion antes de informar o propagar el problema
            catch (Exception error)
            {
                ErrorVenta(error);
            }
        }

        // responde al cambio del texto; revisa el estado de busqueda o de seleccion antes de continuar
        private void cboBuscarProducto_TextUpdate(object? sender, EventArgs e)
        {
            productoElegido = null;
        }

        // recibe la opcion que el usuario confirmo en el desplegable; no confundirla con escribir texto sin elegir
        private void cboBuscarProducto_SelectionChangeCommitted(object? sender, EventArgs e)
        {
            productoElegido = cboBuscarProducto.SelectedItem as OpcionProducto;
        }

        // resuelve la variante desde la opcion seleccionada o una coincidencia exacta del texto autocompletado
        private void ElegirProducto()
        {
            // el autocompletado nativo puede completar el texto sin confirmar la seleccion
            productoElegido = cboBuscarProducto.SelectedItem as OpcionProducto;
            if (productoElegido == null)
            {
                int indice = cboBuscarProducto.FindStringExact(cboBuscarProducto.Text.Trim());
                if (indice >= 0)
                {
                    productoElegido = cboBuscarProducto.Items[indice] as OpcionProducto;
                }
            }
        }

        // agrega la variante al detalle temporal y actualiza los totales sin borrar las opciones de productos
        private void AgregarProducto()
        {
            try
            {
                ElegirProducto();
                DetalleNuevaVenta detalle = registroVentas.PrepararProducto(productoElegido?.Producto, productoElegido?.Variante, (int)nudCantidad.Value, detallesVenta);
                detallesVenta.Add(detalle);
                MostrarDetalle();
                // limpia la seleccion y conserva la lista para elegir otro producto
                cboBuscarProducto.DroppedDown = false;
                cboBuscarProducto.SelectedIndex = -1;
                cboBuscarProducto.Text = string.Empty;
                productoElegido = null;
                cboBuscarProducto.Focus();
            }
            // atiende el error de esta operacion antes de informar o propagar el problema
            catch (Exception error)
            {
                ErrorVenta(error);
            }
        }

        // actualiza las filas de productos y vuelve a calcular los importes
        private void MostrarDetalle()
        {
            dgvDetalleVenta.Rows.Clear();
            // procesa cada item de detalles venta sin cambiar el orden de la coleccion
            foreach (DetalleNuevaVenta item in detallesVenta)
            {
                // anade una fila visible; el orden de los valores debe coincidir con el de las columnas
                dgvDetalleVenta.Rows.Add(item.Producto, item.Variante, item.Cantidad, item.PrecioUnitario.ToString("C"), (item.Cantidad * item.PrecioUnitario).ToString("C"), "Quitar", item.StockFisico, item.StockComprometido);
            }

            ActualizarTotales();
        }

        // el total base incluye productos descuento y envio antes de los recargos dato total base de tipo decimal se obtiene al consultar la propiedad mediante la expresion siguiente
        private decimal TotalBase
        {
            get
            {
                decimal costoEnvioAplicable;
                if (rdoEnvioDomicilio.Checked)
                {
                    costoEnvioAplicable = nudCostoEnvio.Value;
                }
                else
                {
                    costoEnvioAplicable = 0;
                }

                return registroVentas.CalcularTotalBase(detallesVenta, nudDescuento.Value, costoEnvioAplicable);
            }
        }

        // el total final suma los recargos de los medios ya agregados dato total con recargos de tipo decimal se obtiene al consultar la propiedad mediante la expresion siguiente
        private decimal TotalConRecargos => TotalBase + pagosVenta.Sum(pagoActual => pagoActual.Recargo);

        // refleja subtotal descuento envio y total en los labels
        private void ActualizarTotales()
        {
            decimal subtotal = detallesVenta.Sum(detalleProducto => detalleProducto.Cantidad * detalleProducto.PrecioUnitario);
            lblSubtotal.Text = subtotal.ToString("C");
            lblDescuentoAplicado.Text = "- " + nudDescuento.Value.ToString("C");
            decimal costoEnvioResumen;
            if (rdoEnvioDomicilio.Checked)
            {
                costoEnvioResumen = nudCostoEnvio.Value;
            }
            else
            {
                costoEnvioResumen = 0m;
            }
            lblEnvio.Text = (costoEnvioResumen).ToString("C");
            if (!montoPagoEditado)
            {
                ActualizarMontoSugerido();
            }

            MostrarPagos();
        }

        // muestra cuotas solo para medios que admiten financiacion
        private void ActualizarCuotas()
        {
            bool mostrar = cboMedioPago.SelectedItem is MedioPago medio && medio.TipoCaja == "Crédito";
            lblCuotasPago.Visible = cboCantCuotas.Visible = mostrar;
            // comprueba si hay elementos para continuar con esta operacion
            if (!mostrar && cboCantCuotas.Items.Count > 0)
            {
                cboCantCuotas.SelectedIndex = 0;
            }
        }

        // obtiene el recargo del medio y las cuotas seleccionadas
        private decimal PorcentajeRecargoSeleccionado()
        {
            if (cboMedioPago.SelectedItem is not MedioPago medio)
            {
                return 0;
            }

            int cuotas;
            if (cboCantCuotas.Visible && cboCantCuotas.SelectedItem != null)
            {
                cuotas = Convert.ToInt32(cboCantCuotas.SelectedItem);
            }
            else
            {
                cuotas = 1;
            }
            return registroVentas.PorcentajeRecargo(medio, cuotas);
        }

        // toma el importe final recibido y separa base y recargo agrega un cobro temporal despues de validar el saldo; los pagos se guardan definitivamente al registrar la venta
        private void AgregarPago()
        {
            try
            {
                if (TotalBase > 0 && pagosVenta.Sum(pagoActual => pagoActual.Monto) >= TotalBase)
                {
                    if (cboCondicionPago.Text == "Pago parcial")
                    {
                        // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                        throw new InvalidOperationException("Ya se cubrió el total de la venta. Seleccione Pago total en la condición del pago inicial y luego registre la venta.");
                    }

                    // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                    throw new InvalidOperationException("Ya se cubrió el total de la venta. No agregue otro pago; presione Registrar venta.");
                }

                int cuotas;
                if (cboCantCuotas.Visible)
                {
                    cuotas = Convert.ToInt32(cboCantCuotas.SelectedItem);
                }
                else
                {
                    cuotas = 1;
                }
                Pago pago = registroVentas.PrepararPago(cboMedioPago.SelectedItem as MedioPago, cuotas, nudMontoPago.Value, TotalBase, pagosVenta, txtCuentaDestino.Text);
                pagosVenta.Add(pago);
                txtCuentaDestino.Clear();
                MostrarPagos();
                montoPagoEditado = false;
                ActualizarMontoSugerido();
                // comprueba si hay elementos para continuar con esta operacion
                if (dgvPagos.Rows.Count > 0)
                {
                    dgvPagos.CurrentCell = dgvPagos.Rows[dgvPagos.Rows.Count - 1].Cells[colMedioPago.Index];
                    dgvPagos.FirstDisplayedScrollingRowIndex = dgvPagos.Rows.Count - 1;
                }
            }
            // atiende el error de esta operacion antes de informar o propagar el problema
            catch (Exception error)
            {
                ErrorVenta(error);
            }
        }

        // muestra base recargo total cobrado y saldo pendiente
        private void MostrarPagos()
        {
            dgvPagos.Rows.Clear();
            // procesa cada pago de pagos venta sin cambiar el orden de la coleccion
            foreach (Pago pago in pagosVenta)
            {
                string descripcionCuotas;
                if (pago.Cuotas > 1)
                {
                    descripcionCuotas = $" ({pago.Cuotas} cuotas)";
                }
                else
                {
                    descripcionCuotas = "";
                }
                // anade una fila visible; el orden de los valores debe coincidir con el de las columnas
                dgvPagos.Rows.Add(pago.Medio + (descripcionCuotas), pago.Monto.ToString("C"), pago.Recargo.ToString("C"), (pago.Monto + pago.Recargo).ToString("C"), "Quitar");
            }

            ActualizarResumenPago();
        }

        // muestra lo cobrado y el importe previsto con el medio seleccionado
        private void ActualizarResumenPago()
        {
            decimal pagadoConRecargos = pagosVenta.Sum(pagoActual => pagoActual.Monto + pagoActual.Recargo);
            decimal saldoBase = Math.Max(0, TotalBase - pagosVenta.Sum(p => p.Monto));
            decimal recargoPrevisto = registroVentas.CalcularRecargo(saldoBase, PorcentajeRecargoSeleccionado());
            decimal totalPrevisto = TotalConRecargos + recargoPrevisto;
            bool estimado = recargoPrevisto > 0;
            if (estimado)
            {
                lblTotalTexto.Text = "TOTAL EST.:";
            }
            else
            {
                lblTotalTexto.Text = "TOTAL:";
            }
            lblTotal.Text = totalPrevisto.ToString("C");
            string etiquetaSaldo;
            if (estimado)
            {
                etiquetaSaldo = "Saldo estimado";
            }
            else
            {
                etiquetaSaldo = "Saldo";
            }
            lblTotalPagoActual.Text = $"Pago: {pagadoConRecargos:C}    {(etiquetaSaldo)}: {Math.Max(0, totalPrevisto - pagadoConRecargos):C}";
            if (cboMedioPago.SelectedItem is MedioPago && saldoBase > 0)
            {
                lblCobroPrevisto.Text = $"A cobrar por el saldo con {cboMedioPago.Text}: {(saldoBase + recargoPrevisto):C}";
            }
            else
            {
                lblCobroPrevisto.Text = string.Empty;
            }
        }

        // habilita los datos de domicilio y suma el envio cuando corresponde
        private void ActualizarEntrega()
        {
            bool envio = rdoEnvioDomicilio.Checked;
            usarDireccionRegistrada.Enabled = envio && !string.IsNullOrWhiteSpace(clienteElegido?.Direccion);
            ActualizarDireccionEntrega();
            if (rdoRetiroPosterior.Checked)
            {
                txtDomicilioEntrega.Clear();
            }

            cboTipoLogistica.Enabled = envio;
            nudCostoEnvio.Enabled = envio;
            dtpFechaEntrega.Enabled = envio || rdoRetiroPosterior.Checked;
            ActualizarTotales();
        }

        // copia la direccion del cliente o permite escribir una distinta
        private void ActualizarDireccionEntrega()
        {
            if (usarDireccionRegistrada.Checked)
            {
                txtDomicilioEntrega.Text = clienteElegido?.Direccion ?? string.Empty;
            }
            else 
                if (clienteElegido != null && txtDomicilioEntrega.Text == clienteElegido.Direccion)
                {
                    txtDomicilioEntrega.Clear();
                }

            txtDomicilioEntrega.Enabled = rdoEnvioDomicilio.Checked && !usarDireccionRegistrada.Checked;
        }

        // prepara los grids las cuotas los medios y la presentacion inicial
        private void CargarVenta()
        {
            CargarClientes();
            CargarProductos();
            dtpFechaPago.Value = DateTime.Today;
            dtpFechaPago.MinDate = DateTime.Today;
            lblMontoPago.Text = "Importe cobrado";
            colMontoPago.HeaderText = "Base de venta";
            colRecargoPago.HeaderText = "Recargo";
            colTotalPago.HeaderText = "Total cobrado";
            colMontoPago.Visible = colRecargoPago.Visible = false;
            btnRegistrarVenta.Text = "Registrar venta";
            lblAclaracionDocumentos.Text = "La garantía y el remito se generan con el mismo número de venta.";
            dgvClienteSeleccionado.Columns.Clear();
            // procesa cada nombre de new[] { "documento", "nombre", "apellido", "telefono", "direccion", "localidad", "codigo postal" } sin cambiar el orden de la coleccion
            foreach (string nombre in new[]
            {
                "Documento",
                "Nombre",
                "Apellido",
                "Teléfono",
                "Dirección",
                "Localidad",
                "Código postal"
            }

            )
            {
                dgvClienteSeleccionado.Columns.Add(nombre, nombre);
            }

            dgvClienteSeleccionado.ReadOnly = true;
            dgvClienteSeleccionado.AllowUserToAddRows = false;
            dgvClienteSeleccionado.RowHeadersVisible = false;
            dgvClienteSeleccionado.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvClienteSeleccionado.Height = 90;
            cboCantCuotas.Items.AddRange(new object[] { 1, 2, 3, 6, 9, 12 });
            cboCantCuotas.SelectedIndex = 0;
            rdoRetiroLocal.Checked = true;
            cboCondicionPago.SelectedItem = "Pago total";
            try
            {
                List<MedioPago> mediosDisponibles = mediosVenta.ObtenerTodos();
                cboMedioPago.DisplayMember = nameof(MedioPago.NombreMedioPago);
                cboMedioPago.DataSource = mediosDisponibles;
                // selecciona efectivo al abrir la venta si esta disponible
                int indiceEfectivo = mediosDisponibles.FindIndex(medioPago => medioPago.NombreMedioPago.Trim().Equals("Efectivo", StringComparison.OrdinalIgnoreCase));
                if (indiceEfectivo >= 0)
                {
                    cboMedioPago.SelectedIndex = indiceEfectivo;
                }
            }
            // atiende el error de esta operacion antes de informar o propagar el problema
            catch (Exception error)
            {
                ErrorVenta(error);
            }

            ActualizarCuotas();
            ActualizarTotales();
        }

        // recoge los datos; negocio valida y registra la venta reune la carga de pantalla, solicita el registro y conserva su numero un error de documentos posterior no debe duplicar la venta
        private void RegistrarVenta()
        {
            if (ventaRegistrada > 0)
            {
                return;
            }

            try
            {
                decimal totalVenta = TotalConRecargos;
                if (totalVenta > 100_000_000m)
                {
                    MessageBox.Show(this, "El máximo permitido para la venta es $100.000.000,00.", "Importe máximo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int id = registroVentas.Registrar(ObtenerVentaDelFormulario(totalVenta));
                lblNumeroVenta.Text = $"Venta N.° {id}";
                btnRegistrarVenta.Enabled = false;
                ventaRegistrada = id;
                try
                {
                    DocumentosPrana.MostrarVenta(this, id, txtSegundoTelefono.Text.Trim());
                }
                // atiende el error de esta operacion antes de informar o propagar el problema
                catch (Exception error)
                {
                    MessageBox.Show(this, $"La venta N.º {id} quedó registrada.\nNo se pudieron abrir o exportar los archivos: {error.Message}\nPuede reimprimirlos desde el panel principal, sin registrar otra venta.", "Venta registrada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            // atiende el error de esta operacion antes de informar o propagar el problema
            catch (Exception ex)
            {
                ErrorVenta(ex);
                return;
            }

            if (MessageBox.Show(this, "La venta se registró correctamente. ¿Desea cargar otra venta?", "Venta registrada", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes && Parent?.FindForm() is Interfaz interfaz)
            {
                interfaz.AbrirFormulario(new frmNuevaVenta());
            }
        }

        // presenta los errores de consulta o registro al usuario
        private static void ErrorVenta(Exception ex) => MessageBox.Show(ex.Message, "Nueva venta", MessageBoxButtons.OK, MessageBoxIcon.Error);
        // conserva el cliente asociado al texto mostrado en el combobox
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
            public override string ToString() => $"{Cliente.Nombre} {Cliente.Apellido} - {Cliente.TipoDocumento}: {Cliente.DNI}";
        }

        // representa una variante de producto en la lista de sugerencias
        private sealed class OpcionProducto
        {
            // conserva producto como producto para intercambiar este dato entre las capas
            public producto Producto
            {
                get;
            }
            // conserva variante como variante producto para intercambiar este dato entre las capas
            public VarianteProducto Variante
            {
                get;
            }

            // prepara opcion producto antes de que se utilice la instancia
            public OpcionProducto(producto producto, VarianteProducto variante)
            {
                Producto = producto;
                Variante = variante;
            }

            // resuelve to string con los datos recibidos y entrega el resultado al codigo que lo llama
            public override string ToString() => $"{Producto.NombreProducto} - {Variante.Color} {Variante.CondicionProducto}";
        }

        // carga los datos cuando se abre el formulario
        private void frmNuevaVenta_Load(object sender, EventArgs e) => CargarVenta();
        // eventos del disenador: el doble clic abre la accion de cada control atiende el evento paint del control o formulario indicado por el nombre del metodo
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
        }

        // interpreta la columna y fila pulsadas para ejecutar la accion de esa celda; los encabezados no son registros
        private void dgvDetalleVenta_CellContentClick(object? sender, DataGridViewCellEventArgs e)
        {
            // comprueba que el clic corresponda a una fila de datos y no al encabezado
            if (e.RowIndex >= 0 && e.ColumnIndex == colEliminar.Index)
            {
                detallesVenta.RemoveAt(e.RowIndex);
                MostrarDetalle();
            }
        }

        // manejador sin instrucciones no realiza ninguna accion adicional
        private void panel1_Paint_1(object sender, PaintEventArgs e)
        {
        }

        // manejador sin instrucciones no realiza ninguna accion adicional
        private void pnlContenidoVenta_Paint(object sender, PaintEventArgs e)
        {
        }

        // manejador sin instrucciones no realiza ninguna accion adicional
        private void panel1_Paint_2(object sender, PaintEventArgs e)
        {
        }

        // manejador sin instrucciones no realiza ninguna accion adicional
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
        }

        // actualiza los controles relacionados cuando cambia la seleccion puede dispararse tambien al cargar los datos
        private void cboCondicionPago_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboCondicionPago.SelectedItem?.ToString() == "Pago total")
            {
                montoPagoEditado = false;
                ActualizarMontoSugerido();
            }
        }

        // actualiza los controles relacionados cuando cambia la seleccion puede dispararse tambien al cargar los datos
        private void cboTipoLogistica_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        // ejecuta la accion de btn registrar venta cuando el usuario pulsa el control
        private void btnRegistrarVenta_Click(object? sender, EventArgs e)
        {
            RegistrarVenta();
        }

        // responde al cambio del texto; revisa el estado de busqueda o de seleccion antes de continuar
        private void cboCliente_TextUpdate(object? sender, EventArgs e)
        {
            // ignora los cambios de texto que cargarclientes hace al repoblar el combo
            if (actualizandoListaClientes)
            {
                return;
            }

            if (clienteElegido != null)
            {
                bool limpiarDireccion = usarDireccionRegistrada.Checked;
                clienteElegido = null;
                lblClienteSeleccionado.Text = "Ningún cliente seleccionado";
                dgvClienteSeleccionado.Rows.Clear();
                usarDireccionRegistrada.Checked = false;
                if (limpiarDireccion)
                {
                    txtDomicilioEntrega.Clear();
                }

                ActualizarEntrega();
            }

            // reinicia la espera en cada tecla para evitar consultar la base de datos por cada caracter
            esperaClientes.Stop();
            esperaClientes.Start();
        }

        // ejecuta la accion pendiente del temporizador, por ejemplo una consulta despues de una pausa al escribir
        private void esperaClientes_Tick(object? sender, EventArgs e)
        {
            esperaClientes.Stop();
            // ejecuta la consulta con el texto final que quedo en el combo
            CargarClientes(cboCliente.Text);
        }

        // recibe la opcion que el usuario confirmo en el desplegable; no confundirla con escribir texto sin elegir
        private void cboCliente_SelectionChangeCommitted(object? sender, EventArgs e)
        {
            ElegirCliente();
        }

        // ejecuta la accion de btn agregar cliente cuando el usuario pulsa el control
        private void BtnAgregarCliente_Click(object? sender, EventArgs e)
        {
            ElegirCliente();
        }

        // ejecuta la accion de btn agregar producto cuando el usuario pulsa el control
        private void btnAgregarProducto_Click(object? sender, EventArgs e)
        {
            AgregarProducto();
        }

        // resuelve nud descuento value changed con los datos recibidos y entrega el resultado al codigo que lo llama
        private void nudDescuento_ValueChanged(object? sender, EventArgs e)
        {
            ActualizarTotales();
        }

        // ejecuta la accion de btn agregar pago cuando el usuario pulsa el control
        private void btnAgregarPago_Click(object? sender, EventArgs e)
        {
            AgregarPago();
        }

        // interpreta la columna y fila pulsadas para ejecutar la accion de esa celda; los encabezados no son registros
        private void dgvPagos_CellContentClick(object? sender, DataGridViewCellEventArgs e)
        {
            // comprueba que el clic corresponda a una fila de datos y no al encabezado
            if (e.RowIndex >= 0 && e.ColumnIndex == colQuitarPago.Index)
            {
                pagosVenta.RemoveAt(e.RowIndex);
                MostrarPagos();
            }
        }

        // resuelve rdo retiro local checked changed con los datos recibidos y entrega el resultado al codigo que lo llama
        private void rdoRetiroLocal_CheckedChanged(object? sender, EventArgs e)
        {
            ActualizarEntrega();
        }

        // resuelve rdo retiro posterior checked changed con los datos recibidos y entrega el resultado al codigo que lo llama
        private void rdoRetiroPosterior_CheckedChanged(object? sender, EventArgs e)
        {
            ActualizarEntrega();
        }

        // resuelve rdo envio domicilio checked changed con los datos recibidos y entrega el resultado al codigo que lo llama
        private void rdoEnvioDomicilio_CheckedChanged(object? sender, EventArgs e)
        {
            ActualizarEntrega();
        }

        // resuelve usar direccion registrada checked changed con los datos recibidos y entrega el resultado al codigo que lo llama
        private void usarDireccionRegistrada_CheckedChanged(object? sender, EventArgs e)
        {
            ActualizarDireccionEntrega();
        }

        // resuelve nud costo envio value changed con los datos recibidos y entrega el resultado al codigo que lo llama
        private void nudCostoEnvio_ValueChanged(object? sender, EventArgs e)
        {
            ActualizarTotales();
        }

        // actualiza los controles relacionados cuando cambia la seleccion puede dispararse tambien al cargar los datos
        private void cboMedioPago_SelectedIndexChanged(object? sender, EventArgs e)
        {
            ActualizarCuotas();
            if (!montoPagoEditado)
            {
                ActualizarMontoSugerido();
            }

            MostrarPagos();
        }

        // actualiza los controles relacionados cuando cambia la seleccion puede dispararse tambien al cargar los datos
        private void cboCantCuotas_SelectedIndexChanged(object? sender, EventArgs e)
        {
            MostrarPagos();
        }

        // resuelve nud monto pago value changed con los datos recibidos y entrega el resultado al codigo que lo llama
        private void nudMontoPago_ValueChanged(object? sender, EventArgs e)
        {
            if (!actualizandoMontoSugerido)
            {
                montoPagoEditado = true;
            }

            ActualizarResumenPago();
        }

        // manejador sin instrucciones no realiza ninguna accion adicional
        private void pnlFormaEntrega_Paint(object sender, PaintEventArgs e)
        {
        }

        // manejador sin instrucciones no realiza ninguna accion adicional
        private void pnlCliente_Paint(object sender, PaintEventArgs e)
        {
        }

        // ejecuta la accion de label3 cuando el usuario pulsa el control
        private void label3_Click(object sender, EventArgs e)
        {
        }

        // manejador sin instrucciones no realiza ninguna accion adicional
        private void pnlProductos_Paint(object sender, PaintEventArgs e)
        {
        }

        // ejecuta la accion de lbl cobro previsto cuando el usuario pulsa el control
        private void lblCobroPrevisto_Click(object sender, EventArgs e)
        {
        }

        // lee los campos actuales y construye la solicitud; este metodo no guarda cambios ni modifica la seleccion
        private venta ObtenerVentaDelFormulario(decimal totalVenta)
        {
            string modalidadEntregaSeleccionada;
            if (rdoEnvioDomicilio.Checked)
            {
                modalidadEntregaSeleccionada = "Envío a domicilio";
            }
            else
            {
                if (rdoRetiroPosterior.Checked)
                {
                    modalidadEntregaSeleccionada = "Retiro posterior en el local";
                }
                else
                {
                    if (rdoRetiroLocal.Checked)
                    {
                        modalidadEntregaSeleccionada = "Retiro en el local";
                    }
                    else
                    {
                        modalidadEntregaSeleccionada = string.Empty;
                    }
                }
            }

            decimal costoEnvioVenta;
            if (rdoEnvioDomicilio.Checked)
            {
                costoEnvioVenta = nudCostoEnvio.Value;
            }
            else
            {
                costoEnvioVenta = 0;
            }

            return new venta
            {
                IdCliente = clienteElegido?.IdCliente ?? 0,
                IdUsuario = SesionActual.Usuario?.idUsuario ?? 0,
                Descuento = nudDescuento.Value,
                Total = totalVenta,
                CondicionPago = cboCondicionPago.SelectedItem?.ToString() ?? string.Empty,
                FechaPago = dtpFechaPago.Value,
                ObservacionPago = txtObservacionPago.Text.Trim(),
                ModalidadEntrega = modalidadEntregaSeleccionada,
                DomicilioEntrega = txtDomicilioEntrega.Text.Trim(),
                FechaEntrega = dtpFechaEntrega.Value,
                HorarioSugerido = txtHorarioSugerido.Text.Trim(),
                ObservacionesEntrega = txtObservacionesEntrega.Text.Trim(),
                TipoLogistica = cboTipoLogistica.SelectedItem?.ToString() ?? string.Empty,
                CostoEnvio = costoEnvioVenta,
                Detalles = detallesVenta.ToList(),
                Pagos = pagosVenta.ToList(),
                SegundoTelefono = txtSegundoTelefono.Text.Trim()

            };
        }
    }
}
