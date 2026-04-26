using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MD.Tools.BaseDataAccess.Core.Interfaces
{
    public interface IBaseWebRequestProperty
    {
        //PROPERTIES
        string Name { get; }
        object Value { get; set; }
        List<IBaseWebRequestProperty> Items { get; set; }
        bool IsArray { get; }
        bool IsQueryStringParam { get; }

        //METHODS
        string ToJson();
        string ToUrlEncodedValue();
    }
}
