param(
    [Parameter(Mandatory = $true)]
    [string] $ScriptPath,
    [Parameter(ValueFromRemainingArguments = $true)]
    [string[]] $Args
)

Set-StrictMode -Version 3.0
$ErrorActionPreference = "Stop"

$runner = Join-Path $PSScriptRoot "Tools\cross-platform\run-script.ps1"
& $runner -ScriptPath $ScriptPath @Args
exit $LASTEXITCODE
