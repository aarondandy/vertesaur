using System;
using System.Linq.Expressions;
using System.Reflection;
using Vertesaur.Generation.Contracts;

namespace Vertesaur.Generation.Expressions
{
	public class CeilingExpression : ReducableUnaryExpressionBase
	{
		private static readonly MethodInfo MathCeilingDoubleMethod;
		private static readonly MethodInfo MathCeilingDecimalMethod;

		static CeilingExpression() {
			MathCeilingDoubleMethod = typeof(Math).GetMethod(
				"Ceiling",
				BindingFlags.Public | BindingFlags.Static,
				null, new[] { typeof(double) }, null);
			MathCeilingDecimalMethod = typeof(Math).GetMethod(
				"Ceiling",
				BindingFlags.Public | BindingFlags.Static,
				null, new[] { typeof(decimal) }, null);
		}

		public CeilingExpression(Expression input, IExpressionGenerator generator = null)
			: base(input, generator) { }

		public override Expression Reduce() {
			if (typeof(double) == Type)
				return Call(MathCeilingDoubleMethod, UnaryParameter);
			if (typeof(decimal) == Type)
				return Call(MathCeilingDecimalMethod, UnaryParameter);
			return Convert(Call(MathCeilingDoubleMethod, Convert(UnaryParameter, typeof(double))), Type);
		}
	}
}
