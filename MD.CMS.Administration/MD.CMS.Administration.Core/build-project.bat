"E:\nuget.exe" restore "MD.CMS.Administration.Core.csproj"
set msBuildDir=%ProgramFiles(x86)%\Microsoft Visual Studio\2019\Enterprise\MSBuild\Current\Bin
call "%msBuildDir%\msbuild.exe" "MD.CMS.Administration.Core.csproj" /p:Configuration=%1 /l:FileLogger,Microsoft.Build.Engine /t:rebuild
set msBuildDir=

call "%~dp0..\..\3rdParty\docfx.console\2.56.4\tools\docfx.exe" "%~dp0..\docfx\docfx.json"
robocopy "E:\documentation" "%~dp0..\MD.CMS.Administration.Core.Web\scripts\documentation" /COPYALL /E
rmdir "E:\documentation" /s/q
pause
