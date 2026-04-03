output "numbers_url" {
  description = "numbers service invoke URL"
  value       = module.numbers.invoke_url
}

output "esme_squalor_url" {
  description = "esme_squalor service invoke URL"
  value       = module.esme_squalor.invoke_url
}

output "orchestrator_url" {
  description = "orchestrator invoke URL"
  value       = module.orchestrator.invoke_url
}

output "frontend_url" {
  description = "S3 static website URL"
  value       = "http://${aws_s3_bucket_website_configuration.frontend.website_endpoint}"
}
