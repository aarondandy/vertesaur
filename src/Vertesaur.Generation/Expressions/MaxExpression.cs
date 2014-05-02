using System;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using Vertesaur.Generation.Utility;
using Vertesaur.Utility;

namespace Vertesaur.Generation.Expressions
{
    /// <summary>
    /// A maximum value expression which determines the maximum value of two expressions.
    /// </summary>
    public class MaxExpression : ReducibleBinaryExpressionBase
    {

        /// <summary>
        /// Creates a new maximum value expression.
        /// </summary>
        /// <param name="left">An expression to use.</param>
        /// <param name="right">An expression to use.</param>
        /// <param name="generator">/// <inheritdoc/></param>
        public MaxExpression(Expression left, Expression right, IExpressionGenerator generator = null)
            : base(left, right, generator) {
            Contract.Requires(null != left);
            Contract.Requires(null != right);
        }

        /// <inheritdoc/>
        public override Type Type {
            get {
                Contract.Ensures(Contract.Result<Type>() != null);
                return LeftParameter.Type;
            }
        }

        /// <inheritdoc/>
        public override Expression Reduce() {
            Contract.Ensures(Contract.Result<Expression>() != null);
            var method = typeof(Math).GetPublicStaticInvokableMethod("Max", LeftParameter.Type, RightParameter.Type);
            if (null != method)
                return method.BuildCallExpression(LeftParameter, RightParameter);

            if (LeftParameter.IsMemoryLocationOrConstant() && RightParameter.IsMemoryLocationOrConstant())
                return GenerateExpression(LeftParameter, RightParameter);

            return new BlockExpressionBuilder()
                .AddUsingMemoryLocationsOrConstants(
                    args => new[] { GenerateExpression(args[0], args[1]) },
                    LeftParameter, RightParameter
                )
                .GetExpression();
        }

        private Expression GenerateExpression(Expression left, Expression right) {
            Contract.Requires(left != null);
            Contract.Requires(right != null);
            Contract.Requires(left.IsMemoryLocationOrConstant());
            Contract.Requires(right.IsMemoryLocationOrConstant());
            Contract.Ensures(Contract.Result<Expression>() != null);
            var result = Condition(
                ReductionExpressionGenerator.GenerateOrThrow("GREATEREQUAL", left, right),
                left,
                right
            );
            Contract.Assume(result != null);
            return result;
        }

    }
}
