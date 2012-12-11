﻿using System;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using Vertesaur.Generation.Contracts;

namespace Vertesaur.Generation.ExpressionBuilder
{
	/// <summary>
	/// An expression that can be reduced to a compilable expression.
	/// </summary>
	public abstract class ReducableExpressionBase : Expression
	{

		private readonly IExpressionGenerator _reductionExpressionGenerator;

		/// <summary>
		/// A basic reducable expression.
		/// </summary>
		/// <param name="reductionExpressionGenerator">The optional expression generator to be used in reduction.</param>
		protected ReducableExpressionBase(IExpressionGenerator reductionExpressionGenerator = null) {
			_reductionExpressionGenerator = reductionExpressionGenerator;
		}

		public override bool CanReduce { get { return true; } }

		public override ExpressionType NodeType { get { return ExpressionType.Extension; } }

		public abstract override Type Type { get; }

		/// <summary>
		/// An expression generator that can be used to create the required expressions.
		/// </summary>
		public IExpressionGenerator ReductionExpressionGenerator{
			get{
				Contract.Ensures(Contract.Result<IExpressionGenerator>() != null);
				Contract.EndContractBlock();
				var result = _reductionExpressionGenerator ?? MefCombinedExpressionGenerator.Default;
				Contract.Assume(null != result);
				return result;
			}
		}

		[ContractInvariantMethod]
		private void CodeContractInvariant(){
			Contract.Invariant(null != ReductionExpressionGenerator);
		}

	}
}
