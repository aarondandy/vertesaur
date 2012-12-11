using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using Vertesaur.Generation.Contracts;

namespace Vertesaur.Generation.ExpressionBuilder
{
	/// <summary>
	/// An expression representing the square of another expression. SquareExpression(expression) = expression * expression.
	/// </summary>
	public class SquareExpression : ReducableUnaryExpressionBase
	{

		/// <summary>
		/// Creates a new square expression for the given expression.
		/// </summary>
		/// <param name="parameter">The expression to square.</param>
		/// <param name="reductionExpressionGenerator">The optional expression generator used for reductions.</param>
		public SquareExpression(Expression parameter, IExpressionGenerator reductionExpressionGenerator = null)
			: base(parameter, reductionExpressionGenerator)
		{
			Contract.Requires(null != parameter);
			Contract.EndContractBlock();
		}

		public override Expression Reduce() {
			if (UnaryParameter is SquareRootExpression)
				return ((SquareRootExpression)UnaryParameter).UnaryParameter;
			if (UnaryParameter is ParameterExpression || UnaryParameter is ConstantExpression){
				return ReductionExpressionGenerator.GenerateExpression(
					"MULTIPLY", UnaryParameter, UnaryParameter);
			}
			var tempLocal = Parameter(Type);
			return Block(
				new[] {tempLocal},
				Assign(tempLocal, UnaryParameter),
				ReductionExpressionGenerator.GenerateExpression(
					"MULTIPLY", tempLocal, tempLocal)
			);
		}

	}
}
