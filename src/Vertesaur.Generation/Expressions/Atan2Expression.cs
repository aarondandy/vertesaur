using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using System.Reflection;
using Vertesaur.Generation.Contracts;

namespace Vertesaur.Generation.Expressions
{
	public class Atan2Expression : ReducableBinaryExpressionBase
	{
		private static readonly MethodInfo MathAtan2DoubleMethod;

		static Atan2Expression() {
			MathAtan2DoubleMethod = typeof(Math).GetMethod(
				"Atan2",
				BindingFlags.Public | BindingFlags.Static,
				null, new[] { typeof(double),typeof(double) }, null);
		}

		public Atan2Expression(Expression left, Expression right, IExpressionGenerator generator = null)
			: base(left, right, generator) { }

		public override Expression Reduce() {
			if (typeof(double) == LeftParameter.Type && typeof(double) == RightParameter.Type)
				return Call(MathAtan2DoubleMethod, LeftParameter, RightParameter);
			if (typeof(float) == LeftParameter.Type && typeof(float) == RightParameter.Type)
				return ReductionExpressionGenerator.GenerateConversionExpression(
					typeof(float),
					Call(MathAtan2DoubleMethod,
						ReductionExpressionGenerator.GenerateConversionExpression(typeof(double),LeftParameter),
						ReductionExpressionGenerator.GenerateConversionExpression(typeof(double),RightParameter)
					)
				);

			if (LeftParameter.IsMemoryLocationOrConstant() && RightParameter.IsMemoryLocationOrConstant())
				return GenerateAtan2Expressions(LeftParameter, RightParameter);

			return new BlockExpressionBuilder().AddUsingMemoryLocationsOrConstants(
				args => new[]{GenerateAtan2Expressions(args[0], args[1])},
				LeftParameter, RightParameter
			).GetExpression();
		}

		private Expression GenerateAtan2Expressions(Expression y, Expression x) {
			Contract.Ensures(Contract.Result<IEnumerable<Expression>>() != null);
			Contract.Assume(null != y);
			Contract.Assume(null != x);
			var gen = ReductionExpressionGenerator;

			var xGreaterZero = gen.Generate("ATAN", gen.Generate("DIVIDE", y, x));
			var xEqualZero = Condition(
				gen.Generate("EQUAL", y, gen.Generate("ZERO", y.Type)),
				// y == 0 && x == 0
				gen.Generate("INVALID", y.Type) ?? gen.Generate("ZERO", y.Type),
				Condition(
					gen.Generate("GREATER", y, gen.Generate("ZERO", y.Type)),
					// y > 0 && x == 0
					gen.Generate("HALFPI", y.Type),
					// y < 0 && x == 0
					gen.Generate("NEGATE", gen.Generate("HALFPI", y.Type))
				)
			);
			var xLessZero = Condition(
				gen.Generate("GREATEREQUAL", y, gen.Generate("ZERO", x.Type)),
				// y >= 0 && x < 0
				gen.Generate("ADD",
					gen.Generate("ATAN", gen.Generate("DIVIDE", y, x)),
					gen.Generate("PI", x.Type)),
				// y < 0 && x < 0
				gen.Generate("SUBTRACT",
					gen.Generate("ATAN", gen.Generate("DIVIDE", y, x)),
					gen.Generate("PI", x.Type))
			);
			var xLequalZero = Condition(
				gen.Generate("EQUAL", x, gen.Generate("ZERO", x.Type)),
				xEqualZero,
				xLessZero
			);

			return Condition(
				gen.Generate("GREATER", x, gen.Generate("ZERO", x.Type)),
				xGreaterZero,
				xLequalZero
			);
		}

		public override Type Type {
			get {
				var t = LeftParameter.Type == RightParameter.Type
					? LeftParameter.Type
					: Reduce().Type;
				return t;
			}
		}
	}
}
