# Base de datos y scripts de Prana Electrónica

Prácticas Profesionalizantes III · Tecnicatura Superior en Análisis de Sistemas

Autor: Valentín Morello · ISFT 189 · Año 2026

Revisión documental: 11 de noviembre de 2026. Documento técnico para acompañar el proyecto y su defensa.

## 1. Finalidad y alcance

La base de datos conserva clientes, catálogo, variantes, existencias, ingresos, ventas, pagos, entregas, usuarios e historial. Se utiliza SQL Server Express LocalDB y un modelo relacional: cada entidad se identifica mediante una clave y las relaciones vinculan los registros que forman una misma operación.

El documento describe los scripts y el código de la copia del proyecto. No presenta como verificado el contenido de una instalación particular. Una base utilizada durante pruebas puede contener información distinta de la que recibe una instalación nueva.

## 2. Motor, instancia, base y archivos

LocalDB es el motor que permite ejecutar las consultas. La instancia utilizada es (localdb)\MSSQLLocalDB y la base se llama PranaElectronica. La conexión usa autenticación integrada de Windows: las credenciales del acceso a Prana son cuentas de la aplicación, no usuarios independientes de SQL Server.

Cuando debe crear la base, el inicializador propone los archivos PranaElectronica.mdf y PranaElectronica_log.ldf en %LOCALAPPDATA%\PranaElectronica\Data del usuario que ejecuta Prana. El MDF contiene los datos y las estructuras; el LDF es el registro de transacciones utilizado por SQL Server para su recuperación. El LDF no debe confundirse con LogActividad ni eliminarse manualmente.

La base pertenece al entorno local del usuario. Cambiar de computadora o de cuenta de Windows no traslada automáticamente sus datos. El ejecutable del instalador no contiene una copia de las operaciones comerciales del equipo que lo generó.

## 3. Cuándo se prepara la base

Program.Main inicializa Windows Forms y llama a InicializadorBaseDatos.AsegurarBaseDeDatos antes de mostrar el acceso. El instalador distribuye los archivos SQL, pero no ejecuta por sí mismo las instrucciones de creación de la base.

1. Se crea, si falta, la carpeta de datos del usuario.
2. Se abre una conexión con master y se consulta DB_ID para saber si existe PranaElectronica.
3. Si no existe, se ejecuta CREATE DATABASE con las rutas MDF y LDF previstas.
4. Se abre una conexión con PranaElectronica.
5. Se inicia la preparación transaccional del esquema y se obtiene un bloqueo de aplicación.
6. Si no hay tablas de usuario, se ejecuta el script de creación.
7. Se ejecutan el script de actualización y el de caja y comprobantes, en ese orden.
8. Se confirma la transacción y se abre la pantalla de acceso.

Si falla la preparación se muestra el error y no se abre el acceso. La comprobación de base vacía cuenta las tablas de usuario: si existe una tabla ajena en una base incompleta, no se toma como vacía. El actualizador exige las tablas esenciales y puede rechazar esa estructura parcial.

## 4. Scripts ejecutados automáticamente y orden

- CrearEstructuraPrana_LocalDB.sql: se ejecuta solamente cuando la base no tiene tablas de usuario. Construye tablas, relaciones, restricciones y configuración inicial.
- ActualizarEstructuraPrana_LocalDB.sql: se ejecuta después de la creación y en los siguientes inicios. Adapta estructuras de versiones anteriores.
- CajaYComprobantes.sql: se ejecuta al final. Incorpora caja, referencias de cobro, medios fijos y compatibilidad de instalaciones anteriores.

DiagnosticarTablasProducto.sql es una consulta de diagnóstico: no forma parte de esa secuencia. Puede distribuirse por el comodín de archivos SQL del instalador, pero el inicializador únicamente ejecuta los tres nombres indicados. Tener un archivo .sql en la carpeta no significa que Prana lo ejecute.

## 5. Qué crea el script inicial

El script define tablas con tipos de datos, claves primarias, columnas IDENTITY, relaciones y comprobaciones. La estructura inicial y los agregados compatibles del propio script abarcan las siguientes entidades; CajaYComprobantes agrega las dos de caja.

