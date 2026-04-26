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
    public interface IAdminByte : IDisposable
    {
        /// <summary>
        /// Code for the file content file
        /// </summary>
        byte[] FileContent { get; }

        /// <summary>
        /// file extension
        /// </summary>
        string Extension { get; }

        /// <summary>
        /// Url for the javascript file
        /// </summary>
        string Url { get; }
    }
}
