using MD.Tools.BaseDataAccess.Core.Entities;
using MD.Tools.Helpers.Core;
using System.Linq;
using System.Linq.Expressions;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Linq.Base
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="C"></typeparam>
    /// <typeparam name="QC"></typeparam>
    /// <typeparam name="E"></typeparam>
    public abstract class BaseEntityQueryableProvider<C, QC, E> : BaseEntityQueryableProvider<C, QC, E, int>
        where C : BaseEntityQueryableContext<E, int>, new()
        where QC : BaseEntityQueryableQueryContext<E>, ISingleton<QC>, new()
        where E : BaseEntity
    {
        public BaseEntityQueryableProvider()
        {

        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="C"></typeparam>
    /// <typeparam name="QC"></typeparam>
    /// <typeparam name="E"></typeparam>
    /// <typeparam name="N"></typeparam>
    public abstract class BaseEntityQueryableProvider<C, QC, E, N> : IQueryProvider
        where C : BaseEntityQueryableContext<E, N>, new()
        where QC : BaseEntityQueryableQueryContext<E, N>, ISingleton<QC>, new()
        where E : BaseEntity<N>
    {
        public class QueryContext
        {
            internal static object Execute(Expression expression, bool isEnumerable)
            {
                return null;
            }
        }


        public IQueryable CreateQuery(Expression expression)
        {
            C obj = new C();
            return obj;
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            C obj = new C();
            return (IQueryable<TElement>) obj;
        }

        public object Execute(Expression expression)
        {
            return Execute<E>(expression);
        }

        public TResult Execute<TResult>(Expression expression)
        {
            var isEnumerable = (typeof(TResult).Name == "IEnumerable`1");
            return (TResult)new QC().Execute(expression, isEnumerable);
        }
    }
}
