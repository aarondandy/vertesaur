using System.Diagnostics.Contracts;
using System.Linq.Expressions;

namespace Vertesaur.Generation.Expressions
{

    /// <summary>
    /// An inverse hyperbolic tangent expression.
    /// </summary>
    public class AtanhExpression : ReducibleUnaryExpressionBase
    {

        /// <summary>
        /// Creates a new inverse hyperbolic tangent expression.
        /// </summary>
        /// <param name="input">The expression to calculate the inverse hyperbolic tangent of.</param>
        /// <param name="generator">The optional expression generator used during reduction.</param>
        public AtanhExpression(Expression input, IExpressionGenerator generator = null)
            : base(input, generator) {
            Contract.Requires(null != input);
        }

        /// <inheritdoc/>
        public override Expression Reduce() {
            Contract.Ensures(Contract.Result<Expression>() != null);
            if (UnaryParameter.IsMemoryLocationOrConstant())
                return CreateExpression(UnaryParameter);

            return new BlockExpressionBuilder().AddUsingMemoryLocationOrConstant(
                x => new[] { CreateExpression(x) },
                UnaryParameter
            ).GetExpression();
        }

        private Expression CreateExpression(Expression input) {
            Contract.Requires(null != input);
            Contract.Requires(input.IsMemoryLocationOrConstant());
            Contract.Ensures(Contract.Result<Expression>() != null);
            var gen = ReductionExpressionGenerator;
            return gen.Generate("DIVIDE",
                gen.Generate("LOG",
                    gen.Generate("DIVIDE",
                        gen.Generate("ADD", gen.Generate("1", input.Type), input),
                        gen.Generate("SUBTRACT", gen.Generate("1", input.Type), input)
                    )
                ),
                gen.Generate("2", input.Type)
            );
        }

    }
}
