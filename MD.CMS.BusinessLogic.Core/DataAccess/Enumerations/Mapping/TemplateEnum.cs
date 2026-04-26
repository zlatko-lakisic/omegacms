using MD.Tools.Helpers.Core.TypeAttributes;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Enumerations.Mapping
{
    internal enum TemplateEnum
    {
        [StringValue("TemplateId")]
        TemplateId,
        [StringValue("Name")]
        Name,
        [StringValue("Description")]
        Description,
        [StringValue("TemplateUrl")]
        TemplateUrl
    }

    internal enum TemplateParametersEnum
    {
        [StringValue("_TemplateId")]
        TemplateId,
        [StringValue("_Name")]
        Name,
        [StringValue("_Description")]
        Description,
        [StringValue("_TemplateUrl")]
        TemplateUrl
    }

    internal enum TemplateSPEnum
    {
        [StringValue("Templates_Select")]
        Select,
        [StringValue("Templates_SelectAll")]
        SelectAll,
        [StringValue("Templates_Delete")]
        Delete,
        [StringValue("Templates_Insert")]
        Insert,
        [StringValue("Templates_Update")]
        Update,
        [StringValue("FolderTemplate_Insert")]
        ConnectWithFolder,
        [StringValue("FolderTemplate_Delete")]
        DeleteConnectionWithFolder,
        [StringValue("ContentTemplate_Insert")]
        ConnectWithContent,
        [StringValue("ContentTemplate_Delete")]
        DeleteConnectionWithContent
    }
}