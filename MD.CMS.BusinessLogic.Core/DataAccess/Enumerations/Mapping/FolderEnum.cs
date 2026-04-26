using MD.Tools.Helpers.Core.TypeAttributes;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Enumerations.Mapping
{
    public enum FolderEnum
    {
        [StringValue("FolderId")]
        FolderId,
        [StringValue("ParentId")]
        ParentId,
        [StringValue("Name")]
        Name,
        [StringValue("Description")]
        Description,
        [StringValue("FolderPath")]
        FolderPath

    }

    internal enum FolderParamatersEnum
    {
        [StringValue("_FolderId")]
        FolderId,
        [StringValue("_ParentId")]
        ParentId,
        [StringValue("_Name")]
        Name,
        [StringValue("_Description")]
        Description,
        [StringValue("_FolderPath")]
        FolderPath

    }

    internal enum FolderSPEnum
    {
        [StringValue("Folders_Select")]
        Select,
        [StringValue("Folders_Delete")]
        Delete,
        [StringValue("Folders_Insert")]
        Insert,
        [StringValue("Folders_Update")]
        Update,
        [StringValue("Folders_SelectByParentId")]
        SelectByParentId,
        [StringValue("Folders_SelectRoots")]
        SelectRoots,
        [StringValue("Folders_GetByFolderPath")]
        GetByFolderPath
    }
}