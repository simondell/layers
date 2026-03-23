# Task 006 — GitHub Actions CI

Add two workflow files.

## ci.yml

Trigger: `pull_request` on any path under `services/`

Steps:
1. `actions/checkout`
2. `actions/setup-dotnet` with `dotnet-version: 8.x`
3. `dotnet restore layers.sln`
4. `dotnet build layers.sln --no-restore --configuration Release`
5. `dotnet test layers.sln --no-build --configuration Release`

## tf-plan.yml

Trigger: `pull_request` on any path under `infra/`

Steps:
1. `actions/checkout`
2. `hashicorp/setup-terraform`
3. `terraform init`
4. `terraform validate`
5. `terraform plan` (using `AWS_ACCESS_KEY_ID` + `AWS_SECRET_ACCESS_KEY` from GitHub secrets)
6. Post the plan output as a PR comment

## Acceptance criteria

- A PR touching `services/` triggers `ci.yml` and all tests pass
- A PR touching `infra/` triggers `tf-plan.yml` and posts a plan comment
- AWS credentials are stored as GitHub Actions secrets (not in the workflow file)
