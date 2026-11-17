# Manual de usuario de Prana Electrónica

Edición del **17 de noviembre de 2026**, revisada contra los formularios y las validaciones de esta copia del proyecto.

- [PDF para leer o imprimir](Manual_de_Usuario_Completo_Prana.pdf): 44 páginas.
- [Word editable](Manual_de_Usuario_Completo_Prana.docx).
- [HTML autónomo con índice enlazado](Manual_de_Usuario_Completo_Prana.html).

Incluye **51 capítulos y 34 figuras**: instalación, acceso, permisos, clientes, productos y variantes, ingresos, ventas, pagos y recargos, documentos, entregas, caja por turnos, resumen mensual, alertas, informes, historial, usuarios, localidades y opciones compartidas de stock. Conserva ejemplos y prácticas guiadas.

Cambios principales de esta edición:

- Teléfono por área y número, con diez dígitos en total; segundo teléfono opcional con el mismo formato.
- Domicilio con calle y número o S/N, localidad independiente y destino propio para cada preparación de entrega.
- Administración de localidades y corrección, unificación y eliminación de opciones de stock, disponibles para el administrador.
- Caja sin saldos iniciales ni conteo de billetes. Balance de ingresos menos gastos, separado por moneda; cobros de ventas por su base sin recargo.
- Apertura explícita, varios turnos diarios y ejemplo de cierre actualizado a $28.000.
- Capturas renovadas y referencias ajustadas a los controles actuales.

## Regeneración

El Word es la fuente editable de esta edición. El generador convierte sus párrafos, tablas e imágenes a HTML autónomo y exporta el PDF mediante Microsoft Edge:

```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File Documentacion/Manual_Usuario_Completo/Generar-Manual.ps1
powershell -NoProfile -ExecutionPolicy Bypass -File Documentacion/Manual_Usuario_Completo/Verificar-Manual.ps1
```

`-SoloHtml` omite la exportación del PDF. Los scripts requieren Windows PowerShell y Microsoft Edge para la exportación; la verificación del PDF utiliza Windows.Data.Pdf.

Durante la elaboración se retiraron del repositorio las fuentes y herramientas anteriores. Esta edición conserva las capturas dentro del Word y del HTML; no depende de las carpetas retiradas. El generador nuevo permite mantener las tres versiones a partir del Word.

## Verificación

Se comprobaron las partes XML y las 34 imágenes del Word, las imágenes incorporadas y los 51 capítulos del HTML, y la apertura y renderizado del PDF de 44 páginas. Se revisaron visualmente páginas representativas. El resultado está en [verificacion_edicion_2026-11-17.json](verificacion_edicion_2026-11-17.json), con las páginas renderizadas en `revision_2026-11-17`.

Las capturas se obtuvieron sin consultas ni operaciones sobre la base comercial, con los eventos comerciales retirados. Los comprobantes de ejemplo usan datos ficticios. Esta revisión documental no equivale a una prueba funcional completa del sistema; Word no se renderizó con Microsoft Word.
