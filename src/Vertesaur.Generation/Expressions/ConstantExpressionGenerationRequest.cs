using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using Vertesaur.Generation.Utility;

namespace Vertesaur.Generation.Expressions
{
    /// <summary>
    /// A constant expression generation request to get an expression representing a constant value.
    /// </summary>
    public class ConstantExpressionGenerationRequest : IExpressionGenerationRequest
    {
        /// <summary>
        /// Creates a new function expression generation request. This request is for a function that accepts one or more expressions as input.
        /// </summary>
        /// <param name="generator">The primary generator to be used for the generation of sub expressions.</param>
        /// <param name="expressionName">The name of the requested constant.</param>
        /// <param name="resultType">The desired type of the constant.</param>
        public ConstantExpressionGenerationRequest(IExpressionGenerator generator, string expressionName, Type resultType) {
            if (null == generator) throw new ArgumentNullException("generator");
            if (String.IsNullOrEmpty(expressionName)) throw new ArgumentException("Invalid expression name.", "expressionName");
            if (null == resultType) throw new ArgumentNullException("resultType");
            Contract.EndContractBlock();
            TopLevelGenerator = generator;
            ExpressionName = expressionName;
            DesiredResultType = resultType;
        }

        [ContractInvariantMethod]
        private void CodeContractInvariants() {
            Contract.Invariant(TopLevelGenerator != null);
            Contract.Invariant(!String.IsNullOrEmpty(ExpressionName));
            Contract.Invariant(DesiredResultType != null);
        }

        /// <inheritdoc/>
        public IExpressionGenerator TopLevelGenerator { get; private set; }

        /// <inheritdoc/>
        public string ExpressionName { get; private set; }

        /// <inheritdoc/>
        public IList<Expression> InputExpressions { get { return ArrayUtility<Expression>.Empty; } }

        /// <inheritdoc/>
        public Type DesiredResultType { get; private set; }
    }
}
