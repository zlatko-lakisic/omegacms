using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Entities.Json
{
    public class JsonFieldOptionItem
    {
        public string label { get; set; }
        public bool @checked { get; set; }
        public override string ToString()
        {
            return label;
        }
    }
}
