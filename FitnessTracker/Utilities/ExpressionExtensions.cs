using System.Linq.Expressions;

namespace FitnessTracker.Utilities
{
    public static class ExpressionExtensions
    {
        public static Expression<Func<T, bool>>? Combine<T>(this IEnumerable<Expression<Func<T, bool>>> expressions)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T), "x");
            Expression? body = null;

            foreach (Expression<Func<T, bool>> expression in expressions)
            {
                Expression replacedBody = new ReplaceParameterVisitor(expression.Parameters[0], parameter).Visit(expression.Body);

                body = body == null
                    ? replacedBody
                    : Expression.OrElse(body, replacedBody);
            }

            return body is null ? null : Expression.Lambda<Func<T, bool>>(body, parameter);
        }

        private class ReplaceParameterVisitor(ParameterExpression oldParameter, ParameterExpression newParameter) : ExpressionVisitor
        {
            private readonly ParameterExpression oldParameter = oldParameter;
            private readonly ParameterExpression newParameter = newParameter;

            protected override Expression VisitParameter(ParameterExpression node) => node == oldParameter ? newParameter : base.VisitParameter(node);
        }
    }
}
