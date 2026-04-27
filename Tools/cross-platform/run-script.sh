#!/usr/bin/env bash
set -euo pipefail

if [[ $# -lt 1 ]]; then
  echo "Usage: $0 <script-path-without-extension|script-path> [args...]"
  exit 1
fi

target="$1"
shift || true

normalize_base() {
  local input="$1"
  input="${input%.sh}"
  input="${input%.ps1}"
  input="${input%.bat}"
  printf "%s" "$input"
}

base="$(normalize_base "$target")"
script_dir="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
repo_root="$(cd "${script_dir}/../.." && pwd)"

resolve_path() {
  local candidate="$1"
  if [[ "$candidate" = /* ]]; then
    printf "%s" "$candidate"
  else
    printf "%s/%s" "$repo_root" "$candidate"
  fi
}

base_path="$(resolve_path "$base")"
sh_path="${base_path}.sh"
ps1_path="${base_path}.ps1"
bat_path="${base_path}.bat"

is_windows=false
case "${OSTYPE:-}" in
  msys*|cygwin*|win32*) is_windows=true ;;
esac

if [[ "$is_windows" == false ]]; then
  if [[ -f "$sh_path" ]]; then
    exec bash "$sh_path" "$@"
  fi
  if [[ -f "$ps1_path" ]]; then
    if command -v pwsh >/dev/null 2>&1; then
      exec pwsh -NoProfile -ExecutionPolicy Bypass -File "$ps1_path" "$@"
    fi
    echo "No Linux .sh alternative for '$base', and 'pwsh' is not installed."
    exit 2
  fi
  if [[ -f "$bat_path" ]]; then
    echo "No Linux .sh alternative for '$base' (only .bat exists)."
    exit 3
  fi
  echo "Script not found: $base (.sh/.ps1/.bat)"
  exit 4
fi

# Windows host
if [[ -f "$bat_path" ]]; then
  exec cmd.exe /c "$bat_path" "$@"
fi
if [[ -f "$ps1_path" ]]; then
  exec powershell.exe -NoProfile -ExecutionPolicy Bypass -File "$ps1_path" "$@"
fi
if [[ -f "$sh_path" ]]; then
  exec bash "$sh_path" "$@"
fi

echo "Script not found: $base (.bat/.ps1/.sh)"
exit 4
