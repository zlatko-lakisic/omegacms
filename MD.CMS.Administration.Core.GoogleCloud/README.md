<p align="center">
  <img src="Assets/banner.png" alt="OmegaCMS" width="100%" />
</p>

# MD.CMS.Administration.Core.GoogleCloud

Administration UI host for **Google Cloud**.

## Project metadata

- **Product** (`.csproj`): `OmegaCMS Google Cloud App Engine Administration`
- **Target framework:** `net10.0`

## Responsibilities

- Implements the primary project role described above.
- Exposes contracts, runtime behavior, or host wiring consumed by sibling projects in `MD.CMS.Core.sln`.
- Uses repository-level configuration and environment conventions documented in the wiki.

## Cloud/runtime notes

- **Google Cloud**: Align service configuration, credentials, and environment mapping with platform conventions.

## Build

From the repository root:

    dotnet build .\MD.CMS.Administration.Core.GoogleCloud\MD.CMS.Administration.Core.GoogleCloud.csproj -c Debug

## Optional local run

If this project is an executable host, run:

    dotnet run --project .\MD.CMS.Administration.Core.GoogleCloud\MD.CMS.Administration.Core.GoogleCloud.csproj

Library projects should usually be consumed through a host project instead of running directly.

## Key files

- `MD.CMS.Administration.Core.GoogleCloud\MD.CMS.Administration.Core.GoogleCloud.csproj`

## Documentation

- [Solution layout](https://github.com/zlatko-lakisic/omegacms/wiki/Solution-Layout)
- [Build and run](https://github.com/zlatko-lakisic/omegacms/wiki/Build-and-Run)
- [AWS and serverless](https://github.com/zlatko-lakisic/omegacms/wiki/AWS-and-Serverless)
- [OmegaCMS solution wiki](https://github.com/zlatko-lakisic/omegacms/wiki)
- [Omega IT LLC](https://omega-it.solutions)
