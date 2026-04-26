using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MD.Tools.BaseDataAccess.Plugins.Core.Mapping.Culture
{
    public enum Methods : int
    {
        Insert = 1,
        Update = 2,
        Delete = 3,
        GetByLCID = 4,
        GetByCode = 5,
        GetAll = 6,
        GetAllAvailableForContentId = 7,
        GetApproved = 8,
        SearchCms = 9
    }
}
