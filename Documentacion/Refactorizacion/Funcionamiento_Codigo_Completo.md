# Funcionamiento del código del sistema

## Inicio del programa: Program.cs

Al ejecutar el programa, lo primero que se ejecuta es el archivo **Program.cs**, donde se configuran los formularios de Windows Forms y se asegura la base de datos, ya sea creándola o conectándose a una existente.

Para eso se llama al método **AsegurarBaseDeDatos()** de la clase **InicializadorBaseDatos**. Este método prepara la carpeta donde se guardan los archivos de la base, dentro de **AppData\Local\PranaElectronica\Data** del usuario de Windows. Si la carpeta ya existe, se conserva.

Después construye dos variables con las rutas completas de los archivos:

- **rutaMdf:** ubicación de **PranaElectronica.mdf**, que almacena las tablas y los datos.
- **rutaLdf:** ubicación de **PranaElectronica_log.ldf**, que almacena el registro de transacciones utilizado para la recuperación de la base.

Estas variables solamente contienen las direcciones; todavía no crean los archivos.

A continuación, abre una conexión con la base **master** de LocalDB para comprobar si existe **PranaElectronica**. Si no existe, la crea indicando las rutas MDF y LDF. Si ya existe, utiliza la base registrada.

Luego se conecta a **PranaElectronica** y prepara su estructura. Si está vacía, ejecuta el script que crea las tablas y los datos iniciales. Después aplica los scripts de actualización y de caja, también en los siguientes inicios.

Si la preparación termina correctamente, **Application.Run(new login())** crea y muestra el formulario de acceso. Si ocurre un error, se muestra un mensaje y la aplicación termina sin abrir el login.

## Inicio de sesión: login

Al presionar **Ingresar**, el formulario crea un objeto de la clase **CN_Usuario** y llama a su método **IniciarSesion**, enviándole el nombre de usuario y la contraseña escritos en pantalla.

Este método utiliza **CD_Usuario** para buscar la cuenta en la base de datos. Comprueba que exista, esté activa y no esté bloqueada. Después verifica la contraseña ingresada contra el hash almacenado.

Si la contraseña es incorrecta, registra el intento fallido. Al llegar a tres intentos, la cuenta queda bloqueada durante quince minutos. Si la contraseña es correcta, restablece los intentos fallidos.

El método devuelve un objeto **ResultadoInicioSesion**, que reúne el resultado y la información necesaria para que el formulario decida cómo continuar.

El formulario guarda ese objeto en la variable **resultado** y consulta **resultado.Autenticado**. Si es **false**, muestra el mensaje recibido, limpia la contraseña y termina ese intento. Si es **true**, guarda el usuario en **SesionActual**, abre **Interfaz** y oculta el login.

## Interfaz principal y navegación

En el constructor de **Interfaz** se llama a **InitializeComponent** para crear y configurar los controles. Después se consultan los permisos del usuario para mostrar u ocultar los botones correspondientes, como Usuarios, Informes, Historial y Medios de pago.

Una vez que el usuario inicia sesión correctamente, se crea y muestra la ventana **Interfaz**. Al cargarse, se ejecuta **Interfaz_Load**, que abre por defecto **frmPanelPrincipal** dentro del panel de contenido. Después, el usuario puede cambiar de sección mediante los botones del menú lateral.

La clase tiene una variable llamada **formularioActivo**, que guarda la referencia al formulario que se está mostrando. Al principio su valor es **null** porque todavía no se abrió ninguna sección.

Cuando se presiona un botón del menú, se crea el formulario correspondiente y se llama a **AbrirFormulario**. Este método primero cierra el formulario anterior si existe y después guarda el nuevo en **formularioActivo**.

Luego establece **TopLevel** en **false** para poder colocar el formulario dentro del panel, le quita los bordes y configura **DockStyle.Fill** para que ocupe todo el espacio disponible. Finalmente, limpia el contenido anterior del panel, agrega el nuevo formulario y lo muestra con **Show**.

De esta manera, el menú lateral permanece visible mientras el usuario cambia entre las distintas secciones del sistema.

## Panel principal: frmPanelPrincipal

El formulario **frmPanelPrincipal** es la primera pantalla que se muestra dentro de **Interfaz** después de iniciar sesión. Permite consultar un resumen de las ventas y acceder a nueva venta, stock, caja y comprobantes.

Al crearse el formulario se ejecuta **InitializeComponent**, que prepara los controles de la pantalla. También se configura la grilla para mostrar solamente las columnas definidas: número de venta, fecha, cliente y total.

Para cargar la información se ejecuta el método **CargarVentas**, que llama a **BuscarVentas** de la clase **CN_Venta** y le pasa el contenido del buscador. Si está vacío, devuelve las últimas diez ventas; si tiene texto, devuelve las ventas que coinciden con la búsqueda. Esa lista se asigna al **DataSource** de la grilla para mostrarla y se actualiza el texto que indica cuántos resultados se encontraron.

Después se llama a **ObtenerResumen** para consultar el importe vendido hoy, la cantidad de operaciones del día, el saldo pendiente de cobro y la cantidad de ventas pendientes. Estos datos se muestran en los indicadores superiores y corresponden al resumen general, independientemente de lo escrito en el buscador. Si ocurre un error durante la carga, se limpian los resultados y se muestra un mensaje.

Cuando se selecciona una venta de la grilla, se obtiene el objeto **venta** asociado a esa fila y se muestran debajo su número y sus productos. El botón de comprobantes utiliza el identificador de esa venta para mostrar sus documentos. Si no hay una venta seleccionada, se solicita seleccionar una.

Los botones de nueva venta, stock y caja buscan la ventana **Interfaz** que contiene al formulario y llaman a su método **AbrirFormulario**, pasando una nueva instancia de la pantalla correspondiente para mostrarla dentro del panel de contenido.

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

## Nueva venta: frmNuevaVenta.cs

### Servicios, selecciones y listas temporales

El formulario **frmNuevaVenta** prepara una venta completa antes de guardarla. Mantiene referencias a **CN_Cliente**, **CN_Producto**, **CN_MedioPago** y **CN_RegistroVenta**. Los atributos **clienteElegido** y **productoElegido** guardan las selecciones actuales. **detallesVenta** conserva los productos y sus cantidades; **pagosVenta** conserva los cobros preparados. Ambas listas están en memoria hasta registrar la venta. **ventaRegistrada** conserva el identificador devuelto después del guardado y evita volver a registrar desde esa misma instancia.

### Constructor: documentos obligatorios y búsqueda de clientes

El constructor ejecuta **InitializeComponent**, configura los textos de documentos y deshabilita la fecha de pago. La garantía y el remito se generan siempre al registrar la venta; no hay una casilla para elegirlo. También prepara un temporizador de 250 milisegundos para buscar clientes después de una pausa al escribir. Cuando se libera el formulario se libera ese temporizador.

### CargarVenta: configuración y datos iniciales

**frmNuevaVenta_Load** llama a **CargarVenta**. Este método carga clientes, productos y medios de pago, prepara las columnas del cliente seleccionado —incluidas localidad y código postal— y agrega las cuotas disponibles. Inicialmente selecciona retiro en el local, pago total y efectivo si se encuentra entre los medios disponibles. Después actualiza cuotas e importes.

### CargarClientes: consulta por prefijo y filtro de estado

**CargarClientes** recibe un texto o toma el contenido del ComboBox y consulta **CN_Cliente.buscarCliente** con búsqueda por prefijo. El filtro es cero cuando se buscan solamente activos y cuatro cuando se incluyen clientes sin compras recientes, sin incluir eliminados. Las coincidencias se agregan al ComboBox como objetos de la clase interna **OpcionCliente**.

### OpcionCliente: objeto conservado y texto visible

**OpcionCliente** conserva el objeto **Cliente** completo y sobrescribe **ToString** para mostrar nombre, apellido y documento. El texto visible permite reconocer la opción, mientras que el objeto conserva el identificador y los demás datos necesarios para la venta.

### TextUpdate y esperaClientes: cuándo volver a buscar

**cboCliente_TextUpdate** invalida el cliente anterior si se empieza a escribir otra búsqueda. Limpia su detalle y, cuando corresponde, el domicilio copiado. Después reinicia el temporizador. **esperaClientes_Tick** detiene el temporizador y ejecuta la consulta. **actualizandoListaClientes** distingue esos cambios programados de los cambios realizados por el usuario y evita repetir consultas por restaurar el texto.

### Enter y casilla de clientes sin compras recientes

**cboCliente_Enter** despliega las opciones ya cargadas si existen. El cambio de la casilla para incluir clientes sin compras recientes repite la búsqueda con el filtro correspondiente.

### ElegirCliente: conservar la selección y mostrar sus datos

**ElegirCliente** comprueba que la opción seleccionada sea un **OpcionCliente**, detiene la búsqueda pendiente, asigna su cliente a **clienteElegido** y completa la grilla del cliente. También reinicia los datos de domicilio para que no se conserve la dirección de otra persona. Lo llaman la confirmación de selección del ComboBox y el manejador **BtnAgregarCliente_Click**; este último no crea un cliente nuevo.

### CargarProductos y OpcionProducto: opciones del desplegable

**CargarProductos** consulta los productos de la base de datos y recorre sus variantes. Por cada combinación agrega un **OpcionProducto** al ComboBox. Esa clase conserva dos objetos: el producto y su variante. Su **ToString** muestra el nombre del producto seguido del color y la condición de la variante. No agrega automáticamente el modelo ni la marca a ese texto.

