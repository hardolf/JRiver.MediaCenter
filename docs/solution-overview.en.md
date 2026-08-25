# LyricsFinder / JRiver.MediaCenter — architecture and onboarding report

Status: reviewed 2026-08-25 on branch `master`. This report is analysis only — no code was changed.

> English version. The Danish original is [`solution-overview.da.md`](solution-overview.da.md).

---

## 1. What is this?

The repo holds extensions for **JRiver Media Center** (MC). The bulk of it is
**LyricsFinder**: a tool that fetches song lyrics from online lyric services and writes them
back into MC's media library.

The same code ships in two guises:

| Edition | Assembly | Host |
|---|---|---|
| Stand-alone | `LyricsFinderExe.exe` | Windows Forms app |
| MC plug-in | `LyricsFinderPlugin.dll` | COM object embedded in Media Center |

Both are thin shells around `LyricsFinderCore`, a `UserControl` that contains the entire UI and
all the logic. `LyricsFinderPlugin` **inherits** directly from `LyricsFinderCore`, and
`MainForm` in the exe hosts the same control. The constructor flag
`base(isStandAlone, entryAssembly)` is the only real difference — among other things it picks
which log4net configuration is read.

### Solutions

| File | Contents |
|---|---|
| `LyricsFinder.sln` | **The active solution.** 15 code projects. |
| `MediaCenter.sln` | The "everything" solution. **Currently broken** — it points at `LyricsFinder\LyricServices\ApiseedsService\ApiseedsService.csproj`, which has been deleted and now only exists under `LyricServices.Old`. It also adds `Visualizations`. |

The other `.sln` files under `Lib/` and `C# Plugin Template/` are third-party SDK samples and
templates — not part of the product.

---

## 2. Projects and dependencies

Every project is a **classic (non-SDK) csproj**, `net48`, `AnyCPU`, using `packages.config` for
NuGet. `Directory.Build.props` sets exactly one thing globally: `LangVersion 13.0` — C# 13
against .NET Framework 4.8, which is supported but brings no runtime features.

```
                    ┌──────────────────┐        ┌────────────────────┐
                    │ LyricsFinderExe  │        │ LyricsFinderPlugin │
                    │   (WinExe)       │        │ (COM, inherits Core)│
                    └────────┬─────────┘        └─────────┬──────────┘
                             │                            │
                             └────────────┬───────────────┘
                                          ▼
                              ┌────────────────────────┐
                              │   LyricsFinderCore     │  ← UI + orchestration + data model
                              │  (Library, COM-visible)│
                              └───┬───────────┬────────┘
                                  │           │
                    ┌─────────────┘           └──────────────┐
                    ▼                                        ▼
          ┌──────────────────┐                    ┌────────────────────┐
          │    McWsProxy     │                    │ MessageInspection  │
          │ (MCWS REST client)│                   │   (WCF tracing)    │
          └────────┬─────────┘                    └─────────┬──────────┘
                   │                                        │
                   └──────────────┬─────────────────────────┘
                                  ▼
                        ┌──────────────────┐
                        │     Utility      │  ← strings, HTTP, async/UI helpers, XML
                        └──────────────────┘
                                  ▲
   ┌──────────────────────────────┴──────────────────────────────┐
   │  6 lyric service plug-ins (each: Library + MSTest in one dll)│
   │  AZLyrics · CajunLyrics · ChartLyrics · Lololyrics ·         │
   │  MusiXmatch · Stands4                                        │
   │  → reference Core + McWsProxy + Utility                      │
   └──────────────────────────────────────────────────────────────┘
```

Non-code projects:

| Project | Role |
|---|---|
| `Installation` | Console exe that **builds the release zips**. Also owns `SharedAssemblyInfo.cs` (shared version for all assemblies) and `Setup.iss` (Inno Setup). |
| `Documentation` | Empty `Main()`. Exists only to copy HTML documentation into `Build/`. |
| `MjpCreator` | Generates MC `.mjp` package files. **No longer used** — the call has been commented out in `Installation/Program.cs` since v1.3.1, because MC uses RegSvr32 rather than RegAsm. |
| `Visualizations` | ASP.NET-based; only in `MediaCenter.sln`. Independent of LyricsFinder. |

### Key architectural detail: lyric services are loaded dynamically

`LyricsFinderCore` does **not** reference the service projects. Instead
`InitLyricServicesAsync()` (`LyricsFinderCore.Private.cs:524`) scans the program folder, calls
`Assembly.LoadFrom()` on **every `*.dll`**, and instantiates every type deriving from
`AbstractLyricService`. The service projects, by contrast, hold a compile-time reference *to*
Core so they can inherit from it. The dependency is inverted at runtime — hence the
`ProjectDependencies` entries in the .sln file and the many `xcopy` post-build events.

