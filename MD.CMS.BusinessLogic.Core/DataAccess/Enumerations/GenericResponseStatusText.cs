using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MD.Tools.Helpers.Core.TypeAttributes;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Enumerations
{
    public enum GenericResponseStatusText
    {
        [StringValue("Ok")]
        Ok,
        [StringValue("Fail")]
        Fail
    }
}