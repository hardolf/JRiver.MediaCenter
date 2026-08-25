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
- Do not build or run tests unless I ask; the pre-build steps delete output folders and the tests require live network and real API tokens.
- Do not modify existing tests to hide a defect without explaining why.
- Add or update tests when behavior changes.
- Clearly report compilation errors, analyzer warnings, and uncertain assumptions.

## Workflow
- Before changing code: explain the reason, affected projects, and expected impact.
- List files that will change before major refactoring.
- Stop and ask before making decisions that affect the database schema, XML data file schema,
  public API contracts, authentication, deployment, or security.
