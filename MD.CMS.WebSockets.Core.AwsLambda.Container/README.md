<p align="center">
  <img src="Assets/banner.png" alt="OmegaCMS" width="100%" />
</p>

# MD.CMS.WebSockets.Core.AwsLambda.Container

**WebSocket** support on **AWS** (container or Lambda, depending on the project).

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

## Build

From the repository root:

    dotnet build .\MD.CMS.WebSockets.Core.AwsLambda.Container\MD.CMS.WebSockets.Core.AwsLambda.Container.csproj -c Debug

## Optional local run

If this project is an executable host, run:

    dotnet run --project .\MD.CMS.WebSockets.Core.AwsLambda.Container\MD.CMS.WebSockets.Core.AwsLambda.Container.csproj

Library projects should usually be consumed through a host project instead of running directly.

## Key files

- `MD.CMS.WebSockets.Core.AwsLambda.Container\MD.CMS.WebSockets.Core.AwsLambda.Container.csproj`
- `aws-lambda-tools-defaults.json` (AWS deployment defaults)

## Documentation

- [Solution layout](https://github.com/zlatko-lakisic/omegacms/wiki/Solution-Layout)
- [Build and run](https://github.com/zlatko-lakisic/omegacms/wiki/Build-and-Run)
- [AWS and serverless](https://github.com/zlatko-lakisic/omegacms/wiki/AWS-and-Serverless)
- [OmegaCMS solution wiki](https://github.com/zlatko-lakisic/omegacms/wiki)
- [Omega IT LLC](https://omega-it.solutions)
