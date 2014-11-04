using System;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;

namespace Vertesaur.Generation.Expressions
{
    /// <summary>
    /// Generates expressions that can be compiled at run-time.
    /// </summary>
    [ContractClass(typeof(ExpressionGeneratorCodeContract))]
    public interface IExpressionGenerator
    {
        /// <summary>
        /// Generates expressions that are described by an expression generation request.
        /// </summary>
        /// <param name="request">The request for an expression that is to be generated.</param>
        /// <returns>A generated expression matching the request or null if the requested expression could not be generated.</returns>
        Expression Generate(IExpressionGenerationRequest request);
    }

    [ContractClassFor(typeof(IExpressionGenerator))]
    internal abstract class ExpressionGeneratorCodeContract : IExpressionGenerator
    {

        public Expression Generate(IExpressionGenerationRequest request) {
            Contract.Requires(null != request);
            Contract.EndContractBlock();
            throw new NotImplementedException();
        }
    }

}
