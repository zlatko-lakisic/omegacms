using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace MD.CMS.BusinessLogic.Administration.Core.Addons
{
    /// <summary>
    /// Admin javascript interface used to generate the code and url for each javascript file in the addon
    /// </summary>
    public interface IAdminJavaScript : IDisposable
    {
        /// <summary>
        /// Code for the javascript file
        /// </summary>
        string Code { get; }
        /// <summary>
        /// Url for the javascript file
        /// </summary>
        string Url { get; }
        /// <summary>
        /// Where this resource will be included, default is Body
        /// </summary>
        AdminResourceLocationEnum Location { get; }
    }
}
