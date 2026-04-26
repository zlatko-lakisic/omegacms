using MD.Tools.BaseDataAccess.Core.Entities;
using MD.Tools.BaseDataAccess.Plugins.Core.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Entities
{
    public class ContentTypeDataSource : BaseEntity<long>
    {
        #region Attributes
        private long _contentTypeDefinitionId;
        private string _connectionString;
		private string _dbType;
		#endregion

		#region Properties
        public long ContentTypeDefinitionId
        {
            get { return _contentTypeDefinitionId; }
            set { _contentTypeDefinitionId = value; }
        }
        public string ConnectionString
        {
            get { return _connectionString; }
            set { _connectionString = value; }
        }

		public string DbType { get => _dbType; set => _dbType = value; }
		#endregion
	}
}
