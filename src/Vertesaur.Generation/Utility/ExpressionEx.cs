using System;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using System.Reflection;
using Vertesaur.Generation.Expressions;

namespace Vertesaur.Generation.Utility
{
    internal static class ExpressionEx
    {

        public static MethodCallExpression BuildCallExpression(this MethodInfo method, Expression arg0) {
            Contract.Requires(method != null);
            Contract.Requires(arg0 != null);
            Contract.Ensures(Contract.Result<MethodCallExpression>() != null);
            var result = Expression.Call(method, arg0);
            Contract.Assume(result != null);
            return result;
        }

        public static MethodCallExpression BuildCallExpression(this MethodInfo method, Expression arg0, Expression arg1) {
            Contract.Requires(method != null);
            Contract.Requires(arg0 != null);
            Contract.Requires(arg1 != null);
            Contract.Ensures(Contract.Result<MethodCallExpression>() != null);
            var result = Expression.Call(method, arg0, arg1);
            Contract.Assume(result != null);
            return result;
        }

        public static MethodCallExpression BuildCallExpression(this MethodInfo method, params Expression[] args) {
            Contract.Requires(method != null);
            Contract.Requires(args != null);
            Contract.Requires(Contract.ForAll(args, x => x != null));
            Contract.Ensures(Contract.Result<MethodCallExpression>() != null);
            var result = Expression.Call(method, args);
            Contract.Assume(result != null);
            return result;
        }

        [Obsolete]
        public static UnaryExpression BuildConversion(this Expression expression, Type type) {
            Contract.Requires(expression != null);
            Contract.Requires(type != null);
            Contract.Ensures(Contract.Result<UnaryExpression>() != null);
            var result = Expression.Convert(expression, type);
            Contract.Assume(result != null);
            return result;
        }

        public static Expression BuildConversionCall(this IExpressionGenerator gen, MethodInfo method, Expression input, Type resultType) {
            Contract.Requires(gen != null);
            Contract.Requires(method != null);
            Contract.Requires(input != null);

            var methodParams = method.GetParameters();
            if (methodParams.Length > 0) {
                Contract.Assume(methodParams[0] != null);
                var paramType = methodParams[0].ParameterType;
                if (input.Type != paramType) {
                    input = gen.GenerateConversionOrThrow(paramType, input);
                }
            }

            Expression result = method.BuildCallExpression(input);
            if (resultType != null && result.Type != resultType) {
                result = gen.GenerateConversionOrThrow(resultType, result);
            }

            return result;
        }

        public static Expression BuildConversionCall(this IExpressionGenerator gen, MethodInfo method, Type resultType, params Expression[] inputExpressions) {
            Contract.Requires(gen != null);
            Contract.Requires(method != null);
            Contract.Requires(inputExpressions != null);
            Contract.Requires(Contract.ForAll(inputExpressions, x => x != null));

            var methodParams = method.GetParameters();
            if(methodParams.Length != inputExpressions.Length)
                throw new ArgumentException(String.Format("{0} input expressions are required.", methodParams.Length));

            for (int i = 0; i < methodParams.Length; i++) {
                Contract.Assume(methodParams[i] != null);
                var paramType = methodParams[i].ParameterType;
                var inputExpression = inputExpressions[i];
                if (inputExpression.Type != paramType) {
                    inputExpressions[i] = gen.GenerateConversionOrThrow(paramType, inputExpression);
                }
            }

            Expression result = method.BuildCallExpression(inputExpressions);
            if (resultType != null && result.Type != resultType) {
                result = gen.GenerateConversionOrThrow(resultType, result);
            }

            return result;
        }



    }
}