---

## 3. Target frameworks, packages and central configuration

* **TFM:** `net48` throughout. `LangVersion 13.0`.
* **Signing:** `LyricsFinderCore`, `LyricsFinderPlugin`, `McWsProxy`, `Utility` and
  `MessageInspection` are strong-named. The `.snk` files live in the repo.
* **COM:** `LyricsFinderCore` and `LyricsFinderPlugin` are `ComVisible` with a `ProgId`.
  `RegisterForComInterop` is set in **Debug**, so Visual Studio must run as administrator for
  debug builds. `LyricsFinderPlugin` has a `COMReference` to MC's type library
  (GUID `{03457D73-…}`), which can only be resolved if Media Center is installed.

Runtime packages (development and analyzer packages omitted):

| Package | Version | Used by |
|---|---|---|
| `log4net` | 3.3.1 | Core |
| `Newtonsoft.Json` | 13.0.4 | Core, MusiXmatch |
| `HtmlAgilityPack` | 1.12.4 | AZLyrics, Lololyrics |
| `System.ComponentModel.Annotations` | 5.0.0 | Core |
| `Microsoft.CodeAnalysis.NetAnalyzers` | 10.0.203 | all |
| MSTest 4.2.1 + `Microsoft.NET.Test.Sdk` 18.4.0 | | the 6 service projects |

The service projects also drag in a **large transitive tail** via `Microsoft.NET.Test.Sdk` →
ApplicationInsights → Azure.Core → OpenTelemetry → `Microsoft.Extensions.*` 10.0.7 → MSAL.
Roughly 60 packages per service project, all of it a consequence of unit tests living in the
**same assembly** as the production code.

Central configuration:

| File | Meaning |
|---|---|
| `LyricsFinderCore/App.config` → `LyricsFinderCore.dll.config` | `localAppDataFile` = `%USERPROFILE%\Documents\LyricsFinder\LyricsFinder.xml` |
| `LyricsFinderCore/Log4net.Standalone.xml` / `Log4net.Plugin.xml` | log4net appenders; log files in the same folder |
| `LyricServices/*/App.config` | **Initial** seeding only, of `ServiceName`, `CreditUrl`, `Comment` and so on. After first start everything is read from the XML data file. |
| `Installation/Properties/SharedAssemblyInfo.cs` | `AssemblyVersion 1.3.7.0`, linked into Core, Exe and Plugin |

---

## 4. Architecture and data flow

### Startup

```
Program.Main / Plugin.Init
  → LyricsFinderCore.InitCoreAsync()
      1. InitLoggingAsync()          – picks the Standalone or Plugin log4net config
      2. LyricsFinderCoreConfigurationSectionHandler.Init()
      3. InitLocalDataAsync()
           a. ensure/verify write access to the data folder
           b. InitLyricServicesAsync()  → Assembly.LoadFrom on every *.dll
           c. LyricsFinderDataType.Load(xml)  → deserialize previously saved services
           d. merge in new services + RefreshServiceSettingsAsync() per service
      4. if MCWS setup is missing → OptionForm
      5. McRestService.Init(accessKey, url, user, password)   ← static fields
      6. ReloadPlaylistAsync(isReconnect: true)
```

### Connecting to Media Center

`McWsProxy` is a **static** class over MC's REST API (MCWS, default
`http://localhost:52199/MCWS/v1`).

```
ConnectAsync()
  → GET /Alive             (retry up to MaxMcWsConnectAttempts, 500 ms pause)
  → compare AccessKey against the configured one
  → GET /Authenticate      (Basic auth, user/password)  → McWsToken stored statically
  → every other call sends ?Token=<McWsToken> in the query string
```

### The search flow (the heart of the application)

```
SearchAllProcessAsync
  ├─ creates MaxQueueLength (default 10) "workers" over one shared Queue<int>
  └─ each worker:
       row = MainGridView.Rows[i]
       LyricSearch.SearchAsync(data, mcItem, …)
         ├─ for each active service: service.Clone()   ← clone per search, avoids shared state
         │     → clone.ProcessAsyncWrapper(mcItem, ct, isGetAll)
         │          → ProcessAsync (service-specific: HTTP/HTML scrape/SOAP/JSON)
         │          → if nothing found: retry with parenthesized text stripped from
         │            artist/album/title
         │          → AddFoundLyric() → FoundLyricList + hit counters
         ├─ isGetAll     : Task.WhenAll (all services in parallel)
         ├─ serial flag  : await each task in order, stop at the first hit
         └─ otherwise    : AsyncUtility.WhenAny(predicate) – first service with a hit wins
       finally: reload the XML data file, fold the clones' counters back, save the XML again
  └─ result is written into the DataGridView cell (not yet into Media Center)
```

