using System;
using System.Linq.Expressions;
using System.Reflection;
using Vertesaur.Generation.Contracts;

namespace Vertesaur.Generation.Expressions
{
	public class LogExpression : ReducableUnaryExpressionBase
	{
		private static readonly MethodInfo MathLogMethod;

		static LogExpression() {
			MathLogMethod = typeof(Math).GetMethod(
				"Log",
				BindingFlags.Public | BindingFlags.Static,
				null, new[] { typeof(double) }, null);
		}

		public LogExpression(Expression input, IExpressionGenerator generator = null)
			: base(input, generator) { }

		public override Expression Reduce() {
			return typeof(double) == Type
				? (Expression)Call(MathLogMethod, UnaryParameter)
				: Convert(Call(MathLogMethod, Convert(UnaryParameter, typeof(double))), Type);
		}
	}
}
