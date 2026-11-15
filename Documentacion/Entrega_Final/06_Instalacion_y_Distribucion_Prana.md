# Instalación y distribución de Prana Electrónica

Prácticas Profesionalizantes III · Tecnicatura Superior en Análisis de Sistemas

Autor: Valentín Morello · ISFT 189 · Año 2026

Fecha de revisión: 11 de noviembre de 2026. Versión definida en el instalador: 1.0.3.

## 1. Propósito

El instalador permite distribuir Prana Electrónica en una computadora Windows sin instalar Visual Studio ni compilar el proyecto en el equipo del usuario. Reúne la aplicación publicada, sus dependencias, los scripts de preparación de la base y el paquete de SQL Server Express LocalDB. La publicación incluye el entorno de ejecución de .NET; LocalDB se instala como requisito separado cuando no se detecta una versión admitida.

Este anexo documenta la configuración del proyecto. Distingue la generación del paquete, la instalación y el primer inicio: son etapas diferentes. Compilar correctamente la aplicación no demuestra que el instalador funcione en una computadora sin herramientas de desarrollo.

## 2. Qué es Inno Setup y para qué sirve

Inno Setup es una herramienta para crear instaladores de programas para Windows. En este proyecto toma los archivos publicados de Prana y los reúne en un ejecutable que presenta un asistente de instalación. Permite copiar archivos, crear accesos directos, registrar la desinstalación, reconocer una instalación anterior y ejecutar requisitos previos.

No reemplaza a Visual Studio ni compila el código C# de Prana. Primero se publica la aplicación con .NET y después se empaqueta esa publicación con Inno Setup. La computadora del usuario no necesita Inno Setup, Visual Studio ni el SDK: recibe el instalador terminado.

Las instrucciones se escriben en un archivo de texto con extensión .iss. Inno Setup Compiler lee ese archivo y construye el ejecutable de instalación. Modificar el .iss no modifica un instalador creado anteriormente: debe volver a compilarse.

## 3. Preparación desde una computadora nueva

1. Contar con el proyecto completo y el SDK necesario para publicar Prana con .NET 10.
2. Descargar Inno Setup desde https://jrsoftware.org/isdl.php, ejecutar su instalador y completar el asistente. Se instala en la computadora que construye la entrega.
3. Obtener el asistente de SQL Server Express 2022 desde Microsoft para descargar el paquete independiente de LocalDB previsto para la entrega.
4. Abrir el asistente y seleccionar Descargar medios o Download Media. Esta opción descarga archivos para instalarlos después; no equivale a instalar inmediatamente el servidor completo.
5. Seleccionar LocalDB de 64 bits, elegir una carpeta y descargar SqlLocalDB.msi. Si el asistente presenta opciones diferentes, seguir la indicación de Microsoft para obtener el paquete independiente de LocalDB.
6. Copiar SqlLocalDB.msi a la carpeta Instalador del proyecto, junto a PranaActual.iss. El asistente SQL2022-SSEI-Expr.exe no reemplaza al MSI que espera el script.
7. Instalar también LocalDB en el equipo de desarrollo si se necesita ejecutar y probar Prana allí. Conservar el MSI para incorporarlo al instalador final.

Referencia oficial de LocalDB: https://learn.microsoft.com/sql/database-engine/configure-windows/sql-server-express-localdb.

## 4. Qué significa SQL Server Express, LocalDB y Descargar medios

SQL Server Express es una edición de SQL Server. LocalDB es una variante ligera que permite trabajar con instancias locales asociadas al usuario de Windows. Prana se conecta a MSSQLLocalDB: instalar solamente una instancia convencional llamada SQLEXPRESS no equivale a disponer de la instancia que utiliza la aplicación.

Medios de instalación significa los archivos que se descargan para instalar un componente. Descargar medios permite conservar el paquete y ejecutarlo posteriormente o incorporarlo a un instalador. No se refiere a los medios de pago del comercio.

SqlLocalDB.msi instala el motor; no contiene por sí mismo los clientes, productos o ventas de Prana. Los scripts del proyecto preparan la estructura de la base al iniciar la aplicación. Deben distinguirse motor, estructura y datos de trabajo.

SQL Server Management Studio es una herramienta para administrar bases de datos. Puede utilizarse en desarrollo y soporte, pero no reemplaza al motor ni es un requisito del usuario final para ejecutar Prana.

## 5. Orden completo de preparación

Proyecto guardado → publicación de Prana → verificación de archivos → incorporación de SqlLocalDB.msi → revisión de PranaActual.iss → compilación con Inno Setup → prueba del instalador → distribución.

