using System;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using System.Reflection;
using Vertesaur.Generation.Contracts;

namespace Vertesaur.Generation.Expressions
{
	/// <summary>
	/// A base 10 logarithm expression.
	/// </summary>
	public class Log10Expression : ReducableUnaryExpressionBase
	{
		private static readonly MethodInfo MathLog10Method;

		static Log10Expression() {
			MathLog10Method = typeof(Math).GetMethod(
				"Log10",
				BindingFlags.Public | BindingFlags.Static,
				null, new[] { typeof(double) }, null);
		}

		/// <summary>
		/// Creates a new base 10 logarithm expression.
		/// </summary>
		/// <param name="input">The expression to calculate the base 10 logarithm of.</param>
		/// <param name="generator">The optional expression generator used during reduction.</param>
		public Log10Expression(Expression input, IExpressionGenerator generator = null)
			: base(input, generator) { Contract.Requires(null != input); }

		/// <inheritdoc/>
		public override Expression Reduce() {
			return typeof(double) == Type
				? (Expression)Call(MathLog10Method, UnaryParameter)
				: Convert(Call(MathLog10Method, Convert(UnaryParameter, typeof(double))), Type);
		}
	}
}
