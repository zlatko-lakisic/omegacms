using HealthChecks.UI.Client;
using MD.CMS.BusinessLogic.GoogleCloud.Core.ConfigParsers;
using MD.CMS.BusinessLogic.WebApi.Core.WebSockets;
using MD.CMS.WebApi.Core.BusinessLogic.WebSockets.Message;
using MD.CMS.WebApi.Core.BusinessLogic.WebSockets.Permissions;
using MD.CMS.WebApi.Core.BusinessLogic.WebSockets.SystemInfo;
using MD.CMS.WebApi.Core.BusinessLogic.WebSockets.User;
using MD.CMS.WebApi.Core.GoogleCloud.BusinessLogic;
using MD.Tools.Helpers.Core.Config;
using MD.Tools.Helpers.Core.Logging;
using MD.Tools.Helpers.Core.Plugins;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using MD.CMS.WebApi.Core.Properties;
using MD.CMS.WebApi.Core.GoogleCloud.BusinessLogic.Middleware;

namespace MD.CMS.WebApi.Core.GoogleCloud
{
    public class Startup : Core.Startup
    {
        internal static ConcurrentDictionary<string, IOmegaWebSocket> Sockets;

        public Startup(IConfiguration configuration) : base(configuration)
        {
            _swaggerXmlFilePath = AppContext.BaseDirectory;
            ConfigParser.Providers.Add(new GoogleConfigParser());
            LicenseValidSetter = new LicenseValidDelegate(GoogleLicenseValidator);
            (typeof(Startup)).LogVerbose("OmegaCMS - Google Cloud Startup - Constructor");
        }

        public static async Task<bool> GoogleLicenseValidator(HttpContext context)
        {
            return true;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
            services.AddHealthChecks();
            services.AddSingleton<WebSocketServerConnectionManager>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public override void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory logger)
        {
            base.ConfigureWithActions(app, env, logger, preMiddleWareAction: (app, env, logger) => {
                Sockets = new ConcurrentDictionary<string, IOmegaWebSocket>();
                Sockets.TryAdd(string.Join('-', new GetUnreadByUserSocket().UrlsToBindTo), new GetUnreadByUserSocket());
                Sockets.TryAdd(string.Join('-', (new ValidateTokenSocket()).UrlsToBindTo), new ValidateTokenSocket());
                Sockets.TryAdd(string.Join('-', new GetAllJobs().UrlsToBindTo), new GetAllJobs());
                Sockets.TryAdd(string.Join('-', new UserPermissionsSocket().UrlsToBindTo), new UserPermissionsSocket());
                Sockets.TryAdd(string.Join('-', new ProfileTypePermissionsSocket().UrlsToBindTo), new ProfileTypePermissionsSocket());

                foreach (IOmegaWebSocket socket in PluginLoader<IOmegaWebSocket>.GetAll(Settings.Default.PluginsFileProviderType, Settings.Default.PluginsDirectory))
                {
                    if (!Sockets.ContainsKey(string.Join('-', socket.UrlsToBindTo)))
                    {
                        Sockets.TryAdd(string.Join('-', socket.UrlsToBindTo), socket);
                    }
                }

                app.UseWebSockets();
                app.UseMiddleware<WebSocketServerMiddleware>();
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/readiness_check", new HealthCheckOptions()
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
                });
                endpoints.MapHealthChecks("/liveness_check", new HealthCheckOptions()
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
                });
            });
        }
    }
}