### TextUpdate, SelectionChangeCommitted y ElegirProducto

Al escribir, **cboBuscarProducto_TextUpdate** limpia la referencia a la opción anterior. **SelectionChangeCommitted** conserva la opción elegida por el usuario. **ElegirProducto** vuelve a resolverla antes de agregar: primero revisa **SelectedItem** y, si no hay objeto seleccionado, intenta encontrar una coincidencia exacta con el texto autocompletado mediante **FindStringExact**.

### AgregarProducto y PrepararProducto: cantidad y repetidos

**AgregarProducto** pasa el producto, la variante, la cantidad y los detalles existentes a **CN_RegistroVenta.PrepararProducto**. El negocio valida la selección y rechaza una variante repetida, indicando que se quite y vuelva a agregar con la cantidad requerida. Si la preparación termina correctamente, incorpora el detalle a la lista temporal y llama a **MostrarDetalle**.

### MostrarDetalle y Quitar: filas de la venta en preparación

**MostrarDetalle** reconstruye las filas con producto, variante, cantidad, precio unitario, subtotal, botón Quitar, stock físico y stock comprometido. Después recalcula los totales. El botón Quitar elimina el elemento de **detallesVenta** por su índice y reconstruye la presentación. Todavía no se está eliminando una venta de la base de datos.

### Limpiar la selección y conservar las opciones de productos

Después de agregar se limpia la selección y el texto del ComboBox, pero se conserva la lista de opciones para elegir otro producto. La búsqueda de productos de esta pantalla no exige escribir cinco letras.

### TotalBase y TotalConRecargos: importes de la venta

**TotalBase** obtiene el importe de los productos menos el descuento, más el envío cuando se selecciona envío a domicilio. Delega el cálculo en **CalcularTotalBase**. **TotalConRecargos** agrega al total base los recargos de los pagos que ya se incorporaron a la lista temporal.

### ActualizarTotales y ActualizarMontoSugerido

**ActualizarTotales** calcula el subtotal y actualiza descuento y envío. Si el usuario no modificó el importe del pago, llama a **ActualizarMontoSugerido**. Este último toma la base pendiente, calcula el recargo del medio seleccionado y propone el importe final a cobrar. La bandera **actualizandoMontoSugerido** permite distinguir ese cambio automático de un importe escrito por el usuario.

### ActualizarEntrega y ActualizarDireccionEntrega

**ActualizarEntrega** habilita domicilio, logística, costo y fecha según la modalidad elegida. La fecha se puede completar para envío y retiro posterior. **ActualizarDireccionEntrega** copia la dirección del cliente cuando se marca la casilla correspondiente y evita editarla mientras se está usando esa dirección registrada. Cambiar la modalidad o el costo vuelve a calcular los importes.

### ActualizarCuotas y PorcentajeRecargoSeleccionado

**ActualizarCuotas** muestra el selector de cuotas solamente cuando el medio tiene tipo de caja Crédito. Fuera de crédito vuelve a la primera opción. **PorcentajeRecargoSeleccionado** obtiene el medio y las cuotas aplicables y consulta el porcentaje correspondiente en **CN_RegistroVenta**.

### AgregarPago: separar el cobro y validar la condición de pago

En esta pantalla el campo representa el dinero cobrado con recargo incluido. **AgregarPago** llama a **PrepararPago**, que separa la base y el recargo. Antes comprueba si ya se cubrió la venta: si la condición sigue en pago parcial, indica seleccionar pago total; si ya está en pago total, indica registrar la venta sin agregar otro pago.

### pagosVenta, MostrarPagos y Quitar

Si la preparación es válida, agrega el objeto **Pago** a **pagosVenta**, limpia la referencia de cuenta, actualiza las filas y propone el importe para el saldo siguiente. **MostrarPagos** reconstruye las filas con el medio y el total cobrado; mantiene los datos de base y recargo aunque esas columnas se configuran ocultas. Quitar un pago lo elimina de la lista temporal.

### ActualizarResumenPago: cobrado, deuda y recargo previsto

**ActualizarResumenPago** calcula lo cobrado, la base pendiente y el recargo previsto para cobrar ese saldo con el medio seleccionado. Puede mostrar un total estimado: ese recargo futuro es una cotización y no un pago guardado. Cambiar medio, cuotas o importe actualiza la presentación. Cambiar a pago total vuelve a sugerir el importe pendiente.

### ObtenerVentaDelFormulario: construir el objeto venta

**ObtenerVentaDelFormulario** construye un objeto **venta** con cliente, usuario de la sesión, descuento, condición del pago, modalidad y datos de entrega, segundo teléfono y copias de las listas de productos y pagos. Construir ese objeto no lo guarda.

### RegistrarVenta y CN_RegistroVenta: validación y guardado

**RegistrarVenta** comprueba que esa instancia no tenga una venta ya registrada y revisa el límite de importe del formulario. Después llama a **CN_RegistroVenta.Registrar**, donde se valida el contenido y se solicita a la capa de datos guardar la operación. Entre las validaciones de negocio están cliente y usuario válidos, productos, cantidades, descuento, modalidad, fecha del cobro y correspondencia entre total y pagos.

### CD_RegistroVenta.Guardar: conexión y transacción

**CN_RegistroVenta.Registrar** llama a **CD_RegistroVenta.Guardar**, pasando el objeto **venta** y un booleano que indica si se retira inmediatamente. **Guardar** abre una **SqlConnection** y crea una **SqlTransaction**. Todos los comandos del registro utilizan esa misma conexión y transacción. Así, el encabezado, los productos, los pagos, los datos de entrega y las actividades del historial se confirman juntos. Los bloques **using** liberan los recursos al salir del método.

Primero obtiene la hora de SQL Server mediante **CD_Caja.Ahora** y verifica con **CD_Caja.ExigirDiaAbierto** que se pueda operar en esa fecha. Después obtiene los identificadores de la modalidad de entrega y los estados. **ObtenerOCrearIdPorNombre** busca cada nombre y, si no existe, lo crea dentro de la misma transacción. La venta queda Pendiente si no hay pagos, Pagada si la condición es Pago total o Parcial en los demás casos. El estado de sus detalles será Entregado si hay retiro inmediato o Pendiente si no lo hay.

### Siguiente número: MAX, COALESCE y bloqueo de Venta

El número se calcula dentro de **Guardar** con la consulta **SELECT COALESCE(MAX(id_venta), 2499) + 1 FROM dbo.Venta WITH (TABLOCKX, HOLDLOCK)**. **MAX(id_venta)** devuelve el mayor identificador de las ventas que existen en la tabla. **COALESCE** usa 2499 cuando no hay registros, por lo que la primera venta recibe 2500. El resultado se convierte a **int** y se conserva en la variable **idVenta**.

**TABLOCKX** solicita un bloqueo exclusivo sobre la tabla **Venta** y **HOLDLOCK** mantiene la protección hasta que termina la transacción. Si otra operación intenta guardar una venta al mismo tiempo, debe esperar. Cuando puede continuar, calcula el siguiente número a partir de los registros que quedaron guardados. El bloqueo también puede hacer esperar otras consultas sobre esa tabla mientras se registra la venta.

Por ejemplo, si el mayor número guardado es 2508, el método elige 2509. Si el guardado falla y se revierte, la fila 2509 desaparece y el siguiente intento vuelve a calcular 2509. Si ya existe una venta 3505, el siguiente número será 3506: este cambio no renumera registros existentes ni completa huecos anteriores. Tampoco utiliza una tabla adicional para llevar el contador.

### IDENTITY_INSERT: guardar explícitamente idVenta

La columna **id_venta** conserva su definición **IDENTITY** en la base, pero este método ya no le pide a SQL Server que elija automáticamente el identificador. Ejecuta **SET IDENTITY_INSERT dbo.Venta ON**, que permite indicar un valor para esa columna, y luego inserta el encabezado con el parámetro **@venta**, cuyo valor es **idVenta**. Los demás parámetros contienen cliente, usuario, estado, total, modalidad y descuento; la fecha del encabezado se obtiene con **SYSDATETIME()**.

Después del insert ejecuta **SET IDENTITY_INSERT dbo.Venta OFF**. Por eso un salto en el contador interno de IDENTITY no determina el número utilizado por este método: el número se obtiene de las filas existentes. Esta configuración requiere que el usuario de la conexión tenga autorización para utilizar **IDENTITY_INSERT** sobre **Venta**.

### Detalles, pagos, entrega e historial relacionados con la venta

El método recorre **venta.Detalles** e inserta cada variante en **Detalle_Venta**, con cantidad, precio, subtotal y estado. Todos los detalles reciben el mismo **idVenta**. Después recorre **venta.Pagos** e inserta los cobros en **Pago**: **monto** conserva la base y **monto_final** conserva base más recargo. También guarda medio, cuotas, usuario, fecha, tipo de caja y cuenta de destino.

Guarda domicilio, fecha estimada, horario, observaciones, logística, costo de envío y condición del pago en **Venta_Entrega**. Si corresponde retiro inmediato, llama a **CD_Entrega.RegistrarRetiroInmediato** dentro de la misma transacción. Finalmente registra **REGISTRAR_VENTA** en **LogActividad** y agrega **APLICAR_DESCUENTO** si hubo descuento. Estas escrituras quedan relacionadas con el número elegido al principio.

### Commit, Rollback y limpieza de IDENTITY_INSERT

