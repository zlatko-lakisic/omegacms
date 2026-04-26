using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MD.Tools.Helpers.Core.TypeAttributes;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Enumerations
{
    /// <summary>
    /// Actions can be REDIRECT, PUBLISH and END respectively. 
    /// Redirect is used when chain needs to continue the flow or go step back. 
    /// Publish will end chain and publish content and it will be used only on last step. 
    /// End action will end chain without publishing content.
    /// </summary>
    public enum StepActionAction : int
    {
        Redirect = 1,
        Publish = 2,
        End = 3
    }
}