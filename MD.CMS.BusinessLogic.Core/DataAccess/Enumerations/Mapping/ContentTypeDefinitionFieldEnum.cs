using MD.Tools.Helpers.Core.TypeAttributes;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Enumerations.Mapping
{
    internal enum ContentTypeDefinitionFieldEnum
    {
        [StringValue("ContentTypeDefinitionFieldId")]
        ContentTypeDefinitionFieldId,
        [StringValue("ContentTypeDefinitionId")]
        ContentTypeDefinitionId,
        [StringValue("AttributeTypeDefinitionId")]
        AttributeTypeDefinitionId,
        [StringValue("Name")]
        Name,
        [StringValue("Description")]
        Description,
        [StringValue("DefaultValue")]
        DefaultValue,
        [StringValue("Order")]
        Order,
        [StringValue("Options")]
        Options,
        [StringValue("ListValue")]
        ListValue,
		[StringValue("Delimiter")]
		Delimiter,
		[StringValue("DataBound")]
		DataBound,
		[StringValue("DataSourceId")]
		DataSourceId,
		[StringValue("DataSourceField")]
		DataSourceField,
		[StringValue("DataBoundReadOnly")]
		DataBoundReadOnly,
        [StringValue("IsDataBoundPrimaryKey")]
        IsDataBoundPrimaryKey
    }

    internal enum ContentTypeDefinitionFieldParametersEnum
    {
        [StringValue("_ContentTypeDefinitionFieldId")]
        ContentTypeDefinitionFieldId,
        [StringValue("_ContentTypeDefinitionId")]
        ContentTypeDefinitionId,
        [StringValue("_AttributeTypeDefinitionId")]
        AttributeTypeDefinitionId,
        [StringValue("_Name")]
        Name,
        [StringValue("_DefaultValue")]
        DefaultValue,
        [StringValue("_Order")]
        Order,
        [StringValue("_Options")]
        Options,
        [StringValue("_ListValue")]
        ListValue,
		[StringValue("_Delimiter")]
		Delimiter,
		[StringValue("_DataBound")]
		DataBound,
		[StringValue("_DataSourceId")]
		DataSourceId,
		[StringValue("_DataSourceField")]
		DataSourceField,
		[StringValue("_DataBoundReadOnly")]
		DataBoundReadOnly,
        [StringValue("_IsDataBoundPrimaryKey")]
        IsDataBoundPrimaryKey
    }

    internal enum ContentTypeDefinitionFieldSPEnum
    {
        [StringValue("ContentTypeDefinitionFields_Select")]
        Select,
        [StringValue("ContentTypeDefinitionFields_Delete")]
        Delete,
        [StringValue("ContentTypeDefinitionFields_Insert")]
        Insert,
        [StringValue("ContentTypeDefinitionFields_Update")]
        Update,
        [StringValue("ContentTypeDefinitionFields_SelectByContentTypeDefinitionId")]
        SelectByContentTypeDefinitionId
    }
}