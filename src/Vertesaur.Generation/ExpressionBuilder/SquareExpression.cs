using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Vertesaur.Generation.Contracts;

namespace Vertesaur.Generation.ExpressionBuilder
{
	public class SquareExpression : Expression
	{

		public SquareExpression(Expression parameter, IBasicExpressionGenerator basicExpressionGenerator = null) {
			if(null == parameter) throw new ArgumentNullException("parameter");
			Contract.Ensures(ValueParameter != null);
			Contract.EndContractBlock();
			ValueParameter = parameter;
			BasicExpressionGenerator = basicExpressionGenerator;
		}

		/// <summary>
		/// An expression generator that can be used to create the required expressions.
		/// </summary>
		public IBasicExpressionGenerator BasicExpressionGenerator { get; private set; }

		/// <summary>
		/// The value or expression to find the square of.
		/// </summary>
		public Expression ValueParameter { get; private set; }

		public override bool CanReduce { get { return true; } }

		public override ExpressionType NodeType { get { return ExpressionType.Extension; } }

		public override Expression Reduce() {
			if (ValueParameter is SquareRootExpression)
				return ((SquareRootExpression)ValueParameter).ValueParameter;
			if (ValueParameter is ParameterExpression || ValueParameter is ConstantExpression) {
				return (BasicExpressionGenerator ?? DefaultBasicExpressionGenerator.Default)
					.GetBinaryExpression(BasicBinaryOperationType.Multiply, ValueParameter.Type, ValueParameter, ValueParameter);
			}
			var tempLocal = Expression.Parameter(ValueParameter.Type);
			return Block(
				new[] {tempLocal},
				Assign(tempLocal, ValueParameter),
				(BasicExpressionGenerator ?? DefaultBasicExpressionGenerator.Default)
					.GetBinaryExpression(BasicBinaryOperationType.Multiply, tempLocal.Type, tempLocal, tempLocal)
			);
		}

		[ContractInvariantMethod]
		private void CodeContractInvariant() {
			Contract.Invariant(ValueParameter != null);
		}

	}
}
