#!/usr/bin/env bash
set -euo pipefail

if [[ $# -lt 1 ]]; then
  echo "Usage: $0 <csproj-path>"
  exit 1
fi

script_dir="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
project_path="${script_dir}/$1"

if [[ ! -f "$project_path" ]]; then
  echo "file '$1' does not exist"
  exit 4
fi

python3 - "$project_path" <<'PY'
import re
import sys
from pathlib import Path

content = Path(sys.argv[1]).read_text(encoding="utf-8")
match = re.search(r"<Version>(.*?)</Version>", content, re.IGNORECASE | re.DOTALL)
if not match:
    sys.exit(2)
print(match.group(1).strip())
PY
