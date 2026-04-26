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
    /// <typeparam name="T"></typeparam>
    public class ContentProvider<T> : BaseEntityQueryableProvider<ContentContext<T>, ContentQueryContext<T>, T, string>
        where T : Entities.Content, new()
    {
    }
}
