using System;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Entities
{
    public class AccessToken
    {
        #region Attributes
        private string _token;
        private DateTime _timeout;
        #endregion

        #region Properties
        public DateTime Timeout { get => _timeout; set => _timeout = value; }
        public string Token { get => _token; set => _token = value; }
        #endregion
    }
}
