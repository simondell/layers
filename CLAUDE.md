# Claude instructions for `layers`

## Workflow

- Work is tracked as GitHub issues (see [Issues](https://github.com/simondell/layers/issues))
- Each issue becomes a branch and a PR — never commit feature work directly to `main`
- Branch naming: `<type>/<number>_<description>` mirroring the conventional commit type, e.g. `feat/004_frontend_ui`

## Commit messages

All commit messages must follow **conventional commits** format:

```
<type>: <short description>

[optional body]
```

Common types: `feat`, `fix`, `chore`, `docs`, `test`, `ci`, `refactor`

## Code style

### C#

- ASP.NET Core Minimal API only — no controllers, no classes for route registration
- Top-level statements throughout — routes registered inline on `app`:
  ```csharp
  var builder = WebApplication.CreateBuilder(args);
  builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);
  var app = builder.Build();
  app.MapGet("/route", () => ...);
  app.Run();
  ```
- Named `HttpClient` registrations for inter-service calls — never `new HttpClient()`
- Leaf service URLs from configuration, not hardcoded

### General

- No commented-out code
- No speculative abstractions — solve the problem at hand

## Project overview

A learning project. Plain JS frontend hosted on S3, three C# .NET 10 backend services deployed as AWS Lambdas, infrastructure managed with Terraform, CI via GitHub Actions.

### Services

| Name | Role | Local port |
|---|---|---|
| `orchestrator` | BFF — receives `count` (1–3) from frontend, calls the leaf services, returns a total | 5000 |
| `numbers` | Returns a random integer 1–100 | 5001 |
| `esme_squalor` | Returns `"in"` or `"out"` for a given number based on today's day score | 5002 |

### App logic

Frontend sends `GET /result?count=N` to the orchestrator. Orchestrator:
1. Calls `numbers` N times in parallel
2. Calls `esme_squalor /verdict?number=<n>` for each result
3. Sums the "in" numbers, returns `{ "total": <sum> }` (0 if none)

### Esme's rule

A number is **in** if `number > day_score`, where day_score is the sum of alphabetic positions (A=1…Z=26) of today's 3-letter day abbreviation:

| MON | TUE | WED | THU | FRI | SAT | SUN |
|---|---|---|---|---|---|---|
| 42 | 46 | 32 | 49 | 33 | 40 | 54 |

## Tech stack

- **Frontend:** plain HTML, CSS, vanilla JS — no framework. Hosted on S3 static website. Terraform handles upload and orchestrator URL injection via `replace()`.
- **Backend:** C# .NET 10, ASP.NET Core Minimal API
- **Lambda packaging:** `Amazon.Lambda.AspNetCoreServer.Hosting` — same binary runs locally (Kestrel) and on AWS (Lambda). No Docker, no SAM.
- **Infrastructure:** Terraform (AWS provider `~> 6.27`) — `infra/modules/lambda-service/` is a reusable module instantiated once per service. Lambda timeout 30s, memory 256MB.
- **CI:** GitHub Actions — `ci.yml` (build + test) and `tf-plan.yml` (terraform plan on PRs)
- **Testing:** xUnit + `WebApplicationFactory` for in-process integration tests

## Configuration

- Leaf service URLs read from configuration using **colon separators**: `Services:Numbers:BaseUrl`, `Services:EsmeSqualor:BaseUrl`. ASP.NET Core normalises `__` env var names to `:` internally — code must use the colon form.
- CORS allowed origins configured via `Cors:AllowedOrigins` — `appsettings.Development.json` for local dev (`http://localhost:8080`), Lambda env var for production (S3 URL injected by Terraform).
- Local frontend development: serve `frontend/` on port 8080 (e.g. `python -m http.server 8080`)

## Build and deploy

- `./build.sh` — publishes all three services as zips in `build/`
- `cd infra && terraform apply` — deploys Lambdas, API Gateways, S3 frontend, and uploads frontend files with orchestrator URL injected

## AWS

- Each Lambda gets its own API Gateway v2 (HTTP API)
- Orchestrator receives the leaf service URLs as Lambda environment variables, injected by Terraform outputs
- Frontend hosted on S3 static website — bucket managed by Terraform
