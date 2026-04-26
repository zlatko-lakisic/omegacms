using MD.CMS.BusinessLogic.Core.DataAccess.Entities.GenericContent;
using System;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Entities
{
    public class ContentTypeDefinitionFieldValue : GenericContentFieldValue
    {
        #region Attributes
        private long _contentTypeDefinitionFieldId;
        private long _contentTypeDefinitionId;
        private string _contentId;
        private int _lCID;
        private DateTime _dateCreated;
        #endregion

        #region Properties
        public long ContentTypeDefinitionFieldId
        {
            get { return _contentTypeDefinitionFieldId; }
            set 
            { 
                _contentTypeDefinitionFieldId = value;
                Id = value;
            }
        }

        public long ContentTypeDefinitionId
        {
            get { return _contentTypeDefinitionId; }
            set { _contentTypeDefinitionId = value; }
        }

        public string ContentId
        {
            get { return _contentId; }
            set { _contentId = value; }
        }

        public int LCID
        {
            get { return _lCID; }
            set { _lCID = value; }
        }

        public DateTime DateCreated
        {
            get { return _dateCreated; }
            set 
            {
                _dateCreated = value;
            }
        }
        #endregion

        public ContentTypeDefinitionFieldValue() : base()
        {
        }

        public ContentTypeDefinitionFieldValue(ContentTypeDefinitionField obj) : base(obj)
        {
            if (obj != null)
            {
                ContentTypeDefinitionId = obj.ContentTypeDefinitionId;
                ContentTypeDefinitionFieldId = obj.Id;
            }
        }

        public ContentTypeDefinitionFieldValue(ContentTypeDefinitionFieldValue obj) : base(obj)
        {
            if (obj != null)
            {
                ContentTypeDefinitionId = obj.ContentTypeDefinitionId;
                ContentTypeDefinitionFieldId = obj.ContentTypeDefinitionFieldId;
                LCID = obj.LCID;
                ContentId = obj.ContentId;
                DateCreated = obj.DateCreated;
            }
        }
    }
}
