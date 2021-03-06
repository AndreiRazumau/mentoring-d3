﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ExpressionsAndIQueryable.Tests
{
    public class Query<T> : IQueryable<T>
    {
        #region [Construction]

        public Query(Expression expression, UserQueryProvider provider)
        {
            this.Expression = expression;
            this.Provider = provider;
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
            return this.Provider.Execute<IEnumerable<T>>(this.Expression).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)this).GetEnumerator();
        }

        #endregion
    }
}
