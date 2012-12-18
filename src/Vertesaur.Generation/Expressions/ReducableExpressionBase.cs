using System;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using Vertesaur.Generation.Contracts;

namespace Vertesaur.Generation.Expressions
{
	/// <summary>
	/// An expression that can be reduced to a compilable expression.
	/// </summary>
	public abstract class ReducibleExpressionBase : Expression
	{

		private readonly IExpressionGenerator _reductionExpressionGenerator;

		/// <summary>
		/// A basic reducible expression.
		/// </summary>
		/// <param name="reductionExpressionGenerator">The optional expression generator to be used in reduction.</param>
		protected ReducibleExpressionBase(IExpressionGenerator reductionExpressionGenerator = null) {
			_reductionExpressionGenerator = reductionExpressionGenerator;
		}

		/// <inheritdoc/>
		public override bool CanReduce { get { return true; } }

		/// <inheritdoc/>
		public override ExpressionType NodeType { get { return ExpressionType.Extension; } }

		/// <inheritdoc/>
		public abstract override Type Type { get; }

		/// <summary>
		/// An expression generator that can be used to create the required expressions.
		/// </summary>
		public IExpressionGenerator ReductionExpressionGenerator{
			get{
				Contract.Ensures(Contract.Result<IExpressionGenerator>() != null);
				Contract.EndContractBlock();
				var result = _reductionExpressionGenerator ??
#if !NO_MEF
					MefCombinedExpressionGenerator.Default
#else
					CombinedExpressionGenerator.GenerateDefaultMefReplacement()
#endif
					;
				Contract.Assume(null != result);
				return result;
			}
		}

		/// <inheritdoc/>
		public abstract override Expression Reduce();

		[ContractInvariantMethod]
		private void CodeContractInvariant(){
			Contract.Invariant(null != ReductionExpressionGenerator);
		}

	}
}
