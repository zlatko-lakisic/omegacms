using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MD.Tools.BaseDataAccess.Plugins.Core
{
    public interface IExtendedMethodProperty : IMethodProperty
    {
        #region Properties
        string PropertyNameValue { get; }
        DbType PropertyType { get; }
        #endregion
    }
}
