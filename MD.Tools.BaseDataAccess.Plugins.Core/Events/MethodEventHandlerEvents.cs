using System;
using System.Collections.Generic;
using System.Text;

namespace MD.Tools.BaseDataAccess.Plugins.Core.Events
{
    public class MethodEventHandlerEvents<T>
        where T : class
    {
        public delegate void OnBeforeExecuteHandler(MethodEventHandlerEventObjArg<T> objArg, params MethodEventHandlerEventArg[] args);
        public delegate void OnAfterExecuteHandler(MethodEventHandlerEventObjArg<T> objArg, params MethodEventHandlerEventArg[] args);
    }
}
