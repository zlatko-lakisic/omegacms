using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MD.Tools.BaseDataAccess.Core.Interfaces;
using MD.Tools.Helpers.Core;
using MD.Tools.BaseDataAccess.Core.Entities;
using MD.Tools.Helpers.Core.Extensions.DataRow;
using MD.Tools.BaseDataAccess.Plugins.Core;
using MD.Tools.BaseDataAccess.PluginMethods.Core.DataAccess;

namespace MD.Tools.BaseDataAccess.PluginMethods.Core.Controllers
{
    public abstract class BaseController<T> : BaseDataAccess<T>
        where T : class, new()
    {
        private bool? _isAuthorized;
        protected bool IsAuthorized { get => _isAuthorized ?? false; set => _isAuthorized = value; }
    }
}
