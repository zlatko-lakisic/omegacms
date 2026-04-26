using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.V2.Options;
using MD.CMS.BusinessLogic.Core.DataAccess.Linq.Base;
using MD.CMS.BusinessLogic.Core.Helpers.Extensions;
using MD.Tools.Helpers.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Linq.Content
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ContentQueryContext<T> : BaseEntityQueryableQueryContext<T, string>, ISingleton<ContentQueryContext<T>>
        where T : Entities.Content, new()
    {
        private static ContentQueryContext<T> _singletonInstance;

        public ContentQueryContext<T> GetSingletonInstance()
        {
            if(_singletonInstance == null)
            {
                _singletonInstance = new ContentQueryContext<T>();
            }
            return _singletonInstance;
        }

        internal override object Execute(Expression expression, bool isEnumerable)
        {
            Controllers.DataBoundContentController<T>.GetNewInstance().Caller(UserMakingTheCall).DefaultPlugin(UseDefaultPlugin).Execute(new DataBoundContentRequestOptions());
            return null;
        }
    }
}
