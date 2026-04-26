using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Entities.Field.Validation
{
    public class TypeValidationType : Editable
    {
        #region Attributes
        private EmailType _email;
        private WebAddressType _webAddress;
        #endregion

        #region Properties

        public EmailType Email
        {
            get { return _email; }
            set { _email = value; }
        }

        public WebAddressType WebAddress
        {
            get { return _webAddress; }
            set { _webAddress = value; }
        }
        #endregion

        #region Methods
        public TypeValidationType()
        {
            _email = new EmailType();
            _webAddress = new WebAddressType();
        }
        #endregion
    }
}
