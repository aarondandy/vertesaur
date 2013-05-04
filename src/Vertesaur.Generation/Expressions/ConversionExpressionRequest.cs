using System;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using Vertesaur.Utility;

namespace Vertesaur.Generation.Expressions
{
    /// <summary>
    /// Creates a new conversion expression request.
    /// </summary>
    public class ConversionExpressionRequest : IExpressionGenerationRequest
    {

        /// <summary>
        /// Creates a new function expression generation request. This request is for a function that accepts one or more expressions as input.
        /// </summary>
        /// <param name="generator">The primary generator to be used for the generation of sub expressions.</param>
        /// <param name="inputExpression">The name of the requested constant.</param>
        /// <param name="resultType">The desired type of the constant.</param>
        public ConversionExpressionRequest(IExpressionGenerator generator, Expression inputExpression, Type resultType) {
            if (null == generator) throw new ArgumentNullException("generator");
            if (null == inputExpression) throw new ArgumentNullException("inputExpression");
            if (null == resultType) throw new ArgumentNullException("resultType");
            Contract.EndContractBlock();
            TopLevelGenerator = generator;
            InputExpression = inputExpression;
            DesiredResultType = resultType;
        }

        [ContractInvariantMethod]
        private void CodeContractInvariants() {
            Contract.Invariant(TopLevelGenerator != null);
            Contract.Invariant(InputExpression != null);
            Contract.Invariant(DesiredResultType != null);
        }

        /// <summary>
        /// The expression to be converted.
        /// </summary>
        public Expression InputExpression { get; private set; }

        /// <inheritdoc/>
        public IExpressionGenerator TopLevelGenerator { get; private set; }

        /// <inheritdoc/>
        public string ExpressionName {
            get {
                Contract.Ensures(!String.IsNullOrEmpty(ExpressionName));
                return "Convert";
            }
        }

        /// <inheritdoc/>
        public ReadOnlyCollection<Expression> InputExpressions {
            get {
                Contract.Ensures(Contract.Result<ReadOnlyCollection<Expression>>() != null);
                return new[] { InputExpression }.AsReadOnly();
            }
        }

        /// <inheritdoc/>
        public Type DesiredResultType { get; private set; }
    }
}
