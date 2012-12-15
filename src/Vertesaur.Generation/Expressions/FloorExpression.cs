using System;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using System.Reflection;
using Vertesaur.Generation.Contracts;

namespace Vertesaur.Generation.Expressions
{

	/// <summary>
	/// A floor expression.
	/// </summary>
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

		/// <summary>
		/// Creates a new floor expression.
		/// </summary>
		/// <param name="input">The expression to calculate the floor of.</param>
		/// <param name="generator">The optional expression generator used during reduction.</param>
		public FloorExpression(Expression input, IExpressionGenerator generator = null)
			: base(input, generator) { Contract.Requires(null != input); }

		/// <inheritdoc/>
		public override Expression Reduce() {
			if (typeof(double) == Type)
				return Call(MathFloorDoubleMethod, UnaryParameter);
			if (typeof(decimal) == Type)
				return Call(MathFloorDecimalMethod, UnaryParameter);
			return Convert(Call(MathFloorDoubleMethod, Convert(UnaryParameter, typeof(double))), Type);
		}

	}
}
