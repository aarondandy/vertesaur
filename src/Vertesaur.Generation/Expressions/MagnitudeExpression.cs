using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using Vertesaur.Generation.Contracts;

namespace Vertesaur.Generation.Expressions
{
	/// <summary>
	/// An expression representing the magnitude of a set of expressions representing coordinates.
	/// </summary>
	public class MagnitudeExpression : ReducableExpressionBase
	{

		/// <summary>
		/// Creates a new magnitude expression.
		/// </summary>
		/// <param name="components">The coordinate expressions.</param>
		/// <param name="reductionExpressionGenerator">The optional expression generator used for reduction.</param>
		public MagnitudeExpression(IList<Expression> components, IExpressionGenerator reductionExpressionGenerator = null)
			: base(reductionExpressionGenerator){
			Contract.Requires(null != components);
			Contract.Requires(components.Count != 0);
			Contract.Requires(components.All(x => null != x));
			Contract.Ensures(null != InnerExpression);
			Contract.EndContractBlock();
			InnerExpression = new SquaredMagnitudeExpression(components, reductionExpressionGenerator);
		}

		/// <summary>
		/// The inner squared magnitude expression.
		/// </summary>
		public SquaredMagnitudeExpression InnerExpression { get; private set; }

		/// <inheritdoc/>
		public override Type Type { get { return InnerExpression.Type; } }

		/// <inheritdoc/>
		public override Expression Reduce(){
			Contract.Ensures(Contract.Result<Expression>() != null);
			// TODO: use some square root utility method that does not take the square root of a square
			return ReductionExpressionGenerator.Generate("SquareRoot",InnerExpression)
				?? new SquareRootExpression(InnerExpression, ReductionExpressionGenerator);
		}

	}
}
