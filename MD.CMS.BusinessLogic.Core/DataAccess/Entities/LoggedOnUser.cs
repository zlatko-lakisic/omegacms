using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Entities
{
    public class LoggedOnUser : User
    {
        private string _sessionId;

        public string SessionId
        {
            get { return _sessionId; }
            set { _sessionId = value; }
        }

        public LoggedOnUser() : base() { }
        public LoggedOnUser(LoggedOnUser user) : base(user) { }
        public LoggedOnUser(User user) : base(user) { }
    }
}
