# Cajas por apertura y varios cierres diarios

## Gestión de caja: frmCaja

La fecha sirve para buscar jornadas y el identificador **IdCaja** distingue cada apertura. Se permiten varios cierres de una misma fecha, pero una sola caja abierta a la vez, compartida entre los operadores. El formulario utiliza **CN_Caja** para validar y **CD_Caja** para consultar o guardar.

### Inicialización y selección de una caja

El constructor crea los controles y deja la hora de inicio como dato de consulta. **Cargar** completa cierres pendientes de días anteriores, llama a **negocio.Listar** y carga las aperturas de la fecha en **cboCaja**. Si se estaba consultando una, conserva su identificador; de lo contrario selecciona la última. Después llama a **Consultar** con fecha e identificador. Consultar no abre una caja automáticamente.

**MostrarCaja** presenta los datos y habilita las acciones según el estado. Una caja abierta de hoy permite registrar, corregir saldos y observaciones y cerrar. Una cerrada permite imprimir. Si no hay apertura seleccionada, se pueden preparar los datos iniciales antes de abrir. **cargando** impide que completar los controles se interprete como una edición del usuario.

### Abrir caja y preparar el turno siguiente

Si no hay caja, el usuario completa los saldos iniciales y presiona **Abrir caja**. Si está viendo un cierre y no existe otra apertura vigente, **Nueva caja** primero limpia los campos y deja la selección vacía para preparar otro turno; después se confirma con **Abrir caja**. No se copia automáticamente el balance anterior.

**CN_Caja.Abrir** exige sesión y valida saldos no negativos y observaciones de hasta mil caracteres. **CD_Caja.Abrir** obtiene un bloqueo compartido con todos los escritores, comprueba que no exista otra caja abierta y toma la fecha y hora de SQL Server. Inserta Caja con estado Abierta y responsable de apertura y conserva su identificador en IdCaja. La hora de inicio no se modifica desde la pantalla.

### Datos iniciales y JSON

Una vez abierta, los cambios de saldos y observaciones llaman a **GuardarDatosAbiertos**, negocio y datos. El UPDATE utiliza **id**, no solamente fecha. Si otra persona cerró ese turno y abrió otro, se rechaza la edición desactualizada. Se actualiza el JSON de la misma fila; no se crean archivos JSON ni una fila por cada cambio. Los movimientos mantienen sus registros originales.

**CierreCaja** contiene IdCaja, Cerrada, Numero, Fecha, Inicio, Fin, saldos, observaciones, usuario y listas de movimientos y billetes. Numero permanece en cero mientras está abierta y corresponde al identificador de la caja cuando está cerrada. No existe un contador independiente para cierres. Al consultar, las columnas SQL de identificación y horarios prevalecen sobre el JSON histórico.

### Cobros, ingresos y gastos de cada apertura

**Pago.id_caja** y **Caja_Movimiento.id_caja** relacionan cada operación con su apertura. Al registrar una venta o un pago posterior, **ExigirCajaAbierta** devuelve la caja vigente dentro de la misma transacción que guarda el cobro. Si no hay apertura, la operación se rechaza y se solicita abrirla desde Caja y gastos. Una misma venta puede tener cobros en cajas distintas.

Al registrar un movimiento manual, el formulario también envía IdCaja. La capa de datos comprueba que siga siendo la activa. **LeerMovimientos** reúne cobros y movimientos manuales no anulados y filtra por id_caja. Así una consulta no incluye otros cierres del mismo día. Los pagos aportan su base sin recargo y los pesos y dólares se calculan por separado.

### Anulación y ventana de motivo

La anulación requiere administrador tanto en el formulario como en negocio. Se abre **frmMotivoAnulacion** mediante ShowDialog para solicitar el motivo. Datos comprueba que el movimiento pertenezca a la caja que sigue abierta, marca anulado, conserva el motivo y registra ANULAR_MOVIMIENTO. Una nueva apertura no permite anular operaciones del turno ya cerrado. Los cobros de ventas no se anulan desde esta pantalla.

### Cierre manual y control de concurrencia

**Cerrar** muestra una confirmación en mayúsculas que aclara que no se agregarán operaciones a esa caja y que se puede abrir otra para continuar. Datos bloquea la apertura, vuelve a consultar sus movimientos por identificador y los compara con lo mostrado. Si cambiaron, solicita actualizar. Si coinciden, completa el responsable y la hora final y actualiza la misma fila a Cerrada con su JSON definitivo.

El bloqueo de aplicación **Prana.Caja.Activa** se utiliza al abrir, cerrar, cobrar y registrar o anular movimientos. Un índice único filtrado impide además dos filas con estado Abierta. Después de un cierre no se pueden registrar nuevas operaciones hasta abrir otra caja. No se elimina la fila cerrada ni se bloquea toda la fecha.

### Cierre automático y recuperación

El temporizador de **Program** sigue comprobando el cambio de día cada segundo y también recupera pendientes antes del login. **CerrarDiasAnteriores** busca aperturas de fechas anteriores que continúan abiertas. Cierra cada una por identificador con CIERRE AUTOMÁTICO y medianoche como hora final. No vuelve a cerrar los turnos manuales anteriores ni abre automáticamente la caja del nuevo día.

### Impresión y resumen mensual

**DocumentosPrana.GuardarCierre** genera el HTML del cierre seleccionado. El nombre del archivo contiene fecha y número, por lo que dos cierres del mismo día no se reemplazan entre sí. **frmResumenMensualCaja** consulta los movimientos originales del mes y agrupa por fecha, medio y tipo. No suma los JSON de los cierres ni agrega saldos iniciales como ingresos, evitando duplicar importes cuando existen varios turnos.

## Tablas y migración

Caja conserva id, fecha, estado, id_usuario de cierre, fecha_cierre y datos. Se agregan **inicio**, **fin** e **id_usuario_apertura**. La fecha deja de ser única. Pago y Caja_Movimiento incorporan **id_caja** obligatorio con clave foránea a Caja.

El inicializador ejecuta **CajasPorTurno.sql** después de CajaYComprobantes dentro de su transacción. Asocia por fecha las operaciones de la estructura anterior antes de permitir varios turnos. Si falta una caja histórica, la crea; si encuentra una asignación ambigua, interrumpe en lugar de elegir un turno arbitrario. Recupera horarios del JSON, completa días anteriores pendientes y crea las restricciones e índices. Repetir el inicio conserva las relaciones ya asignadas.

## Verificación

`--turnos-validaciones` comprueba sesión, límites y estados de controles sin base de datos. `--turnos-caja` ensaya aperturas sucesivas y concurrentes, cobros de una venta en turnos distintos, separación de importes, selección desactualizada, cierre automático y total mensual. `--caja-unificada` ensaya la migración desde las tablas anteriores. Las pruebas con SQL utilizan exclusivamente su base aislada configurada en Pruebas/CajaComprobantes.
