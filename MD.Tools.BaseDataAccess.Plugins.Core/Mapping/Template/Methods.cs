namespace MD.Tools.BaseDataAccess.Plugins.Core.Mapping.Template
{
    public enum Methods : int
    {
        GetAll = 1,
        GetById = 2,
        Insert = 3,
        Update = 4,
        Delete = 5,
        ConnectWithFolder = 6,
        DeleteConnectionWithFolder = 7,
        ConnectWithContent = 8,
        DeleteConnectionWithContent = 9,
        GetByFolder = 10,
        GetByContent = 11,
        DeleteByFolder = 12,
        Search = 13,
        GetAllWithPagination = 14,
        GetAllCount = 15
    }
}