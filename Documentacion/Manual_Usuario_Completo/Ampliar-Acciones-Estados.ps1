# explica los valores que el empleado puede encontrar en cada pantalla
$acciones = @(
    @('REGISTRAR_VENTA', 'Se registró una venta', 'Número de venta, cliente, total y descuento'),
    @('APLICAR_DESCUENTO', 'Se registró una venta con descuento', 'Número de venta, descuento y total final; puede aparecer junto con REGISTRAR_VENTA'),
    @('REGISTRAR_PAGO', 'Se agregó un cobro posterior desde Pagos', 'Venta, importe base y recargo; los pagos iniciales forman parte del registro de la venta'),
    @('PREPARAR_ENTREGA', 'Se apartaron unidades para una entrega', 'Venta, preparación, variante y cantidad; preparar no significa que el cliente ya recibió los productos'),
    @('CONFIRMAR_ENTREGA', 'Se confirmó una entrega o un retiro inmediato', 'Venta y entrega; en un retiro inmediato el detalle lo aclara'),
    @('CREAR_PRODUCTO', 'Se creó un producto con sus variantes', 'Nombre del producto creado'),
    @('MODIFICAR_PRODUCTO', 'Se guardaron cambios desde la modificación de productos', 'Variante afectada, nombre, precio nuevo y costo nuevo'),
    @('DESACTIVAR_VARIANTE', 'Se desactivó una variante', 'Identificador de la variante; no significa que se borraron sus ventas anteriores'),
    @('REGISTRAR_INGRESO_STOCK', 'Se registró mercadería recibida', 'Proveedor, documento o remito del proveedor y cantidad de variantes'),
    @('BAJA_CLIENTE', 'Se dio de baja a un cliente', 'Cliente afectado; sus antecedentes se conservan'),
    @('ACTIVAR_CLIENTE', 'Se activó un cliente inactivo', 'Cliente que vuelve a estar activo'),
    @('RESTAURAR_CLIENTE', 'Se restauró un cliente dado de baja', 'Cliente recuperado; es una acción distinta de activar un cliente inactivo'),
    @('DAR_DE_BAJA_USUARIO', 'Se desactivó una cuenta de acceso', 'Cuenta afectada y administrador que realizó la acción'),
    @('DESBLOQUEAR_USUARIO', 'Un administrador restableció el acceso de una cuenta bloqueada', 'Cuenta desbloqueada, responsable y fecha'),
    @('ANULAR_MOVIMIENTO', 'Un administrador anuló un ingreso o gasto manual de caja', 'Identificador del movimiento y motivo escrito en Detalle; deja de sumar en los totales vigentes')
)
# convierte filas y encabezados en una tabla del documento conservando sus valores
function Tabla-Consulta($Filas, $Cabecera) {
    $tabla = '<table><thead><tr>'
    foreach ($titulo in $Cabecera) { $tabla += '<th>' + [Net.WebUtility]::HtmlEncode($titulo) + '</th>' }
    $tabla += '</tr></thead><tbody>'
    foreach ($fila in $Filas) {
        $tabla += '<tr>'
        foreach ($valor in $fila) { $tabla += '<td>' + [Net.WebUtility]::HtmlEncode($valor) + '</td>' }
        $tabla += '</tr>'
    }
    return $tabla + '</tbody></table>'
}
$chapters[30].body += '<h2>Acciones que podés encontrar</h2><p>La columna Acción describe qué se hizo. No indica un estado pendiente de completar. Estos son los nombres que puede mostrar el historial de esta versión:</p>' + (Tabla-Consulta $acciones @('Acción', 'Qué significa', 'Qué revisar')) + '<h2>Cómo buscar una acción</h2><p>Copiá el nombre de la tabla en el filtro Acción y elegí el período. También podés escribir una parte, por ejemplo ENTREGA para encontrar PREPARAR_ENTREGA y CONFIRMAR_ENTREGA. Revisá Usuario, Entidad, Detalle y Fecha para identificar qué ocurrió.</p><p>El filtro Remito corresponde al número de venta de las actividades de venta. Para investigar cambios de clientes, productos, usuarios o movimientos de caja, dejalo vacío y usá Acción, Usuario y fechas. No todas las actividades incluyen un identificador de registro.</p><aside>El historial no contiene una acción para cada clic ni para cada tarea disponible. Por ejemplo, no registra aquí cada inicio de sesión, creación de cliente, creación de usuario, cambio de recargo o cierre de caja. La ausencia de una acción no demuestra por sí sola que una operación no se realizó. Consultá también la pantalla de la operación y sus documentos.</aside>'
$pagos = @(
    @('Pendiente', 'Venta registrada sin pago inicial'),
    @('Parcial', 'Se registró un pago parcial; consultá Pagos para ver cuánto falta cobrar'),
    @('Pagada', 'Se completó el pago de la venta')
)
$chapters[5].body += '<h2>Estados de pago de la venta</h2>' + (Tabla-Consulta $pagos @('Estado', 'Significado')) + '<p>El estado de pago y el de entrega se consultan por separado. Una venta pagada puede tener productos pendientes de entrega. El panel principal muestra un listado resumido: estos datos también se utilizan en las consultas y búsquedas de ventas.</p>'
$entregas = @(
    @('Pendiente', 'Todavía no hay unidades preparadas ni entregadas'),
    @('Preparación parcial', 'Se preparó una parte de las unidades de la venta'),
    @('Preparada', 'Se prepararon todas las unidades, pero todavía no se confirmó ninguna entrega'),
    @('Entrega parcial', 'Se confirmó la entrega de una parte de las unidades; revisá qué falta'),
    @('Entregada', 'Se confirmó la entrega de todas las unidades'),
    @('Cancelada', 'La consulta reconoce una venta anulada o cancelada; esta indicación no significa que exista un botón para cancelarla en Entregas')
)
$chapters[22].body += '<h2>Cómo interpretar el avance de la entrega</h2>' + (Tabla-Consulta $entregas @('Estado de entrega de la venta', 'Significado')) + '<p>Si una venta tiene unidades ya entregadas y otras preparadas, puede seguir figurando Entrega parcial. Revisá las cantidades de cada producto antes de preparar otra entrega.</p>'
$chapters[7].body += '<h2>Estados del cliente</h2><p><b>Activo:</b> cliente disponible para operar. <b>Inactivo:</b> cliente sin actividad reciente según el control del sistema; se puede activar. <b>Dado de baja o eliminado:</b> cliente retirado de uso, con sus antecedentes conservados; corresponde restaurarlo. No confundas inactividad con baja.</p><p><b>Sin deuda</b> indica que no tiene saldo pendiente; una deuda indica dinero por cobrar. Los pendientes de entrega se revisan aparte: no tener deuda no significa que ya recibió toda la mercadería.</p>'
$chapters[26].body += '<h2>Caja abierta, cerrada y movimientos anulados</h2><p><b>Abierta:</b> permite registrar las operaciones admitidas para hoy. <b>Cerrada:</b> conserva el cierre y bloquea nuevos cobros y movimientos de esa fecha. <b>CIERRE AUTOMÁTICO:</b> identifica al responsable indicado en un cierre realizado al cambiar de día; no es una acción del Historial.</p><p>Un movimiento anulado conserva su antecedente, pero no integra los totales vigentes. Su motivo se consulta en Historial buscando ANULAR_MOVIMIENTO.</p>'
$chapters[32].body += '<h2>Cuenta activa, bloqueada o dada de baja</h2><p>Una cuenta activa puede iniciar sesión con credenciales válidas. Una cuenta bloqueada no puede acceder mientras dure el bloqueo; revisá la fecha y hora que se informa o solicitá el desbloqueo a un administrador. Una cuenta dada de baja pierde el acceso y conserva sus antecedentes. Desbloquear una cuenta no equivale a crear un usuario nuevo.</p>'
$chapters[9].body += '<h2>Variante activa o desactivada</h2><p>Una variante activa está habilitada para su uso. Una variante desactivada conserva sus antecedentes, pero deja de estar disponible para nuevas operaciones que exigen variantes activas. El estado activo no garantiza que haya unidades suficientes: revisá también las existencias.</p>'
$chapters[33].body = $chapters[33].body.Replace('Tipo en caja', 'tipo del medio')
