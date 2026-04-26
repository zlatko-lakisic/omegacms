using System;
using System.Text;
using System.Security.Cryptography;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities.GenericContent;
using System.Collections.Generic;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Entities
{
    public class ContentTypeDefinitionField : GenericContentField
    {
        #region Attributes
        private long _contentTypeDefinitionId;
        private List<long> _referenceContentTypeDefinitionId;
        #endregion

        #region Properties

        public long ContentTypeDefinitionId
        {
            get { return _contentTypeDefinitionId; }
            set { _contentTypeDefinitionId = value; }
        }

        public override string UniqueId
        {
            get
            {
                using (MD5 md5 = MD5.Create())
                {
                    byte[] hash = md5.ComputeHash(Encoding.Default.GetBytes(string.Format("{0}{1}", this.Id, this.ContentTypeDefinitionId)));
                    return string.Format("_{0}", (new Guid(hash)).ToString().Replace("-", string.Empty));
                }
            }
        }

        public List<long> ReferenceContentTypeDefinitionId { get => _referenceContentTypeDefinitionId; set => _referenceContentTypeDefinitionId = value; }
        #endregion

        #region Methods
        public ContentTypeDefinitionField() : base()
        {
            _referenceContentTypeDefinitionId = new List<long>();
        }

        public ContentTypeDefinitionField(ContentTypeDefinitionField obj) : base(obj)
        {
            if (obj != null)
            {
                ContentTypeDefinitionId = obj.ContentTypeDefinitionId;
                ReferenceContentTypeDefinitionId = obj.ReferenceContentTypeDefinitionId;
            }
        }

        public ContentTypeDefinitionField(ContentTypeDefinitionFieldValue obj) : base(obj)
        {
            if (obj != null)
            {
                ContentTypeDefinitionId = obj.ContentTypeDefinitionId;
            }
        }
        #endregion
    }
}