dotnet publish prepara la aplicación y sus dependencias; Inno Setup construye el asistente que las instalará. Si cambia el código, deben repetirse ambas etapas. Si cambia únicamente una opción del .iss, debe volver a compilarse el instalador.

## 6. Herramientas y requisitos

Para generar el paquete se necesita el proyecto completo, un SDK compatible con .NET 10, Inno Setup compatible con el script y el archivo SqlLocalDB.msi de 64 bits correspondiente a LocalDB 2022 o 2025. El instalador de LocalDB debe obtenerse de Microsoft y colocarse en Instalador/SqlLocalDB.msi.

El equipo de destino necesita Windows compatible con la publicación win-x64 y con la versión de LocalDB incluida. El script admite plataformas x64compatibles; no genera un paquete de 32 bits. La compatibilidad con una edición concreta de Windows debe verificarse antes de distribuir.

La instalación solicita permisos de administrador porque copia archivos en Archivos de programa y puede instalar LocalDB. El uso cotidiano se realiza con la cuenta habitual de Windows. No hace falta instalar el SDK de .NET ni Visual Studio en el equipo de destino.

## 7. Archivos que intervienen

- Prana.Presentacion.csproj: aplicación Windows Forms dirigida a net10.0-windows.
- Properties/PublishProfiles/FolderProfile.pubxml: publicación Release, win-x64 y SelfContained=true.
- obj/InstaladorActual/publicacion: archivos publicados que consume Inno Setup.
- Instalador/PranaActual.iss: definición del asistente, archivos, accesos directos y requisito LocalDB.
- Instalador/SqlLocalDB.msi: paquete de LocalDB que se incorpora al ejecutable del instalador.
- BaseDeDatos: scripts SQL distribuidos con el programa.
- Instaladores/PranaElectronica-Setup-1.0.3.exe: nombre de salida definido por el script.

La versión y el nombre de salida son valores del script: no se actualizan automáticamente al modificar el código.

## 8. Generación de la publicación

Primero se guarda el trabajo y se cierra cualquier ejecución de Prana. Desde la raíz del repositorio se ejecuta:

```powershell
dotnet publish Prana.Presentacion.csproj -c Release -r win-x64 --self-contained true -o obj/InstaladorActual/publicacion
```

Release identifica la configuración de distribución; win-x64 selecciona el destino; self-contained incluye el entorno necesario para ejecutar .NET. No se utiliza PublishSingleFile: la salida contiene el ejecutable y otros archivos necesarios. Debe distribuirse toda la salida mediante el instalador, no solamente Prana.Presentacion.exe.

Si la publicación falla, no se debe compilar un instalador con archivos anteriores suponiendo que contienen los últimos cambios. Es conveniente utilizar una carpeta de publicación limpia para cada entrega, conservando fuera de ella las versiones que se quieran archivar.

## 9. Compilación con Inno Setup

1. Verificar que exista Instalador/SqlLocalDB.msi y que corresponda a la versión y arquitectura previstas.
2. Publicar la aplicación y comprobar que exista obj/InstaladorActual/publicacion/Prana.Presentacion.exe.
3. Abrir Instalador/PranaActual.iss en Inno Setup Compiler.
4. Revisar MyAppVersion y OutputBaseFilename para identificar la entrega.
5. Conservar AppId=PranaElectronica al generar actualizaciones de esta aplicación.
6. Ejecutar Build → Compile y comprobar el resultado de compilación.
7. Localizar el ejecutable generado en Instaladores y probarlo en un equipo o máquina virtual de ensayo.

La carpeta de publicación se calcula en relación con el archivo .iss. Si se traslada únicamente ese script a otra ubicación, las rutas relativas pueden dejar de encontrar los archivos. El resultado para entregar al usuario es el ejecutable del instalador; no necesita recibir el proyecto ni el archivo .iss.

## 10. Secuencia de instalación

1. El usuario ejecuta el instalador y autoriza la elevación de permisos de Windows.
2. El asistente se presenta en español y propone instalar en Archivos de programa/PranaElectronica.
3. Se ofrece crear un acceso directo en el escritorio. También se configura un acceso en el menú Inicio.
4. Antes de copiar la aplicación se comprueba LocalDB. Si falta, se ejecuta su paquete incorporado y se espera el resultado.
5. Si el requisito se completa, se copian la publicación y los scripts SQL.
6. Se registran los accesos directos y la información de desinstalación.
7. Al finalizar se ofrece abrir Prana. La ejecución posterior al asistente utiliza runasoriginaluser, para abrirla con el usuario original y no con la identidad elevada del instalador.

