#Requires -Version 5.1
<#
.SYNOPSIS
  Generates README.md next to each *.csproj under the solution root (one README per project folder).
#>
$ErrorActionPreference = 'Stop'
# Script lives in <repo>\Tools; solution root is parent directory
$root = Split-Path $PSScriptRoot -Parent
if (-not (Test-Path (Join-Path $root 'MD.CMS.Core.sln'))) {
  throw "Run this script from the repository (expected MD.CMS.Core.sln next to the Tools folder). Current root: $root"
}

$repoWiki = 'https://github.com/zlatko-lakisic/omegacms/wiki'
# Per-project banner: place `Assets/banner.png` next to the README in each project folder
$bannerImageRelativeToReadme = 'Assets/banner.png'

function Get-ReadmeBody {
  param(
    [string]$projName,
    [string]$relCsproj,
    [string]$tfm,
    [string]$product,
    [string]$isPack,
    [string]$awsType,
    [string]$bannerImageSrc,
    [string]$projectDir
  )

  $bannerHtml = @"
<p align="center">
  <img src="$bannerImageSrc" alt="OmegaCMS" width="100%" />
</p>

"@

  $isTestProject = ($projName -match 'Tests$' -or $projName -match 'CoreTests$' -or $projName -match '\.Tests$')
  $isAws = ($projName -match 'Aws' -or $projName -match 'Lambda' -or $awsType)
  $isContainer = ($projName -match 'Container')
  $isGoogleCloud = ($projName -match 'GoogleCloud')
  $isAzure = ($projName -match 'AzureFunctions')
  $isHosted = ($projName -match 'Hosted')

  $cloudNotes = @()
  if ($isAws) {
    $cloudNotes += '- **AWS**: This project participates in AWS deployments (Lambda, container image, or shared AWS integration logic).'
    if ($awsType) { $cloudNotes += ('- **AWS project type** (`AWSProjectType`): `{0}`.' -f $awsType) }
  }
  if ($projName -match 'Lambda') {
    $cloudNotes += '- **Lambda runtime**: Validate handler/bootstrap configuration and environment variables before packaging and deploy.'
  }
  if ($isContainer) {
    $cloudNotes += '- **Container packaging**: Keep image tag/versioning aligned with deployment scripts or CI release variables.'
  }
  if ($isGoogleCloud) {
    $cloudNotes += '- **Google Cloud**: Align service configuration, credentials, and environment mapping with platform conventions.'
  }
  if ($isAzure) {
    $cloudNotes += '- **Azure Functions**: Confirm trigger/binding config and app settings for function-hosted execution.'
  }
  if ($isHosted) {
    $cloudNotes += '- **Hosted ASP.NET Core**: Run with local launch profiles (IIS Express or Kestrel) for day-to-day development.'
  }

  $hasAwsDefaults = Test-Path (Join-Path $projectDir 'aws-lambda-tools-defaults.json')
  $hasDockerfile = Test-Path (Join-Path $projectDir 'Dockerfile')
  $keyFiles = @('- `{0}`' -f $relCsproj)
  if ($hasAwsDefaults) { $keyFiles += '- `aws-lambda-tools-defaults.json` (AWS deployment defaults)' }
  if ($hasDockerfile) { $keyFiles += '- `Dockerfile` (container image build definition)' }

  if ($isTestProject) {
    return @"
$bannerHtml
# $projName

Test project for OmegaCMS that validates behavior, integration points, or deployment-specific wiring for related production projects.

## What this project covers

- Unit, integration, or host-configuration tests for the corresponding feature area.
- Regression protection for refactors, runtime upgrades, and configuration updates.

## Run tests

From the repository root:

    dotnet test .\$relCsproj

**Target framework:** $tfm

## Key files

$(($keyFiles + @()) -join "`n")

## Documentation

- [Testing]($repoWiki/Testing)
- [Solution wiki]($repoWiki)
- [Omega IT LLC](https://omega-it.solutions)

"@
  }

  $blurb = $null
  switch -Regex ($projName) {
    '^MD\.CMS\.BusinessLogic\.Core$' { $blurb = 'Core business rules, services, and domain logic. Consumed by Web API, administration, and serverless hosts. XML documentation is emitted in **Debug** builds.'; break }
    '^MD\.CMS\.BusinessLogic\.WebApi\.Core$' { $blurb = 'Bridges the core business layer to the REST/Web API (controllers and integration points).'; break }
    '^MD\.CMS\.BusinessLogic\.Administration\.Core$' { $blurb = 'Administration-oriented business rules and support for the operator UI.'; break }
    '^MD\.CMS\.BusinessLogic\.Aws\.Core$' { $blurb = 'AWS-specific business logic shared by Lambda and related API hosts.'; break }
    '^MD\.CMS\.BusinessLogic\.AwsLambda\.Core$' { $blurb = 'Business logic for the **AWS Lambda** packaging of the API and admin stack.'; break }
    '^MD\.CMS\.BusinessLogic\.GoogleCloud\.Core$' { $blurb = 'Google Cloud–related business services (for example storage integration).'; break }
    '^MD\.CMS\.WebApi\.Core$' { $blurb = 'Main REST API layer (controllers, filters, middleware) — referenced by all Web API **host** projects (hosted, Lambda, Google Cloud).'; break }
    '^MD\.CMS\.WebApi\.Core\.Hosted$' { $blurb = '**Hosted** ASP.NET Core app exposing the Web API under IIS or Kestrel for normal server deployments and local development.'; break }
    '^MD\.CMS\.WebApi\.Core\.AwsLambda$' {
      $blurb = '**AWS Lambda** host for the Web API using [Amazon.Lambda.AspNetCoreServer](https://www.nuget.org/packages/Amazon.Lambda.AspNetCoreServer). See `aws-lambda-tools-defaults.json`. This project started from the AWS .NET serverless Web API sample; OmegaCMS customizations live alongside that template.'
      break
    }
    '^MD\.CMS\.WebApi\.Core\.GoogleCloud$' { $blurb = 'Web API host for **Google Cloud** (ASP.NET Core).'; break }
    '^MD\.CMS\.WebApi\.Core\.AwsLambda\.Container$' { $blurb = '**Container** image for the Web API on AWS Lambda (container-based deployment).'; break }
    '^MD\.CMS\.WebApi\.Sockets' { $blurb = '**WebSocket** / real-time API on **AWS Lambda**.'; break }
    '^MD\.CMS\.WebSockets' { $blurb = '**WebSocket** support on **AWS** (container or Lambda, depending on the project).'; break }
    '^MD\.CMS\.WebApi\.Core\.AwsLambda\.Tests$' { $blurb = 'Automated tests for the Lambda Web API host and related configuration.'; break }
    '^MD\.CMS\.Administration\.Core\.AwsLambda\.Container$' { $blurb = '**Container** image for the **administration** Lambda host.'; break }
    '^MD\.CMS\.Administration\.Core\.AwsLambda$' {
      $blurb = '**AWS Lambda** host for the **administration** web app. See `aws-lambda-tools-defaults.json`. May include template text from the AWS .NET sample in addition to OmegaCMS assets.'
      break
    }
    '^MD\.CMS\.Administration\.Core\.GoogleCloud$' { $blurb = 'Administration UI host for **Google Cloud**.'; break }
    '^MD\.CMS\.Administration\.Core\.AzureFunctions$' { $blurb = 'Administration entry point for **Azure Functions** hosting.'; break }
    '^MD\.CMS\.Administration\.Core\.Hosted$' { $blurb = '**Hosted** ASP.NET Core site for the operator administration experience (IIS / Kestrel).'; break }
    '^MD\.CMS\.Administration\.Core$' {
      $blurb = 'Core **admin** host and bootstrapping for the static administration app. The client app, **Yarn**, and **Gulp** live under the parent `MD.CMS.Administration` folder.'
      break
    }
    '^MD\.CMS\.Installer\.Hosted\.Core$' { $blurb = 'Hosted **installer** flow for deploying Administration, Web API, and related components to a server.'; break }
    '^MD\.CMS\.AwsLambda\.Container\.Core$' { $blurb = 'Shared **container** packaging for AWS Lambda images used by the solution.'; break }
    '^MD\.CMS\.Tools\.BaseDataAccess\.PluginMethods$' { $blurb = 'CMS-specific **plugin methods** built on `MD.Tools.BaseDataAccess.PluginMethods.Core`.'; break }
    '^MD\.CMS\.Template$' { $blurb = '**Fuse/Angular** template site and modern app (`src/modern-app`) for demos and client integration patterns. Not the main production admin host.'; break }
    '^MD\.Tools\.AsyncTask\.Processor$' { $blurb = 'Background **async task** worker process (queue or job–driven processing per configuration).'; break }
    '^MD\.Tools\.Licensing$' { $blurb = 'Licensing validation and helpers used by the platform and tests.'; break }
    '^MD\.Tools\.Helpers\.Core$' { $blurb = 'Shared helper libraries (e.g. e-mail, utilities) referenced across the solution.'; break }
    '^MD\.Tools\.BaseDataAccess\.Core$' { $blurb = 'Core **data access** abstractions and shared types.'; break }
    '^MD\.Tools\.BaseDataAccess\.Plugins\.Core$' { $blurb = 'Plugin model for **database and file** providers.'; break }
    '^MD\.Tools\.BaseDataAccess\.PluginMethods\.Core$' { $blurb = 'Dynamic **plugin methods** infrastructure for data access and rules.'; break }
  }

  if (-not $blurb) { $blurb = "OmegaCMS component **$projName**. See the project file and solution layout for exact references and responsibilities." }

  $meta = @()
  if ($product) { $meta += ('- **Product** (`.csproj`): `{0}`' -f $product) }
  if ($isPack -eq 'false') { $meta += '- **Packable:** no (application/host/test project).' }
  if ($isPack -eq 'true') { $meta += '- **Packable:** yes (can produce a NuGet package when packed).' }
  $meta += ('- **Target framework:** `{0}`' -f $tfm)

  $responsibilities = @(
    '- Implements the primary project role described above.'
    '- Exposes contracts, runtime behavior, or host wiring consumed by sibling projects in `MD.CMS.Core.sln`.'
    '- Uses repository-level configuration and environment conventions documented in the wiki.'
  )

  $cloudSection = if ($cloudNotes.Count) { @"
## Cloud/runtime notes

$(($cloudNotes + @()) -join "`n")

"@ } else { '' }

  return @"
$bannerHtml
# $projName

$blurb

## Project metadata

$(($meta + @()) -join "`n")

## Responsibilities

$(($responsibilities + @()) -join "`n")

$cloudSection
## Build

From the repository root:

    dotnet build .\$relCsproj -c Debug

## Optional local run

If this project is an executable host, run:

    dotnet run --project .\$relCsproj

Library projects should usually be consumed through a host project instead of running directly.

## Key files

$(($keyFiles + @()) -join "`n")

## Documentation

- [Solution layout]($repoWiki/Solution-Layout)
- [Build and run]($repoWiki/Build-and-Run)
- [AWS and serverless]($repoWiki/AWS-and-Serverless)
- [OmegaCMS solution wiki]($repoWiki)
- [Omega IT LLC](https://omega-it.solutions)

"@
}

$csprojs = Get-ChildItem $root -Recurse -Filter '*.csproj' | Where-Object { $_.FullName -notmatch '\\(obj|bin)(\\|/)' }
foreach ($f in $csprojs) {
  $path = $f.FullName
  $dir = $f.DirectoryName
  $projName = $f.BaseName
  $relCsproj = $path.Substring($root.Length).TrimStart('\') -replace '/', '\'
  [xml]$xml = [IO.File]::ReadAllText($path)
  $pgs = @($xml.Project.PropertyGroup)
  $tfm = $null
  $product = $null
  $isPack = $null
  $aws = $null
  foreach ($g in $pgs) {
    if (-not $tfm -and $g.TargetFramework) { $tfm = [string]$g.TargetFramework }
    if (-not $product -and $g.Product) { $product = [string]$g.Product }
    if ($null -eq $isPack -and $g.IsPackable) { $isPack = [string]$g.IsPackable }
    if (-not $aws -and $g.AWSProjectType) { $aws = [string]$g.AWSProjectType }
  }
  if (-not $tfm) { $tfm = 'net10.0' }

  $body = Get-ReadmeBody -projName $projName -relCsproj $relCsproj -tfm $tfm -product $product -isPack $isPack -awsType $aws -bannerImageSrc $bannerImageRelativeToReadme -projectDir $dir
  $readmePath = Join-Path $dir 'README.md'
  $legacy = Join-Path $dir 'Readme.md'
  if (Test-Path $legacy) { Remove-Item $legacy -Force }
  [IO.File]::WriteAllText($readmePath, $body.TrimEnd() + "`n", [Text.UTF8Encoding]::new($false))
  Write-Output "Wrote $readmePath"
}

Write-Output "Done. $($csprojs.Count) README.md files."
