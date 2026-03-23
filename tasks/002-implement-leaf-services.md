# Task 002 — Implement leaf services

Add real behaviour to `numbers` and `esme_squalor`.

## numbers

- `GET /number` — returns a JSON response `{ "value": <random int 1–100> }`
- Uses `System.Random` (or `RandomNumberGenerator` for better distribution)

## esme_squalor

- `GET /verdict?number=<int>` — returns `{ "verdict": "in" }` or `{ "verdict": "out" }`
- Rule: number is **in** if `number > day_score`
- Day score = sum of alphabetic positions (A=1…Z=26) of today's 3-letter day abbreviation
  - MON=42, TUE=46, WED=32, THU=49, FRI=33, SAT=40, SUN=54

## Acceptance criteria

- `GET /number` returns a number between 1 and 100
- `GET /verdict?number=99` returns `"in"` on any day (99 beats every day score)
- `GET /verdict?number=1` returns `"out"` on any day (1 loses to every day score)
- Unit tests cover the day-score calculation and the in/out boundary logic
