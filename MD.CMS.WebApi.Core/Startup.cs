using MD.Tools.Helpers.Core.Config;
using MD.Tools.Helpers.Core.Plugins;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using MD.CMS.WebApi.Core.Properties;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.HttpOverrides;
using MD.CMS.WebApi.Core.BusinessLogic;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MD.Tools.Helpers.Core.Logging;
using MD.CMS.BusinessLogic.WebApi.Core.Addons;
using MD.CMS.BusinessLogic.WebApi.Core.Modeles;
using MD.Tools.Helpers.Core.Caching.Providers;
using MD.Tools.Helpers.Core.Caching;
using Microsoft.OpenApi.Models;
using MD.CMS.WebApi.Core.Filters.Swagger;
using MD.CMS.WebApi.Core.Middleware;
using System;
using Microsoft.AspNetCore.Mvc.Controllers;
using System.Reflection;
using System.IO;
using MD.CMS.WebApi.Core.BusinessLogic.Extensions;
using Microsoft.AspNetCore.Mvc;
using MD.CMS.BusinessLogic.Core.DataAccess.Providers.Authentication;
using MD.CMS.BusinessLogic.Core.Helpers.Extensions;
using System.Threading.Tasks;

namespace MD.CMS.WebApi.Core
{
    public class Startup
    {
        public delegate Task<bool> LicenseValidDelegate(HttpContext context);

        private IEnumerable<IAdminWebApiLoader> _adminWebApiLoaders;
        protected static string _basePath = ReflectionHelper.GetDefaultPluginPath;
        protected static string _swaggerXmlFilePath = string.Empty;
        internal static LicenseValidDelegate LicenseValid = new LicenseValidDelegate(_licenseValidatorFunctionAsync);
        protected static LicenseValidDelegate LicenseValidSetter
        {
            set
            {
                LicenseValid = value;
            }
        }

        internal static async Task<bool> _licenseValidatorFunctionAsync(HttpContext context)
        {
            return (
                    context.Connection.RemoteIpAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork &&
                    context.Connection.RemoteIpAddress.ToString() == "127.0.0.1"
                )  || (
                    context.Connection.RemoteIpAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6 &&
                    (context.Connection.RemoteIpAddress.IsIPv6LinkLocal || context.Connection.RemoteIpAddress.IsIPv6SiteLocal)
                )  || Tools.Licensing.LicenseValidate.ValidateLicense(Tools.Licensing.License.ReadLicenseFile(Directory.GetCurrentDirectory()),
                    Tools.Licensing.ServerKey.ReadServerKeyFile(Directory.GetCurrentDirectory()),
                    Tools.Licensing.ComponentEnum.WebApi,
                    WebApiAddonAppBuilder.Default.WebApiSystemVersion,
                    Tools.Licensing.ClientKey.ReadClientKeyFile(Directory.GetCurrentDirectory()),
                    await MD.CMS.BusinessLogic.Core.DataAccess.Controllers.UserController.GetNewInstance().DefaultPlugin(true).Caller(MD.CMS.BusinessLogic.Core.DataAccess.Entities.User.SystemUser()).GetCountAsync(),
                    context.Request.Host.Value);
        }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public Startup(IConfiguration configuration, string path) : this(configuration)
        {
            _basePath = path;
        }

        public IConfiguration Configuration { get; }

        public void PreloadConfigurations()
        {
            (typeof(Startup)).LogVerbose($"Path is: {_basePath}");
            (typeof(Startup)).LogVerbose("Loading built in configurations...");
            DateTime statrt = DateTime.Now;
            int pluginCount = 0;
            try
            {
                foreach (IConfigParsable obj in PluginLoader<IConfigParsable>.GetAll((int)MD.Tools.Helpers.Core.FileProvider.FileProviderEnum.Hosted, _basePath))
                {
                    obj.GetStaticInstance().Parse(Configuration.GetSection("Config"));
                    pluginCount++;
                }
            } 
            catch (Exception e)
            {
                (typeof(Startup)).Log(e);
            }
            (typeof(Startup)).LogVerbose($"{pluginCount} built in configurations found and loaded. Took {DateTime.Now.Subtract(statrt).TotalMilliseconds} ms.");

            (typeof(Startup)).LogVerbose($"Loading other configurations from {Settings.Default.PluginsFileProviderType}...");
            statrt = DateTime.Now;
            pluginCount = 0;
            try
            {
                foreach (IPluginConfigParsable obj in PluginLoader<IPluginConfigParsable>.GetAll(Settings.Default.PluginsFileProviderType, Settings.Default.PluginsDirectory))
                {
                    obj.GetStaticInstance().Parse(Configuration.GetSection("Config"));
                    pluginCount++;
                }
            }
            catch (Exception e)
            {
                (typeof(Startup)).Log(e);
            }
            (typeof(Startup)).LogVerbose($"{pluginCount} other configurations found and loaded. Took {DateTime.Now.Subtract(statrt).TotalMilliseconds} ms.");
        }

