using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Entities.Field.Validation
{
    public class NumbersType : Editable
    {
        #region Attributes
        private int _from;
        private int _to;
        #endregion

        #region Properties

        public int To
        {
            get { return _to; }
            set { _to = value; }
        }

        public int From
        {
            get { return _from; }
            set { _from = value; }
        }
        #endregion
    }
}
