"E:\nuget.exe" restore "MD.Tools.Helpers.Core.csproj"
set msBuildDir=%ProgramFiles(x86)%\Microsoft Visual Studio\2019\Enterprise\MSBuild\Current\Bin
call "%msBuildDir%\msbuild.exe" "MD.Tools.Helpers.Core.csproj" /p:Configuration=%1 /l:FileLogger,Microsoft.Build.Engine /t:rebuild
set msBuildDir=
pause