- Rol y Usuario: perfiles y cuentas; contraseña protegida mediante hash, actividad, intentos fallidos y bloqueo temporal.
- Cliente y Localidad: identificación, documento, teléfono, dirección, ciudad, código postal y estado del cliente.
- Categoria y Marca: clasificación de los productos.
- Producto: nombre general, modelo, descripción, categoría y marca.
- Variante_Producto: color, condición, precio, costo, existencias y actividad; pertenece a un Producto.
- Proveedor, Ingreso_Stock y Detalle_Ingreso: proveedor y cabecera de recepción, con variantes, cantidades y costos de cada renglón.
- Venta y Detalle_Venta: cabecera de la operación y artículos vendidos, con cantidad y precio aplicado.
- Estado_Venta y Estado_Variante_Producto: estados de la venta y de sus artículos en el circuito de entrega.
- Modalidad_Entrega y Venta_Entrega: modalidad y datos acordados de destino, fecha, horario, logística, costo y observaciones.
- Pago y Medio_Pago: cobros de cada venta, medio, cuotas, importes, fecha y referencia.
- Remito, Detalle_Remito, Envio y Estado_Envio: preparación y confirmación de unidades para entregar.
- LogActividad: registro de acciones, usuario, entidad afectada, detalle y fecha.
- Recibo: tabla del esquema conservada; su existencia no significa que haya un circuito completo de emisión de recibos independientes en las pantallas.
- Caja_Movimiento: gastos e ingresos manuales y sus anulaciones.
- Caja_Cierre: fecha única de cierre y resumen guardado como JSON.

Producto es la tabla del catálogo utilizada por la aplicación. Una tabla adicional llamada PRODUCTOS no se crea con los scripts actuales y no debe confundirse con aquella. Deben revisarse sus dependencias antes de eliminar una tabla ajena en una instalación existente.

## 6. Datos exactos de una instalación nueva

En una base nueva, después de completar los tres scripts, existe configuración básica, pero no operaciones de demostración.

- Rol 1: Administrador.
- Rol 2: Vendedor.
- Cuenta admin, activa, asociada al rol Administrador. La contraseña se almacena como hash BCrypt. Las credenciales iniciales se entregan por un canal separado; este documento no reproduce contraseñas ni hashes.
- Cuatro medios activos: Efectivo, Transferencia, Débito y Crédito.
- En los medios creados inicialmente, el recargo general y los recargos de 2, 3, 6, 9 y 12 cuotas se cargan en cero.
- No se cargan clientes, productos, variantes, marcas, categorías, proveedores, localidades, ingresos, ventas, pagos, entregas, cierres ni movimientos manuales de ejemplo.
- Los contadores de las tablas no constituyen filas: preparar el próximo número de venta no crea una venta ficticia.

La numeración de ventas se prepara para comenzar en 2500 cuando corresponde. El script distingue una identidad nunca utilizada de una ya utilizada y no retrocede una numeración superior ni renumera las ventas existentes. No se debe prometer el número 2500 en una base que ya tiene operaciones.

## 7. Datos que aparecen al utilizar el sistema

Los estados y modalidades no requieren un catálogo de ventas ficticias al comenzar: el código obtiene el registro por su nombre o lo crea cuando la operación lo necesita. En ventas se utilizan, entre otros, Pagada o Parcial y los estados de detalle Pendiente o Entregado. En entregas se utilizan Pendiente y Entregado.

Marcas, categorías, proveedores y localidades se incorporan al cargar los datos correspondientes según las opciones de cada pantalla. Un alta de producto no implica que todos esos catálogos estuvieran precargados en la instalación.

La cuenta administradora se incluye para posibilitar el primer acceso. Las cuentas adicionales se crean desde Usuarios y los recargos comerciales deben configurarse antes de cobrar con porcentajes distintos de cero.

## 8. Qué cambia ActualizarEstructuraPrana_LocalDB.sql

Comprueba primero que existan Usuario, Venta, Medio_Pago, Rol y Cliente. Si falta estructura esencial, informa que la base está incompleta. No intenta reconstruir una base parcial como si fuera una instalación nueva.

Entre sus cambios se encuentran actividad y eliminación lógica del cliente, recargos por cuotas, bloqueo de cuentas, precisión del descuento, LogActividad y Venta_Entrega. También adapta el tipo de envío, códigos postales como texto, condiciones PRIMERA y OUTLET y el tipo de documento DNI o CUIT.

Convierte la condición antigua Seña para reserva a Pago parcial. Al adaptar costo a decimal(10,2), comprueba que los importes existentes entren en el rango y rechaza la conversión si exceden su capacidad, en lugar de recortarlos silenciosamente.

