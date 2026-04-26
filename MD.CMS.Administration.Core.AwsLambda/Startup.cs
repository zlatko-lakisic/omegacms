using MD.CMS.BusinessLogic.Administration.Core.Addons;
using MD.CMS.BusinessLogic.Aws.Core.ConfigParsers;
using MD.CMS.BusinessLogic.Aws.Core.FileProviders.S3;
using MD.Tools.Helpers.Core.Config;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MD.CMS.Administration.Core.AwsLambda
{
    public class Startup : Core.Startup
    {
        public static string StartupPath 
        {
            get
            {
                return _basePath;
            }
            set
            {
                _basePath = value;
            }
        }

        public Startup(IConfiguration configuration) : base(configuration)
        {
            Tools.Helpers.Core.FileProvider.DynamicFileProvider.AddFileProvider<AWSS3FileProvider>();
            ConfigParser.Providers.Add(new LambdaConfigParser());
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public override void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            AdminAddonAppBuilder.Default.AdminSystemVersion = System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version.ToString();
            base.Configure(app, env);
        }
    }
}