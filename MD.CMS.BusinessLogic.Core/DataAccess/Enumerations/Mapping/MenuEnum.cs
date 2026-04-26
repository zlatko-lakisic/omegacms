using MD.Tools.Helpers.Core.TypeAttributes;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Enumerations.Mapping
{
    internal enum MenuEnum
    {
        [StringValue("_MenuId")]
        MenuId,
        [StringValue("_ParentId")]
        ParentId,
        [StringValue("_MenuName")]
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
        [StringValue("_menuPath")]
        MenuPath,
        [StringValue("_options")]
        Options
    }

    internal enum MenuParamatersEnum
    {
        [StringValue("MenuId")]
        MenuId,
        [StringValue("ParentId")]
        ParentId,
        [StringValue("MenuName")]
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
        [StringValue("MenuPath")]
        MenuPath,
        [StringValue("Options")]
        Options
    }

    internal enum MenuSPEnum
    {
        [StringValue("Menus_Select")]
        Select,
        [StringValue("Menus_Delete")]
        Delete,
        [StringValue("Menus_Insert")]
        Insert,
        [StringValue("Menus_Update")]
        Update,
        [StringValue("Menus_SelectByParentId")]
        SelectByParentId,
        [StringValue("MenuContent_Insert")]
        MenuContent_Insert,
        [StringValue("Menus_SelectAll")]
        SelectAll,
        [StringValue("Menus_SelectByContent")]
        SelectByContent,
        [StringValue("Menus_GetByMenuPath")]
        GetByMenuPath
    }
}