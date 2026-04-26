using MD.CMS.BusinessLogic.WebApi.Core.WebSockets;
using MD.CMS.WebApi.Core.BusinessLogic;
using MD.CMS.WebApi.Core.BusinessLogic.WebSockets.Message;
using MD.CMS.WebApi.Core.BusinessLogic.WebSockets.Permissions;
using MD.CMS.WebApi.Core.BusinessLogic.WebSockets.SystemInfo;
using MD.CMS.WebApi.Core.BusinessLogic.WebSockets.User;
using MD.CMS.WebApi.Core.Hosted.BusinessLogic;
using MD.CMS.WebApi.Core.Hosted.BusinessLogic.Middleware;
using MD.CMS.WebApi.Core.Properties;
using MD.Tools.Helpers.Core.Plugins;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;

namespace MD.CMS.WebApi.Core.Hosted
{
    public class Startup : Core.Startup
    {
        internal static ConcurrentDictionary<string, IOmegaWebSocket> Sockets;

        public Startup(IConfiguration configuration) : base(configuration)
        {
            _swaggerXmlFilePath = AppContext.BaseDirectory;
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
            services.AddSingleton<WebSocketServerConnectionManager>();
        }

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
                app.UseMiddleware<LicenseMiddleware>();
                app.UseMiddleware<WebSocketServerMiddleware>();
            });
        }
    }
}
