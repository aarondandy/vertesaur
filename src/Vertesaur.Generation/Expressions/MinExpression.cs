using System;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using Vertesaur.Generation.Utility;
using Vertesaur.Utility;

namespace Vertesaur.Generation.Expressions
{

    /// <summary>
    /// A minimum value expression which determines the minimum of two values.
    /// </summary>
    public class MinExpression : ReducibleBinaryExpressionBase
    {

        /// <summary>
        /// Creates a new minimum value expression.
        /// </summary>
        /// <param name="left">An expression to use.</param>
        /// <param name="right">An expression to use.</param>
        /// <param name="generator">The optional expression generator used during reduction.</param>
        public MinExpression(Expression left, Expression right, IExpressionGenerator generator = null)
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
            var method = typeof(Math).GetPublicStaticInvokableMethod("Min", LeftParameter.Type, RightParameter.Type);
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
            Contract.Requires(null != left);
            Contract.Requires(null != right);
            Contract.Requires(left.IsMemoryLocationOrConstant());
            Contract.Requires(right.IsMemoryLocationOrConstant());
            Contract.Ensures(Contract.Result<Expression>() != null);
            return Condition(
                ReductionExpressionGenerator.GenerateOrThrow("LESSEQUAL", left, right),
                left,
                right
            );
        }
    }
}
