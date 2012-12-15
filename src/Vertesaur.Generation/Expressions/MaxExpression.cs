using System;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using System.Reflection;
using Vertesaur.Generation.Contracts;

namespace Vertesaur.Generation.Expressions
{
	/// <summary>
	/// A maximum value expression which determines the maximum value of two expressions.
	/// </summary>
	public class MaxExpression : ReducableBinaryExpressionBase
	{

		/// <summary>
		/// Creates a new maximum value expression.
		/// </summary>
		/// <param name="left">An expression to use.</param>
		/// <param name="right">An expression to use.</param>
		/// <param name="generator">/// <inheritdoc/></param>
		public MaxExpression(Expression left, Expression right, IExpressionGenerator generator = null)
			: base(left, right, generator) { Contract.Requires(null != left); Contract.Requires(null != right); }

		/// <inheritdoc/>
		public override Type Type {
			get { return  LeftParameter.Type; }
		}

		/// <inheritdoc/>
		public override Expression Reduce() {
			var method = typeof(Math).GetMethod(
				"Max",
				BindingFlags.Public | BindingFlags.Static,
				null, new[] { LeftParameter.Type,RightParameter.Type }, null);
			if (null != method)
				return Call(method, LeftParameter, RightParameter);

			if (LeftParameter.IsMemoryLocationOrConstant() && RightParameter.IsMemoryLocationOrConstant())
				return GenerateExpression(LeftParameter, RightParameter);

			return new BlockExpressionBuilder()
				.AddUsingMemoryLocationsOrConstants(
					args => new[] { GenerateExpression(args[0], args[1]) },
					LeftParameter, RightParameter
				)
				.GetExpression();
		}

		private Expression GenerateExpression(Expression left, Expression right) {
			Contract.Assume(null != left);
			Contract.Assume(null != right);
			Contract.Assume(left.IsMemoryLocationOrConstant());
			Contract.Assume(right.IsMemoryLocationOrConstant());
			return Expression.Condition(
				ReductionExpressionGenerator.Generate("GREATEREQUAL", left, right),
				left,
				right
			);
		}

	}
}
