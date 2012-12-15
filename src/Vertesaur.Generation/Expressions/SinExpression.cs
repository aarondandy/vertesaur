using System;
using System.Linq.Expressions;
using System.Reflection;
using Vertesaur.Generation.Contracts;

namespace Vertesaur.Generation.Expressions
{
	public class SinExpression : ReducableUnaryExpressionBase
	{
		private static readonly MethodInfo MathSinMethod;

		static SinExpression() {
			MathSinMethod = typeof(Math).GetMethod(
				"Sin",
				BindingFlags.Public | BindingFlags.Static,
				null, new[] { typeof(double) }, null);
		}

		public SinExpression(Expression input, IExpressionGenerator generator = null)
			: base(input, generator) { }

		public override Expression Reduce() {
			return typeof(double) == Type
				? (Expression)Call(MathSinMethod, UnaryParameter)
				: Convert(Call(MathSinMethod, Convert(UnaryParameter, typeof(double))), Type);
		}

	}
}
