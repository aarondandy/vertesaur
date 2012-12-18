using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using Vertesaur.Generation.Contracts;

namespace Vertesaur.Generation.Expressions
{
	/// <summary>
	/// An expression that can be reduced to a compilable expression and which also takes two expressions as input.
	/// </summary>
	public abstract class ReducibleBinaryExpressionBase : ReducibleExpressionBase
	{
		/// <summary>
		/// Creates a new reducible expression with the given left and right parameters.
		/// </summary>
		/// <param name="leftParameter">The first or left parameter.</param>
		/// <param name="rightParameter">The second or right parameter.</param>
		/// <param name="reductionExpressionGenerator">The optional expression generator used for reduction.</param>
		protected ReducibleBinaryExpressionBase(
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

		/// <summary>
		/// The first or left parameter.
		/// </summary>
		public Expression LeftParameter { get; private set; }
		/// <summary>
		/// The second or right parameter.
		/// </summary>
		public Expression RightParameter { get; private set; }

		[ContractInvariantMethod]
		[Conditional("CONTRACTS_FULL")]
		private void CodeContractInvariant() {
			Contract.Invariant(LeftParameter != null);
			Contract.Invariant(RightParameter != null);
		}

	}
}
