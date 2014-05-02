using System;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using System.Reflection;
using Vertesaur.Generation.Utility;
using Vertesaur.Utility;

namespace Vertesaur.Generation.Expressions
{
    /// <summary>
    /// A hyperbolic cosine expression.
    /// </summary>
    public class CoshExpression : ReducibleUnaryExpressionBase
    {

        private static readonly MethodInfo MathCoshMethod;

        static CoshExpression() {
            MathCoshMethod = typeof(Math).GetPublicStaticInvokableMethod("Cosh", typeof(double));
        }

        /// <summary>
        /// Creates a new hyperbolic cosine expression.
        /// </summary>
        /// <param name="input">The expression to calculate the hyperbolic cosine of.</param>
        /// <param name="generator">The optional expression generator used during reduction.</param>
        public CoshExpression(Expression input, IExpressionGenerator generator = null)
            : base(input, generator) {
            Contract.Requires(null != input);
        }

        /// <inheritdoc/>
        public override Expression Reduce() {
            Contract.Ensures(Contract.Result<Expression>() != null);
            return ReductionExpressionGenerator.BuildConversionCall(MathCoshMethod, UnaryParameter, Type);
        }

    }
}
