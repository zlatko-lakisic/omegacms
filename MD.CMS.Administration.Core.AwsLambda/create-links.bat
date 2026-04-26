rmdir wwwroot\assets
rmdir wwwroot\css
rmdir wwwroot\js
rmdir wwwroot\scripts
rmdir node_modules

mkdir wwwroot
mkdir wwwroot\scripts
mklink /d wwwroot\assets ..\..\MD.CMS.Administration\MD.CMS.Administration.Core.Web\assets
mklink /d wwwroot\css ..\..\MD.CMS.Administration\MD.CMS.Administration.Core.Web\css
mklink /d wwwroot\js ..\..\MD.CMS.Administration\MD.CMS.Administration.Core.Web\js
mklink /d wwwroot\scripts\app ..\..\..\MD.CMS.Administration\MD.CMS.Administration.Core.Web\scripts\app
mklink /d wwwroot\scripts\businessLogic_ts ..\..\..\MD.CMS.Administration\MD.CMS.Administration.Core.Web\scripts\businessLogic_ts
mklink /d wwwroot\scripts\businessLogicMinified ..\..\..\MD.CMS.Administration\MD.CMS.Administration.Core.Web\scripts\businessLogicMinified
mklink /d wwwroot\scripts\plugins ..\..\..\MD.CMS.Administration\MD.CMS.Administration.Core.Web\scripts\plugins
mklink /d node_modules ..\MD.CMS.Administration\node_modules