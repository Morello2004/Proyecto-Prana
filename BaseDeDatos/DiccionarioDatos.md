# Diccionario de datos — Prana Electrónica

**Base de datos:** `PranaElectronica`  
**Esquema:** `dbo`  
**Motor:** SQL Server / LocalDB  
**Fuente:** estructura consultada en la base de datos y scripts del proyecto.

Este documento describe las tablas y columnas informadas. Las descripciones funcionales se basan en los nombres y el uso conocido del sistema; cuando el propósito exacto no se desprende del esquema, se indica de forma general. Los números de la columna «N.º» son los ordinales de SQL Server: si hay saltos, no significa necesariamente que falten campos; pueden quedar huecos por cambios anteriores del esquema.

## Convenciones

- **PK:** clave primaria.
- **FK → tabla.columna:** clave foránea y destino.
- **Nulo:** `Sí` indica que la columna acepta `NULL`.
- **IDENTITY:** valor generado automáticamente por SQL Server.
- Los valores predeterminados se muestran en la columna **Predeterminado**. `—` significa que no se informó uno.
- Los importes se expresan como valores decimales; la moneda o unidad depende del contexto de la operación.

## Tablas del sistema comercial

### `Categoria`
Clasifica los productos.

| N.º | Columna | Tipo | Nulo | Clave | Predeterminado | Descripción |
|---:|---|---|:---:|---|---|---|
| 1 | `id_categoria` | `int` | No | PK, IDENTITY | — | Identificador de la categoría. |
| 2 | `nombre_categoria` | `nvarchar(100)` | No | — | — | Nombre de la categoría. |

### `Cliente`
Información de clientes y datos asociados para ventas y entregas.

| N.º | Columna | Tipo | Nulo | Clave | Predeterminado | Descripción |
|---:|---|---|:---:|---|---|---|
| 1 | `id_cliente` | `int` | No | PK, IDENTITY | — | Identificador del cliente. |
| 2 | `nombre_cliente` | `nvarchar(100)` | No | — | — | Nombre. |
| 3 | `apellido_cliente` | `nvarchar(100)` | No | — | — | Apellido. |
| 4 | `dni` | `nvarchar(11)` | No | — | — | Número de documento. El tipo de documento se registra por separado. |
| 5 | `telefono` | `nvarchar(20)` | No | — | — | Teléfono de contacto. |
| 7 | `direccion` | `nvarchar(200)` | Sí | — | — | Dirección del cliente. |
| 8 | `fecha_registro` | `datetime2(7)` | No | — | — | Fecha y hora de registro. |
| 9 | `saldo_a_favor` | `decimal(10,2)` | Sí | — | `(0)` | Saldo a favor disponible. |
| 10 | `id_localidad` | `int` | Sí | FK → `Localidad.id_localidad` | — | Localidad del cliente. |
| 11 | `codigo_postal` | `nvarchar(12)` | Sí | — | — | Código postal asociado al cliente. |
| 12 | `tipo_documento` | `nvarchar(4)` | No | — | `(N'DNI')` | Tipo de documento; el esquema admite `DNI` o `CUIT`. |
| 13 | `activo` | `bit` | No | — | `((1))` | Indicador de actividad. |
| 14 | `eliminado` | `bit` | No | — | `((0))` | Indicador de baja lógica. |

### `Detalle_Ingreso`
Renglones de productos/variantes incluidos en un ingreso de stock.

| N.º | Columna | Tipo | Nulo | Clave | Predeterminado | Descripción |
|---:|---|---|:---:|---|---|---|
| 1 | `id_detalle_ingreso` | `int` | No | PK, IDENTITY | — | Identificador del renglón. |
| 2 | `id_ingreso` | `int` | No | FK → `Ingreso_Stock.id_ingreso` | — | Ingreso al que pertenece. |
| 3 | `id_variante` | `int` | No | FK → `Variante_Producto.id_variante` | — | Variante ingresada. |
| 4 | `cantidad` | `int` | No | — | — | Unidades ingresadas. |
| 5 | `costo_unitario` | `decimal(10,2)` | No | — | — | Costo por unidad en ese ingreso. |

