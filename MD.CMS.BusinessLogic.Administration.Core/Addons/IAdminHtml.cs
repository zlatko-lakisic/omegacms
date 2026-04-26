using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace MD.CMS.BusinessLogic.Administration.Core.Addons
{
    /// <summary>
    /// Admin html interface used to generate the code and url for each html file in the addon
    /// </summary>
    public interface IAdminHtml : IDisposable
    {
        /// <summary>
        /// Code for the html file
        /// </summary>
        string Code { get; }
        /// <summary>
        /// Url for the html file
        /// </summary>
        string Url { get; }
    }
}
