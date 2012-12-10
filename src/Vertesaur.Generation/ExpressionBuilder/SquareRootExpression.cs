using System;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using System.Reflection;
using Vertesaur.Generation.Contracts;

namespace Vertesaur.Generation.ExpressionBuilder
{
	/// <summary>
	/// An expression representing the square-root of another expression. SquareRootExpression(expression) = (expression)^(1/2).
	/// </summary>
	public class SquareRootExpression : ReducableUnaryExpressionBase
	{

		private static readonly MethodInfo MathSqrtMethod;

		static SquareRootExpression() {
			MathSqrtMethod = typeof(Math).GetMethod(
				"Sqrt",
				BindingFlags.Public | BindingFlags.Static,
				null,
				new[] { typeof(double) },
				null);
		}

		public SquareRootExpression(Expression parameter, IExpressionGenerator reductionExpressionGenerator = null)
			: base(parameter, reductionExpressionGenerator)
		{
			Contract.Requires(null != parameter);
			Contract.EndContractBlock();
		}

		public override Expression Reduce() {
			if (UnaryParameter is SquareExpression)
				return ((SquareExpression)UnaryParameter).UnaryParameter;
			return typeof(double) == Type
				? (Expression)Call(MathSqrtMethod, UnaryParameter)
				: Convert(Call(MathSqrtMethod, Convert(UnaryParameter, typeof(double))), Type);
		}

	}
}
