using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MD.Tools.BaseDataAccess.Plugins.Core.Mapping.ReportScheduler
{
    public enum Methods : int
    {
        Insert = 1,
        Update = 2,
        Delete = 3,
        GetAll = 4,
        GetById = 5,
        SelectByReportDefinitionId = 6,
        SelectByAuthorId = 7,
        GetSchedulersForProcessing = 8,
        GetAllWithPagination = 9,
        GetAllCount = 10
    }
}