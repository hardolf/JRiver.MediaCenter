# Project instructions

## Technology
- Language: C#
- Platform: .NET 4.8
- Nullable reference types: disabled
- Asynchronous code: use async/await throughout; avoid .Result and .Wait()
- Dependency injection: use constructor injection where appropriate
- Data access: Internet services, e.g. JSON / REST API
- Testing: MSTest

## Code principles
- Preserve the existing architecture and public APIs unless I explicitly request changes.
- Propose a plan first for changes spanning multiple projects or files.
- Prefer small, reversible changes.
- Use CancellationToken in I/O, database, and HTTP calls where appropriate.
- Do not log secrets, tokens, personal data, or connection strings.
- Do not introduce new NuGet packages without first explaining the need, license, and alternatives.

## Quality
- Do not run a normal build or run tests unless I ask; the pre-build steps delete output folders and the tests require live network and real API tokens.
- Exception - the compile-check below is pre-approved and may be run without asking, to verify that C# edits compile:

  ```
  MSBuild.exe LyricsFinder.sln -nologo -v:m -m -t:Build -p:Configuration=Debug -p:OutputPath=<scratchpad>\compilecheck\ -p:PreBuildEvent= -p:PostBuildEvent= -p:RegisterForComInterop=false
  ```

  Every switch is load-bearing, so run it verbatim and change nothing:
  - `OutputPath` into the scratchpad keeps `Build\`, `Output\` and `Release\` untouched, and
    neutralises the `del /s /q "$(TargetDir)*.*"` pre-build events because TargetDir derives from it.
  - Empty `PreBuildEvent`/`PostBuildEvent` also skip the Inno Setup packaging step.
  - `RegisterForComInterop=false` keeps the Windows registry untouched. Note it does not skip
    *un*registration: `MSB3392 - access denied` means a leftover
    `obj\**\*.UnmanagedRegistration.cache` from an earlier Visual Studio build is being cleaned up.
    That is expected, self-healing, and not a compile error; the next VS build restores the cache.
  - Call it `MSBuild.exe`, not `msbuild` - a directory named `MSBuild` sits next to the exe on PATH.
  - `dotnet build` cannot build this solution; the projects are old-style csproj, not SDK-style.

  It only verifies compilation. Anything beyond it - a real build, packaging or tests - still needs my go-ahead.
- Do not modify existing tests to hide a defect without explaining why.
- Add or update tests when behavior changes.
- Clearly report compilation errors, analyzer warnings, and uncertain assumptions.

## Workflow
- Before changing code: explain the reason, affected projects, and expected impact.
- List files that will change before major refactoring.
- Stop and ask before making decisions that affect the database schema, XML data file schema,
  public API contracts, authentication, deployment, or security.
