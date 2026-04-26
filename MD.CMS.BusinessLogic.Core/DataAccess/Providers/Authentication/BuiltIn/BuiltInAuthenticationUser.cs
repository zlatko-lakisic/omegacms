using MD.Tools.BaseDataAccess.Core.Entities;
using System.Collections.Generic;
using System.Globalization;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Providers.Authentication.BuiltIn
{
    public class BuiltInAuthenticationUser : BaseEntity<string>, IAuthUser
    {
        #region Attributes
        private string _authDataString;
        private string _username;
        private List<AuthUserField> _metaDataFieldValues;
        private List<MemberOf> _memberOf;
        #endregion

        #region Properties
        public string Username => _username;
        public IEnumerable<AuthUserField> MetaDataFieldValues 
        { 
            get => _metaDataFieldValues; 
            set 
            {
                _metaDataFieldValues = new List<AuthUserField>(value);
            } 
        }
        public string AuthDataString { get => _authDataString; set => _authDataString = value; }
        public string ReferenceId => Id.ToString(CultureInfo.InvariantCulture);

        public IEnumerable<MemberOf> MemberOf { get => _memberOf; set => _memberOf = new List<MemberOf>(value); }
        #endregion

        #region Methods
        public BuiltInAuthenticationUser()
        {
            _metaDataFieldValues = new List<AuthUserField>();
        }
        #endregion
    }
}
