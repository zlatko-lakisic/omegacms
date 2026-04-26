using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MD.Tools.BaseDataAccess.Plugins.Core
{
    public interface IMethodStatus
    {
        Boolean OperationStarted { get; }
        Boolean? OnAfterCompleted { get; set; }
        Boolean? OnBeforeCompleted { get; set; }
    }
}
