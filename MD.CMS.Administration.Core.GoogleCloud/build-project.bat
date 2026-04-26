cd %~dp0
"E:\nuget.exe" restore "MD.CMS.Administration.Core.Hosted.csproj"
set msBuildDir=%ProgramFiles(x86)%\Microsoft Visual Studio\2019\Enterprise\MSBuild\Current\Bin
echo %1
call "%msBuildDir%\msbuild.exe" "MD.CMS.Administration.Core.Hosted.csproj" /p:Configuration=%1 /l:FileLogger,Microsoft.Build.Engine
set msBuildDir=
pause
