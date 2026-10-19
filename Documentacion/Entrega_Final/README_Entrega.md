# Entrega final - Sistema de Gestión Comercial Prana Electrónica

**Autor:** Valentín Morello  
**Institución:** ISFT 189  
**Carrera:** Tecnicatura Superior en Análisis de Sistemas  
**Materia:** Prácticas Profesionalizantes III  
**Año:** 2026  
**Edición:** 2 de octubre de 2026

Prana Electrónica es una aplicación de escritorio para organizar clientes, productos y variantes, ingresos de stock, ventas, pagos, entregas, caja y comprobantes. La documentación describe el código actual y distingue funciones implementadas, límites y mejoras futuras.

## Documentos y orden recomendado de lectura

| Orden | Documento | Páginas | Contenido |
|---|---|---:|---|
| 1 | [01_Informe_Final_Prana](01_Informe_Final_Prana.docx) - [PDF](01_Informe_Final_Prana.pdf) | 12 | Informe académico: problema, objetivos, alcance, arquitectura, módulos, procesos y conclusión. |
| 2 | [02_Manual_de_Usuario_Prana](02_Manual_de_Usuario_Prana.docx) - [PDF](02_Manual_de_Usuario_Prana.pdf) | 11 | Procedimientos de operación con los nombres de los controles actuales. |
| 3 | [03_Manual_Tecnico_Prana](03_Manual_Tecnico_Prana.docx) - [PDF](03_Manual_Tecnico_Prana.pdf) | 12 | Tecnologías, solución, persistencia, seguridad, publicación, instalación y mantenimiento. |
| 4 | [04_Plan_y_Resultados_de_Pruebas_Prana](04_Plan_y_Resultados_de_Pruebas_Prana.docx) - [PDF](04_Plan_y_Resultados_de_Pruebas_Prana.pdf) | 27 | 45 casos con diez campos, resultados de ejecución y cobertura pendiente. |
| 5 | [05_Anexo_Reglas_Funcionales_y_Tecnicas](05_Anexo_Reglas_Funcionales_y_Tecnicas.docx) - [PDF](05_Anexo_Reglas_Funcionales_y_Tecnicas.pdf) | 9 | Reglas actuales, decisiones de negocio, límites y 19 tareas técnicas identificadas en esta entrega. |

Los archivos DOCX son editables y los PDF conservan la presentación para entregar o imprimir. Los índices y números de página están actualizados. El manual de usuario incluye una captura real de caja con datos ficticios; los diagramas representan clases, responsabilidades y relaciones del proyecto.

## Ejecutar el sistema

1. Utilizar Windows de 64 bits compatible con la publicación .NET 10 para Windows.
2. Instalar SQL Server LocalDB por separado. El instalador de Prana no incluye ese motor.
3. Ejecutar el instalador y abrir el acceso directo con la cuenta habitual de Windows. La publicación es autocontenida e incluye el runtime .NET.
4. Esperar la preparación de la base local. La aplicación utiliza `(localdb)\MSSQLLocalDB` y el catálogo `PranaElectronica` con autenticación integrada. Los archivos se crean en `%LOCALAPPDATA%/PranaElectronica/Data`.
5. Ingresar con el usuario inicial **admin** y la credencial inicial provista por el responsable de instalación. No se publican contraseñas ni hashes en esta entrega.
6. Configurar medios de pago, clientes, productos y existencias antes de registrar operaciones comerciales.

Para ejecutar desde fuentes, instalar un SDK .NET 10 y Visual Studio con soporte para escritorio .NET; abrir `PranaElectronica.slnx`, seleccionar `Prana.Presentacion` como proyecto de inicio y ejecutar. Desde la raíz del repositorio también puede utilizarse:

```powershell
dotnet run --project Prana.Presentacion.csproj
```

La aplicación prepara la base antes de mostrar el login. La conexión definida en `App.config` no cambia por sí sola el destino fijo del inicializador; el manual técnico explica esta limitación. No deben utilizarse pruebas de integración contra la base comercial.

## Instalador y publicación

La ubicación en el repositorio es `Instaladores/PranaElectronica-Setup-2026-10-02.exe`, fuera de esta carpeta documental. El script correspondiente es `Instalador/PranaActual.iss`.

El instalador existente fue generado antes de los últimos cambios de usuarios y búsqueda. Debo regenerar y probar la publicación antes de distribuir una versión actual; el nombre del archivo no acredita que incluya esos cambios. Esta entrega documental no recompila ni reemplaza el instalador. La instalación en una computadora limpia y la actualización integral están planificadas.

Publicación equivalente desde la raíz del repositorio:

```powershell
dotnet publish Prana.Presentacion.csproj -c Release -r win-x64 --self-contained true -o obj/InstaladorActual/publicacion
```

Después debe comprobarse el comportamiento de esa publicación y compilarse el script Inno Setup que apunta a dicha carpeta. El manual técnico detalla versión, AppId, scripts SQL y límites del reemplazo de archivos.

## Diagramas

La carpeta `Diagramas` contiene nueve diagramas: arquitectura por capas, casos de uso, entidad-relación, componentes y paquetes, despliegue, y secuencias de venta, ingreso de stock, pago y preparación/confirmación de entrega. Cada uno incluye fuente PlantUML `.puml`, imagen `.png`, versión `.svg` y `.pdf`.

