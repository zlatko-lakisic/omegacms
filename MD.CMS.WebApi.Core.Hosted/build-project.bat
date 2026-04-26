"E:\nuget.exe" restore "MD.CMS.WebApi.Core.Hosted.csproj"
set msBuildDir=%ProgramFiles(x86)%\Microsoft Visual Studio\2019\Enterprise\MSBuild\Current\Bin
call "%msBuildDir%\msbuild.exe" "MD.CMS.WebApi.Core.Hosted.csproj" /p:Configuration=%1 /l:FileLogger,Microsoft.Build.Engine /t:Clean;Compile
set msBuildDir=
pause