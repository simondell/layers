terraform {
  required_providers {
    aws = {
      source  = "hashicorp/aws"
      version = "~> 5.0"
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
  }
}
