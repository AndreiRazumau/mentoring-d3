using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ExpressionsAndIQueryable.Tests
{
    public class UserExpressionVisitor : ExpressionVisitor
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

            if (node.Method.DeclaringType == typeof(string))
            {
                switch (node.Method.Name)
                {
                    case "Contains":
                        this.StringMethodTraversal(node, "*", "*");
                        break;
                    case "StartsWith":
                        this.StringMethodTraversal(node, null, "*");
                        break;
                    case "EndsWith":
                        this.StringMethodTraversal(node, "*", null);

                        break;
                    default:
                        throw new NotSupportedException($"Method {node.Method.Name} is not supported");
                }

                return node;
            }

            throw new NotSupportedException($"Method of type {node.Method.DeclaringType} is not supported");
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            switch (node.NodeType)
            {
                case ExpressionType.Equal:
                    switch (node.Left.NodeType)
                    {
                        case ExpressionType.MemberAccess:
                            this.BinaryNodeTraversal(node.Left, node.Right);
                            break;
                        case ExpressionType.Constant:
                            this.BinaryNodeTraversal(node.Right, node.Left);
                            break;
                    }
                    return node;
                case ExpressionType.AndAlso:
                    this.AndNodeTraversal(node.Right, node.Left);
                    return node;
                default:
                    throw new NotSupportedException($"Operation {node.NodeType} is not supported");
            }
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            this._resultString.Append(node.Member.Name).Append(":");
            return node;
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            this._resultString.Append(node.Value);
            return node;
        }

        #endregion

        #region [Private methods]

        private void AndNodeTraversal(Expression left,
                                      Expression right)
        {
            this.AddOpenBracket();
            this.Visit(left);
            this.AddCloseBracket();
            this.AddAndAlsoOperator();
            this.AddOpenBracket();
            this.Visit(right);
            this.AddCloseBracket();
        }

        private void BinaryNodeTraversal(Expression left,
                                         Expression right)
        {
            this.Visit(left);
            this.AddOpenBracket();
            this.Visit(right);
            this.AddCloseBracket();
        }

        private void StringMethodTraversal(MethodCallExpression expression,
                                           string beforeValueCharacter,
                                           string afterValueCharacter)
        {
            this.Visit(expression.Object);
            this.AddOpenBracket(beforeValueCharacter);
            this.Visit(expression.Arguments[0]);
            this.AddCloseBracket(afterValueCharacter);
        }

        private void AddAndAlsoOperator()
        {
            this._resultString.Append("&&");
        }

        private void AddOpenBracket(string additionalCharacters = null)
        {
            this._resultString.Append("(");
            if (!string.IsNullOrEmpty(additionalCharacters))
            {
                this._resultString.Append(additionalCharacters);
            }
        }

        private void AddCloseBracket(string additionalCharacters = null)
        {
            if (!string.IsNullOrEmpty(additionalCharacters))
            {
                this._resultString.Append(additionalCharacters);
            }

            this._resultString.Append(")");
        }

        #endregion
    }
}
