# Lib

## Interop.MediaCenter.dll

Interop assembly for the JRiver Media Center COM automation API (`MCAutomation`,
`IMJAutomationEvents_FireMJEventEventHandler`), used by `LyricsFinderPlugin`.

It replaces the former `<COMReference Include="MediaCenter">`, which resolved the type
library through the registry key
`HKCR\TypeLib\{03457D73-676C-4BB0-A275-D12D30ADB89A}`. That key is not created by every
Media Center installation, so the build broke on machines where Media Center was present
but the type library was unregistered. Referencing the generated assembly directly makes
the build independent of the registry and of a local Media Center installation.

The reference uses `EmbedInteropTypes=True`, so the types are embedded into
`LyricsFinderPlugin.dll` and this file is a build-time dependency only - it is not copied
to the output folder and must not be deployed.

### Regenerating

Only needed if JRiver changes the automation API. Generated from the type library shipped
with Media Center 36 (type library version 1.0):

```
"C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.8 Tools\TlbImp.exe" ^
    "C:\Program Files\J River\Media Center 36\Media Center 36.tlb" ^
    /out:Interop.MediaCenter.dll /namespace:MediaCenter /machine:Agnostic
```

Note that `.gitignore` excludes `*.dll`; the exception for this file is at the end of that
file.