Note that the search result lands **only in the grid**. Only when the user saves is
`McRestService.SetInfoAsync(key, "Lyrics", lyrics)` called
(`LyricsFinderCore.Private.cs:879`), which writes the text back to MC.

### Persistence

All user data — MCWS URL, user and password, window positions, grid columns, and **the entire
service list with its tokens and counters** — lives in a single XML file,
`%USERPROFILE%\Documents\LyricsFinder\LyricsFinder.xml`, serialized with `XmlSerializer` via
`SharedComponents.Serialize`. `LyricsFinderDataType.IsChanged` decides "dirty" by
**serializing the whole object to a string and comparing it against a stored baseline string**.

---

## 5. Technical risks, code smells and security

Ordered by how much they matter in practice.

### 5.1 `XmlSerializer` with `knownTypes` → assembly leak (high)

`Serialize.ToXmlWithNewlines` and `XmlDeserializeFromString` call
`new XmlSerializer(type, knownTypes)`. **Only** the `XmlSerializer(Type)` and
`XmlSerializer(Type, string)` overloads cache the generated serialization assembly; every other
overload generates a fresh dynamic assembly per call, which can never be unloaded.

The call path makes this serious: `IsChanged` serializes the whole data model on **every
read**, `SaveAsync` does it again, and `LyricSearch.SearchAsync` runs `Load` + `SaveAsync` in
its `finally` for **every single song**. A search across a 1000-track playlist therefore
produces thousands of dynamic assemblies plus as many full read/write round trips on the XML
file. Expected symptom: growing memory use and increasing sluggishness the longer a
"Search all" runs.

### 5.2 Counters may never be saved (high)

In `LyricSearch.SearchAsync` (`LyricSearch.cs:128`) the **parameter** `lyricsFinderData` is
**overwritten** with a fresh object read from disk. The clones' counters are then folded back
onto the `service` objects, which came from the **original** — now overwritten — instance,
while `SaveAsync()` is called on the **newly loaded** one. The increments to
`RequestCountTotal` / `HitCountTotal` are thus applied to objects that are never saved. This
should be verified against observed behaviour, but the code reads as a bug.

### 5.3 HttpClient is recreated on every authenticated call (high)

`SharedComponents.Utility.CreateHttpClient` is called from `HttpGetStringAsync` /
`HttpGetImageAsync` **every time** a username and password are supplied. It `Dispose()`s the
existing static `HttpClient` and creates a new one. Two problems:

* **Socket exhaustion / TIME_WAIT** — the classic HttpClient anti-pattern.
* **Race** — the field `_httpClientWithCredentials` is static and mutable. With two
  authenticated calls in flight (say `SetInfoAsync` across several rows), one can dispose the
  client the other is using → `ObjectDisposedException`.

Note also that `timeoutMilliSeconds` only takes effect for the *credentialed* client, and only
when it has just been recreated; `new TimeSpan(ms * 10000)` additionally overflows above
roughly 214 seconds.

### 5.4 No URL encoding in MCWS calls (high)

`McRestService.CreateRequestUrl` builds query strings by plain string interpolation. There is
**not a single** call to `Uri.EscapeDataString` or `HttpUtility.UrlEncode` anywhere in the
codebase. Worst case:

```csharp
case McCommandEnum.SetInfo:
    sb.Append($"…&Field={field}&Value={value}");   // value = the entire lyric text
```

A lyric containing `&`, `#`, `+` or `%` is either truncated or misparsed, and multi-KB texts
risk exceeding the server's URL length limit. `System.Web` is `using`'d in the file, but
`HttpUtility` is never used. Writing lyrics should be a POST with a body.

### 5.5 The threading model holds only by accident (medium-high)

`SearchAllProcessWorkerAsync` does three things that are normally illegal:

* `Queue<int>.Dequeue()` from ten concurrent workers without a lock — `Queue<T>` is not
  thread-safe.
* Direct access to `MainGridView.Rows[…]` and cell values from async workers.
* The `IsDataChanged` **getter** has side effects — it sets `FileSaveMenuItem.Enabled` and
  `DataChangedTextBox.Visible`.