Si todos los pasos terminan correctamente, **transaccion.Commit()** confirma las escrituras y el método devuelve **idVenta**. Recién entonces el formulario recibe el número de una venta guardada. La garantía y el remito utilizan ese mismo identificador; no tienen un contador independiente.

Si ocurre un error, **catch** ejecuta **transaccion.Rollback()** para deshacer las escrituras de la operación y **throw** propaga el error a quien llamó al método. La próxima venta calcula nuevamente el número desde las filas existentes. El método no confirma por separado el encabezado antes de guardar sus detalles.

La variable **insercionExplicitaActivada** indica si podría haber quedado habilitado **IDENTITY_INSERT**. El bloque **finally** intenta desactivarlo si hubo un error antes de llegar al OFF normal. Si esa limpieza también falla, **SqlConnection.ClearPool(conexion)** descarta las conexiones del grupo correspondiente para evitar reutilizar esa sesión con la opción activada. La limpieza no convierte un registro fallido en uno exitoso.

### Identificador obtenido, documentos y errores posteriores

Cuando recibe el identificador, actualiza el número visible, deshabilita el botón y conserva el identificador en **ventaRegistrada**. A continuación intenta mostrar los documentos. Si falla esa parte, informa que la venta ya se guardó y que se pueden reimprimir los documentos sin repetirla. Si falla el registro, muestra el error y termina la operación sin mostrar ningún mensaje de éxito.

### Confirmación para abrir otra venta

Después del registro correcto pregunta si se desea cargar otra venta. Si se acepta, busca la ventana **Interfaz** y le solicita abrir una nueva instancia de **frmNuevaVenta**.

### Ventana incluida: frmComprobantes

### MostrarVenta: consultar venta y generar los archivos

**DocumentosPrana.MostrarVenta** consulta la información vigente mediante **CN_Comprobante.Obtener**, que devuelve un objeto de la clase **venta** con su **Cliente**, sus **Detalles**, sus **Pagos** y sus datos de entrega. Los documentos utilizan esa misma clase; no hay una clase de datos adicional para los comprobantes. Después incorpora el segundo teléfono recibido, genera los archivos y abre **frmComprobantes** con **ShowDialog**.

### Constructores, Abrir y botones Garantía y Remito

**frmComprobantes** tiene un constructor sin parámetros para preparar los controles y otro que recibe el número y la carpeta de documentos. Este último guarda ambos datos y actualiza el título y el mensaje. El botón de garantía llama a **Abrir("Garantia")** y el de remito a **Abrir("Remito")**. **Abrir** construye la ruta del archivo HTML y utiliza **DocumentosPrana.Abrir** para abrirlo con la aplicación asociada de Windows. Si no puede hacerlo, muestra el error.

### Qué hace esta ventana y desde dónde se utiliza

Esta ventana no registra una venta ni un pago. También se utiliza al consultar comprobantes desde el panel principal. En cambio, el botón de garantía de Pagos abre directamente el archivo de garantía actualizado, como se explica en su sección.

## Clientes: frmClientes.cs

### Constructor, grilla y control de selección

**frmClientes** reúne creación, consulta, modificación y cambios de estado del cliente. El constructor configura la grilla para usar sus columnas definidas y prepara un ComboBox editable para buscar. La bandera **seleccionandoCliente** evita tratar la reconstrucción de opciones como una búsqueda escrita por el usuario.

### CargarClientes: cargar y presentar los resultados

**frmClientes_Load** llama a **CargarClientes**. Este método crea un **CN_Cliente**, consulta según el estado seleccionado y asigna la lista al **DataSource**. También coloca el texto Modificar cliente en las filas y actualiza el botón de estado.

### ObtenerClienteDelFormulario y tipo de documento

**ObtenerClienteDelFormulario** construye un **Cliente** con nombre, apellido, documento, tipo, teléfono, dirección, código postal y un objeto **Localidad** con el nombre ingresado. Asigna la fecha actual y saldo a favor inicial cero. El cambio del tipo de documento limita la entrada a once caracteres para CUIT y ocho para los demás casos configurados.

### Crear cliente: guardar, limpiar y actualizar

**btn_CrearCliente_Click** obtiene ese objeto y lo pasa a **NuevoCliente**. Si el resultado es correcto, muestra el aviso, limpia los campos y recarga la lista. Si la operación falla, informa el resultado o la excepción y conserva el flujo de error.

### BuscarClientes y ActualizarSugerencias

**BuscarClientes** consulta por prefijo mediante **CN_Cliente**. El cambio de texto inicia **esperaBusqueda**, cuyo evento **Tick** llama a **ActualizarSugerencias**. Este método reconstruye el desplegable con objetos **OpcionCliente**, conserva el texto escrito y abre las coincidencias si el control tiene el foco. La clase interna conserva el cliente completo y presenta nombre, documento y estado de cuenta.

### Elegir una coincidencia, Buscar, Enter y temporizador

Al confirmar una sugerencia se llama a **MostrarClientes** con una lista que contiene ese cliente. El botón Buscar consulta el texto ingresado y muestra las coincidencias. Enter confirma una opción si existe o ejecuta la búsqueda; **SuppressKeyPress** evita que la tecla siga siendo procesada por el control. **OcultarSugerencias** detiene el temporizador y cierra el desplegable. El evento **Disposed** libera el temporizador.

### ActualizarBotonEstadoCliente: baja, activación y restauración

**ActualizarBotonEstadoCliente** obtiene el cliente de la fila actual y decide si el botón debe indicar Dar de baja, Activar o Restaurar. Para dar de baja se comprueba el permiso **DarDeBajaClientes**. **btnCambiarEstadoCliente_Click** solicita confirmación y llama a **DarDeBajaCliente**, **ActivarCliente** o **RestaurarCliente** según el estado. Después vuelve a consultar los registros.

### CellClick: habilitar la edición de un cliente

**dgv_listadoClientes_CellClick** distingue la columna pulsada. En la columna de modificación, el primer clic habilita los datos editables de esa fila, cambia el botón a Guardar cambios e inicia la edición. Si el cliente está eliminado, solicita restaurarlo antes.

### Guardar cambios, EndEdit y formato de la deuda

Al guardar, termina la edición con **EndEdit**, obtiene los valores de las celdas, actualiza el objeto **Cliente**, reconstruye su **Localidad** y llama a **ModificarCliente**. Si se guarda, recarga la grilla y muestra la confirmación. La columna de deuda usa **CellFormatting** para destacar importes positivos: es un cambio visual, no una actualización de deuda en la base.

### Ventana incluida: frmDeudaCliente

### Ver detalle: apertura y cliente recibido

Al pulsar la columna Ver detalle se obtiene el cliente de esa fila, se crea **frmDeudaCliente(cliente)** y se abre con **ShowDialog(this)**. El constructor conserva el cliente recibido y muestra su nombre y documento.

### Load y ObtenerDeudasCliente: ventas con saldo y errores

En **frmDeudaCliente_Load** se llama a **CN_Venta.ObtenerDeudasCliente** con el identificador del cliente. La lista de ventas pendientes se asigna a **dgvDeudas**. Un texto informa si no hay deudas o cuántas ventas tienen saldo. El saldo presentado corresponde a cada venta completa. Ante un error se muestra que la consulta no pudo realizarse. Esta ventana consulta deudas; no registra cobros.

### CD_Cliente: diferencia entre activo, inactivo y eliminado

Antes de presentar clientes, la consulta actualiza su marca **activo** usando la actividad y la deuda. Si **eliminado** es verdadero, activo queda falso. Si el cliente tiene deuda o actividad dentro del último año, queda activo; en los demás casos queda inactivo. Por eso inactivo y dado de baja no son el mismo estado: la baja depende de eliminado y tiene prioridad sobre el cálculo de actividad.

La actividad considerada incluye registros que permiten reconocer compras o reactivaciones. Al activar o restaurar se registra su actividad correspondiente, para que esa decisión no se pierda inmediatamente al recalcular la lista. La pantalla elige entre dar de baja, activar o restaurar según esas marcas. Los saldos y los productos pendientes se calculan con las operaciones relacionadas, no leyendo el texto de los comentarios del código ni la descripción del historial.

### ValidarTelefono: código de área y número juntos

**CN_Cliente.ValidarTelefono** recibe un texto que puede ser null y comprueba la expresión **\A(?:11|[23][0-9])[0-9]{8}\z**. **\A** exige el comienzo del texto y **\z** su final exacto. El primer grupo admite 11 o una pareja de dígitos que comience con 2 o 3. **[0-9]{8}** exige ocho dígitos adicionales. En total acepta diez dígitos sin espacios, guiones, paréntesis ni prefijos internacionales. Por ejemplo, 2323516501 cumple el formato; 549888 555 no lo cumple. La expresión comprueba un formato, no consulta si el número existe ni si pertenece al cliente.

**NuevoCliente** y **ModificarCliente** llaman al mismo método después de validar el documento y antes de consultar o guardar en la base. Si falla, lanza **InvalidOperationException** con un mensaje que indica el formato y un ejemplo. Así la modificación desde la grilla no evita la validación utilizada al crear un cliente. Los teléfonos anteriores no se reescriben automáticamente: un valor inválido debe corregirse al guardar una modificación.

El constructor de **frmClientes** agrega el ejemplo al **PlaceholderText** del campo y limita a diez caracteres la edición de **ColTelefono**. La etiqueta indica Teléfono con área. Estos ajustes ayudan al usuario, pero la comprobación decisiva sigue estando en negocio y también rechaza valores inválidos que lleguen por otro camino.

