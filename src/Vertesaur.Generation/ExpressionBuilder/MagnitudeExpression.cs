using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Vertesaur.Generation.Contracts;

namespace Vertesaur.Generation.ExpressionBuilder
{
	public class MagnitudeExpression : Expression
	{

		public MagnitudeExpression(IList<Expression> components, IBasicExpressionGenerator basicExpressionGenerator = null)
			: this(new SquaredMagnitudeExpression(components, basicExpressionGenerator)) { }

		public MagnitudeExpression(SquaredMagnitudeExpression innerExpression) {
			if(null == innerExpression) throw new ArgumentNullException();
			Contract.EndContractBlock();
			InnerExpression = innerExpression;
		}

		public SquaredMagnitudeExpression InnerExpression { get; private set; }

		public override bool CanReduce { get { return true; } }

		public override ExpressionType NodeType { get { return ExpressionType.Extension; } }

		public override Expression Reduce() {
			Expression result = null;
			if (null != InnerExpression.BasicExpressionGenerator)
				result = InnerExpression.BasicExpressionGenerator.GetUnaryExpression(BasicUnaryOperationType.SquareRoot, InnerExpression.Type, InnerExpression);
			if(null == result)
				result = DefaultBasicExpressionGenerator.Default.GetUnaryExpression(BasicUnaryOperationType.SquareRoot, InnerExpression.Type, InnerExpression);
			return result;
		}

	}
}
