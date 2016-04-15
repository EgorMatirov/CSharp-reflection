using System;
using System.Linq.Expressions;

namespace Reflection_Algebra
{
    public static class DerivativeCalculator
    {
        private static Expression GetDerivativeInternal(Expression expression)
        {
            switch (expression.NodeType)
            {
                case ExpressionType.Lambda:
                {
                    var lambda = (LambdaExpression) expression;
                    return GetDerivativeInternal(lambda.Body);
                }
                case ExpressionType.Add:
                {
                    var binaryExpression = (BinaryExpression)expression;
                    var leftDerivative = GetDerivativeInternal(binaryExpression.Left);
                    var rightDerivative = GetDerivativeInternal(binaryExpression.Right);
                    return Expression.Add(leftDerivative, rightDerivative);
                }
                case ExpressionType.Multiply:
                {
                    var binaryExpression = (BinaryExpression)expression;
                    var left = binaryExpression.Left;
                    var right = binaryExpression.Right;
                    var leftDerivative = GetDerivativeInternal(binaryExpression.Left);
                    var rightDerivative = GetDerivativeInternal(binaryExpression.Right);
                    var firstMultiplication = Expression.Multiply(leftDerivative, right);
                    var secondMultiplication = Expression.Multiply(left, rightDerivative);
                    return Expression.Add(firstMultiplication, secondMultiplication);
                }
                case ExpressionType.Call:
                {
                    var callExpression = (MethodCallExpression)expression;
                    var singleDoubleParameter = new[] {typeof (double)};
                    if (callExpression.Method == typeof(Math).GetMethod("Sin", singleDoubleParameter))
                    {
                        var argumentDerivative = GetDerivativeInternal(callExpression.Arguments[0]);
                        var cosCall = Expression.Call(typeof (Math).GetMethod("Cos", singleDoubleParameter),
                            callExpression.Arguments[0]);
                        return Expression.Multiply(argumentDerivative, cosCall);
                    }
                    break;
                }
                case ExpressionType.Constant:
                    return Expression.Constant(0.0);
                case ExpressionType.Parameter:
                    return Expression.Constant(1.0);
            }
            throw new NotImplementedException($"Cannot find derivative of {expression.NodeType}");
        }

        public static Func<double, double> GetDerivative(Expression<Func<double, double>> f)
        {
            var result = Expression.Lambda<Func<double, double>>(GetDerivativeInternal(f), f.Parameters);
            return result.Compile();
        } 
    }
}
