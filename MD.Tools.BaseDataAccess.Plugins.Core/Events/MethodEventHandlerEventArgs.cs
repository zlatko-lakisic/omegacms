using System;
using System.Collections.Generic;
using System.Text;

namespace MD.Tools.BaseDataAccess.Plugins.Core.Events
{
    public class MethodEventHandlerEventObjArg<T>
        where T : class
    {
        public T Obj { get; set; }
    }
    public class MethodEventHandlerEventArg
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
