using MD.CMS.BusinessLogic.Core.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MD.CMS.BusinessLogic.Core.DataAccess
{
    public class DataAccessSettings
    {
        #region Attributes
        private static int _selectedLcid;
        #endregion

        #region Properties
        /// <summary>
        /// Selected LCID
        /// </summary>
        public static int SelectedLcid
        {
            get
            {
                if (_selectedLcid.Equals(default(int)))
                {
                    _selectedLcid = Settings.Default.DefaultLcid;
                }
                return _selectedLcid;
            }
            set { _selectedLcid = value; }
        }
        #endregion
    }
}
