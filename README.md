# layers

A learning project exploring C#, AWS Lambda, Terraform, and GitHub Actions.

## What it does

A simple web page with buttons labelled 1, 2, 3. Click a button to send that number to the backend. The backend:

1. Calls the **numbers** service N times to get N random numbers (1–100)
2. Asks **esme_squalor** whether each number is "in" or "out"
3. Sums the "in" numbers and returns the total (or 0)

## Services

| Service | Role |
|---|---|
| `orchestrator` | BFF — receives requests from the frontend, coordinates the other two services |
| `numbers` | Returns a random number between 1 and 100 |
| `esme_squalor` | Returns "in" if the supplied number exceeds today's day-name score, otherwise "out" |

### Esme's rule

The day score is the sum of the alphabetic positions (A=1 … Z=26) of the three letters in today's abbreviated day name:

| Day | Score |
|---|---|
| MON | 42 |
| TUE | 46 |
| WED | 32 |
| THU | 49 |
| FRI | 33 |
| SAT | 40 |
| SUN | 54 |

A number is **in** if `number > day score`.

## Stack

- **Frontend:** plain HTML, CSS, vanilla JS
- **Backend:** C# .NET 8, ASP.NET Core Minimal API (local) + AWS Lambda (deployed)
- **Infrastructure:** Terraform (AWS — Lambda + API Gateway v2)
- **CI:** GitHub Actions

## Local development

_Instructions to follow once services are scaffolded._

## Deployment

_Instructions to follow once Terraform is wired up._
