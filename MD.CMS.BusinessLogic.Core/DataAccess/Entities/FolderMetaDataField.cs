using MD.Tools.BaseDataAccess.Core.Entities;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Entities
{
    public class FolderMetaDataField : BaseEntity<long>
    {
        #region Attributes
        private long _folderId;
        private bool _checked;
        private long _metaDataFieldId;
        private bool _isRequired;
        private string _name;
        #endregion

        #region Properties
        public long MetaDataFieldId
        {
            get { return _metaDataFieldId; }
            set { _metaDataFieldId = value; }
        }

        public bool IsRequired
        {
            get { return _isRequired; }
            set { _isRequired = value; }
        }

        public long FolderId
        {
            get { return _folderId; }
            set { _folderId = value; }
        }

        public bool Checked
        {
            get { return _checked; }
            set { _checked = value; }
        }

        public string Name { get => _name; set => _name = value; }
        #endregion

        #region Methods
        public FolderMetaDataField()
        {
        }

        public FolderMetaDataField(FolderMetaDataField obj) :
            base(obj)
        {
            this._checked = obj.Checked;
            this._isRequired = obj.IsRequired;
            this._folderId = obj.FolderId;
            this._metaDataFieldId = obj.MetaDataFieldId;
            this._name = obj.Name;
        }
        #endregion
    }
}
