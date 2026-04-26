using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MD.Tools.BaseDataAccess.Plugins.Core.Mapping.Step
{
    public enum Methods : int
    {
        Insert = 1,
        Update = 2,
        Delete = 3,
        GetAll = 4,
        GetById = 5,
        SelectByChainId = 6

    }
}