using System;
using System.Linq.Expressions;
using System.Reflection;
using Vertesaur.Generation.Contracts;

namespace Vertesaur.Generation.Expressions
{
	public class ExpExpression : ReducableUnaryExpressionBase
	{
		private static readonly MethodInfo MathExpMethod;

		static ExpExpression() {
			MathExpMethod = typeof(Math).GetMethod(
				"Exp",
				BindingFlags.Public | BindingFlags.Static,
				null, new[] { typeof(double) }, null);
		}

		public ExpExpression(Expression input, IExpressionGenerator generator = null)
			: base(input, generator) { }

		public override Expression Reduce() {
			if (typeof(double) == Type)
				return Call(MathExpMethod, UnaryParameter);
			if (typeof(float) != Type && typeof(int) != Type) {
				var eValue = ReductionExpressionGenerator.Generate("E", Type);
				if (null != eValue) {
					var pow = ReductionExpressionGenerator.Generate("POW", eValue, UnaryParameter);
					if (null != pow) {
						return pow;
					}
				}
			}
			return Convert(Call(MathExpMethod, Convert(UnaryParameter, typeof(double))), Type);
		}
	}
}