It works in practice because none of the awaits on the path back to the worker use
`ConfigureAwait(false)`, so every continuation is posted back to the UI thread and thereby
serialized. But that is an invisible contract: **a single `ConfigureAwait(false)` added
anywhere in the `LyricSearch` or `AbstractLyricService` chain will break both the queue and
the grid access.** Use `ConcurrentQueue<int>.TryDequeue` and explicit UI marshalling instead of
relying on the synchronization context.

### 5.6 `WhenAny(predicate)` does not swallow exceptions as intended (medium)

`AsyncUtility.WhenAny` (`AsyncUtility.cs:378`) calls `condition(task)`, and the predicate in
`LyricSearch` is `t => t.Result.LyricResult == …`. On a faulted task, `.Result` throws an
**`AggregateException`**, not the original `LyricServiceBaseException`. So
`catch (LyricServiceBaseException)` at `LyricSearch.cs:112` never fires, and a quota overrun on
the parallel path propagates as an unexpected exception type. Same pattern on the serial path
(`LyricSearch.cs:102`, `task.Result` after `await`).

### 5.7 CR+LF does not survive the configuration file (medium)

Multi-line values lose their line breaks. This hits both when `LyricsFinder.xml` is created for
the first time and on subsequent saves.

**The mechanism** is XML's attribute-value normalization rule (XML 1.0 §3.3.3): a *literal* CR,
LF or TAB inside an **attribute value** is replaced by a single space by any conforming parser.
The line break is gone the moment the value is read, and cannot be recovered afterwards. Only
the character references `&#xD;` / `&#xA;` survive. The service settings live in exactly such
attributes — `<add key="Comment" value="…" />` — and are therefore exposed.

**The evidence is in the repo.** MusiXmatch writes its `Comment` using entities and makes it
all the way through to the data file:

```xml
<!-- MusiXmatchService/App.config -->
value="No user ID required.&#xD;&#xA;Token required, may be obtained from:&#xD;&#xA;…"
```
```xml
<!-- LyricsFinder.xml, lines 118-121 -->
<Comment>No user ID required.&#xD;
Token required, may be obtained from:&#xD;
…</Comment>
```

AZLyrics and Stands4 write raw line breaks inside the attribute, and their text now carries
**double spaces** exactly where the line breaks used to be — the signature of CR and LF each
being turned into one space:

```
"No user ID or token required.··Use this service for manual lyric searches only,··as automatic…"
```

This is already true in `HEAD`, not just in the working tree: the damage happened at some
earlier point and was committed, so the three `App.config` files have themselves become the
corrupted source.

**The byte counts confirm the write path itself is sound.** Counting the current data file
(`%USERPROFILE%\Documents\LyricsFinder\LyricsFinder.xml`, 9257 bytes):

| Measurement | Count |
|---|---|
| `&#xD;` entities | 12 |
| Literal CR bytes | 160 |
| Literal LF bytes | 172 |

All 160 CRs are part of CR+LF pairs and come from `Indent = true`, whose `NewLineChars`
defaults to `"\r\n"` — that is pure pretty-printing between elements. The remaining
`172 − 160 = 12` lone LFs match the 12 entities **exactly**: every embedded line break in text
content is written as `&#xD;` plus a literal LF, precisely as `NewLineHandling.Entitize`
prescribes. None of them were lost.

The conclusion is therefore unambiguous: `XmlSerializeToFile` does the right thing, and the
fault lies entirely upstream in the `App.config` attributes. That is also why a fix in the
serialization code will not help — the source is what needs repairing.

**Consequences for the two symptoms observed:**

* *New file* — `AbstractLyricService.RefreshServiceSettingsAsync` seeds `Comment`,
  `CreditTextFormat` and others from `App.config` via `ServiceSettingsValue`. The parser hands
  back already-flattened text, so a fresh `LyricsFinder.xml` inherits the flattening. The data
  file's write path is itself correct — the source was destroyed beforehand.
* *Saving changes* — as long as the values are written back into an attribute without
  entitizing, they are flattened again on the next read. The round trip is lossy every time.

Note where this hurts: `CreditTextFormat` is the block appended to **every single saved lyric**
in the media library. It is intact in the current data file (`&#xD;` on lines 125-133), but it
sits in the same attribute layer and is one flattening accident away from losing its line
breaks in everything that gets saved.

**Direction for a fix.** The quick way out is to use `&#xD;&#xA;` in all `App.config`
attributes, as MusiXmatch already does. That works, but the entities must be maintained by hand
forever — a developer who presses Enter inside an attribute destroys the value without noticing.

