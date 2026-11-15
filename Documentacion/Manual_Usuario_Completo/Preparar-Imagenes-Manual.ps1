# ubica las referencias sobre los controles de las capturas actuales
$capturasActuales = Get-Content (Join-Path $root 'imagenes/catalogo.json') -Raw -Encoding UTF8 | ConvertFrom-Json

# prepara i ma ge n m an ua l con los datos recibidos y devuelve el resultado al generador
function Imagen-Manual($Indice, $Nombre, $Recorte, $Referencias) {
    $captura = $capturasActuales | Where-Object name -eq $Nombre | Select-Object -First 1
    if (!$captura) { throw "No existe la captura $Nombre" }
    if (!$Recorte) { $Recorte = @(0, 0, $captura.width, $captura.height) }
    if ($Recorte[0]+$Recorte[2] -gt $captura.width -or $Recorte[1]+$Recorte[3] -gt $captura.height) { throw "Recorte fuera de la captura $Nombre" }
    $marcas = New-Object 'System.Collections.Generic.List[object]'
    foreach ($referencia in $Referencias) {
        $partes = $referencia.Split('|', 2)
        if ($partes[0].StartsWith('=')) {
            $control = $captura.controls | Where-Object text -eq $partes[0].Substring(1) | Select-Object -First 1
        } else {
            $control = $captura.controls | Where-Object name -eq $partes[0] | Select-Object -First 1
        }
        if (!$control) { throw "No existe el control $($partes[0]) en $Nombre" }
        $x = [int]($control.x + [Math]::Min($control.w/2, 100))
        $y = [int]($control.y + [Math]::Min($control.h/2, 25))
        if ($x -lt $Recorte[0] -or $y -lt $Recorte[1] -or $x -ge $Recorte[0]+$Recorte[2] -or $y -ge $Recorte[1]+$Recorte[3]) { throw "Referencia fuera del recorte: $referencia" }
        $marcas.Add(@($x,$y,$partes[1]))
    }
    $chapters[$Indice].img = $Nombre
    $chapters[$Indice].crop = $Recorte
    $chapters[$Indice].marks = $marcas.ToArray()
}