### `Detalle_Remito`
Renglones de artículos incluidos en un remito.

| N.º | Columna | Tipo | Nulo | Clave | Predeterminado | Descripción |
|---:|---|---|:---:|---|---|---|
| 1 | `id_detalle_remito` | `int` | No | PK, IDENTITY | — | Identificador del renglón. |
| 2 | `id_remito` | `int` | No | FK → `Remito.id_remito` | — | Remito al que pertenece. |
| 3 | `id_variante` | `int` | No | FK → `Variante_Producto.id_variante` | — | Variante asociada. |
| 4 | `cantidad` | `int` | No | — | — | Cantidad consignada. |
| 5 | `descripcion` | `nvarchar(300)` | No | — | — | Descripción del artículo en el remito. |

### `Detalle_Venta`
Renglones de productos vendidos.

| N.º | Columna | Tipo | Nulo | Clave | Predeterminado | Descripción |
|---:|---|---|:---:|---|---|---|
| 1 | `id_detalle_venta` | `int` | No | PK, IDENTITY | — | Identificador del renglón. |
| 2 | `id_venta` | `int` | No | FK → `Venta.id_venta` | — | Venta a la que pertenece. |
| 3 | `id_variante` | `int` | No | FK → `Variante_Producto.id_variante` | — | Variante vendida. |
| 4 | `cantidad` | `int` | No | — | — | Unidades vendidas. |
| 5 | `precio_unitario` | `decimal(10,2)` | No | — | — | Precio aplicado por unidad. |
| 6 | `subtotal` | `decimal(10,2)` | No | — | — | Importe del renglón. |
| 7 | `id_estado_variante_producto` | `int` | No | FK → `Estado_Variante_Producto.id_estado_variante_producto` | — | Estado de la variante asociado al renglón. |

### `Envio`
Datos del envío asociado a un remito.

| N.º | Columna | Tipo | Nulo | Clave | Predeterminado | Descripción |
|---:|---|---|:---:|---|---|---|
| 1 | `id_envio` | `int` | No | PK, IDENTITY | — | Identificador del envío. |
| 2 | `id_remito` | `int` | No | FK → `Remito.id_remito` | — | Remito asociado. |
| 3 | `direccion` | `nvarchar(200)` | No | — | — | Dirección de entrega. |
| 4 | `horario_sugerido` | `nvarchar(50)` | Sí | — | — | Horario sugerido para la entrega. |
| 5 | `tipo_envio` | `nvarchar(30)` | No | — | — | Modalidad del envío, por ejemplo entrega a domicilio o retiro en local. |
| 6 | `costo` | `decimal(10,2)` | No | — | `((0))` | Costo del envío. |
| 7 | `id_estado_envio` | `int` | No | FK → `Estado_Envio.id_estado_envio` | — | Estado actual del envío. |
| 8 | `fecha_entrega` | `datetime2(7)` | Sí | — | — | Fecha y hora de entrega, si corresponde. |

### `Estado_Envio`
Catálogo de estados de envío.

| N.º | Columna | Tipo | Nulo | Clave | Predeterminado | Descripción |
|---:|---|---|:---:|---|---|---|
| 1 | `id_estado_envio` | `int` | No | PK, IDENTITY | — | Identificador del estado. |
| 2 | `nombre` | `nvarchar(30)` | No | — | — | Nombre del estado. |

### `Estado_Variante_Producto`
Catálogo de estados utilizados en el detalle de venta de una variante.

| N.º | Columna | Tipo | Nulo | Clave | Predeterminado | Descripción |
|---:|---|---|:---:|---|---|---|
| 1 | `id_estado_variante_producto` | `int` | No | PK, IDENTITY | — | Identificador del estado. |
| 2 | `nombre_estado_variante_producto` | `nvarchar(100)` | No | — | — | Nombre del estado. |

