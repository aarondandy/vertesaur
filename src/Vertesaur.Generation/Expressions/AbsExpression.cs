using System;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using Vertesaur.Generation.Utility;
using Vertesaur.Utility;

namespace Vertesaur.Generation.Expressions
{
    /// <summary>
    /// An absolute value expression.
    /// </summary>
    public class AbsExpression : ReducibleUnaryExpressionBase
    {

        /// <summary>
        /// Creates a new absolute value expression.
        /// </summary>
        /// <param name="input">The expression to find the absolute value of.</param>
        /// <param name="generator">The optional expression generator used during reduction.</param>
        public AbsExpression(Expression input, IExpressionGenerator generator = null) : base(input, generator) {
            Contract.Requires(null != input);
        }

        /// <inheritdoc/>
        public override Expression Reduce() {
            Contract.Ensures(Contract.Result<Expression>() != null);
            var method = typeof(Math).GetPublicStaticInvokableMethod("Abs", Type);
            if (null != method)
                return ReductionExpressionGenerator.BuildConversionCall(method, UnaryParameter, Type);

            if (UnaryParameter.IsMemoryLocationOrConstant())
                return GenerateExpression(UnaryParameter);

            return new BlockExpressionBuilder()
                .AddUsingMemoryLocationOrConstant(
                    local => new[] { GenerateExpression(local) }, UnaryParameter)
                .GetExpression();
        }

        private Expression GenerateExpression(Expression parameter) {
            Contract.Requires(parameter != null);
            Contract.Requires(parameter.IsMemoryLocationOrConstant());
            Contract.Ensures(Contract.Result<Expression>() != null);
            var gen = ReductionExpressionGenerator;
            return Condition(
                gen.GenerateOrThrow("GREATEREQUAL", parameter, gen.GenerateOrThrow("ZERO", Type)),
                parameter,
                gen.GenerateOrThrow("NEGATE", parameter)
            );
        }

    }
}
