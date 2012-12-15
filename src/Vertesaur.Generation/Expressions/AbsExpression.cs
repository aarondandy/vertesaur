using System;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using System.Reflection;
using Vertesaur.Generation.Contracts;

namespace Vertesaur.Generation.Expressions
{
	/// <summary>
	/// An absolute value expression.
	/// </summary>
	public class AbsExpression : ReducableUnaryExpressionBase
	{

		/// <summary>
		/// Creates a new absolute value expression.
		/// </summary>
		/// <param name="input">The expression to find the absolute value of.</param>
		/// <param name="generator">The optional expression generator used during reduction.</param>
		public AbsExpression(Expression input, IExpressionGenerator generator = null)
			: base(input, generator){
			Contract.Requires(null != input);
		}

		/// <inheritdoc/>
		public override Expression Reduce() {
			var method = typeof(Math).GetMethod(
				"Abs",
				BindingFlags.Public | BindingFlags.Static,
				null, new[] { Type }, null);
			if (null != method)
				return Call(method, UnaryParameter);

			if (UnaryParameter.IsMemoryLocationOrConstant())
				return GenerateExpression(UnaryParameter);

			return new BlockExpressionBuilder()
				.AddUsingMemoryLocationOrConstant(
					local => new[]{GenerateExpression(local)},UnaryParameter)
				.GetExpression();
		}

		private Expression GenerateExpression(Expression parameter) {
			Contract.Assume(null != parameter);
			Contract.Assume(parameter.IsMemoryLocationOrConstant());
			return Expression.Condition(
				ReductionExpressionGenerator.Generate("GREATEREQUAL", parameter, ReductionExpressionGenerator.Generate("ZERO", Type)),
				parameter,
				ReductionExpressionGenerator.Generate("NEGATE", parameter)
			);
		}

	}
}
