using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MD.Tools.Helpers.Core.TypeAttributes;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Enumerations
{
    /// <summary>
    /// Combination operator can be of type AND or OR. 
    /// AND operator requires content to be approved by all the administrators in the list 
    /// OR operator requires only one of them to approve.
    /// </summary>
    public enum StepComboOperator
    {
        AND = 1,
        OR = 2
    }
}