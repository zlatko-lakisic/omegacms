namespace MD.Tools.BaseDataAccess.Plugins.Core.Mapping.Menu
{
    public enum Methods : int
    {
        GetById = 1,
        GetByMenuPath = 2,
        GetByParentId = 3,
        GetByContent = 4,
        GetAll = 5,
        Insert = 6,
        Delete = 7,
        Update = 8,
        MenuSearchByName = 9,
        GetByContentId = 10,
        DeleteByParentId = 11,
        GetByParentIdWithPagination = 12,
        GetByParentIdCount = 13,
        GetMenuByPath = 14,
        MenusSearch = 15
    }
}