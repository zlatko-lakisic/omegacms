using MD.CMS.BusinessLogic.Core.DataAccess.Entities.Field;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities.GenericContent;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Entities
{
    public class ProfileTypeField : GenericContentField
    {
        #region Attributes
        private long _profileTypeId;
        #endregion

        #region Properties
        public long ProfileTypeId
        {
            get { return _profileTypeId; }
            set { _profileTypeId = value; }
        }

        public override string UniqueId
        {
            get
            {
                using (MD5 md5 = MD5.Create())
                {
                    byte[] hash = md5.ComputeHash(Encoding.Default.GetBytes(string.Format("{0}{1}", this.Id, this.ProfileTypeId)));
                    return string.Format("_{0}", (new Guid(hash)).ToString().Replace("-", string.Empty));
                }
            }
        }
        #endregion

        #region Methods
        public ProfileTypeField()
        {
            //DO NOTHING
        }

        public ProfileTypeField(ProfileTypeField obj) : base(obj)
        {
            _profileTypeId = obj.ProfileTypeId;
        }

        public ProfileTypeField(ProfileTypeFieldValue obj) : base(obj)
        {
            _profileTypeId = obj.ProfileTypeId;
        }
        #endregion
    }
}
