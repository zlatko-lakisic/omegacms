using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Entities.Json
{
    public class JsonField
    {
        public string label { get; set; }
        public string field_type { get; set; }
        public string cid { get; set; }
        public bool required { get; set; }
        public long field_id { get; set; }
        public JsonFieldOptions field_options { get; set; }
    }
}
