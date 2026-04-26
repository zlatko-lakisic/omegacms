using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace MD.Tools.Helpers.Core.Templating
{
    /// <summary>
    /// Orders Name Value Pairs by their key length and the alphabetically
    /// </summary>
    public class KeyLengthComparer : IComparer<string>
    {

        #region IComparer<KeyValuePair<string,object>> Members

        /// <summary>
        /// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>
        /// Value
        /// Condition
        /// Less than zero
        /// <paramref name="x"/> is less than <paramref name="y"/>.
        /// Zero
        /// <paramref name="x"/> equals <paramref name="y"/>.
        /// Greater than zero
        /// <paramref name="x"/> is greater than <paramref name="y"/>.
        /// </returns>
        public int Compare([AllowNull] string x, [AllowNull] string y)
        {
            if (x is null)
            {
                throw new ArgumentNullException(nameof(x));
            }

            if (y is null)
            {
                throw new ArgumentNullException(nameof(y));
            }

            int result = 0;
            result = x.Length.CompareTo(y.Length);
            if (result == 0) result = -string.Compare(x, y, StringComparison.OrdinalIgnoreCase);
            return -result;
        }
        #endregion
    }
}
