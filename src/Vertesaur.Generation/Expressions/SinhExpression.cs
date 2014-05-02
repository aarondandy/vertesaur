using System;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using System.Reflection;
using Vertesaur.Generation.Utility;
using Vertesaur.Utility;

namespace Vertesaur.Generation.Expressions
{
    /// <summary>
    /// A hyperbolic sine expression.
    /// </summary>
    public class SinhExpression : ReducibleUnaryExpressionBase
    {

        private static readonly MethodInfo MathSinhMethod;

        static SinhExpression() {
            MathSinhMethod = typeof(Math).GetPublicStaticInvokableMethod("Sinh", typeof(double));
        }

        /// <summary>
        /// Creates a new hyperbolic sine expression.
        /// </summary>
        /// <param name="input">The expression to calculate the hyperbolic sine of.</param>
        /// <param name="generator">The optional expression generator used during reduction.</param>
        public SinhExpression(Expression input, IExpressionGenerator generator = null)
            : base(input, generator) {
            Contract.Requires(null != input);
        }

        /// <inheritdoc/>
        public override Expression Reduce() {
            Contract.Ensures(Contract.Result<Expression>() != null);
            return ReductionExpressionGenerator.BuildConversionCall(MathSinhMethod, UnaryParameter, Type);
        }

    }
}
