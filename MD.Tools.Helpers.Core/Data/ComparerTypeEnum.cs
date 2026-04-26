using System;
using System.Collections.Generic;
using System.Text;

namespace MD.Tools.Helpers.Core.Data
{
	/// <summary>
	/// Comparer Type Enumeration
	/// </summary>
    public enum ComparerTypeEnum : int
	{
		/// <summary>
		/// Equals
		/// </summary>
		Equals = 1,
		/// <summary>
		/// Does not equal
		/// </summary>
		NotEquals = 2,
		/// <summary>
		/// Like
		/// </summary>
		Like = 3,
		/// <summary>
		/// Greater than (left) &gt; (right)
		/// </summary>
		GreaterThan = 4,
		/// <summary>
		/// Greater than or equal to (left) &gt;= (right)
		/// </summary>
		GreaterThanOrEqualTo = 5,
		/// <summary>
		/// Less than (left) &lt; (right)
		/// </summary>
		LessThan = 6,
		/// <summary>
		/// Less than or equal to (left) &lt;= (right)
		/// </summary>
		LessThanOrEqualTo = 7
	}
}
