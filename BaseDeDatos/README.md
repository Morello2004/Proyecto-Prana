El inicio normal de la aplicación llama a `InicializadorBaseDatos.AsegurarBaseDeDatos()`, en el proyecto `Prana.Datos`. Si la base está vacía, crea la estructura; si ya contiene tablas, aplica únicamente las actualizaciones pendientes. No vuelve a crear usuarios ni restablece contraseñas.

Los archivos que se distribuyen con el programa son `Prana.Datos/CrearEstructuraPrana_LocalDB.sql` y `Prana.Datos/ActualizarEstructuraPrana_LocalDB.sql`. Ambos se copian automáticamente al compilar y publicar. La creación completa ya incluye las actualizaciones actuales. La actualización puede repetirse y conserva columnas, recargos, medios desactivados y datos existentes.

Para actualizar manualmente una base existente, seleccionar esa base en SQL Server y ejecutar `ActualizarEstructuraPrana_LocalDB.sql`; este archivo no cambia de base mediante `USE`. Se recomienda usar el inicio de la aplicación, que aplica la creación y las actualizaciones en una transacción con bloqueo para evitar dos actualizaciones simultáneas.

Las actualizaciones actuales cubren los recargos por cuotas, el campo `activo` de medios de pago, la capacidad monetaria de `Venta.descuento`, las tablas `LogActividad` y `Venta_Entrega`, la conversión de `Tipo_Envio` a la columna controlada `Envio.tipo_envio`, la eliminación de los indicadores de formato de `Recibo` y `Remito` y del catálogo `Formato_Documento`, la eliminación del indicador redundante `Venta_Entrega.generar_remito` y el rol Vendedor. Los estados de pago y entrega siguen creándose al registrar las operaciones correspondientes.

Verificación realizada en una base separada con datos ficticios: instalación nueva, dos arranques simultáneos, repetición, actualización de esquema antiguo y parcialmente actualizado, administrador renombrado, conservación de ventas/pagos/recargos y reversión completa ante un fallo deliberado.

El login registra los intentos fallidos por usuario. Al tercer error bloquea la cuenta durante 15 minutos; un administrador autenticado puede restablecer los intentos desde Gestión de usuarios. Si no hay otro administrador disponible, el bloqueo se libera automáticamente al vencer el plazo.

La baja de usuarios es lógica: Gestión de usuarios cambia `Usuario.activo` a falso, conserva sus ventas y registros, e impide nuevos inicios de sesión. La acción queda registrada en `LogActividad`. La pantalla muestra las cuentas inactivas, pero no ofrece reactivarlas.

