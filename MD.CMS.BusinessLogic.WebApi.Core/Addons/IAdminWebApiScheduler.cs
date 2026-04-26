using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MD.CMS.BusinessLogic.WebApi.Core.Addons
{
    public interface IAdminWebApiScheduler : IDisposable
    {
        /// <summary>
        /// Initialize async method
        /// </summary>
        void IntitalizeWebApiAsync();
        /// <summary>
        /// Start async method
        /// </summary>
        void WebApiAsyncStart();
        /// <summary>
        /// Stop async method
        /// </summary>
        void WebApiAsyncStop();
    }
}
