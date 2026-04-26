using System.Collections.Generic;
using MD.CMS.BusinessLogic.Core.Properties;
using System;
using MD.CMS.BusinessLogic.Core.Helpers.Collections;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities.Base;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Entities
{
    public class Menu : BaseHierarchycEntity<Menu, long>, IHierarchyData
    {
        #region Attributes
        private long _parentId;
        private string _name;
        private string _description;
        private Menu _parent;
        private List<Menu> _children;
        private List<MenuContent> _items;
        private List<Content> _contents;
        private string _freeTextField;
        private int _lcid;
        private int _folderId;
        private string _menuPath;
        private string _options;
        private int _childrenTotalCount;
        private int _itemsTotalCount;
        private int _contentsTotalCount;
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

        public override Menu Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }

        public long ParentId
        {
            get { return _parentId; }
            set { _parentId = value; }
        }

        public override List<Menu> Children
        {
            get
            {
                if (_children == null)
                {
                    _children = new List<Menu>();
                }
                return _children;
            }
            set { _children = value; }
        }

        public bool IsNew
        {
            get
            {
                return Id.Equals(default(long));
            }
        }

        public List<MenuContent> Items
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

        public string MenuPath
        {
            get { return _menuPath; }
            set { _menuPath = value; }
        }

        public List<Content> Contents
        {
            get { return _contents; }
            set { _contents = value; }
        }

        public string Options
        {
            get { return _options; }
            set { _options = value; }
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

        public int ContentsTotalCount
        {
            get { return _contentsTotalCount; }
            set { _contentsTotalCount = value; }
        }
        #endregion
    }
}
