using MD.Tools.Helpers.Core.TypeAttributes;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Enumerations.Mapping
{
    internal enum TaxonomyEnum
    {
        [StringValue("_TaxonomyId")]
        TaxonomyId,
        [StringValue("_ParentId")]
        ParentId,
        [StringValue("_TaxonomyName")]
        Name,
        [StringValue("_Description")]
        Description,
        [StringValue("_ContentId")]
        ContentId,
        [StringValue("_LCID")]
        LCID,
        [StringValue("_DateCreated")]
        DateCreated,
        [StringValue("_FolderId")]
        FolderId,
        [StringValue("_taxonomyPath")]
        TaxonomyPath
    }

    internal enum TaxonomyParamatersEnum
    {
        [StringValue("TaxonomyId")]
        TaxonomyId,
        [StringValue("ParentId")]
        ParentId,
        [StringValue("TaxonomyName")]
        Name,
        [StringValue("Description")]
        Description,
        [StringValue("ContentId")]
        ContentId,
        [StringValue("LCID")]
        LCID,
        [StringValue("DateCreated")]
        DateCreated,
        [StringValue("FolderId")]
        FolderId,
        [StringValue("TaxonomyPath")]
        TaxonomyPath,
        [StringValue("Order")]
        Order

    }

    internal enum TaxonomySPEnum
    {
        [StringValue("Taxonomies_Select")]
        Select,
        [StringValue("Taxonomies_Delete")]
        Delete,
        [StringValue("Taxonomies_Insert")]
        Insert,
        [StringValue("Taxonomies_Update")]
        Update,
        [StringValue("Taxonomies_SelectByParentId")]
        SelectByParentId,
        [StringValue("TaxonomyContent_Insert")]
        TaxonomyContent_Insert,
        [StringValue("Taxonomies_SelectAll")]
        SelectAll,
        [StringValue("Taxonomies_SelectByContent")]
        SelectByContent,
        [StringValue("Taxonomies_GetByTaxonomyPath")]
        GetByTaxonomyPath
    }
}