using MD.Tools.Helpers.Core.TypeAttributes;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Enumerations.Mapping
{
    internal enum MediaContentEnum
    {

        [StringValue("MediaContentId")]
        MediaContentId,
        [StringValue("LCID")]
        LCID,
        [StringValue("FolderId")]
        FolderId,
        [StringValue("FileType")]
        FileType,
        [StringValue("Size")]
        Size,
        [StringValue("Path")]
        Path,
        [StringValue("Name")]
        Name,
        [StringValue("Description")]
        Description,
        [StringValue("MediaContentCount")]
        MediaContentCount,
        [StringValue("PreviewUrl")]
        PreviewUrl,
        [StringValue("FullNameFile")]
        FullNameFile
     
    }

    internal enum MediaContentParametersEnum
    {
        [StringValue("_MediaContentId")]
        MediaContentId,
        [StringValue("_LCID")]
        LCID,
        [StringValue("_FileType")]
        FileType,
        [StringValue("_FolderId")]
        FolderId,
        [StringValue("_Size")]
        Size,
        [StringValue("_Path")]
        Path,
        [StringValue("_Name")]
        Name,
        [StringValue("_Description")]
        Description,
        [StringValue("_MediaContentCount")]
        MediaContentCount,
        [StringValue("_PreviewUrl")]
        PreviewUrl,
        [StringValue("_FullName")]
        FullName



    }

    internal enum MediaContentSPEnum
    {
        [StringValue("MediaContent_Select")]
        Select,
        [StringValue("MediaContent_SelectAll")]
        SelectAll,
        [StringValue("MediaContent_Delete")]
        Delete,
        [StringValue("MediaContentInsert")]
        Insert,
        [StringValue("MediaContent_Update")]
        Update,
        [StringValue("MediaContent_GetByFolderId")]
        SelectByFolderId,
        [StringValue("MediaContent_SelectAllCount")]
        SelectAllCount,
         [StringValue("mediacontent_Updatepath")]
        Updatepath,
          [StringValue("getBaseInformationMediaContent")]
         getBaseInformationMediaContent,
          [StringValue("MediaContent_updatePreviewurl")]
          UpdatePreviewUrl,
          [StringValue("MediaContentUpdateFullName")]
          UpdateFullName
    }


}
