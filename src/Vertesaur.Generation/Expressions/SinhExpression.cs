using System;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using System.Reflection;
using Vertesaur.Generation.Contracts;

namespace Vertesaur.Generation.Expressions
{
	/// <summary>
	/// A hyperbolic sine expression.
	/// </summary>
	public class SinhExpression : ReducibleUnaryExpressionBase
	{

		private static readonly MethodInfo MathSinhMethod;

		static SinhExpression() {
			MathSinhMethod = typeof(Math).GetMethod(
				"Sinh",
				BindingFlags.Public | BindingFlags.Static,
				null, new[] { typeof(double) }, null);
		}

		/// <summary>
		/// Creates a new hyperbolic sine expression.
		/// </summary>
		/// <param name="input">The expression to calculate the hyperbolic sine of.</param>
		/// <param name="generator">The optional expression generator used during reduction.</param>
		public SinhExpression(Expression input, IExpressionGenerator generator = null)
			: base(input, generator) { Contract.Requires(null != input); }

		/// <inheritdoc/>
		public override Expression Reduce() {
			return typeof(double) == Type
				? (Expression)Call(MathSinhMethod, UnaryParameter)
				: Convert(Call(MathSinhMethod, Convert(UnaryParameter, typeof(double))), Type);
		}

	}
}
