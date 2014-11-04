using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;

namespace Vertesaur.Generation.Expressions
{

    /// <summary>
    /// Combines various generic operation providers together.
    /// Provides a requested expression from the first provider to successfully build one.
    /// </summary>
    public class CombinedExpressionGenerator : IExpressionGenerator
    {

#if NO_MEF
        /// <summary>
        /// Replaces the MEF expression generator for frameworks that do not have MEF.
        /// </summary>
        public static CombinedExpressionGenerator GenerateDefaultMefReplacement() {
            Contract.Ensures(Contract.Result<CombinedExpressionGenerator>() != null);
            return new CombinedExpressionGenerator(new IExpressionGenerator[] {
                new CoreExpressionGenerator(),
                new VectorExpressionGenerator()
            });
        }
#endif

        private readonly IExpressionGenerator[] _expressionGenerators;

        /// <summary>
        /// Constructs a new combined expression generators from the given operation providers.
        /// </summary>
        /// <param name="expressionGenerators">The ordered list of generators to combine.</param>
        public CombinedExpressionGenerator(IEnumerable<IExpressionGenerator> expressionGenerators) {
            if (null == expressionGenerators) throw new ArgumentNullException("expressionGenerators");
            Contract.EndContractBlock();
            _expressionGenerators = expressionGenerators.Where(x => x != null).ToArray();
            Contract.Assume(Contract.ForAll(_expressionGenerators, x => x != null));
        }

        [ContractInvariantMethod]
        private void ObjectInvariants() {
            Contract.Invariant(_expressionGenerators != null);
            Contract.Invariant(Contract.ForAll(_expressionGenerators, x => x != null));
        }

        /// <inheritdoc/>
        public Expression Generate(IExpressionGenerationRequest request) {
            if (null == request) throw new ArgumentNullException("request");
            Contract.EndContractBlock();

            return _expressionGenerators
                .Select(x => x.Generate(request))
                .FirstOrDefault(x => null != x);
        }
    }
}
