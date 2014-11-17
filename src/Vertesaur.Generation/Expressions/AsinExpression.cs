using System;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using System.Reflection;
using Vertesaur.Generation.Utility;
using Vertesaur.Utility;

namespace Vertesaur.Generation.Expressions
{
    /// <summary>
    /// An arc-sine expression.
    /// </summary>
    public class AsinExpression : ReducibleUnaryExpressionBase
    {
        private static readonly MethodInfo MathAsinMethod;

        static AsinExpression() {
            MathAsinMethod = typeof(Math).GetPublicStaticInvokableMethod("Asin", typeof(double));
        }

        /// <summary>
        /// Creates a new arc-sine expression.
        /// </summary>
        /// <param name="input">The expression to calculate the arc-sine of.</param>
        /// <param name="generator">The optional expression generator used during reduction.</param>
        public AsinExpression(Expression input, IExpressionGenerator generator = null)
            : base(input, generator) {
            Contract.Requires(null != input);
        }

        /// <inheritdoc/>
        public override Expression Reduce() {
            Contract.Ensures(Contract.Result<Expression>() != null);
            return ReductionExpressionGenerator.BuildConversionCall(MathAsinMethod, UnaryParameter, Type);
        }
    }
}
