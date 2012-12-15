using System;
using System.Linq.Expressions;
using System.Reflection;
using Vertesaur.Generation.Contracts;

namespace Vertesaur.Generation.Expressions
{
	public class AtanExpression : ReducableUnaryExpressionBase
	{
		private static readonly MethodInfo MathAtanMethod;

		static AtanExpression() {
			MathAtanMethod = typeof(Math).GetMethod(
				"Atan",
				BindingFlags.Public | BindingFlags.Static,
				null, new[] { typeof(double) }, null);
		}

		public AtanExpression(Expression input, IExpressionGenerator generator = null)
			: base(input, generator) { }

		public override Expression Reduce() {
			return typeof(double) == Type
				? (Expression)Call(MathAtanMethod, UnaryParameter)
				: Convert(Call(MathAtanMethod, Convert(UnaryParameter, typeof(double))), Type);
		}
	}
}
