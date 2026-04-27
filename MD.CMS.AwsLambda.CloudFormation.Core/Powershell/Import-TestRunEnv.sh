#!/usr/bin/env bash
set -euo pipefail

import_test_run_env() {
  local root_path="$1"
  local path="${root_path}/TestRun.env"
  [[ -f "$path" ]] || return 0

  while IFS= read -r line || [[ -n "$line" ]]; do
    line="$(echo "$line" | sed -e 's/^[[:space:]]*//' -e 's/[[:space:]]*$//')"
    [[ -z "$line" || "${line:0:1}" == "#" ]] && continue
    if [[ "$line" == *"="* ]]; then
      local key="${line%%=*}"
      local value="${line#*=}"
      key="$(echo "$key" | sed -e 's/^[[:space:]]*//' -e 's/[[:space:]]*$//')"
      value="$(echo "$value" | sed -e 's/^[[:space:]]*//' -e 's/[[:space:]]*$//')"
      export "$key=$value"
    fi
  done < "$path"
}

assert_test_run_required_env() {
  local missing=()
  for name in "$@"; do
    if [[ -z "${!name:-}" ]]; then
      missing+=("$name")
    fi
  done
  if [[ ${#missing[@]} -gt 0 ]]; then
    echo "Missing required environment variable(s): ${missing[*]}."
    echo "Copy TestRun.env.example to TestRun.env and fill values, or export them before running."
    return 1
  fi
}
