using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Entities.Field.Validation
{
    public class EmailType : Editable
    {
        #region Attributes
        private string _domain;
        private string _extension;
        #endregion

        #region Properties

        public string Domain
        {
            get { return _domain; }
            set { _domain = value; }
        }

        public string Extension
        {
            get { return _extension; }
            set { _extension = value; }
        }
        #endregion
    }
}