### `Estado_Venta`
Catálogo de estados de venta.

| N.º | Columna | Tipo | Nulo | Clave | Predeterminado | Descripción |
|---:|---|---|:---:|---|---|---|
| 1 | `id_estado_venta` | `int` | No | PK, IDENTITY | — | Identificador del estado. |
| 2 | `nombre_estado_venta` | `nvarchar(100)` | No | — | — | Nombre del estado. |

### `Ingreso_Stock`
Encabezado de un ingreso de mercadería de proveedor.

| N.º | Columna | Tipo | Nulo | Clave | Predeterminado | Descripción |
|---:|---|---|:---:|---|---|---|
| 1 | `id_ingreso` | `int` | No | PK, IDENTITY | — | Identificador del ingreso. |
| 2 | `id_proveedor` | `int` | No | FK → `Proveedor.id_proveedor` | — | Proveedor del ingreso. |
| 3 | `remito_proveedor` | `nvarchar(50)` | No | — | — | Número o referencia del remito del proveedor. |
| 4 | `fecha_ingreso` | `datetime2(7)` | No | — | — | Fecha y hora de ingreso. |

### `Localidad`
Catálogo de localidades.

| N.º | Columna | Tipo | Nulo | Clave | Predeterminado | Descripción |
|---:|---|---|:---:|---|---|---|
| 1 | `id_localidad` | `int` | No | PK, IDENTITY | — | Identificador de la localidad. |
| 2 | `nombre_localidad` | `nvarchar(100)` | No | — | — | Nombre de la localidad. |
| 3 | `codigo_postal` | `nvarchar(12)` | Sí | — | — | Código postal de la localidad. |

### `Marca`
Catálogo de marcas de productos.

| N.º | Columna | Tipo | Nulo | Clave | Predeterminado | Descripción |
|---:|---|---|:---:|---|---|---|
| 1 | `id_marca` | `int` | No | PK, IDENTITY | — | Identificador de la marca. |
| 2 | `nombre_marca` | `nvarchar(100)` | No | — | — | Nombre de la marca. |

### `Medio_Pago`
Catálogo de medios de pago y recargos por cantidad de cuotas.

| N.º | Columna | Tipo | Nulo | Clave | Predeterminado | Descripción |
|---:|---|---|:---:|---|---|---|
| 1 | `id_medio_pago` | `int` | No | PK, IDENTITY | — | Identificador del medio de pago. |
| 2 | `nombre_medio_pago` | `nvarchar(32)` | No | — | — | Nombre del medio. |
| 3 | `recargo_medio_pago` | `decimal(5,2)` | Sí | — | `(0)` | Recargo general configurado para el medio. |
| 4 | `recargo_2` | `decimal(5,2)` | No | — | `((0))` | Recargo para 2 cuotas. |
| 5 | `recargo_3` | `decimal(5,2)` | No | — | `((0))` | Recargo para 3 cuotas. |
| 6 | `recargo_6` | `decimal(5,2)` | No | — | `((0))` | Recargo para 6 cuotas. |
| 7 | `recargo_9` | `decimal(5,2)` | No | — | `((0))` | Recargo para 9 cuotas. |
| 8 | `recargo_12` | `decimal(5,2)` | No | — | `((0))` | Recargo para 12 cuotas. |
| 9 | `activo` | `bit` | No | — | `((1))` | Indica si el medio de pago está habilitado. |
| 10 | `tipo_caja` | `nvarchar(20)` | No | — | `(N'Otros')` | Clasificación del medio usada en los movimientos de caja. |

### `Modalidad_Entrega`
Catálogo de modalidades de entrega asociadas a las ventas.

| N.º | Columna | Tipo | Nulo | Clave | Predeterminado | Descripción |
|---:|---|---|:---:|---|---|---|
| 1 | `id_modalidad_entrega` | `int` | No | PK, IDENTITY | — | Identificador de la modalidad. |
| 2 | `nombre_modalidad_entrega` | `nvarchar(100)` | No | — | — | Nombre de la modalidad. |

