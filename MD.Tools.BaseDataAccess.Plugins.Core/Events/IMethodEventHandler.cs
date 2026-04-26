using System;
using System.Collections.Generic;
using System.Text;

namespace MD.Tools.BaseDataAccess.Plugins.Core.Events
{
    public interface IMethodEventHandler<T>
        where T : class
    {
        Mapping.Entities Entity { get; }
        int MethodId { get; }
        Mapping.MethodTypes MethodType { get; }
        event MethodEventHandlerEvents<T>.OnBeforeExecuteHandler OnBeforeExecute;
        event MethodEventHandlerEvents<T>.OnAfterExecuteHandler OnAfterExecute;
    }
}
