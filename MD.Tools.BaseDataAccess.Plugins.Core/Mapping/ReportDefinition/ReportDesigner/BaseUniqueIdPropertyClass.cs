using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MD.Tools.BaseDataAccess.Plugins.Core.Mapping.ReportDefinition.ReportDesigner
{
    public class BaseUniqueIdPropertyClass
    {
        #region Attributes
        private string _uniqueId;
        private Property _property;
        #endregion

        #region Properties
        /// <summary>
        /// Unique Id
        /// </summary>
        public string UniqueId
        {
            get { return _uniqueId; }
            set { _uniqueId = value; }
        }
        /// <summary>
        /// Property
        /// </summary>
        public Property Property
        {
            get { return _property; }
            set { _property = value; }
        }
        #endregion
    }
}
