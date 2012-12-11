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

		/// <summary>
		/// Creates a new redicable expression with the given single parameter.
		/// </summary>
		/// <param name="singleParameter">The single input parameter for the expression.</param>
		/// <param name="reductionExpressionGenerator">The optional expression generator used for reduction.</param>
		protected ReducableUnaryExpressionBase(
			Expression singleParameter,
			IExpressionGenerator reductionExpressionGenerator = null
		) : base(reductionExpressionGenerator){
			if(null == singleParameter) throw new ArgumentNullException("singleParameter");
			Contract.EndContractBlock();
			UnaryParameter = singleParameter;
			Contract.Assume(null != UnaryParameter);
		}

		/// <summary>
		/// The single input parameter for the expression.
		/// </summary>
		public Expression UnaryParameter { get; private set; }

		/// <inheritdoc/>
		public override Type Type { get { return UnaryParameter.Type; } }

		[ContractInvariantMethod]
		private void CodeContractInvariant() {
			Contract.Invariant(UnaryParameter != null);
		}

	}
}
