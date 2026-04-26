namespace MD.Tools.BaseDataAccess.Plugins.Core.Mapping.Folder
{
    public enum Methods : int
    {        GetById=1,
        GetFolderByPath=2,
        GetRoots=3,
        GetByParentId=4,
        Insert=5,
        Delete=6,
        Update=7,
        DeleteByParentId = 8,
        SelectByParentIdWithPagination = 9,
        SelectByParentIdCount = 10,
        Search = 11
    }
}
