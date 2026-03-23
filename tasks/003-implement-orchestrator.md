# Task 003 — Implement orchestrator

Wire up the BFF logic.

## Endpoint

`GET /result?count=<1|2|3>`

1. Call `numbers` service `count` times (sequentially or in parallel — parallel preferred)
2. For each number received, call `esme_squalor /verdict?number=<n>`
3. Sum all numbers whose verdict is `"in"`
4. Return `{ "total": <sum> }` (0 if none are "in")

## Configuration

The leaf service base URLs must come from environment variables / configuration:

| Variable | Local default | AWS value |
|---|---|---|
| `Services__Numbers__BaseUrl` | `http://localhost:5001` | API Gateway URL (injected by Terraform) |
| `Services__EsmeSqualor__BaseUrl` | `http://localhost:5002` | API Gateway URL (injected by Terraform) |

Use named `HttpClient` registrations (not raw `new HttpClient()`).

## Acceptance criteria

- `GET /result?count=1` returns `{ "total": <n> }` where n is 0 or a number 1–100
- `GET /result?count=3` makes 3 calls to numbers and 3 calls to esme_squalor
- Unit tests mock both HttpClients and verify the summation logic
- Integration test uses `WebApplicationFactory` with stub leaf service URLs