El modelo entidad-relación representa 27 tablas y 28 relaciones declaradas, con claves y campos relacionados. `Envio.tipo_envio` admite únicamente “Envío a domicilio” y “Retiro en local”. Para leer el diagrama a mayor escala, utilizar SVG o ampliar su PDF. Recibo pertenece al esquema, pero no representa un circuito operativo completo en pantalla.

## Pruebas y evidencia

La compilación de las pruebas originales finalizó con 0 errores y 86 advertencias; la copia aislada ampliada, con 0 errores y 84 advertencias.

- La suite original se detuvo en una expectativa histórica de ciudad obligatoria; la regla actual permite ciudad vacía.
- La suite de presentación completó 22 verificaciones correctas.
- La copia aislada ampliada completó 125 verificaciones correctas y una no satisfactoria: permitió un nombre de usuario duplicado al editar una cuenta.
- Los registros OK incluyen verificaciones repetidas de distribución a distintos anchos y no equivalen a 125 casos funcionales distintos.
- El plan contiene 45 casos: 30 aprobados, 13 planificados y 2 no satisfactorios. No atribuye resultados a instalación limpia, actualización o procedimientos que no se ejecutaron.

Las corridas de integración utilizaron datos ficticios en un catálogo temporal separado, con protección contra reutilización de una base existente y eliminación del catálogo creado al finalizar. La aplicación comercial no fue el destino de estas pruebas. Los cuatro archivos de `Evidencias` conservan resultados sin rutas personales ni credenciales.

## Fuente complementaria pendiente

Para completar la conservación íntegra del anexo histórico, debo incorporar `Documentacion_Funcional_Tecnica_Prana_Electronica.docx`. Ese archivo no estaba disponible en el material de trabajo. El anexo entregado organiza las reglas de la implementación actual y 19 tareas con identificadores propios; no se presenta como una reproducción de la lista original de 59 tareas. La correspondencia con esa lista queda pendiente de incorporar la fuente.

## Repositorio

[Proyecto Prana en GitHub](https://github.com/Morello2004/Proyecto-Prana)

## Lista de archivos entregados

- `README_Entrega.md`
- `01_Informe_Final_Prana.docx`
- `01_Informe_Final_Prana.pdf`
- `02_Manual_de_Usuario_Prana.docx`
- `02_Manual_de_Usuario_Prana.pdf`
- `03_Manual_Tecnico_Prana.docx`
- `03_Manual_Tecnico_Prana.pdf`
- `04_Plan_y_Resultados_de_Pruebas_Prana.docx`
- `04_Plan_y_Resultados_de_Pruebas_Prana.pdf`
- `05_Anexo_Reglas_Funcionales_y_Tecnicas.docx`
- `05_Anexo_Reglas_Funcionales_y_Tecnicas.pdf`
- `Diagramas/01_Arquitectura_por_capas.pdf`
- `Diagramas/01_Arquitectura_por_capas.png`
- `Diagramas/01_Arquitectura_por_capas.puml`
- `Diagramas/01_Arquitectura_por_capas.svg`
- `Diagramas/02_Casos_de_uso_general.pdf`
- `Diagramas/02_Casos_de_uso_general.png`
- `Diagramas/02_Casos_de_uso_general.puml`
- `Diagramas/02_Casos_de_uso_general.svg`
- `Diagramas/03_Modelo_entidad_relacion.pdf`
- `Diagramas/03_Modelo_entidad_relacion.png`
- `Diagramas/03_Modelo_entidad_relacion.puml`
- `Diagramas/03_Modelo_entidad_relacion.svg`
- `Diagramas/04_Componentes_y_paquetes.pdf`
- `Diagramas/04_Componentes_y_paquetes.png`
- `Diagramas/04_Componentes_y_paquetes.puml`
- `Diagramas/04_Componentes_y_paquetes.svg`
- `Diagramas/05_Despliegue.pdf`
- `Diagramas/05_Despliegue.png`
- `Diagramas/05_Despliegue.puml`
- `Diagramas/05_Despliegue.svg`
- `Diagramas/06_Secuencia_registrar_venta.pdf`
- `Diagramas/06_Secuencia_registrar_venta.png`
- `Diagramas/06_Secuencia_registrar_venta.puml`
- `Diagramas/06_Secuencia_registrar_venta.svg`
- `Diagramas/07_Secuencia_ingresar_stock.pdf`
- `Diagramas/07_Secuencia_ingresar_stock.png`
- `Diagramas/07_Secuencia_ingresar_stock.puml`
- `Diagramas/07_Secuencia_ingresar_stock.svg`
- `Diagramas/08_Secuencia_registrar_pago.pdf`
- `Diagramas/08_Secuencia_registrar_pago.png`
- `Diagramas/08_Secuencia_registrar_pago.puml`
- `Diagramas/08_Secuencia_registrar_pago.svg`
- `Diagramas/09_Secuencia_preparar_y_confirmar_entrega.pdf`
- `Diagramas/09_Secuencia_preparar_y_confirmar_entrega.png`
- `Diagramas/09_Secuencia_preparar_y_confirmar_entrega.puml`
- `Diagramas/09_Secuencia_preparar_y_confirmar_entrega.svg`
- `Evidencias/compilacion.txt`
- `Evidencias/pruebas_ampliadas.txt`
- `Evidencias/pruebas_originales.txt`
- `Evidencias/pruebas_presentacion.txt`
