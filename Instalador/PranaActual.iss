; constante del compilador myappname: centraliza un valor reutilizado por las secciones siguientes
#define MyAppName "Prana Electronica"
; constante del compilador myappversion: centraliza un valor reutilizado por las secciones siguientes
#define MyAppVersion "1.0.3"
; constante del compilador myappexename: centraliza un valor reutilizado por las secciones siguientes
#define MyAppExeName "Prana.Presentacion.exe"
; constante del compilador publishdir: centraliza un valor reutilizado por las secciones siguientes
#define PublishDir "..\obj\InstaladorActual\publicacion"

; seccion setup: agrupa las instrucciones de esta etapa del instalador
[Setup]
; identidad estable de la aplicacion: conservarla permite reconocer actualizaciones
AppId=PranaElectronica
; nombre visible del programa en el asistente y en windows
AppName={#MyAppName}
; version identificada por este paquete; no cambia automaticamente al modificar c#
AppVersion={#MyAppVersion}
; nombre del distribuidor que figura en la informacion del programa
AppPublisher=Prana Electronica
; destino propuesto; autopf elige archivos de programa segun el contexto de instalacion
DefaultDirName={autopf}\PranaElectronica
; grupo del menu inicio en el que se crean los accesos
DefaultGroupName={#MyAppName}
; icono del programa mostrado en la informacion de desinstalacion
UninstallDisplayIcon={app}\{#MyAppExeName}
; plataformas que pueden ejecutar este instalador; la publicacion de prana es win-x64
ArchitecturesAllowed=x64compatible
; activa el modo de instalacion de 64 bits en las plataformas compatibles
ArchitecturesInstallIn64BitMode=x64compatible
; solicita permisos de administrador para copiar el programa e instalar localdb si falta
PrivilegesRequired=admin
; carpeta de salida del compilador, relativa a este archivo iss
OutputDir=..\Instaladores
; nombre del ejecutable de instalacion sin extension; actualizarlo al preparar una entrega
OutputBaseFilename=PranaElectronica-Setup-1.0.3
; algoritmo empleado para comprimir los archivos incorporados
Compression=lzma2
; comprime el conjunto de archivos para reducir el tamano del paquete
SolidCompression=yes
; aspecto del asistente de instalacion; no modifica la logica comercial
WizardStyle=modern
; reutiliza el destino de una instalacion anterior reconocida por appid
UsePreviousAppDir=yes
; permite solicitar el cierre de aplicaciones que impidan reemplazar archivos
CloseApplications=yes
; decide si se reinician aplicaciones cerradas por ese mecanismo; run controla la apertura final de prana
RestartApplications=no

; seccion languages: agrupa las instrucciones de esta etapa del instalador
[Languages]
; configura el idioma, la opcion o el acceso de esta seccion; las constantes se resuelven segun el destino instalado
Name: "spanish"; MessagesFile: "compiler:Languages\Spanish.isl"

; seccion tasks: agrupa las instrucciones de esta etapa del instalador
[Tasks]
Name: "desktopicon"; Description: "Crear un acceso directo en el escritorio"; GroupDescription: "Accesos directos:"

; seccion files: agrupa las instrucciones de esta etapa del instalador
[Files]
; incorpora el msi para extraccion temporal; no lo deja como archivo permanente del programa
Source: "SqlLocalDB.msi"; Flags: dontcopy
; empaqueta los archivos source y los copia en destdir al instalar las rutas de origen se resuelven al compilar
Source: "{#PublishDir}\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "..\BaseDeDatos\*.sql"; DestDir: "{app}\BaseDeDatos"; Flags: ignoreversion

; seccion icons: agrupa las instrucciones de esta etapa del instalador
[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

; seccion run: agrupa las instrucciones de esta etapa del instalador
[Run]
; ofrece abrir prana al finalizar con el usuario original; no inicia otra instalacion de sql
Filename: "{app}\{#MyAppExeName}"; Description: "Abrir {#MyAppName}"; Flags: nowait postinstall skipifsilent runasoriginaluser

// seccion code: agrupa las instrucciones de esta etapa del instalador
[Code]
// localdb 2022 o 2025 de 64 bits
// comprueba el registro de localdb 2022/2025 de 64 bits; detectar una instalacion no verifica todavia la conexion a la base
function LocalDBInstalado: Boolean;
begin
// establece la respuesta de la preparacion; vacio permite seguir y un mensaje informa el motivo del bloqueo
  Result :=
    RegKeyExists(HKLM64, 'SOFTWARE\Microsoft\Microsoft SQL Server Local DB\Installed Versions\16.0') or
    RegKeyExists(HKLM64, 'SOFTWARE\Microsoft\Microsoft SQL Server Local DB\Installed Versions\17.0');
end;

// inno llama esta funcion antes de copiar prana un result con texto detiene la preparacion; needsrestart informa si hay que reiniciar
function PrepareToInstall(var NeedsRestart: Boolean): String;
var
  Codigo: Integer;
  Parametros: String;
begin
  Result := '';
// si el requisito ya existe, omite el msi y continua sin instalarlo otra vez
  if LocalDBInstalado then Exit;

// extrae a la carpeta temporal el msi incorporado mediante dontcopy
  ExtractTemporaryFile('SqlLocalDB.msi');
// construye los argumentos de windows installer: instalar, mostrar progreso, no reiniciar y generar registro detallado
  Parametros := '/i "' + ExpandConstant('{tmp}\SqlLocalDB.msi') +
    '" /passive /norestart IACCEPTSQLLOCALDBLICENSETERMS=YES /L*v "' +
    ExpandConstant('{tmp}\Prana-LocalDB.log') + '"';
// inicia msiexec y espera su finalizacion; codigo recibe el resultado del instalador de localdb
  if not Exec(ExpandConstant('{sys}\msiexec.exe'), Parametros, '',
    SW_SHOW, ewWaitUntilTerminated, Codigo) then
  begin
    Result := 'No se pudo iniciar la instalacion de LocalDB.';
// sale de la funcion sin ejecutar los pasos posteriores
    Exit;
  end;
// estos resultados requieren reinicio se detiene para que el usuario reinicie y ejecute nuevamente el paquete
  if (Codigo = 3010) or (Codigo = 1641) then
  begin
    NeedsRestart := True;
    Result := 'LocalDB requiere reiniciar Windows. Reinicie y ejecute este instalador nuevamente.';
    Exit;
  end;
// un resultado distinto de cero interrumpe con el codigo y la ruta del registro para diagnosticarlo
  if Codigo <> 0 then
  begin
    Result := 'No se pudo instalar LocalDB. Codigo: ' + IntToStr(Codigo) +
      '. Consulte el archivo ' + ExpandConstant('{tmp}\Prana-LocalDB.log');
    Exit;
  end;
// verifica otra vez la version despues del msi para no continuar con un requisito que no quedo disponible
  if not LocalDBInstalado then
    Result := 'No se detecto LocalDB 2022/2025 de 64 bits. Verifique el archivo SqlLocalDB.msi utilizado.';
end;
