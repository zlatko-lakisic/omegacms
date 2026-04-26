using MD.Tools.BaseDataAccess.Plugins.Core.Mapping.ReportDefinition.ReportDesigner.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MD.Tools.BaseDataAccess.Plugins.Core.Mapping.ReportDefinition.ReportDesigner
{
    public class Column : BaseUniqueIdPropertyClass
    {
        #region Attributes
        private ColumnModificationType _type;
        private string _value;
        #endregion

        #region Properties
        /// <summary>
        /// Column ModificationType Type
        /// </summary>
        public ColumnModificationType Type
        {
            get { return _type; }
            set { _type = value; }
        }
        /// <summary>
        /// Filter Value
        /// </summary>
        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }
        #endregion
    }
}
