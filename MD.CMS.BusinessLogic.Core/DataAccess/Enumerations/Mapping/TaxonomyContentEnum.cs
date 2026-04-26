using MD.Tools.Helpers.Core.TypeAttributes;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Enumerations.Mapping
{
    internal enum TaxonomyContentEnum
    {
        [StringValue("_TaxonomyId")]
        TaxonomyId,
        [StringValue("_ContentId")]
        ContentId,
        [StringValue("_Title")]
        Title,
        [StringValue("_LCID")]
        LCID,
        [StringValue("_DateCreated")]
        DateCreated
    }

    internal enum TaxonomyContentParamatersEnum
    {
        [StringValue("TaxonomyId")]
        TaxonomyId,
        [StringValue("ContentId")]
        ContentId,
        [StringValue("Title")]
        Title,
        [StringValue("LCID")]
        LCID,
        [StringValue("DateCreated")]
        DateCreated,
        [StringValue("folderpath")]
        folderpath,
        [StringValue("type")]
        type

    }

    internal enum TaxonomyContentSPEnum
    {
        [StringValue("TaxonomyContent_Select")]
        Select,
        [StringValue("TaxonomyContent_Delete")]
        Delete,
        [StringValue("TaxonomyContent_Insert")]
        Insert,
        [StringValue("TaxonomyContent_Update")]
        Update,
        [StringValue("TaxonomyContent_SelectAll")]
        SelectAll,
        [StringValue("TaxonomyContent_GetByTaxonomyId")]
        GetByTaxonomyId
    }
}