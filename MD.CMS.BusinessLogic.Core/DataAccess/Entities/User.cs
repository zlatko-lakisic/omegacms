using MD.CMS.BusinessLogic.Core.Properties;
using MD.Tools.BaseDataAccess.Core.Entities;
using MD.Tools.BaseDataAccess.Plugins.Core.Mapping.ReportDefinition.ReportDesigner.CustomAttributes;
using System;
using System.Globalization;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Entities
{
    public class User : BaseEntity<string>
    {
        #region Attributes
        private string _username;
        private ProfileTypeList _profileTypes;
        private long _profileTypeId;
        private string oldPassword;
        private string _token;
        private DateTime _dateRefreshToken;
        private bool _administrationAllowed;
        private string _referenceId;
        private string _authenticationProvider;
        private static User _systemUser;
        private string _password;
        #endregion

        #region Properties

        public string Username
        {
            get { return _username; }
            set { _username = value; }
        }

        [OmitPropertyFromReport]
        public string Token
        {
            get { return _token; }
            set { _token = value; }
        }

        [OmitPropertyFromReport]
        public string DateRefreshToken
        {
            get
            {
                return _dateRefreshToken.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _dateRefreshToken = DateTime.Parse(value, CultureInfo.InvariantCulture);
                }

            }
        }

        [OmitPropertyFromReport]
        public bool IsNew
        {
            get
            {
                return string.IsNullOrEmpty(Id) || string.CompareOrdinal(Id, "0").Equals(0);
            }
        }

        [OmitPropertyFromReport]
        public ProfileTypeList ProfileTypes
        {
            get
            {
                if (_profileTypes == null)
                {
                    _profileTypes = new ProfileTypeList();
                }
                return _profileTypes;
            }
            set { _profileTypes = value; }
        }

        [OmitPropertyFromReport]
        public long ProfileTypeId
        {
            get { return _profileTypeId; }
            set { _profileTypeId = value; }
        }

        public bool AdministrationAllowed { get => _administrationAllowed; set => _administrationAllowed = value; }

        public bool IsRoot => !IsNew && Id.Equals(Settings.Default.RootId());

        public string ReferenceId { get => _referenceId; set => _referenceId = value; }
        public string AuthenticationProvider { get => _authenticationProvider; set => _authenticationProvider = value; }
        public string Password { get => _password; set => _password = value; }

        #endregion

        #region Methods
        public User() { }
        public User(User obj)
            : base (obj)
        {
            this._username = obj.Username;
            this._profileTypeId = obj.ProfileTypeId;
            this._profileTypes = obj.ProfileTypes;
            this._administrationAllowed = obj.AdministrationAllowed;
            this._token = obj.Token;
            this._authenticationProvider = obj.AuthenticationProvider;
            this._referenceId = obj.ReferenceId;
        }

        public static User SystemUser()
        {
            if (_systemUser == null)
            {
                Random random = new Random();
                _systemUser = new User()
                {
                    Id = random.Next(int.MinValue+10000, int.MaxValue).ToString(CultureInfo.InvariantCulture)
                };
                _systemUser.ProfileTypes.Add(new ProfileType()
                {
                    Id = random.Next(int.MinValue+10000, int.MaxValue)
                });
            }
            return _systemUser;
        }
        #endregion
    }
}
