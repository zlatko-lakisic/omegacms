using MD.Tools.Helpers.Core.TypeAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Enumerations.Mapping
{
    internal enum ContentTypeDataSourceJoinEnum
	{
        [StringValue("RightDataSourceId")]
		RightDataSourceId,
        [StringValue("LeftRightDataSourceJoinType")]
		LeftRightDataSourceJoinType,
        [StringValue("LeftFieldId")]
		LeftFieldId,
		[StringValue("RightFieldId")]
		RightFieldId
	}
}
