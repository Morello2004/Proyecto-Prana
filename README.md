# Prana Electrónica

Sistema de gestión comercial de escritorio desarrollado en C#, Windows Forms y SQL Server LocalDB. Incluye clientes, localidades, productos, stock, ventas, pagos, entregas, caja, comprobantes e informes.

Proyecto académico de Prácticas Profesionalizantes de la Tecnicatura Superior en Análisis de Sistemas.

## Instalación

El instalador actual es **PranaElectronica-Setup-1.0.4.exe**, para Windows de 64 bits. Incluye .NET y el instalador de SQL Server LocalDB. Se distribuye como archivo adjunto de una [Release](https://github.com/Morello2004/Proyecto-Prana/releases), separado del código fuente.

Al iniciar, la aplicación crea la base local si no existe. Las bases existentes deben tener el esquema compatible: no se ejecutan migraciones automáticas.

## Compilar desde el código

Requisitos: Windows, SDK de .NET compatible con `net10.0-windows` y SQL Server LocalDB 2022 o 2025 de 64 bits para ejecutar la aplicación.

```powershell
dotnet restore Prana.Presentacion.csproj
dotnet build Prana.Presentacion.csproj -c Release
dotnet run --project Prana.Presentacion.csproj
```

La conexión utiliza autenticación integrada de Windows y la instancia `(localdb)\MSSQLLocalDB`; se configura en `App.config`.

## Estructura

- Archivos raíz: interfaz Windows Forms y punto de entrada.
- `Prana.negocio`: reglas y servicios de negocio.
- `Prana.Datos`: acceso a datos e inicialización de la base.
- `Prana.Entidades`: modelos compartidos.
- `BaseDeDatos/InstalarPrana.sql`: esquema inicial necesario para nuevas instalaciones.
- `Properties` y `Plantillas`: recursos y configuración de publicación.
- `Instalador/PranaActual.iss`: definición del instalador.

## Documentación

- [Manual de usuario (PDF)](Documentacion/Manual_Usuario_Completo/Manual_de_Usuario_Completo_Prana.pdf)
- [Manual de usuario (Word)](Documentacion/Manual_Usuario_Completo/Manual_de_Usuario_Completo_Prana.docx)
- [Funcionamiento del código](Documentacion/Refactorizacion/Funcionamiento_Codigo_Completo.md)
- [Explicación de los formularios](Documentacion/Refactorizacion/Explicacion_Tecnica_Formularios.md)

## Generar el instalador

```powershell
dotnet publish Prana.Presentacion.csproj -c Release -r win-x64 --self-contained true -o obj/InstaladorActual/publicacion
ISCC.exe Instalador/PranaActual.iss
```

Se necesita Inno Setup en el `PATH` y `Instalador/SqlLocalDB.msi`, obtenido de Microsoft. El MSI y el ejecutable generado se mantienen fuera del repositorio. El resultado queda en `instaladores/` y se adjunta a la Release correspondiente en GitHub.
