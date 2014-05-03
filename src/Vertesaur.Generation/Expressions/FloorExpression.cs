using System;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using System.Reflection;
using Vertesaur.Generation.Utility;

namespace Vertesaur.Generation.Expressions
{

    /// <summary>
    /// A floor expression.
    /// </summary>
    public class FloorExpression : ReducibleUnaryExpressionBase
    {

        private static readonly MethodInfo MathFloorDoubleMethod;
        private static readonly MethodInfo MathFloorDecimalMethod;

        static FloorExpression() {
            MathFloorDoubleMethod = typeof(Math).GetPublicStaticInvokableMethod("Floor", typeof(double));
            MathFloorDecimalMethod = typeof(Math).GetPublicStaticInvokableMethod("Floor", typeof(decimal));
        }

        /// <summary>
        /// Creates a new floor expression.
        /// </summary>
        /// <param name="input">The expression to calculate the floor of.</param>
        /// <param name="generator">The optional expression generator used during reduction.</param>
        public FloorExpression(Expression input, IExpressionGenerator generator = null)
            : base(input, generator) {
            Contract.Requires(null != input);
        }

        /// <inheritdoc/>
        public override Expression Reduce() {
            Contract.Ensures(Contract.Result<Expression>() != null);
            var method = typeof(decimal) == Type
                ? MathFloorDecimalMethod
                : MathFloorDoubleMethod;
            return ReductionExpressionGenerator.BuildConversionCall(method, UnaryParameter, Type);
        }

    }
}
