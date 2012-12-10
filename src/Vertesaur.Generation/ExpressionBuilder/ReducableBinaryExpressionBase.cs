using System;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using Vertesaur.Generation.Contracts;

namespace Vertesaur.Generation.ExpressionBuilder
{
	/// <summary>
	/// An expression that can be reduced to a compilable expression and which also takes two expressions as input.
	/// </summary>
	public abstract class ReducableBinaryExpressionBase : ReducableExpressionBase
	{

		protected ReducableBinaryExpressionBase(
			Expression leftParameter,
			Expression rightParameter,
			IExpressionGenerator reductionExpressionGenerator = null
		) : base(reductionExpressionGenerator){
			if (null == leftParameter) throw new ArgumentNullException("leftParameter");
			if (null == rightParameter) throw new ArgumentNullException("rightParameter");
			Contract.EndContractBlock();
			LeftParameter = leftParameter;
			RightParameter = rightParameter;
			Contract.Assume(null != LeftParameter);
			Contract.Assume(null != RightParameter);
		}

		public Expression LeftParameter { get; private set; }

		public Expression RightParameter { get; private set; }

		[ContractInvariantMethod]
		private void CodeContractInvariant() {
			Contract.Invariant(LeftParameter != null);
			Contract.Invariant(RightParameter != null);
		}

	}
}