        public void PreloadPlugins()
        {
            (typeof(Startup)).LogVerbose($"Loading IAdminWebApiLoader(s) from {Settings.Default.PluginsFileProviderType}...");
            DateTime statrt = DateTime.Now;
            int pluginCount = 0;
            try
            {
                _adminWebApiLoaders = PluginLoader<IAdminWebApiLoader>.GetAll(Settings.Default.PluginsFileProviderType, Settings.Default.PluginsDirectory);
                (typeof(Startup)).LogVerbose($"{pluginCount} other IAdminWebApiLoader(s) found and loaded. Took {DateTime.Now.Subtract(statrt).TotalMilliseconds} ms.");
            }
            catch (Exception e)
            {
                (typeof(Startup)).Log(e);
            }

            try
            {
                foreach (IAdminWebApiLoader loader in _adminWebApiLoaders)
                {
                    loader.ParseConfiguration(Configuration);
                }
            }
            catch (Exception e)
            {
                (typeof(Startup)).Log(e);
            }

            try
            {
                foreach (IOmegaServerCachingProvider cacheProvider in PluginLoader<IOmegaServerCachingProvider>.GetAll(Settings.Default.PluginsFileProviderType, Settings.Default.PluginsDirectory))
                {
                    OmegaCacheController.Instance.AddCachingProvider(cacheProvider);
                    if (OmegaCacheController.Instance.CachingProviders[cacheProvider.ProviderName].Config != null && OmegaCacheController.Instance.CachingProviders[cacheProvider.ProviderName].Config.GetStaticInstance() != null)
                    {
                        OmegaCacheController.Instance.CachingProviders[cacheProvider.ProviderName].Config.GetStaticInstance().Parse(Configuration.GetSection("Config"));
                    }
                }
            }
            catch (Exception e)
            {
                (typeof(Startup)).Log(e);
            }

            try
            {
                AuthenticationProviderLoader.Load();
                AuthenticationProviderLoader.Load(Settings.Default.PluginsFileProviderType, Settings.Default.PluginsDirectory);
            }
            catch (Exception e)
            {
                (typeof(Startup)).Log(e);
            }
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public virtual void  ConfigureServices(IServiceCollection services)
        {
            PreloadConfigurations();
            PreloadPlugins();

            services.AddConnections();
            services.AddControllers(options =>
            {
                options.AllowEmptyInputInBodyModelBinding = true;
            });

            (typeof(Startup)).LogVerbose($"Loading Additional Controllers...");
            try
            {
                foreach (IAdminWebApiLoader obj in _adminWebApiLoaders)
                {
                    try
                    {
                        (typeof(Startup)).LogVerbose($"Loading Controller From: {obj.LoaderType.Assembly.GetName()}");
                        services.AddControllers(options =>
                        {
                            options.AllowEmptyInputInBodyModelBinding = true;
                        }).AddApplicationPart(obj.LoaderType.Assembly);
                    }
                    catch (Exception e)
                    {
                        (typeof(Startup)).Log(e);
                    }
                }
            }
            catch (Exception e)
            {
                (typeof(Startup)).Log(e);
            }

            (typeof(Startup)).LogVerbose($"Running configure services in plugins");
            try
            {
                foreach (IAdminWebApiLoader obj in _adminWebApiLoaders)
                {
                    try
                    {
                        obj.ConfigureServices(services);
                    }
                    catch (Exception e)
                    {
                        (typeof(Startup)).Log(e);
                    }
                }
            }
            catch (Exception e)
            {
                (typeof(Startup)).Log(e);
            }

            (typeof(Startup)).LogVerbose($"Generating Swagger Doc");
            services.AddSwaggerGen(c =>
            {
                try
                {
                    c.SwaggerDoc($"v{WebApiAddonAppBuilder.Default.WebApiSystemVersion}", new OpenApiInfo { Title = "Omega CMS Web Api", Version = $"v{WebApiAddonAppBuilder.Default.WebApiSystemVersion}" });
                    c.OperationFilter<SwaggerAuthorizationFilter>();
                    c.OperationFilter<SwaggerOperationIdFilter>();
                    c.CustomSchemaIds(type => type.ToString()
                            .Replace("[", "_")
                            .Replace("]", "_")
                            .Replace(",", "-")
                            .Replace("`", "_")
                            .Replace("+", "_")
                        );
                    c.TagActionsBy(api =>
                    {
                        if (api.GroupName != null)
                        {
                            return new[] { api.GroupName };
                        }

                        var controllerActionDescriptor = api.ActionDescriptor as ControllerActionDescriptor;
                        if (controllerActionDescriptor != null)
                        {
                            return new[] { controllerActionDescriptor.ControllerName };
                        }

                        throw new InvalidOperationException("Unable to determine tag for endpoint.");
                    });
                    c.DocInclusionPredicate((name, api) => true);

                    if (!string.IsNullOrEmpty(_swaggerXmlFilePath))
                    {
                        string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                        string xmlPath = Path.Combine(_swaggerXmlFilePath, xmlFile);
                        c.IncludeXmlComments(xmlPath);
                    }
                }
                catch (InvalidOperationException e)
                {
                    typeof(Startup).Log(e);
                }
                catch (Exception e)
                {
                    typeof(Startup).Log(e);
                }
            });

            if (Settings.Default.EnableCors)
            {
                services.AddCors(options =>
                {
                    options.AddPolicy("OmegaCorsPolicy",
                    builder =>
                    {
                        builder.WithOrigins(Settings.Default.CorsOrigins.Cast<string>().ToArray())
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                    });
                });
            }

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.All;
            });