### `Pago`
Pagos registrados para una venta.

| N.º | Columna | Tipo | Nulo | Clave | Predeterminado | Descripción |
|---:|---|---|:---:|---|---|---|
| 1 | `id_pago` | `int` | No | PK, IDENTITY | — | Identificador del pago. |
| 2 | `id_venta` | `int` | No | FK → `Venta.id_venta` | — | Venta a la que se imputa el pago. |
| 3 | `id_usuario` | `int` | No | FK → `Usuario.id_usuario` | — | Usuario que registra el pago. |
| 4 | `id_medio_pago` | `int` | No | FK → `Medio_Pago.id_medio_pago` | — | Medio utilizado. |
| 5 | `cuotas` | `int` | Sí | — | — | Cantidad de cuotas, si aplica. |
| 6 | `monto` | `decimal(10,2)` | No | — | — | Importe base del pago. |
| 7 | `monto_final` | `decimal(10,2)` | No | — | — | Importe final del pago, considerando recargos si corresponden. |
| 8 | `fecha_pago` | `datetime2(7)` | No | — | — | Fecha y hora del pago. |
| 9 | `tipo_caja` | `nvarchar(20)` | Sí | — | — | Clasificación del pago para caja. |
| 10 | `cuenta_destino` | `nvarchar(100)` | No | — | `(N'')` | Cuenta de destino, cuando corresponde. |

### `Producto`
Datos generales de los productos comercializados.

| N.º | Columna | Tipo | Nulo | Clave | Predeterminado | Descripción |
|---:|---|---|:---:|---|---|---|
| 1 | `id_producto` | `int` | No | PK, IDENTITY | — | Identificador del producto. |
| 2 | `nombre_producto` | `nvarchar(150)` | No | — | — | Nombre del producto. |
| 3 | `id_marca` | `int` | No | FK → `Marca.id_marca` | — | Marca del producto. |
| 4 | `descripcion` | `nvarchar(500)` | Sí | — | — | Descripción adicional. |
| 5 | `id_categoria` | `int` | No | FK → `Categoria.id_categoria` | — | Categoría del producto. |
| 6 | `modelo` | `varchar(15)` | Sí | — | — | Modelo o referencia del producto. |

### `Proveedor`
Datos de proveedores.

| N.º | Columna | Tipo | Nulo | Clave | Predeterminado | Descripción |
|---:|---|---|:---:|---|---|---|
| 1 | `id_proveedor` | `int` | No | PK, IDENTITY | — | Identificador del proveedor. |
| 2 | `nombre_proveedor` | `nvarchar(150)` | No | — | — | Nombre o razón social. |
| 3 | `telefono` | `nvarchar(20)` | Sí | — | — | Teléfono de contacto. |

### `Recibo`
Registro de recibos emitidos para pagos.

| N.º | Columna | Tipo | Nulo | Clave | Predeterminado | Descripción |
|---:|---|---|:---:|---|---|---|
| 1 | `id_recibo` | `int` | No | PK, IDENTITY | — | Identificador del recibo. |
| 2 | `id_pago` | `int` | No | FK → `Pago.id_pago` | — | Pago documentado. |
| 3 | `fecha_emision` | `datetime2(7)` | No | — | — | Fecha y hora de emisión. |

### `Remito`
Registro de remitos asociados a ventas.

| N.º | Columna | Tipo | Nulo | Clave | Predeterminado | Descripción |
|---:|---|---|:---:|---|---|---|
| 1 | `id_remito` | `int` | No | PK, IDENTITY | — | Identificador del remito. |
| 2 | `id_venta` | `int` | No | FK → `Venta.id_venta` | — | Venta documentada. |
| 3 | `fecha_emision` | `datetime2(7)` | No | — | — | Fecha y hora de emisión. |

### `Rol`
Catálogo de roles de usuario.

