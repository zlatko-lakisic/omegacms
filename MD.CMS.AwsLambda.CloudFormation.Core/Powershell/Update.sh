#!/usr/bin/env bash
set -euo pipefail

script_dir="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
pwsh_script="${script_dir}/Update.ps1"

if command -v pwsh >/dev/null 2>&1; then
  exec pwsh -NoProfile -ExecutionPolicy Bypass -File "$pwsh_script" "$@"
fi

echo "Update.sh requires PowerShell (pwsh) to run the existing deployment logic."
exit 2
