using MD.Tools.BaseDataAccess.Core.Entities;
using MD.CMS.BusinessLogic.Core.Helpers.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities.Permissions;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Entities.Base
{
    /// <summary>
    /// Base Heirartcyc Entity
    /// </summary>
    /// <typeparam name="T">Class type to implement heirarchy on, must implement IHierarchyData</typeparam>
    /// <typeparam name="K">Base entity ID type</typeparam>

    public abstract class BaseHierarchycEntity<T> : BaseHierarchycEntity<T, long>
        where T : class, IHierarchyData, new()
    {

    }

    public abstract class BaseHierarchycEntity<T, K> : BaseEntity<K>
        where T : class, IHierarchyData, new()
    {
        #region IHierarchyData Members

        abstract public T Parent { get; set; }

        abstract public List<T> Children { get; set; }

        private string _entityPath;

        public string EntityPath
        {
            get { return _entityPath; }
            set { _entityPath = value; }
        }

        public IHierarchicalEnumerable GetChildren()
        {

            EntityHierarchycalCollection<T> children = new EntityHierarchycalCollection<T>();
            children.AddRange(Children);

            return children;
        }

        public IHierarchyData GetParent()
        {
            return Parent;
        }

        public bool HasChildren
        {
            get
            {
                return Children.Count > 0;
            }
        }

        // Gets the hierarchical data node that the object represents.
        public object Item
        {
            get { return this.Id; }
        }

        // Gets the hierarchical path of the node.
        public string Path
        {
            get { return this.EntityPath; }
        }

        public string Type
        {
            get { return GetType().ToString(); }
        }
        #endregion
    }
}
