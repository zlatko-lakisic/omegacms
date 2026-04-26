using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace MD.CMS.BusinessLogic.Administration.Core.Addons
{
    /// <summary>
    /// Interface used to initiate loading of all java script files in the addon
    /// </summary>
    public interface IAdminAddonLoader : IDisposable
    {
        List<IAdminHtml> HtmlToIntercept { get; }
        List<IAdminJavaScript> ScriptsToIntercept { get; }
        List<IAdminCss> CssToIntercept { get; }
        List<IAdminByte> FilesToIntercept { get; }

        Dictionary<string, System.Resources.ResourceManager> Resources { get; }

        /// <summary>
        /// Initialize method
        /// </summary>
        void Intitalize();
        /// <summary>
        /// Run configuration tasks
        /// </summary>
        /// <param name="app"></param>
        void Configure(AdminAddonAppBuilder app);

        /// <summary>
        /// Parse the configuration for the loader
        /// </summary>
        /// <param name="configuration"></param>
        void ParseConfiguration(IConfiguration configuration);
    }
}
