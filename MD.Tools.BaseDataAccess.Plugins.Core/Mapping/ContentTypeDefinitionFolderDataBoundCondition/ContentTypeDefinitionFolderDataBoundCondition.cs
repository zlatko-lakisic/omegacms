using MD.Tools.Helpers.Core.Data;

namespace MD.Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFolderDataBoundCondition
{
    public class ContentTypeDefinitionFolderDataBoundCondition
    {
        #region Enum
        public enum ConditionType
        {
            None = 0,
            PrimaryKey = 1
        }
        #endregion

        #region Attributes
        private long _contentTypeDefinitionId;
        private long _folderId;
        private long _contentTypeDefinitionFieldId;
        private object _value;
        private ComparerTypeEnum _comparer;
        private string _leftField;
        private ConditionType _type;
        #endregion

        #region Properties
        public object Value { get => _value; set => _value = value; }
        public long ContentTypeDefinitionFieldId { get => _contentTypeDefinitionFieldId; set => _contentTypeDefinitionFieldId = value; }
        public long FolderId { get => _folderId; set => _folderId = value; }
        public long ContentTypeDefinitionId { get => _contentTypeDefinitionId; set => _contentTypeDefinitionId = value; }
        public ComparerTypeEnum Comparer { get => _comparer; set => _comparer = value; }
        public string LeftField { get => _leftField; set => _leftField = value; }
        public ConditionType Type { get => _type; set => _type = value; }
        #endregion
    }
}
