using System;
using System.Linq.Expressions;
using System.Reflection;
using Vertesaur.Generation.Contracts;

namespace Vertesaur.Generation.Expressions
{
	public class AsinExpression : ReducableUnaryExpressionBase
	{
		private static readonly MethodInfo MathAsinMethod;

		static AsinExpression() {
			MathAsinMethod = typeof(Math).GetMethod(
				"Asin",
				BindingFlags.Public | BindingFlags.Static,
				null, new[] { typeof(double) }, null);
		}

		public AsinExpression(Expression input, IExpressionGenerator generator = null)
			: base(input, generator) { }

		public override Expression Reduce() {
			return typeof(double) == Type
				? (Expression)Call(MathAsinMethod, UnaryParameter)
				: Convert(Call(MathAsinMethod, Convert(UnaryParameter, typeof(double))), Type);
		}
	}
}
