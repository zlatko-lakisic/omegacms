using MD.CMS.BusinessLogic.Core.DataAccess.Linq.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Linq.Content
{
    /// <summary>
    /// 
    /// </summary>
    public class ContentContext : ContentContext<Entities.Content>
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ContentContext<T> : BaseEntityQueryableContext<T, string>
        where T : Entities.Content, new()
    {
        public ContentContext() : base()
        {
            Provider = new ContentProvider<T>();
            Expression = System.Linq.Expressions.Expression.Constant(this);
        }
    }
}