El script utiliza compresión LZMA2 y compresión sólida. No contiene una descarga de requisitos durante la instalación: incluye el MSI dentro del paquete. Esto permite distribuir los archivos necesarios juntos, aunque no constituye una prueba de funcionamiento sin conexión.

## 11. Detección e instalación de LocalDB

La función LocalDBInstalado consulta el registro de Windows de 64 bits bajo SOFTWARE/Microsoft/Microsoft SQL Server Local DB/Installed Versions. Reconoce las claves 16.0 y 17.0, correspondientes a las versiones previstas por el script. Una instalación anterior que no tenga esas claves no satisface esta comprobación.

Si LocalDB está detectado, se omite la ejecución del MSI. Esta detección comprueba su registro de instalación, no que una instancia concreta pueda abrir la base. Esa conexión se comprueba cuando inicia Prana.

Si falta, PrepareToInstall extrae SqlLocalDB.msi a la carpeta temporal y llama a msiexec.exe. Se usa instalación pasiva, se impide un reinicio automático y se proporciona IACCEPTSQLLOCALDBLICENSETERMS=YES. El responsable de distribución debe revisar las condiciones de redistribución y licencia del paquete utilizado.

El resultado se interpreta así:

- Código 0: ejecución satisfactoria; se comprueba nuevamente que LocalDB esté detectado.
- Códigos 3010 o 1641: se solicita reiniciar y volver a ejecutar el instalador para continuar.
- Otro código o imposibilidad de iniciar msiexec: se detiene la instalación y se muestra el problema.
- Ejecución satisfactoria sin detectar la versión esperada: se detiene para revisar el MSI utilizado.

El registro detallado de esa ejecución se escribe como Prana-LocalDB.log en la carpeta temporal utilizada por el instalador. El mensaje de error informa la ubicación. El MSI se declara con dontcopy: queda incorporado para extracción temporal y no como archivo permanente de la aplicación.

## 12. Primer inicio y preparación de datos

El instalador copia los scripts, pero no ejecuta directamente las consultas de creación. Al abrir Prana, Program.Main llama a InicializadorBaseDatos.AsegurarBaseDeDatos antes de mostrar el acceso.

El inicializador se conecta a (localdb)/MSSQLLocalDB mediante autenticación integrada de Windows y comprueba la existencia de PranaElectronica. Cuando debe crearla, utiliza la carpeta %LOCALAPPDATA%/PranaElectronica/Data del usuario que ejecuta el programa y genera PranaElectronica.mdf y PranaElectronica_log.ldf.

Si la base está vacía ejecuta CrearEstructuraPrana_LocalDB.sql. Después ejecuta ActualizarEstructuraPrana_LocalDB.sql y CajaYComprobantes.sql. Las actualizaciones se aplican también en inicios posteriores, con comprobaciones de existencia.

La preparación del esquema utiliza una transacción y un bloqueo de aplicación para evitar dos actualizaciones simultáneas. Si falla, se revierte esa transacción y se muestra el error de inicialización; no se abre el acceso. La creación física de la base ocurre antes de esa transacción: no se debe afirmar que un error del esquema elimina también los archivos físicos.

La creación inicial incorpora la cuenta administradora prevista por el script y los datos básicos de configuración. No incorpora clientes, productos ni ventas de trabajo. Las credenciales iniciales se entregan por un canal separado; este documento no reproduce contraseñas ni hashes.

## 13. Actualización de una instalación

El AppId se mantiene para identificar la misma aplicación. UsePreviousAppDir conserva la ubicación anterior; CloseApplications solicita cerrar aplicaciones que bloqueen archivos; RestartApplications=no evita su reinicio automático por ese mecanismo. La opción final de abrir Prana se maneja por separado.

Los archivos incluidos se reemplazan mediante ignoreversion. La aplicación aplica las modificaciones del esquema al iniciar. Por eso una actualización comprende tanto archivos del programa como posibles cambios de la base.

Debe realizarse un respaldo antes de actualizar. Los scripts actuales incluyen eliminación de columnas sin uso; no corresponde describir todas las migraciones como simples agregados o garantizar que ninguna elimina información. La transacción del esquema tampoco equivale a una reversión completa de todos los pasos del instalador.