| N.º | Columna | Tipo | Nulo | Clave | Predeterminado | Descripción |
|---:|---|---|:---:|---|---|---|
| 1 | `id_rol_usuario` | `int` | No | PK, IDENTITY | — | Identificador del rol. |
| 2 | `nombre_rol_usuario` | `nvarchar(20)` | No | — | — | Nombre del rol. |

### `Usuario`
Cuentas de acceso, rol y datos para el control de intentos fallidos.

| N.º | Columna | Tipo | Nulo | Clave | Predeterminado | Descripción |
|---:|---|---|:---:|---|---|---|
| 1 | `id_usuario` | `int` | No | PK, IDENTITY | — | Identificador del usuario. |
| 2 | `nombre_usuario` | `nvarchar(100)` | No | — | — | Nombre de inicio de sesión. |
| 4 | `hash_contrasena` | `nvarchar(255)` | No | — | — | Hash de la contraseña; no es la contraseña en texto plano. |
| 5 | `id_rol_usuario` | `int` | No | FK → `Rol.id_rol_usuario` | — | Rol asignado. |
| 6 | `activo` | `bit` | No | — | `((1))` | Indica si la cuenta está habilitada. |
| 7 | `intentos_login_fallidos` | `int` | No | — | `((0))` | Cantidad de intentos fallidos registrados. |
| 8 | `bloqueado_hasta` | `datetime2(7)` | Sí | — | — | Fecha y hora hasta la que la cuenta está bloqueada, si aplica. |

### `Variante_Producto`
Variantes comerciales de un producto, con condición, precio y stock.

| N.º | Columna | Tipo | Nulo | Clave | Predeterminado | Descripción |
|---:|---|---|:---:|---|---|---|
| 1 | `id_variante` | `int` | No | PK, IDENTITY | — | Identificador de la variante. |
| 2 | `color` | `nvarchar(50)` | No | — | — | Color de la variante. |
| 3 | `condicion_producto` | `nvarchar(10)` | No | — | — | Condición comercial del producto; la aplicación reconoce, entre otras, `PRIMERA` y `OUTLET`. |
| 4 | `precio` | `decimal(10,2)` | No | — | — | Precio de venta de la variante. |
| 5 | `stock_disponible` | `int` | No | — | — | Unidades disponibles. |
| 6 | `id_producto` | `int` | No | FK → `Producto.id_producto` | — | Producto al que pertenece. |
| 7 | `activo` | `bit` | No | — | `((1))` | Indica si la variante está habilitada. |
| 8 | `costo` | `decimal(10,2)` | No | — | `((0))` | Costo de la variante. |

### `Venta`
Encabezado de ventas, incluyendo cliente, usuario, estado e importes.

| N.º | Columna | Tipo | Nulo | Clave | Predeterminado | Descripción |
|---:|---|---|:---:|---|---|---|
| 1 | `id_venta` | `int` | No | PK, IDENTITY | — | Identificador de la venta. |
| 2 | `id_cliente` | `int` | No | FK → `Cliente.id_cliente` | — | Cliente de la venta. |
| 3 | `id_usuario` | `int` | No | FK → `Usuario.id_usuario` | — | Usuario que registra la venta. |
| 4 | `fecha_venta` | `datetime2(7)` | No | — | — | Fecha y hora de la venta. |
| 5 | `id_estado_venta` | `int` | No | FK → `Estado_Venta.id_estado_venta` | — | Estado de la venta. |
| 6 | `total` | `decimal(10,2)` | No | — | — | Importe total registrado. |
| 7 | `id_modalidad_entrega` | `int` | No | FK → `Modalidad_Entrega.id_modalidad_entrega` | — | Modalidad de entrega seleccionada. |
| 8 | `descuento` | `decimal(10,2)` | Sí | — | `((0))` | Descuento aplicado. |

### `Venta_Entrega`
Datos complementarios de logística y condición de pago de una venta.

