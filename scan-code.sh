#!/usr/bin/env bash
set -euo pipefail

if [[ $# -lt 1 ]]; then
  echo "Usage: $0 <sonar-token>"
  exit 1
fi

token="$1"
script_dir="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

cd "$script_dir"
dotnet restore
dotnet-sonarscanner begin /k:"Omega-CMS-Server-Side" /d:sonar.host.url="http://sonarqube.omegacms.io" /d:sonar.login="$token"
dotnet build -t:Rebuild
dotnet-sonarscanner end /d:sonar.login="$token"
