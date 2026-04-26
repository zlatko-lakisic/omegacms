using MD.Tools.Helpers.Core.TypeAttributes;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Enumerations.Mapping
{
    internal enum MetaDataFieldEnum
    {
        [StringValue("MetaDataFieldId")]
        MetaDataFieldId,
        [StringValue("AttributeTypeDefinitionId")]
        AttributeTypeDefinitionId,
        [StringValue("Name")]
        Name,
        [StringValue("DefaultValue")]
        DefaultValue,
        [StringValue("ListValue")]
        ListValue,
        [StringValue("Delimiter")]
        Delimiter,
        [StringValue("IsRequired")]
        IsRequired,       
    }
    internal enum MetaDataFieldParametersEnum
    {
        [StringValue("_MetaDataFieldId")]
        MetaDataFieldId,
        [StringValue("_AttributeTypeDefinitionId")]
        AttributeTypeDefinitionId,
        [StringValue("_Name")]
        Name,
        [StringValue("_DefaultValue")]
        DefaultValue,
        [StringValue("_ListValue")]
        ListValue,
        [StringValue("_Delimiter")]
        Delimiter
    }
    
    internal enum MetaDataFieldSPEnum
    {
        [StringValue("MetaDataFields_Select")]
        Select,
        [StringValue("MetaDataFields_Delete")]
        Delete,
        [StringValue("MetaDataFields_Insert")]
        Insert,
        [StringValue("MetaDataFields_Update")]
        Update,
        [StringValue("MetaDataFields_SelectAll")]
        SelectAll
    }
}
