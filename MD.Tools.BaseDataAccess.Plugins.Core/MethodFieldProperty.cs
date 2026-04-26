namespace MD.Tools.BaseDataAccess.Plugins.Core
{
    public class MethodFieldProperty : MethodProperty
    {
        #region Attributes
        private string _fieldName;
        private bool _isPrimaryKey;
        #endregion

        #region Properties
        public string FieldName { get => _fieldName; set => _fieldName = value; }
        public bool IsPrimaryKey { get => _isPrimaryKey; set => _isPrimaryKey = value; }
        #endregion

        #region Methods
        public MethodFieldProperty(string fieldName, bool isPrimaryKey) : base(default(int))
        {
            _fieldName = fieldName;
            _isPrimaryKey = isPrimaryKey;
        }
        #endregion
    }
}