The durable fix is to move the multi-line settings out of attributes and into child elements.
Note that the rule there is a different one: in **element content**, CR+LF is normalized to a
single LF (XML 1.0 §2.11) — so the line break *survives*, it just becomes LF instead of CR+LF.
That is the decisive difference from attributes, where the break disappears entirely.

*Before — the break dies on read:*

```xml
<appSettings>
  <add key="Comment" value="No user ID required.
Token required, may be obtained from:
https://about.musixmatch.com/api-pricing" />
</appSettings>
```

*After — the break survives as LF:*

```xml
<lyricService>
  <ServiceName>MusiXmatch</ServiceName>
  <Comment>No user ID required.
Token required, may be obtained from:
https://about.musixmatch.com/api-pricing</Comment>
</lyricService>
```

The read side gets simpler at the same time. These settings are only used for seeding at first
start, so the whole `ConfigurationManager` machinery can go in favour of plain XML reading:

```csharp
// Replaces ConfigurationManager.OpenExeConfiguration(assy.Location) in RefreshServiceSettingsAsync
private static Dictionary<string, string> ReadServiceSettings(Type serviceType)
{
    var configPath = Assembly.GetAssembly(serviceType).Location + ".config";
    var ret = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
    var doc = new XmlDocument { XmlResolver = null };

    using (var reader = XmlReader.Create(configPath, new XmlReaderSettings { XmlResolver = null }))
        doc.Load(reader);

    foreach (XmlNode node in doc.SelectNodes("/configuration/lyricService/*"))
    {
        // Element content comes back with LF; WinForms wants CR+LF.
        ret[node.Name] = node.InnerText.LfToCrLf();
    }

    return ret;
}
```

Two points about that shape: `LfToCrLf()` already exists in `SharedComponents.Utility` and does
exactly that last step, and `XmlResolver = null` keeps the XXE line consistent with the rest of
the codebase.

Be aware that `Utility.DllConfigurationFile()` *cannot* be used here — it calls
`Assembly.GetExecutingAssembly()` and therefore points at `Utility.dll`, not at the service
being read. `Assembly.GetAssembly(serviceType)` is the correct form, and it is also what
`RefreshServiceSettingsAsync` already uses.

Whichever route is chosen: repair the three committed `App.config` files at the same time.
Their line breaks cannot be inferred from the code and must be typed back in by hand.

### 5.8 Two divergent serialization paths (medium, needs verifying)

Related to the above, and worth looking at in the same pass. `Serialize` has two write paths
that do not use the same serializer:

* `XmlSerializeToFile` uses `objectInstance.GetType()` — the concrete type.
* `XmlSerializeToString` calls `ToXmlWithNewlines<T>(this T obj, …)` with `objectInstance`
  **statically typed as `object`**. Type inference therefore binds `T` to `object`, and
  `new XmlSerializer(typeof(object), knownTypes)` is constructed — a different serializer from
  the one the file path uses.

The consequence is that the `IsChanged` baseline (`InitialXml`) is produced by a different
serializer from the one that writes the file. The two are internally consistent, so dirty
detection probably works — but if the `object`-typed serializer omits fields that the concrete
one includes, changes to precisely those fields will go undetected and `SaveAsync` will
silently skip the save (`if (IsChanged && IsSaveOk)`). This is a plausible contributing cause
of "changes are not saved", but I have not verified which fields actually differ.

### 5.9 Security

| Topic | Assessment |
|---|---|
| **MCWS password in cleartext** | `MainDataType.McWsPassword` is an `[XmlElement]` and is stored unencrypted in `LyricsFinder.xml` under `Documents`. The same goes for the Stands4 `Token`/`UserId` and the MusiXmatch token. Consider DPAPI (`ProtectedData`) or Windows Credential Manager. |
| **Token in the query string** | `McWsToken` is sent as `?Token=…` on every URL. Against `localhost` the exposure is limited, but tokens end up in log files: `HttpGetStringAsync` embeds the full URL in its exception messages, which are written to `LyricsFinder.*.error.log`. |
| **`Assembly.LoadFrom` on every `*.dll`** | Any DLL placed in the program folder is loaded and instantiated. The folder is `Program Files` and requires admin rights, so this is not an acute vulnerability — but there is no strong-name or signature validation of "plug-ins". |
| **`.snk` files in the repo** | The strong-name keys are committed. Strong naming is not a security boundary, but anyone can now sign assemblies with the same identity. |
| **XXE** | Handled correctly — `XmlResolver = null` is set consistently in `McMplResponse`, `McPlayListsResponse` and `LyricsFinderDataType`. |
| **HTML scraping** | AZLyrics and Lololyrics parse foreign HTML with HtmlAgilityPack and put the result into a `DataGridView`. The content is never rendered as HTML, so the XSS risk is low. |

