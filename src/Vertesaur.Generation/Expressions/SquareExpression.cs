using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using Vertesaur.Generation.Contracts;

namespace Vertesaur.Generation.Expressions
{
	/// <summary>
	/// An expression representing the square of another expression. SquareExpression(expression) = expression * expression.
	/// </summary>
	public class SquareExpression : ReducableUnaryExpressionBase
	{

		/// <summary>
		/// Creates a new square expression for the given expression.
		/// </summary>
		/// <param name="unaryParameter">The expression to square.</param>
		/// <param name="reductionExpressionGenerator">The optional expression generator used for reductions.</param>
		public SquareExpression(Expression unaryParameter, IExpressionGenerator reductionExpressionGenerator = null)
			: base(unaryParameter, reductionExpressionGenerator)
		{
			Contract.Requires(null != unaryParameter);
			Contract.EndContractBlock();
		}

		/// <inheritdoc/>
		public override Expression Reduce() {
			if (UnaryParameter is SquareRootExpression)
				return ((SquareRootExpression)UnaryParameter).UnaryParameter;
			if (UnaryParameter is ParameterExpression || UnaryParameter is ConstantExpression)
				return ReductionExpressionGenerator.GenerateExpression("MULTIPLY", UnaryParameter, UnaryParameter);

			return new BlockExpressionBuilder().AddUsingAssignedLocal(local => new[] {
				ReductionExpressionGenerator.GenerateExpression("MULTIPLY", local, local)
			},UnaryParameter).GetExpression();
		}

	}
}
