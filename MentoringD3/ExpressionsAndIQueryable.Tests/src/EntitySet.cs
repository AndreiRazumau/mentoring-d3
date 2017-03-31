using ExpressionsAndIQueryable.Tests.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ExpressionsAndIQueryable.Tests
{
    public class EntitySet<T> : IQueryable<T> where T : UserEntity
    {
        #region [Construction]

        public EntitySet()
        {
            this.Expression = Expression.Constant(this);
            this.Provider = new UserQueryProvider();
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
