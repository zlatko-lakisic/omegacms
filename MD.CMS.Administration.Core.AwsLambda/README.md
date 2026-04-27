<p align="center">
  <img src="Assets/banner.png" alt="OmegaCMS" width="100%" />
</p>

# MD.CMS.Administration.Core.AwsLambda

**AWS Lambda** host for the **administration** web app. See `aws-lambda-tools-defaults.json`. May include template text from the AWS .NET sample in addition to OmegaCMS assets.

## Project metadata

- **Product** (`.csproj`): `OmegaCMS Aws Lambda Administration`
- **Target framework:** `net10.0`

## Responsibilities

- Implements the primary project role described above.
- Exposes contracts, runtime behavior, or host wiring consumed by sibling projects in `MD.CMS.Core.sln`.
- Uses repository-level configuration and environment conventions documented in the wiki.

## Cloud/runtime notes

- **AWS**: This project participates in AWS deployments (Lambda, container image, or shared AWS integration logic).
- **AWS project type** (`AWSProjectType`): `Lambda`.
- **Lambda runtime**: Validate handler/bootstrap configuration and environment variables before packaging and deploy.

## Build

From the repository root:

    dotnet build .\MD.CMS.Administration.Core.AwsLambda\MD.CMS.Administration.Core.AwsLambda.csproj -c Debug

## Optional local run

If this project is an executable host, run:

    dotnet run --project .\MD.CMS.Administration.Core.AwsLambda\MD.CMS.Administration.Core.AwsLambda.csproj

Library projects should usually be consumed through a host project instead of running directly.

## Key files

- `MD.CMS.Administration.Core.AwsLambda\MD.CMS.Administration.Core.AwsLambda.csproj`
- `aws-lambda-tools-defaults.json` (AWS deployment defaults)

## Documentation

- [Solution layout](https://github.com/zlatko-lakisic/omegacms/wiki/Solution-Layout)
- [Build and run](https://github.com/zlatko-lakisic/omegacms/wiki/Build-and-Run)
- [AWS and serverless](https://github.com/zlatko-lakisic/omegacms/wiki/AWS-and-Serverless)
- [OmegaCMS solution wiki](https://github.com/zlatko-lakisic/omegacms/wiki)
- [Omega IT LLC](https://omega-it.solutions)
