using ExpressionsAndIQueryable.Tests.E3SClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ExpressionsAndIQueryable.Tests
{
    public class E3SEntitySet<T> : IQueryable<T> where T : E3SEntity
    {
        #region [Construction]

        public E3SEntitySet()
        {
            this.Expression = Expression.Constant(this);
            this.Provider = new E3SLinqProvider();
        }

        #endregion

        #region [IQueryable]

        public Type ElementType
        {
            get
            {
                return typeof(T);
            }
        }

        public Expression Expression { get; }

        public IQueryProvider Provider { get; }

        public IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
