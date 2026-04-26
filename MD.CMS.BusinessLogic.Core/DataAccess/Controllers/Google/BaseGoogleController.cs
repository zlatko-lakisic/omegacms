using MD.Tools.BaseDataAccess.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Google
{
    public abstract class BaseGoogleController<T> : MD.Tools.BaseDataAccess.PluginMethods.Core.Controllers.BaseController<T>, IBaseControllerSettings
        where T : class, IBaseControllerSettings, new()
    {
        public string ConnectionString
        {
            get { throw new NotImplementedException(); }
        }
    }
}
