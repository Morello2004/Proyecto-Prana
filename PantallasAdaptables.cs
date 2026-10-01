namespace Prana.Presentacion;

public partial class frmNuevaVenta
{
    private void ConfigurarDistribucion()
    {
        foreach (var grid in new[] { dataGridView2, dgvDetalleVenta, dgvPagos })
            EstiloGestion.Grilla(grid);
        colEliminar.AutoSizeMode = colQuitarPago.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
        colEliminar.Width = colQuitarPago.Width = 80;
        pnlCliente.Height = 205;
        pnlProductos.Height = 260;
        pnlPago.Height = 555;
        dgvPagos.Height = 180;
        lblTotalPagoActual.AutoSize = false;
        lblTotalPagoActual.SetBounds(20, 490, 680, 44);
        lblTotalPagoActual.ForeColor = Color.White;
        lblCobroPrevisto.AutoSize = false;
        lblCobroPrevisto.Height = 44;
        btnConfirmarPagoActual.Location = new Point(20, 445);
        // Distribuye domicilio, fecha y horario dentro del ancho útil.
        txtDomicilioEntrega.Width = 330;
        lblFechaEntrega.Left = dtpFechaEntrega.Left = 365;
        dtpFechaEntrega.Width = 185;
        lblHorarioSugerido.Left = txtHorarioSugerido.Left = 570;
        txtHorarioSugerido.Width = 140;
        lblHorarioSugerido.ForeColor = Color.White;
        foreach (var control in new Control[] { dataGridView2, dgvDetalleVenta, dgvPagos,
            txtObservacionesEntrega, txtObservacionPago, lblTotalPagoActual, lblCobroPrevisto })
            DistribucionAdaptable.Ancho(control);
        dgvDetalleVenta.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvPagos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        foreach (var valor in new[] { lblSubtotal, lblDescuentoAplicado, lblEnvio, lblTotal }) valor.Left = 530;
        foreach (var titulo in new[] { lblSubtotalTexto, lblDescuentoAplicadoTexto, lblEnvioTexto, lblTotalTexto }) titulo.Left = 350;
        DistribucionAdaptable.Pagina(this, pnlEncabezadoVenta, pnlCliente, pnlProductos,
            pnlFormaEntrega, pnlDescuentoTotales, pnlPago, pnlDocumentos);
        txtBuscarCliente.SizeChanged += (_, _) => listaClientes.Width = txtBuscarCliente.Width;
        txtBuscarProducto.SizeChanged += (_, _) => listaProductos.Width = txtBuscarProducto.Width;
    }
}

public partial class frmMediosPagos
{
    private void ConfigurarDistribucion()
    {
        var campos = DistribucionAdaptable.Fila(
            DistribucionAdaptable.Campo(lblNombre, txtNombreMedioPago),
            DistribucionAdaptable.Campo(lblGeneral, nudRecargoGeneral, 130),
            DistribucionAdaptable.Campo(lblRecargo3, nudRecargo3, 130),
            DistribucionAdaptable.Campo(lblRecargo6, nudRecargo6, 130),
            DistribucionAdaptable.Campo(lblRecargo9, nudRecargo9, 130),
            DistribucionAdaptable.Campo(lblRecargo12, nudRecargo12, 130),
            DistribucionAdaptable.Campo(new Label { Text = "Tipo en caja" }, cboTipoCaja, 160));
        var acciones = DistribucionAdaptable.Fila(btnGuardar, btnNuevo, btnDesactivar);
        lblSeccion.Height = 28;
        lblSeccion.AutoSize = false;
        pnlListado.Height = 300;
        DistribucionAdaptable.Pagina(this, pnlEncabezado, lblSeccion, campos, acciones, pnlListado);
    }
}

public partial class frmClientes
{
    private void ConfigurarDistribucion()
    {
        // El listado debajo del alta aprovecha todo el ancho del monitor.
        panel2.Height = 270;
        PnlBusquedaCliente.Height = 100;
        var listado = new Panel { Height = 290 };
        listado.Controls.Add(dgv_listadoClientes);
        listado.Controls.Add(label4);
        label4.Dock = DockStyle.Top;
        label4.AutoSize = false;
        label4.Height = 32;
        dgv_listadoClientes.Dock = DockStyle.Fill;
        dgv_listadoClientes.BringToFront();
        DistribucionAdaptable.Pagina(this, pnlEncabezadoClientes, PnlBusquedaCliente, panel2, listado);
    }
}

