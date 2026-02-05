# Repository Guidelines

## Project Structure & Module Organization
- `COINNP.Client/` contains the client wrapper and mapping helpers for COIN NP.
- `COINNP.Entities/` contains the NP message records, enums, and shared model types.
- `COINNP.Tests/` contains MSTest-based unit tests for client/entity behavior.
- `TestApp/` is a small console app that wires up `NPClient` with `appsettings.json` for manual runs.
- `.github/` contains CI workflow and Dependabot configuration.
- `COINNP.sln` is the solution entry point.

## Build, Test, and Development Commands
- `dotnet restore COINNP.sln` restores NuGet packages for the solution.
- `dotnet build COINNP.sln -c Release` builds all projects (use `Debug` for local iteration).
- `dotnet test COINNP.sln -c Release` runs the full test suite.
- `dotnet test COINNP.Tests/COINNP.Tests.csproj` runs only the unit tests.
- `dotnet run --project TestApp/TestApp.csproj` runs the sample console app.

## Coding Style & Naming Conventions
- C# with `nullable` and `implicit usings` enabled; follow existing patterns.
- Indentation: 4 spaces, braces on new lines (typical .NET style).
- Public types and members use `PascalCase`; locals and parameters use `camelCase`.
- File names generally match the primary type they contain (e.g., `NPClient.cs`).
- No formatter/linter is enforced in this repo; keep changes consistent with nearby code.

## Testing Guidelines
- Framework: MSTest (`[TestClass]`, `[TestMethod]`).
- Naming: test classes end with `Tests` (e.g., `ValueHelperParseTests`).
- Aim for focused unit tests; add coverage when changing mapping/parsing logic.
- Optional coverage collection is available via `dotnet test --collect:"XPlat Code Coverage"`.

## Commit & Pull Request Guidelines
- Commit messages are short and imperative; dependabot updates typically use `Bump ...`.
- A conventional prefix appears occasionally (e.g., `chore:`); use it when it adds clarity.
- No PR template is present, so include: summary, testing performed, and any breaking changes.
- Link issues when applicable and note any config changes (e.g., `appsettings.json`).

## Configuration & Security Notes
- `TestApp/appsettings.json` holds NP client configuration and file paths.
- Do not commit real private keys or secrets; use local paths or user secrets for testing.
