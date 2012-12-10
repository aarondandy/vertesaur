using System;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using Vertesaur.Generation.Contracts;

namespace Vertesaur.Generation.ExpressionBuilder
{
	/// <summary>
	/// An expression that can be reduced to a compilable expression and which also takes a single expression as input.
	/// </summary>
	public abstract class ReducableUnaryExpressionBase : ReducableExpressionBase
	{

		protected ReducableUnaryExpressionBase(
			Expression singleParameter,
			IExpressionGenerator reductionExpressionGenerator = null
		) : base(reductionExpressionGenerator){
			if(null == singleParameter) throw new ArgumentNullException("singleParameter");
			Contract.EndContractBlock();
			UnaryParameter = singleParameter;
			Contract.Assume(null != UnaryParameter);
		}

		public Expression UnaryParameter { get; private set; }

		public override Type Type {
			get { return UnaryParameter.Type; }
		}

		[ContractInvariantMethod]
		private void CodeContractInvariant() {
			Contract.Invariant(UnaryParameter != null);
		}

	}
}
