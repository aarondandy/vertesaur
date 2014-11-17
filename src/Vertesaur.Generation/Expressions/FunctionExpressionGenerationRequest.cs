using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using Vertesaur.Generation.Utility;

namespace Vertesaur.Generation.Expressions
{
    /// <summary>
    /// An expression generation request for a function that takes expressions as input.
    /// </summary>
    public class FunctionExpressionGenerationRequest : IExpressionGenerationRequest
    {

        /// <summary>
        /// Creates a new function expression generation request. This request is for a function that accepts one or more expressions as input.
        /// </summary>
        /// <param name="generator">The primary generator to be used for the generation of sub expressions.</param>
        /// <param name="expressionName">The name of the requested expression.</param>
        /// <param name="inputs">The input expressions for the generated expression.</param>
        [ContractVerification(false)] // TODO: remove when CC bugs are fixed
        public FunctionExpressionGenerationRequest(IExpressionGenerator generator, string expressionName, params Expression[] inputs) {
            if (null == generator) throw new ArgumentNullException("generator");
            if (String.IsNullOrEmpty(expressionName)) throw new ArgumentException("Invalid expression name.", "expressionName");
            if (null == inputs) throw new ArgumentNullException("inputs");
            if (inputs.Length == 0) throw new ArgumentException("At least one input expression is required.", "inputs");
            Contract.Requires(Contract.ForAll(inputs, x => x != null));

            TopLevelGenerator = generator;
            ExpressionName = expressionName;
            _inputExpressions = inputs; // TODO: consider a clone

            if (_inputExpressions.ContainsNull())
                throw new ArgumentException("No expressions may not be null", "inputs");
        }

        [ContractInvariantMethod]
        private void CodeContractInvariants() {
            Contract.Invariant(TopLevelGenerator != null);
            Contract.Invariant(!String.IsNullOrEmpty(ExpressionName));
            Contract.Invariant(_inputExpressions != null);
            Contract.Invariant(Contract.ForAll(_inputExpressions, x => x != null));
        }

        private readonly Expression[] _inputExpressions;

        /// <inheritdoc/>
        public IExpressionGenerator TopLevelGenerator { get; private set; }

        /// <inheritdoc/>
        public string ExpressionName { get; private set; }

        /// <inheritdoc/>
        public IList<Expression> InputExpressions {
            get {
                var result = _inputExpressions.AsReadOnly();
                Contract.Assume(Contract.ForAll(result, x => x != null));
                return result;
            }
        }

        /// <inheritdoc/>
        public Type DesiredResultType { get { return null; } }
    }
}
