using System;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using System.Reflection;
using Vertesaur.Generation.Contracts;
using Vertesaur.Utility;

namespace Vertesaur.Generation.Expressions
{
    /// <summary>
    /// A hyperbolic tangent expression.
    /// </summary>
    public class TanhExpression : ReducibleUnaryExpressionBase
    {

        private static readonly MethodInfo MathTanhMethod;

        static TanhExpression() {
            MathTanhMethod = typeof(Math).GetPublicStaticInvokableMethod("Tanh", typeof(double));
        }

        /// <summary>
        /// Creates a new hyperbolic tangent expression.
        /// </summary>
        /// <param name="input">The expression to find the hyperbolic tangent of.</param>
        /// <param name="generator">The optional expression generator used during reduction.</param>
        public TanhExpression(Expression input, IExpressionGenerator generator = null)
            : base(input, generator) {
            Contract.Requires(null != input);
        }

        /// <inheritdoc/>
        public override Expression Reduce() {
            Contract.Ensures(Contract.Result<Expression>() != null);
            return typeof(double) == Type
                ? (Expression)Call(MathTanhMethod, UnaryParameter)
                : Convert(Call(MathTanhMethod, Convert(UnaryParameter, typeof(double))), Type);
        }

    }
}
