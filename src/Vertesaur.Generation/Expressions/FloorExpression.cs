using System;
using System.Linq.Expressions;
using System.Reflection;
using Vertesaur.Generation.Contracts;

namespace Vertesaur.Generation.Expressions
{
	public class FloorExpression : ReducableUnaryExpressionBase
	{

		private static readonly MethodInfo MathFloorDoubleMethod;
		private static readonly MethodInfo MathFloorDecimalMethod;

		static FloorExpression() {
			MathFloorDoubleMethod = typeof(Math).GetMethod(
				"Floor",
				BindingFlags.Public | BindingFlags.Static,
				null, new[] { typeof(double) }, null);
			MathFloorDecimalMethod = typeof(Math).GetMethod(
				"Floor",
				BindingFlags.Public | BindingFlags.Static,
				null, new[] { typeof(decimal) }, null);
		}

		public FloorExpression(Expression input, IExpressionGenerator generator = null)
			: base(input, generator) { }

		public override Expression Reduce() {
			if (typeof(double) == Type)
				return Call(MathFloorDoubleMethod, UnaryParameter);
			if (typeof(decimal) == Type)
				return Call(MathFloorDecimalMethod, UnaryParameter);
			return Convert(Call(MathFloorDoubleMethod, Convert(UnaryParameter, typeof(double))), Type);
		}

	}
}
