using ExpressionsAndIQueryable.Tests.Entities;
using System;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;

namespace ExpressionsAndIQueryable.Tests
{
    public class UserQueryProvider : IQueryProvider
    {
        #region [IQueryProvider]

        public IQueryable CreateQuery(Expression expression)
        {
            throw new NotImplementedException();
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            return new Query<TElement>(expression, this);
        }

        public object Execute(Expression expression)
        {
            throw new NotImplementedException();
        }

        public TResult Execute<TResult>(Expression expression)
        {
            var translator = new UserExpressionVisitor();
            var queryString = translator.Translate(expression);
            return (TResult)(IEnumerable)new UserEntity[] { new UserEntity() { Result = queryString } };
        }

        #endregion
    }
}
