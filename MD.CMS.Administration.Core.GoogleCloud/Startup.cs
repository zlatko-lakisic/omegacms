using HealthChecks.UI.Client;
using MD.CMS.BusinessLogic.Administration.Core.Addons;
using MD.CMS.BusinessLogic.GoogleCloud.Core.ConfigParsers;
using MD.CMS.BusinessLogic.GoogleCloud.Core.FileProviders;
using MD.Tools.Helpers.Core.Config;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

namespace MD.CMS.Administration.Core.GoogleCloud
{
    public class Startup : Core.Startup
    {
        public Startup(IConfiguration configuration) : base(configuration)
        {
            ConfigParser.Providers.Add(new GoogleConfigParser());
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
            services.AddHealthChecks();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public override void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            AdminAddonAppBuilder.Default.AdminSystemVersion = System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version.ToString();
            base.Configure(app, env);

            AppBuilder.ApplicationBuilder.UseFileServer(new FileServerOptions
            {
                FileProvider = new GoogleCloudStorageFileProvider(MD.CMS.Administration.Core.Properties.Settings.Default.UploadsRootPath),
                RequestPath = new PathString(BusinessLogic.Core.Properties.Settings.Default.FileUploadPath),
                EnableDirectoryBrowsing = false
            });

            app.UseRouting();
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