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
  $deepDive = $null
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

  switch -Regex ($projName) {
    '^MD\.CMS\.WebApi\.Core\.AwsLambda$' {
      $deepDive = @'
## Cloud setup deep dive

**Setup path**
- Start from `aws-lambda-tools-defaults.json` for region/profile/stack defaults.
- Verify Lambda handler + runtime compatibility (template defaults still reference older runtime values).
- Ensure VPC IDs, CORS/session domain, plugin folder settings, and DB plugin config values are environment-specific.

**Required elements**
- AWS account + deploy credentials/profile.
- S3 bucket for deployment package and a unique stack name.
- `serverless.template` parameters for network, plugin layers, and config overrides.
- Runtime config values for data-access plugins and e-mail/session behavior.

**Effects in the system**
- Publishes the main REST API endpoint shape consumed by admin/client applications.
- Controls API timeout behavior and plugin loading location in Lambda (`/opt/...`).
- Drives infrastructure updates through CloudFormation/SAM deployment parameters.
'@
      break
    }
    '^MD\.CMS\.WebApi\.Core\.AwsLambda\.Container$' {
      $deepDive = @'
## Cloud setup deep dive

**Setup path**
- Use `aws-lambda-tools-defaults.json` plus container-oriented template parameters (`BasePluginsLayer`, `ProductLayer`, `WebAppPath`, `AppReferencePath`).
- Keep image/package versioning aligned with CI/CD and layer ARNs.
- Validate base path/stage routing and plugin directory mount expectations.

**Required elements**
- Lambda container-capable deployment flow (ECR/image publishing in your release pipeline).
- Stable layer ARNs for shared plugins and product assemblies.
- Stack/network parameters (VPC subnets/security groups, timeout, trace level).

**Effects in the system**
- Changes deployment surface from zip-function style to container-style runtime packaging.
- Allows larger dependency footprints and explicit filesystem layout under `/opt`.
- Any stage/base-folder mismatch directly affects reachable API routes.
'@
      break
    }
    '^MD\.CMS\.WebApi\.Sockets\.Core\.AwsLambda$' {
      $deepDive = @'
## Cloud setup deep dive

**Setup path**
- Configure the Lambda websocket stack defaults (`GatewayName`, VPC settings, long API timeout values).
- Validate websocket-specific route and session configuration.
- Confirm plugin/data-access settings match the websocket workload and backend DB capacity.

**Required elements**
- AWS websocket API Gateway integration and Lambda permissions.
- Network access to backend data stores from the configured VPC.
- Correct stack/stage naming so websocket clients connect to the intended endpoint.

**Effects in the system**
- Enables real-time communication channels for CMS features requiring push/stream behavior.
- Timeout and VPC settings strongly impact connection stability and throughput.
- Misconfiguration can degrade both websocket reliability and shared backend data performance.
'@
      break
    }
    '^MD\.CMS\.WebSockets\.Core\.AwsLambda\.Container$' {
      $deepDive = @'
## Cloud setup deep dive

**Setup path**
- Configure container-layer parameters (`BasePluginsLayer`, `ProductLayer`, `WebAppPath`, function entrypoint path).
- Keep websocket stage/base path values consistent with client connection URLs.
- Validate VPC + timeout settings tuned for long-lived websocket operations.

**Required elements**
- Container image publication flow and compatible Lambda runtime base image.
- Stable plugin layer references and mounted plugin directories.
- CloudFormation/SAM parameter set for gateway, networking, and observability settings.

**Effects in the system**
- Deploys websocket runtime as a containerized Lambda host, increasing packaging flexibility.
- Route/stage changes directly impact active websocket client connection endpoints.
- Network policy errors show up as connection drops or backend access failures.
'@
      break
    }
    '^MD\.CMS\.Administration\.Core\.AwsLambda$' {
      $deepDive = @'
## Cloud setup deep dive

**Setup path**
- Use `aws-lambda-tools-defaults.json` to configure stack, bucket, region, and template parameters.
- Populate admin-specific settings (`MDCMSAdministrationCorePluginsDirectory`, provider options, maps/translation keys where used).
- Confirm static admin payload and plugin directory mapping are available to the Lambda host.

**Required elements**
- AWS deploy credentials/profile and S3 bucket for artifacts.
- Correct API gateway naming/stage for administration routes.
- File-provider/plugin configuration for assets and extension loading.

**Effects in the system**
- Hosts the administration UI on Lambda with cloud-managed scaling behavior.
- Determines how admin static/plugin assets are resolved at runtime.
- Incorrect provider/plugin settings can break admin panel features even if the host starts successfully.
'@
      break
    }
    '^MD\.CMS\.Administration\.Core\.AwsLambda\.Container$' {
      $deepDive = @'
## Cloud setup deep dive

**Setup path**
- Configure container template parameters (`BasePluginsLayer`, `ProductLayer`, `WebAppPath`, startup entrypoint).
- Keep `StageName` and gateway names aligned with administration URL expectations.
- Ensure plugin and static admin assets are included in image/layer layout.

**Required elements**
- Container build/publish workflow integrated into release pipeline.
- Valid layer ARNs and stack parameters for VPC/network and trace settings.
- Consistent environment values for admin host URLs and plugin providers.

**Effects in the system**
- Moves administration hosting to containerized Lambda deployment with explicit filesystem/runtime shape.
- Faster iteration on bundled dependencies, but tighter coupling to image release cadence.
- Stage/path mismatches lead to inaccessible admin routes or broken static asset loading.
'@
      break
    }
    '^MD\.CMS\.AwsLambda\.Container\.Core$' {
      $deepDive = @'
## Cloud setup deep dive

**Setup path**
- Treat this project as shared container deployment scaffolding for Lambda-oriented hosts.
- Keep default template values minimal, then override through environment/release variables per service.
- Reuse common network/trace defaults while preserving per-application stack naming.

**Required elements**
- Shared AWS account conventions (naming, region, VPC IDs).
- Common template parameter strategy used by downstream API/admin/socket container projects.
- CI release process that injects real values for empty defaults.

**Effects in the system**
- Provides a consistent baseline for multiple Lambda container projects.
- Reduces duplication in deployment conventions across cloud-hosted services.
- Changes here can cascade into API/admin/socket deployment behavior.
'@
      break
    }
    '^MD\.CMS\.WebApi\.Core\.GoogleCloud$' {
      $deepDive = @'
## Cloud setup deep dive

**Setup path**
- Use `app.yaml` and `google-deploy.bat` as deployment entry points for Google Cloud hosting.
- Validate `Program.cs`/`Startup.cs` configuration against expected GCP environment variables.
- Keep OpenAPI artifacts (`openapi.yaml`, `swagger.json`) in sync with exposed routes.

**Required elements**
- GCP project/service account permissions for deploy/runtime operations.
- App Engine (or equivalent service) configuration in `app.yaml`.
- Runtime app settings for DB/plugins/session/CORS mapped from deployment environment.

**Effects in the system**
- Exposes REST API on Google Cloud runtime instead of AWS-hosted pathways.
- Deployment config influences scaling, routing, and request handling characteristics.
- Drift between OpenAPI docs and runtime config can impact client integrations.
'@
      break
    }
    '^MD\.CMS\.Administration\.Core\.GoogleCloud$' {
      $deepDive = @'
## Cloud setup deep dive

**Setup path**
- Use Google deployment helpers (`google-deploy.bat`, `publish-project.bat`, `create-links.bat`) and `dispatch.yaml`/`app.yaml`.
- Ensure admin static files and ASP.NET host output are published to the expected GCP runtime path.
- Confirm URL routing in `dispatch.yaml` matches admin entry URLs.

**Required elements**
- GCP deploy permissions and configured target project/service.
- Valid app routing + service mapping (`app.yaml`, `dispatch.yaml`).
- Consistent admin host URL/environment settings matching the cloud domain.

**Effects in the system**
- Hosts administration UI on GCP-managed runtime and routing layer.
- Routing or publish-script misconfiguration typically surfaces as 404/static asset issues.
- Directly affects operator access path and admin panel responsiveness.
'@
      break
    }
    '^MD\.CMS\.Administration\.Core\.AzureFunctions$' {
      $deepDive = @'
## Cloud setup deep dive

**Setup path**
- Configure startup and host behavior from `Program.cs` and `Startup.cs`.
- Align `appsettings.Development.json`/environment settings with Azure Functions app settings.
- Validate local launch profile before publishing to Azure Function App resources.

**Required elements**
- Azure subscription/resource group + Function App target.
- Function runtime-compatible settings and bindings.
- Environment values for downstream CMS/API dependencies and storage/providers.

**Effects in the system**
- Runs administration host flow inside Azure Functions hosting model.
- Function runtime constraints (cold start, binding config) influence admin startup/latency.
- Incorrect app settings can silently break dependency resolution at startup.
'@
      break
    }
    '^MD\.CMS\.BusinessLogic\.Aws\.Core$' {
      $deepDive = @'
## Cloud setup deep dive

**Setup path**
- Reference this library from AWS host projects (Lambda/API/container) rather than deploying it alone.
- Keep AWS-specific adapters/config abstractions aligned with host-level runtime values.
- Validate package version compatibility with consuming Lambda host projects.

**Required elements**
- Consuming AWS host projects that provide concrete environment and deployment settings.
- Shared contracts for file-provider/plugin and data-access behavior.

**Effects in the system**
- Centralizes AWS-targeted business logic used across multiple services.
- Changes can affect API/admin behavior simultaneously in AWS-hosted runtimes.
'@
      break
    }
    '^MD\.CMS\.BusinessLogic\.AwsLambda\.Core$' {
      $deepDive = @'
## Cloud setup deep dive

**Setup path**
- Use as shared Lambda-focused business logic dependency for API/admin Lambda hosts.
- Ensure Lambda hosts pass expected configuration keys and plugin/runtime paths.
- Keep versions aligned with dependent host projects and deployment layers.

**Required elements**
- Lambda host projects that reference this library.
- Consistent environment variable naming and plugin provider configuration.

**Effects in the system**
- Consolidates Lambda-centric business behavior and reduces duplication.
- Impacts request handling logic across every Lambda host that references it.
'@
      break
    }
    '^MD\.CMS\.BusinessLogic\.GoogleCloud\.Core$' {
      $deepDive = @'
## Cloud setup deep dive

**Setup path**
- Consume from GoogleCloud host projects and keep cloud-specific integrations scoped here.
- Validate environment/config assumptions against GCP deployment profiles.
- Verify compatibility with data-access and helper libraries used by GCP hosts.

**Required elements**
- GoogleCloud host projects that reference this core package.
- Stable configuration contract for credentials/endpoints used by GCP runtime.

**Effects in the system**
- Encapsulates cloud-provider-specific business rules for GCP deployments.
- Updates here propagate to both API/admin cloud-host behavior on Google Cloud.
'@
      break
    }
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
$(if ($deepDive) { "$deepDive`n" } else { '' })
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
