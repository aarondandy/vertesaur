using System;
using System.Linq.Expressions;
using System.Reflection;
using Vertesaur.Generation.Contracts;

namespace Vertesaur.Generation.Expressions
{
	public class TanExpression : ReducableUnaryExpressionBase
	{
		private static readonly MethodInfo MathTanMethod;

		static TanExpression() {
			MathTanMethod = typeof(Math).GetMethod(
				"Tan",
				BindingFlags.Public | BindingFlags.Static,
				null, new[] { typeof(double) }, null);
		}

		public TanExpression(Expression input, IExpressionGenerator generator = null)
			: base(input, generator) { }

		public override Expression Reduce() {
			return typeof(double) == Type
				? (Expression)Call(MathTanMethod, UnaryParameter)
				: Convert(Call(MathTanMethod, Convert(UnaryParameter, typeof(double))), Type);
		}
	}
}
