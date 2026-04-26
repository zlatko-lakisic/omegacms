namespace MD.Tools.BaseDataAccess.Plugins.Core.Mapping.Session
{
    public enum Methods : int
    {
        AddUser = 1,
        GetLoggedOnUser = 2,
        RemoveUserById = 3,
        RemoveUserByAuthData = 4,
        ClearOldSessions = 5,
        ExtendSession = 6
    }
}