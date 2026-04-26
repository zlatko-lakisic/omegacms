using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MD.CMS.BusinessLogic.Administration.Core.Resources
{
    public class ResourceManager
    {
        #region Attributes
        private static Dictionary<string, System.Resources.ResourceManager> _loadedresources;
        #endregion

        #region Properties
        /// <summary>
        /// Loaded system resources
        /// </summary>
        public static Dictionary<string, System.Resources.ResourceManager> Loadedresources
        {
            get 
            {
                if (_loadedresources == null)
                {
                    _loadedresources = new Dictionary<string, System.Resources.ResourceManager>();
                }
                return _loadedresources; 
            }
        }
        #endregion
    }
}
