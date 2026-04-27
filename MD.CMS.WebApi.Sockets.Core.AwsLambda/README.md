<p align="center">
  <img src="Assets/banner.png" alt="OmegaCMS" width="100%" />
</p>

# MD.CMS.WebApi.Sockets.Core.AwsLambda

**WebSocket** / real-time API on **AWS Lambda**.

## Project metadata

- **Product** (`.csproj`): `OmegaCMS  Aws Lambda Web API Socket Services`
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

## Build

From the repository root:

    dotnet build .\MD.CMS.WebApi.Sockets.Core.AwsLambda\MD.CMS.WebApi.Sockets.Core.AwsLambda.csproj -c Debug

## Optional local run

If this project is an executable host, run:

    dotnet run --project .\MD.CMS.WebApi.Sockets.Core.AwsLambda\MD.CMS.WebApi.Sockets.Core.AwsLambda.csproj

Library projects should usually be consumed through a host project instead of running directly.

## Key files

- `MD.CMS.WebApi.Sockets.Core.AwsLambda\MD.CMS.WebApi.Sockets.Core.AwsLambda.csproj`
- `aws-lambda-tools-defaults.json` (AWS deployment defaults)

## Documentation

- [Solution layout](https://github.com/zlatko-lakisic/omegacms/wiki/Solution-Layout)
- [Build and run](https://github.com/zlatko-lakisic/omegacms/wiki/Build-and-Run)
- [AWS and serverless](https://github.com/zlatko-lakisic/omegacms/wiki/AWS-and-Serverless)
- [OmegaCMS solution wiki](https://github.com/zlatko-lakisic/omegacms/wiki)
- [Omega IT LLC](https://omega-it.solutions)
