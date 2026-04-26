using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Entities.Field
{

    public class Options
    {
        public Validation.ValidationType validation { get; set; }
        public string helpText { get; set; }
        public string access { get; set; }
        public string cssClass { get; set; }
        public string toggle { get; set; }
        public bool hidden { get; set; }
        public bool enabled { get; set; }
        public dynamic style { get; set; }
        public List<KeyValuePair<string, string>> metadata { get; set; }
        public Json.JsonGridTileData gridTileData { get; set; }
        public CollectionType<Constraint> constraints { get; set; }
        public bool linkToTitle { get; set; }

        public Options()
        {
            enabled = true;
            gridTileData = new Json.JsonGridTileData();
        }
    }
}
