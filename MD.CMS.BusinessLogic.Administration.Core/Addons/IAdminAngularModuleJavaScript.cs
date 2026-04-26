using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace MD.CMS.BusinessLogic.Administration.Core.Addons
{
    /// <summary>
    /// Admin javascript interface used to register angular modules
    /// </summary>
    public interface IAdminAngularModuleJavaScript : IDisposable
    {
        /// <summary>
        /// Angular module name
        /// </summary>
        string ModuleName { get; }
        /// <summary>
        /// Determine wether the script is a root angular module
        /// </summary>
        bool IsRootAngularModule { get; }
        /// <summary>
        /// Where this resource will be included, default is Body
        /// </summary>
        AdminResourceLocationEnum Location { get; }
    }
}
