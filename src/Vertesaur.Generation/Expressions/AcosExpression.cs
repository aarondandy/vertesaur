using System;
using System.Linq.Expressions;
using System.Reflection;
using Vertesaur.Generation.Contracts;

namespace Vertesaur.Generation.Expressions
{
	public class AcosExpression : ReducableUnaryExpressionBase
	{
		private static readonly MethodInfo MathAcosMethod;

		static AcosExpression() {
			MathAcosMethod = typeof(Math).GetMethod(
				"Acos",
				BindingFlags.Public | BindingFlags.Static,
				null, new[] { typeof(double) }, null);
		}

		public AcosExpression(Expression input, IExpressionGenerator generator = null)
			: base(input, generator) { }

		public override Expression Reduce() {
			return typeof(double) == Type
				? (Expression)Call(MathAcosMethod, UnaryParameter)
				: Convert(Call(MathAcosMethod, Convert(UnaryParameter, typeof(double))), Type);
		}
	}
}
