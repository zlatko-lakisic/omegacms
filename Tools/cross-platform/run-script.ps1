param(
    [Parameter(Mandatory = $true)]
    [string] $ScriptPath,
    [Parameter(ValueFromRemainingArguments = $true)]
    [string[]] $Args
)

Set-StrictMode -Version 3.0
$ErrorActionPreference = "Stop"

function Normalize-Base([string]$inputPath) {
    $normalized = $inputPath
    foreach ($ext in @(".sh", ".ps1", ".bat")) {
        if ($normalized.EndsWith($ext, [System.StringComparison]::OrdinalIgnoreCase)) {
            $normalized = $normalized.Substring(0, $normalized.Length - $ext.Length)
        }
    }
    return $normalized
}

$base = Normalize-Base $ScriptPath
$repoRoot = (Resolve-Path (Join-Path $PSScriptRoot "..\..")).Path
$basePath =
if ([System.IO.Path]::IsPathRooted($base)) { $base }
else { Join-Path $repoRoot $base }

$shPath = "$basePath.sh"
$ps1Path = "$basePath.ps1"
$batPath = "$basePath.bat"

$isWindows = [System.Runtime.InteropServices.RuntimeInformation]::IsOSPlatform([System.Runtime.InteropServices.OSPlatform]::Windows)

if (-not $isWindows) {
    if (Test-Path -LiteralPath $shPath) {
        & bash $shPath @Args
        exit $LASTEXITCODE
    }
    if (Test-Path -LiteralPath $ps1Path) {
        if (Get-Command pwsh -ErrorAction SilentlyContinue) {
            & pwsh -NoProfile -ExecutionPolicy Bypass -File $ps1Path @Args
            exit $LASTEXITCODE
        }
        throw "No Linux .sh alternative for '$base', and 'pwsh' is not installed."
    }
    if (Test-Path -LiteralPath $batPath) {
        throw "No Linux .sh alternative for '$base' (only .bat exists)."
    }
    throw "Script not found: $base (.sh/.ps1/.bat)"
}

if (Test-Path -LiteralPath $batPath) {
    & cmd.exe /c $batPath @Args
    exit $LASTEXITCODE
}
if (Test-Path -LiteralPath $ps1Path) {
    & powershell.exe -NoProfile -ExecutionPolicy Bypass -File $ps1Path @Args
    exit $LASTEXITCODE
}
if (Test-Path -LiteralPath $shPath) {
    & bash $shPath @Args
    exit $LASTEXITCODE
}

throw "Script not found: $base (.bat/.ps1/.sh)"
