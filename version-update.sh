#!/usr/bin/env bash
set -euo pipefail

script_dir="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
new_value="${1:-0.89.15}"

files=(
  "MD.CMS.Administration.Core.Hosted/MD.CMS.Administration.Core.Hosted.csproj"
  "MD.CMS.WebApi.Core.Hosted/MD.CMS.WebApi.Core.Hosted.csproj"
  "MD.Tools.AsyncTask.Processor/MD.Tools.AsyncTask.Processor.csproj"
  "MD.CMS.Installer.Hosted.Core/MD.CMS.Installer.Hosted.Core.csproj"
  "MD.CMS.Administration.Core.GoogleCloud/MD.CMS.Administration.Core.GoogleCloud.csproj"
  "MD.CMS.WebApi.Core.GoogleCloud/MD.CMS.WebApi.Core.GoogleCloud.csproj"
  "MD.CMS.Administration.Core.AwsLambda/MD.CMS.Administration.Core.AwsLambda.csproj"
  "MD.CMS.WebApi.Core.AwsLambda/MD.CMS.WebApi.Core.AwsLambda.csproj"
  "MD.CMS.WebApi.Sockets.Core.AwsLambda/MD.CMS.WebApi.Sockets.Core.AwsLambda.csproj"
)

python3 - "$script_dir" "$new_value" "${files[@]}" <<'PY'
import re
import sys
from pathlib import Path

root = Path(sys.argv[1])
new_value = sys.argv[2]
targets = sys.argv[3:]

for rel in targets:
    target = root / rel
    if not target.exists():
        print(f"skip (not found): {rel}")
        continue
    content = target.read_text(encoding="utf-8")
    updated = re.sub(r"(<Version>).*?(</Version>)", rf"\1{new_value}\2", content, count=1, flags=re.IGNORECASE | re.DOTALL)
    target.write_text(updated, encoding="utf-8")
    print(f"updated: {rel}")
PY
