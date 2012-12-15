using System;
using System.Linq.Expressions;
using System.Reflection;
using Vertesaur.Generation.Contracts;

namespace Vertesaur.Generation.Expressions
{
	public class SinhExpression : ReducableUnaryExpressionBase
	{

		private static readonly MethodInfo MathSinhMethod;

		static SinhExpression() {
			MathSinhMethod = typeof(Math).GetMethod(
				"Sinh",
				BindingFlags.Public | BindingFlags.Static,
				null, new[] { typeof(double) }, null);
		}

		public SinhExpression(Expression input, IExpressionGenerator generator = null)
			: base(input, generator) { }

		public override Expression Reduce() {
			return typeof(double) == Type
				? (Expression)Call(MathSinhMethod, UnaryParameter)
				: Convert(Call(MathSinhMethod, Convert(UnaryParameter, typeof(double))), Type);
		}

	}
}
