# Cross-platform script runner

Use these wrappers to execute repository scripts on Windows and Linux/macOS without changing command names manually.

## Entry points

- `run-script.ps1` (PowerShell)
- `run-script.sh` (bash)

Both call the shared runner under `Tools/cross-platform/`.

## Usage

```powershell
./run-script.ps1 get-version MD.CMS.WebApi.Core/MD.CMS.WebApi.Core.csproj
./run-script.ps1 MD.CMS.AwsLambda.CloudFormation.Core/TestRun-Create
```

```bash
./run-script.sh get-version MD.CMS.WebApi.Core/MD.CMS.WebApi.Core.csproj
./run-script.sh MD.CMS.AwsLambda.CloudFormation.Core/TestRun-Create
```

Pass script path either:

- without extension (`get-version`, `MD.CMS.Template/init`)
- or with extension (`.bat`, `.ps1`, `.sh`)

The runner chooses the best match by OS:

- **Linux/macOS**: `.sh` -> `.ps1` (via `pwsh`) -> fail on `.bat`
- **Windows**: `.bat` -> `.ps1` -> `.sh`

## Linux alternatives added

- Root utility scripts: `get-version.sh`, `version-update.sh`, `scan-code.sh`
- Template scripts: `MD.CMS.Template/init.sh`, `init-npm.sh`, `init-bower.sh`, `init-iis.sh`
- CloudFormation test scripts:
  - `MD.CMS.AwsLambda.CloudFormation.Core/TestRun-Create.sh`
  - `MD.CMS.AwsLambda.CloudFormation.Core/TestRun-Update.sh`
  - `MD.CMS.AwsLambda.CloudFormation.Core/TestRun-Delete.sh`
  - `MD.CMS.AwsLambda.CloudFormation.Core/Powershell/*.sh` wrappers

## Notes

- Some legacy Windows-specific scripts (IIS / `.bat` build helpers) still require Windows tools.
- CloudFormation `.sh` wrappers reuse existing PowerShell logic via `pwsh` to keep behavior consistent.