Retira estructuras sin uso: Tipo_Envio después de su adaptación, Formato_Documento y sus referencias, Devolucion y Tipo_Resolucion. Elimina también la columna generar_remito y los correos sin uso de Usuario, Cliente y Proveedor, retirando la restricción o índice de correo que impediría hacerlo.

Para los nombres de usuario crea UX_Usuario_Nombre si no existe y no hay duplicados anteriores. Si hay nombres duplicados antiguos, permite iniciar sin ese índice; las operaciones actuales de crear o modificar usuario comprueban igualmente que el nombre no esté ocupado, incluyendo cuentas dadas de baja.

Por estas eliminaciones, el comentario histórico que describe el script como sin borrados no expresa todo su contenido actual. Debe revisarse el script ejecutable y respaldar antes de actualizar.

## 9. Qué cambia CajaYComprobantes.sql

Prepara la numeración de Venta; agrega la clasificación del medio en caja y conserva esa clasificación en cada Pago. Incorpora cuenta_destino, Caja_Movimiento y Caja_Cierre. Añade código postal al cliente cuando falta y puede recuperarlo desde la localidad para instalaciones anteriores.

Elimina Venta_Comprobante cuando existe: los documentos actuales se construyen consultando venta, cliente, detalle y pagos; no dependen de esa copia JSON antigua. Esto es diferente del JSON de Caja_Cierre, que sí se conserva para los cierres.

Recupera eventos faltantes del historial a partir de ventas anteriores y descuentos, usando NOT EXISTS para no repetirlos en cada inicio. Esas inserciones reconstruyen historial de operaciones existentes; no crean ventas nuevas.

Normaliza y reactiva los cuatro medios fijos, conserva sus recargos generales y los de Crédito, y deja en cero los recargos por cuotas de los medios que no son Crédito. Si falta un medio fijo, lo inserta con recargos cero. Por lo tanto, no debe describirse este script como uno que solo crea tablas o como uno que nunca actualiza registros existentes.

## 10. Significado de las instrucciones SQL

- CREATE DATABASE y CREATE TABLE: crean la base y sus tablas, respectivamente.
- ALTER TABLE: cambia una tabla existente; puede agregar, cambiar o quitar columnas o restricciones.
- DROP TABLE, DROP COLUMN y DROP INDEX: eliminan el objeto indicado. Su utilización exige revisar dependencias y efecto sobre datos anteriores.
- INSERT: incorpora filas. UPDATE modifica filas existentes. SELECT consulta sin ser, por sí solo, una modificación.
- PRIMARY KEY: identifica inequívocamente cada registro. FOREIGN KEY obliga a que la referencia corresponda a un registro válido de la tabla relacionada.
- IDENTITY: genera identificadores. IDENTITY_INSERT permite cargar explícitamente un identificador, como los roles iniciales. DBCC CHECKIDENT ajusta el contador; no inserta una operación comercial.
- NOT NULL: exige un valor. NULL: permite ausencia de dato. DEFAULT: propone un valor cuando la inserción no lo especifica. WITH VALUES completa filas existentes al agregar ciertas columnas con valor predeterminado.
- CHECK: restringe valores, por ejemplo cantidades positivas, importes no negativos o monedas ARS/USD.
- UNIQUE e índice único: impiden repetir la clave definida. Un índice no es una tabla adicional.
- IF NOT EXISTS, OBJECT_ID y COL_LENGTH: consultan si existe una fila, un objeto o una columna antes de aplicar una acción.
- THROW: interrumpe con un mensaje cuando una condición impide continuar.
- EXEC y sp_executesql: ejecutan instrucciones construidas como texto, por ejemplo adaptaciones de columnas y restricciones antiguas.
- GO: separa lotes en el archivo. No es una instrucción que SqlCommand envíe al motor: el inicializador divide el archivo por esos separadores y ejecuta cada lote.

## 11. Tipos de datos utilizados

int identifica registros o cantidades enteras. bit representa estados verdadero/falso. nvarchar conserva texto Unicode; sus límites varían según el campo. datetime2 almacena fecha y hora; date solamente fecha. decimal guarda importes con precisión fija: decimal(10,2) admite hasta ocho cifras enteras y dos decimales; no significa diez cifras enteras.

nvarchar(max) se utiliza en el JSON del cierre. La restricción ISJSON comprueba su formato, pero no equivale a validar todas las reglas comerciales del contenido. La lógica del programa también interviene al construir y consultar el cierre.

## 12. Relaciones y transacciones operativas

