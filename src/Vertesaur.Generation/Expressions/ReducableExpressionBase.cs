﻿using System;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;

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
        public sealed override ExpressionType NodeType { get { return ExpressionType.Extension; } }

        /// <inheritdoc/>
        public abstract override Type Type { get; }

        /// <summary>
        /// An expression generator that can be used to create the required expressions.
        /// </summary>
        public IExpressionGenerator ReductionExpressionGenerator {
            get {
                Contract.Ensures(Contract.Result<IExpressionGenerator>() != null);
#if !NO_MEF
                Contract.Assume(MefCombinedExpressionGenerator.Default != null);
                return _reductionExpressionGenerator ?? MefCombinedExpressionGenerator.Default;
#else
                return _reductionExpressionGenerator ?? CombinedExpressionGenerator.GenerateDefaultMefReplacement();
#endif
            }
        }

        /// <inheritdoc/>
        public abstract override Expression Reduce();

    }

}
