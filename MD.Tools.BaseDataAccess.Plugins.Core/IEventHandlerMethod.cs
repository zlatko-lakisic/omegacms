using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MD.Tools.BaseDataAccess.Plugins.Core
{
    public interface IEventHandlerMethod<M, P> : IMethod<M, P>
        where P : IMethodProperty
    {
        #region Properties
        event Method.OnBeforeExecuteHandler OnBeforeExecute;
        event Method.OnAfterExecuteHandler OnAfterExecute;
        void BindTaskStatus(ref IMethodStatus taskStatus);
        #endregion
    }
}
