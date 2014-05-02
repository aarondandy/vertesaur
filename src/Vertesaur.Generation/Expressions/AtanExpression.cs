using System;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using System.Reflection;
using Vertesaur.Generation.Utility;
using Vertesaur.Utility;

namespace Vertesaur.Generation.Expressions
{
    /// <summary>
    /// An arc-tangent expression.
    /// </summary>
    public class AtanExpression : ReducibleUnaryExpressionBase
    {
        private static readonly MethodInfo MathAtanMethod;

        static AtanExpression() {
            MathAtanMethod = typeof(Math).GetPublicStaticInvokableMethod("Atan", typeof(double));
        }

        /// <summary>
        /// Creates a new arc-tangent expression.
        /// </summary>
        /// <param name="input">The expression to calculate the arc-tangent of.</param>
        /// <param name="generator">The optional expression generator used during reduction.</param>
        public AtanExpression(Expression input, IExpressionGenerator generator = null)
            : base(input, generator) {
            Contract.Requires(null != input);
        }

        /// <inheritdoc/>
        public override Expression Reduce() {
            Contract.Ensures(Contract.Result<Expression>() != null);
            return ReductionExpressionGenerator.BuildConversionCall(MathAtanMethod, UnaryParameter, Type);
        }
    }
}
