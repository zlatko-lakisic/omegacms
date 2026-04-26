using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Entities
{
    public class Session
    {
        #region Attributes
        private string _userId;
        private string _username;
        private string _authdata;
        private string _sessionId;
        private DateTime _dateAdded;
        private string _sessionDomain;
        #endregion

        #region Properties
        public string UserId
        {
            get { return _userId; }
            set { _userId = value; }
        }
        public string Username
        {
            get { return _username; }
            set { _username = value; }
        }
        public string Authdata
        {
            get { return _authdata; }
            set { _authdata = value; }
        }
        public string SessionId
        {
            get { return _sessionId; }
            set { _sessionId = value; }
        }
        public DateTime DateAdded
        {
            get { return _dateAdded; }
            set { _dateAdded = value; }
        }
        public string SessionDomain
        {
            get { return _sessionDomain; }
            set { _sessionDomain = value; }
        }
        #endregion
    }
}
