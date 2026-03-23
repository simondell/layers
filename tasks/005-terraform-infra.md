# Task 005 — Terraform infrastructure

Define all AWS infrastructure as code.

## Resources (per service, × 3)

- `aws_lambda_function` — .NET 8 managed runtime, deployment package from `build/` zip
- `aws_iam_role` + `aws_iam_role_policy_attachment` — Lambda execution role with basic execution policy
- `aws_apigatewayv2_api` — HTTP API
- `aws_apigatewayv2_stage` — `$default` stage with auto-deploy
- `aws_apigatewayv2_integration` — Lambda proxy integration
- `aws_apigatewayv2_route` — catch-all `ANY /{proxy+}`
- `aws_lambda_permission` — allows API Gateway to invoke the Lambda

## Module structure

- `infra/modules/lambda-service/` — reusable module instantiated once per service
- `infra/main.tf` — instantiates the module 3×, passes orchestrator the leaf service URLs as env vars
- `infra/outputs.tf` — exports all three invoke URLs

## Acceptance criteria

- `terraform init` succeeds
- `terraform validate` passes
- `terraform plan` produces a sensible plan (no errors)
- After `terraform apply`, all three API Gateway URLs respond correctly
