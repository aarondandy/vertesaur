using System;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using System.Reflection;
using Vertesaur.Generation.Utility;
using Vertesaur.Utility;

namespace Vertesaur.Generation.Expressions
{
    /// <summary>
    /// A ceiling expression.
    /// </summary>
    public class CeilingExpression : ReducibleUnaryExpressionBase
    {
        private static readonly MethodInfo MathCeilingDoubleMethod;
        private static readonly MethodInfo MathCeilingDecimalMethod;

        static CeilingExpression() {
            MathCeilingDoubleMethod = typeof(Math).GetPublicStaticInvokableMethod("Ceiling", typeof(double));
            MathCeilingDecimalMethod = typeof(Math).GetPublicStaticInvokableMethod("Ceiling", typeof(decimal));
        }

        /// <summary>
        /// Creates a new ceiling expression.
        /// </summary>
        /// <param name="input">The expression to calculate the ceiling of.</param>
        /// <param name="generator">The optional expression generator used during reduction.</param>
        public CeilingExpression(Expression input, IExpressionGenerator generator = null)
            : base(input, generator) {
            Contract.Requires(null != input);
        }

        /// <inheritdoc/>
        public override Expression Reduce() {
            Contract.Ensures(Contract.Result<Expression>() != null);
            var method = typeof(decimal) == Type
                ? MathCeilingDecimalMethod
                : MathCeilingDoubleMethod;
            return ReductionExpressionGenerator.BuildConversionCall(method, UnaryParameter, Type);
        }
    }
}
