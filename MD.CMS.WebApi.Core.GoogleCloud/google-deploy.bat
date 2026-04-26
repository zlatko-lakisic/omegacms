dotnet clean
dotnet publish MD.CMS.WebApi.Core.GoogleCloud.csproj -c Release
gcloud app deploy --project=omegacmsrun -q bin\Release\netcoreapp3.1\publish\app.yaml