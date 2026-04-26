using MD.Tools.Helpers.Core.TypeAttributes;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Enumerations.Mapping
{
    public enum ContentEnum
    {
        [StringValue("ContentId")]
        ContentId,
        [StringValue("LCID")]
        LCID,
        [StringValue("DateCreated")]
        DateCreated,
        [StringValue("AuthorId")]
        AuthorId,
        [StringValue("FolderId")]
        FolderId,
        [StringValue("Title")]
        Title,
        [StringValue("Html")]
        Html,
        [StringValue("SearchTerm")]
        SearchTerm,
        [StringValue("ContentTypeDefinitionId")]
        ContentTypeDefinitionId,
        [StringValue("ContentCount")]
        ContentCount,
        [StringValue("Alias")]
        Alias,
        [StringValue("TaxonomyId")]
        TaxonomyId,
        [StringValue("IsPublished")]
        IsPublished,
        [StringValue("Folderpath")]
        Folderpath,
        [StringValue("ApprovalPending")]
        ApprovalPending
        
    }

    internal enum ContentParametersEnum
    {
        [StringValue("_ContentId")]
        ContentId,
        [StringValue("_LCID")]
        LCID,
        [StringValue("_DateCreated")]
        DateCreated,
        [StringValue("_AuthorId")]
        AuthorId,
        [StringValue("_FolderId")]
        FolderId,
        [StringValue("_Title")]
        Title,
        [StringValue("_Html")]
        Html,
        [StringValue("_SearchTerm")]
        SearchTerm,
        [StringValue("_ContentTypeDefinitionId")]
        ContentTypeDefinitionId,
        [StringValue("_ContentCount")]
        ContentCount,
        [StringValue("_TaxonomyId")]
        TaxonomyId,
        [StringValue("_ApprovalPending")]
        ApprovalPending
    }

    internal enum ContentSPEnum
    {
        [StringValue("Contents_Select")]
        Select,
        [StringValue("Contents_Delete")]
        Delete,
        [StringValue("Contents_Insert")]
        Insert,
        [StringValue("Contents_Update")]
        Update,
        [StringValue("Contents_SelectByFolderId")]
        SelectByFolderId,
        [StringValue("Contents_SelectBySearchTerm")]
        SelectBySearchTerm,
        [StringValue("Contents_SelectAllCount")]
        SelectAllCount,
        [StringValue("ContentGetBytaxonomy")]
        GetByTaxonomyId
    }
}