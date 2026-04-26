"E:\nuget.exe" restore "MD.CMS.BusinessLogic.Core.csproj"
set msBuildDir=%ProgramFiles(x86)%\Microsoft Visual Studio\2019\Enterprise\MSBuild\Current\Bin
call "%msBuildDir%\msbuild.exe" "MD.CMS.BusinessLogic.Core.csproj" /p:Configuration=%1 /l:FileLogger,Microsoft.Build.Engine
set msBuildDir=
pause
