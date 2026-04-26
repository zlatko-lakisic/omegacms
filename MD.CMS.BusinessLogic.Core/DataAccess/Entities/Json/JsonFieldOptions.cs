using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Entities.Json
{
    public class JsonFieldOptions
    {
        public string size { get; set; }
        public int minlength { get; set; }
        public int maxlength { get; set; }
        public bool include_blank_option { get; set; }
        public JsonFieldOptionItem[] options { get; set; }
    }
}
