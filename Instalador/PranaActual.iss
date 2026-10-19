#define MyAppName "Prana Electronica"
#define MyAppVersion "1.0.2"
#define MyAppExeName "Prana.Presentacion.exe"
#define PublishDir "..\obj\InstaladorActual\publicacion"

[Setup]
; Conservar este AppId para actualizar instalaciones anteriores.
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
OutputBaseFilename=PranaElectronica-Setup-2026-10-06
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
Source: "{#PublishDir}\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "..\BaseDeDatos\*.sql"; DestDir: "{app}\BaseDeDatos"; Flags: ignoreversion recursesubdirs createallsubdirs

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "Abrir {#MyAppName}"; Flags: nowait postinstall skipifsilent runasoriginaluser
