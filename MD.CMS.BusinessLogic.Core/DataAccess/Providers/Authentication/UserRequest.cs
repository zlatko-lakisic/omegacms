namespace MD.CMS.BusinessLogic.Core.DataAccess.Providers.Authentication
{
    public class UserRequest
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string PageToken { get; set; }
        public string SearchText { get; set; }
    }
}
