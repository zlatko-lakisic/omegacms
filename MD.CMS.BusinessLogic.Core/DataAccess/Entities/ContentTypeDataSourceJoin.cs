using MD.Tools.BaseDataAccess.Core.Entities;
using MD.Tools.BaseDataAccess.Plugins.Core.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Entities
{
    public class ContentTypeDataSourceJoin
    {
        #region Attributes
        private long _rightDataSourceId;
        private string _leftRightDataSourceJoinType;
		private long _leftFieldId;
		private long _rightFieldId;
		#endregion

		#region Properties
		public long RightDataSourceId { get => _rightDataSourceId; set => _rightDataSourceId = value; }
		public string LeftRightDataSourceJoinType { get => _leftRightDataSourceJoinType; set => _leftRightDataSourceJoinType = value; }
		public long LeftFieldId { get => _leftFieldId; set => _leftFieldId = value; }
		public long RightFieldId { get => _rightFieldId; set => _rightFieldId = value; }
		#endregion
	}
}
