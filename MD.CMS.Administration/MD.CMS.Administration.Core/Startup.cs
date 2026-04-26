using MD.CMS.Administration.Core.Handlers;
using MD.CMS.Administration.Core.Properties;
using MD.CMS.Administration.Core.Modules;
using MD.CMS.BusinessLogic.Administration.Core.Resources;
using MD.Tools.Helpers.Core.Config;
using MD.Tools.Helpers.Core.Plugins;
using MD.Tools.Helpers.Core.Web.Formatters;
using MD.Tools.Helpers.Core.Web.Stream;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.AspNetCore.HttpOverrides;
using MD.CMS.BusinessLogic.Administration.Core.Addons;
using WebMarkupMin.AspNetCore3;
using System;
using System.Linq;

namespace MD.CMS.Administration.Core
{
    public class Startup
    {
        private static IEnumerable<IAdminAddonLoader> _adminAddonLoaders;
        public AdminAddonAppBuilder _appBuilder;
        private static bool _checkLicense;
        protected static string _basePath = ReflectionHelper.GetDefaultPluginPath;

        public static bool GetCheckLicense()
        {
            return _checkLicense;
        }

        public IConfiguration Configuration { get; }
        public static IEnumerable<IAdminAddonLoader> AdminAddonLoaders { get => _adminAddonLoaders; }
        public AdminAddonAppBuilder AppBuilder { get => _appBuilder; }

        public Startup(IConfiguration configuration, bool checkLicense = false)
        {
            Configuration = configuration;
            _checkLicense = checkLicense;
        }

        public virtual void ConfigureServices(IServiceCollection services)
        {
            Console.WriteLine($"Path is: {_basePath}");

            IEnumerable<IConfigParsable> basicSettings = PluginLoader<IConfigParsable>.GetAll((int)MD.Tools.Helpers.Core.FileProvider.FileProviderEnum.Hosted, _basePath);

            Console.WriteLine($"Basic settings loaded are : {string.Join(", ", basicSettings.Select(b => b.SectionName))}");

            foreach (IConfigParsable obj in basicSettings)
            {
                obj.GetStaticInstance().Parse(Configuration.GetSection("Config"));
            }
            foreach (IConfigParsable obj in PluginLoader<IPluginConfigParsable>.GetAll(Settings.Default.PluginsFileProviderType, Settings.Default.PluginsDirectory))
            {
                obj.GetStaticInstance().Parse(Configuration.GetSection("Config"));
            }

            _adminAddonLoaders = PluginLoader<IAdminAddonLoader>.GetAll(Settings.Default.PluginsFileProviderType, Settings.Default.PluginsDirectory);

            foreach (IAdminAddonLoader loader in _adminAddonLoaders)
            {
                loader.ParseConfiguration(Configuration);
            }

            // Load all resources
            ResourceManager.Loadedresources.Add("SupportedLanguages", MD.CMS.Administration.Core.Resources.SupportedLanguages.ResourceManager);
            ResourceManager.Loadedresources.Add("Labels", MD.CMS.Administration.Core.Resources.Labels.ResourceManager);
            ResourceManager.Loadedresources.Add("Menus", MD.CMS.Administration.Core.Resources.Menus.ResourceManager);
            ResourceManager.Loadedresources.Add("Titles", MD.CMS.Administration.Core.Resources.Titles.ResourceManager);

            foreach (IAdminAddonLoader loader in _adminAddonLoaders)
            {
                loader.Intitalize();
                if (loader.Resources != null)
                {
                    foreach (KeyValuePair<string, System.Resources.ResourceManager> resource in loader.Resources)
                    {
                        ResourceManager.Loadedresources.Add(resource.Key, resource.Value);
                    }
                }
            }

            RegisterRootAngularModuleAddons.Preload();

            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.AddSingleton<JavaScriptFormatter>();
            services.AddSingleton<JsonSerializerSettings>();

            services.AddMvcCore().AddFormatterMappings();

            services.AddMvc().AddMvcOptions(options => options.EnableEndpointRouting = false);

            services.AddWebMarkupMin(
                options =>
                {
                    options.AllowCompressionInDevelopmentEnvironment = true;
                    options.AllowMinificationInDevelopmentEnvironment = true;
                })
                .AddHtmlMinification(options => {
                    options.MinificationSettings.AttributeQuotesRemovalMode = WebMarkupMin.Core.HtmlAttributeQuotesRemovalMode.KeepQuotes;
                    options.MinificationSettings.CollapseBooleanAttributes = false;
                    options.MinificationSettings.EmptyTagRenderMode = WebMarkupMin.Core.HtmlEmptyTagRenderMode.SpaceAndSlash;
                    options.MinificationSettings.RemoveRedundantAttributes = false;
                    options.MinificationSettings.RemoveEmptyAttributes = false;
                    options.MinificationSettings.RemoveOptionalEndTags = false;
                    options.MinificationSettings.RemoveTagsWithoutContent = false;
                    options.MinificationSettings.MinifyEmbeddedCssCode = false;
                    options.MinificationSettings.MinifyEmbeddedJsCode = false;
                    options.MinificationSettings.MinifyEmbeddedJsonData = false;
                    options.MinificationSettings.MinifyInlineCssCode = false;
                    options.MinificationSettings.MinifyInlineJsCode = false;
                    options.MinificationSettings.PreserveCase = true;
                    options.MinificationSettings.RemoveJsProtocolFromAttributes = false;
                    options.MinificationSettings.RemoveCdataSectionsFromScriptsAndStyles = false;
                    options.MinificationSettings.WhitespaceMinificationMode = WebMarkupMin.Core.WhitespaceMinificationMode.Safe;

                    options.MinificationSettings.ProcessableScriptTypeList = string.Empty;
                    options.MinificationSettings.MinifyKnockoutBindingExpressions = false;
                    options.MinificationSettings.MinifyAngularBindingExpressions = false;
                    options.MinificationSettings.CustomAngularDirectiveList = string.Empty;
                })
                .AddHttpCompression();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            AdminAddonAppBuilder.Default.AdminSystemTitle = Properties.Resources.MDCMSAdministrationTitle;

            _appBuilder = new AdminAddonAppBuilder(app);

            _appBuilder.ApplicationBuilder.UsePathBase(Settings.Default.BaseFolder);

            if (!MD.CMS.BusinessLogic.Core.Properties.Settings.Default.ProductionMode)
            {
                app.UseDeveloperExceptionPage();
            }

            _appBuilder.ApplicationBuilder.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            _appBuilder.ApplicationBuilder.UseMiddleware<RegisterRootAngularModuleAddons>();
            _appBuilder.ApplicationBuilder.UseMiddleware<ResourcesHandler>();
            _appBuilder.ApplicationBuilder.UseMiddleware<InterceptorRedirectMiddleware>();

            _appBuilder.ApplicationBuilder.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(Settings.Default.DefaultUILanguage)
            });