public partial class frmStock
{
    private void ConfigurarDistribucion()
    {
        var filtros = DistribucionAdaptable.Fila(
            DistribucionAdaptable.Campo(lblcodigo, txtBuscarProducto, 260),
            DistribucionAdaptable.Campo(label6, comboBoxCateogria),
            DistribucionAdaptable.Campo(label7, cboMarca),
            DistribucionAdaptable.Campo(label2, comboBoxCondicion), btnBuscarProducto, BtnLimpiarFiltros);
        var generales = DistribucionAdaptable.Fila(
            DistribucionAdaptable.Campo(label8, textBoxNombreProducto, 220),
            DistribucionAdaptable.Campo(label15, textBoxModelo, 220),
            DistribucionAdaptable.Campo(label5, textBoxCategoria, 170),
            DistribucionAdaptable.Campo(label4, textBoxMarca, 170),
            DistribucionAdaptable.Campo(label13, textBoxDescripcion, 650));
        var variantes = DistribucionAdaptable.Fila(
            DistribucionAdaptable.Campo(label3, comboBoxCondicion2),
            DistribucionAdaptable.Campo(label10, textBoxColor, 160),
            DistribucionAdaptable.Campo(label11, nudPrecio, 150), btnAgregarVariante);
        var acciones = DistribucionAdaptable.Fila(btnGuardarProducto, btnCancelar);
        var proveedor = DistribucionAdaptable.Fila(
            iconButton1,
            DistribucionAdaptable.Campo(label19, comboBoxSeleccionaProveedor, 320),
            DistribucionAdaptable.Campo(label18, dtpFechaIngresoStock, 230),
            DistribucionAdaptable.Campo(new Label { Text = "Remito del proveedor", ForeColor = Color.White }, textBoxRtoProveedor, 260));
        var totales = DistribucionAdaptable.Fila(label25, LblTotalIngresoDinero, label26,
            labelCantidadIngresada, ButtonRegistrarIngreso);
        foreach (var grid in new[] { dataGridViewBusquedaProducto, dgvVariantes, dataGridViewDetalleIngreso }) grid.Height = 175;
        foreach (var titulo in new[] { label28, label29, label31 }) { titulo.AutoSize = false; titulo.Height = 28; }
        DistribucionAdaptable.Pagina(this, pnlEncabezadostock, panel3, filtros, dataGridViewBusquedaProducto,
            panel4, label28, generales, label29, variantes, dgvVariantes, acciones,
            panel5, label31, proveedor, dataGridViewDetalleIngreso, totales);
    }
}

public partial class frmEntregas
{
    private void ConfigurarDistribucion()
    {
        var filtros = DistribucionAdaptable.Fila(txtBuscar, btnBuscar, chkPendientes);
        var preparar = DistribucionAdaptable.Fila(
            DistribucionAdaptable.Campo(lblCantidad, nudCantidad, 90),
            DistribucionAdaptable.Campo(lblDireccion, txtDireccion, 300),
            DistribucionAdaptable.Campo(lblHorario, txtHorario, 150), btnPreparar);
        contenido.Controls.Remove(pnlPendientes);
        contenido.Controls.Remove(pnlEntregas);
        pnlPendientes.Dock = pnlEntregas.Dock = DockStyle.None;
        pnlPendientes.Height = 220;
        pnlEntregas.Height = 260;
        lblSeleccion.Height = 32;
        lblAyuda.Height = 40;
        DistribucionAdaptable.Pagina(this, pnlTitulo, filtros, pnlPendientes, lblSeleccion, preparar, lblAyuda, pnlEntregas);
    }
}

public partial class frmPagos
{
    private void ConfigurarDistribucion()
    {
        var filtros = DistribucionAdaptable.Fila(txtBuscar, btnBuscar, chkPendientes);
        var cobro = DistribucionAdaptable.Fila(
            DistribucionAdaptable.Campo(lblImporte, nudImporte, 215),
            DistribucionAdaptable.Campo(lblMedio, cboMedio, 220),
            DistribucionAdaptable.Campo(lblCuotas, cboCuotas, 90),
            DistribucionAdaptable.Campo(new Label { Text="Cuenta destino / referencia" }, txtCuentaDestino, 220), btnRegistrar);
        contenido.Controls.Remove(pnlVentas);
        contenido.Controls.Remove(pnlPagos);
        pnlVentas.Dock = pnlPagos.Dock = DockStyle.None;
        pnlVentas.Height = 230;
        pnlPagos.Height = 220;
        lblSeleccion.Height = 32;
        lblCobro.Height = 42;
        DistribucionAdaptable.Pagina(this, pnlTitulo, filtros, pnlVentas, lblSeleccion, cobro, lblCobro, pnlPagos);
    }
}