### 5.10 Build setup

* **Hard-coded Visual Studio paths.** `BuildRelease.subroutine.cmd:17-18` only looks in
  `D:\Program Files [(x86)]\Microsoft Visual Studio\{2017,2019,2022,2026}\Community\…`. The
  script does not work on a machine where VS sits on C:, or with Professional/Enterprise. Use
  `vswhere.exe`.
* **`del /s /q "$(TargetDir)*.*"` as a pre-build** in Core, Exe and Plugin, combined with
  post-build `xcopy` chains that copy across project folders and into
  `Installation\Build\{Plugin,Standalone,LyricServices}`. `LyricsFinderCore` **empties**
  `Build\LyricServices` in its post-build. The whole thing depends on the build order in the
  `.sln` file's `ProjectDependencies` and is neither incremental-safe nor parallel-build-safe.
* **Binding redirects do not match the package versions.** `LyricsFinderCore/App.config`
  redirects `Newtonsoft.Json` to `12.0.0.0`, while `packages.config` has 13.0.4 (assembly
  version `13.0.0.0`). Likewise for `System.Buffers` (`4.0.3.0` vs. package 4.6.1) and
  `System.Numerics.Vectors`. Can produce a `FileLoadException` at runtime.
* **`MediaCenter.sln` is broken** — missing `ApiseedsService.csproj`, see §2.
* **No CI.** `.github/` contains only `ISSUE_TEMPLATE`. No workflows.
* **Version inconsistency.** `SharedAssemblyInfo.cs` = 1.3.7.0; `Setup.iss` `AppVersion` and
  the registry `Version` = hard-coded 1.0.0; `ReleaseNotes.html` tops out at v1.3.1; the latest
  commit is titled "Starting v1.4.0 changes".
* **`Setup.iss`: `IsMc32()` can never return `true`.** `McVersion` is set to
  `Copy(McInstVersions[Idx], 1, 2)` — just the two version digits, e.g. `"34"` — but `IsMc32`
  tests `Pos('32-bit', McVersion)`. A 32-bit MC installation therefore gets 64-bit `RegAsm` and
  `HKLM64` registry keys. If the user selects only the `standalone` component, the version page
  is skipped and `NextButtonClick` never sets `InstallDir`.
* **`BuildAndInstallLyricsFinder.cmd`**: `:wait / if exist %_out_file% goto :wait` loops for as
  long as the file **exists** — the logic looks inverted relative to the comment "Waiting for
  the build completion".

### 5.11 Other code smells

* **`LyricsFinderCore` is a prime candidate for splitting up**: `LyricsFinderCore.cs` (1369
  lines) + `.Private.cs` (1296) + `.Process.cs` (486) in one partial class that mixes UI, MCWS
  orchestration, file system and business logic. This is the main reason no unit tests exist
  for the core.
* **`StackTrace` inside a property getter.**
  `LyricsFinderCoreConfigurationSectionHandler.Instance` builds a `new StackTrace()` and reads
  `GetFrame(1).GetMethod().Name` on **every** access, to work out who is calling. Expensive,
  and unreliable as soon as the JIT inlines.
* **`Utility.GetLinkerTime`** reads the PE header timestamp. With deterministic builds
  (`<Deterministic>true</Deterministic>` in `McWsProxy`; the Roslyn default) that field is a
  hash, not a date — so the function yields meaningless "build times".
* **`RandomizedDelayAsync`** creates `new Random()` per call. On .NET Framework it is seeded
  from the system clock, so concurrent calls within the same tick get an **identical** delay —
  which defeats the whole purpose of spreading requests out to avoid rate limiting.
* **`AbstractLyricService._semaphoreSlim` is `static`.** Counters for **all** services are
  serialized through one global semaphore, even though the lock only protects instance fields.
* **`AbstractLyricService.ProcessAsyncWrapper`** accesses `ret.LyricResult` without a null
  check — an override returning `null` yields an NRE.
* **`InitLocalDataAsync`**: `Path.Combine(DataDirectory, dataFile + ".tmp")` where `dataFile`
  is already an absolute path. It works, because `Combine` ignores the first argument when the
  second is rooted, but it is unintentional and confusing.
