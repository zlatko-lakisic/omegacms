using MD.Tools.BaseDataAccess.Core.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Linq.Base
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseEntityQueryableContext<T> : BaseEntityQueryableContext<T, int>
        where T : BaseEntity
    {
        public BaseEntityQueryableContext(): base()
        {
        }

        internal BaseEntityQueryableContext(IQueryProvider provider, Expression expression)
            : base(provider, expression)
        {
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="N"></typeparam>
    public abstract class BaseEntityQueryableContext<T, N> : IOrderedQueryable<T>
        where T : BaseEntity<N>
    {
        public BaseEntityQueryableContext()
        {
        }

        internal BaseEntityQueryableContext(IQueryProvider provider, Expression expression)
        {
            Provider = provider;
            Expression = expression;
        }

        public Type ElementType
        {
            get { return typeof(T); }
        }

        public Expression Expression { get; protected set; }
        public IQueryProvider Provider { get; protected set; }

        public IEnumerator<T> GetEnumerator()
        {
            return Provider.Execute<IEnumerable<T>>(Expression).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
