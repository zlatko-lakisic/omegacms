cd %~dp0
"E:\nuget.exe" restore "MD.CMS.Administration.Core.AwsLambda.csproj"
set msBuildDir=%ProgramFiles(x86)%\Microsoft Visual Studio\2019\Enterprise\MSBuild\Current\Bin
call "%msBuildDir%\msbuild.exe" "MD.CMS.Administration.Core.AwsLambda.csproj" /l:FileLogger,Microsoft.Build.Engine
set msBuildDir=

aws s3 mb s3://%1
aws s3 sync ..\MD.CMS.Administration\MD.CMS.Administration.Core.Web\ s3://%1