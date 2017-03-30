using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ExpressionsAndIQueryable.Tests
{
    public class ExpressiontTranslator : ExpressionVisitor
    {
        #region [Private members]

        private StringBuilder _resultString;

        #endregion

        #region [Construction]

        public string Translate(Expression expression)
        {
            this._resultString = new StringBuilder();
            this.Visit(expression);

            return this._resultString.ToString();
        }

        #endregion

        #region [Override methods]

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (node.Method.DeclaringType == typeof(Queryable) && node.Method.Name == "Where")
            {
                var predicate = node.Arguments[1];
                this.Visit(predicate);

                return node;
            }

            if (node.Method.DeclaringType == typeof(string) && node.Method.Name == "Contains")
            {
                var value = node.Arguments[0];
                this.Visit(value);

                return node;
            }

            return base.VisitMethodCall(node);
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            switch (node.NodeType)
            {
                case ExpressionType.Equal:
                    {
                        switch (node.Left.NodeType)
                        {
                            case ExpressionType.MemberAccess:
                                this.BinaryNodeTraversal(node.Left, node.Right);
                                break;
                            case ExpressionType.Constant:
                                this.BinaryNodeTraversal(node.Right, node.Left);
                                break;
                        }

                        break;
                    }

                default:
                    throw new NotSupportedException($"Operation {node.NodeType} is not supported");
            }

            return node;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            this._resultString.Append(node.Member.Name).Append(":");

            return base.VisitMember(node);
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            this._resultString.Append("(").Append(node.Value).Append(")");

            return base.VisitConstant(node);
        }

        #endregion

        #region [Private methods]

        private void BinaryNodeTraversal(Expression left, Expression right)
        {
            this.Visit(left);
            this.Visit(right);
        }

        #endregion
    }
}
