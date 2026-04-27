#!/usr/bin/env bash
set -euo pipefail

script_dir="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
"$script_dir/init-npm.sh"
"$script_dir/init-bower.sh"
"$script_dir/init-iis.sh"
