output "invoke_url" {
  description = "API Gateway invoke URL"
  value       = aws_apigatewayv2_stage.default.invoke_url
}

output "function_arn" {
  description = "Lambda function ARN"
  value       = aws_lambda_function.service.arn
}
