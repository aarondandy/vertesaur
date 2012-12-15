using System;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using System.Reflection;
using Vertesaur.Generation.Contracts;

namespace Vertesaur.Generation.Expressions
{
	/// <summary>
	/// A tangent expression.
	/// </summary>
	public class TanExpression : ReducableUnaryExpressionBase
	{
		private static readonly MethodInfo MathTanMethod;

		static TanExpression() {
			MathTanMethod = typeof(Math).GetMethod(
				"Tan",
				BindingFlags.Public | BindingFlags.Static,
				null, new[] { typeof(double) }, null);
		}

		/// <summary>
		/// Creates a new tangent expression.
		/// </summary>
		/// <param name="input">The expression to find the tangent of.</param>
		/// <param name="generator">The optional generator used during reduction.</param>
		public TanExpression(Expression input, IExpressionGenerator generator = null)
			: base(input, generator) { Contract.Requires(null != input); }

		/// <inheritdoc/>
		public override Expression Reduce() {
			return typeof(double) == Type
				? (Expression)Call(MathTanMethod, UnaryParameter)
				: Convert(Call(MathTanMethod, Convert(UnaryParameter, typeof(double))), Type);
		}
	}
}
