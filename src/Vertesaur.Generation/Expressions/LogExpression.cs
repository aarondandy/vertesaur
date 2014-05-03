using System;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using System.Reflection;
using Vertesaur.Generation.Utility;

namespace Vertesaur.Generation.Expressions
{
    /// <summary>
    /// An arbitrary base logarithm expression.
    /// </summary>
    public class LogExpression : ReducibleBinaryExpressionBase
    {

        private static readonly MethodInfo MathLogMethod;

        static LogExpression() {
            MathLogMethod = typeof (Math).GetPublicStaticInvokableMethod("Log", typeof (double), typeof (double));
        }

        /// <summary>
        /// Creates a new logarithm of the given <paramref name="b">base</paramref>.
        /// </summary>
        /// <param name="value">The value expression to calculate the logarithm of.</param>
        /// <param name="b">The base expression.</param>
        /// <param name="generator">The optional expression generator used during reduction.</param>
        public LogExpression(Expression value, Expression b, IExpressionGenerator generator = null)
            : base(value, b, generator) {
            Contract.Requires(value != null);
            Contract.Requires(b != null);
        }

        /// <inheritdoc/>
        public override Type Type { get { return LeftParameter.Type; } }

        /// <inheritdoc/>
        public override Expression Reduce() {
            Contract.Ensures(Contract.Result<Expression>() != null);
            return ReductionExpressionGenerator.BuildConversionCall(
                MathLogMethod,
                Type,
                LeftParameter,
                RightParameter);
        }
    }
}
