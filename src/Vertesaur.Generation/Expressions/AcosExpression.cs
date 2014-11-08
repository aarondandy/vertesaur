using System;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using System.Reflection;
using Vertesaur.Generation.Utility;
using Vertesaur.Utility;

namespace Vertesaur.Generation.Expressions
{
    /// <summary>
    /// An arc-cosine expression.
    /// </summary>
    public class AcosExpression : ReducibleUnaryExpressionBase
    {
        private static readonly MethodInfo MathAcosMethod;

        static AcosExpression() {
            MathAcosMethod = typeof(Math).GetPublicStaticInvokableMethod("Acos", typeof(double));
        }

        /// <summary>
        /// Creates a new arc-cosine expression.
        /// </summary>
        /// <param name="input">The expression to calculate the arc-cosine of.</param>
        /// <param name="generator">The optional expression generator used during reduction.</param>
        public AcosExpression(Expression input, IExpressionGenerator generator = null)
            : base(input, generator) {
            Contract.Requires(null != input);
        }

        /// <inheritdoc/>
        public override Expression Reduce() {
            Contract.Ensures(Contract.Result<Expression>() != null);
            Contract.Assume(MathAcosMethod != null);
            return ReductionExpressionGenerator.BuildConversionCall(MathAcosMethod, UnaryParameter, Type);
        }
    }
}
