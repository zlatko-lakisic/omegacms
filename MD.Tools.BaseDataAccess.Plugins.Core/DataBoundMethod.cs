using MD.Tools.BaseDataAccess.Plugins.Core.Mapping;
using MD.Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFolderDataBoundCondition;
using MD.Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFolderDataBoundSorting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MD.Tools.BaseDataAccess.Plugins.Core
{
	public class DataBoundMethod : PagedMethod, IMethodStatus, IDisposable
	{
		#region Attributes
		private string _databaseType;
		private string _connectionString;
		private IEnumerable<string> _fields;
        private List<ContentTypeDefinitionFolderDataBoundCondition> _conditions;
        private List<ContentTypeDefinitionFolderDataBoundSorting> _sorts;
        private Boolean _operationStarted;
		private Boolean? _onAfterCompleted;
		private Boolean? _onBeforeCompleted;
		private Boolean _reindexingFinished;
		private bool _endCalled;
		private List<MethodFieldProperty> _fieldProperties;
		private Mapping.MethodTypes _methodType;
		#endregion

		#region Properties
		public string DatabaseType
		{
			get
			{
				return _databaseType;
			}
		}
		public dynamic ConnectionString
		{
			get
			{
				if(!string.IsNullOrEmpty(_connectionString)){
					return JsonConvert.DeserializeObject<dynamic>(_connectionString);
				}
				return null;
			}
		}

		public IEnumerable<string> Fields { get => _fields; }
		/// <summary>
		/// Operation status
		/// </summary>
		public Boolean OperationStarted
		{
			get { return _operationStarted; }
		}

		public Boolean? OnAfterCompleted
		{
			get { return _onAfterCompleted; }
			set { _onAfterCompleted = value; }
		}

		public Boolean? OnBeforeCompleted
		{
			get { return _onBeforeCompleted; }
			set { _onBeforeCompleted = value; }
		}

		public List<ContentTypeDefinitionFolderDataBoundCondition> Conditions { get => _conditions; set => _conditions = value; }
        public List<ContentTypeDefinitionFolderDataBoundSorting> Sorts { get => _sorts; set => _sorts = value; }
		public List<MethodFieldProperty> FieldProperties { get => _fieldProperties; set => _fieldProperties = value; }
		public MethodTypes MethodType { get => _methodType; set => _methodType = value; }
		#endregion

		#region Methods
		public DataBoundMethod(string databaseType, string connectionString)
		{
			_databaseType = databaseType;
			_connectionString = connectionString;
			_operationStarted = true;
			_reindexingFinished = false;
			_onAfterCompleted = null;
			_onBeforeCompleted = null;
			_endCalled = false;
			_conditions = new List<ContentTypeDefinitionFolderDataBoundCondition>();
            _sorts = new List<ContentTypeDefinitionFolderDataBoundSorting>();
			_fieldProperties = new List<MethodFieldProperty>();
		}
		public DataBoundMethod(string databaseType, string connectionString, IEnumerable<string> fields)
		{
			_databaseType = databaseType;
			_connectionString = connectionString;
			_fields = fields;
			_operationStarted = true;
			_reindexingFinished = false;
			_onAfterCompleted = null;
			_onBeforeCompleted = null;
			_endCalled = false;
			_conditions = new List<ContentTypeDefinitionFolderDataBoundCondition>();
			_sorts = new List<ContentTypeDefinitionFolderDataBoundSorting>();
			_fieldProperties = new List<MethodFieldProperty>();
		}
		public DataBoundMethod(DataBoundMethod obj)
		{
			_databaseType = obj._databaseType;
			_connectionString = obj._connectionString;
			_fields = obj._fields;
			_operationStarted = obj._operationStarted;
			_reindexingFinished = obj._reindexingFinished;
			_onAfterCompleted = obj._onAfterCompleted;
			_onBeforeCompleted = obj._onBeforeCompleted;
			_endCalled = obj._endCalled;
			_conditions = obj._conditions;
			_sorts = obj._sorts;
			_fieldProperties = obj._fieldProperties;
		}

		public void Dispose()
		{
			//Do Nothing
		}
		#endregion
	}
}
