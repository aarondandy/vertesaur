using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using Vertesaur.Generation.Contracts;

namespace Vertesaur.Generation.ExpressionBuilder
{
	/// <summary>
	/// An expression representing the magnitude of a set of expressions representing coordinates.
	/// </summary>
	public class MagnitudeExpression : ReducableExpressionBase
	{

		public MagnitudeExpression(IList<Expression> components, IExpressionGenerator reductionExpressionGenerator = null)
			: base(reductionExpressionGenerator){
			Contract.Requires(null != components);
			Contract.Requires(components.Count != 0);
			Contract.Requires(components.All(x => null != x));
			Contract.Ensures(null != InnerExpression);
			Contract.EndContractBlock();
			InnerExpression = new SquaredMagnitudeExpression(components, reductionExpressionGenerator);
		}

		public SquaredMagnitudeExpression InnerExpression { get; private set; }

		public override System.Type Type { get { return InnerExpression.Type; } }

		public override Expression Reduce(){
			return ReductionExpressionGenerator.GenerateExpression(
				new FunctionExpressionGenerationRequest(
					ReductionExpressionGenerator,
					"SquareRoot",
					InnerExpression
				)
			) ?? new SquareRootExpression(InnerExpression, ReductionExpressionGenerator);
		}

	}
}