Imagen-Manual 2 'login' $null @('txtusuario|Nombre de usuario','txtclave|Contraseña','iconButton1|Ingresar')
Imagen-Manual 3 'Interfaz' @(0,0,168,515) @('btnPanelPrincipal|Panel principal','btnNuevaVenta|Nueva venta','btnClientes|Clientes','btnEntregas|Entregas')
Imagen-Manual 5 'frmPanelPrincipal' $null @('pnlHoy|Ventas de hoy','pnlSaldo|Saldo pendiente de cobro','pnlPendientes|Ventas por cobrar','btnBuscar|Buscar ventas','btnUltimas|Últimas 10 / Actualizar')
Imagen-Manual 6 'frmClientes_completa' @(0,145,650,275) @('txtNombreCliente|Nombre','txtDocumentoCliente|Número de documento','txtCodigoPostal|Código postal','btn_CrearCliente|Crear cliente')
Imagen-Manual 7 'frmClientes_completa' @(0,65,1443,655) @('cboBuscarCliente|Buscar un cliente','cboFiltroEstadoCliente|Activos, inactivos o eliminados','dgv_listadoClientes|Clientes encontrados y sus acciones')
Imagen-Manual 8 'frmDeudaCliente' $null @('=Cerrar|Cerrar la consulta de deuda')
Imagen-Manual 9 'frmStock_completa' @(0,60,1499,325) @('txtBuscarProducto|Buscar por nombre o modelo','btnBuscarProducto|Buscar','dgvBusquedaProductos|Variantes, precios y existencias')
Imagen-Manual 10 'frmStock_completa' @(0,390,1100,620) @('txtNombreProducto|Nombre del producto','txtModeloProducto|Modelo','txtColorVariante|Color de la variante','btnAgregarVariante|Agregar variante','btnGuardarProducto|Guardar Producto')
Imagen-Manual 12 'frmStock_completa' @(0,1000,1499,454) @('iconButton1|Buscar producto','comboBoxSeleccionaProveedor|Proveedor','txtDocumentoProveedor|Documento del proveedor','ButtonRegistrarIngreso|Registrar ingreso')
Imagen-Manual 13 'buttonSeleccionarProducto' $null @('buttonBuscarProducto|Buscar coincidencias','dgvProductos|Usá Agregar en la línea de la variante','buttonCancelar|Cerrar el selector')
Imagen-Manual 14 'frmNuevaVenta_completa' @(0,75,1330,600) @('cboCliente|Elegir cliente','cboBuscarProducto|Elegir producto y variante','nudCantidad|Cantidad','btnAgregarProducto|Agregar al detalle')
Imagen-Manual 15 'frmNuevaVenta_completa' @(0,678,1330,269) @('txtDomicilioEntrega|Domicilio de esta entrega','dtpFechaEntrega|Fecha estimada','txtHorarioSugerido|Horario sugerido','nudCostoEnvio|Costo del envío')
Imagen-Manual 16 'frmNuevaVenta_completa' @(0,952,680,165) @('nudDescuento|Descuento en pesos','lblTotal|Total de la venta')
Imagen-Manual 17 'frmNuevaVenta_completa' @(0,1122,1330,491) @('cboCondicionPago|Pago total o parcial','cboMedioPago|Medio de pago','nudMontoPago|Importe cobrado','btnAgregarPago|Agregar medio al pago')
Imagen-Manual 19 'frmNuevaVenta_completa' @(0,1618,800,124) @('btnRegistrarVenta|Registrar venta una sola vez')
Imagen-Manual 20 'frmComprobantes' $null @('btnGarantia|Ver / imprimir garantía','btnRemito|Ver / imprimir remito')
Imagen-Manual 21 'frmPagos_completa' $null @('txtBuscar|Localizar la venta','dgvVentas|Ventas encontradas','nudImporte|Importe a cancelar sin recargo','btnRegistrar|Registrar pago','btnGarantia|Emitir garantía actualizada')
Imagen-Manual 22 'frmEntregas' @(0,90,1200,445) @('=Preparar entrega|Preparar entrega después de elegir producto y cantidad')
Imagen-Manual 23 'frmEntregas' @(0,555,1200,280) @('=Confirmar entrega seleccionada|Confirmar solamente una entrega realizada')
Imagen-Manual 24 'frmCaja_completa' @(0,0,984,690) @('fecha|Fecha de consulta','cboCaja|Caja / turno','registrar|Registrar movimiento','anular|Anular movimiento: solo administrador')
Imagen-Manual 25 'frmMotivoAnulacion' $null @('txtMotivo|Motivo concreto de la anulación','btnAceptar|Anular','btnCancelar|Cancelar')
Imagen-Manual 26 'frmCaja_completa' @(0,690,984,404) @('saldo|Saldo inicial ARS','saldoUsd|Saldo inicial USD','abrir|Abrir caja / Nueva caja','cerrar|Cerrar caja seleccionada','imprimir|Ver / imprimir cierre')
Imagen-Manual 27 'frmResumenMensualCaja_completa' $null @('=Consultar / actualizar|Consultar el mes elegido')
Imagen-Manual 28 'frmAlertas_completa' $null @()
Imagen-Manual 29 'frmInformesVentas_completa' $null @('desde|Fecha Desde','hasta|Fecha Hasta','producto|Producto','ubicacion|Localidad','buscar|Consultar con los filtros elegidos')
Imagen-Manual 30 'frmHistorial_completa' $null @('txtRemito|Número de venta o remito','txtAccion|Acción a consultar','txtUsuario|Usuario que realizó la acción','btnFiltrar|Filtrar','btnUltimas|Últimas 10')
Imagen-Manual 31 'frmUsuarios_completa' $null @('selector|Elegir una cuenta o crear una nueva','txtNombreUsuario|Nombre de acceso','txtContrasena|Contraseña','cmbRol|Rol','btnGuardar|Guardar la cuenta')
Imagen-Manual 32 'frmUsuarios_completa' $null @('selector|Elegir el usuario correcto','btnDesbloquear|Desbloquear usuario','btnDarDeBaja|Dar de baja usuario')
Imagen-Manual 33 'frmMediosPagos_completa' $null @('dgvMediosPagos|Elegir el medio de pago','nudRecargoGeneral|Recargo general en porcentaje','nudRecargo3|Porcentaje para tres cuotas de Crédito','btnGuardar|Guardar recargos')