            services.AddSingleton<JsonSerializerSettings>();

            services.AddMvc(options => {
                /*if (!string.IsNullOrEmpty(Settings.Default.BaseApiPath))
                {
                    options.UseCentralRoutePrefix(new RouteAttribute($"{Settings.Default.BaseApiPath}/"));
                }*/
            }).AddNewtonsoftJson(opt =>
            {
                opt.SerializerSettings.ContractResolver = new DefaultContractResolver();
                opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                opt.SerializerSettings.Error = (object sender, Newtonsoft.Json.Serialization.ErrorEventArgs args) =>
                {
                    Logger.LogWarning("Error while serializing/deserializing the object!\\nobject = {0}\\nargs= {1}", JsonConvert.SerializeObject(sender), JsonConvert.SerializeObject(args));
                };

            }).AddMvcOptions(options => options.EnableEndpointRouting = false);

            services.AddDistributedMemoryCache();
            //services.TryAddEnumerable(ServiceDescriptor.Singleton<IPageApplicationModelProvider, MdCacheFilterApplicationModelProvider>());

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory logger)
        {
            ConfigureWithActions(app, env, logger, null);
        }
        public virtual void ConfigureWithActions(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory logger, Action<IApplicationBuilder, IWebHostEnvironment, ILoggerFactory> preMiddleWareAction)
        {
            ConfigureWithActions(app, env, logger, preMiddleWareAction, null);
        }

        public virtual void ConfigureWithActions(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory logger,
                                                 Action<IApplicationBuilder, IWebHostEnvironment, ILoggerFactory> preMiddleWareAction = null,
                                                 Action<IApplicationBuilder, IWebHostEnvironment, ILoggerFactory> postMiddleWareAction = null)
        {
            if (Settings.Default.EnableCors)
            {
                app.UseCors("OmegaCorsPolicy");
            }

            if (!MD.CMS.BusinessLogic.Core.Properties.Settings.Default.ProductionMode)
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    IExceptionHandlerFeature contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        if (Logger.IsAvailable)
                        {
                            typeof(Startup).Log(contextFeature.Error);
                        }
                        context.Response.ContentType = "application/json";
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        await context.Response.WriteAsync(new ErrorDetails(contextFeature.Error, context.Response.StatusCode).ToString()).ConfigureAwait(false);
                    }
                });
            });

            if (!string.IsNullOrEmpty(Settings.Default.BaseApiPath))
            {
                app.UsePathBase(Settings.Default.BaseApiPath);
            }

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.All
            });

            app.UseMiddleware<SessionDomainMiddleware>();

            app.UseMiddleware<LcidMiddleware>();

            app.UseRouting();

            if (preMiddleWareAction != null)
            {
                preMiddleWareAction(app, env, logger);
            }
            app.UseMiddleware<SwaggerRestrictAccessMiddleware>();
            if (postMiddleWareAction != null)
            {
                postMiddleWareAction(app, env, logger);
            }

            if (_adminWebApiLoaders != null)
            {
                foreach (IAdminWebApiLoader obj in _adminWebApiLoaders)
                {
                    obj.Configure(app);
                }
            }

            app.UseMiddleware(typeof(ErrorHandlingMiddleware));

            app.UseSwagger(c =>
            {
                c.SerializeAsV2 = true;
            });

            app.UseMvc();

            Settings.Default.ContentRootPath = env.ContentRootPath;
            Settings.Default.ContentRootPath = env.WebRootPath;
        }
    }
}
