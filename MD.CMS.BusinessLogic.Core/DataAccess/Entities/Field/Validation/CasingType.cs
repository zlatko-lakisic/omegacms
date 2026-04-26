using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Entities.Field.Validation
{
    public class CasingType : Editable
    {
        #region Attributes
        private bool _upperCase;
        private bool _lowerCase;
        #endregion

        #region Properties

        public bool UpperCase
        {
            get { return _upperCase; }
            set { _upperCase = value; }
        }

        public bool LowerCase
        {
            get { return _lowerCase; }
            set { _lowerCase = value; }
        }
        #endregion
    }
}
