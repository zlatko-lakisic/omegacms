using MD.Tools.Helpers.Core.TypeAttributes;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Enumerations.Mapping
{
    internal enum ContentTypeDefinitionFieldValueEnum
    {
        [StringValue("ContentTypeDefinitionId")]
        ContentTypeDefinitionId,
        [StringValue("ContentId")]
        ContentId,
        [StringValue("LCID")]
        LCID,
        [StringValue("DateCreated")]
        DateCreated,
        [StringValue("ContentTypeDefinitionFieldId")]
        ContentTypeDefinitionFieldId,
        [StringValue("Value")]
        Value,
        [StringValue("Name")]
        Name,
        [StringValue("AttributeTypeDefinitionId")]
        AttributeTypeDefinitionId
    }

    internal enum ContentTypeDefinitionFieldValueParametersEnum
    {
        [StringValue("_ContentTypeDefinitionId")]
        ContentTypeDefinitionId,
        [StringValue("_ContentId")]
        ContentId,
        [StringValue("_LCID")]
        LCID,
        [StringValue("_DateCreated")]
        DateCreated,
        [StringValue("_ContentTypeDefinitionFieldId")]
        ContentTypeDefinitionFieldId,
        [StringValue("_Value")]
        Value

    }

    internal enum ContentTypeDefinitionFieldValueSPEnum
    {
        [StringValue("ContentTypeDefinitionFieldValues_Select")]
        Select,
        [StringValue("ContentTypeDefinitionFieldValues_Delete")]
        Delete,
        [StringValue("ContentTypeDefinitionFieldValues_Insert")]
        Insert,
        [StringValue("ContentTypeDefinitionFieldValues_Update")]
        Update,
        [StringValue("ContentTypeDefinitionFieldValues_SelectByContent")]
        SelectByContent
    }
}