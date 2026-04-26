namespace MD.Tools.BaseDataAccess.Plugins.Core.Mapping.Message
{
    public enum Methods : int
    {
        GetByIdAndUserId = 1,
        GetAll = 2,
        GetByMessageFolder = 3,
        GetByMessageFolderAndUser = 4,
        GetByParent = 5,
        GetByUserId = 6,
        Insert = 7,
        Update = 8,
        Delete = 9,
        GetByMessageFolderAndUserCount = 10,
        Search = 11,
        SearchCount = 12,
        GetByMainThread = 13,
        GetByMainThreadAndUser = 14,
        GetUnreadByUser = 15
    }
}
