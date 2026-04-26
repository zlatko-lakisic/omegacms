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
        public static IEnumerable<JsonNested> GetJson<T>(this IEnumerable<Folder<T>> folders)
            where T : Content, new()
        {
            List<JsonNested> nestedList = new List<JsonNested>();

            foreach (Folder<T> folder in folders)
            {
                JsonNested nested = new JsonNested()
                {
                    Name = folder.Name,
                    Value = folder.Id.ToString(),
                    ParentId = folder.ParentId.ToString()
                };
                nested.Children = folder.Children.GetJson();
                nestedList.Add(nested);
            }

            return nestedList;
        }
    }
}