| N.º | Columna | Tipo | Nulo | Clave | Predeterminado | Descripción |
|---:|---|---|:---:|---|---|---|
| 1 | `id_venta` | `int` | No | PK y FK → `Venta.id_venta` | — | Venta asociada; también identifica el registro de entrega. |
| 2 | `domicilio` | `nvarchar(200)` | Sí | — | — | Domicilio informado para la entrega. |
| 3 | `fecha_estimada` | `datetime2(7)` | Sí | — | — | Fecha estimada de entrega. |
| 4 | `horario_sugerido` | `nvarchar(50)` | Sí | — | — | Horario sugerido. |
| 5 | `observaciones` | `nvarchar(500)` | Sí | — | — | Observaciones logísticas. |
| 6 | `tipo_logistica` | `nvarchar(50)` | Sí | — | — | Información del tipo o detalle logístico. |
| 7 | `costo_envio` | `decimal(10,2)` | No | — | — | Costo de envío asociado a la venta. |
| 8 | `observacion_pago` | `nvarchar(500)` | Sí | — | — | Observación relacionada con el pago. |
| 9 | `condicion_pago` | `nvarchar(30)` | No | — | — | Condición de pago acordada. |

## Tablas de caja y auditoría

### `Caja`
Una fila por apertura, conservada al pasar de Abierta a Cerrada. Puede haber varios cierres por fecha y una sola caja abierta a la vez.

| Columna | Tipo | Nulo | Regla o significado |
|---|---|:---:|---|
| id | int | No | Identidad y clave primaria; se conserva al cerrar |
| fecha | date | No | Fecha de la apertura; puede repetirse en varios turnos |
| estado | nvarchar(8) | No | Abierta o Cerrada |
| id_usuario | int | Sí | FK a Usuario; responsable del cierre manual |
| fecha_cierre | datetime2 | Sí | Momento de registro del cierre |
| datos | nvarchar(max) | No | JSON válido con los datos de la caja |
| inicio | datetime2 | No | Hora real de apertura |
| fin | datetime2 | Sí | Fin del turno; nulo mientras está abierto |
| id_usuario_apertura | int | Sí | Usuario que abrió; puede faltar en datos históricos |

Una caja abierta no tiene responsable ni fecha de cierre. La cerrada tiene fecha de registro y admite responsable nulo para cierres automáticos. Caja_Abierta y Caja_Cierre solo se consultan durante la migración de versiones anteriores y luego se retiran tras verificar la copia.

### `Caja_Movimiento`
Movimientos de caja, incluidos gastos e ingresos no derivados directamente de pagos.

| N.º | Columna | Tipo | Nulo | Clave | Predeterminado | Descripción |
|---:|---|---|:---:|---|---|---|
| 1 | `id` | `int` | No | PK, IDENTITY | — | Identificador del movimiento. |
| 2 | `fecha` | `datetime2(7)` | No | — | `(sysdatetime())` | Fecha y hora del movimiento. |
| 3 | `concepto` | `nvarchar(200)` | No | — | — | Motivo o concepto del movimiento. |
| 4 | `tipo` | `nvarchar(20)` | No | — | — | Tipo de movimiento; el script admite `Gasto`, `Ingreso extra` o `Café`. |
| 5 | `medio` | `nvarchar(20)` | No | — | — | Medio o forma utilizada para el movimiento. |
| 6 | `moneda` | `char(3)` | No | — | — | Código de moneda; el script admite `ARS` o `USD`. |
| 7 | `importe` | `decimal(18,2)` | No | — | — | Importe del movimiento; el script exige un valor mayor que cero. |
| 8 | `cuenta` | `nvarchar(100)` | No | — | — | Cuenta asociada al movimiento. |
| 9 | `id_usuario` | `int` | No | FK → `Usuario.id_usuario` | — | Usuario que registra el movimiento. |
| 10 | `anulado` | `bit` | No | — | `((0))` | Indica si el movimiento fue anulado. |
| 11 | `motivo_anulacion` | `nvarchar(200)` | Sí | — | — | Motivo de anulación, si corresponde. |

