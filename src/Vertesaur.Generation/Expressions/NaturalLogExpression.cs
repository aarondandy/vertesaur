using System;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using System.Reflection;
using Vertesaur.Generation.Utility;
using Vertesaur.Utility;

namespace Vertesaur.Generation.Expressions
{
    /// <summary>
    /// A natural logarithm expression.
    /// </summary>
    public class NaturalLogExpression : ReducibleUnaryExpressionBase
    {
        private static readonly MethodInfo MathNaturalLogMethod;

        static NaturalLogExpression() {
            MathNaturalLogMethod = typeof(Math).GetPublicStaticInvokableMethod("Log", typeof(double));
        }

        /// <summary>
        /// Creates a new natural logarithm expression.
        /// </summary>
        /// <param name="input">The expression to calculate the natural logarithm of.</param>
        /// <param name="generator">The optional expression generator used during reduction.</param>
        public NaturalLogExpression(Expression input, IExpressionGenerator generator = null)
            : base(input, generator) {
            Contract.Requires(null != input);
        }

        /// <inheritdoc/>
        public override Expression Reduce() {
            Contract.Ensures(Contract.Result<Expression>() != null);
            var gen = ReductionExpressionGenerator;
            if (Type == typeof(double) || Type == typeof(float))
                return gen.BuildConversionCall(MathNaturalLogMethod, UnaryParameter, Type);

            return gen.GenerateOrThrow("LOG", UnaryParameter, gen.GenerateOrThrow("E", Type));
        }
    }
}
