# Task 001 — Scaffold services

Create the .NET solution and three service project stubs so the solution builds cleanly. No business logic yet — just the skeleton.

## Deliverables

- `layers.sln` referencing all six projects (3 services + 3 test projects)
- For each of `orchestrator`, `numbers`, `esme_squalor`:
  - `<Service>.csproj` — .NET 8, references `Amazon.Lambda.AspNetCoreServer.Hosting`
  - `Program.cs` — dual-mode entry point (runs as Kestrel locally, Lambda on AWS)
  - `Endpoints.cs` — empty route registration stub
  - `Tests/<Service>.Tests.csproj` — xUnit test project referencing the service project
  - `Tests/EndpointTests.cs` — single placeholder test that asserts `true`

## Acceptance criteria

- `dotnet build layers.sln` passes with no errors
- `dotnet test layers.sln` passes (placeholder tests)
- `dotnet run` in each service directory starts a Kestrel server without errors