## Stock y productos: frmStock.cs

### BindingList y estado de las operaciones temporales

**frmStock** contiene tres recorridos: preparar un producto con variantes, buscar y modificar variantes existentes, y registrar ingresos de mercadería. Mantiene dos **BindingList<VarianteProducto>**: **variantesTemporales** para el producto en preparación e **ingresoTemporales** para las variantes que se incorporarán mediante un ingreso. **BindingList** notifica cambios de la colección a los controles enlazados.

### Constructor y Load: controles, listas y opciones

El constructor prepara los ComboBox de marca, categoría y proveedor con autocompletado nativo y fija las longitudes máximas de los campos. **frmStock_Load** carga opciones de marcas, categorías y proveedores, enlaza las listas temporales con sus grillas y configura las columnas del ingreso. En esa grilla producto, variante y condición se muestran como referencia; cantidad y costo se completan en las celdas correspondientes. También carga los filtros de marca y categoría para buscar productos.

### Agregar variante: validar color, condición, precio y repetidos

**btnAgregarVariante_Click** exige color, condición y precio mayor que cero. Comprueba que no exista ya una variante temporal con el mismo color y condición, sin distinguir mayúsculas. Construye un **VarianteProducto**, lo incorpora a **variantesTemporales**, completa las celdas de nombre y modelo y limpia los campos específicos de la variante. Esta operación todavía no registra el producto ni ingresa unidades al stock.

### ObtenerProductoDelFormulario: datos generales y variantes

**ObtenerProductoDelFormulario** construye un **producto** con nombre, modelo, categoría, marca, descripción y una copia de las variantes temporales. Los campos se asignan separadamente: el formulario no concatena nombre, modelo, marca y color en un único atributo. La forma de presentar esos datos depende después del selector o del comprobante que los utilice.

### Guardar Producto: validaciones, guardado e historial

**btnGuardarProducto_Click** comprueba los campos requeridos y que exista al menos una variante. Pasa el objeto a **CN_Producto.NuevoProducto**, que aplica sus reglas y solicita el guardado. Si se guarda, registra la actividad de creación, actualiza las opciones y limpia el formulario. Los errores previstos de datos se muestran como advertencias y los demás como error de guardado.

### Cancelar y métodos de limpieza

**LimpiarCamposVariante** borra color, condición y precio de preparación. **LimpiarFormulario** también borra los datos generales y la lista temporal. El botón Cancelar pide confirmación antes de ejecutar esa limpieza. **CargarOpcionesProducto** vuelve a consultar las marcas y categorías disponibles.

### Buscar producto: filtros y resultados por variante

**btnBuscarProducto_Click** obtiene nombre, modelo, categoría, marca y condición de los controles utilizados como filtros. Cuando no se eligió una categoría o marca, envía **null**. Consulta **CN_Producto.buscarProducto** y agrega una fila por variante con sus identificadores, datos descriptivos, precio, costo y stocks físico y comprometido.

### Limpiar filtros: restablecer la búsqueda

**BtnLimpiarFiltros_Click** borra el texto de búsqueda, las selecciones de categoría, marca y condición y las filas de resultados. Es una limpieza de pantalla; no elimina los productos de la base de datos.

### Desactivar variante: confirmación y actualización

**dgvBusquedaProductos_CellContentClick** distingue el botón de desactivar del botón de modificar. Al desactivar solicita confirmación, llama a **DesactivarVariante**, registra la actividad y repite la búsqueda. Se desactiva la variante; el formulario no ejecuta un borrado físico de la fila.

### filaEnEdicion, BeginInvoke y edición de una fila

Al iniciar una modificación guarda el índice de la fila en **filaEnEdicion**, cambia el texto del botón y comienza la edición. Impide guardar desde otra fila mientras esa modificación siga pendiente. **BeginInvoke** programa el inicio de la edición para después del procesamiento del clic actual. El evento **CellClick** permite comenzar a editar una celda de esa fila, excluyendo identificadores, stock comprometido y botones.

### Guardar modificación: lectura y validación de las celdas

Para guardar llama a **EndEdit**, lee las celdas y comprueba datos obligatorios y conversiones de precio, costo y stock físico. Después llama a **ActualizarProductoDesdeStock**, registra la actividad, termina el estado de edición y repite la consulta. Las validaciones de negocio pueden ser más estrictas que las comprobaciones iniciales de pantalla.

### CargarProductoParaModificar: método separado del recorrido actual

El archivo también contiene **CargarProductoParaModificar**, que copia un producto a los campos y variantes temporales. No debe confundirse con el recorrido de modificación de la grilla: en el código revisado el botón de búsqueda modifica las celdas y llama a **ActualizarProductoDesdeStock**; no llama a ese método de copia.

### Buscar producto y OnVarianteSeleccionada

El botón cuyo manejador es **iconButton1_Click** abre el selector de productos. Antes se suscribe al evento **OnVarianteSeleccionada**. Cuando el selector entrega una combinación de producto y variante, el formulario la agrega a **ingresoTemporales**, completa las celdas descriptivas y deja cantidad y costo para completar.

### CellEndEdit y ActualizarTotalesIngreso

**dgvDetalleIngreso_CellEndEdit** recalcula al terminar de editar cantidad o costo. **ActualizarTotalesIngreso** recorre las filas, calcula cantidad por costo y muestra las sumas. Su cálculo de presentación no reemplaza la validación definitiva al registrar.

### Registrar ingreso: proveedor, documento, cantidades y costos

**ButtonRegistrarIngreso_Click** termina la edición y comprueba que haya elementos, proveedor y documento del proveedor. Recorre las filas y construye una lista de tuplas con identificador de variante, cantidad y costo unitario. Exige cantidades enteras positivas y un costo convertible cuando se completó. Después llama a **CN_Producto.RegistrarIngreso**, pasando proveedor, fecha, remito y detalles.

### LimpiarIngreso y errores posteriores al guardado

Si se guarda, **LimpiarIngreso** vacía la lista temporal, proveedor y documento y restablece la fecha y los totales. Después se actualizan proveedores y se registra la actividad. Si falla esa actualización posterior, se informa expresamente que el ingreso ya se guardó para evitar repetirlo.

### Ventana incluida: frmSeleccionarProducto, clase buttonSeleccionarProducto

### Clase del formulario y evento de selección

El archivo **frmSeleccionarProducto.cs** declara una clase que se llama **buttonSeleccionarProducto**, aunque hereda de **Form**: es una ventana, no un botón. Mantiene un **CN_Producto** y expone el evento **OnVarianteSeleccionada** de tipo **Action<VarianteProducto, producto>**.

### Load y CargarProductos: buscar y presentar variantes

Al cargar ejecuta **CargarProductos** con texto vacío. El botón Buscar repite la consulta con el texto escrito. Por cada producto y variante agrega una fila con identificador, nombre, modelo, color, condición, stock físico y costo.

### Botón Agregar: consultar y devolver la variante elegida

Al pulsar la columna Agregar obtiene el identificador de variante, vuelve a consultar el producto mediante **obtenerProductoPorVariante**, encuentra la variante correspondiente y ejecuta **OnVarianteSeleccionada?.Invoke**. De esa manera entrega ambos objetos al código que se suscribió desde Stock. La selección no registra por sí misma un ingreso.

### Show, Cancelar y diferencia entre evento y propiedades

Stock abre esta ventana mediante **Show()**. Agregar una variante no la cierra automáticamente, lo que permite incorporar otras. Cancelar establece el resultado Cancel y cierra la ventana. Las propiedades **ProductoSeleccionado** y **VarianteSeleccionada** están declaradas, pero este recorrido devuelve la selección mediante el evento, no mediante esas propiedades.

### Ventana incluida: frmOpcionesStock

### btnOpcionesStock_Click y permiso de administrador

Stock muestra **Administrar opciones** solamente si la sesión tiene **Permisos.AdministrarOpcionesStock**, que no está entre los permisos del vendedor. El clic vuelve a exigirlo y abre **frmOpcionesStock** con **ShowDialog(this)**. Al cerrar, actualiza las opciones de producto y los proveedores. La ventana utiliza controles nativos de Windows Forms: selector del tipo, listado de nombres, cantidad de registros vinculados, campo de corrección y selector de destino para unificar.

### Cargar, Seleccionar y Ejecutar

**Cargar** solicita las opciones a **CN_OpcionStock.Obtener**. La grilla muestra Nombre y Usos. **Seleccionar** copia el nombre al campo de edición y ofrece como destinos las demás opciones. **Ejecutar** distingue corregir, eliminar y unificar; exige una selección, muestra una confirmación con los nombres concretos y llama a negocio. Después vuelve a consultar para presentar el resultado guardado. Los errores mantienen abierta la ventana y se informan mediante MessageBox.

### CN_OpcionStock y CD_OpcionStock

Negocio exige el permiso tanto al consultar como al guardar. Normaliza el nombre a mayúsculas y comprueba hasta cien caracteres o cincuenta para colores. **TipoOpcionStock** limita los tipos admitidos. La capa de datos utiliza una correspondencia interna de tablas y columnas; el texto del usuario se envía como parámetro y nunca se utiliza como identificador SQL.

