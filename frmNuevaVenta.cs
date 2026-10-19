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
        private Cliente? clienteElegido;
        private OpcionProducto? productoElegido;
        private readonly List<DetalleNuevaVenta> detallesVenta = new();
        private readonly List<PagoNuevaVenta> pagosVenta = new();
        private bool actualizandoTexto;
        private int ventaRegistrada;


        // inicializa el diseñador y conecta los controles con la lógica de venta
        public frmNuevaVenta()
        {
            InitializeComponent();

            ConfigurarVenta();
            ConfigurarDistribucion();
            chkGenerarRecibo.Text = "Generar garantía";
            chkGenerarRecibo.Checked = true;
            chkGenerarRecibo.Enabled = false;
            lblAclaracionDocumentos.Text = "La garantía y el remito se generan con el mismo número de venta.";
            btnRegistrarVenta.Text = "Registrar venta";
            dtpFechaPago.Enabled = false;
            colMontoPago.Visible = colRecargoPago.Visible = false;
            cboTipoLogistica.SelectedIndex = 0;

        }

        // conecta las búsquedas y los cambios de cliente producto entrega y pago
        private void ConfigurarVenta()
        {
            ConfigurarLista(listaClientes, pnlCliente, txtBuscarCliente);
            ConfigurarLista(listaProductos, pnlProductos, txtBuscarProducto);
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

        // Agrega el producto; puede quedar pendiente de ingreso de stock.
        private void AgregarProducto()
        {
            try
            {
                var detalle = registroVentas.PrepararProducto(productoElegido?.Producto,
                    productoElegido?.Variante, (int)nudCantidad.Value, detallesVenta);
                detallesVenta.Add(detalle);
                MostrarDetalle();
                txtBuscarProducto.Clear();
                productoElegido = null;
            }
            catch (Exception ex) { ErrorVenta(ex); }
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
            bool mostrar = cboMedioPago.SelectedItem is MedioPago medio && medio.TipoCaja == "Crédito";
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
            try
            {
                int cuotas = cboCantCuotas.Visible ? Convert.ToInt32(cboCantCuotas.SelectedItem) : 1;
                var pago = registroVentas.PrepararPago(cboMedioPago.SelectedItem as MedioPago,
                    cuotas, nudMontoPago.Value, TotalBase, pagosVenta, txtCuentaDestino.Text);
                pagosVenta.Add(pago);
                txtCuentaDestino.Clear();
                MostrarPagos();
                nudMontoPago.Value = 0;
                if (dgvPagos.Rows.Count > 0)
                {
                    dgvPagos.CurrentCell = dgvPagos.Rows[dgvPagos.Rows.Count - 1].Cells[colMedioPago.Index];
                    dgvPagos.FirstDisplayedScrollingRowIndex = dgvPagos.Rows.Count - 1;
                }
            }
            catch (Exception ex) { ErrorVenta(ex); }
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
            // Completa el importe con el saldo pendiente y el recargo del medio elegido
            decimal importeSugerido = saldoBase + recargoPrevisto;

            if (nudMontoPago.Value != importeSugerido)
            {
                nudMontoPago.Value = importeSugerido;
            }

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
            btnRegistrarVenta.Text = "Registrar venta";
            lblAclaracionDocumentos.Text = "La garantía y el remito se generan con el mismo número de venta.";
            dataGridView2.Columns.Clear();
            foreach (string nombre in new[] { "DNI", "Nombre", "Apellido", "Teléfono", "Dirección" }) dataGridView2.Columns.Add(nombre, nombre);
            dataGridView2.ReadOnly = true;
            dataGridView2.AllowUserToAddRows = false;
            dataGridView2.RowHeadersVisible = false;
            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView2.Height = 90;
            cboCantCuotas.Items.AddRange(new object[] { 1, 2, 3, 6, 9, 12 });
            cboCantCuotas.SelectedIndex = 0;
            rdoRetiroLocal.Checked = true;
            cboCondicionPago.SelectedItem = "Pago total";
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

        // recoge los datos; negocio valida y registra la venta
        private void RegistrarVenta()
        {
            if (ventaRegistrada > 0) return;
            try
            {
                int id = registroVentas.Registrar(new NuevaVenta
                {
                    IdCliente = clienteElegido?.IdCliente ?? 0,
                    IdUsuario = SesionActual.Usuario?.idUsuario ?? 0,
                    Descuento = nudDescuento.Value,
                    Total = TotalConRecargos,
                    CondicionPago = cboCondicionPago.SelectedItem?.ToString() ?? string.Empty,
                    FechaPago = dtpFechaPago.Value,
                    ObservacionPago = txtObservacionPago.Text.Trim(),
                    ModalidadEntrega = rdoEnvioDomicilio.Checked ? "Envío a domicilio" : rdoRetiroPosterior.Checked ? "Retiro posterior en el local" : rdoRetiroLocal.Checked ? "Retiro en el local" : string.Empty,
                    DomicilioEntrega = txtDomicilioEntrega.Text.Trim(),
                    FechaEntrega = dtpFechaEntrega.Value,
                    HorarioSugerido = txtHorarioSugerido.Text.Trim(),
                    ObservacionesEntrega = txtObservacionesEntrega.Text.Trim(),
                    TipoLogistica = cboTipoLogistica.SelectedItem?.ToString() ?? string.Empty,
                    CostoEnvio = rdoEnvioDomicilio.Checked ? numericUpDown1.Value : 0,
                    Detalles = detallesVenta.ToList(),
                    Pagos = pagosVenta.ToList(),
                    SegundoTelefono = textBox1.Text.Trim()
                });
                lblNumeroVenta.Text = $"Venta N.° {id}";
                btnRegistrarVenta.Enabled = false;
                ventaRegistrada = id;
                try { DocumentosPrana.MostrarVenta(this, id, textBox1.Text.Trim()); }
                catch (Exception ex)
                {
                    MessageBox.Show(this, $"La venta N.º {id} quedó registrada.\nNo se pudieron abrir o exportar los archivos: {ex.Message}\nPuede reimprimirlos desde el panel principal, sin registrar otra venta.", "Venta registrada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        // eventos del diseñador: el doble clic abre la acción de cada control
        private void panel1_Paint(object sender, PaintEventArgs e) { }
        private void dgvDetalleVenta_CellContentClick(object? sender, DataGridViewCellEventArgs e) { if (e.RowIndex >= 0 && e.ColumnIndex == colEliminar.Index) { detallesVenta.RemoveAt(e.RowIndex); MostrarDetalle(); } }
        private void panel1_Paint_1(object sender, PaintEventArgs e) { }
        private void pnlContenidoVenta_Paint(object sender, PaintEventArgs e) { }
        private void panel1_Paint_2(object sender, PaintEventArgs e) { }
        private void radioButton1_CheckedChanged(object sender, EventArgs e) { }
        private void cboCondicionPago_SelectedIndexChanged(object sender, EventArgs e) { }
        private void cboTipoLogistica_SelectedIndexChanged(object sender, EventArgs e) { }

        private void btnRegistrarVenta_Click(object? sender, EventArgs e)
        {
            RegistrarVenta();
        }
        private void txtBuscarCliente_TextChanged(object? sender, EventArgs e) {
            // Ignora los cambios de texto realizados al seleccionar un cliente.
            if (actualizandoTexto) return;

            // Reinicia la espera antes de buscar nuevas coincidencias.
            esperaClientes.Stop();

            // Oculta las sugerencias anteriores.
            listaClientes.Visible = false;

            // Busca solamente cuando se escriben al menos dos caracteres.
            if (txtBuscarCliente.Text.Trim().Length >= 2) 
                esperaClientes.Start();
        }

        private void txtBuscarProducto_TextChanged(object? sender, EventArgs e) { if (actualizandoTexto) return; productoElegido = null; esperaProductos.Stop(); esperaProductos.Start(); }

        private void esperaClientes_Tick(object? sender, EventArgs e) { esperaClientes.Stop(); BuscarClientes(); }

        private void esperaProductos_Tick(object? sender, EventArgs e) { esperaProductos.Stop(); BuscarProductos(); }

        private void listaClientes_Click(object? sender, EventArgs e) { ElegirCliente(); }

        private void listaProductos_Click(object? sender, EventArgs e) { ElegirProducto(); }

        private void txtBuscarCliente_KeyDown(object? sender, KeyEventArgs e) { Navegar(e, listaClientes, ElegirCliente); }

        private void txtBuscarProducto_KeyDown(object? sender, KeyEventArgs e) { Navegar(e, listaProductos, ElegirProducto); }

        private void BtnAgregarCliente_Click(object? sender, EventArgs e) { ElegirCliente(); }

        private void btnAgregarProducto_Click(object? sender, EventArgs e) { AgregarProducto(); }

        private void nudDescuento_ValueChanged(object? sender, EventArgs e) { ActualizarTotales(); }

        private void btnAgregarPago_Click(object? sender, EventArgs e) { AgregarPago(); }

        private void dgvPagos_CellContentClick(object? sender, DataGridViewCellEventArgs e) { if (e.RowIndex >= 0 && e.ColumnIndex == colQuitarPago.Index) { pagosVenta.RemoveAt(e.RowIndex); MostrarPagos(); } }

        private void rdoRetiroLocal_CheckedChanged(object? sender, EventArgs e) { ActualizarEntrega(); }

        private void rdoRetiroPosterior_CheckedChanged(object? sender, EventArgs e) { ActualizarEntrega(); }

        private void rdoEnvioDomicilio_CheckedChanged(object? sender, EventArgs e) { ActualizarEntrega(); }

        private void usarDireccionRegistrada_CheckedChanged(object? sender, EventArgs e) { ActualizarDireccionEntrega(); }

        private void numericUpDown1_ValueChanged(object? sender, EventArgs e) { ActualizarTotales(); }

        private void cboMedioPago_SelectedIndexChanged(object? sender, EventArgs e) { ActualizarCuotas(); MostrarPagos(); }

        private void cboCantCuotas_SelectedIndexChanged(object? sender, EventArgs e) { MostrarPagos(); }

        private void nudMontoPago_ValueChanged(object? sender, EventArgs e) { ActualizarResumenPago(); }

        private void pnlFormaEntrega_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pnlCliente_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
