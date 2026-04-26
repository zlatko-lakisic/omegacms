using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MD.CMS.BusinessLogic.WebApi.Core.Modeles
{
    public class SimpleResponse<T>
    {
        public T Content { get; set; }

        public SimpleResponse(T value)
        {
            Content = value;
        }
    }

    public class SimpleResponse : SimpleResponse<string>
    {
        public SimpleResponse(string value) : base(value)
        {

        }
    }
}
