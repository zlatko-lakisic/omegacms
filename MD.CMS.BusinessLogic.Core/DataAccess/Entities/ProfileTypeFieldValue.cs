using MD.CMS.BusinessLogic.Core.DataAccess.Entities.GenericContent;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Entities
{
    public class ProfileTypeFieldValue : GenericContentFieldValue
    {
        #region Attributes
        private long _profileTypeFieldId;
        private long _profileTypeId;
        private string _userId;
        #endregion

        #region Properties
        public long ProfileTypeFieldId
        {
            get { return _profileTypeFieldId; }
            set { _profileTypeFieldId = value; }
        }

        public long ProfileTypeId
        {
            get { return _profileTypeId; }
            set { _profileTypeId = value; }
        }

        public string UserId
        {
            get { return _userId; }
            set { _userId = value; }
        }
        #endregion


        public ProfileTypeFieldValue()
        {

        }

        public ProfileTypeFieldValue(ProfileTypeField obj) : base(obj)
        {
            if (obj != null)
            {
                ProfileTypeFieldId = obj.Id;
                ProfileTypeId = obj.ProfileTypeId;
            }
        }

        public ProfileTypeFieldValue(ProfileTypeFieldValue obj) : base(obj)
        {
            if (obj != null)
            {
                ProfileTypeFieldId = obj.Id;
                ProfileTypeId = obj.ProfileTypeId;
                UserId = obj.UserId;
            }
        }

        public ProfileTypeField ToProfileTypeField()
        {
            return new ProfileTypeField(this);
        }
    }
}
