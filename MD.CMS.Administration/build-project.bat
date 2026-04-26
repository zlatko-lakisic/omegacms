cd %~dp0
"E:\nuget.exe" restore "src\MD.CMS.Administration.Core.csproj"
set msBuildDir=%ProgramFiles(x86)%\Microsoft Visual Studio\2019\Enterprise\MSBuild\Current\Bin
echo %1
call "%msBuildDir%\msbuild.exe" "src\MD.CMS.Administration.Core.csproj" /p:Configuration=%1 /l:FileLogger,Microsoft.Build.Engine
set msBuildDir=
pause
