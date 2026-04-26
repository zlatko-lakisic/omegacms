using MD.Tools.Helpers.Core.TypeAttributes;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Enumerations.Mapping
{
    internal enum MetaDataFieldValueEnum
    {
        [StringValue("ContentId")]
        ContentId,
        [StringValue("LCID")]
        LCID,
        [StringValue("DateCreated")]
        DateCreated,
        [StringValue("MetaDataFieldId")]
        MetaDataFieldId,
        [StringValue("Value")]
        Value
    }
    internal enum MetaDataFieldValueParametersEnum
    {
        [StringValue("_ContentId")]
        ContentId,
        [StringValue("_LCID")]
        LCID,
        [StringValue("_DateCreated")]
        DateCreated,
        [StringValue("_MetaDataFieldId")]
        MetaDataFieldId,
        [StringValue("_Value")]
        Value
    }

    internal enum MetaDataFieldValueSPEnum
    {
        [StringValue("MetaDataFieldValues_Select")]
        Select,
        [StringValue("MetaDataFieldValues_Delete")]
        Delete,
        [StringValue("MetaDataFieldValues_Insert")]
        Insert
    }
}
