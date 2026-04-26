cd %~dp0
call "%msBuildDir%\msbuild.exe" /t:Restore
dotnet-sonarscanner begin /k:"Omega-CMS-Server-Side" /d:sonar.host.url="http://sonarqube.omegacms.io" /d:sonar.login="%1"
set msBuildDir=%ProgramFiles(x86)%\Microsoft Visual Studio\2019\Enterprise\MSBuild\Current\Bin
call "%msBuildDir%\msbuild.exe" /t:Rebuild
dotnet-sonarscanner end /d:sonar.login="%1"