**CD_OpcionStock.Guardar** abre una transacción, bloquea la tabla de opciones y comprueba que el origen todavía exista. Corregir actualiza el nombre conservando sus relaciones. Unificar cambia las referencias del origen al destino y elimina la opción sobrante. Eliminar comprueba primero que no haya productos o ingresos vinculados. Una restricción de la base también puede rechazar la eliminación; cualquier error revierte la operación completa.

### Colores e historial de los cambios

Los colores están guardados como texto en Variante_Producto. Se consultan agrupando los nombres normalizados y se corrigen actualizando las variantes correspondientes. Unificar dos colores aplica el nombre elegido a las variantes del origen. Eliminar un color utilizado no borra variantes: se rechaza y solicita corregir o unificar.

Los cambios se registran en la misma transacción con las acciones **MODIFICAR_OPCION_STOCK**, **UNIFICAR_OPCION_STOCK** y **ELIMINAR_OPCION_STOCK**. En colores, la unificación sigue el mismo procedimiento de corrección del texto y queda registrada como MODIFICAR_OPCION_STOCK. La entidad identifica Proveedor, Categoria, Marca o Color; el detalle conserva nombre anterior y nombre de destino. No se agregaron tablas nuevas para esta administración.

### CargarOpcionesProducto y ObtenerColores

El campo **txtColorVariante** ahora es un **ComboBox**, aunque conserva el nombre del control anterior para mantener sus referencias. El constructor configura Color, Categoría, Marca y Proveedor con **DropDownStyle.DropDown**, **AutoCompleteSource.ListItems** y **AutoCompleteMode.SuggestAppend**. Así se puede elegir una opción existente o escribir otra. El autocompletado es nativo de Windows Forms.

**CargarOpcionesProducto** limpia y vuelve a cargar las opciones de esos campos. Para colores llama a **CN_Producto.ObtenerColores**, que delega en **CD_Producto.ObtenerColores**. La consulta lee **Variante_Producto.color**, quita espacios en los extremos, convierte a mayúsculas, excluye textos vacíos y utiliza **DISTINCT** para mostrar cada color una sola vez. No existe una tabla Color independiente.

Escribir un color nuevo no guarda nada en ese momento. **btnAgregarVariante_Click** lo copia al objeto temporal **VarianteProducto**; al guardar el producto se conserva en la columna color de sus variantes. La recarga posterior permite ofrecerlo en el desplegable. **LimpiarCamposVariante** restablece SelectedIndex a menos uno y vacía Text, conservando las opciones del control.

### OnVarianteSeleccionada: impedir repetidos en el ingreso

**iconButton1_Click** abre el selector y se suscribe a **OnVarianteSeleccionada**. Antes de ejecutar **ingresoTemporales.Add**, el manejador utiliza **Any** para comprobar si ya existe una variante con el mismo **IdVariante**. Si existe, muestra que se modifique la cantidad en la fila actual y ejecuta return. No agrega otra fila ni suma unidades automáticamente.

La comprobación se hace sobre la lista del ingreso de Stock, por lo que sigue funcionando al cerrar y volver a abrir el selector o al abrir más de una ventana de selección. El identificador corresponde a la variante: dos colores o condiciones diferentes de un mismo producto pueden ocupar filas distintas. La lista se vacía al limpiar o completar el ingreso y no impide registrar esa variante en otro ingreso posterior.

### dgvDetalleIngreso_CellContentClick: botón Eliminar

La columna nativa **colEliminarIngreso**, declarada en **frmStock.Designer.cs**, es un **DataGridViewButtonColumn** con el texto Eliminar en sus filas. Pertenece a **dgvDetalleIngreso**, donde se preparan cantidad y costo del ingreso; no pertenece a la ventana que busca productos.

**dgvDetalleIngreso_CellContentClick** descarta los clics en encabezados y las columnas que no son Eliminar. Recupera **DataBoundItem** de la fila como **VarianteProducto** y llama a **ingresoTemporales.Remove**. Como esa lista es una **BindingList**, el cambio se refleja en la grilla. Después ejecuta **ActualizarTotalesIngreso** para recalcular cantidad e importe. Retira un elemento temporal: no borra el producto, la variante ni un ingreso ya guardado. Esa variante puede volver a elegirse desde el selector.

### productosIngresados y detalle del historial

Durante **ButtonRegistrarIngreso_Click**, el formulario recorre las filas y prepara dos listas. **detalles** conserva tuplas con **idVariante**, **cantidad** y **costoUnitario** para el guardado. **productosIngresados** conserva los textos descriptivos destinados al historial. Por cada fila lee nombre del producto, color y condición, reúne los datos no vacíos y agrega una descripción con cantidad, por ejemplo HELADERA WHIRLPOOL (BLANCA, PRIMERA) x 3.

La lista de descripciones se prepara antes de llamar a **LimpiarIngreso**, porque ese método vacía las filas de la operación terminada. Después del guardado, **string.Join** une sus elementos con punto y coma y construye **detalleIngreso** con proveedor, **Doc.proveedor**, número o código del documento y productos. Ese texto se envía a **cnLogActividad.Registrar** con acción **REGISTRAR_INGRESO_STOCK**, entidad IngresoStock e identificador null.

La variable local **remito** contiene **txtDocumentoProveedor.Text.Trim()**. El nombre de la variable y el nombre **remito_proveedor** de la columna no hacen que sea un remito de venta: en este circuito identifica el documento recibido del proveedor. Los textos antiguos del historial pueden seguir diciendo Remito o Cantidad de variantes porque se guardaron con la descripción que existía en aquel momento; consultar el historial no reconstruye esos textos.

### CN_Producto y CD_Producto.RegistrarIngreso: escritura del ingreso

**CN_Producto.RegistrarIngreso** valida el proveedor y los detalles y solicita el guardado. En **CD_Producto.RegistrarIngreso** se abre una conexión y una transacción. Si se recibió el nombre del proveedor, **CD_Proveedor.ObtenerOCrear** busca el existente o lo crea dentro de esa misma transacción. Después se inserta **Ingreso_Stock**, guardando proveedor, documento y fecha; **OUTPUT INSERTED.id_ingreso** devuelve el identificador del encabezado.

Por cada tupla se inserta **Detalle_Ingreso** con ese identificador, variante, cantidad y costo unitario. También se actualiza **Variante_Producto**, sumando cantidad a **stock_disponible** y estableciendo el costo unitario del ingreso como costo de la variante. No se cambia el precio de venta con ese comando. Si la variante no existe, se informa el error y se revierte el conjunto. Si todos los pasos terminan, Commit confirma encabezado, proveedor nuevo si lo hubo, detalles y existencias.

El historial se registra después desde el formulario y no pertenece a esa transacción. Por eso el mensaje de error posterior aclara que el ingreso ya se guardó si falla la actualización de opciones o el historial. No se debe registrar otra vez para intentar producir la actividad faltante. La aplicación todavía no tiene una ventana específica que consulte los detalles completos de todos los ingresos: el historial muestra la descripción que se escribió al registrarlos.

## Pagos: frmPagos.cs

### Servicios, bandera cargando y propiedad Seleccionada

**frmPagos** registra cobros de ventas existentes. Mantiene un **CN_Pago** y una bandera **cargando**. La propiedad **Seleccionada** obtiene el objeto **venta** asociado a la fila actual de **dgvVentas** o devuelve **null** si no existe.

### Constructor y ConfigurarGrillas

El constructor ejecuta **ConfigurarGrillas**, que define explícitamente las columnas de ventas y pagos mediante **DataPropertyName**. La grilla de ventas muestra número, fecha, cliente, total, pagado y saldo pendiente. La de pagos presenta fecha, medio, cuotas, base aplicada a la venta, recargo y total cobrado. También configura el campo como Importe a cancelar sin recargo y prepara las cuotas.

### Buscar: medios, filtro de pendientes y ventas

**frmPagos_Load** llama a **Buscar**. Este método carga los medios de pago y los ordena colocando efectivo primero. Después consulta **negocio.Buscar** con el texto y la opción de pendientes, asigna las ventas a la grilla y actualiza la cantidad de resultados. Durante la carga mantiene **cargando** en verdadero para evitar procesar selecciones intermedias. En el **finally** vuelve a falso y llama a **SeleccionarVenta**.

### SeleccionarVenta: pagos anteriores e importe sugerido

**SeleccionarVenta** limpia la lista de pagos anterior y el importe, obtiene la venta actual y habilita la garantía si existe. Después consulta los pagos mediante **Listar**, los muestra y propone como importe a cancelar el saldo pendiente, limitado por el máximo del control. Finalmente ejecuta **MostrarCobro**.

### MostrarCobro: base, recargo, total y saldo futuro

En esta pantalla el importe ingresado es la base que se desea cancelar de la deuda. **MostrarCobro** llama a **CN_Pago.Preparar** con venta, medio, cuotas e importe. Muestra base, porcentaje, recargo, total a cobrar y saldo que quedará después. Por ejemplo, cancelar una base de 100 con un recargo del 10 % implica cobrar 110 y reducir la deuda en 100.

### Cambiar medio o cuotas y habilitar Registrar

Cambiar medio, cuotas o importe vuelve a cotizar; no guarda un pago. **ActualizarCuotas** muestra y habilita cuotas solamente para Crédito y vuelve a la primera opción para los demás medios. El botón Registrar queda habilitado cuando la selección es válida, el importe no supera el saldo y la sesión tiene el permiso **RegistrarPagos**.

### Registrar: confirmar el cobro y actualizar la búsqueda