Actualizar el programa no traslada automáticamente la información entre computadoras o cuentas de Windows. LocalDB y la ubicación de datos están vinculados al usuario: ese traslado requiere un procedimiento de respaldo y recuperación verificado.

## 14. Desinstalación

Inno Setup registra la desinstalación de los archivos que distribuye y de los accesos directos. El script no define la eliminación de la base creada en AppData ni la desinstalación de LocalDB. Este último puede ser utilizado por otras aplicaciones y no se administra como un archivo propio de Prana.

Desinstalar y volver a instalar no constituye un procedimiento de borrado de operaciones ni una recuperación de datos. La conservación efectiva y posterior reutilización de la base deben comprobarse en las pruebas de desinstalación y reinstalación.

## 15. Verificaciones para la entrega

Registrar para cada prueba: versión del paquete, versión de Windows, versión de LocalDB, resultado esperado, resultado observado y evidencia. No marcar como aprobada una prueba que solo se revisó en el código.

- Equipo limpio: instalación sin Visual Studio ni .NET previo, incorporación de LocalDB y apertura del acceso.
- LocalDB existente: omisión del MSI y conexión correcta al iniciar.
- Reinicio solicitado: interrupción controlada y continuación al ejecutar el instalador otra vez.
- Actualización: conservación de clientes, productos, ventas y cuentas; aplicación del esquema nuevo.
- Operación básica: ingreso, creación de usuario, consulta de productos, venta, cobro y comprobantes.
- Usuario habitual: apertura sin ejecutar Prana como administrador.
- Error del requisito: mensaje y disponibilidad del registro de LocalDB.
- Desinstalación y reinstalación: comportamiento de los accesos y recuperación de los datos conservados.

Este anexo se apoya en la inspección de los archivos del proyecto. No certifica una instalación completa en un equipo limpio ni la identidad del MSI disponible. Esas evidencias deben adjuntarse cuando se ejecuten las pruebas.

## 16. Explicación de las instrucciones del instalador

El archivo .iss tiene secciones entre corchetes. Cada sección agrupa instrucciones de una finalidad. Las líneas de configuración comentadas comienzan con punto y coma; la sección Code utiliza Pascal Script y admite comentarios con dos barras.

Definiciones iniciales:

