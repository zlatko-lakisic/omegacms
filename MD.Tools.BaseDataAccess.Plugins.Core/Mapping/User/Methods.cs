namespace MD.Tools.BaseDataAccess.Plugins.Core.Mapping.User
{
    public enum Methods : int
    {
        GetAll = 1,
        GetById = 2,
        GetByUsernameAndPassword = 3,
        Insert = 4,
        Delete = 5,
        Update = 6,
        GetUsersByProfileTypeCount = 7,
        SelectAllWithPagination = 8,
        SelectAllCount = 9,
        GetIdByUserName = 10,
        UpdateUser = 11,
        UpdateUserByToken = 12,
        GetIdByToken=13,
        Search = 14,
        UpdateToken = 15,
        GetCount = 16,
        GetByReferenceIdAndProvider = 17
    }
}