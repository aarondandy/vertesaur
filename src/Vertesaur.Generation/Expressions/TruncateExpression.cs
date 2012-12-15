using System;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using System.Reflection;
using Vertesaur.Generation.Contracts;

namespace Vertesaur.Generation.Expressions
{
	/// <summary>
	/// A truncate expression which removes any decimals from a number, leaving only the integer part.
	/// </summary>
	public class TruncateExpression : ReducableUnaryExpressionBase
	{
		private static readonly MethodInfo MathCeilingDoubleMethod;
		private static readonly MethodInfo MathCeilingDecimalMethod;

		static TruncateExpression() {
			MathCeilingDoubleMethod = typeof(Math).GetMethod(
				"Truncate",
				BindingFlags.Public | BindingFlags.Static,
				null, new[] { typeof(double) }, null);
			MathCeilingDecimalMethod = typeof(Math).GetMethod(
				"Truncate",
				BindingFlags.Public | BindingFlags.Static,
				null, new[] { typeof(decimal) }, null);
		}

		/// <summary>
		/// Creates a new truncate expression.
		/// </summary>
		/// <param name="input">The expression to truncate.</param>
		/// <param name="generator">The optional generator used during reduction.</param>
		public TruncateExpression(Expression input, IExpressionGenerator generator = null)
			: base(input, generator) { Contract.Requires(null != input); }

		/// <inheritdoc/>
		public override Expression Reduce() {
			if (typeof(double) == Type)
				return Call(MathCeilingDoubleMethod, UnaryParameter);
			if (typeof(decimal) == Type)
				return Call(MathCeilingDecimalMethod, UnaryParameter);
			return Convert(Call(MathCeilingDoubleMethod, Convert(UnaryParameter, typeof(double))), Type);
		}
	}
}
