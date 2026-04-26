using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MD.Tools.BaseDataAccess.Core.Entities
{
    public abstract partial class BaseEntity : BaseEntity<int>
    {
        //Empty Class
    }

    public abstract partial class BaseEntity<T>
    {
        #region Attributes
        private T _id;
        private bool _isDeleted;
        public delegate void NotFoundEvent();
        private NotFoundEvent _notFound;
        #endregion

        #region Properties
        /// <summary>
        /// Id
        /// </summary>
        public T Id
        {
            get { return _id; }
            set { _id = value; }
        }
        /// <summary>
        /// Is Deleted?
        /// </summary>
        public bool IsDeleted
        {
            get { return _isDeleted; }
            set { _isDeleted = value; }
        }
        /// <summary>
        /// Not found event callback
        /// </summary>
        public NotFoundEvent NotFound
        {
            get { return _notFound; }
            set { _notFound = value; }
        }
        #endregion

        #region Methods
        public BaseEntity() { }
        public BaseEntity(BaseEntity<T> obj)
        {
            this._id = obj.Id;
            this._isDeleted = obj._isDeleted;
        }

        public virtual bool ShouldSerializeId()
        {
            return true;
        }

        public virtual bool ShouldSerializeIsDeleted()
        {
            return true;
        }

        public virtual bool ShouldSerializeNotFound()
        {
            return false;
        }

        public virtual string GetPermissionEntityId()
        {
            return _id.ToString();
        }
        #endregion
    }
}
