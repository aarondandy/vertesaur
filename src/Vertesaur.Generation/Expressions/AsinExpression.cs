using System;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using System.Reflection;
using Vertesaur.Generation.Contracts;

namespace Vertesaur.Generation.Expressions
{
	/// <summary>
	/// An arc-sine expression.
	/// </summary>
	public class AsinExpression : ReducibleUnaryExpressionBase
	{
		private static readonly MethodInfo MathAsinMethod;

		static AsinExpression() {
			MathAsinMethod = typeof(Math).GetMethod(
				"Asin",
				BindingFlags.Public | BindingFlags.Static,
				null, new[] { typeof(double) }, null);
		}

		/// <summary>
		/// Creates a new arc-sine expression.
		/// </summary>
		/// <param name="input">The expression to calculate the arc-sine of.</param>
		/// <param name="generator">The optional expression generator used during reduction.</param>
		public AsinExpression(Expression input, IExpressionGenerator generator = null)
			: base(input, generator) { Contract.Requires(null != input); }
		/// <inheritdoc/>
		public override Expression Reduce() {
			return typeof(double) == Type
				? (Expression)Call(MathAsinMethod, UnaryParameter)
				: Convert(Call(MathAsinMethod, Convert(UnaryParameter, typeof(double))), Type);
		}
	}
}
