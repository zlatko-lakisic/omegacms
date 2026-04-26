using MD.Tools.Helpers.Core.TypeAttributes;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Enumerations.Mapping
{
    internal enum MenuContentEnum
    {
        [StringValue("_MenuId")]
        MenuId,
        [StringValue("_ContentId")]
        ContentId,
        [StringValue("_Title")]
        Title,
        [StringValue("_LCID")]
        LCID,
        [StringValue("_DateCreated")]
        DateCreated
    }

    internal enum MenuContentParamatersEnum
    {
        [StringValue("MenuId")]
        MenuId,
        [StringValue("ContentId")]
        ContentId,
        [StringValue("Title")]
        Title,
        [StringValue("LCID")]
        LCID,
        [StringValue("DateCreated")]
        DateCreated,
        [StringValue("folderpath")]
        folderpath

    }

    internal enum MenuContentSPEnum
    {
        [StringValue("MenuContent_Select")]
        Select,
        [StringValue("MenuContent_Delete")]
        Delete,
        [StringValue("MenuContent_Insert")]
        Insert,
        [StringValue("MenuContent_Update")]
        Update,
        [StringValue("MenuContent_SelectAll")]
        SelectAll,
        [StringValue("MenuContent_GetByMenuId")]
        GetByMenuId
    }
}