using System;
using System.Linq.Expressions;
using System.Reflection;
using Vertesaur.Generation.Contracts;

namespace Vertesaur.Generation.Expressions
{
	public class Log10Expression : ReducableUnaryExpressionBase
	{
		private static readonly MethodInfo MathLog10Method;

		static Log10Expression() {
			MathLog10Method = typeof(Math).GetMethod(
				"Log10",
				BindingFlags.Public | BindingFlags.Static,
				null, new[] { typeof(double) }, null);
		}

		public Log10Expression(Expression input, IExpressionGenerator generator = null)
			: base(input, generator) { }

		public override Expression Reduce() {
			return typeof(double) == Type
				? (Expression)Call(MathLog10Method, UnaryParameter)
				: Convert(Call(MathLog10Method, Convert(UnaryParameter, typeof(double))), Type);
		}
	}
}
