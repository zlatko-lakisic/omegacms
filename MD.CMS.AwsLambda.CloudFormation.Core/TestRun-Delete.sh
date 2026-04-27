#!/usr/bin/env bash
set -euo pipefail

root="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
source "${root}/Powershell/Import-TestRunEnv.sh"

import_test_run_env "$root"
assert_test_run_required_env OMEGA_CF_STACK_NAME

"${root}/Powershell/Delete.sh" -stackName "${OMEGA_CF_STACK_NAME}"
