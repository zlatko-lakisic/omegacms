using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MD.Tools.BaseDataAccess.Plugins.Core.Mapping.RWDPermissions
{
    public enum Methods : int
    {
        GetFolderProfileTypePermissionByFolderAndProfileType = 1,

        GetContentProfileTypePermissionByContentAndProfileType = 2,
        GetMediaContentProfileTypePermissionByMediaContentAndProfileType = 3,
        GetFolderUserPermissionByFolderAndUser = 4,
        GetContentUserPermissionByContentAndUser = 5,
        GetMediaContentUserPermissionByMediaContentAndUser = 6,

        GetFolderProfileTypePermissionByProfileTypeId = 7,

        GetContentProfileTypePermissionByProfileTypeId = 8,
        GetMediaContentProfileTypePermissionByProfileTypeId = 9,
        GetFolderUserPermissionByUserId = 10,
        GetContentUserPermissionByUserId = 11,
        GetMediaContentUserPermissionByUserId = 12,
        FolderProfileTypePermissionInsert = 13,
        ContentProfileTypePermissionInsert = 14,
        MediaContentProfileTypePermissionInsert = 15,
        FolderUserPermissionInsert = 16,
        ContentUserPermissionInsert = 17,
        MediaContentUserPermissionInsert = 18,
        FolderProfileTypePermissionUpdate = 19,

        FolderUserPermissionUpdate = 20,

        FolderProfileTypePermissionDelete = 21,
        ContentProfileTypePermissionDelete = 22,
        MediaContentProfileTypePermissionDelete = 23,

        FolderUserPermissionDelete = 24,
        ContentUserPermissionDelete = 25,
        MediaContentUserPermissionDelete = 26,

        GetAllPermissionsByProfileType = 27,
        GetAllPermissionsByUser = 28,

        GetContentUserPermissionsByContent = 29,
        GetFolderUserPermissionsByFolder = 30,
        GetMediaContentUserPermissionsByMediaContent = 31,
        MediaCntUserPerm_DeletePermissionByMediaCnt = 33,
        FolderUserPermissions_DeleteByFolder = 34,
        MediaCntProfileTypePerms_DeleteByMediaCnt = 35,
        FolderProfileTypePermissionDeleteByFolderId = 36,

        GetUserPermissionByFolderAndUser = 37,
        GetProfileTypePermissionByContentAndProfileType = 38,
        GetUserPermissionByContentAndUser = 39,
        GetUserPermissionByMediaContentAndUser = 40
    }
}
