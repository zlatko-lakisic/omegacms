using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using MD.CMS.BusinessLogic.Core.Properties;
using MD.CMS.BusinessLogic.Administration.Core.Addons;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Net;
using MD.Tools.Helpers.Core.Web.Formatters;
using System;
using MD.Tools.Helpers.Core.Logging;
using System.IO;

namespace MD.CMS.Administration.Core.Modules
{
    public class RegisterRootAngularModuleAddons
    { 
        private readonly RequestDelegate _next;
        private JavaScriptFormatter _outputFormatter;
        private IHttpResponseStreamWriterFactory _streamWriterFactory;
        private static IEnumerable<IAdminAngularModuleJavaScript> scripts = new List<IAdminAngularModuleJavaScript>();

        public RegisterRootAngularModuleAddons(RequestDelegate next, JavaScriptFormatter outputFormatter, IHttpResponseStreamWriterFactory streamWriterFactory)
        {
            _next = next;
            _outputFormatter = outputFormatter;
            _streamWriterFactory = streamWriterFactory;
        }

        public async Task Invoke(HttpContext context)
        {
            if (IsModulesScript(context))
            {
                await BeginRequest(context);
            }
            else
            {
                await _next.Invoke(context);
            }
        }

        private bool IsModulesScript(HttpContext context)
        {
            return context.Request.Path.ToString().ToLowerInvariant().Contains(Settings.Default.AdministrationAngularRootModuleRegisterUrl.ToLowerInvariant());
        }

        private async Task BeginRequest(HttpContext context)
        {
            try
            {
                string modules = Settings.Default.AdministrationAddonsModuleCode;
                bool modulesFound = false;
                if (scripts != null && scripts.Any())
                {
                    scripts = scripts.Where(s => s.IsRootAngularModule);
                    if (scripts.Any())
                    {
                        modules = modules.Replace("#modulesGoHere#", string.Join(",", scripts.Select(s => string.Format("'{0}'", s.ModuleName))), StringComparison.InvariantCulture);
                        modulesFound = true;
                    }
                }

                if (!modulesFound)
                {
                    modules = modules.Replace("#modulesGoHere#", string.Empty);
                }

                context.Response.ContentType = "text/javascript";
                context.Response.StatusCode = (int)HttpStatusCode.OK;
                await context.Response.WriteAsync(modules);
            }
            catch(Exception e)
            {
                Tools.Helpers.Core.Logging.Logger.Log(e);
            }
        }

        public static void Preload()
        {
            scripts = MD.Tools.Helpers.Core.Plugins.PluginLoader<IAdminAngularModuleJavaScript>.GetAll(Properties.Settings.Default.PluginsFileProviderType, Properties.Settings.Default.PluginsDirectory);
        }
    }
}
