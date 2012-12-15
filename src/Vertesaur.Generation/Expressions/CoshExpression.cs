using System;
using System.Linq.Expressions;
using System.Reflection;
using Vertesaur.Generation.Contracts;

namespace Vertesaur.Generation.Expressions
{
	public class CoshExpression : ReducableUnaryExpressionBase
	{

		private static readonly MethodInfo MathCoshMethod;

		static CoshExpression() {
			MathCoshMethod = typeof(Math).GetMethod(
				"Cosh",
				BindingFlags.Public | BindingFlags.Static,
				null, new[] { typeof(double) }, null);
		}

		public CoshExpression(Expression input, IExpressionGenerator generator = null)
			: base(input, generator) { }

		public override Expression Reduce() {
			return typeof(double) == Type
				? (Expression)Call(MathCoshMethod, UnaryParameter)
				: Convert(Call(MathCoshMethod, Convert(UnaryParameter, typeof(double))), Type);
		}

	}
}
