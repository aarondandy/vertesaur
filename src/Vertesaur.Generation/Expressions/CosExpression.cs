using System;
using System.Linq.Expressions;
using System.Reflection;
using Vertesaur.Generation.Contracts;

namespace Vertesaur.Generation.Expressions
{
	public class CosExpression : ReducableUnaryExpressionBase
	{
		private static readonly MethodInfo MathCosMethod;

		static CosExpression() {
			MathCosMethod = typeof(Math).GetMethod(
				"Cos",
				BindingFlags.Public | BindingFlags.Static,
				null, new[] { typeof(double) }, null);
		}

		public CosExpression(Expression input, IExpressionGenerator generator = null)
			: base(input, generator) { }

		public override Expression Reduce() {
			return typeof(double) == Type
				? (Expression)Call(MathCosMethod, UnaryParameter)
				: Convert(Call(MathCosMethod, Convert(UnaryParameter, typeof(double))), Type);
		}
	}
}
