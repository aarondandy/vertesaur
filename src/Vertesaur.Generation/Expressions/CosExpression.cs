using System;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using System.Reflection;
using Vertesaur.Generation.Contracts;

namespace Vertesaur.Generation.Expressions
{
	/// <summary>
	/// A cosine expression.
	/// </summary>
	public class CosExpression : ReducibleUnaryExpressionBase
	{
		private static readonly MethodInfo MathCosMethod;

		static CosExpression() {
			MathCosMethod = typeof(Math).GetMethod(
				"Cos",
				BindingFlags.Public | BindingFlags.Static,
				null, new[] { typeof(double) }, null);
		}

		/// <summary>
		/// Creates a new cosine expression.
		/// </summary>
		/// <param name="input">The expression to calculate the cosine of.</param>
		/// <param name="generator">The optional expression generator used during reduction.</param>
		public CosExpression(Expression input, IExpressionGenerator generator = null)
			: base(input, generator) { Contract.Requires(null != input); }

		/// <inheritdoc/>
		public override Expression Reduce() {
			return typeof(double) == Type
				? (Expression)Call(MathCosMethod, UnaryParameter)
				: Convert(Call(MathCosMethod, Convert(UnaryParameter, typeof(double))), Type);
		}
	}
}