* **Dead code.** `LyricServices.Old/` (ApiseedsService, LyricWikiService) is in no solution.
  `McPlayControlForm.cs` (411 lines) sits in the Core folder but is **not** listed in
  `LyricsFinderCore.csproj` and is therefore never compiled. `MjpCreator` is out of service.
  `Serialize.cs` carries the comment "Not used in this solution version" while actually being
  central.
* **Generated files in the source tree.** `MessageInspection.dll/.pdb/.xml`,
  `*.GeneratedMSBuildEditorConfig.editorconfig` and `*.CodeAnalysisLog.xml` sit alongside the
  source in every service folder, copied there by `PostBuild.cmd`. They are not git-tracked,
  but they clutter the listing.
* **Exception discipline.** Around 50 occurrences of `throw new Exception(...)` — hard to catch
  selectively. There is, by contrast, a well-developed `LyricServiceExceptions.cs` hierarchy
  that simply is not used consistently.
* **95 `async void` methods.** Almost all are WinForms event handlers and therefore
  conventional, but `LyricsFinderExe/Program.cs` installs **neither**
  `Application.ThreadException` nor `AppDomain.UnhandledException`. An exception escaping an
  `async void` handler closes the program without a log entry.

---

## 6. Test gaps

There are **28 `[TestMethod]`** in total — all in the six lyric service projects, and all of
them **integration tests against live third-party websites**:

| Project | Tests |
|---|---|
| Lololyrics | 10 |
| CajunLyrics | 8 |
| AZLyrics | 4 |
| ChartLyrics / MusiXmatch / Stands4 | 2 each |

They carry three hard out-of-process dependencies:

