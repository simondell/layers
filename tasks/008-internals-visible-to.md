# Task 008 — Replace partial class hack with InternalsVisibleTo

Remove the `public partial class Program { }` workaround from service
`Program.cs` files and replace it with `InternalsVisibleTo` in the
corresponding `.csproj` files.

## Background

`WebApplicationFactory<Program>` requires the test project to reference
the `Program` type from the service project. With top-level statements,
`Program` is generated as an `internal` class. The `partial class`
declaration makes it public — but pollutes the code file with noise.

The cleaner solution is to declare `InternalsVisibleTo` in the service's
`.csproj`, giving the test project access to internal types without
touching `Program.cs`.

## Changes

For each affected service (`orchestrator`, `esme_squalor`):

1. Remove `public partial class Program { }` from `Program.cs`
2. Add to the service `.csproj`:
   ```xml
   <ItemGroup>
     <InternalsVisibleTo Include="<Service>.Tests" />
   </ItemGroup>
   ```

## Acceptance criteria

- Neither `Program.cs` file contains `partial class Program`
- `dotnet test layers.slnx` still passes (all tests green)
