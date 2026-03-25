#!/bin/bash
set -e

ROOT="$(cd "$(dirname "$0")" && pwd)"
PUBLISH_FLAGS="-c Release -r linux-x64 --self-contained -p:AssemblyName=bootstrap --nologo -v quiet"

services=("numbers" "esme_squalor" "orchestrator")

for service in "${services[@]}"; do
  csproj=$(ls "$ROOT/services/$service"/*.csproj)
  echo "Building $service..."
  dotnet publish "$csproj" $PUBLISH_FLAGS -o "$ROOT/build/$service"
  (cd "$ROOT/build/$service" && zip -r "$ROOT/build/$service.zip" . -x "*.pdb")
  echo "  -> build/$service.zip"
done

echo ""
echo "All services built. To deploy:"
echo "  cd infra && terraform init && terraform apply"
