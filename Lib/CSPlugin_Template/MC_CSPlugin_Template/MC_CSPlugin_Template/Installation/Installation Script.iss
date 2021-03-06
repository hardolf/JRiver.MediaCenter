; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

[Setup]
AppName=Template Plugin for JRMC11
AppVerName=Plugin Version 0.0.0.1 Alpha
AppPublisher=Publisher Here
AppPublisherURL=www.mywebsite.com
AppSupportURL=www.mywebsite.com
AppUpdatesURL=www.mywebsite.com
DefaultDirName={pf}\J River\Media Center 11\Plugins\TemplatePlugin
DefaultGroupName=Template Plugin
DisableProgramGroupPage=yes
OutputBaseFilename=Setup
Compression=lzma
SolidCompression=yes
DirExistsWarning=No

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Files]
Source: "..\Build Files\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Icons]
Name: "{group}\{cm:UninstallProgram, Plugin}"; Filename: "{uninstallexe}"

[Registry]
Root: HKLM; Subkey: "Software\J. River\Media Jukebox\Plugins\Interface\Template Plugin"; Flags: uninsdeletekey
Root: HKLM; Subkey: "Software\J. River\Media Jukebox\Plugins\Interface\Template Plugin"; ValueType: dword; ValueName: "IVersion"; ValueData: "00000001"; Flags: uninsdeletekey
Root: HKLM; Subkey: "Software\J. River\Media Jukebox\Plugins\Interface\Template Plugin"; ValueType: string; ValueName: "Company"; ValueData: "Mr Smiths Plugins"; Flags: uninsdeletekey
Root: HKLM; Subkey: "Software\J. River\Media Jukebox\Plugins\Interface\Template Plugin"; ValueType: string; ValueName: "Version"; ValueData: "0.0.0.1"; Flags: uninsdeletekey
Root: HKLM; Subkey: "Software\J. River\Media Jukebox\Plugins\Interface\Template Plugin"; ValueType: string; ValueName: "URL"; ValueData: "www.MrSmithsPlugins.Com"; Flags: uninsdeletekey
Root: HKLM; Subkey: "Software\J. River\Media Jukebox\Plugins\Interface\Template Plugin"; ValueType: string; ValueName: "Copyright"; ValueData: "Copyright (c) 2006, Mr Smith."; Flags: uninsdeletekey
Root: HKLM; Subkey: "Software\J. River\Media Jukebox\Plugins\Interface\Template Plugin"; ValueType: dword; ValueName: "PluginMode"; ValueData: "00000001"; Flags: uninsdeletekey
Root: HKLM; Subkey: "Software\J. River\Media Jukebox\Plugins\Interface\Template Plugin"; ValueType: string; ValueName: "ProdID"; ValueData: "Template_MCPlugin.CSTemplate"; Flags: uninsdeletekey

[Run]
Filename: "{win}\Microsoft.NET\Framework\v2.0.50727\regasm"; Parameters: "/Codebase MC_CSPlugin_Template.dll";          WorkingDir: "{app}\"; StatusMsg: "Registering Plugin"; Flags:runhidden

[UninstallRun]
Filename: "{win}\Microsoft.NET\Framework\v2.0.50727\regasm"; Parameters: "/unregister MC_CSPlugin_Template.dll";          WorkingDir: "{app}\"; StatusMsg: "Registering Plugin"; Flags:runhidden
