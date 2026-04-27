<p align="center">
  <img src="Assets/banner.png" alt="OmegaCMS" width="100%" />
</p>

# MD.CMS.AwsLambda.Container.Core

Shared **container** packaging for AWS Lambda images used by the solution.

## Project metadata

- **Product** (`.csproj`): `OmegaCMS Aws Lambda Container`
- **Target framework:** `net10.0`

## Responsibilities

- Implements the primary project role described above.
- Exposes contracts, runtime behavior, or host wiring consumed by sibling projects in `MD.CMS.Core.sln`.
- Uses repository-level configuration and environment conventions documented in the wiki.

## Cloud/runtime notes

- **AWS**: This project participates in AWS deployments (Lambda, container image, or shared AWS integration logic).
- **AWS project type** (`AWSProjectType`): `Lambda`.
- **Lambda runtime**: Validate handler/bootstrap configuration and environment variables before packaging and deploy.
- **Container packaging**: Keep image tag/versioning aligned with deployment scripts or CI release variables.

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

## Build

From the repository root:

    dotnet build .\MD.CMS.AwsLambda.Container.Core\MD.CMS.AwsLambda.Container.Core.csproj -c Debug

## Optional local run

If this project is an executable host, run:

    dotnet run --project .\MD.CMS.AwsLambda.Container.Core\MD.CMS.AwsLambda.Container.Core.csproj

Library projects should usually be consumed through a host project instead of running directly.

## Key files

- `MD.CMS.AwsLambda.Container.Core\MD.CMS.AwsLambda.Container.Core.csproj`
- `aws-lambda-tools-defaults.json` (AWS deployment defaults)

## Documentation

- [Solution layout](https://github.com/zlatko-lakisic/omegacms/wiki/Solution-Layout)
- [Build and run](https://github.com/zlatko-lakisic/omegacms/wiki/Build-and-Run)
- [AWS and serverless](https://github.com/zlatko-lakisic/omegacms/wiki/AWS-and-Serverless)
- [OmegaCMS solution wiki](https://github.com/zlatko-lakisic/omegacms/wiki)
- [Omega IT LLC](https://omega-it.solutions)
