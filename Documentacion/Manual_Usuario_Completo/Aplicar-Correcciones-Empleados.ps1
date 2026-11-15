# adapta las explicaciones a las pantallas y permisos de la edicion actual
$chapters[0].body = '<p>Este manual explica cómo instalar Prana y utilizarlo en el trabajo diario de vendedores y administradores.</p><p>Buscá la tarea en el índice y seguí sus pasos. Cada imagen muestra la pantalla de esa tarea; los números señalan los campos o botones nombrados debajo. Los ejemplos de clientes, documentos e importes son ficticios y se utilizan solamente para capacitación.</p><p>Las capturas no muestran información del comercio. Cuando busques ventas, clientes o productos en tu puesto, sus datos aparecerán en los listados de la pantalla correspondiente.</p><aside>Antes de confirmar una venta, comprobá cliente, producto, cantidad, entrega e importe. Si aparece un error, conservá el mensaje y consultá si la operación se guardó antes de repetirla.</aside>'

$filasPermisos = @(
    @('Consultar ventas y emitir garantía o remito', 'Sí', 'Sí'),
    @('Crear, modificar, activar y restaurar clientes', 'Sí', 'Sí'),
    @('Dar de baja clientes', 'No', 'Sí'),
    @('Crear productos y variantes', 'Sí', 'Sí'),
    @('Modificar productos, precios, costos y stock físico', 'Sí', 'Sí'),
    @('Desactivar variantes e ingresar mercadería', 'Sí', 'Sí'),
    @('Registrar ventas y cobrar saldos', 'Sí', 'Sí'),
    @('Consultar, preparar y confirmar entregas', 'Sí', 'Sí'),
    @('Consultar alertas', 'Sí', 'Sí'),
    @('Registrar ingresos y gastos de caja', 'Sí', 'Sí'),
    @('Cerrar caja e imprimir el cierre', 'Sí', 'Sí'),
    @('Consultar el resumen mensual de caja', 'Sí', 'Sí'),
    @('Anular movimientos de caja', 'No', 'Sí'),
    @('Modificar recargos de medios de pago', 'No', 'Sí'),
    @('Consultar Informes e Historial', 'No', 'Sí'),
    @('Crear, modificar, desbloquear y dar de baja usuarios', 'No', 'Sí')
)
$tablaPermisos = '<table><thead><tr><th>Tarea</th><th>Vendedor</th><th>Administrador</th></tr></thead><tbody>'
foreach ($fila in $filasPermisos) { $tablaPermisos += '<tr><td>' + $fila[0] + '</td><td>' + $fila[1] + '</td><td>' + $fila[2] + '</td></tr>' }
$chapters[4].body = '<p>Cada empleado ingresa con su propia cuenta. Estas son las tareas disponibles para cada rol:</p>' + $tablaPermisos + '</tbody></table><p>Preparar entrega significa elegir los productos y cantidades que se van a entregar y completar sus datos. Confirmar entrega significa marcar que esa entrega preparada se realizó. Ambos pasos están disponibles para el vendedor y el administrador.</p><aside>El vendedor puede registrar gastos y cerrar la caja. Para anular un ingreso o gasto debe intervenir un administrador.</aside>'