1. **Network and third-party services.** The tests look up hard-coded songs ("Bruce Daigrepont
   – La Jalouserie") and fail when a site changes its markup, goes down, or rate-limits. The
   AZLyrics configuration itself warns that automatic searching can get your IP banned.
2. **The user's real data file.** `[TestInitialize]` calls
   `LyricsFinderDataType.GetLyricService<T>()`, which reads
   `%USERPROFILE%\Documents\LyricsFinder\LyricsFinder.xml`. The tests therefore **cannot** run
   on a clean machine or in CI, and they depend on the user having configured real API tokens
   for Stands4 and MusiXmatch.
3. **Test code inside the production assembly.** The MSTest references live in the service DLLs
   themselves, which consequently ship with the whole test infrastructure. The post-build script
   removes `Microsoft.VisualStudio.TestPlatform*` by hand.

**Entirely untested:**

| Area | Comment |
|---|---|
| `SharedComponents.Utility` | Around 30 pure string functions (`ToSentenceCase`, `ToTitleCase`, `RemoveParenthesizedText`, `ToNormalizedLineEndings`, `TrimStringLines`) — trivial to test, and with known edge cases: `ToTitleCase` indexes `lines[i][0]`, and `CapitalizeWordTitle` throws on words containing spaces. |
| `Serialize` / `LyricsFinderDataType` | Round trip, the v1.1→v1.2 migration (`<Service>` → `<LyricService>`), `IsChanged` semantics. |
| `McWsProxy` | `CreateRequestUrl` per command — exactly where the encoding bug sits. Testable entirely without a network. |
| `LyricSearch` | The orchestration: parallel vs. serial, first-hit, counter fold-back. No network needed with fake services. |
| `AbstractLyricService` | Quota handling, retry-without-parentheses, duplicate filtering in `AddFoundLyric`. |
| `LyricsFinderCore` | None — and the code is not structured for it. |
| `Installation` / `Setup.iss` | None. |

**Recommended order** if tests are to be invested in: (1) a separate `*.Tests` project per
layer, so MSTest leaves the production assemblies; (2) pure unit tests of `Utility`,
`McRestService.CreateRequestUrl` and `Serialize` — fast, hermetic, and covering two of the
suspected bugs above; (3) `LyricSearch` with in-memory fakes; (4) move the existing network
tests into a separate category that is disabled by default.

---

## 7. Build, test and run commands

### Prerequisites

* Windows, Visual Studio 2022 or newer with the **.NET desktop development** workload —
  classic csproj files and the WinForms designer. `dotnet build` alone is **not** enough.
* .NET Framework 4.8 Developer Pack.
* JRiver Media Center installed — `LyricsFinderPlugin` has a `COMReference` to MC's type
  library and cannot be built without it.
* Debug builds set `RegisterForComInterop` → **run Visual Studio as administrator**.

### Restore and build

NuGet restore must go through `nuget.exe` or `msbuild -t:Restore`, not `dotnet restore` — the
projects use the `packages.config` format:

```powershell
# from the repo root
nuget restore LyricsFinder.sln
msbuild LyricsFinder.sln -p:Configuration=Debug   -p:Platform="Any CPU"
msbuild LyricsFinder.sln -p:Configuration=Release -p:Platform="Any CPU"
```

Locate `msbuild` without hard-coding the path:

```powershell
$vs = & "${env:ProgramFiles(x86)}\Microsoft Visual Studio\Installer\vswhere.exe" `
        -latest -requires Microsoft.Component.MSBuild -property installationPath
& "$vs\MSBuild\Current\Bin\MSBuild.exe" LyricsFinder.sln -p:Configuration=Release
```

The project's own scripts — these require VS to sit on `D:\`, see §5.10:

```
LyricsFinder\Installation\BuildRelease.cmd                   # elevated clean+release build
LyricsFinder\Installation\BuildAndInstallLyricsFinder.cmd    # build, then Output\Setup.exe
```

Note that `BuildRelease.subroutine.cmd` runs `-t:Clean,Build`, and that several projects carry
`del /s /q "$(TargetDir)*.*"` as a pre-build — building is destructive to output folders.

### Tests

```powershell
vstest.console.exe `
  LyricsFinder\LyricServices\CajunLyricsService\bin\Debug\CajunLyricsService.dll `
  LyricsFinder\LyricServices\LololyricsService\bin\Debug\LololyricsService.dll `
  LyricsFinder\LyricServices\AZLyricsService\bin\Debug\AZLyricsService.dll `
  LyricsFinder\LyricServices\ChartLyricsService\bin\Debug\ChartLyricsService.dll `
  LyricsFinder\LyricServices\MusiXmatchService\bin\Debug\MusiXmatchService.dll `
  LyricsFinder\LyricServices\Stands4Service\bin\Debug\Stands4Service.dll
```

Or through Test Explorer in Visual Studio. Remember the prerequisites from §6: network, an
existing `%USERPROFILE%\Documents\LyricsFinder\LyricsFinder.xml`, and valid tokens for Stands4
and MusiXmatch. Tests in CI will fail.

### Running

**Stand-alone:**

```
LyricsFinder\LyricsFinderExe\bin\Debug\LyricsFinderExe.exe
```

The first start shows `OptionForm`, where the MCWS URL, Access Key, username and password must
be filled in. The Access Key is found in Media Center under *Tools → Options → Media Network*.

**As an MC plug-in:** requires COM registration and a registry key under
`HKLM\SOFTWARE\J. River\Media Center <version>\Plugins\Interface\LyricsFinder` — use the
installer (`Installation\Output\Setup.exe`) rather than doing it by hand.

### Logs and data

```
%USERPROFILE%\Documents\LyricsFinder\LyricsFinder.xml                    # all user data
%USERPROFILE%\Documents\LyricsFinder\LyricsFinder.Standalone.log         # info
%USERPROFILE%\Documents\LyricsFinder\LyricsFinder.Standalone.error.log   # warn+
%USERPROFILE%\Documents\LyricsFinder\LyricsFinder.Plugin[.error].log
```

Delete `LyricsFinder.xml` to reset to factory settings — the services are recreated from the
`App.config` seeds on the next start.

---

## 8. If I had to prioritize

1. **URL encoding in `McRestService`** (§5.4) — a small fix for a directly user-visible bug.
2. **Cache the `XmlSerializer` instances** (§5.1) and drop the per-song `Load` + `Save` from
   `LyricSearch`'s `finally` — that is both the leak and the biggest performance problem.
3. **Verify the counter logic** in `LyricSearch.SearchAsync` (§5.2).
4. **One static `HttpClient`** with `HttpRequestMessage`-based auth instead of recreating the
   client per call (§5.3).
5. **Move multi-line settings out of XML attributes** (§5.7) and repair the three `App.config`
   files whose line breaks have already been flattened. Attribute normalization is lossy on
   every single read, and `CreditTextFormat` ends up in every saved lyric.
6. **`ConcurrentQueue` plus explicit UI marshalling** in the search workers (§5.5), so the code
   does not depend on an invisible contract.
7. **Clean up the build**: `vswhere` instead of `D:\` paths, fix the binding redirects, repair
   or remove `MediaCenter.sln`, delete `LyricServices.Old` and `McPlayControlForm.cs`.
8. **Separate test projects** plus unit tests of `Utility`, `CreateRequestUrl` and `Serialize`,
   so there is a safety net to refactor against at all. A round-trip test of multi-line text
   through `App.config` → data file → `App.config` would have caught §5.7.
