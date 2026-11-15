# Manual de capacitación y operación de Prana

Edición del **12 de noviembre de 2026**, destinada a vendedores y administradores del comercio.

- [Manual editable en Word](Manual_de_Usuario_Completo_Prana.docx).
- [Manual en PDF](Manual_de_Usuario_Completo_Prana.pdf): 76 páginas.
- [Versión HTML autónoma](Manual_de_Usuario_Completo_Prana.html), con índice y todas las imágenes incorporadas.

El manual tiene **49 capítulos y 32 figuras**. Incluye instalación, acceso, navegación, tabla comparativa de roles, clientes, productos y variantes, ingresos, ventas, pagos y recargos, documentos, entregas, caja, cierre automático, resumen mensual, alertas, informes, historial, usuarios, problemas frecuentes y prácticas guiadas. Las acciones se explican dentro de su pantalla y se utilizan los nombres visibles de sus controles.

## Fuentes y generación

`contenido.json` conserva los capítulos de base. `Preparar-Contenido-Manual.ps1` aplica las ampliaciones, las correcciones de `Aplicar-Correcciones-Empleados.ps1` y las referencias actuales de `Preparar-Imagenes-Manual.ps1`. `contenido_edicion.json` contiene el resultado final de los 49 capítulos. Word y PDF se generan desde ese mismo contenido.

Desde la raíz del repositorio:

```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File Documentacion/Manual_Usuario_Completo/Generar-Edicion-Final.ps1
```

El parámetro `-SoloHtml` genera Word y HTML sin exportar el PDF. La exportación a PDF requiere Microsoft Edge. `Generar-Word.ps1` permite regenerar solamente Word.

Para renovar las capturas y ejemplos:

```powershell
dotnet run --project Pruebas/Manual/Capturas.csproj -- Documentacion/Manual_Usuario_Completo/imagenes
dotnet run --project Pruebas/Manual/Capturas.csproj -- Documentacion/Manual_Usuario_Completo/imagenes --ejemplos
powershell -NoProfile -ExecutionPolicy Bypass -File Documentacion/Manual_Usuario_Completo/Exportar-Browser.ps1 -Ejemplos
```

La herramienta de capturas retira los eventos comerciales antes de mostrar los formularios: no ejecuta las consultas de carga ni registra operaciones en la base comercial. Los ejemplos utilizan datos ficticios. `imagenes_edicion` contiene los recortes con sus referencias numeradas y `figuras_edicion.json` relaciona cada figura con su capítulo.

## Verificación de la edición

Se verifican el contenido y las partes XML de Word, las imágenes incorporadas, los recortes y la correspondencia de las referencias con los controles. Se abrió y renderizó el PDF con el lector de Windows; se revisaron páginas representativas, incluida la tabla de permisos y las pantallas de navegación y ventas. Estas verificaciones documentales no equivalen a una prueba funcional completa con la base del comercio. El documento Word no se renderizó con Microsoft Word porque no está disponible en este entorno.

Las correcciones acordadas quedan registradas en [Correcciones incorporadas](Correcciones_Pendientes_Manual.md). La copia para entrega se publica como `Documentacion/Entrega_Final/08_Manual_de_Capacitacion_Empleados_Prana.docx` y su PDF del mismo nombre.