Un Producto puede tener varias variantes. Una Venta pertenece a un cliente y a un usuario y tiene varios detalles y pagos. Cada detalle identifica la variante vendida, no solamente su nombre. Un ingreso agrupa detalles de mercadería de un proveedor. Los remitos vinculan las cantidades preparadas con la entrega.

Al registrar una venta, el acceso a datos utiliza una transacción: reúne cabecera, detalles, pagos, logística y las acciones de retiro inmediato que correspondan. Si falla una parte, se revierte esa operación. Registrar un pago posterior también comprueba el saldo y el estado de la caja dentro de su transacción.

Las relaciones evitan referencias inexistentes, pero no sustituyen todas las reglas: la aplicación valida, por ejemplo, documentos, condiciones de pago, disponibilidad y permisos. No todas las reglas de la pantalla tienen una restricción SQL equivalente.

## 13. Importes, recargos y cierres

Pago.monto conserva el importe aplicado a cancelar la venta, sin el recargo de ese cobro. Pago.monto_final conserva lo realmente cobrado con su recargo. La diferencia entre ambos es el recargo registrado.

Ejemplo: se cancelan $30.000 con un 10 % de recargo. monto es $30.000 y monto_final es $33.000. El cobro reduce la deuda base en $30.000. La consulta de saldo utiliza el total de venta y los cobros finales de manera consistente con los recargos incorporados al total.

La garantía muestra el abonado con recargos y los medios utilizados. Por la regla específica de este proyecto, Caja y el resumen mensual contabilizan monto, sin recargos. Los cierres anteriores se presentan con esa misma base al consultar los pagos vinculados, sin reescribir el JSON histórico guardado. No deben confundirse estos importes al comparar garantía y caja.

## 14. Repetición, bloqueo y recuperación ante errores

Idempotente significa que un script está preparado para repetirse sin duplicar las estructuras o configuraciones que comprueba. No significa que no realice cambios: puede actualizar registros, incorporar configuración faltante o retirar columnas antiguas.

PrepararEsquema utiliza una transacción y sp_getapplock, con recurso Prana.ActualizarEsquema y propietario Transaction, para serializar dos preparaciones simultáneas. SET XACT_ABORT ON y el bloque de manejo de errores permiten revertir la transacción del esquema si un lote falla.

La creación física de la base ocurre antes de esta transacción; por lo tanto, no se debe afirmar que cualquier error elimina también los archivos MDF/LDF. El respaldo sigue siendo necesario, especialmente antes de migraciones que eliminan objetos.

El inicializador y App.config deben apuntar al mismo destino. Cambiar solamente la conexión de la aplicación no cambia las constantes del inicializador. Las pruebas de integración requieren configuración aislada y no deben ejecutarse contra la base comercial.

## 15. Verificación para acompañar la entrega

En una base de ensayo nueva, verificar los dos roles, admin activo, cuatro medios con recargos cero y ausencia de datos comerciales. Ejecutar nuevamente la preparación y comprobar que no duplica esos registros.

En una copia de una instalación anterior, comprobar conservación de operaciones, adaptación de columnas y eliminación únicamente de los objetos previstos. Verificar usuario dado de baja con nombre reservado, documento DNI/CUIT, recargos, venta, pago, entrega y cierre. Registrar resultados reales y mensajes; no considerar una revisión de código como prueba de ejecución.

La verificación de índices, columnas y dependencias puede consultarse en sys.tables, sys.columns, sys.indexes y sys.foreign_keys. Antes de borrar PRODUCTOS u otro objeto ajeno, revisar esquema, filas y dependencias; la coincidencia de nombres no demuestra que sea seguro eliminarlo.

## 16. Archivos de referencia

Program.cs; Prana.Datos/InicializadorBaseDatos.cs; App.config; Prana.Datos/Conexion.cs; CrearEstructuraPrana_LocalDB.sql; ActualizarEstructuraPrana_LocalDB.sql; CajaYComprobantes.sql; CD_RegistroVenta.cs; CD_Pago.cs; CD_Caja.cs y CD_Comprobante.cs.

Síntesis para la defensa: el instalador entrega el motor requerido y los scripts; al abrir Prana se crea la base si falta, se construye el esquema si está vacío y se aplican las actualizaciones. La instalación comienza con roles, una cuenta administradora y cuatro medios de pago, sin ventas ni productos de ejemplo. A partir de allí la operación diaria incorpora los datos del comercio.