- #define MyAppName: nombre visible del programa.
- #define MyAppVersion: versión de esta entrega. Debe actualizarse antes de generar una nueva versión.
- #define MyAppExeName: ejecutable al que apuntan los accesos directos.
- #define PublishDir: carpeta de archivos publicados. Los dos puntos indican subir una carpeta desde la ubicación del .iss.
- Las expresiones como {#MyAppName} insertan el valor definido durante la compilación.

Setup: identidad y ubicación.

- AppId: identidad de la aplicación instalada. Se conserva al actualizar; no debe cambiarse en cada versión.
- AppName y AppVersion: nombre y versión que identifica el asistente.
- AppPublisher: nombre del distribuidor.
- DefaultDirName: carpeta propuesta para instalar. {autopf} representa la carpeta de archivos de programa adecuada al contexto.
- DefaultGroupName: grupo de accesos en el menú Inicio.
- UninstallDisplayIcon: icono de la información de desinstalación. {app} representa la carpeta elegida para Prana.

Setup: plataforma y permisos.

- ArchitecturesAllowed=x64compatible: limita las plataformas admitidas por el paquete.
- ArchitecturesInstallIn64BitMode=x64compatible: utiliza el modo de instalación de 64 bits correspondiente.
- PrivilegesRequired=admin: solicita elevación para copiar en Archivos de programa e instalar LocalDB cuando falta. No obliga a usar Prana diariamente como administrador.

Setup: salida y comportamiento.

- OutputDir: carpeta donde se genera el instalador. Se interpreta respecto de la ubicación del script.
- OutputBaseFilename: nombre del instalador sin la extensión .exe. Debe revisarse junto con la versión; no cambia automáticamente.
- Compression=lzma2: método de compresión de los archivos incorporados.
- SolidCompression=yes: comprime los archivos como un conjunto para reducir el tamaño.
- WizardStyle=modern: aspecto del asistente.
- UsePreviousAppDir=yes: conserva la carpeta de una instalación anterior reconocida.
- CloseApplications=yes: permite detectar y solicitar el cierre de aplicaciones que bloqueen archivos. No reemplaza guardar el trabajo antes de actualizar.
- RestartApplications=no: evita reiniciar automáticamente esas aplicaciones. La oferta final de abrir Prana se configura por separado.

Languages y Tasks:

- Languages configura español mediante Spanish.isl. El prefijo compiler: localiza ese archivo dentro de Inno Setup.
- Tasks define la opción desktopicon. Description y GroupDescription son los textos visibles de la casilla y su grupo. Permite que el usuario elija crear un acceso en el escritorio.

Files: archivos incorporados al instalador.

- Source: archivo o conjunto que Inno debe encontrar al compilar. Si falta el MSI o la publicación, no se obtiene correctamente el paquete.
- DestDir: destino donde se copiará el archivo al instalar. {app} identifica la carpeta de Prana.
- SqlLocalDB.msi con dontcopy: incorpora el MSI para extraerlo temporalmente desde Code. No lo copia como un archivo permanente del programa.
- PublishDir con asterisco: incluye todos los archivos publicados, no solamente el ejecutable.
- ignoreversion: reemplaza los archivos distribuidos sin decidir según su versión individual.
- recursesubdirs: incorpora las subcarpetas de la publicación.
- createallsubdirs: conserva también la estructura de carpetas.
- BaseDeDatos con asterisco.sql: copia los scripts SQL a la subcarpeta BaseDeDatos. No ejecuta consultas por el hecho de copiarlos.

Icons: accesos directos.

- Name determina la ubicación del acceso. {group} corresponde al grupo del menú Inicio y {autodesktop} al escritorio adecuado al contexto.
- Filename indica el ejecutable que se abrirá.
- Tasks: desktopicon condiciona el acceso del escritorio a la opción que marcó el usuario.

Run: apertura al finalizar.

- Filename apunta al ejecutable instalado. Description es el texto de la opción Abrir Prana.
- nowait permite terminar el asistente sin esperar que se cierre Prana.
- postinstall ofrece la apertura en la página final.
- skipifsilent omite esa apertura opcional en modo silencioso.
- runasoriginaluser abre Prana con la identidad original, en lugar de la identidad elevada que instaló los archivos. Esto permite preparar los datos locales para el usuario habitual.

Code: detección de LocalDB.

- LocalDBInstalado devuelve verdadero si encuentra alguna de las versiones admitidas.
- RegKeyExists comprueba una clave del registro. HKLM64 identifica la vista de 64 bits del equipo.
- Las claves 16.0 y 17.0 representan las versiones previstas. El operador or permite aceptar cualquiera de ellas.
- Detectar una clave no comprueba la conexión a una instancia: esa verificación ocurre al iniciar Prana.

Code: ejecución del requisito.

- PrepareToInstall es un evento que Inno llama antes de copiar la aplicación. Result vacío permite continuar; un texto en Result detiene la preparación y muestra el motivo.
- NeedsRestart informa por referencia que se necesita reiniciar. Exit termina la función cuando LocalDB ya se detectó o cuando se debe interrumpir el proceso.
- ExtractTemporaryFile extrae SqlLocalDB.msi del paquete. ExpandConstant resuelve constantes como {tmp}, la carpeta temporal de esta ejecución.
- Parametros construye los argumentos para Windows Installer. Las comillas protegen rutas con espacios.
- Exec inicia msiexec.exe desde {sys}, la carpeta del sistema. ewWaitUntilTerminated espera su finalización; Codigo recibe el resultado.
- /i indica instalar el MSI. /passive muestra el progreso con interacción reducida. /norestart evita reiniciar automáticamente.
- IACCEPTSQLLOCALDBLICENSETERMS=YES proporciona la aceptación que requiere el paquete. El responsable debe revisar las condiciones de licencia y redistribución del MSI utilizado.
- /L*v genera un registro detallado en Prana-LocalDB.log.
- Código 0 permite continuar si la detección posterior es correcta. 3010 y 1641 informan la necesidad de reiniciar. Los otros códigos detienen la preparación con un mensaje.

## 17. Compilación del script y ejecución del instalador

El desarrollador abre PranaActual.iss en Inno Setup Compiler y selecciona Build → Compile. La herramienta interpreta el archivo, verifica las rutas Source, incorpora el MSI y comprime la publicación. El resultado es PranaElectronica-Setup-1.0.3.exe. Para generar ese archivo no necesita abrir la base comercial.

También puede compilarse mediante ISCC.exe desde una terminal. Este ejemplo supone Inno Setup 6 instalado en su ubicación habitual; se debe adaptar la ruta si corresponde:

```powershell
& "C:\Program Files (x86)\Inno Setup 6\ISCC.exe" "Instalador\PranaActual.iss"
```

El usuario final ejecuta el .exe generado, no el .iss ni el comando de compilación. El asistente instala los requisitos y archivos, crea los accesos y permite abrir Prana. Al abrir Prana se prepara la base y luego se presenta el acceso.

## 18. Archivos de referencia para la defensa

La configuración descrita puede contrastarse con Instalador/PranaActual.iss, Properties/PublishProfiles/FolderProfile.pubxml, Prana.Presentacion.csproj, Program.cs, Prana.Datos/InicializadorBaseDatos.cs y los tres scripts de preparación mencionados.

Explicación breve para la exposición: primero se publica Prana con .NET incluido. Inno Setup empaqueta esa publicación junto con LocalDB y los scripts. En el equipo de destino instala el requisito si falta, copia el programa y crea los accesos. La base se prepara al abrir Prana con la cuenta habitual del usuario. Las actualizaciones reemplazan el programa y aplican los cambios de esquema en el siguiente inicio.

## 19. Script completo de Inno Setup

Este listado corresponde a Instalador/PranaActual.iss. Se compila desde su carpeta prevista con la publicación y SqlLocalDB.msi disponibles. No es un script SQL.

```ini
#define MyAppName "Prana Electronica"
#define MyAppVersion "1.0.3"
#define MyAppExeName "Prana.Presentacion.exe"
#define PublishDir "..\obj\InstaladorActual\publicacion"

[Setup]
AppId=PranaElectronica
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher=Prana Electronica
DefaultDirName={autopf}\PranaElectronica
DefaultGroupName={#MyAppName}
UninstallDisplayIcon={app}\{#MyAppExeName}
ArchitecturesAllowed=x64compatible
ArchitecturesInstallIn64BitMode=x64compatible
PrivilegesRequired=admin
OutputDir=..\Instaladores
OutputBaseFilename=PranaElectronica-Setup-1.0.3
Compression=lzma2
SolidCompression=yes
WizardStyle=modern
UsePreviousAppDir=yes
CloseApplications=yes
RestartApplications=no

[Languages]
Name: "spanish"; MessagesFile: "compiler:Languages\Spanish.isl"

[Tasks]
Name: "desktopicon"; Description: "Crear un acceso directo en el escritorio"; GroupDescription: "Accesos directos:"

[Files]
Source: "SqlLocalDB.msi"; Flags: dontcopy
Source: "{#PublishDir}\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "..\BaseDeDatos\*.sql"; DestDir: "{app}\BaseDeDatos"; Flags: ignoreversion

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "Abrir {#MyAppName}"; Flags: nowait postinstall skipifsilent runasoriginaluser

[Code]
// LocalDB 2022 o 2025 de 64 bits.
function LocalDBInstalado: Boolean;
begin
  Result :=
    RegKeyExists(HKLM64, 'SOFTWARE\Microsoft\Microsoft SQL Server Local DB\Installed Versions\16.0') or
    RegKeyExists(HKLM64, 'SOFTWARE\Microsoft\Microsoft SQL Server Local DB\Installed Versions\17.0');
end;

function PrepareToInstall(var NeedsRestart: Boolean): String;
var
  Codigo: Integer;
  Parametros: String;
begin
  Result := '';
  if LocalDBInstalado then Exit;

  ExtractTemporaryFile('SqlLocalDB.msi');
  Parametros := '/i "' + ExpandConstant('{tmp}\SqlLocalDB.msi') +
    '" /passive /norestart IACCEPTSQLLOCALDBLICENSETERMS=YES /L*v "' +
    ExpandConstant('{tmp}\Prana-LocalDB.log') + '"';
  if not Exec(ExpandConstant('{sys}\msiexec.exe'), Parametros, '',
    SW_SHOW, ewWaitUntilTerminated, Codigo) then
  begin
    Result := 'No se pudo iniciar la instalacion de LocalDB.';
    Exit;
  end;
  if (Codigo = 3010) or (Codigo = 1641) then
  begin
    NeedsRestart := True;
    Result := 'LocalDB requiere reiniciar Windows. Reinicie y ejecute este instalador nuevamente.';
    Exit;
  end;
  if Codigo <> 0 then
  begin
    Result := 'No se pudo instalar LocalDB. Codigo: ' + IntToStr(Codigo) +
      '. Consulte el archivo ' + ExpandConstant('{tmp}\Prana-LocalDB.log');
    Exit;
  end;
  if not LocalDBInstalado then
    Result := 'No se detecto LocalDB 2022/2025 de 64 bits. Verifique el archivo SqlLocalDB.msi utilizado.';
end;

```
