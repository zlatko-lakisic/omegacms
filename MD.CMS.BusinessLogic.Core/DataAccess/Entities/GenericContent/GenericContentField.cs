using System;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Entities.GenericContent
{
    public abstract class GenericContentField : BaseDataBindableField
    {
        #region Attributes
        private string _description;
        private int _order;
        #endregion

        #region Properties

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public int Order
        {
            get { return _order; }
            set { _order = value; }
        }

        public virtual string UniqueId
        {
            get
            {
                using (MD5 md5 = MD5.Create())
                {
                    byte[] hash = md5.ComputeHash(Encoding.Default.GetBytes(string.Format("{0}", this.Id)));
                    return string.Format("_{0}", (new Guid(hash)).ToString().Replace("-", string.Empty));
                }
            }
        }
        #endregion

        #region Methods
        public GenericContentField() : base()
        {
            //DO NOTHING
        }

        public GenericContentField(GenericContentField obj) : base(obj)
        {
            if (obj != null)
            {
                _description = obj.Description;
                _order = obj.Order;
            }
        }
        #endregion
    }
}
