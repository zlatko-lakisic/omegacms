using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MD.Tools.BaseDataAccess.Plugins.Core.Mapping.ReportDefinition.ReportDesigner
{
    public class Limit
    {
        #region Attributes
        private int _from;
        private int _to;
        #endregion

        #region Properties
        /// <summary>
        /// Lower limit
        /// </summary>
        public int From
        {
            get { return _from; }
            set { _from = value; }
        }
        /// <summary>
        /// Upper limit
        /// </summary>
        public int To
        {
            get { return _to; }
            set { _to = value; }
        }
        #endregion
    }
}
