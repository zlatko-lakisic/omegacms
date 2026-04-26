using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MD.CMS.BusinessLogic.Administration.Core.Addons
{
    public interface IAdminAddonScheduler : IDisposable
    {
        /// <summary>
        /// Initialize async method
        /// </summary>
        void IntitalizeAdminAsync();
        /// <summary>
        /// Start async method
        /// </summary>
        void AdminAsyncStart();
        /// <summary>
        /// Stop async method
        /// </summary>
        void AdminAsyncStop();
    }
}
