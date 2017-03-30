using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ExpressionsAndIQueryable.Tests
{
    public class ExpressionToFTSRequestTranslator : ExpressionVisitor
    {
        private StringBuilder _resultString;

        public string Translate(Expression exp)
        {
            this._resultString = new StringBuilder();
            this.Visit(exp);

            return this._resultString.ToString();
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (node.Method.DeclaringType == typeof(Queryable) && node.Method.Name == "Where")
            {
                var predicate = node.Arguments[1];
                this.Visit(predicate);

                return node;
            }

            return base.VisitMethodCall(node);
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            switch (node.NodeType)
            {
                case ExpressionType.Equal:
                    if (!(node.Left.NodeType == ExpressionType.MemberAccess))
                    {
                        throw new NotSupportedException(string.Format("Left operand should be property or field", node.NodeType));
                    }

                    if (!(node.Right.NodeType == ExpressionType.Constant))
                    {
                        throw new NotSupportedException(string.Format("Right operand should be constant", node.NodeType));
                    }

                    this.Visit(node.Left);
                    this._resultString.Append("(");
                    this.Visit(node.Right);
                    this._resultString.Append(")");
                    break;

                default:
                    throw new NotSupportedException(string.Format("Operation {0} is not supported", node.NodeType));
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
            this._resultString.Append(node.Value);

            return node;
        }
    }
}
