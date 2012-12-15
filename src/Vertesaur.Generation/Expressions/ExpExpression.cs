using System;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using System.Reflection;
using Vertesaur.Generation.Contracts;

namespace Vertesaur.Generation.Expressions
{
	/// <summary>
	/// An expression equivalent to the constant E raised to the Nth power.
	/// </summary>
	public class ExpExpression : ReducableUnaryExpressionBase
	{
		private static readonly MethodInfo MathExpMethod;

		static ExpExpression() {
			MathExpMethod = typeof(Math).GetMethod(
				"Exp",
				BindingFlags.Public | BindingFlags.Static,
				null, new[] { typeof(double) }, null);
		}

		/// <summary>
		/// Creates a new E^N expression.
		/// </summary>
		/// <param name="input">The expression used to raise E to a power.</param>
		/// <param name="generator">The optional expression generator used during reduction.</param>
		public ExpExpression(Expression input, IExpressionGenerator generator = null)
			: base(input, generator) { Contract.Requires(null != input); }

		/// <inheritdoc/>
		public override Expression Reduce() {
			if (typeof(double) == Type)
				return Call(MathExpMethod, UnaryParameter);
			if (typeof(float) != Type && typeof(int) != Type) {
				var eValue = ReductionExpressionGenerator.Generate("E", Type);
				if (null != eValue) {
					var pow = ReductionExpressionGenerator.Generate("POW", eValue, UnaryParameter);
					if (null != pow) {
						return pow;
					}
				}
			}
			return Convert(Call(MathExpMethod, Convert(UnaryParameter, typeof(double))), Type);
		}
	}
}
