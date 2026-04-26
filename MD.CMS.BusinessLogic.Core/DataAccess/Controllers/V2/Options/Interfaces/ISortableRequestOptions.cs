using MD.Tools.Helpers.Core.TypeAttributes;
using System;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Controllers.V2.Options.Interfaces
{
    public interface ISortableRequestOptions<T>
        where T : Enum
    {
        T SortField { get; }
        SortDirection SortDirection { get; }
    }

    public enum SortDirection
    {
        [StringValue("ASC")]
        Ascending,
        [StringValue("DESC")]
        Descending
    }
}
