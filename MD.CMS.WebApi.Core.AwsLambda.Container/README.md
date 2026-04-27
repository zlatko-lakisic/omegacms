<p align="center">
  <img src="Assets/banner.png" alt="OmegaCMS" width="100%" />
</p>

# MD.CMS.WebApi.Core.AwsLambda.Container

**Container** image for the Web API on AWS Lambda (container-based deployment).

## Project metadata

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

## Build

From the repository root:

    dotnet build .\MD.CMS.WebApi.Core.AwsLambda.Container\MD.CMS.WebApi.Core.AwsLambda.Container.csproj -c Debug

## Optional local run

If this project is an executable host, run:

    dotnet run --project .\MD.CMS.WebApi.Core.AwsLambda.Container\MD.CMS.WebApi.Core.AwsLambda.Container.csproj

Library projects should usually be consumed through a host project instead of running directly.

## Key files

- `MD.CMS.WebApi.Core.AwsLambda.Container\MD.CMS.WebApi.Core.AwsLambda.Container.csproj`
- `aws-lambda-tools-defaults.json` (AWS deployment defaults)

## Documentation

- [Solution layout](https://github.com/zlatko-lakisic/omegacms/wiki/Solution-Layout)
- [Build and run](https://github.com/zlatko-lakisic/omegacms/wiki/Build-and-Run)
- [AWS and serverless](https://github.com/zlatko-lakisic/omegacms/wiki/AWS-and-Serverless)
- [OmegaCMS solution wiki](https://github.com/zlatko-lakisic/omegacms/wiki)
- [Omega IT LLC](https://omega-it.solutions)
