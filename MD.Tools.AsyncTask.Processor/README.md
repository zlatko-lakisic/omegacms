<p align="center">
  <img src="Assets/banner.png" alt="OmegaCMS" width="100%" />
</p>

# MD.Tools.AsyncTask.Processor

Background **async task** worker process (queue or jobâ€“driven processing per configuration).

## Project metadata

- **Product** (`.csproj`): `Omega Async Task Processor`
- **Target framework:** `net10.0`

## Responsibilities

- Implements the primary project role described above.
- Exposes contracts, runtime behavior, or host wiring consumed by sibling projects in `MD.CMS.Core.sln`.
- Uses repository-level configuration and environment conventions documented in the wiki.



## Build

From the repository root:

    dotnet build .\MD.Tools.AsyncTask.Processor\MD.Tools.AsyncTask.Processor.csproj -c Debug

## Optional local run

If this project is an executable host, run:

    dotnet run --project .\MD.Tools.AsyncTask.Processor\MD.Tools.AsyncTask.Processor.csproj

Library projects should usually be consumed through a host project instead of running directly.

## Key files

- `MD.Tools.AsyncTask.Processor\MD.Tools.AsyncTask.Processor.csproj`

## Documentation

- [Solution layout](https://github.com/zlatko-lakisic/omegacms/wiki/Solution-Layout)
- [Build and run](https://github.com/zlatko-lakisic/omegacms/wiki/Build-and-Run)
- [AWS and serverless](https://github.com/zlatko-lakisic/omegacms/wiki/AWS-and-Serverless)
- [OmegaCMS solution wiki](https://github.com/zlatko-lakisic/omegacms/wiki)
- [Omega IT LLC](https://omega-it.solutions)
