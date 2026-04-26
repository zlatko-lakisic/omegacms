setlocal enableDelayedExpansion
set "newValue=0.89.15"

type "MD.CMS.Administration.Core.Hosted\MD.CMS.Administration.Core.Hosted.csproj"|jrepl "(<Version>).*(</Version>)" "$1!newValue!$2" >fileName.xml.new
move /y "fileName.xml.new" "MD.CMS.Administration.Core.Hosted\MD.CMS.Administration.Core.Hosted.csproj"
type "MD.CMS.WebApi.Core.Hosted\MD.CMS.WebApi.Core.Hosted.csproj"|jrepl "(<Version>).*(</Version>)" "$1!newValue!$2" >fileName.xml.new
move /y "fileName.xml.new" "MD.CMS.WebApi.Core.Hosted\MD.CMS.WebApi.Core.Hosted.csproj"
type "MD.Tools.AsyncTask.Processor\MD.Tools.AsyncTask.Processor.csproj"|jrepl "(<Version>).*(</Version>)" "$1!newValue!$2" >fileName.xml.new
move /y "fileName.xml.new" "MD.Tools.AsyncTask.Processor\MD.Tools.AsyncTask.Processor.csproj"
type "MD.CMS.Installer.Hosted.Core\MD.CMS.Installer.Hosted.Core.csproj"|jrepl "(<Version>).*(</Version>)" "$1!newValue!$2" >fileName.xml.new
move /y "fileName.xml.new" "MD.CMS.Installer.Hosted.Core\MD.CMS.Installer.Hosted.Core.csproj"

type "MD.CMS.Administration.Core.GoogleCloud\MD.CMS.Administration.Core.GoogleCloud.csproj"|jrepl "(<Version>).*(</Version>)" "$1!newValue!$2" >fileName.xml.new
move /y "fileName.xml.new" "MD.CMS.Administration.Core.GoogleCloud\MD.CMS.Administration.Core.GoogleCloud.csproj"
type "MD.CMS.WebApi.Core.GoogleCloud\MD.CMS.WebApi.Core.GoogleCloud.csproj"|jrepl "(<Version>).*(</Version>)" "$1!newValue!$2" >fileName.xml.new
move /y "fileName.xml.new" "MD.CMS.WebApi.Core.GoogleCloud\MD.CMS.WebApi.Core.GoogleCloud.csproj"

type "MD.CMS.Administration.Core.AwsLambda\MD.CMS.Administration.Core.AwsLambda.csproj"|jrepl "(<Version>).*(</Version>)" "$1!newValue!$2" >fileName.xml.new
move /y "fileName.xml.new" "MD.CMS.Administration.Core.AwsLambda\MD.CMS.Administration.Core.AwsLambda.csproj"
type "MD.CMS.WebApi.Core.AwsLambda\MD.CMS.WebApi.Core.AwsLambda.csproj"|jrepl "(<Version>).*(</Version>)" "$1!newValue!$2" >fileName.xml.new
move /y "fileName.xml.new" "MD.CMS.WebApi.Core.AwsLambda\MD.CMS.WebApi.Core.AwsLambda.csproj"
type "MD.CMS.WebApi.Sockets.Core.AwsLambda\MD.CMS.WebApi.Sockets.Core.AwsLambda.csproj"|jrepl "(<Version>).*(</Version>)" "$1!newValue!$2" >fileName.xml.new
move /y "fileName.xml.new" "MD.CMS.WebApi.Sockets.Core.AwsLambda\MD.CMS.WebApi.Sockets.Core.AwsLambda.csproj"