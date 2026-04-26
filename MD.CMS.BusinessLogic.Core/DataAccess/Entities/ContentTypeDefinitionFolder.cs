using MD.Tools.BaseDataAccess.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Entities
{
    public class ContentTypeDefinitionFolder : BaseEntity<long>
    {
        #region Attributes
        private long _folderId;
  
        private long _contenttypedefinitionId;
        private string _title;
        #endregion

        #region Properties

     

        public long FolderId
        {
            get { return _folderId; }
            set { _folderId = value; }
        }
        public long ContentTypeDefinitionId
        {
            get { return _contenttypedefinitionId; }
            set { _contenttypedefinitionId = value; }
        }
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }
        #endregion
    }
}
