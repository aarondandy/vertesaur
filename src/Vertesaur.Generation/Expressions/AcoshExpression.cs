using System.Diagnostics.Contracts;
using System.Linq.Expressions;

namespace Vertesaur.Generation.Expressions
{

    /// <summary>
    /// An inverse hyperbolic cosine expression.
    /// </summary>
    public class AcoshExpression : ReducibleUnaryExpressionBase
    {

        /// <summary>
        /// Creates a new inverse hyperbolic cosine expression.
        /// </summary>
        /// <param name="input">The expression to calculate the inverse hyperbolic cosine of.</param>
        /// <param name="generator">The optional expression generator used during reduction.</param>
        public AcoshExpression(Expression input, IExpressionGenerator generator = null)
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
            Contract.Requires(input != null);
            Contract.Requires(input.IsMemoryLocationOrConstant());
            Contract.Ensures(Contract.Result<Expression>() != null);
            var gen = ReductionExpressionGenerator;
            return gen.GenerateOrThrow("LOG",
                gen.GenerateOrThrow("ADD",
                    gen.GenerateOrThrow("SQUAREROOT",
                        gen.GenerateOrThrow("SUBTRACT",
                            gen.GenerateOrThrow("SQUARE", input),
                            gen.GenerateOrThrow("1", input.Type)
                        )
                    ),
                    input
                )
            );
        }

    }
}
