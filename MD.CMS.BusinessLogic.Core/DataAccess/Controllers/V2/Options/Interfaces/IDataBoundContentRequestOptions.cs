using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using System.Collections.Generic;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Controllers.V2.Options.Interfaces
{
    public interface IDataBoundContentRequestOptions : IContentRequestOptions
    {
        IEnumerable<ContentTypeDefinitionFolderDataBoundCondition> DataBoundConditions { get; }
    }
}
