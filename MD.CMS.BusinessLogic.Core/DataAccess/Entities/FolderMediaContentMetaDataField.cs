using MD.Tools.BaseDataAccess.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Entities
{
    public class FolderMediaContentMetaDataField : BaseEntity<long>
    {
        private long _folderId;
        private long _metaDataFieldId;
        private bool _isRequired;
        private bool _checked;
        private string _name;      
        public long FolderId
        {
            get { return _folderId; }
            set { _folderId = value; }
        }

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

        public bool Checked
        {
            get { return _checked; }
            set { _checked = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
    }
}
