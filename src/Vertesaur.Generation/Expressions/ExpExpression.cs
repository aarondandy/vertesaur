using System;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using System.Reflection;
using Vertesaur.Generation.Utility;
using Vertesaur.Utility;

namespace Vertesaur.Generation.Expressions
{
    /// <summary>
    /// An expression equivalent to the constant E raised to the Nth power.
    /// </summary>
    public class ExpExpression : ReducibleUnaryExpressionBase
    {
        private static readonly MethodInfo MathExpMethod;

        static ExpExpression() {
            MathExpMethod = typeof(Math).GetPublicStaticInvokableMethod("Exp", typeof(double));
        }

        /// <summary>
        /// Creates a new E^N expression.
        /// </summary>
        /// <param name="input">The expression used to raise E to a power.</param>
        /// <param name="generator">The optional expression generator used during reduction.</param>
        public ExpExpression(Expression input, IExpressionGenerator generator = null)
            : base(input, generator) {
            Contract.Requires(null != input);
        }

        /// <inheritdoc/>
        public override Expression Reduce() {
            Contract.Ensures(Contract.Result<Expression>() != null);

            var gen = ReductionExpressionGenerator;

            
            if (typeof (double) == Type) {
                Contract.Assume(MathExpMethod != null);
                return gen.BuildConversionCall(MathExpMethod, UnaryParameter, Type);
            }

            var result = gen.GenerateOrThrow("POW", gen.GenerateOrThrow("E", Type), UnaryParameter);
            if (result.Type != Type)
                result = gen.GenerateConversionOrThrow(Type, result);

            return result;
        }
    }
}
