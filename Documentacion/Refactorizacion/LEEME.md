# Refactorización para facilitar la lectura

Fecha: 11 de noviembre de 2026.

Para estudiar el recorrido de las pantallas, consultar [Explicación técnica de los formularios](Explicacion_Tecnica_Formularios.md). Cada módulo incluye sus ventanas auxiliares, los métodos principales, las consultas, los guardados y el manejo de errores.

La explicación está organizada en secciones numeradas: 0_ Inicio del programa, 1_ Login, 2_ Interfaz y las demás pantallas. Cada operación tiene su subtítulo y las ventanas auxiliares se desarrollan dentro de la sección que las abre. [Abrir la versión para WordPad](Explicacion_Tecnica_Formularios_WordPad.rtf).

## Alcance

Se recorrieron los 112 archivos C# del proyecto, incluidos presentación, diseñadores, negocio, datos, entidades y pruebas. El objetivo es mejorar la lectura de la implementación existente, sin incorporar pantallas ni operaciones comerciales nuevas.

## Cambios realizados

- Formato consistente, separación de instrucciones y alineación de bloques.
- Llaves explícitas en 310 bloques de condiciones, recorridos y uso de recursos que tenían una sola instrucción.
- Renombrado de 137 variables locales o parámetros de expresiones anónimas mediante sus símbolos de C#: por ejemplo, cn pasa a conexion, tx a transaccion, cmd a comando y r a lector cuando corresponde y no provoca colisiones de nombres.
- Inicializadores de objetos extensos distribuidos por propiedades para revisar cada dato sin leer una línea completa de formulario.
- Renombrado de 21 controles en Nueva Venta, Clientes y Stock, con sus referencias, nombres internos y pruebas que los consultan. El detalle figura en controles_renombrados.json.
- Separación de la lectura de campos en ObtenerVentaDelFormulario, ObtenerClienteDelFormulario, ObtenerProductoDelFormulario y ObtenerMovimientoDelFormulario. Construyen la misma solicitud en el momento en que antes se construía en línea; no guardan nada por sí solos.
- Reutilización de la calculadora de ventas dentro de CN_Pago para las cotizaciones existentes.
- Ajuste de comentarios para que acompañen los nuevos nombres y distingan parámetros SQL de valores de controles.

Los métodos y propiedades públicos existentes se mantienen. No se modificaron el esquema SQL, las migraciones, los criterios de cobro o caja ni los valores de configuración del instalador en esta refactorización.

## Ejemplos de lectura

En Nueva Venta, dgvClienteSeleccionado reemplaza a dataGridView2; nudCostoEnvio reemplaza a numericUpDown1; txtSegundoTelefono reemplaza a textBox1. En Clientes los nombres de los campos identifican expresamente al cliente. En Stock se distinguen los datos del producto, la variante y el documento del proveedor.

RegistrarVenta conserva el flujo anterior: revisa el total, obtiene la solicitud del formulario, registra, guarda el número, abre los documentos y ofrece continuar. La separación de la solicitud no altera el tratamiento de errores ni las condiciones de pago.

## Verificaciones realizadas

- Compilación de Prana.Presentacion: cero errores; permanecen advertencias de compilación.
- Comparación de literales de los 112 archivos contra la copia anterior: consultas SQL, mensajes, formatos, importes y límites conservados. Se admiten únicamente los cambios documentados de nombres internos de controles.
- Pruebas de presentación: selección y autocompletado, agregado consecutivo, rechazo de variantes repetidas, columna Cantidad, contraste de filas y garantía con varios medios de pago.
- Pruebas de recargos: recargo general, porcentaje por cuotas y cambio de medio sin modificar la deuda.
- Pruebas del diseñador: construcción de formularios y eventos sin duplicaciones.

Estos resultados no sustituyen una prueba completa contra una base comercial ni una instalación en una computadora limpia. La refactorización no ejecutó migraciones ni cambió datos de trabajo.

## Evidencia y lectura posterior

verificacion_estructura.json registra las modificaciones de nombres locales y bloques por archivo. controles_renombrados.json registra los nombres de controles. Las pruebas existentes continúan dentro de Pruebas y pueden ejecutarse de forma independiente.

Los informes de Documentacion/Comentarios corresponden a la pasada anterior de documentación. Sus verificaciones de código idéntico describen aquella etapa; no deben interpretarse como que esta refactorización no modificó identificadores o estructura.

## Decisiones escritas con if y else

Se reemplazaron 120 expresiones ternarias en 31 archivos por bloques if y else. Las decisiones de retorno, asignacion, campos de objetos y fragmentos de comprobantes se leen ahora como pasos separados con tipos explicitos.

No se modificaron los signos de tipos anulables, el acceso condicional ni la sustitucion de null: tienen una funcion distinta del operador ternario. La comprobacion sintactica final encontro cero expresiones ternarias en los archivos C# del sistema y de sus pruebas.

La compilacion y las pruebas especificas de presentacion y recargos pasaron. El archivo sin_ternarios.json registra el alcance por archivo. Para mantener este estilo, las nuevas decisiones deben escribirse con if y else y los tipos deben indicarse explicitamente siempre que puedan nombrarse.

## Actualización de la explicación técnica

La edición del 12 de noviembre de 2026 incorpora la numeración transaccional de ventas, las quince acciones del historial, su recorrido por negocio y datos, las diferencias de transacción y el cálculo de estados de pago, entrega y cliente. Medios de pago explica la clasificación interna sin mostrar el campo Tipo en caja. El archivo RTF se regeneró y su texto se comparó con el Markdown completo.