            RewriteOptions rewrite = new RewriteOptions().AddRewrite(@"^((?!(js|img|css|scripts|assets))(?<language>[A-Za-z]{2,4})([_-](?<script>[A-Za-z]{4}|[0-9]{3}))?([_-](?<country>[A-Za-z]{2}|[0-9]{3}))?([_-]x[_-](?<private>[A-Za-z0-9-_]+))?)\/(.*)", "home/index/?lang=$1", false)
                                                         .AddRewrite(@"^((?!(js|img|css|scripts|assets))(?<language>[A-Za-z]{2,4})([_-](?<script>[A-Za-z]{4}|[0-9]{3}))?([_-](?<country>[A-Za-z]{2}|[0-9]{3}))?([_-]x[_-](?<private>[A-Za-z0-9-_]+))?)$", "home/index/?lang=$1", false);
            _appBuilder.ApplicationBuilder.UseRewriter(rewrite);

            foreach (IAdminAddonLoader loader in _adminAddonLoaders)
            {
                loader.Configure(_appBuilder);
            }

            _appBuilder.ApplicationBuilder.Use(async (context, next) =>
            {
                switch (context.Response.StatusCode)
                {
                    case 404:
                        context.Request.Path = "/Error404";
                        break;
                    case 402:
                        context.Request.Path = "/Error402";
                        break;
                    case 500:
                        context.Request.Path = "/Error500";
                        break;
                }
                await next().ConfigureAwait(false);
            });

            _appBuilder.ApplicationBuilder.UseStaticFiles();

            _appBuilder.ApplicationBuilder.UseWebMarkupMin();

            _appBuilder.ApplicationBuilder.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}");
            });

            _appBuilder.ApplicationBuilder.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"{Properties.Settings.Default.ApiBase}swagger/v{AdminAddonAppBuilder.Default.AdminSystemVersion}/swagger.json", $"Omega CMS Web Api v{AdminAddonAppBuilder.Default.AdminSystemVersion}");
                c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
                c.EnableDeepLinking();
                c.ShowCommonExtensions();
            });

            Settings.Default.ContentRootPath = env.ContentRootPath;
            Settings.Default.WebRootPath = env.WebRootPath;
        }
    }
}
