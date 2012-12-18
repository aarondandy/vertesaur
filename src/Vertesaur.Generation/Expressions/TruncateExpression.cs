using System;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using System.Reflection;
using Vertesaur.Generation.Contracts;
using Vertesaur.Utility;

namespace Vertesaur.Generation.Expressions
{
	/// <summary>
	/// A truncate expression which removes any decimals from a number, leaving only the integer part.
	/// </summary>
	public class TruncateExpression : ReducibleUnaryExpressionBase
	{
		private static readonly MethodInfo MathCeilingDoubleMethod;
		private static readonly MethodInfo MathCeilingDecimalMethod;

		static TruncateExpression() {
			MathCeilingDoubleMethod = typeof(Math).GetPublicStaticInvokableMethod("Truncate",typeof(double));
			MathCeilingDecimalMethod = typeof(Math).GetPublicStaticInvokableMethod("Truncate",typeof(decimal));
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
