using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MD.Tools.BaseDataAccess.Plugins.Core
{
	public class PagedMethod
	{
		#region Attributes
		private int _pagingFrom;
		private bool _pagingEnabled;
		private int _pagingSize;
		#endregion

		#region Properties
		public int PagingSize { get => _pagingSize; set => _pagingSize = value; }
		public bool PagingEnabled { get => _pagingEnabled; set => _pagingEnabled = value; }
		public int PagingFrom { get => _pagingFrom; set => _pagingFrom = value; }
		#endregion

		#region Methods
		public PagedMethod()
		{
			_pagingFrom = 0;
			_pagingSize = 10;
			_pagingEnabled = false;
		}
		public PagedMethod(int pagingFrom, int pagingSize, bool pagingEnabled)
		{
			_pagingFrom = pagingFrom;
			_pagingSize = pagingSize;
			_pagingEnabled = pagingEnabled;
		}
		#endregion
	}
}
