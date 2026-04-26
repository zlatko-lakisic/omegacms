dotnet clean MD.CMS.Administration.Core.GoogleCloud.csproj
dotnet publish MD.CMS.Administration.Core.GoogleCloud.csproj -c Release
gcloud app deploy --project=omegacmsrun -q bin\Release\netcoreapp3.1\publish\app.yaml
gcloud app deploy --project=omegacmsrun -q bin\Release\netcoreapp3.1\publish\dispatch.yaml