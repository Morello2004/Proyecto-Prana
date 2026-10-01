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
    public partial class frmNuevaVenta : Form
    {
        // servicios y estado temporal de la venta
        private readonly CN_Cliente clientesVenta = new();
        private readonly CN_Producto productosVenta = new();
        private readonly CN_MedioPago mediosVenta = new();
        private readonly CN_RegistroVenta registroVentas = new();
        private readonly ListBox listaClientes = new();
        private readonly ListBox listaProductos = new();
        private readonly System.Windows.Forms.Timer esperaClientes = new() { Interval = 250 };
        private readonly System.Windows.Forms.Timer esperaProductos = new() { Interval = 250 };
        private Cliente? clienteElegido;
        private OpcionProducto? productoElegido;
        private readonly List<DetalleNuevaVenta> detallesVenta = new();
        private readonly List<PagoNuevaVenta> pagosVenta = new();
        private readonly Label lblCobroPrevisto = new()
        {
            AutoSize = true,
            ForeColor = Color.White,
            Font = new Font("Segoe UI", 10F, FontStyle.Bold),
            Location = new Point(20, 245)
        };
        private readonly CheckBox usarDireccionRegistrada = new()
        {
            AutoSize = true,
            ForeColor = Color.White,
            Location = new Point(520, 55),
            Text = "Usar dirección registrada"
        };
        private bool actualizandoTexto;
        private int ventaRegistrada;
        private readonly TextBox txtCuentaDestino = new() { MaxLength = 100, Location = new Point(280, 200), Width = 250 };

        // inicializa el diseñador y conecta los controles con la lógica de venta
        public frmNuevaVenta()
        {
            InitializeComponent();
            ConfigurarVenta();
            ConfigurarDistribucion();
            chkGenerarRecibo.Text = "Generar garantía";
            chkGenerarRecibo.Checked = chkGenerarRemito.Checked = true;
            chkGenerarRecibo.Enabled = chkGenerarRemito.Enabled = false;
            lblAclaracionDocumentos.Text = "Garantía y remito se generan con el mismo número de venta.";
            btnConfirmarPagoActual.Text = "Registrar venta";
            dtpFechaPago.Enabled = false;
            colMontoPago.Visible = colRecargoPago.Visible = false;
            pnlPago.Controls.Add(new Label { Text = "Cuenta destino / referencia", AutoSize = true, ForeColor = Color.White, Location = new Point(280, 182) });
            pnlPago.Controls.Add(txtCuentaDestino);
        }

        // conecta las búsquedas y los cambios de cliente producto entrega y pago
        private void ConfigurarVenta()
        {
            ConfigurarLista(listaClientes, pnlCliente, txtBuscarCliente);
            ConfigurarLista(listaProductos, pnlProductos, txtBuscarProducto);
            txtBuscarCliente.TextChanged += (_, _) => { if (actualizandoTexto) return; clienteElegido = null; esperaClientes.Stop(); esperaClientes.Start(); };
            txtBuscarProducto.TextChanged += (_, _) => { if (actualizandoTexto) return; productoElegido = null; esperaProductos.Stop(); esperaProductos.Start(); };
            esperaClientes.Tick += (_, _) => { esperaClientes.Stop(); BuscarClientes(); };
            esperaProductos.Tick += (_, _) => { esperaProductos.Stop(); BuscarProductos(); };
            listaClientes.Click += (_, _) => ElegirCliente();
            listaProductos.Click += (_, _) => ElegirProducto();
            txtBuscarCliente.KeyDown += (_, e) => Navegar(e, listaClientes, ElegirCliente);
            txtBuscarProducto.KeyDown += (_, e) => Navegar(e, listaProductos, ElegirProducto);
            BtnAgregarCliente.Click += (_, _) => ElegirCliente();
            btnAgregarProducto.Click += (_, _) => AgregarProducto();
            dgvDetalleVenta.CellContentClick += (_, e) => { if (e.RowIndex >= 0 && e.ColumnIndex == colEliminar.Index) { detallesVenta.RemoveAt(e.RowIndex); MostrarDetalle(); } };
            nudDescuento.ValueChanged += (_, _) => ActualizarTotales();
            btnAgregarPago.Click += (_, _) => AgregarPago();
            dgvPagos.CellContentClick += (_, e) => { if (e.RowIndex >= 0 && e.ColumnIndex == colQuitarPago.Index) { pagosVenta.RemoveAt(e.RowIndex); MostrarPagos(); } };
            btnConfirmarPagoActual.Click += (_, _) => RegistrarVenta();
            rdoRetiroLocal.CheckedChanged += (_, _) => ActualizarEntrega();
            rdoRetiroPosterior.CheckedChanged += (_, _) => ActualizarEntrega();
            rdoEnvioDomicilio.CheckedChanged += (_, _) => ActualizarEntrega();
            pnlFormaEntrega.Controls.Add(usarDireccionRegistrada);
            usarDireccionRegistrada.CheckedChanged += (_, _) => ActualizarDireccionEntrega();
            pnlPago.Controls.Add(lblCobroPrevisto);
            numericUpDown1.ValueChanged += (_, _) => ActualizarTotales();
            cboMedioPago.SelectedIndexChanged += (_, _) => { ActualizarCuotas(); MostrarPagos(); };
            cboCantCuotas.SelectedIndexChanged += (_, _) => MostrarPagos();
            nudMontoPago.ValueChanged += (_, _) => ActualizarResumenPago();
        }

        // coloca las sugerencias debajo del campo de búsqueda
        private static void ConfigurarLista(ListBox lista, Panel panel, TextBox buscador)
        {
            lista.Location = new Point(buscador.Left, buscador.Bottom + 2);
            lista.Size = new Size(buscador.Width, 120);
            lista.Font = buscador.Font;
            lista.Visible = false;
            panel.Controls.Add(lista);
            lista.BringToFront();
        }

        // permite elegir sugerencias con las flechas y enter
        private static void Navegar(KeyEventArgs e, ListBox lista, Action elegir)
        {
            if (!lista.Visible) return;
            if (e.KeyCode == Keys.Down && lista.Items.Count > 0) { lista.SelectedIndex = Math.Min(lista.SelectedIndex + 1, lista.Items.Count - 1); e.Handled = true; }
            if (e.KeyCode == Keys.Up && lista.Items.Count > 0) { lista.SelectedIndex = Math.Max(lista.SelectedIndex - 1, 0); e.Handled = true; }
            if (e.KeyCode == Keys.Enter) { elegir(); e.SuppressKeyPress = true; }
            if (e.KeyCode == Keys.Escape) { lista.Visible = false; e.SuppressKeyPress = true; }
        }

        // consulta clientes después de una pausa breve al escribir
        private void BuscarClientes()
        {
            listaClientes.Items.Clear();
            if (txtBuscarCliente.Text.Trim().Length < 2) { listaClientes.Visible = false; return; }
            try
            {
                foreach (Cliente cliente in clientesVenta.buscarCliente(txtBuscarCliente.Text).Take(15)) listaClientes.Items.Add(new OpcionCliente(cliente));
                listaClientes.Visible = listaClientes.Items.Count > 0;
                listaClientes.BringToFront();
            }
            catch (Exception ex) { ErrorVenta(ex); }
        }

        // conserva el cliente elegido y muestra solo ese cliente en el grid
        private void ElegirCliente()
        {
            if (listaClientes.SelectedItem is not OpcionCliente opcion) return;
            clienteElegido = opcion.Cliente;
            esperaClientes.Stop();
            actualizandoTexto = true;
            txtBuscarCliente.Text = opcion.ToString();
            actualizandoTexto = false;
            listaClientes.Visible = false;
            lblClienteSeleccionado.Text = opcion.ToString();
            dataGridView2.Rows.Clear();
            dataGridView2.Rows.Add(opcion.Cliente.DNI, opcion.Cliente.Nombre, opcion.Cliente.Apellido, opcion.Cliente.Telefono, opcion.Cliente.Direccion);
            usarDireccionRegistrada.Checked = false;
            txtDomicilioEntrega.Clear();
            ActualizarEntrega();
        }

        // consulta variantes y muestra el stock sin ocultar los productos agotados
        private void BuscarProductos()
        {
            listaProductos.Items.Clear();
            if (txtBuscarProducto.Text.Trim().Length < 2) { listaProductos.Visible = false; return; }
            try
            {
                foreach (producto item in productosVenta.buscarProducto(txtBuscarProducto.Text, null, null, "", ""))
                    foreach (VarianteProducto variante in item.Variantes)
                        listaProductos.Items.Add(new OpcionProducto(item, variante));
                if (listaProductos.Items.Count == 0) listaProductos.Items.Add("Sin productos para esta búsqueda");
                listaProductos.Visible = true;
                listaProductos.BringToFront();
            }
            catch (Exception ex) { ErrorVenta(ex); }
        }

        // conserva la variante elegida fuera del grid de detalle
        private void ElegirProducto()
        {
            if (listaProductos.SelectedItem is not OpcionProducto opcion) return;
            productoElegido = opcion;
            esperaProductos.Stop();
            actualizandoTexto = true;
            txtBuscarProducto.Text = opcion.ToString();
            actualizandoTexto = false;
            listaProductos.Visible = false;
        }

        // agrega la cantidad elegida sin superar el stock disponible
        private void AgregarProducto()
        {
            if (productoElegido == null) { MessageBox.Show("Seleccione un producto de la lista."); return; }
            int cantidad = (int)nudCantidad.Value;
            int existente = detallesVenta.Where(d => d.IdVariante == productoElegido.Variante.IdVariante).Sum(d => d.Cantidad);
            if (existente + cantidad > productoElegido.Variante.StockFisico - productoElegido.Variante.StockComprometido) { MessageBox.Show("La cantidad supera el stock disponible."); return; }
            detallesVenta.Add(new DetalleNuevaVenta { IdVariante = productoElegido.Variante.IdVariante, Producto = productoElegido.Producto.NombreProducto, Variante = productoElegido.Variante.Color + " " + productoElegido.Variante.CondicionProducto, Cantidad = cantidad, PrecioUnitario = productoElegido.Variante.Precio });
            MostrarDetalle();
            txtBuscarProducto.Clear();
            productoElegido = null;
        }

        // actualiza las filas de productos y vuelve a calcular los importes
        private void MostrarDetalle()
        {
            dgvDetalleVenta.Rows.Clear();
            foreach (DetalleNuevaVenta item in detallesVenta)
                dgvDetalleVenta.Rows.Add(item.Producto + " x" + item.Cantidad, item.Variante, item.PrecioUnitario.ToString("C"), (item.Cantidad * item.PrecioUnitario).ToString("C"), "Quitar");
            ActualizarTotales();
        }

        // el total base incluye productos descuento y envío antes de los recargos
        private decimal TotalBase => registroVentas.CalcularTotalBase(detallesVenta, nudDescuento.Value, rdoEnvioDomicilio.Checked ? numericUpDown1.Value : 0);
        // el total final suma los recargos de los medios ya agregados
        private decimal TotalConRecargos => TotalBase + pagosVenta.Sum(p => p.Recargo);

        // refleja subtotal descuento envío y total en los labels
        private void ActualizarTotales()
        {
            decimal subtotal = detallesVenta.Sum(d => d.Cantidad * d.PrecioUnitario);
            lblSubtotal.Text = subtotal.ToString("C");
            lblDescuentoAplicado.Text = "- " + nudDescuento.Value.ToString("C");
            lblEnvio.Text = (rdoEnvioDomicilio.Checked ? numericUpDown1.Value : 0m).ToString("C");
            MostrarPagos();
        }

        // muestra cuotas solo para medios que admiten financiación
        private void ActualizarCuotas()
        {
            bool mostrar = cboMedioPago.SelectedItem is MedioPago medio && (medio.Recargo3 > 0 || medio.Recargo6 > 0 || medio.Recargo9 > 0 || medio.Recargo12 > 0 || medio.NombreMedioPago.Contains("tarjeta", StringComparison.OrdinalIgnoreCase));
            label2.Visible = cboCantCuotas.Visible = mostrar;
            if (!mostrar && cboCantCuotas.Items.Count > 0) cboCantCuotas.SelectedIndex = 0;
        }

        // obtiene el recargo del medio y las cuotas seleccionadas
        private decimal PorcentajeRecargoSeleccionado()
        {
            if (cboMedioPago.SelectedItem is not MedioPago medio) return 0;
            int cuotas = cboCantCuotas.Visible && cboCantCuotas.SelectedItem != null ? Convert.ToInt32(cboCantCuotas.SelectedItem) : 1;
            return registroVentas.PorcentajeRecargo(medio, cuotas);
        }

        // toma el importe final recibido y separa base y recargo
        private void AgregarPago()
        {
            if (cboMedioPago.SelectedItem is not MedioPago medio || nudMontoPago.Value <= 0) { MessageBox.Show("Seleccione un medio e ingrese un monto."); return; }
            int cuotas = cboCantCuotas.Visible ? Convert.ToInt32(cboCantCuotas.SelectedItem) : 1;
            decimal porcentaje = PorcentajeRecargoSeleccionado();
            decimal cobrado = nudMontoPago.Value;
            // divide por el factor de recargo para que el importe ingresado sea lo cobrado
            (decimal montoBase, decimal recargo) = registroVentas.DesglosarCobro(cobrado, porcentaje);
            if (montoBase <= 0 || pagosVenta.Sum(p => p.Monto) + montoBase > TotalBase) { MessageBox.Show("El importe cobrado supera el saldo de la venta."); return; }
            pagosVenta.Add(new PagoNuevaVenta { CuentaDestino = txtCuentaDestino.Text.Trim(), IdMedioPago = medio.IdMedioPago, Medio = medio.NombreMedioPago, Monto = montoBase, Recargo = recargo, Cuotas = cuotas });
            txtCuentaDestino.Clear();
            MostrarPagos();
            nudMontoPago.Value = 0;
            if (dgvPagos.Rows.Count > 0)
            {
                dgvPagos.CurrentCell = dgvPagos.Rows[dgvPagos.Rows.Count - 1].Cells[colMedioPago.Index];
                dgvPagos.FirstDisplayedScrollingRowIndex = dgvPagos.Rows.Count - 1;
            }
        }

        // muestra base recargo total cobrado y saldo pendiente
        private void MostrarPagos()
        {
            dgvPagos.Rows.Clear();
            foreach (PagoNuevaVenta pago in pagosVenta)
                dgvPagos.Rows.Add(pago.Medio + (pago.Cuotas > 1 ? $" ({pago.Cuotas} cuotas)" : ""), pago.Monto.ToString("C"), pago.Recargo.ToString("C"), (pago.Monto + pago.Recargo).ToString("C"), "Quitar");
            ActualizarResumenPago();
        }

        // muestra lo cobrado y el importe previsto con el medio seleccionado
        private void ActualizarResumenPago()
        {
            decimal pagadoConRecargos = pagosVenta.Sum(p => p.Monto + p.Recargo);
            decimal saldoBase = Math.Max(0, TotalBase - pagosVenta.Sum(p => p.Monto));
            decimal recargoPrevisto = registroVentas.CalcularRecargo(saldoBase, PorcentajeRecargoSeleccionado());
            decimal totalPrevisto = TotalConRecargos + recargoPrevisto;
            bool estimado = recargoPrevisto > 0;
            lblTotalTexto.Text = estimado ? "TOTAL EST.:" : "TOTAL:";
            lblTotal.Text = totalPrevisto.ToString("C");
            lblTotalPagoActual.Text = $"Pago: {pagadoConRecargos:C}    {(estimado ? "Saldo estimado" : "Saldo")}: {Math.Max(0, totalPrevisto - pagadoConRecargos):C}";
            lblCobroPrevisto.Text = cboMedioPago.SelectedItem is MedioPago && saldoBase > 0
                ? $"A cobrar por el saldo con {cboMedioPago.Text}: {(saldoBase + recargoPrevisto):C}"
                : string.Empty;
        }

        // habilita los datos de domicilio y suma el envío cuando corresponde
        private void ActualizarEntrega()
        {
            bool envio = rdoEnvioDomicilio.Checked;
            usarDireccionRegistrada.Enabled = envio && !string.IsNullOrWhiteSpace(clienteElegido?.Direccion);
            ActualizarDireccionEntrega();
            cboTipoLogistica.Enabled = envio;
            numericUpDown1.Enabled = envio;
            dtpFechaEntrega.Enabled = envio || rdoRetiroPosterior.Checked;
            ActualizarTotales();
        }

        // copia la dirección del cliente o permite escribir una distinta
        private void ActualizarDireccionEntrega()
        {
            if (usarDireccionRegistrada.Checked)
                txtDomicilioEntrega.Text = clienteElegido?.Direccion ?? string.Empty;
            else if (clienteElegido != null && txtDomicilioEntrega.Text == clienteElegido.Direccion)
                txtDomicilioEntrega.Clear();
            txtDomicilioEntrega.Enabled = rdoEnvioDomicilio.Checked && !usarDireccionRegistrada.Checked;
        }

        // prepara los grids las cuotas los medios y la presentación inicial
        private void CargarVenta()
        {
            dtpFechaPago.Value = DateTime.Today;
            dtpFechaPago.MinDate = DateTime.Today;
            lblMontoPago.Text = "Importe cobrado";
            colMontoPago.HeaderText = "Base de venta";
            colRecargoPago.HeaderText = "Recargo";
            colTotalPago.HeaderText = "Total cobrado";
            colMontoPago.Visible = colRecargoPago.Visible = false;
            pnlPago.Controls.Remove(btnConfirmarPagoActual);
            pnlDocumentos.Controls.Add(btnConfirmarPagoActual);
            btnConfirmarPagoActual.Location = new Point(270, 53);
            btnConfirmarPagoActual.Text = "Registrar venta";
            chkGenerarRemito.Text = "Datos para remito";
            lblAclaracionDocumentos.Text = "Los datos de entrega se guardarán para confeccionar el remito.";
            dataGridView2.Columns.Clear();
            foreach (string nombre in new[] { "DNI", "Nombre", "Apellido", "Teléfono", "Dirección" }) dataGridView2.Columns.Add(nombre, nombre);
            dataGridView2.ReadOnly = true;
            dataGridView2.AllowUserToAddRows = false;
            dataGridView2.RowHeadersVisible = false;
            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView2.Height = 90;
            cboCantCuotas.Items.AddRange(new object[] { 1, 3, 6, 9, 12 });
            cboCantCuotas.SelectedIndex = 0;
            rdoRetiroLocal.Checked = true;
            cboCondicionPago.SelectedIndex = 2;
            try
            {
                List<MedioPago> mediosDisponibles = mediosVenta.ObtenerTodos();
                cboMedioPago.DisplayMember = nameof(MedioPago.NombreMedioPago);
                cboMedioPago.DataSource = mediosDisponibles;
                // selecciona efectivo al abrir la venta si está disponible
                int indiceEfectivo = mediosDisponibles.FindIndex(m => m.NombreMedioPago.Trim().Equals("Efectivo", StringComparison.OrdinalIgnoreCase));
                if (indiceEfectivo >= 0) cboMedioPago.SelectedIndex = indiceEfectivo;
            }
            catch (Exception ex) { ErrorVenta(ex); }
            ActualizarCuotas();
            ActualizarTotales();
        }

        // valida la venta completa y envía todos los datos al registro
        private void RegistrarVenta()
        {
            if (ventaRegistrada > 0) return;
            if (clienteElegido == null || detallesVenta.Count == 0) { MessageBox.Show("Seleccione un cliente y al menos un producto."); return; }
            if (nudDescuento.Value > detallesVenta.Sum(d => d.Cantidad * d.PrecioUnitario)) { MessageBox.Show("El descuento no puede superar el subtotal."); return; }
            if (!rdoRetiroLocal.Checked && !rdoRetiroPosterior.Checked && !rdoEnvioDomicilio.Checked) { MessageBox.Show("Seleccione una forma de entrega."); return; }
            if (rdoEnvioDomicilio.Checked && (string.IsNullOrWhiteSpace(txtDomicilioEntrega.Text) || cboTipoLogistica.SelectedIndex < 0)) { MessageBox.Show("Complete domicilio y logística."); return; }
            if (cboCondicionPago.SelectedIndex < 0) { MessageBox.Show("Seleccione la condición del pago."); return; }
            if (dtpFechaPago.Value.Date < DateTime.Today) { MessageBox.Show("La fecha de pago no puede ser anterior a hoy."); return; }
            decimal pagado = pagosVenta.Sum(p => p.Monto);
            if (pagado > TotalBase) { MessageBox.Show("Los pagos superan el total actualizado. Ajuste los medios de pago."); return; }
            if (cboCondicionPago.SelectedIndex == 2 && pagado != TotalBase) { MessageBox.Show("El pago total debe cubrir la venta."); return; }
            if (cboCondicionPago.SelectedIndex < 2 && (pagado <= 0 || pagado >= TotalBase)) { MessageBox.Show("Ingrese un pago inicial parcial."); return; }
            try
            {
                int id = registroVentas.Registrar(new NuevaVenta
                {
                    IdCliente = clienteElegido.IdCliente,
                    IdUsuario = SesionActual.Usuario?.idUsuario ?? throw new InvalidOperationException("No hay usuario autenticado."),
                    Descuento = nudDescuento.Value,
                    Total = TotalConRecargos,
                    CondicionPago = cboCondicionPago.Text,
                    FechaPago = dtpFechaPago.Value,
                    ObservacionPago = txtObservacionPago.Text.Trim(),
                    ModalidadEntrega = rdoEnvioDomicilio.Checked ? "Envío a domicilio" : rdoRetiroPosterior.Checked ? "Retiro posterior en el local" : "Retiro en el local",
                    DomicilioEntrega = txtDomicilioEntrega.Text.Trim(),
                    FechaEntrega = dtpFechaEntrega.Value,
                    HorarioSugerido = txtHorarioSugerido.Text.Trim(),
                    ObservacionesEntrega = txtObservacionesEntrega.Text.Trim(),
                    TipoLogistica = cboTipoLogistica.Text,
                    CostoEnvio = rdoEnvioDomicilio.Checked ? numericUpDown1.Value : 0,
                    GenerarRemito = true,
                    Detalles = detallesVenta.ToList(),
                    Pagos = pagosVenta.ToList()
                });
                lblNumeroVenta.Text = $"Venta N.° {id}";
                btnConfirmarPagoActual.Enabled = false;
                ventaRegistrada = id;
                try { DocumentosPrana.MostrarVenta(this, id); }
                catch (Exception ex)
                {
                    MessageBox.Show(this, $"La venta N.º {id} quedó registrada y sus comprobantes están guardados en la base.\nNo se pudieron abrir o exportar los archivos: {ex.Message}\nPuede reimprimirlos desde el panel principal, sin registrar otra venta.", "Venta registrada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex) { ErrorVenta(ex); }
        }

        // presenta los errores de consulta o registro al usuario
        private static void ErrorVenta(Exception ex) => MessageBox.Show(ex.Message, "Nueva venta", MessageBoxButtons.OK, MessageBoxIcon.Error);

        // representa una opción de cliente en la lista de sugerencias
        private sealed class OpcionCliente
        {
            public Cliente Cliente { get; }
            public OpcionCliente(Cliente cliente) => Cliente = cliente;
            public override string ToString() => $"{Cliente.DNI} - {Cliente.Nombre} {Cliente.Apellido}";
        }
        // representa una variante de producto en la lista de sugerencias
        private sealed class OpcionProducto
        {
            public producto Producto { get; }
            public VarianteProducto Variante { get; }
            public OpcionProducto(producto producto, VarianteProducto variante) { Producto = producto; Variante = variante; }
            public override string ToString() => $"{Producto.NombreProducto} {Producto.Modelo} - {Variante.Color} {Variante.CondicionProducto} - {Variante.Precio:C} (stock {Variante.StockFisico - Variante.StockComprometido})";
        }

        // carga los datos cuando se abre el formulario
        private void frmNuevaVenta_Load(object sender, EventArgs e) => CargarVenta();

        // mantiene los eventos vacíos que todavía referencia el diseñador
        private void panel1_Paint(object sender, PaintEventArgs e) { }
        private void dgvDetalleVenta_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void panel1_Paint_1(object sender, PaintEventArgs e) { }
        private void pnlContenidoVenta_Paint(object sender, PaintEventArgs e) { }
        private void panel1_Paint_2(object sender, PaintEventArgs e) { }
        private void radioButton1_CheckedChanged(object sender, EventArgs e) { }
        private void cboCondicionPago_SelectedIndexChanged(object sender, EventArgs e) { }
        private void cboTipoLogistica_SelectedIndexChanged(object sender, EventArgs e) { }

        private void btnConfirmarPagoActual_Click(object sender, EventArgs e)
        {

        }
    }
}
