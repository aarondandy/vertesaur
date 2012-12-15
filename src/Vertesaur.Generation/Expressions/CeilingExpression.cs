using System;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using System.Reflection;
using Vertesaur.Generation.Contracts;

namespace Vertesaur.Generation.Expressions
{
	/// <summary>
	/// A ceiling expression.
	/// </summary>
	public class CeilingExpression : ReducableUnaryExpressionBase
	{
		private static readonly MethodInfo MathCeilingDoubleMethod;
		private static readonly MethodInfo MathCeilingDecimalMethod;

		static CeilingExpression() {
			MathCeilingDoubleMethod = typeof(Math).GetMethod(
				"Ceiling",
				BindingFlags.Public | BindingFlags.Static,
				null, new[] { typeof(double) }, null);
			MathCeilingDecimalMethod = typeof(Math).GetMethod(
				"Ceiling",
				BindingFlags.Public | BindingFlags.Static,
				null, new[] { typeof(decimal) }, null);
		}

		/// <summary>
		/// Creates a new ceiling expression.
		/// </summary>
		/// <param name="input">The expression to calculate the ceiling of.</param>
		/// <param name="generator">The optional expression generator used during reduction.</param>
		public CeilingExpression(Expression input, IExpressionGenerator generator = null)
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
