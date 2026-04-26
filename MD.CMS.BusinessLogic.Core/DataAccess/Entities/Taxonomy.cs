using System.Collections.Generic;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities.Base;
using MD.CMS.BusinessLogic.Core.Helpers.Collections;
using MD.CMS.BusinessLogic.Core.Properties;
using MD.Tools.BaseDataAccess.Plugins.Core.Mapping.ReportDefinition.ReportDesigner.CustomAttributes;
namespace MD.CMS.BusinessLogic.Core.DataAccess.Entities
{
    public class Taxonomy : BaseHierarchycEntity<Taxonomy, long>, IHierarchyData
    {
        #region Attributes
        private long _parentId;
        private string _name;
        private string _description;
        private Taxonomy _parent;
        private List<Taxonomy> _children;
        private List<TaxonomyContent> _items;
        private List<Content> _contents;
        private string _freeTextField;
        private int _lcid;
        private int _folderId;
        private string _taxonomyPath;
        private int _childrenTotalCount;
        private int _itemsTotalCount;
        private int _order;
        #endregion

        #region Properties
        public int FolderId
        {
            get { return _folderId; }
            set { _folderId = value; }
        }

        public int LCID
        {
            get
            {
                if (_lcid.Equals(default(int)))
                    _lcid = Settings.Default.DefaultLcid;
                return _lcid;
            }
            set { _lcid = value; }
        }
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        [OmitPropertyFromReport]
        public override Taxonomy Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }

        public long ParentId
        {
            get { return _parentId; }
            set { _parentId = value; }
        }

        [OmitPropertyFromReport]
        public override List<Taxonomy> Children
        {
            get 
            {
                if (_children == null)
                {
                    _children = new List<Taxonomy>();
                }
                return _children; 
            }
            set { _children = value; }
        }

        [OmitPropertyFromReport]
        public List<Content> Contents
        {
            get { return _contents; }
            set { _contents = value; }
        }

        public bool IsNew
        {
            get
            {               
               return Id.Equals(default(long));              
            }
        }

        public List<TaxonomyContent> Items
        {
            get { return _items; }
            set { _items = value; }
        }

        public override string ToString()
        {
            return Id.ToString();
        }

        public string FreeTextField
        {
            get { return _freeTextField; }
            set { _freeTextField = value; }
        }

        public string TaxonomyPath
        {
            get { return _taxonomyPath; }
            set { _taxonomyPath = value; }
        }

        public int ChildrenTotalCount
        {
            get { return _childrenTotalCount; }
            set { _childrenTotalCount = value; }
        }

        public int ItemsTotalCount
        {
            get { return _itemsTotalCount; }
            set { _itemsTotalCount = value; }
        }

        public int Order { get => _order; set => _order = value; }
        #endregion
    }
}
