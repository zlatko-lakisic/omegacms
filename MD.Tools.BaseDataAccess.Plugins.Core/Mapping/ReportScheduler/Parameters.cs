using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MD.Tools.BaseDataAccess.Plugins.Core.Mapping.ReportScheduler
{
    public enum Parameters : int
    {
        ReportSchedulerId = 1,
        Name = 2,
        AuthorId = 3,
        DateCreated = 4,
        DateEdited = 5,
        IsRecurring = 6,
        Interval = 7,
        Start = 8,
         End = 9,
        ReportDefinitionId = 10,
        IsActive = 11,
        IsDeleted = 12,
        Sort = 13,
        SearchTerm = 14,
        SearchColumn = 15
    }
}