# Task 009 — Fix numbers service test

The existing test in `services/numbers/Tests/UnitTest1.cs` calls
`Random.Shared.Next` directly and proves nothing about the service.

Replace it with a proper integration test using `WebApplicationFactory`
that calls `GET /number` and asserts the response value is between 1
and 100.

## Changes

1. Add `<InternalsVisibleTo Include="Numbers.Tests" />` to `Numbers.csproj`
2. Add `Microsoft.AspNetCore.Mvc.Testing` package to `Numbers.Tests.csproj`
3. Replace the placeholder test with a `WebApplicationFactory`-based test

## Acceptance criteria

- Test hits the actual `/number` endpoint
- Asserts `value` is between 1 and 100 inclusive
- `dotnet test layers.slnx` still passes
