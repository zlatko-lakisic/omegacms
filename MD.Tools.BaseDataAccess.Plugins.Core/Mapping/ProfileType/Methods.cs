namespace MD.Tools.BaseDataAccess.Plugins.Core.Mapping.ProfileType
{
    public enum Methods : int
    {
        GetById = 1,
        GetAll = 2,
        GetByUser = 3,
        Insert = 4,
        Update = 5,
        Delete = 6,
        GetNotBelongingProfileTypesByUser = 7,
        GetByUserCount = 8,
        Search = 9,
        GetAllWithPagination = 10,
        GetAllCount = 11
    }
}