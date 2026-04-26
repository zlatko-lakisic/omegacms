Param
(
    [Parameter(Mandatory=$true)] [string]$version
)

try {
    $jsonPackage = Get-Content ".\\omega-cms-businesslogic\\package.json" -raw | ConvertFrom-Json
    $jsonPackage.version = $version
    $jsonPackage | ConvertTo-Json -depth 32| set-content ".\\omega-cms-businesslogic\\package.json"

    npm publish -access public
} catch {
  Write-Host "An error occurred:"
  Write-Host $_
}