**Registrar** conserva el número de venta y llama a **negocio.Registrar** con medio, cuotas, base y cuenta de destino. Si se guarda, limpia la cuenta y vuelve a buscar. Conserva el número antes de recargar porque una venta totalmente pagada puede desaparecer del filtro de pendientes. Después ofrece emitir una garantía actualizada.

### Eventos de búsqueda, selección y manejo de errores

El botón Buscar y Enter ejecutan **Buscar**. Cambiar de fila ejecuta **SeleccionarVenta**. Los errores de consulta o registro se muestran mediante el método **Error**.

### btnGarantia_Click y EmitirGarantia

**btnGarantia_Click** usa el identificador de la venta seleccionada. **EmitirGarantia** vuelve a consultar la información de la venta y todos sus pagos mediante **CN_Comprobante.Obtener**, genera los documentos y abre directamente el HTML de garantía. No crea otra venta ni repite el cobro.

### Pagos incluidos y errores al abrir el documento

La garantía presenta los pagos con sus medios y lo cobrado con recargo. El detalle de cuotas corresponde al pago con crédito. Si falla la generación o apertura después de registrar un pago, el aviso indica que no se repita el pago y que se intente emitir nuevamente. En este recorrido no se abre **frmComprobantes**: se abre directamente el archivo.

### CD_Pago: cambio de estado y conservación de la base

Al registrar un cobro posterior, **CD_Pago** obtiene el saldo y verifica que la cotización siga siendo válida. Si **ImporteBase** coincide con el saldo, elige **Pagada**; de lo contrario elige **Parcial**. Obtiene el identificador de ese estado, inserta el pago con base y monto final y actualiza Venta sumando solamente el nuevo recargo al total. No vuelve a sumar el importe base al precio de la venta, porque esa base ya formaba parte de la deuda.

El número de cuotas se conserva internamente en el pago. En la presentación de los documentos se menciona cuando corresponde al medio Crédito; un pago en efectivo no debe describirse como una compra en una cuota. La caja utiliza los importes base para sus sumas de cobros, mientras que la garantía informa lo pagado con recargo y el medio utilizado. Son consultas con propósitos diferentes sobre los mismos pagos.

## Medios de pago: frmMediosPagos.cs

### Constructor, servicio e identificador seleccionado

**frmMediosPagos** modifica los porcentajes de recargo de los medios existentes. Mantiene un **CN_MedioPago** y el identificador del medio seleccionado. El constructor deja el nombre como solo lectura y deshabilita Guardar y los campos de cuotas hasta que corresponda editarlos

### CargarMedios: consulta y columnas

**frmMediosPagos_Load** llama a **CargarMedios**, que consulta los medios y asigna la lista al **DataSource**. Oculta identificador, activo y TipoCaja, coloca títulos a las demás columnas y limpia la selección. La pantalla muestra el nombre del medio y sus recargos; no incluye un selector Tipo en caja.

### CellClick y ConfigurarCuotas: completar los porcentajes

**dgvMediosPagos_CellClick** obtiene el objeto **MedioPago** de la fila, conserva su identificador y su tipo en variables internas y completa el nombre y los porcentajes visibles. Si el tipo es Crédito, habilita los recargos de dos, tres, seis, nueve y doce cuotas. Fuera de crédito, los controles de cuotas permanecen deshabilitados y se presentan con cero.

### ObtenerMedioDesdeFormulario, Guardar y LimpiarSeleccion

**ObtenerMedioDesdeFormulario** construye el objeto con esos valores. **btnGuardar_Click** exige una selección, incorpora el identificador y llama a **CN_MedioPago.Modificar**. Después confirma y recarga la lista. **LimpiarSeleccion** borra los campos, restablece el identificador a cero y deshabilita Guardar. Esta pantalla no abre otro formulario ni registra cobros: cambia la configuración utilizada al calcularlos.

### tipoCajaSeleccionado: clasificación interna sin control visible

La pantalla conserva el tipo del medio elegido en **tipoCajaSeleccionado**. **dgvMediosPagos_CellClick** lo obtiene del objeto **MedioPago** y **ObtenerMedioDesdeFormulario** lo incorpora al objeto que se envía a negocio. Esto permite validar los recargos del medio sin mostrar una etiqueta ni un ComboBox Tipo en caja. La columna **TipoCaja** también se oculta en el listado. **LimpiarSeleccion** restablece esa variable a un texto vacío.

Eliminar esos controles no elimina el dato interno **TipoCaja**: sigue utilizándose para reconocer Crédito, habilitar cuotas y clasificar cobros. En esta pantalla el usuario modifica porcentajes, no la clasificación de los medios predefinidos.

## Entregas: frmEntregas.cs

### Servicio y propiedad Producto

**frmEntregas** utiliza **CN_Entrega** y separa productos por preparar de entregas ya preparadas. La propiedad **Producto** obtiene el **ProductoPorEntregar** de la fila actual de la primera grilla.

### Buscar: productos pendientes y entregas preparadas

**frmEntregas_Load** ejecuta **Buscar**. Este método consulta **Pendientes** con el texto escrito y **Listar** con ese texto y la casilla de pendientes. Asigna los resultados a las dos grillas, muestra sus cantidades y actualiza los controles de selección y confirmación. Si falla, limpia ambas listas y muestra el error.

### Seleccionar y ActualizarConfirmacion: campos y permisos

**Seleccionar** muestra venta, cliente y producto, restablece cantidad a uno y toma la dirección disponible. Si la modalidad comienza con Retiro, muestra Retiro en local y deja la dirección como solo lectura. El botón Preparar exige una selección y el permiso **GestionarEntregas**. **ActualizarConfirmacion** habilita Confirmar solamente para una entrega con estado Pendiente y una sesión con ese permiso.

### Preparar: validar y apartar las unidades

**Preparar** pasa producto, cantidad, dirección y horario a **CN_Entrega.Preparar**. El negocio comprueba permiso, cantidad pendiente y datos requeridos antes de solicitar la operación a datos. Al terminar se vuelve a consultar y se informa que la entrega fue preparada y el stock reservado.

### Confirmar: registrar que la entrega se realizó

**Confirmar** obtiene el **EntregaRegistrada** seleccionado y pasa su identificador de envío a **CN_Entrega.Confirmar**. Si termina correctamente, recarga las grillas y muestra la confirmación. Preparar y confirmar son pasos distintos; seleccionar una fila no ejecuta ninguno de ellos.

### Eventos de búsqueda y selección

Buscar y Enter vuelven a consultar. Los eventos de selección de cada grilla actualizan sus controles. Las operaciones se realizan en esta misma pantalla; no se abre un formulario auxiliar propio para prepararlas o confirmarlas.

### CD_Venta.Buscar: cálculo del avance de entrega

La consulta suma las unidades vendidas por variante y obtiene las unidades preparadas desde **Remito** y **Detalle_Remito**. Para reconocer las entregadas comprueba que exista un Envio con estado **Entregado**. Excluye las preparaciones con envío cancelado y limita cada cantidad contabilizada a la cantidad vendida de esa variante. De esta manera, las relaciones con varias filas no deben multiplicar artificialmente los productos vendidos.

La expresión **CASE** evalúa las condiciones en orden. Si la venta está anulada o cancelada devuelve **Cancelada**. Si todas las unidades están entregadas devuelve **Entregada**; si solo hay algunas entregadas devuelve **Entrega parcial**. Cuando no hay entregas confirmadas, devuelve **Preparada** si todo está preparado, **Preparación parcial** si solo hay una parte y **Pendiente** si todavía no hay preparación. CASE cumple en SQL una función de decisión comparable a los bloques if y else de C#.

El estado calculado describe la venta completa. El estado de un envío o de un detalle individual pertenece a otro nivel. Una venta puede tener una entrega confirmada y otras cantidades todavía pendientes; por eso no alcanza con encontrar una sola fila Entregado para considerar entregada toda la venta. Este cálculo tampoco determina si la venta está pagada.

## Informes de ventas: frmInformesVentas.cs

### Constructor, servicio y filtros iniciales

**frmInformesVentas** utiliza **CN_InformeVentas** para consultar ventas mediante un objeto **FiltroInformeVentas**. El constructor prepara los ComboBox con autocompletado nativo, configura longitudes, establece inicialmente desde un mes antes hasta hoy y configura una grilla de solo lectura.

### CargarOpciones: opciones de los desplegables

**frmInformesVentas_Load** llama a **CargarOpciones** y después a **Consultar**. **CargarOpciones** recibe un **Dictionary<string, List<string>>** y distribuye sus listas entre producto, categoría, marca, cliente, envío y ubicación. Agrega una opción vacía para permitir dejar esos criterios sin completar.

### Seleccionar o escribir producto: categoría y marca

Al confirmar un producto, **producto_SelectionChangeCommitted** consulta su categoría y marca. Si obtiene una combinación, completa ambos controles y los deshabilita para esa selección. El resultado es una tupla anulable: **HasValue** indica si se encontró la información. Si se vuelve a escribir en el producto, **producto_TextChanged** limpia esos datos y habilita nuevamente sus controles.

### Consultar: fechas y límites de importe

**Consultar** comprueba que Desde no sea posterior a Hasta. Los importes mínimo y máximo se convierten en **decimal?**: un valor cero significa que no se aplica ese límite y se envía **null**. Si ambos existen, exige que el mínimo no supere al máximo.

### FiltroInformeVentas y ConfigurarColumnas

