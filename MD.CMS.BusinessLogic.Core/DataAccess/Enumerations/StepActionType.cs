using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MD.Tools.Helpers.Core.TypeAttributes;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Enumerations
{
    /// <summary>
    /// Step can finished as approved or rejected
    /// </summary>
    public enum StepActionType : int
    {
        Approved = 1,
        Rejected = 2
    }
}