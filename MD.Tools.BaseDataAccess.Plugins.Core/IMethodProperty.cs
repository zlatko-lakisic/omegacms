using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MD.Tools.BaseDataAccess.Plugins.Core
{
    public interface IMethodProperty
    {
        #region Properties
        int Id { get; set; }
        object Value { get; set; }
        bool IsArray { get; set; }
        #endregion
    }
}
