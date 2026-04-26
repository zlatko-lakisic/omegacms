using System;
using System.Collections.Generic;
using System.Text;

namespace MD.Tools.Helpers.Core.Data
{
    /// <summary>
    /// Data Transform
    /// </summary>
    public enum DataTransformEnum : int
    {
        /// <summary>
        /// To string
        /// </summary>
        ToString = 1,
        /// <summary>
        /// To integer
        /// </summary>
        ToInt = 2,
        /// <summary>
        /// To long
        /// </summary>
        ToLong = 3,
        /// <summary>
        /// To date time
        /// </summary>
        ToDateTime = 4,
        /// <summary>
        /// To float
        /// </summary>
        ToFloat = 5
    }
}
