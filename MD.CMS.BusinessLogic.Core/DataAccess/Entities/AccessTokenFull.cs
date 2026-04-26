using System;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Entities
{
    public class AccessTokenFull : AccessToken
    {
        #region Enum
        public enum AccessTokenType
        {
            User,
            Application
        }
        #endregion

        #region Attributes
        private AccessTokenType _type;
        private User _user;
        private Application _application;
        #endregion

        #region Properties
        public AccessTokenType Type { get => _type; set => _type = value; }
        public User User { get => _user; set => _user = value; }
        public Application Application { get => _application; set => _application = value; }
        #endregion

        #region Methods
        public AccessTokenFull(AccessToken token)
        {

        }

        public AccessTokenFull GetData(string privateToken)
        {


            return this;
        }

        public AccessToken GetToken()
        {


            return this;
        }
        #endregion
    }
}
