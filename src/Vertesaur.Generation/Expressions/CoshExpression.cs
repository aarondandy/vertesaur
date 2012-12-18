using System;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using System.Reflection;
using Vertesaur.Generation.Contracts;
using Vertesaur.Utility;

namespace Vertesaur.Generation.Expressions
{
	/// <summary>
	/// A hyperbolic cosine expression.
	/// </summary>
	public class CoshExpression : ReducibleUnaryExpressionBase
	{

		private static readonly MethodInfo MathCoshMethod;

		static CoshExpression() {
			MathCoshMethod = typeof(Math).GetPublicStaticInvokableMethod("Cosh", typeof(double) );
		}

		/// <summary>
		/// Creates a new hyperbolic cosine expression.
		/// </summary>
		/// <param name="input">The expression to calculate the hyperbolic cosine of.</param>
		/// <param name="generator">The optional expression generator used during reduction.</param>
		public CoshExpression(Expression input, IExpressionGenerator generator = null)
			: base(input, generator) { Contract.Requires(null != input); }

		/// <inheritdoc/>
		public override Expression Reduce() {
			return typeof(double) == Type
				? (Expression)Call(MathCoshMethod, UnaryParameter)
				: Convert(Call(MathCoshMethod, Convert(UnaryParameter, typeof(double))), Type);
		}

	}
}
