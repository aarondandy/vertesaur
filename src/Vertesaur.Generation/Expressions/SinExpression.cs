using System;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using System.Reflection;
using Vertesaur.Generation.Contracts;

namespace Vertesaur.Generation.Expressions
{
	/// <summary>
	/// A sine expression.
	/// </summary>
	public class SinExpression : ReducableUnaryExpressionBase
	{
		private static readonly MethodInfo MathSinMethod;

		static SinExpression() {
			MathSinMethod = typeof(Math).GetMethod(
				"Sin",
				BindingFlags.Public | BindingFlags.Static,
				null, new[] { typeof(double) }, null);
		}

		/// <summary>
		/// Creates a new sine expression.
		/// </summary>
		/// <param name="input">The expression to calculate the sine of.</param>
		/// <param name="generator">The optional expression generator used during reduction.</param>
		public SinExpression(Expression input, IExpressionGenerator generator = null)
			: base(input, generator) { Contract.Requires(null != input); }

		/// <inheritdoc/>
		public override Expression Reduce() {
			return typeof(double) == Type
				? (Expression)Call(MathSinMethod, UnaryParameter)
				: Convert(Call(MathSinMethod, Convert(UnaryParameter, typeof(double))), Type);
		}

	}
}
