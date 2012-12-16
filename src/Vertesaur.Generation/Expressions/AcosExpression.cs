using System;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using System.Reflection;
using Vertesaur.Generation.Contracts;

namespace Vertesaur.Generation.Expressions
{
	/// <summary>
	/// An arc-cosine expression.
	/// </summary>
	public class AcosExpression : ReducibleUnaryExpressionBase
	{
		private static readonly MethodInfo MathAcosMethod;

		static AcosExpression() {
			MathAcosMethod = typeof(Math).GetMethod(
				"Acos",
				BindingFlags.Public | BindingFlags.Static,
				null, new[] { typeof(double) }, null);
		}

		/// <summary>
		/// Crates a new arc-cosine expression.
		/// </summary>
		/// <param name="input">The expression to calculate the arc-cosine of.</param>
		/// <param name="generator">The optional expression generator used during reduction.</param>
		public AcosExpression(Expression input, IExpressionGenerator generator = null)
			: base(input, generator) { Contract.Requires(null != input); }

		/// <inheritdoc/>
		public override Expression Reduce() {
			return typeof(double) == Type
				? (Expression)Call(MathAcosMethod, UnaryParameter)
				: Convert(Call(MathAcosMethod, Convert(UnaryParameter, typeof(double))), Type);
		}
	}
}
