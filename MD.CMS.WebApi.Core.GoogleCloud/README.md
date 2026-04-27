<p align="center">
  <img src="Assets/banner.png" alt="OmegaCMS" width="100%" />
</p>

# MD.CMS.WebApi.Core.GoogleCloud

Web API host for **Google Cloud** (ASP.NET Core).

## Project metadata

- **Product** (`.csproj`): `OmegaCMS Google Cloud App Engine Web API`
- **Target framework:** `net10.0`

## Responsibilities

- Implements the primary project role described above.
- Exposes contracts, runtime behavior, or host wiring consumed by sibling projects in `MD.CMS.Core.sln`.
- Uses repository-level configuration and environment conventions documented in the wiki.

## Cloud/runtime notes

- **Google Cloud**: Align service configuration, credentials, and environment mapping with platform conventions.

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

## Build

From the repository root:

    dotnet build .\MD.CMS.WebApi.Core.GoogleCloud\MD.CMS.WebApi.Core.GoogleCloud.csproj -c Debug

## Optional local run

If this project is an executable host, run:

    dotnet run --project .\MD.CMS.WebApi.Core.GoogleCloud\MD.CMS.WebApi.Core.GoogleCloud.csproj

Library projects should usually be consumed through a host project instead of running directly.

## Key files

- `MD.CMS.WebApi.Core.GoogleCloud\MD.CMS.WebApi.Core.GoogleCloud.csproj`

## Documentation

- [Solution layout](https://github.com/zlatko-lakisic/omegacms/wiki/Solution-Layout)
- [Build and run](https://github.com/zlatko-lakisic/omegacms/wiki/Build-and-Run)
- [AWS and serverless](https://github.com/zlatko-lakisic/omegacms/wiki/AWS-and-Serverless)
- [OmegaCMS solution wiki](https://github.com/zlatko-lakisic/omegacms/wiki)
- [Omega IT LLC](https://omega-it.solutions)
