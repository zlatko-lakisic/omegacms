namespace MD.CMS.BusinessLogic.Core.DataAccess.Entities
{
    public class ApplicationFull : Application
    {
        #region Attributes
        private string _privateApiKey;
        private string _name;
        #endregion

        #region Properties
        public string PrivateApiKey { get => _privateApiKey; set => _privateApiKey = value; }
        public string Name { get => _name; set => _name = value; }
        #endregion
    }
}
