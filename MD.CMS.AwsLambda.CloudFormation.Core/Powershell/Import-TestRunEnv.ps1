# Loads test-run secrets and IDs from a dotenv-style file next to the TestRun-*.ps1 scripts.
# Copy TestRun.env.example to TestRun.env in the project root and set values. TestRun.env is gitignored.
Set-StrictMode -Version 3.0
$ErrorActionPreference = "Stop"

function Import-TestRunEnv {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $true)]
        [string] $RootPath
    )
    $path = Join-Path $RootPath "TestRun.env"
    if (-not (Test-Path -LiteralPath $path)) { return }
    Get-Content -LiteralPath $path -Encoding UTF8 | ForEach-Object {
        $line = $_.Trim()
        if ($line.Length -eq 0) { return }
        if ($line.StartsWith("#")) { return }
        $eq = $line.IndexOf("=")
        if ($eq -lt 1) { return }
        $name = $line.Substring(0, $eq).Trim()
        $value = $line.Substring($eq + 1).Trim()
        if ($name.Length -eq 0) { return }
        [System.Environment]::SetEnvironmentVariable($name, $value, "Process")
    }
}

function Assert-TestRunRequiredEnv {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $true)]
        [string[]] $Names
    )
    $missing = @()
    foreach ($n in $Names) {
        $v = [System.Environment]::GetEnvironmentVariable($n, "Process")
        if ([string]::IsNullOrWhiteSpace($v)) { $missing += $n }
    }
    if ($missing.Count -gt 0) {
        $list = $missing -join ", "
        throw "Missing required environment variable(s): $list. Copy TestRun.env.example to TestRun.env, fill in values, or set these in your session before running."
    }
}