$chapters[5].body = [regex]::Replace($chapters[5].body, '<h2>Cómo interpretar cada dato</h2>.*?<p>Seleccionar una venta', '<h2>Consultar una venta</h2><p>El número identifica la venta y se utiliza también en su garantía y remito. La fecha es la de la venta; un cobro posterior tiene su propia fecha en Pagos. El total incluye los recargos ya incorporados.</p><p>Seleccionar una venta', 'Singleline')
$chapters[5].body = $chapters[5].body.Replace('Una fila no representa una cuota.', 'Cada venta del listado corresponde a una compra completa, no a una cuota.')
$chapters[7].body = $chapters[7].body.Replace('ComboBox de búsqueda', 'campo Buscar').Replace('Ver deuda', 'Ver detalle').Replace('se habilitan las celdas editables', 'se habilitan los datos que podés corregir')
$chapters[7].body = $chapters[7].body.Replace('La consulta de deuda usa el identificador del cliente, lo que permite distinguir personas con el mismo nombre.', 'Si hay clientes con el mismo nombre, comprobá el documento para elegir a la persona correcta.')
$chapters[6].body = $chapters[6].body.Replace('Usá Cancelar para limpiar la carga que no vas a guardar.', 'Si no vas a crear ese cliente, no presiones Crear cliente y borrá los datos que escribiste.')
$chapters[8].body = $chapters[8].body.Replace('Clientes → Ver deuda', 'Clientes → Ver detalle')
$chapters[9].body = $chapters[9].body.Replace('Unidades reservadas al preparar entregas.', 'Unidades vendidas que todavía esperan preparación.').Replace('Stock físico menos comprometido.', 'Unidades que el sistema permite asignar al preparar una entrega; la disponibilidad se comprueba antes de confirmar la preparación.')
$chapters[12].body = $chapters[12].body.Replace('elegí la variante y presioná Seleccionar producto.', 'ubicá la variante y presioná Agregar en esa misma línea.')
$chapters[13].body = '<p>Esta ventana se abre desde Ingreso de stock y sirve para elegir qué mercadería recibiste.</p><ol><li>Escribí parte del nombre o modelo y presioná Buscar.</li><li>Buscá el producto correcto y compará modelo, color y condición.</li><li>Presioná Agregar en la línea de esa variante. Se incorpora al detalle del ingreso de Stock.</li><li>Podés agregar otra variante desde la misma ventana. Cuando termines, cerrá el selector con Cancelar.</li><li>En Stock completá la cantidad recibida y el costo unitario de cada variante antes de presionar Registrar ingreso.</li></ol><aside>Agregar una variante desde este selector no guarda el ingreso ni aumenta todavía las unidades. Cancelar cierra el selector; las variantes que ya agregaste permanecen en el detalle de Stock.</aside>'
$chapters[14].body = $chapters[14].body.Replace('Revisá documento, nombre, apellido, teléfono y dirección del cliente.', 'Revisá documento, nombre, apellido, teléfono, dirección, localidad y código postal del cliente.').Replace('producto, variante, precio unitario, subtotal, stock físico y stock comprometido.', 'producto, variante, cantidad, precio unitario, subtotal, stock físico y stock comprometido.')
$chapters[17].body = $chapters[17].body.Replace(' En la captura sin carga este campo conserva el rótulo de diseño Monto.', '')
$chapters[19].body += '<p>La garantía y el remito se generan siempre al registrar la venta. No hay una casilla que debas marcar para habilitarlos. Si el registro falla, se muestra el error y no se informa que la venta se registró correctamente.</p>'
$chapters[20].body = $chapters[20].body.Replace('Se abre un archivo HTML en el navegador asociado.', 'El documento se abre en el navegador de la computadora.').Replace('Los originales HTML se guardan en', 'Los documentos se guardan en').Replace('La garantía emitida ayer conserva la información de aquel momento.', 'Una copia impresa o un PDF guardado ayer conserva los importes de ese momento. Al volver a emitir desde Prana, se reemplazan los archivos de esa venta con los datos actuales.')
$chapters[22].body = $chapters[22].body.Replace('Solo el administrador puede gestionar entregas.', 'El vendedor y el administrador pueden preparar y confirmar entregas.')
$chapters[23].body = $chapters[23].body.Replace('Registra la entrega y aplica la salida de las unidades reservadas.', 'Registra que se entregaron las unidades ya apartadas al preparar; no descuenta dos veces la misma cantidad.')
$chapters[24].body = $chapters[24].body.Replace('Como administrador, abrí Panel principal', 'Abrí Panel principal')
$chapters[25].body = '<p>Esta acción está disponible solamente para el administrador y se utiliza para corregir un gasto o ingreso manual de la caja abierta de hoy.</p>' + $chapters[25].body + '<p>La anulación conserva el movimiento como antecedente, pero lo retira del listado vigente y de los totales de caja. En Historial, el administrador puede buscar la acción ANULAR_MOVIMIENTO y consultar el motivo en Detalle, junto con el usuario y la fecha. Los movimientos de días anteriores o cajas cerradas no se pueden anular desde esta pantalla.</p>'
$chapters[26].body += '<h2>Qué pasa al cambiar de día</h2><p>A medianoche se cierra automáticamente la caja del día que terminó. Los nuevos cobros corresponden al nuevo día. Si Prana estaba cerrado o la computadora apagada, al volver a abrirlo se completan los cierres pendientes.</p><p>Los saldos iniciales, la hora de inicio y las observaciones se guardan mientras los completás. Si salís de Caja y volvés, recuperás esos datos. Cada día tiene su propio saldo inicial: no se copia automáticamente el balance anterior.</p><p>Cuando estás mirando la caja del día y cambia la fecha, la pantalla pasa al nuevo día. Para consultar o imprimir el cierre anterior, elegí su fecha. Un cierre realizado automáticamente se identifica como CIERRE AUTOMÁTICO.</p><aside>Antes de cerrar manualmente, comprobá que todos hayan terminado de cobrar y registrar gastos. El aviso de confirmación aclara que no se podrán agregar más cobros ni movimientos a esa fecha. No confirmes hasta revisar los importes.</aside>'
$chapters[27].body = '<p>El vendedor y el administrador pueden consultar este resumen desde Caja.</p>' + $chapters[27].body
$chapters[32].body = $chapters[32].body.Replace('La cuenta administradora principal, no puede', 'La cuenta administradora principal no puede')
$chapters[35].body = $chapters[35].body.Replace('Si no aparece el catálogo', 'Si no aparecen los productos')
$chapters[36].body = $chapters[36].body.Replace('Revisá rol, selección de fila, estado de la operación y campos obligatorios.', 'En Pagos comprobá qué venta elegiste, el importe y el medio. En Entregas comprobá el producto o la entrega preparada seleccionados. En Usuarios elegí la cuenta desde el selector. Las funciones administrativas requieren una cuenta administradora.')
$chapters[37].body = $chapters[37].body.Replace('Revisar caja y saldos iniciales con el administrador.', 'Revisar caja y completar los saldos iniciales del día.').Replace('Guardar cierre, imprimir / archivar y cerrar la aplicación.', 'Revisar el cierre del día, imprimir o archivar si corresponde y cerrar Prana. Recordar el cierre automático a medianoche.')
$chapters[42].body = $chapters[42].body.Replace('Los HTML se guardan', 'Los documentos se guardan')

foreach ($capitulo in $chapters) {
    $capitulo.body = $capitulo.body.Replace('grillas vacías', 'listados vacíos').Replace('grilla superior', 'lista superior').Replace('en la grilla', 'en el listado').Replace('La grilla', 'El listado').Replace('la grilla', 'el listado').Replace('grillas', 'listados').Replace('grilla', 'listado').Replace('catálogo', 'lista de productos').Replace('Catálogo', 'Medios disponibles').Replace('controlados por negocio', '').Replace('La interfaz revisada', 'Prana').Replace('La versión revisada', 'Prana').Replace('la aplicación revisada', 'Prana').Replace('La navegación revisada no ofrece', 'El menú no tiene').Replace('El límite técnico', 'El máximo permitido')
    $capitulo.body = $capitulo.body.Replace('El catálogo puede mostrar', 'La lista puede mostrar').Replace('Aprender a separar lista de productos, recepción y venta.', 'Aprender a crear un producto, registrar las unidades recibidas y venderlo.')
}

# evita que una explicacion conserve una imagen de una seccion distinta
$chapters[34].img = $null
foreach ($indiceEjemplo in @(40,41,42)) {
    $chapters[$indiceEjemplo].crop = $null
    $chapters[$indiceEjemplo].marks = @()
}
