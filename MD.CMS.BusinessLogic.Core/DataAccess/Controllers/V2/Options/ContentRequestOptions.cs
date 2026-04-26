using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.V2.Options.Interfaces;
using MD.CMS.BusinessLogic.Core.Properties;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Controllers.V2.Options
{
    public class ContentRequestOptions : IContentRequestOptions
    {
        #region Enums
        #endregion

        #region Attributes
        private IEnumerable<string> _contentIds;
        private int _lcid;
        #endregion

        #region Properties
        public IEnumerable<string> ContentIds
        {
            get
            {
                if (_contentIds == null)
                {
                    _contentIds = new List<string>();
                }
                return _contentIds;
            }
            set => _contentIds = value;
        }
        public bool LoadAuthor { get; set; }
        public bool FillFields { get; set; }
        public bool FillMetaData { get; set; }
        public bool OnlyPublished { get; set; }
        public long FolderId { get; set; }
        public int CurrentPageIndex { get; set; }
        public int MaxNumberOfRows { get; set; }
        public string SearchTerm { get; set; }
        public Enumerations.Mapping.ContentEnum SortField { get; set; }
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
        public long TaxonomyId { get; set; }
        public long MenuId { get; set; }
        public string Alias { get; set; }

        public bool DataBound { get; set; }
        public long ContentTypeId { get; set; }
        #endregion

        #region Methods
        public ContentRequestOptions()
        {

        }

        public ContentRequestOptions(IContentRequestOptions obj)
        {
            ContentIds = obj.ContentIds;
            LoadAuthor = obj.LoadAuthor;
            FillFields = obj.FillFields;
            FillMetaData = obj.FillMetaData;
            OnlyPublished = obj.OnlyPublished;
            FolderId = obj.FolderId;
            CurrentPageIndex = obj.CurrentPageIndex;
            MaxNumberOfRows = obj.MaxNumberOfRows;
            SearchTerm = obj.SearchTerm;
            SortField = obj.SortField;
            SortDirection = obj.SortDirection;
            Lcid = obj.Lcid;
            TaxonomyId = obj.TaxonomyId;
            MenuId = obj.MenuId;
            Alias = obj.Alias;
            ContentTypeId = obj.ContentTypeId;
        }
        #endregion
    }
}
