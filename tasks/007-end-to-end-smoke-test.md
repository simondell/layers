# Task 007 — End-to-end smoke test

Verify the deployed stack works and document it.

## Steps

1. Run `terraform apply` from `infra/`
2. Note the three API Gateway invoke URLs from `terraform output`
3. Smoke-test each leaf service directly:
   - `curl <numbers-url>/number` — should return `{ "value": <int> }`
   - `curl <esme-url>/verdict?number=99` — should return `{ "verdict": "in" }`
4. Smoke-test the orchestrator:
   - `curl <orchestrator-url>/result?count=3` — should return `{ "total": <int> }`
5. Update `frontend/app.js` with the deployed orchestrator URL
6. Open `frontend/index.html` in a browser and verify end-to-end

## Documentation

Update `README.md` with:
- Deployed API Gateway URLs
- Local development instructions (`dotnet run` port assignments)
- How to run tests (`dotnet test layers.sln`)

## Acceptance criteria

- All three smoke tests pass
- Frontend works against the deployed orchestrator
- README is up to date
