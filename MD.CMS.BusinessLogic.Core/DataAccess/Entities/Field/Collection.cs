using System.Collections.Generic;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Entities.Field
{
    public class CollectionType<T>
    {
        public IEnumerable<KeyValuePair<string, T>> Collection { get; set; }
    }
}
