namespace MD.CMS.BusinessLogic.Core.DataAccess.Entities.GenericContent
{
    public abstract class BaseDataBindableField : BaseField
    {
        #region Attributes
		private bool _dataBound;
		private long _dataSourceId;
		private string _dataSourceField;
		private bool _dataBoundReadOnly;
        private bool _isDataBoundPrimaryKey;
        #endregion

        #region Properties
		public bool DataBoundReadOnly { get => _dataBoundReadOnly; set => _dataBoundReadOnly = value; }
		public string DataSourceField { get => _dataSourceField; set => _dataSourceField = value; }
		public long DataSourceId { get => _dataSourceId; set => _dataSourceId = value; }
		public bool DataBound { get => _dataBound; set => _dataBound = value; }
        public bool IsDataBoundPrimaryKey { get => _isDataBoundPrimaryKey; set => _isDataBoundPrimaryKey = value; }

        public override bool IsReadOnly
        {
            get
            {
                bool returnValue = false;

                if ((DataBound && DataBoundReadOnly) || (JsonField != null && !JsonField.enabled))
                {
                    returnValue = true;
                }

                return returnValue;
            }
        }
        #endregion

        #region Methods
        public BaseDataBindableField() : base()
        {
            //DO NOTHING
        }

        public BaseDataBindableField(BaseDataBindableField obj) : base(obj)
        {
            if (obj != null)
            {
                _dataBound = obj.DataBound;
                _dataBoundReadOnly = obj.DataBoundReadOnly;
                _dataSourceField = obj.DataSourceField;
                _dataSourceId = obj.DataSourceId;
                _isDataBoundPrimaryKey = obj.IsDataBoundPrimaryKey;
            }
        }
        #endregion
    }
}
