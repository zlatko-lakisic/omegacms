using System.Collections.Generic;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Controllers.V2.Options.Interfaces
{
    public interface IContentRequestOptions : IPageableRequestOptions, ISearchableRequestOptions, ISortableRequestOptions<Enumerations.Mapping.ContentEnum>
    {
        IEnumerable<string> ContentIds { get; }
        bool OnlyPublished { get; set; }
        bool LoadAuthor { get; }
        bool FillFields { get; }
        bool FillMetaData { get; }
        int Lcid { get; }
        long FolderId { get; }
        long TaxonomyId { get; }
        long MenuId { get; }
        string Alias { get; }
        bool DataBound { get; }
        long ContentTypeId { get; }
    }
}
