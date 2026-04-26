using MD.Tools.BaseDataAccess.Core.Entities;
using System.Linq.Expressions;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Linq.Base
{
    public abstract class BaseEntityQueryableQueryContext<T> : BaseEntityQueryableQueryContext<T, int>
        where T : BaseEntity
    {
    }

    public abstract class BaseEntityQueryableQueryContext<T, N>
        where T : BaseEntity<N>
    {
        #region Attributes
        private bool _useDefaultPlugin;
        private Entities.User _userMakingTheCall;
        #endregion

        #region Properties
        /// <summary>
        /// Wether to use the default plugin
        /// </summary>
        public bool UseDefaultPlugin
        {
            get { return _useDefaultPlugin; }
            set { _useDefaultPlugin = value; }
        }
        /// <summary>
        /// What user is making this call - This is not needed for public calls
        /// </summary>
        public Entities.User UserMakingTheCall
        {
            set
            {
                _userMakingTheCall = value;
            }
            get
            {
                return _userMakingTheCall;
            }
        }
        internal abstract object Execute(Expression expression, bool isEnumerable);
        #endregion
    }
}
