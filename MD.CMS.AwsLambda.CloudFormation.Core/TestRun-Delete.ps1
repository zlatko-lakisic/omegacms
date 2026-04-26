# Removes the stack; requires OMEGA_CF_STACK_NAME in TestRun.env (see TestRun.env.example).
Set-StrictMode -Version 3.0
$ErrorActionPreference = "Stop"

$root = $PSScriptRoot
. (Join-Path $root "Powershell\Import-TestRunEnv.ps1")
Import-TestRunEnv -RootPath $root
Assert-TestRunRequiredEnv -Names @("OMEGA_CF_STACK_NAME")

. (Join-Path $root "Powershell\Delete.ps1") -stackName $env:OMEGA_CF_STACK_NAME
