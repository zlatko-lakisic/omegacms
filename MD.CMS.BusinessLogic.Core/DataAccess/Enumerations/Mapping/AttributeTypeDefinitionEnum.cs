using MD.Tools.Helpers.Core.TypeAttributes;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Enumerations.Mapping
{
    internal enum AttributeTypeDefinitionEnum
    {
        [StringValue("AttributeTypeDefinitionId")]
        AttributeTypeDefinitionId,
        [StringValue("Name")]
        Name,
        [StringValue("DefaultValue")]
        DefaultValue,
        [StringValue("Type")]
        Type,
        [StringValue("InputType")]
        InputType

    }

    internal enum AttributeTypeDefinitionParametersEnum
    {
        [StringValue("_AttributeTypeDefinitionId")]
        AttributeTypeDefinitionId,
        [StringValue("_Name")]
        Name,
        [StringValue("_DefaultValue")]
        DefaultValue,
        [StringValue("_Type")]
        Type,
        [StringValue("_InputType")]
        InputType

    }

    internal enum AttributeTypeDefinitionSPEnum
    {
        [StringValue("AttributeTypeDefinitions_Select")]
        Select,
        [StringValue("AttributeTypeDefinitions_Delete")]
        Delete,
        [StringValue("AttributeTypeDefinitions_Insert")]
        Insert,
        [StringValue("AttributeTypeDefinitions_Update")]
        Update
    }   
}