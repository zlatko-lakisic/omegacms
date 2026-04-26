using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace MD.CMS.BusinessLogic.WebApi.Core.Addons
{
    /// <summary>
    /// Interface used to initiate loading of all java script files in the addon
    /// </summary>
    public interface IAdminWebApiLoader : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        Type LoaderType { get; }

        /// <summary>
        /// Run configuration tasks
        /// </summary>
        /// <param name="app"></param>
        void Configure(IApplicationBuilder app);

        /// <summary>
        /// Run configuration services tasks
        /// </summary>
        /// <param name="services"></param>
        void ConfigureServices(IServiceCollection services);

        /// <summary>
        /// Parse the configuration for the loader
        /// </summary>
        /// <param name="configuration"></param>
        void ParseConfiguration(IConfiguration configuration);
    }
}
