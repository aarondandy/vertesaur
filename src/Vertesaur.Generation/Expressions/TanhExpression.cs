using System;
using System.Linq.Expressions;
using System.Reflection;
using Vertesaur.Generation.Contracts;

namespace Vertesaur.Generation.Expressions
{
	public class TanhExpression : ReducableUnaryExpressionBase
	{

		private static readonly MethodInfo MathTanhMethod;

		static TanhExpression() {
			MathTanhMethod = typeof(Math).GetMethod(
				"Tanh",
				BindingFlags.Public | BindingFlags.Static,
				null, new[] { typeof(double) }, null);
		}

		public TanhExpression(Expression input, IExpressionGenerator generator = null)
			: base(input, generator) { }

		public override Expression Reduce() {
			return typeof(double) == Type
				? (Expression)Call(MathTanhMethod, UnaryParameter)
				: Convert(Call(MathTanhMethod, Convert(UnaryParameter, typeof(double))), Type);
		}

	}
}
