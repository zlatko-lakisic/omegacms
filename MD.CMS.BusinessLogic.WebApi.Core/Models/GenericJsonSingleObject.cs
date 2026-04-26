using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MD.CMS.BusinessLogic.WebApi.Core.Modeles
{
    public class GenericJsonSingleObject<T> 
    {
        public string ValueName { get; set; }
        public T[] ValueArray { get; set; }

    }
}