using System.Collections.Generic;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Controllers.V2.Options.Interfaces
{
    public interface IFolderRequestOptions : IPageableRequestOptions, ISearchableRequestOptions, ISortableRequestOptions<Enumerations.Mapping.FolderEnum>
    {
        IEnumerable<long> FolderIds { get; }
        IEnumerable<string> Paths { get; }
        bool FillParent { get; }
        bool FillAllParents { get; }
        bool FillContentTypeDefinitions { get; }
        int Depth { get; }
        bool FillContents { get; }
        bool FillChildren { get; }
        bool FillTemplates { get; }
        FolderRequestOptions ChildFolderRequestOptions { get; }
        FolderRequestOptions ParentFolderRequestOptions { get; }
        DataBoundContentRequestOptions ContentRequestOptions { get; }
        long? ParentId { get; }
        bool OnlyPublished { get; }
        int Lcid { get; }
    }
}
