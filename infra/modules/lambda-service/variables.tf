variable "service_name" {
  description = "Name of the Lambda function and associated resources"
  type        = string
}

variable "zip_path" {
  description = "Path to the deployment zip file"
  type        = string
}

variable "handler" {
  description = "Lambda handler — assembly name for managed dotnet10 runtime"
  type        = string
}

variable "environment_variables" {
  description = "Environment variables for the Lambda function"
  type        = map(string)
  default     = {}
}

variable "timeout" {
  description = "Lambda function timeout in seconds"
  type        = number
  default     = 30
}

variable "memory_size" {
  description = "Lambda function memory in MB"
  type        = number
  default     = 256
}
