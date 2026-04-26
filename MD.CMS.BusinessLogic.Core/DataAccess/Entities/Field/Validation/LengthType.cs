using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Entities.Field.Validation
{
    public class LengthType : Editable
    {
        #region Attributes
        private int _length;
        #endregion

        #region Properties

        public int Length
        {
            get { return _length; }
            set { _length = value; }
        }
        #endregion
    }
}
