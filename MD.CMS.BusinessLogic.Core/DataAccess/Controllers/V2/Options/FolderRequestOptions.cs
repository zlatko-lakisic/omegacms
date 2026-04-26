using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.V2.Options.Interfaces;
using MD.CMS.BusinessLogic.Core.Properties;
using System.Collections.Generic;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Controllers.V2.Options
{
    public class FolderRequestOptions : IFolderRequestOptions
    {
        #region Attributes
        private IEnumerable<long> _folderIds;
        private IEnumerable<string> _paths;
        private int _lcid;
        private DataBoundContentRequestOptions _contentRequestOptions;
        private FolderRequestOptions _childFolderRequestOptions;
        private FolderRequestOptions _parentFolderRequestOptions;
        #endregion

        #region Properties
        public IEnumerable<long> FolderIds
        {
            get
            {
                if (_folderIds == null)
                {
                    _folderIds = new List<long>();
                }
                return _folderIds;
            }
            set => _folderIds = value;
        }
        public IEnumerable<string> Paths
        {
            get
            {
                if (_paths == null)
                {
                    _paths = new List<string>();
                }
                return _paths;
            }
            set => _paths = value;
        }
        public bool OnlyPublished { get; set; }
        public int CurrentPageIndex { get; set; }
        public int MaxNumberOfRows { get; set; }
        public string SearchTerm { get; set; }
        public Enumerations.Mapping.FolderEnum SortField { get; set; }
        public SortDirection SortDirection { get; set; }
        public int Lcid
        {
            get
            {
                if (_lcid.Equals(default))
                {
                    _lcid = Settings.Default.DefaultLcid;
                }
                return _lcid;
            }
            set => _lcid = value;
        }

        public bool FillParent { get; set; }

        public bool FillAllParents { get; set; }

        public int Depth { get; set; }

        public bool FillContents { get; set; }

        public bool FillChildren { get; set; }

        public DataBoundContentRequestOptions ContentRequestOptions 
        { 
            get
            {
                if(_contentRequestOptions == null)
                {
                    _contentRequestOptions = new DataBoundContentRequestOptions();
                }
                return _contentRequestOptions;
            }
            set
            {
                _contentRequestOptions = value;
            }
        }

        public FolderRequestOptions ChildFolderRequestOptions
        {
            get
            {
                if (_childFolderRequestOptions == null)
                {
                    _childFolderRequestOptions = new FolderRequestOptions();
                }
                return _childFolderRequestOptions;
            }
            set
            {
                _childFolderRequestOptions = value;
            }
        }

        public FolderRequestOptions ParentFolderRequestOptions
        {
            get
            {
                if (_parentFolderRequestOptions == null)
                {
                    _parentFolderRequestOptions = new FolderRequestOptions();
                }
                return _parentFolderRequestOptions;
            }
            set
            {
                _parentFolderRequestOptions = value;
            }
        }

        public long? ParentId { get; set; }

        public bool FillContentTypeDefinitions { get; set; }

        public bool FillTemplates { get; set; }
        #endregion

        #region Methods
        public FolderRequestOptions()
        {

        }

        public FolderRequestOptions(IFolderRequestOptions obj)
        {
            FolderIds = obj.FolderIds;
            Paths = obj.Paths;
            OnlyPublished = obj.OnlyPublished;
            CurrentPageIndex = obj.CurrentPageIndex;
            MaxNumberOfRows = obj.MaxNumberOfRows;
            SearchTerm = obj.SearchTerm;
            SortDirection = obj.SortDirection;
            Lcid = obj.Lcid;
            FillParent = obj.FillParent;
            FillAllParents = obj.FillAllParents;
            Depth = obj.Depth;
            FillContents = obj.FillContents;
            FillChildren = obj.FillChildren;
            ContentRequestOptions = obj.ContentRequestOptions;
            ChildFolderRequestOptions = obj.ChildFolderRequestOptions;
            ParentFolderRequestOptions = obj.ParentFolderRequestOptions;
            ParentId = obj.ParentId;
            FillContentTypeDefinitions = obj.FillContentTypeDefinitions;
            FillTemplates = obj.FillTemplates;
        }
        #endregion
    }
}
