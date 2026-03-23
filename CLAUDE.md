# Claude instructions for `layers`

## Workflow

- Work is tracked in `tasks/` as numbered markdown files
- Each task becomes a branch and a PR — never commit feature work directly to `main`
- Branch naming: `<type>/<number>_<description>` mirroring the conventional commit type e.g. `feat/001_scaffold_services`
- All commit messages must follow **conventional commits** format: `<type>: <description>`
  - Common types: `feat`, `fix`, `chore`, `docs`, `test`, `ci`, `refactor`

## Project overview

A learning project. Plain JS frontend, three C# .NET 8 backend services deployed as AWS Lambdas, infrastructure managed with Terraform, CI via GitHub Actions.

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

- **Frontend:** plain HTML, CSS, vanilla JS — no framework, no build step
- **Backend:** C# .NET 8, ASP.NET Core Minimal API
- **Lambda packaging:** `Amazon.Lambda.AspNetCoreServer.Hosting` — same binary runs locally (Kestrel) and on AWS (Lambda). No Docker, no SAM.
- **Infrastructure:** Terraform — `infra/modules/lambda-service/` is a reusable module instantiated once per service
- **CI:** GitHub Actions — `ci.yml` (build + test) and `tf-plan.yml` (terraform plan on PRs)
- **Testing:** xUnit + `WebApplicationFactory` for in-process integration tests

## C# conventions

- Use ASP.NET Core Minimal API (not controllers)
- Register named `HttpClient` instances for inter-service calls — never `new HttpClient()`
- Leaf service URLs come from configuration (`Services__Numbers__BaseUrl`, `Services__EsmeSqualor__BaseUrl`) so they work locally and in Lambda without code changes

## AWS

- Region: `eu-west-2` (London)
- IAM user: `learning_terraform`
- Each Lambda gets its own API Gateway v2 (HTTP API)
- Orchestrator receives the leaf service URLs as Lambda environment variables, injected by Terraform outputs
