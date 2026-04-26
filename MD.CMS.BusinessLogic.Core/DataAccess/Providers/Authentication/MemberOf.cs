namespace MD.CMS.BusinessLogic.Core.DataAccess.Providers.Authentication
{
    public class MemberOf
    {
        #region Attributes
        private string _cmsProfileId;
        private string _providerGroupId;
        #endregion

        #region Properties
        public string ProviderGroupId { get => _providerGroupId; set => _providerGroupId = value; }
        public string CmsProfileId { get => _cmsProfileId; set => _cmsProfileId = value; }
        #endregion
    }
}
