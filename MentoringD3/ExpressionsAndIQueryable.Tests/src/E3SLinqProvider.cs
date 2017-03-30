using ExpressionsAndIQueryable.Tests.E3SClient;
using System;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;

namespace ExpressionsAndIQueryable.Tests
{
    public class E3SLinqProvider : IQueryProvider
    {
        #region [IQueryProvider]

        public IQueryable CreateQuery(Expression expression)
        {
            throw new NotImplementedException();
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            return new E3SQuery<TElement>(expression, this);
        }

        public object Execute(Expression expression)
        {
            throw new NotImplementedException();
        }

        public TResult Execute<TResult>(Expression expression)
        {
            var translator = new ExpressiontTranslator();
            var queryString = translator.Translate(expression);
            return (TResult)(IEnumerable)new E3SEntity[] { new E3SEntity() { Result = queryString } };
        }

        #endregion
    }
}
