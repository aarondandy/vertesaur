using System;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using System.Reflection;
using Vertesaur.Generation.Contracts;
using Vertesaur.Utility;

namespace Vertesaur.Generation.Expressions
{
	/// <summary>
	/// A natural logarithm expression.
	/// </summary>
	public class LogExpression : ReducibleUnaryExpressionBase
	{
		private static readonly MethodInfo MathLogMethod;

		static LogExpression() {
			MathLogMethod = typeof(Math).GetPublicStaticInvokableMethod("Log", typeof(double));
		}

		/// <summary>
		/// Creates a new natural logarithm expression.
		/// </summary>
		/// <param name="input">The expression to calculate the natural logarithm of.</param>
		/// <param name="generator">The optional expression generator used during reduction.</param>
		public LogExpression(Expression input, IExpressionGenerator generator = null)
			: base(input, generator) { Contract.Requires(null != input); }

		/// <inheritdoc/>
		public override Expression Reduce() {
			return typeof(double) == Type
				? (Expression)Call(MathLogMethod, UnaryParameter)
				: Convert(Call(MathLogMethod, Convert(UnaryParameter, typeof(double))), Type);
		}
	}
}
