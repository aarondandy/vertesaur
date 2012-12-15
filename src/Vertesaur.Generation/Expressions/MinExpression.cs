using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Vertesaur.Generation.Contracts;

namespace Vertesaur.Generation.Expressions
{
	public class MinExpression : ReducableBinaryExpressionBase
	{

		public MinExpression(Expression left, Expression right, IExpressionGenerator generator = null)
			: base(left, right, generator) { }

		public override Type Type {
			get { return  LeftParameter.Type; }
		}

		public override Expression Reduce() {
			var method = typeof(Math).GetMethod(
				"Min",
				BindingFlags.Public | BindingFlags.Static,
				null, new[] { LeftParameter.Type,RightParameter.Type }, null);
			if (null != method)
				return Call(method, LeftParameter, RightParameter);

			if (LeftParameter.IsMemoryLocationOrConstant() && RightParameter.IsMemoryLocationOrConstant())
				return GenerateExpression(LeftParameter, RightParameter);

			return new BlockExpressionBuilder()
				.AddUsingMemoryLocationsOrConstants(
					args => new[] { GenerateExpression(args[0], args[1]) },
					LeftParameter, RightParameter
				)
				.GetExpression();
		}

		private Expression GenerateExpression(Expression left, Expression right) {
			Contract.Assume(null != left);
			Contract.Assume(null != right);
			Contract.Assume(left.IsMemoryLocationOrConstant());
			Contract.Assume(right.IsMemoryLocationOrConstant());
			return Expression.Condition(
				ReductionExpressionGenerator.Generate("LESSEQUAL", left, right),
				left,
				right
			);
		}
	}
}
