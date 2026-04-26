using MD.Tools.Helpers.Core.TypeAttributes;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Enumerations.Mapping
{
    internal enum ContentTypeDefinitionEnum
    {
        [StringValue("_ContentTypeDefinitionId")]
        ContentTypeDefinitionId,
        [StringValue("_Name")]
        Name,
        [StringValue("_Description")]
        Description,
        [StringValue("_Options")]
        Options,
        [StringValue("_TaxonomyId")]
        TaxonomyId

    }
    internal enum ContentTypeDefinitionParamatersEnum
    {
        [StringValue("ContentTypeDefinitionId")]
        ContentTypeDefinitionId,
        [StringValue("Name")]
        Name,
        [StringValue("Description")]
        Description,
        [StringValue("Options")]
        Options,
        [StringValue("TaxonomyId")]
        TaxonomyId
    }

    internal enum ContentTypeDefinitionSPEnum
    {
        [StringValue("ContentTypeDefinitions_Select")]
        Select,
        [StringValue("ContentTypeDefinitions_Delete")]
        Delete,
        [StringValue("ContentTypeDefinitions_Insert")]
        Insert,
        [StringValue("ContentTypeDefinitions_Update")]
        Update,
        [StringValue("ContentTypeDefinitions_SelectAll")]
        SelectAll,
        [StringValue("ContentTypeDefinitionsByTaxonomyId")]
        ContentByTaxonomyId
    }
}