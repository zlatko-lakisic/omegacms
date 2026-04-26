using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MD.CMS.BusinessLogic.Core.Helpers.Extensions.Entities
{
    public static partial class EntityExtensions
    {
        public static IEnumerable<JsonNested> GetJson(this IEnumerable<Taxonomy> taxonomies)
        {
            List<JsonNested> nestedList = new List<JsonNested>();

            foreach(Taxonomy taxonomy in taxonomies)
            {
                JsonNested nested = new JsonNested()
                {
                    Name = taxonomy.Name,
                    Value = taxonomy.Id.ToString(),
                    ParentId = taxonomy.ParentId.ToString()
                };
                nested.Children = taxonomy.Children.GetJson();
                nestedList.Add(nested);
            }

            return nestedList;
        }
    }
}