Después construye un **FiltroInformeVentas** con fechas y criterios y lo pasa a **negocio.Buscar**. A Hasta le suma un día para incluir el día seleccionado completo mediante un límite superior exclusivo. El resultado se asigna al **DataSource**; **ConfigurarColumnas** aplica títulos, formatos de fecha y moneda y anchos de cliente y productos.

### Grilla de solo lectura y botón Consultar

La grilla está preparada para consultar y leer texto de varias líneas, no para modificar las ventas. El botón Buscar repite la consulta. El formulario no abre otra ventana propia ni registra operaciones comerciales.

## Alertas: frmAlertas.cs

### Constructor y servicios utilizados

**frmAlertas** utiliza **CN_InformeVentas** para pagos pendientes y **CN_Entrega** para faltantes de mercadería. El constructor prepara los controles y evita el ajuste automático de altura de filas de la primera grilla.

### Load, CargarAsync, await y Task.Run

El evento **frmAlertas_Load** es **async** y espera **CargarAsync**. Este método ejecuta las consultas mediante **await Task.Run**, para que la consulta síncrona de datos no bloquee la interfaz mientras se espera el resultado. Una vez completada cada consulta, continúa actualizando los controles de la pantalla.

### Alertas de pago: primer cobro y mes calendario

Primero obtiene **List<AlertaPagoPendiente>** y muestra si existen ventas que requieren seguimiento. La consulta actual considera ventas con saldo cuyo primer pago alcanzó un mes de antigüedad. Se utiliza un mes calendario con **DATEADD(MONTH, 1, ...)**, no un número fijo de treinta días.

### Alertas de stock: pendientes y unidades faltantes

Después obtiene **List<AlertaStockPendiente>**. El negocio calcula faltantes como pendientes menos disponible, con mínimo cero, y conserva los casos con faltantes positivos. El formulario muestra producto y variante, pendientes de preparar, disponible y unidades faltantes, ocultando el identificador técnico de variante.

### Columnas y errores según la etapa de consulta

Finalmente configura las columnas de pagos: venta, cliente, productos, primer pago, total, pagado, saldo y días vencida. La variable **etapa** permite que el **catch** identifique si falló la consulta de pagos o la de stock y muestre el mensaje correspondiente.

### CellContentClick vacío y momento de actualización

Estas alertas se consultan al cargar la pantalla. El manejador **grilla_CellContentClick** está vacío: pulsar una celda no abre otra ventana ni resuelve automáticamente el pendiente.

## Historial de actividades: frmHistorial.cs

### Constructor, servicio y evento Load

**frmHistorial** consulta actividades registradas mediante **CN_LogActividad**. El constructor prepara los controles y ajusta la altura del panel de filtros. En **frmHistorial_Load** se ejecuta **ConfigurarHistorial**.

### ConfigurarHistorial: últimas diez actividades y columnas

**ConfigurarHistorial** carga inicialmente las diez actividades más recientes y después configura las columnas. Oculta los identificadores internos y presenta usuario, acción, entidad, detalle y fecha. La columna Detalle ocupa el espacio disponible y la fecha se muestra con día, mes, año, hora y minutos.

### CargarActividades: filtros y parámetros

**CargarActividades** recibe desde, hasta y límite. Si se completó un número de remito, comprueba que sea un entero positivo; si no, mantiene ese filtro como **null**. Envía esos criterios junto con acción y usuario a **ObtenerFiltrado** y asigna el resultado a la grilla.

### Filtrar, Últimas 10 y tecla Enter

**btnFiltrar_Click** comprueba el orden de las fechas, suma un día al límite Hasta para incluirlo completo y solicita como máximo cien actividades. **btnUltimas_Click** vuelve a pedir diez actividades sin restringir el rango de fechas; los textos de remito, acción y usuario se siguen pasando porque ese método no los limpia. **Filtro_KeyDown** ejecuta Filtrar cuando se presiona Enter.

### Consultar el motivo de anulación de caja

Cuando se anula un movimiento de caja, la acción es **ANULAR_MOVIMIENTO**, la entidad es Caja y el detalle contiene el motivo ingresado. La consulta también muestra el usuario y la fecha. La fila original de caja queda marcada como anulada; esta pantalla presenta el registro de auditoría de la operación.

### Alcance de la consulta del historial

El historial es una consulta de registros existentes. No abre otra ventana propia ni permite modificar esas actividades desde la grilla.

### LogActividad: datos de cada actividad

La clase **LogActividad** representa una fila del historial. **IdLog** identifica esa actividad; **IdUsuario** identifica a quien la realizó y **NombreUsuario** contiene el nombre obtenido al consultar. **Accion** contiene el nombre de la operación, **Entidad** indica qué tipo de registro se afectó y **IdEntidad** identifica ese registro cuando se conoce. **Detalle** conserva la explicación particular del cambio y **Fecha** indica cuándo se registró.

**IdLog** e **IdEntidad** tienen funciones distintas. El primero identifica la fila del historial; el segundo puede identificar una venta, una variante, un cliente, un usuario o un movimiento de caja. **IdEntidad** admite null porque algunas llamadas registran una descripción sin proporcionar ese identificador. La acción es texto, no un estado que vaya cambiando de Pendiente a Completado.

### CN_LogActividad.Registrar y CD_LogActividad.Registrar

En las operaciones de Stock, el formulario llama a **CN_LogActividad.Registrar** con acción, entidad, identificador opcional y detalle. Negocio obtiene **SesionActual.Usuario.idUsuario** y llama a **CD_LogActividad.Registrar**. Datos construye un **SqlCommand** con un **INSERT INTO dbo.LogActividad**, asigna parámetros tipados, abre una conexión y ejecuta **ExecuteNonQuery**. Un identificador ausente o un detalle vacío se envían como **DBNull.Value**, que representa NULL en SQL Server. La fecha se completa con el valor predeterminado definido en la tabla.

Esta llamada abre su propia conexión. En esos casos, el guardado de Stock y el registro posterior del historial no son una única transacción compartida. Si se guarda el producto pero falla el historial posterior, un error del formulario no demuestra por sí solo que el producto no exista: hay que consultar el resultado antes de repetirlo.

### Acciones escritas dentro de la transacción principal

Otros registros se escriben desde la capa de datos usando la conexión y la transacción de la operación. **CD_RegistroVenta.Guardar** inserta las actividades de la venta; **CD_Pago** y **CD_Entrega** utilizan el método compartido **GestionVentaSql.RegistrarActividad**, que establece la entidad Venta y recibe su identificador. **CD_Cliente**, **CD_Usuario** y **CD_Caja** también contienen escrituras específicas de historial.

Cuando esas escrituras forman parte de la transacción que guarda la operación, **Commit** confirma tanto los datos como sus actividades y **Rollback** deshace ambos. No existe un mecanismo general que registre automáticamente cualquier método ejecutado: el historial se genera únicamente donde el código incluye una llamada o un INSERT para ello.

### REGISTRAR_VENTA y APLICAR_DESCUENTO

**CD_RegistroVenta.Guardar** inserta **REGISTRAR_VENTA** con entidad Venta e identificador **idVenta**. El detalle contiene número de venta o remito, cliente, total y descuento. Utiliza la fecha del encabezado. Si el descuento es mayor que cero, también inserta **APLICAR_DESCUENTO** para la misma venta, con descuento y total final. Una venta con descuento puede producir ambas filas; no son dos ventas diferentes. Los pagos iniciales se guardan como parte de esta operación y no generan aquí una actividad REGISTRAR_PAGO por cada medio.

### REGISTRAR_PAGO

**CD_Pago** registra **REGISTRAR_PAGO** después de insertar un cobro posterior y actualizar la venta. **GestionVentaSql.RegistrarActividad** relaciona la actividad con esa venta y guarda en Detalle el importe base y el recargo. La base cancela deuda; el recargo aumenta tanto el total de la venta como el importe final cobrado. Esa actividad permite investigar el cobro, pero no reemplaza las filas de Pago que se utilizan para calcular importes y emitir la garantía.

### PREPARAR_ENTREGA y CONFIRMAR_ENTREGA

**CD_Entrega** escribe **PREPARAR_ENTREGA** cuando prepara unidades de una variante. El detalle incluye número de venta, preparación interna, variante y cantidad. La acción indica que se prepararon unidades, no que se entregaron todas las de la venta. **CONFIRMAR_ENTREGA** se registra al confirmar una entrega preparada y también en **RegistrarRetiroInmediato**. En este último caso el detalle aclara que se trató de un retiro inmediato en el local.

### CREAR_PRODUCTO, MODIFICAR_PRODUCTO y DESACTIVAR_VARIANTE

**frmStock** llama al servicio de historial después de las operaciones correspondientes. **CREAR_PRODUCTO** usa entidad Producto, identificador null y el nombre creado en Detalle. **MODIFICAR_PRODUCTO** usa entidad VarianteProducto, el identificador de la variante modificada y un detalle con producto, precio nuevo y costo nuevo. Aunque su nombre diga producto, esa llamada identifica la variante editada. **DESACTIVAR_VARIANTE** también utiliza VarianteProducto y su identificador. La desactivación conserva los antecedentes; la actividad no es una instrucción de borrado.

### REGISTRAR_INGRESO_STOCK

