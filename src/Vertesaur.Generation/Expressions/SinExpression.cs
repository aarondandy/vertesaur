using System;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using System.Reflection;
using Vertesaur.Generation.Contracts;
using Vertesaur.Utility;

namespace Vertesaur.Generation.Expressions
{
    /// <summary>
    /// A sine expression.
    /// </summary>
    public class SinExpression : ReducibleUnaryExpressionBase
    {
        private static readonly MethodInfo MathSinMethod;

        static SinExpression() {
            MathSinMethod = typeof(Math).GetPublicStaticInvokableMethod("Sin", typeof(double));
        }

        /// <summary>
        /// Creates a new sine expression.
        /// </summary>
        /// <param name="input">The expression to calculate the sine of.</param>
        /// <param name="generator">The optional expression generator used during reduction.</param>
        public SinExpression(Expression input, IExpressionGenerator generator = null)
            : base(input, generator) {
            Contract.Requires(null != input);
        }

        /// <inheritdoc/>
        public override Expression Reduce() {
            Contract.Ensures(Contract.Result<Expression>() != null);
            return typeof(double) == Type
                ? (Expression)Call(MathSinMethod, UnaryParameter)
                : Convert(Call(MathSinMethod, Convert(UnaryParameter, typeof(double))), Type);
        }

    }
}
