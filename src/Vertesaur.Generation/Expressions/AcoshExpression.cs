using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using Vertesaur.Generation.Contracts;

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
			: base(input, generator) { Contract.Requires(null != input); }

		/// <inheritdoc/>
		public override Expression Reduce() {
			if (UnaryParameter.IsMemoryLocationOrConstant())
				return CreateExpression(UnaryParameter);

			return new BlockExpressionBuilder().AddUsingMemoryLocationOrConstant(
				x => new[] { CreateExpression(x) },
				UnaryParameter
			).GetExpression();
		}

		private Expression CreateExpression(Expression input){
			Contract.Assume(null != input);
			Contract.Assume(input.IsMemoryLocationOrConstant());
			var gen = ReductionExpressionGenerator;
			return gen.Generate("LOG",
				gen.Generate("ADD",
					gen.Generate("SQUAREROOT",
						gen.Generate("SUBTRACT",
							gen.Generate("SQUARE", input),
							gen.Generate("1", input.Type)
						)
					),
					input
				)
			);
		}

	}
}
