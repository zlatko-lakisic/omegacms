using System.Collections.Generic;

namespace MD.CMS.BusinessLogic.Core.Helpers.Collections
{
    public class EntityHierarchycalCollection<T> : List<T>, IHierarchicalEnumerable
        where T : class, new()
    {
        public IHierarchyData GetHierarchyData(object enumeratedItem)
        {
            return enumeratedItem as IHierarchyData;
        }

        public EntityHierarchycalCollection() : base()
        {

        }

        public EntityHierarchycalCollection(IEnumerable<T> items) : base(items)
        {

        }
    }
}
