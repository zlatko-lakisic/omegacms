"E:\nuget.exe" restore "MD.CMS.WebApi.Core.AwsLambda.csproj"
set msBuildDir=%ProgramFiles(x86)%\Microsoft Visual Studio\2019\Enterprise\MSBuild\Current\Bin
call "%msBuildDir%\msbuild.exe" "MD.CMS.WebApi.Core.AwsLambda.csproj" /p:Configuration=%1 /l:FileLogger,Microsoft.Build.Engine /t:Clean;Compile
set msBuildDir=
pause