Después de registrar el ingreso de mercadería, **frmStock** escribe **REGISTRAR_INGRESO_STOCK**, entidad IngresoStock e identificador null. El detalle actual contiene proveedor, Doc.proveedor y la lista de productos con color, condición y cantidad ingresada. El formulario prepara esa lista antes de limpiar el ingreso. Las actividades antiguas pueden conservar únicamente la cantidad de variantes, porque su texto no se reconstruye al consultarlas. El documento del proveedor no es el número de venta que filtra Historial con el campo Remito. El armado de este texto se explica en el apartado productosIngresados y detalle del historial.

### BAJA_CLIENTE, ACTIVAR_CLIENTE y RESTAURAR_CLIENTE

**CD_Cliente.CambiarEstadoCliente** recibe el nombre de la acción y actualiza las marcas **activo** y **eliminado**. **BAJA_CLIENTE** establece eliminado verdadero y activo falso; antes comprueba las condiciones de deuda y entrega. **ACTIVAR_CLIENTE** habilita un cliente inactivo no eliminado. **RESTAURAR_CLIENTE** recupera un cliente eliminado. La actividad usa entidad Cliente e identificador del cliente, junto con el usuario responsable y un detalle que indica el cambio. La actualización y su actividad pertenecen a la misma transacción.

### DAR_DE_BAJA_USUARIO y DESBLOQUEAR_USUARIO

**CD_Usuario** inserta **DAR_DE_BAJA_USUARIO** cuando desactiva una cuenta y **DESBLOQUEAR_USUARIO** cuando un administrador restablece su acceso. La entidad es Usuario. **id_usuario** del historial corresponde al administrador responsable, mientras que **id_entidad** corresponde a la cuenta afectada. Es importante distinguir ambos números: quien ejecuta la acción puede ser una persona distinta de aquella cuya cuenta se modificó.

### ANULAR_MOVIMIENTO y conservación del motivo

**CD_Caja** marca el movimiento como anulado y conserva el motivo. Además inserta **ANULAR_MOVIMIENTO**, con entidad Caja e identificador del movimiento; el detalle de la actividad recibe el motivo indicado por el administrador. Las consultas de caja excluyen los anulados de los totales vigentes. El movimiento original y la actividad son registros diferentes: uno conserva la operación económica y el otro explica quién la anuló y por qué.

### ObtenerFiltrado: SELECT, JOIN y condiciones opcionales

**CN_LogActividad.ObtenerFiltrado** pasa los criterios a **CD_LogActividad.ObtenerFiltrado**. La consulta utiliza **TOP (@Limite)** para limitar filas, un **INNER JOIN** con Usuario para obtener el nombre y **ORDER BY fecha DESC, id_log DESC** para mostrar primero las más recientes. Compara **fecha >= @Desde** y **fecha < @Hasta**; por eso el botón Filtrar envía como Hasta el comienzo del día siguiente al elegido.

Los filtros Acción y Usuario utilizan **CHARINDEX**: aceptan una parte del texto, no exigen el nombre completo. Un texto vacío omite ese filtro. Si Remito es null, no restringe por número; si se completa, exige entidad Venta e **id_entidad** igual al número ingresado. Por lo tanto, ese filtro no busca el documento del proveedor de un ingreso ni el identificador de un movimiento de caja.

**SqlDataReader.Read** recorre el resultado. Por cada fila, el método crea un objeto **LogActividad**, recupera los valores de sus columnas y comprueba **IsDBNull** en los datos opcionales. Finalmente devuelve **List<LogActividad>** para asignarla al **DataSource** del formulario. Esta consulta no actualiza las operaciones que presenta.

### Acciones que no deben suponerse registradas

Esta versión no escribe una fila de historial por cada clic, inicio de sesión, creación de cliente, creación de usuario, cambio de recargo o cierre de caja. Los cierres se conservan en sus propias tablas y documentos. **CIERRE AUTOMÁTICO** es una identificación usada en el cierre, no una acción de esta lista. A las quince acciones del circuito original se suman **MODIFICAR_OPCION_STOCK**, **UNIFICAR_OPCION_STOCK** y **ELIMINAR_OPCION_STOCK**, explicadas dentro del apartado de frmOpcionesStock. El historial tampoco restaura información ni revierte operaciones al seleccionarlas.

## Usuarios: frmUsuarios.cs

### Constructor, CN_Usuario e idSeleccionado

**frmUsuarios** utiliza **CN_Usuario** y mantiene **idSeleccionado** para distinguir creación de modificación. El constructor prepara los controles, selecciona inicialmente el segundo rol del ComboBox y limita el nombre a veinte caracteres.

### Cargar: usuarios activos y opción de creación

**frmUsuarios_Load** ejecuta **Cargar**. Este método obtiene los usuarios, conserva los activos y agrega al principio un objeto con el texto Crear un usuario nuevo y sin identificador. Asigna la lista al selector y ejecuta **Seleccionar** para preparar los campos.

### Seleccionar: nueva cuenta o modificación

**Seleccionar** obtiene el usuario actual y guarda su identificador. Cuando vale cero, limpia el nombre y prepara la pantalla para crear; cuando corresponde a un registro, muestra sus datos y prepara la modificación. Siempre limpia el campo de contraseña: no muestra la contraseña almacenada. Para un usuario existente avisa que dejarla vacía conserva la actual.

### Rol, estado de cuenta y bloqueo

El rol se relaciona con la posición del ComboBox: al guardar se envía **SelectedIndex + 1** y al cargar se selecciona **IdRolUsuario - 1**. También se muestran estado y bloqueo, se habilita desbloquear cuando la cuenta está bloqueada y se habilita dar de baja para un usuario existente activo.

### Guardar: crear o modificar y conservar la contraseña

**btnGuardar_Click** llama a **ModificarUsuario** si hay identificador o a **CrearUsuario** si no lo hay. La creación exige una contraseña con al menos cinco caracteres, una mayúscula y un número; negocio genera su hash antes de enviarla a datos. La unicidad del nombre considera también usuarios dados de baja. Si se guarda, limpia la contraseña, informa el resultado y recarga el selector.

### Desbloquear: confirmación y restablecimiento de intentos

**btnDesbloquear_Click** solicita confirmación y llama a **DesbloquearUsuario**, que requiere el permiso de gestión de usuarios. Se restablecen los intentos para permitir un nuevo ingreso y se recarga la pantalla.

### Dar de baja: confirmación y protección del administrador principal

**btnDarDeBaja_Click** confirma que se perderá el acceso pero se conservarán los registros históricos y llama a **DarDeBajaUsuario**. El negocio exige permiso y protege la cuenta administradora principal con identificador uno. La baja no elimina sus ventas ni su historial. Como el selector carga solamente activos, después de la baja ese usuario deja de aparecer allí.

### Nuevo usuario: volver a la creación

**nuevo_Click** selecciona el primer elemento para volver a la creación. No se abre otra ventana propia: se reutilizan los mismos campos para crear o modificar.

## Conceptos utilizados en las pantallas

### Constructor, InitializeComponent y Load

El constructor prepara una instancia del formulario. **InitializeComponent** crea y configura los controles definidos en su archivo Designer y conecta los eventos declarados allí. El evento **Load** se produce cuando se carga la pantalla y suele utilizarse para consultar los datos iniciales. Crear el formulario no equivale a guardar una operación comercial.

### Eventos, sender y EventArgs

Un evento como **Click**, **SelectionChanged** o **ValueChanged** llama al método conectado cuando ocurre esa acción. **sender** identifica el objeto que produjo el evento y **e** contiene sus datos. Por ejemplo, en un evento de grilla, **e.RowIndex** y **e.ColumnIndex** permiten identificar la celda. Un manejador con el cuerpo vacío no realiza ninguna operación, aunque su nombre sugiera una función.

### DataSource, DataBoundItem y filas temporales

Cuando se asigna una lista al **DataSource**, la grilla muestra sus objetos. **DataBoundItem** permite recuperar el objeto asociado a una fila. En las grillas que se completan con **Rows.Add**, el formulario agrega los valores de las celdas manualmente. Los cambios en listas temporales no se guardan automáticamente: hay que identificar el método que llama a negocio para registrar cada operación.

### Clases de negocio y clases de acceso a datos

Las clases **CN_** corresponden a negocio y las clases **CD_** al acceso a datos. No todos los métodos de negocio tienen las mismas validaciones ni todas las operaciones comprueban permisos de la misma manera; las explicaciones siguientes describen las llamadas que existen en el código.

### Show, ShowDialog y liberación con using

**ShowDialog(this)** abre una ventana modal, por lo que el formulario que la llamó espera su resultado. **Show()** abre una ventana sin esa espera modal. **MessageBox.Show** muestra un mensaje estándar, no un formulario propio del proyecto. Cuando se declara un diálogo mediante **using**, se liberan sus recursos al terminar el bloque o método que lo contiene.

## Eventos vacíos y recorrido entre capas

### Cómo reconocer un evento que no realiza acciones

En varios archivos quedaron métodos de eventos con el cuerpo vacío, especialmente clics de etiquetas, cambios de controles y eventos Paint. No agregan reglas de negocio ni funciones ocultas. Por ejemplo, el clic de la grilla de Alertas está vacío y varios cambios de texto de Stock también. Algunos métodos solo llaman a otro método; la operación real se encuentra en el llamado.

### Cómo seguir una operación hasta la base de datos

Esta guía describe el código revisado, no una nueva funcionalidad ni una prueba integral contra datos comerciales. Para seguir una operación hasta su guardado se continúa desde el método del formulario al **CN_** correspondiente y después al **CD_**. Los Designer sirven para revisar controles y conexiones de eventos; no sustituyen la lectura de esos métodos.

