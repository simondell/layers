terraform {
  required_providers {
    aws = {
      source  = "hashicorp/aws"
      version = "~> 6.27"
    }
  }
}

provider "aws" {
  region = var.aws_region
}

module "numbers" {
  source       = "./modules/lambda-service"
  service_name = "layers-numbers"
  handler      = "Numbers"
  zip_path     = "${path.root}/../build/numbers.zip"
}

module "esme_squalor" {
  source       = "./modules/lambda-service"
  service_name = "layers-esme-squalor"
  handler      = "EsmeSqualor"
  zip_path     = "${path.root}/../build/esme_squalor.zip"
}

module "orchestrator" {
  source       = "./modules/lambda-service"
  service_name = "layers-orchestrator"
  handler      = "Orchestrator"
  zip_path     = "${path.root}/../build/orchestrator.zip"

  environment_variables = {
    Services__Numbers__BaseUrl     = module.numbers.invoke_url
    Services__EsmeSqualor__BaseUrl = module.esme_squalor.invoke_url
    Cors__AllowedOrigins__0        = "http://${aws_s3_bucket_website_configuration.frontend.website_endpoint}"
  }
}

resource "aws_s3_bucket" "frontend" {
  bucket = "layers-frontend-${data.aws_caller_identity.current.account_id}"
}

data "aws_caller_identity" "current" {}

resource "aws_s3_bucket_website_configuration" "frontend" {
  bucket = aws_s3_bucket.frontend.id

  index_document {
    suffix = "index.html"
  }
}

resource "aws_s3_bucket_public_access_block" "frontend" {
  bucket                  = aws_s3_bucket.frontend.id
  block_public_acls       = false
  block_public_policy     = false
  ignore_public_acls      = false
  restrict_public_buckets = false
}

resource "aws_s3_bucket_policy" "frontend" {
  bucket     = aws_s3_bucket.frontend.id
  depends_on = [aws_s3_bucket_public_access_block.frontend]

  policy = jsonencode({
    Version = "2012-10-17"
    Statement = [{
      Effect    = "Allow"
      Principal = "*"
      Action    = "s3:GetObject"
      Resource  = "${aws_s3_bucket.frontend.arn}/*"
    }]
  })
}

