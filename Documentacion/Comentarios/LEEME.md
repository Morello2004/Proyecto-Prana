# Comentarios de estudio del proyecto

La revisión del 12 de noviembre de 2026 abarca 111 archivos C# y 16 scripts: cuatro SQL, once PowerShell y el instalador Inno Setup. Incluye formularios, diseñadores, entidades, negocio, datos y pruebas.

Los comentarios explican responsabilidades, datos, validaciones, cálculos, consultas, transacciones, eventos y preparación de documentos. Cada explicación se escribe en una sola línea, en minúsculas, sin acentos ni puntos. Se agrupan las instrucciones relacionadas para evitar repetir la misma explicación por cada parámetro o control.

La comparación de tokens C# y de las instrucciones de los scripts comprobó que esta pasada conserva el código ejecutable. La compilación terminó sin errores. No se ejecutaron scripts de base de datos ni se modificaron datos del comercio.

La evidencia actual está en resumen_comentarios_finales.json, verificacion_comentarios_finales_cs.json y verificacion_comentarios_finales_scripts.json. Los informes anteriores corresponden a revisiones previas y conservan sus cantidades históricas.

## Orden de lectura sugerido

1. Program.cs y Prana.Datos/InicializadorBaseDatos.cs: inicio y preparación de datos.
2. Prana.Entidades: significado y tipo de los datos.
3. Formularios: acciones del usuario y cambios de presentación.
4. Prana.negocio: validaciones, permisos y cálculos.
5. Prana.Datos: consultas, parámetros y transacciones.
6. BaseDeDatos: estructura inicial, migraciones y configuración básica.
7. Instalador/PranaActual.iss: distribución y requisito LocalDB.
8. Pruebas: ejemplos de comprobaciones y diferencias entre ensayos sin base e integración.

## Sintaxis que aparece en las clases

- using al principio del archivo permite referirse a tipos de un espacio de nombres sin escribir su nombre completo. No abre por sí mismo una conexión.
- using con una variable o bloque libera el recurso al salir; es importante para conexiones, lectores, comandos e imágenes.
- namespace agrupa los tipos. class define una clase. new construye una instancia.
- partial permite repartir una clase en archivos; el formulario y su Designer siguen siendo la misma clase.
- public permite acceder desde otros tipos; private limita el miembro a la clase; internal limita el acceso al ensamblado.
- static pertenece al tipo y no exige una instancia. readonly impide reasignar el campo fuera de su inicialización o constructor, pero no vuelve inmutable una lista que contenga.
- get lee una propiedad; set escribe y value contiene el valor recibido. Una propiedad con expresión puede calcular el resultado cada vez que se consulta.
- var infiere el tipo al compilar; no convierte la variable en un dato sin tipo.
- El signo ? en un tipo permite null. ?. accede solo si el objeto no es null; ?? establece un reemplazo si falta el valor. ! después de una referencia expresa una suposición al compilador: no comprueba nada durante la ejecución.
- == compara; = asigna. && exige ambas condiciones; || acepta alguna. ! niega una condición.
- => expresa una función o acceso breve. En eventos, += conecta un manejador y -= lo retira.
- Un evento avisa que sucedió una acción. Conectar el mismo manejador dos veces puede repetir esa acción.
- List conserva una colección modificable. Add incorpora, Clear vacía, Count cuenta. No confundir una lista temporal con filas guardadas en SQL.
- Where filtra; Select transforma; Any comprueba existencia; Sum suma; OrderBy ordena; ToList materializa un resultado en una lista.
- try delimita un paso que puede fallar; catch atiende una excepción; finally realiza tareas de salida. return termina el método; throw comunica una excepción.
- decimal representa importes con precisión decimal. El sufijo m identifica un literal decimal. El formato C2 muestra moneda con dos decimales, sin cambiar el valor almacenado.
- Los textos interpolados con $ incluyen expresiones entre llaves. Un texto @ permite conservar saltos de línea y barras; las consultas SQL pueden ocupar varias líneas dentro de un solo texto.

## Diferencias importantes para explicar el negocio

Agregar un producto o un pago en Nueva venta prepara datos; Registrar venta confirma la operación. En Pagos, Importe a cancelar es base; en Nueva venta, Importe cobrado es dinero con recargo. La garantía usa cobros finales y Caja aplica la regla de bases sin recargo.

Un producto no es una variante: nombre, modelo, marca y categoría son comunes; color y condición identifican su presentación. Seleccionar texto y confirmar una opción de un ComboBox tampoco son la misma acción.

## Verificación del cambio

verificacion_cs.json registra la comparación de tokens C# antes y después de la documentación. La inserción conserva instrucciones, literales, nombres y operadores. verificacion_scripts.json registra las comprobaciones equivalentes para scripts; se normalizan los finales de línea al comparar SQL y PowerShell.

Se compiló Prana.Presentacion con los comentarios: cero errores. No se ejecutaron migraciones ni se modificaron datos de trabajo para añadir esta documentación. Estos controles no sustituyen las pruebas funcionales de futuras modificaciones de código.
