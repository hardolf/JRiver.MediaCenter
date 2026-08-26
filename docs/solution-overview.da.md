# LyricsFinder / JRiver.MediaCenter — arkitektur- og onboarding-rapport

Status: gennemgang pr. 2026-08-25 på branch `master`, statusopdateret 2026-08-26.
§5.1, §5.4, §5.8 og §5.9 er rettet og testet; resten står i [afsnit 9](#9-udestående).

> Dansk udgave. Den engelske oversættelse ligger i [`solution-overview.en.md`](solution-overview.en.md).

---

## 1. Hvad er det her?

Repoet indeholder udvidelser til **JRiver Media Center** (MC). Langt hovedparten er
**LyricsFinder**: et værktøj der henter sangtekster fra online lyric-tjenester og skriver dem
tilbage i MC's mediebibliotek.

Samme kode udgives i to skikkelser:

| Udgave | Assembly | Vært |
|---|---|---|
| Stand-alone | `LyricsFinderExe.exe` | Windows Forms-app |
| MC plug-in | `LyricsFinderPlugin.dll` | COM-objekt indlejret i Media Center |

Begge er tynde skaller omkring `LyricsFinderCore`, en `UserControl` der indeholder hele UI'et
og al logik. `LyricsFinderPlugin` **arver** direkte fra `LyricsFinderCore`, og `MainForm` i
exe'en hoster den samme kontrol. Konstruktørflaget `base(isStandAlone, entryAssembly)`
er den eneste reelle forskel (bl.a. hvilken log4net-konfiguration der læses).

### 1.1 Solutions

| Fil | Indhold |
|---|---|
| `LyricsFinder.sln` | **Den aktive solution.** 15 kodeprojekter. |
| `MediaCenter.sln` | "Alt"-solution. **Er i øjeblikket brudt** — den peger på `LyricsFinder\LyricServices\ApiseedsService\ApiseedsService.csproj`, som er slettet (ligger nu kun i `LyricServices.Old`). Tilføjer desuden `Visualizations`. |

Øvrige `.sln`-filer under `Lib/` og `C# Plugin Template/` er tredjeparts SDK-eksempler og
skabeloner — ikke en del af produktet.

---

## 2. Projekter og afhængigheder

Alle projekter er **klassiske (non-SDK) csproj'er**, `net48`, `AnyCPU`, med `packages.config`
til NuGet. `Directory.Build.props` sætter kun én ting globalt: `LangVersion 13.0`
(C# 13 mod .NET Framework 4.8 — understøttet, men uden runtime-features).

```
                    ┌──────────────────┐        ┌────────────────────┐
                    │ LyricsFinderExe  │        │ LyricsFinderPlugin │
                    │   (WinExe)       │        │  (COM, arver Core) │
                    └────────┬─────────┘        └─────────┬──────────┘
                             │                            │
                             └────────────┬───────────────┘
                                          ▼
                              ┌────────────────────────┐
                              │   LyricsFinderCore     │  ← UI + orkestrering + datamodel
                              │  (Library, COM-synlig) │
                              └───┬───────────┬────────┘
                                  │           │
                    ┌─────────────┘           └──────────────┐
                    ▼                                        ▼
          ┌──────────────────┐                    ┌────────────────────┐
          │    McWsProxy     │                    │ MessageInspection  │
          │ (MCWS REST-klient)│                   │  (WCF-tracing)     │
          └────────┬─────────┘                    └─────────┬──────────┘
                   │                                        │
                   └──────────────┬─────────────────────────┘
                                  ▼
                        ┌──────────────────┐
                        │     Utility      │  ← strenge, HTTP, async/UI-helpers, XML
                        └──────────────────┘
                                  ▲
   ┌──────────────────────────────┴──────────────────────────────┐
   │  6 lyric-service-plugins (hver: Library + MSTest i samme dll)│
   │  AZLyrics · CajunLyrics · ChartLyrics · Lololyrics ·         │
   │  MusiXmatch · Stands4                                        │
   │  → refererer Core + McWsProxy + Utility                      │
   └──────────────────────────────────────────────────────────────┘
```

### 2.1 Ikke-kode-projekter

| Projekt | Rolle |
|---|---|
| `Installation` | Console-exe der **pakker release-zips**. Ejer også `SharedAssemblyInfo.cs` (delt version for alle assemblies) og `Setup.iss` (Inno Setup). |
| `Documentation` | Tom `Main()`. Findes kun for at kopiere HTML-dokumentation til `Build/`. |
| `MjpCreator` | Genererer MC `.mjp`-pakkefiler. **Ikke længere brugt** (kaldet er udkommenteret i `Installation/Program.cs` siden v1.3.1 — MC bruger RegSvr32, ikke RegAsm). |
| `Visualizations` | ASP.NET-baseret; kun i `MediaCenter.sln`. Uafhængig af LyricsFinder. |

### 2.2 Vigtig arkitekturdetalje: lyric-services indlæses dynamisk

`LyricsFinderCore` refererer **ikke** service-projekterne. I stedet scanner
`InitLyricServicesAsync()` (`LyricsFinderCore.Private.cs:524`) programmappen, kalder
`Assembly.LoadFrom()` på **hver `*.dll`**, og instantierer alle typer der nedarver fra
`AbstractLyricService`. Service-projekterne har derimod compile-time-reference *til* Core
(for at kunne arve). Afhængigheden er altså vendt om ved runtime — deraf `ProjectDependencies`
i .sln-filen og de mange `xcopy`-post-build-events.

---

## 3. Target frameworks, pakker og central konfiguration

* **TFM** (Target Framework Moniker): `net48` overalt. `LangVersion 13.0`.
* **Signering:** `LyricsFinderCore`, `LyricsFinderPlugin`, `McWsProxy`, `Utility`,
  `MessageInspection` er strong-named. `.snk`-filerne ligger i repoet.
* **COM:** `LyricsFinderCore` og `LyricsFinderPlugin` er `ComVisible` med `ProgId`.
  `RegisterForComInterop` er sat i **Debug** — dvs. Visual Studio skal køre som administrator
  ved debug-builds. `LyricsFinderPlugin` har en `COMReference` til MC's type-library
  (GUID `{03457D73-…}`), som kun kan resolves hvis Media Center er installeret.

### 3.1 Runtime-pakker

*Udviklings- og analyzer-pakker er udeladt.*

| Pakke | Version | Bruges af |
|---|---|---|
| `log4net` | 3.3.1 | Core |
| `Newtonsoft.Json` | 13.0.4 | Core, MusiXmatch |
| `HtmlAgilityPack` | 1.12.4 | AZLyrics, Lololyrics |
| `System.ComponentModel.Annotations` | 5.0.0 | Core |
| `Microsoft.CodeAnalysis.NetAnalyzers` | 10.0.203 | alle |
| MSTest 4.2.1 + `Microsoft.NET.Test.Sdk` 18.4.0 | | de 6 service-projekter |

Service-projekterne trækker desuden en **stor transitiv hale** ind via
`Microsoft.NET.Test.Sdk` → ApplicationInsights → Azure.Core → OpenTelemetry →
`Microsoft.Extensions.*` 10.0.7 → MSAL. Ca. 60 pakker pr. serviceprojekt, alle sammen
konsekvens af at unit-tests bor i **samme assembly** som produktionskoden.

### 3.2 Central konfiguration

| Fil | Betydning |
|---|---|
| `LyricsFinderCore/App.config` → `LyricsFinderCore.dll.config` | `localAppDataFile` = `%USERPROFILE%\Documents\LyricsFinder\LyricsFinder.xml` |
| `LyricsFinderCore/Log4net.Standalone.xml` / `Log4net.Plugin.xml` | log4net-appenders, logfiler i samme mappe |
| `LyricServices/*/App.config` | Kun **initiel** seeding af `ServiceName`, `CreditUrl`, `Comment` osv. Efter første start læses alt fra XML-datafilen. |
| `Installation/Properties/SharedAssemblyInfo.cs` | `AssemblyVersion 1.3.7.0` (linket ind i Core, Exe og Plugin) |

---

## 4. Arkitektur og dataflow

### 4.1 Opstart

```
Program.Main / Plugin.Init
  → LyricsFinderCore.InitCoreAsync()
      1. InitLoggingAsync()          – vælger Standalone- eller Plugin-log4net-config
      2. LyricsFinderCoreConfigurationSectionHandler.Init()
      3. InitLocalDataAsync()
           a. sikrer/tester skriveadgang til datamappen
           b. InitLyricServicesAsync()  → Assembly.LoadFrom på alle *.dll
           c. LyricsFinderDataType.Load(xml)  → deserialiser tidligere gemte services
           d. flet nye services ind + RefreshServiceSettingsAsync() pr. service
      4. hvis MCWS-opsætning mangler → OptionForm
      5. McRestService.Init(accessKey, url, user, password)   ← statiske felter
      6. ReloadPlaylistAsync(isReconnect: true)
```

### 4.2 Forbindelse til Media Center

`McWsProxy` er en **statisk** klasse over MC's REST-API (MCWS, default
`http://localhost:52199/MCWS/v1`).

```
ConnectAsync()
  → GET /Alive             (retry op til MaxMcWsConnectAttempts, 500 ms pause)
  → sammenlign AccessKey med den konfigurerede
  → GET /Authenticate      (Basic auth, bruger/password)  → McWsToken gemmes statisk
  → alle øvrige kald sender ?Token=<McWsToken> i query-strengen
```

### 4.3 Søgeforløbet

```
SearchAllProcessAsync
  ├─ opretter MaxQueueLength (default 10) "workers" over én delt Queue<int>
  └─ hver worker:
       row = MainGridView.Rows[i]
       LyricSearch.SearchAsync(data, mcItem, …)
         ├─ for hver aktiv service: service.Clone()   ← klon pr. søgning, undgår delt state
         │     → clone.ProcessAsyncWrapper(mcItem, ct, isGetAll)
         │          → ProcessAsync (service-specifik: HTTP/HTML-scrape/SOAP/JSON)
         │          → hvis intet fundet: prøv igen uden parentes-tekst i artist/album/titel
         │          → AddFoundLyric() → FoundLyricList + hit-tællere
         ├─ isGetAll        : Task.WhenAll (alle services parallelt)
         ├─ serielt-flag    : await hver task i rækkefølge, stop ved første hit
         └─ ellers          : AsyncUtility.WhenAny(predicate) – første service med hit vinder
       finally: genindlæs XML-datafil, læg klonernes tællere tilbage, gem XML igen
  └─ resultat skrives i DataGridView-cellen (endnu ikke i Media Center)
```

Bemærk: søgeresultatet lander **kun i gridet**. Først når brugeren gemmer, kaldes
`McRestService.SetInfoAsync(key, "Lyrics", lyrics)` (`LyricsFinderCore.Private.cs:879`)
som skriver teksten tilbage til MC.

### 4.4 Persistering

Al brugerdata — MCWS-URL/bruger/password, vinduespositioner, gridkolonner, og **hele
service-listen med tokens og tællere** — ligger i én XML-fil,
`%USERPROFILE%\Documents\LyricsFinder\LyricsFinder.xml`, serialiseret med `XmlSerializer`
via `SharedComponents.Serialize`. `LyricsFinderDataType.IsChanged` afgør "dirty" ved at
**serialisere hele objektet til en streng og sammenligne med en gemt baseline-streng**.

---

## 5. Tekniske risici, code smells og sikkerhed

Sorteret efter hvor meget de betyder i praksis.

### 5.1 `XmlSerializer` med `knownTypes` → assembly-læk (høj)

> <span style="color:#0A6E85">**&#10004; Løst 2026-08-26.**</span> Alle fire konstruktionssteder i `SharedComponents/Utility/Serialize.cs` går nu
> gennem en privat `GetSerializer(Type, Type[])` med en statisk
> `ConcurrentDictionary<string, XmlSerializer>`, nøglet på `AssemblyQualifiedName`. Fordi den
> cachede instans er delt, til- og afmelder `XmlDeserializeFromString` nu `UnknownElement`
> under `lock (serializer)` med en `finally`. Kommentaren “Not used in this solution version”
> øverst i filen er fjernet.
>
> Præcisering: lækket ramte også `Stands4Service`. Fordi `knownTypes` er `params`, rammer selv
> et *tomt* array den ikke-cachende overload — rettelsen ruter derfor en tom liste til
> `XmlSerializer(Type)`. Det per-sang `Load` + `SaveAsync` i `LyricSearch`'s `finally` står
> stadig; se §9.1.

`Serialize.ToXmlWithNewlines` og `XmlDeserializeFromString` kalder
`new XmlSerializer(type, knownTypes)`. **Kun** overloads `XmlSerializer(Type)` og
`XmlSerializer(Type, string)` cacher den genererede serialiserings-assembly; alle andre
overloads genererer en ny dynamisk assembly pr. kald, som aldrig kan unloades.

Kaldsstien gør det alvorligt: `IsChanged` serialiserer hele datamodellen ved **hvert opslag**,
`SaveAsync` gør det igen, og `LyricSearch.SearchAsync` kører `Load` + `SaveAsync` i sin
`finally` for **hver eneste sang**. En søgning over en playliste på 1000 numre giver derfor
i størrelsesordenen tusindvis af dynamiske assemblies plus lige så mange fulde
læse/skrive-runder på XML-filen. Forventet symptom: voksende hukommelsesforbrug og
stigende langsommelighed jo længere en "Search all" kører.

### 5.2 Tællere gemmes muligvis aldrig (høj)

I `LyricSearch.SearchAsync` (`LyricSearch.cs:128`) **overskrives parameteren**
`lyricsFinderData` med et frisk objekt fra disk. Derefter lægges klonernes tællere tilbage på
`service`-objekterne, som stammer fra den **oprindelige** (nu overskrevne) instans — men
`SaveAsync()` kaldes på den **nyindlæste** instans. Forøgelserne af `RequestCountTotal` /
`HitCountTotal` skrives altså til objekter der ikke gemmes. Dette bør verificeres mod
faktisk observeret adfærd, men koden læser som en fejl.

### 5.3 HttpClient genskabes ved hvert autentificeret kald (høj)

`SharedComponents.Utility.CreateHttpClient` bliver kaldt fra `HttpGetStringAsync` /
`HttpGetImageAsync` **hver gang** der sendes brugernavn/password. Den `Dispose()`'r den
eksisterende statiske `HttpClient` og laver en ny. To problemer:

* **Socket exhaustion / TIME_WAIT** — det klassiske HttpClient-antimønster.
* **Race** — feltet `_httpClientWithCredentials` er statisk og muterbart. Kører to
  autentificerede kald samtidig (fx `SetInfoAsync` på flere rækker), kan det ene kald
  disposere den klient det andet er ved at bruge → `ObjectDisposedException`.

Bemærk også at `timeoutMilliSeconds` kun har effekt for den *credentialed* klient og kun
når den lige er blevet genskabt; `TimeSpan(ms * 10000)` overløber desuden ved
værdier over ca. 214 sekunder.

### 5.4 Ingen URL-encoding i MCWS-kald (høj)

> <span style="color:#0A6E85">**&#10004; Løst 2026-08-26.**</span> En ny `UrlEncoded()`-extension i
> `SharedComponents/Utility/Utility.cs` anvendes nu på hver interpoleret strengværdi i
> `McRestService.CreateRequestUrl` — `McWsToken` samt `field`/`value` i `SetInfo` og
> `GetInfo`/`GetInfoFull`. Det ubrugte `using System.Web` er fjernet. Verificeret mod en
> kørende Media Center-proces.
>
> To præciseringer til teksten nedenfor: `Uri`/`UriBuilder` escapede allerede mellemrum, CR+LF
> og ikke-ASCII, så det var reelt kun `&`, `#`, `+` og bogstavelige `%XX`-sekvenser der var i
> stykker. Og `HttpUtility` var aldrig et muligt valg — `McWsProxy.csproj` har ingen
> `System.Web`-reference. Samme rå interpolation findes stadig i lyric-tjenesterne
> (`CajunLyricsService`, `LololyricsService`, `MusiXmatchService`, `Stands4Service`) — bevidst
> uden for §5.4's scope.

`McRestService.CreateRequestUrl` bygger query-strenge med ren strenginterpolation. Der findes
**ikke ét eneste** kald til `Uri.EscapeDataString` / `HttpUtility.UrlEncode` i hele
kodebasen. Værst i:

```csharp
case McCommandEnum.SetInfo:
    sb.Append($"…&Field={field}&Value={value}");   // value = hele sangteksten
```

En sangtekst med `&`, `#`, `+` eller `%` bliver enten afkortet eller fejltolket, og fler-KB
tekster risikerer at overskride serverens URL-længdegrænse. `System.Web` er `using`'et i
filen, men `HttpUtility` bruges aldrig. Skrivning af lyrics burde være POST med body.

### 5.5 Trådmodellen holder kun ved et tilfælde (medium-høj)

`SearchAllProcessWorkerAsync` gør tre ting der normalt er ulovlige:

* `Queue<int>.Dequeue()` fra 10 samtidige workers uden lås (`Queue<T>` er ikke trådsikker).
* Direkte adgang til `MainGridView.Rows[…]` og celleværdier fra async-workers.
* `IsDataChanged`-**getteren** har sideeffekter — den sætter `FileSaveMenuItem.Enabled` og
  `DataChangedTextBox.Visible`.

Det virker i praksis, fordi ingen af de awaits der ligger på vejen tilbage til workeren
bruger `ConfigureAwait(false)`, så alle continuations postes tilbage til UI-tråden og
serialiseres dermed. Men det er en usynlig kontrakt: **ét enkelt `ConfigureAwait(false)`
tilføjet et vilkårligt sted i `LyricSearch` eller `AbstractLyricService`-kæden vil ødelægge
både køen og grid-adgangen.** Brug `ConcurrentQueue<int>.TryDequeue` og eksplicit
UI-marshalling i stedet for at stole på synkroniseringskonteksten.

### 5.6 `WhenAny(predicate)` sluger ikke undtagelser som forventet (medium)

`AsyncUtility.WhenAny` (`AsyncUtility.cs:378`) kalder `condition(task)`, og prædikatet i
`LyricSearch` er `t => t.Result.LyricResult == …`. På en fejlet task kaster `.Result` en
**`AggregateException`**, ikke den oprindelige `LyricServiceBaseException`. Derfor rammer
`catch (LyricServiceBaseException)` i `LyricSearch.cs:112` ikke, og en kvoteoverskridelse
i den parallelle sti propagerer som en uventet undtagelsestype. Samme mønster i den serielle
sti (`LyricSearch.cs:102`, `task.Result` efter `await`).

### 5.7 CancellationToken når aldrig HTTP-laget (medium)

Token'et trædes pænt gennem hele det øverste lag og stopper så brat.

Kæden er intakt hele vejen ned:
`SearchAllProcessWorkerAsync` → `LyricSearch.SearchAsync(…, cancellationToken, …)` →
`ProcessAsyncWrapper` → `ProcessAsync` → `ExtractAllLyricTextsAsync` →
`ExtractOneLyricTextAsync(uri, cancellationToken)`.

Bruddet sker i `AbstractLyricService.HttpGetStringAsync(Uri requestUri)`
(`AbstractLyricService.cs:441`), som ikke tager et token og kalder videre til
`SharedComponents.Utility.HttpGetStringAsync(requestUri, timeoutMilliSeconds: TimeoutMilliSeconds)`.
Hverken den eller `HttpGetImageAsync` har en `CancellationToken`-parameter, og `McWsProxy`
indeholder **nul** forekomster af typen overhovedet.

**Konsekvensen er brugersynlig:** Stop under en "Search all" afbryder ikke en igangværende
forespørgsel — den forhindrer kun at næste emne påbegyndes.
`AsyncUtility.RandomizedDelayAsync` forværrer det: dens `Task.Delay(delay)` tager heller intet
token, så med en høj `DelayMilliSecondsBetweenSearches` kan Stop tage flere sekunder pr. worker
før den bider.

**Implementeringsdetalje der bider med det samme:** på .NET Framework 4.8 har
`HttpClient.GetStringAsync` **ingen** CancellationToken-overload — den kom først i .NET 5.
Rettelsen kræver derfor `SendAsync(HttpRequestMessage, cancellationToken)` efterfulgt af
`ReadAsStringAsync()`. Det er nøjagtig den ombygning §5.3 allerede foreslår for at slippe af med
den genskabte `HttpClient`, så de to bør laves i samme ombæring.
`Task.Delay(int, CancellationToken)` findes derimod på net48 og er en ren tilføjelse.

`CLAUDE.md` foreskriver "Use CancellationToken in I/O, database, and HTTP calls where
appropriate", så dette er et hul i forhold til projektets egne regler og ikke blot en
forbedring.

### 5.8 CR+LF overlever ikke konfigurationsfilen (medium)

> <span style="color:#0A6E85">**&#10004; Løst 2026-08-26.**</span> `AbstractLyricService` har fået `ReadServiceSettings(Assembly)`, en
> `ServiceSettings`-dictionary og en etarguments `ServiceSettingsValue(key)`. Konfigurationen
> læses nu direkte som XML, og **begge former understøttes**: attributformen med `&#xD;&#xA;`
> og en `lyricService`-sektion med ét child-element pr. indstilling. Ved sammenstød vinder
> elementformen. Det gamle `Settings`-par er bevaret for kompatibilitet.
>
> MusiXmatch var i forvejen korrekt og er referencen. `CreditTextFormat` er entitiseret igen i
> AZLyrics, Cajun, ChartLyrics og Lololyrics, og `Comment` ligeledes i AZLyrics; alle fem har
> fået en advarsel i header-kommentaren. **Stands4 er lagt om til elementformen**, så begge
> veje afprøves i samme kørsel, og værdierne er verificeret byte-identiske med MusiXmatch'.
>
> Rettelse til teksten nedenfor: et dobbelt mellemrum er *ikke* signaturen på CR+LF. XML
> normaliserer CR+LF til ét LF (§2.11) *før* attributnormaliseringen (§3.3.3), så ét linjeskift
> giver ét mellemrum. De dobbelte mellemrum stammer fra et overskydende mellemrum før
> linjeskiftet.

Flerlinjede værdier mister deres linjeskift. Det rammer både når `LyricsFinder.xml` oprettes
første gang, og ved efterfølgende gem.

**Mekanismen** er XML'ens regel om attributnormalisering (XML 1.0 §3.3.3): et *bogstaveligt*
CR, LF eller TAB inde i en **attributværdi** bliver erstattet af ét mellemrum af enhver
konform parser. Linjeskiftet er væk allerede når værdien læses, og kan ikke gendannes bagefter.
Kun tegnreferencerne `&#xD;` / `&#xA;` overlever. Service-indstillingerne bor netop i
attributter — `<add key="Comment" value="…" />` — og er derfor eksponeret.

**Beviset ligger i repoet.** MusiXmatch skriver sin `Comment` med entiteter og klarer sig
helt igennem til datafilen:

```xml
<!-- MusiXmatchService/App.config -->
value="No user ID required.&#xD;&#xA;Token required, may be obtained from:&#xD;&#xA;…"
```
```xml
<!-- LyricsFinder.xml, linje 118-121 -->
<Comment>No user ID required.&#xD;
Token required, may be obtained from:&#xD;
…</Comment>
```

AZLyrics og Stands4 skriver rå linjeskift i attributten, og deres tekst står nu med
**dobbelte mellemrum** præcis dér hvor linjeskiftene var — signaturen på at CR og LF hver
især er blevet til ét mellemrum:

```
"No user ID or token required.··Use this service for manual lyric searches only,··as automatic…"
```

Det gælder allerede i `HEAD`, ikke kun i arbejdstræet: skaden er sket engang tidligere og er
blevet committet, og de tre `App.config`-filer er dermed selv blevet den korrupte kilde.

#### 5.8.1 Tællingen bekræfter at skrivestien selv er sund

En optælling af den aktuelle datafil
(`%USERPROFILE%\Documents\LyricsFinder\LyricsFinder.xml`, 9257 bytes):

| Måling | Antal |
|---|---|
| `&#xD;`-entiteter | 12 |
| Bogstavelige CR-bytes | 160 |
| Bogstavelige LF-bytes | 172 |

De 160 CR indgår alle i CR+LF-par og stammer fra `Indent = true`, hvis `NewLineChars` som
standard er `"\r\n"` — det er ren pretty-printing mellem elementer. De resterende
`172 − 160 = 12` løse LF matcher **nøjagtigt** de 12 entiteter: hvert indlejret linjeskift i
tekstindhold er skrevet som `&#xD;` plus et bogstaveligt LF, præcis som
`NewLineHandling.Entitize` foreskriver. Ingen af dem er gået tabt.

Konklusionen er derfor entydig: `XmlSerializeToFile` gør det rigtige, og fejlen ligger
udelukkende opstrøms i `App.config`-attributterne. Det er også grunden til at en rettelse i
serialiseringskoden ikke vil hjælpe — kilden skal repareres.

#### 5.8.2 Konsekvens for de to symptomer

* *Ny fil* — `AbstractLyricService.RefreshServiceSettingsAsync` seeder `Comment`,
  `CreditTextFormat` m.fl. fra `App.config` via `ServiceSettingsValue`. Parseren afleverer
  allerede fladet tekst, så en frisk `LyricsFinder.xml` arver fladningen. Datafilens
  skrivestí er i sig selv korrekt — kilden var ødelagt inden.
* *Gem af ændringer* — så længe værdierne skrives tilbage i en attribut uden entitisering,
  flades de igen ved næste læsning. Runden er tabsgivende hver gang.

Bemærk hvor det gør ondt: `CreditTextFormat` er den blok der føjes til **hver eneste gemt
sangtekst** i mediebiblioteket. Den er intakt i den nuværende datafil (`&#xD;` på linje
125-133), men den ligger i samme attribut-lag og er ét fladningsuheld fra at mangle sine
linjeskift i alt hvad der er gemt.

#### 5.8.3 Retningen på en rettelse

Den hurtige udvej er at bruge `&#xD;&#xA;` i alle
`App.config`-attributter, som MusiXmatch allerede gør. Det virker, men entiteterne skal
vedligeholdes i hånden for evigt — en udvikler der trykker Enter i en attribut ødelægger
værdien uden at opdage det.

Den holdbare rettelse er at flytte de flerlinjede indstillinger ud af attributter og over i
child-elementer. Bemærk at reglen dér er en anden: i **elementindhold** normaliseres CR+LF til
et enkelt LF (XML 1.0 §2.11) — linjeskiftet *overlever* altså, det bliver bare LF i stedet for
CR+LF. Det er den afgørende forskel fra attributter, hvor bruddet forsvinder helt.

*Før — bruddet dør ved læsning:*

```xml
<appSettings>
  <add key="Comment" value="No user ID required.
Token required, may be obtained from:
https://about.musixmatch.com/api-pricing" />
</appSettings>
```

*Efter — bruddet overlever som LF:*

```xml
<lyricService>
  <ServiceName>MusiXmatch</ServiceName>
  <Comment>No user ID required.
Token required, may be obtained from:
https://about.musixmatch.com/api-pricing</Comment>
</lyricService>
```

Læsesiden bliver samtidig enklere. Indstillingerne bruges kun til seeding ved første start, så
hele `ConfigurationManager`-maskineriet kan udgå til fordel for almindelig XML-læsning:

```csharp
// Erstatter ConfigurationManager.OpenExeConfiguration(assy.Location) i RefreshServiceSettingsAsync
private static Dictionary<string, string> ReadServiceSettings(Type serviceType)
{
    var configPath = Assembly.GetAssembly(serviceType).Location + ".config";
    var ret = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
    var doc = new XmlDocument { XmlResolver = null };

    using (var reader = XmlReader.Create(configPath, new XmlReaderSettings { XmlResolver = null }))
        doc.Load(reader);

    foreach (XmlNode node in doc.SelectNodes("/configuration/lyricService/*"))
    {
        // Elementindhold kommer tilbage med LF; WinForms vil have CR+LF.
        ret[node.Name] = node.InnerText.LfToCrLf();
    }

    return ret;
}
```

To pointer ved den form: `LfToCrLf()` findes allerede i `SharedComponents.Utility` og gør netop
det sidste skridt, og `XmlResolver = null` holder XXE-linjen fra resten af kodebasen.

Vær opmærksom på at `Utility.DllConfigurationFile()` *ikke* kan bruges her — den kalder
`Assembly.GetExecutingAssembly()` og peger derfor på `Utility.dll`, ikke på den service der
skal læses. `Assembly.GetAssembly(serviceType)` er den rigtige, og er også den form
`RefreshServiceSettingsAsync` allerede bruger.

Uanset hvilken vej der vælges: reparér samtidig de tre committede `App.config`-filer. Deres
linjeskift kan ikke udledes af koden og skal skrives ind igen i hånden.

### 5.9 To uens serialiseringsstier (medium, bør verificeres)

> <span style="color:#0A6E85">**&#10004; Løst 2026-08-26.**</span> En privat `ToXmlWithNewlinesCore(object, Type, Type[])` bærer nu logikken, og
> `XmlSerializeToString` sender `objectInstance.GetType()` i stedet for at lade `T` udlede til
> `object`. Streng- og filstien deler dermed serializer; `ToXmlWithNewlines<T>` er uændret.
>
> Præcisering: den konkrete konsekvens var at `InitialXml` fik rod-elementet `anyType` i stedet
> for `LyricsFinderDataType`. Om den `object`-typede serializer faktisk udelod felter blev
> aldrig verificeret — og behøver det ikke længere.

Beslægtet med ovenstående, og værd at se på i samme omgang. `Serialize` har to skrivestier
der ikke bruger den samme serializer:

* `XmlSerializeToFile` bruger `objectInstance.GetType()` — den konkrete type.
* `XmlSerializeToString` kalder `ToXmlWithNewlines<T>(this T obj, …)` med `objectInstance`
  **statisk typet som `object`**. Typeinferensen binder derfor `T` til `object`, og der
  konstrueres `new XmlSerializer(typeof(object), knownTypes)` — en anden serializer end
  filstien bruger.

Konsekvensen er at `IsChanged`-baselinen (`InitialXml`) produceres af en anden serializer end
den der skriver filen. De to er internt konsistente, så dirty-detektionen fungerer sandsynligvis
— men hvis den `object`-typede serializer udelader felter som den konkrete tager med, vil
ændringer i netop de felter ikke blive opdaget, og `SaveAsync` springer tavst gemningen over
(`if (IsChanged && IsSaveOk)`). Det er en plausibel medvirkende årsag til "ændringer bliver
ikke gemt", men jeg har ikke verificeret hvilke felter der reelt afviger.

### 5.10 Sikkerhed

| Emne | Vurdering |
|---|---|
| **MCWS-password i klartekst** | `MainDataType.McWsPassword` er `[XmlElement]` og gemmes ukrypteret i `LyricsFinder.xml` under `Documents`. Samme gælder Stands4-`Token`/`UserId` og MusiXmatch-token. Overvej DPAPI (`ProtectedData`) eller Windows Credential Manager. |
| **Token i query-streng** | `McWsToken` sendes som `?Token=…` i hver URL. Mod `localhost` er eksponeringen begrænset, men tokens havner i logfiler: `HttpGetStringAsync` indlejrer den fulde URL i sine exception-beskeder, som skrives til `LyricsFinder.*.error.log`. |
| **`Assembly.LoadFrom` på alle `*.dll`** | Enhver DLL lagt i programmappen bliver indlæst og instantieret. Mappen er `Program Files` (adminrettigheder krævet), så det er ikke en akut sårbarhed, men der er ingen strong-name- eller signaturvalidering af "plugins". |
| **`.snk`-filer i repoet** | Strong-name-nøglerne er committet. Strong naming er ikke en sikkerhedsgrænse, men enhver kan nu signere assemblies med samme identitet. |
| **XXE** | Håndteret korrekt — `XmlResolver = null` er sat konsekvent i `McMplResponse`, `McPlayListsResponse` og `LyricsFinderDataType`. |
| **HTML-scraping** | AZLyrics/Lololyrics parser fremmed HTML med HtmlAgilityPack og lægger resultatet i en `DataGridView`. Ingen HTML-rendering af indholdet, så XSS-risikoen er lav. |

### 5.11 Byggeopsætning

* **Hardkodede Visual Studio-stier.** `BuildRelease.subroutine.cmd:17-18` leder kun i
  `D:\Program Files [(x86)]\Microsoft Visual Studio\{2017,2019,2022,2026}\Community\…`.
  Scriptet virker ikke på en maskine hvor VS ligger på C:, eller med Professional/Enterprise.
  Brug `vswhere.exe`.
* **`del /s /q "$(TargetDir)*.*"` som pre-build** i Core, Exe og Plugin, kombineret med
  post-build-`xcopy`-kæder der kopierer på tværs af projektmapper og ind i
  `Installation\Build\{Plugin,Standalone,LyricServices}`. `LyricsFinderCore` **tømmer**
  `Build\LyricServices` i sin post-build. Det hele afhænger af build-rækkefølgen i
  `.sln`-filens `ProjectDependencies` og er ikke inkrementelt-sikkert eller parallelbygge-sikkert.
* **Binding redirects passer ikke til pakkeversionerne.** `LyricsFinderCore/App.config`
  redirect'er `Newtonsoft.Json` til `12.0.0.0`, mens `packages.config` har 13.0.4
  (assembly-version `13.0.0.0`). Tilsvarende for `System.Buffers` (`4.0.3.0` vs. pakke 4.6.1)
  og `System.Numerics.Vectors`. Kan give `FileLoadException` ved runtime.
* **`MediaCenter.sln` er brudt** (manglende `ApiseedsService.csproj`, se §2).
* **Ingen CI.** `.github/` indeholder kun `ISSUE_TEMPLATE`. Ingen workflows.
* **Versionsinkonsistens.** `SharedAssemblyInfo.cs` = 1.3.7.0; `Setup.iss` `AppVersion` og
  registry-`Version` = hardkodet 1.0.0; `ReleaseNotes.html` topper ved v1.3.1; seneste commit
  hedder "Starting v1.4.0 changes".
* **`Setup.iss`: `IsMc32()` kan aldrig returnere `true`.** `McVersion` sættes til
  `Copy(McInstVersions[Idx], 1, 2)` — kun de to versionscifre, fx `"34"` — men `IsMc32`
  tester `Pos('32-bit', McVersion)`. En 32-bit MC-installation får derfor 64-bit `RegAsm`
  og `HKLM64`-nøgler. Vælger brugeren desuden kun `standalone`-komponenten, springes
  version-siden over, og `NextButtonClick` sætter aldrig `InstallDir`.
* **`BuildAndInstallLyricsFinder.cmd`**: `:wait / if exist %_out_file% goto :wait` løkker
  så længe filen **findes** — logikken ser omvendt ud i forhold til kommentaren
  "Waiting for the build completion".

### 5.12 Øvrige code smells

* **`LyricsFinderCore` er en god monolit-kandidat til opdeling**: `LyricsFinderCore.cs` (1369
  linjer) + `.Private.cs` (1296) + `.Process.cs` (486) i én partial klasse, der blander UI,
  MCWS-orkestrering, filsystem og forretningslogik. Det er den primære grund til at der
  ikke findes unit-tests for kernen.
* **`StackTrace` i en property-getter.** `LyricsFinderCoreConfigurationSectionHandler.Instance`
  laver `new StackTrace()` og `GetFrame(1).GetMethod().Name` ved **hvert** opslag, for at
  udlede hvem der kalder. Dyrt, og fejlbehæftet så snart JIT'en inliner.
* **`Utility.GetLinkerTime`** læser PE-headerens tidsstempel. Med deterministiske builds
  (`<Deterministic>true</Deterministic>` i `McWsProxy`; Roslyn-default) er feltet en hash,
  ikke en dato — funktionen giver meningsløse "build-tidspunkter".
* **`RandomizedDelayAsync`** laver `new Random()` pr. kald. På .NET Framework seedes den fra
  systemuret, så samtidige kald inden for samme tick får **identisk** forsinkelse — hvilket
  ophæver formålet (at sprede requests ud for ikke at blive rate-limited).
* **`AbstractLyricService._semaphoreSlim` er `static`.** Alle tællere for **alle** services
  serialiseres gennem én global semafor, selvom låsen kun beskytter instans-felter.
* **`AbstractLyricService.ProcessAsyncWrapper`** kalder `ret.LyricResult` uden null-tjek —
  en override der returnerer `null` giver NRE.
* **`InitLocalDataAsync`**: `Path.Combine(DataDirectory, dataFile + ".tmp")` hvor `dataFile`
  allerede er en absolut sti. Det virker (Combine ignorerer første argument når det andet er
  rooted), men det er utilsigtet og forvirrende.
* **Dødt kode.** `LyricServices.Old/` (ApiseedsService, LyricWikiService) er ikke i nogen
  solution. `McPlayControlForm.cs` (411 linjer) ligger i Core-mappen men står **ikke** i
  `LyricsFinderCore.csproj` og kompileres derfor ikke. `MjpCreator` er sat ud af drift.
  `Serialize.cs` bærer kommentaren "Not used in this solution version", men er faktisk
  central.
* **Genererede filer i kildetræet.** `MessageInspection.dll/.pdb/.xml`,
  `*.GeneratedMSBuildEditorConfig.editorconfig`, `*.CodeAnalysisLog.xml` ligger side om side
  med kildekoden i hver service-mappe (kopieret dertil af `PostBuild.cmd`). De er ikke
  git-trackede, men støjer i mappelistningen.
* **Undtagelsesdisciplin.** ~50 forekomster af `throw new Exception(...)` — svært at
  fange selektivt. Der findes til gengæld et gennemarbejdet
  `LyricServiceExceptions.cs`-hierarki, som bare ikke bruges konsekvent.
* **95 `async void`-metoder.** Næsten alle er WinForms-event-handlere (konventionelt), men
  `LyricsFinderExe/Program.cs` installerer **hverken** `Application.ThreadException` eller
  `AppDomain.UnhandledException`. En undtagelse der slipper ud af en `async void`-handler
  lukker programmet uden log.

---

## 6. Testhuller

Der findes i alt **28 `[TestMethod]`** — alle i de 6 lyric-service-projekter, og alle er
**integrationstests mod live tredjeparts-websites**:

| Projekt | Tests |
|---|---|
| Lololyrics | 10 |
| CajunLyrics | 8 |
| AZLyrics | 4 |
| ChartLyrics / MusiXmatch / Stands4 | 2 hver |

De har tre hårde afhængigheder ud af processen:

1. **Netværk + tredjepartstjenester.** Testene slår hardkodede sange op
   ("Bruce Daigrepont – La Jalouserie") og fejler når en side ændrer markup, er nede, eller
   rate-limiter. AZLyrics-konfigurationen advarer selv om at automatisk søgning kan give
   IP-ban.
2. **Brugerens rigtige datafil.** `[TestInitialize]` kalder
   `LyricsFinderDataType.GetLyricService<T>()`, som læser
   `%USERPROFILE%\Documents\LyricsFinder\LyricsFinder.xml`. Testene kan derfor **ikke** køre
   på en ren maskine eller i CI, og de er afhængige af at brugeren har konfigureret rigtige
   API-tokens (Stands4, MusiXmatch).
3. **Test-kode i produktions-assembly.** MSTest-referencerne ligger i selve
   service-dll'erne, som derfor shipper med hele testinfrastrukturen. Post-build-scriptet
   fjerner `Microsoft.VisualStudio.TestPlatform*` manuelt.

### 6.1 Helt utestet

| Område | Kommentar |
|---|---|
| `SharedComponents.Utility` | ~30 rene strengfunktioner (`ToSentenceCase`, `ToTitleCase`, `RemoveParenthesizedText`, `ToNormalizedLineEndings`, `TrimStringLines`) — trivielle at teste, og der er kendte kanttilfælde (`ToTitleCase` indekserer `lines[i][0]` og `CapitalizeWordTitle` kaster på ord med mellemrum). |
| `Serialize` / `LyricsFinderDataType` | Round-trip, v1.1→v1.2-migreringen (`<Service>` → `<LyricService>`), `IsChanged`-semantik. |
| `McWsProxy` | `CreateRequestUrl` pr. kommando — netop her hvor encoding-fejlen sidder. Kan testes helt uden netværk. |
| `LyricSearch` | Orkestreringen (parallel vs. seriel, first-hit, tællertilbageføring) med fake-services. Ingen netværk nødvendig. |
| `AbstractLyricService` | Kvotehåndtering, retry-uden-parenteser, duplikatfiltrering i `AddFoundLyric`. |
| `LyricsFinderCore` | Ingen — og koden er ikke struktureret til det. |
| `Installation` / `Setup.iss` | Ingen. |

**Anbefalet rækkefølge**, hvis der skal investeres i tests: (1) et separat
`*.Tests`-projekt pr. lag, så MSTest ryger ud af produktions-assemblies; (2) rene
enhedstests af `Utility` + `McRestService.CreateRequestUrl` + `Serialize` — hurtige,
hermetiske, og de dækker to af de mistænkte fejl ovenfor; (3) `LyricSearch` med
in-memory-fakes; (4) de eksisterende netværkstests flyttes til en separat kategori der
er slået fra by default.

---

## 7. Build-, test- og køre-kommandoer

### 7.1 Forudsætninger

* Windows, Visual Studio 2022 eller nyere med workloaden **.NET desktop development**
  (klassiske csproj'er + WinForms-designer). `dotnet build` alene er **ikke** nok.
* .NET Framework 4.8 Developer Pack.
* JRiver Media Center installeret — `LyricsFinderPlugin` har en `COMReference` til MC's
  type-library og kan ikke bygges uden.
* Debug-builds sætter `RegisterForComInterop` → **kør Visual Studio som administrator**.

### 7.2 Restore og build

NuGet-restore skal ske med `nuget.exe`/`msbuild -t:Restore`, ikke `dotnet restore`
(`packages.config`-format):

```powershell
# fra repo-roden
nuget restore LyricsFinder.sln
msbuild LyricsFinder.sln -p:Configuration=Debug   -p:Platform="Any CPU"
msbuild LyricsFinder.sln -p:Configuration=Release -p:Platform="Any CPU"
```

Find `msbuild` uden at hardkode stien:

```powershell
$vs = & "${env:ProgramFiles(x86)}\Microsoft Visual Studio\Installer\vswhere.exe" `
        -latest -requires Microsoft.Component.MSBuild -property installationPath
& "$vs\MSBuild\Current\Bin\MSBuild.exe" LyricsFinder.sln -p:Configuration=Release
```

Projektets eget script (kræver at VS ligger på `D:\`, se §5.11):

```
LyricsFinder\Installation\BuildRelease.cmd              # elevated clean+release-build
LyricsFinder\Installation\BuildAndInstallLyricsFinder.cmd   # build, derefter Output\Setup.exe
```

Bemærk at `BuildRelease.subroutine.cmd` kører `-t:Clean,Build` og at flere projekter har
`del /s /q "$(TargetDir)*.*"` som pre-build — build er destruktivt over for output-mapper.

### 7.3 Tests

```powershell
vstest.console.exe `
  LyricsFinder\LyricServices\CajunLyricsService\bin\Debug\CajunLyricsService.dll `
  LyricsFinder\LyricServices\LololyricsService\bin\Debug\LololyricsService.dll `
  LyricsFinder\LyricServices\AZLyricsService\bin\Debug\AZLyricsService.dll `
  LyricsFinder\LyricServices\ChartLyricsService\bin\Debug\ChartLyricsService.dll `
  LyricsFinder\LyricServices\MusiXmatchService\bin\Debug\MusiXmatchService.dll `
  LyricsFinder\LyricServices\Stands4Service\bin\Debug\Stands4Service.dll
```

Eller via Test Explorer i Visual Studio. Husk forudsætningerne fra §6: netværk, en
eksisterende `%USERPROFILE%\Documents\LyricsFinder\LyricsFinder.xml`, og gyldige tokens
for Stands4 og MusiXmatch. Tests i CI vil fejle.

### 7.4 Kørsel

**Stand-alone:**

```
LyricsFinder\LyricsFinderExe\bin\Debug\LyricsFinderExe.exe
```

Første start viser `OptionForm`, hvor MCWS-URL, Access Key, brugernavn og password skal
udfyldes. Access Key findes i Media Center under *Tools → Options → Media Network*.

**Som MC plug-in:** kræver COM-registrering og en registry-nøgle under
`HKLM\SOFTWARE\J. River\Media Center <version>\Plugins\Interface\LyricsFinder` — brug
installeren (`Installation\Output\Setup.exe`) frem for at gøre det i hånden.

### 7.5 Logs og data

```
%USERPROFILE%\Documents\LyricsFinder\LyricsFinder.xml                    # al brugerdata
%USERPROFILE%\Documents\LyricsFinder\LyricsFinder.Standalone.log         # info
%USERPROFILE%\Documents\LyricsFinder\LyricsFinder.Standalone.error.log   # warn+
%USERPROFILE%\Documents\LyricsFinder\LyricsFinder.Plugin[.error].log
```

Slet `LyricsFinder.xml` for at nulstille til fabriksindstillinger (services genskabes fra
`App.config`-seeds ved næste start).

---

## 8. Hvis jeg skulle prioritere

1. **URL-encoding i `McRestService`** (§5.4) — lille rettelse, direkte brugersynlig fejl.
   <span style="color:#0A6E85">**&#10004; Løst 2026-08-26.**</span>
2. **Cache `XmlSerializer`-instanserne** (§5.1) og fjern `Load`+`Save` pr. sang fra
   `LyricSearch`'s `finally` — det er både lækagen og det største performance-problem.
   <span style="color:#0A6E85">**&#10004; Løst 2026-08-26.**</span> Cachen er på plads; `Load`+`Save` pr. sang står stadig, se §9.1.
3. **Verificér tællerlogikken** i `LyricSearch.SearchAsync` (§5.2).
4. **Én statisk `HttpClient`** med `HttpRequestMessage`-baseret auth i stedet for at
   genskabe klienten (§5.3). Før samtidig `CancellationToken` hele vejen ned gennem
   `HttpGetStringAsync` og `McWsProxy` (§5.7) — det er den samme ombygning, fordi
   `GetStringAsync` på net48 ikke har nogen token-overload.
5. **Flyt flerlinjede indstillinger ud af XML-attributter** (§5.8) og reparér de tre
   `App.config`-filer hvis linjeskift allerede er fladet ud.
   <span style="color:#0A6E85">**&#10004; Løst 2026-08-26.**</span> Begge former læses nu; Stands4 kører elementformen. Attributnormalisering er tabsgivende
   ved hver eneste læsning, og `CreditTextFormat` ender i hver gemt sangtekst.
6. **`ConcurrentQueue` + eksplicit UI-marshalling** i søge-workerne (§5.5), så koden ikke
   afhænger af en usynlig kontrakt.
7. **Ryd op i build**: `vswhere` i stedet for `D:\`-stier, ret binding redirects, reparér
   eller fjern `MediaCenter.sln`, slet `LyricServices.Old` og `McPlayControlForm.cs`.
8. **Separate testprojekter** + enhedstests af `Utility`, `CreateRequestUrl` og `Serialize`,
   så der overhovedet findes et sikkerhedsnet at refaktorere med. En round-trip-test af
   flerlinjet tekst gennem `App.config` → datafil → `App.config` ville have fanget §5.8.

---

## 9. Udestående

Status pr. 2026-08-26. §5.1, §5.4, §5.8 og §5.9 er rettet og testet mod en kørende Media Center.
Herunder står det der er tilbage, inklusive tre forhold der først dukkede op under rettearbejdet.

### 9.1 Tællerne vedligeholdes skævt i `LyricSearch` (medium, ny)

Tre selvstændige problemer i det samme stykke kode, alle omkring `RequestCountToday` /
`RequestCountTotal`:

* **Nulstillingen kapløber med det arbejde den skal gå forud for.** `LyricSearch.cs:67-69` starter
  `ProcessAsyncWrapper` og kalder *derefter* `serviceClone.ResetTotalCountersAsync()`. Klonens
  totaler er ment som en delta for netop denne sang, men klonen er allerede i gang.
  `IncrementRequestCountersAsync` ligger i `finally`'en i `HttpGetStringAsync`
  (`AbstractLyricService.cs:471`), og `RandomizedDelayAsync` kører først, så nulstillingen vinder
  næsten altid — men med `DelayMilliSecondsBetweenSearches` på 0 og et hurtigt svar kan en optælling
  blive nulstillet væk. Rettelsen er at nulstille *før* tasken startes, eller at klone med
  nulstillede tællere.
* **Gemningen pr. sang er en garanteret no-op.** Præcisering af §5.2: forøgelserne lander på
  objekter fra den *oprindelige* instans, mens `SaveAsync()` kaldes på den nyindlæste
  (`LyricSearch.cs:128` og `:150`). Den nyindlæste er uændret siden `Load`, så `IsChanged` er falsk
  og `if (IsChanged && IsSaveOk)` springer skrivningen over. Tællerne går *ikke* tabt — de bliver
  siddende i Core'ens levende model og skrives af et af de øvrige `SaveAsync()`-kald — men denne ene
  koster en fuld XML-læsning pr. sang uden nogensinde at skrive noget.
* **Halvt nulstillet klon.** `ResetTotalCountersAsync` nulstiller kun totalerne. Klonens
  `...Today`-værdier kopieres med fra originalen og ignoreres derefter.

### 9.2 Tre uafhængige regler for daglig nulstilling (lav, ny)

| Sted | Betingelse | Hvornår |
|---|---|---|
| `AbstractLyricService.cs:661` | `LastRequest.Date < Now.Date` | kun ved opstart |
| `LyricsFinderCore.cs:897` | `Now.Date > MainData.LastMcStatusCheck.Date` | på MC-status-timeren |
| `Stands4Service.cs:300` | `QuotaResetTime` i UTC | ved hvert kvotetjek |

De kan nulstille i vilkårlig rækkefølge og på forskellige tidspunkter. Vil man kunne stole på
`RequestCountToday`, bør der være én regel.

### 9.3 `DailyQuota` seedes aldrig fra `App.config` (lav, ny)

`Stands4Service.RefreshServiceSettingsAsync` læser `QuotaResetTimeZone` og `QuotaResetTime`, men
ikke `DailyQuota`. Indstillingen når derfor aldrig frem til propertyen. Effekten er lav i dag, fordi
kvoteberegningen er slået fra med vilje (`ret = false;` med kommentar i `Stands4Service.cs:309-312`
— tjenesten får lov at melde selv), men `DailyQuota`-kolonnen i service-formularen viser værdien.

### 9.4 `IsConfigurationFileUsed` er `static` (lav, ny)

`AbstractLyricService.cs:98` erklærer den `public static bool`, men den tildeles pr. instans i
`RefreshServiceSettingsAsync` (`:632`) og læses som instanstilstand på `:639` og i
`Stands4Service.cs:417` efter `base`-kaldet. Alle tjenester deler altså ét flag. Det virker kun
fordi `InitLyricServicesAsync` (`LyricsFinderCore.Private.cs:452-457`) gennemløber dem sekventielt
og hver override læser flaget umiddelbart efter.

### 9.5 Rå interpolation lever stadig i lyric-tjenesterne (medium)

§5.4 rettede `McRestService.CreateRequestUrl`, men de fire tjenester der selv bygger URL'er —
`CajunLyricsService`, `LololyricsService`, `MusiXmatchService` og `Stands4Service` — interpolerer
stadig artist, album og titel direkte ind i query-strengen. `UrlEncoded()` findes nu i
`SharedComponents/Utility/Utility.cs` og kan bruges direkte.

### 9.6 Stadig åbne fra afsnit 5 og 6

| Punkt | Emne | Bemærkning |
|---|---|---|
| §5.2 | Tællere gemmes muligvis aldrig | Præciseret i §9.1 — ikke tabt, men gemningen pr. sang skriver aldrig |
| §5.3 | HttpClient genskabes pr. kald | Uændret. Bør laves sammen med §5.7 |
| §5.5 | Trådmodellen holder ved et tilfælde | Uændret |
| §5.6 | `WhenAny(predicate)` og `AggregateException` | Uændret |
| §5.7 | CancellationToken når ikke HTTP-laget | Uændret |
| §5.10 | Sikkerhed | Password og tokens ligger stadig i klartekst i datafilen |
| §5.11 | Byggeopsætning | Uændret |
| §5.12 | Øvrige code smells | Kun kommentaren i `Serialize.cs` er væk; resten står |
| §6 | Testhuller | Uændret. `Serialize` og `CreateRequestUrl` er nu rettet uden et sikkerhedsnet under sig |
