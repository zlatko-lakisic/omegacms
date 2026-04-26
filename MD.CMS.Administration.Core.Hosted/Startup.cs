using MD.CMS.BusinessLogic.Administration.Core.Addons;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;

namespace MD.CMS.Administration.Core.Hosted
{
    public class Startup : Core.Startup
    {

        public Startup(IConfiguration configuration) : base(configuration, true)
        {
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public override void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            AdminAddonAppBuilder.Default.AdminSystemVersion = System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version.ToString();
            base.Configure(app, env);

            if (env.IsDevelopment())
            {
                AppBuilder.ApplicationBuilder.UseFileServer(new FileServerOptions
                {
                    FileProvider = new PhysicalFileProvider(env.ContentRootPath + "\\node_modules"),
                    RequestPath = new PathString("/js/ext"),
                    EnableDirectoryBrowsing = false
                });
            }

            AppBuilder.ApplicationBuilder.UseFileServer(new FileServerOptions
            {
                FileProvider = new PhysicalFileProvider(MD.CMS.Administration.Core.Properties.Settings.Default.UploadsRootPath),
                RequestPath = new PathString(BusinessLogic.Core.Properties.Settings.Default.FileUploadPath),
                EnableDirectoryBrowsing = false
            });
        }
    }
}
