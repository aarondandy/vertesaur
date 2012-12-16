﻿using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using Vertesaur.Generation.Contracts;

namespace Vertesaur.Generation.Expressions
{
	/// <summary>
	/// An expression representing the square of another expression. SquareExpression(expression) = expression * expression.
	/// </summary>
	public class SquareExpression : ReducibleUnaryExpressionBase
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
			var squareRootExpression = UnaryParameter as SquareRootExpression;
			if (squareRootExpression != null)
				return squareRootExpression.UnaryParameter;
			if (UnaryParameter.IsMemoryLocationOrConstant())
				return ReductionExpressionGenerator.Generate("MULTIPLY", UnaryParameter, UnaryParameter);

			return new BlockExpressionBuilder().AddUsingMemoryLocationOrConstant(local => new[] {
				ReductionExpressionGenerator.Generate("MULTIPLY", local, local)
			},UnaryParameter).GetExpression();
		}

	}
}
