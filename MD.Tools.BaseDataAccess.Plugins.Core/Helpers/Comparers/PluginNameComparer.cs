using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MD.Tools.BaseDataAccess.Plugins.Core.Helpers.Comparers
{
    public class PluginNameComparer : IComparer
    {
        /// <summary>
        /// Compares array of strings with the array of plugin names
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int Compare(object x, object y)
        {
            if (x is string)
            {
                string X = (string)x;
                if (y is IBaseDataAccessPlugin)
                {
                    IBaseDataAccessPlugin Y = (IBaseDataAccessPlugin)y;
                    return string.CompareOrdinal(X, Y.PluginName);
                }
            }
            if (x is IBaseDataAccessPlugin)
            {
                IBaseDataAccessPlugin X = (IBaseDataAccessPlugin)x;
                if (y is string)
                {
                    string Y = (string)y;
                    return string.CompareOrdinal(X.PluginName, Y);
                }
            }
            return 1;
        }
    }
}