### `LogActividad`
Registro de acciones realizadas por usuarios sobre entidades del sistema.

| N.º | Columna | Tipo | Nulo | Clave | Predeterminado | Descripción |
|---:|---|---|:---:|---|---|---|
| 1 | `id_log` | `int` | No | PK, IDENTITY | — | Identificador del registro de auditoría. |
| 2 | `id_usuario` | `int` | No | FK → `Usuario.id_usuario` | — | Usuario que realizó la acción. |
| 3 | `accion` | `nvarchar(100)` | No | — | — | Código o nombre de la acción. |
| 4 | `entidad` | `nvarchar(100)` | No | — | — | Tipo/nombre de entidad afectada. |
| 5 | `id_entidad` | `int` | Sí | — | — | Identificador del registro afectado. |
| 6 | `detalle` | `nvarchar(1000)` | Sí | — | — | Información adicional de la acción. |
| 7 | `fecha` | `datetime2(7)` | No | — | `(sysdatetime())` | Fecha y hora de registro. |

## Objeto técnico de SQL Server

### `sysdiagrams`
Tabla auxiliar que SQL Server puede crear para almacenar diagramas de base de datos. No representa una entidad funcional del sistema comercial.

| N.º | Columna | Tipo | Nulo | Clave | Predeterminado | Descripción |
|---:|---|---|:---:|---|---|---|
| 1 | `name` | `sysname` | No | — | — | Nombre del diagrama. |
| 2 | `principal_id` | `int` | No | — | — | Identificador del principal propietario. |
| 3 | `diagram_id` | `int` | No | PK, IDENTITY | — | Identificador del diagrama. |
| 4 | `version` | `int` | Sí | — | — | Versión del diagrama. |
| 5 | `definition` | `varbinary(MAX)` | Sí | — | — | Definición binaria del diagrama. |

## Relaciones principales

- `Producto` → `Marca` y `Categoria`.
- `Variante_Producto` → `Producto`.
- `Detalle_Ingreso` → `Ingreso_Stock` y `Variante_Producto`; `Ingreso_Stock` → `Proveedor`.
- `Detalle_Venta` → `Venta`, `Variante_Producto` y `Estado_Variante_Producto`.
- `Venta` → `Cliente`, `Usuario`, `Estado_Venta` y `Modalidad_Entrega`.
- `Pago` → `Venta`, `Usuario` y `Medio_Pago`; `Recibo` → `Pago`.
- `Remito` → `Venta`; `Detalle_Remito` → `Remito` y `Variante_Producto`; `Envio` → `Remito` y `Estado_Envio`.
- `Venta_Entrega` → `Venta` (relación uno a uno por su clave primaria compartida).
- `Usuario` → `Rol`.
- `Cliente` → `Localidad` (relación opcional, porque `id_localidad` acepta nulos).
- `Caja`, `Caja_Movimiento` y `LogActividad` → `Usuario`.

## Observaciones sobre el alcance

1. El diccionario refleja las columnas y tipos copiados desde la consulta de metadatos. Los scripts de actualización pueden haber agregado o modificado columnas entre versiones; para documentar otra instalación, volver a ejecutar la consulta sobre esa base.
2. Se describen como `sysdiagrams` los metadatos de diagramas, no como datos de negocio.
3. La consulta recibida no incluye todas las restricciones (por ejemplo, índices únicos y algunas restricciones `CHECK`). Se mencionan únicamente las reglas conocidas a partir del esquema y scripts disponibles; este documento no sustituye un inventario completo de restricciones.

## Relaciones de cajas por turno

Pago.id_caja y Caja_Movimiento.id_caja son claves foráneas obligatorias a Caja.id. Caja.fecha dejó de ser única. UX_Caja_UnicaAbierta permite una sola fila con estado Abierta. CajasPorTurno.sql asigna las operaciones históricas por fecha antes de habilitar varios turnos diarios y rechaza relaciones ambiguas.
