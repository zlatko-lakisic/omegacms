<p align="center">
  <img src="Assets/banner.png" alt="OmegaCMS" width="100%" />
</p>

# MD.CMS.WebApi.Core.AwsLambda

**AWS Lambda** host for the Web API using [Amazon.Lambda.AspNetCoreServer](https://www.nuget.org/packages/Amazon.Lambda.AspNetCoreServer). See `aws-lambda-tools-defaults.json`. This project started from the AWS .NET serverless Web API sample; OmegaCMS customizations live alongside that template.

## Project metadata

- **Product** (`.csproj`): `OmegaCMS  Aws Lambda Web API Restful Services`
- **Target framework:** `net10.0`

## Responsibilities

- Implements the primary project role described above.
- Exposes contracts, runtime behavior, or host wiring consumed by sibling projects in `MD.CMS.Core.sln`.
- Uses repository-level configuration and environment conventions documented in the wiki.

## Cloud/runtime notes

- **AWS**: This project participates in AWS deployments (Lambda, container image, or shared AWS integration logic).
- **AWS project type** (`AWSProjectType`): `Lambda`.
- **Lambda runtime**: Validate handler/bootstrap configuration and environment variables before packaging and deploy.

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

## Build

From the repository root:

    dotnet build .\MD.CMS.WebApi.Core.AwsLambda\MD.CMS.WebApi.Core.AwsLambda.csproj -c Debug

## Optional local run

If this project is an executable host, run:

    dotnet run --project .\MD.CMS.WebApi.Core.AwsLambda\MD.CMS.WebApi.Core.AwsLambda.csproj

Library projects should usually be consumed through a host project instead of running directly.

## Key files

- `MD.CMS.WebApi.Core.AwsLambda\MD.CMS.WebApi.Core.AwsLambda.csproj`
- `aws-lambda-tools-defaults.json` (AWS deployment defaults)

## Documentation

- [Solution layout](https://github.com/zlatko-lakisic/omegacms/wiki/Solution-Layout)
- [Build and run](https://github.com/zlatko-lakisic/omegacms/wiki/Build-and-Run)
- [AWS and serverless](https://github.com/zlatko-lakisic/omegacms/wiki/AWS-and-Serverless)
- [OmegaCMS solution wiki](https://github.com/zlatko-lakisic/omegacms/wiki)
- [Omega IT LLC](https://omega-it.solutions)
