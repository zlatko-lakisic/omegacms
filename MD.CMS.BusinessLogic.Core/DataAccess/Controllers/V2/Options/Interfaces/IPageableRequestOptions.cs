namespace MD.CMS.BusinessLogic.Core.DataAccess.Controllers.V2.Options.Interfaces
{
    public interface IPageableRequestOptions
    {
        int CurrentPageIndex { get; }
        int MaxNumberOfRows { get; }
    }
}
