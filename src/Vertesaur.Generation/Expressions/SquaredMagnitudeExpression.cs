using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using Vertesaur.Generation.Utility;
using Vertesaur.Utility;

namespace Vertesaur.Generation.Expressions
{
    /// <summary>
    /// An expression representing the squared magnitude of a set of expressions representing coordinates.
    /// </summary>
    public class SquaredMagnitudeExpression : ReducibleExpressionBase
    {

        /// <summary>
        /// Creates a new squared magnitude expression.
        /// </summary>
        /// <param name="components">The coordinate expressions.</param>
        /// <param name="reductionExpressionGenerator">The optional expression generator used for reduction.</param>
        public SquaredMagnitudeExpression(IList<Expression> components, IExpressionGenerator reductionExpressionGenerator = null)
            : base(reductionExpressionGenerator) {
            if (null == components) throw new ArgumentNullException("components");
            if (components.Count == 0) throw new ArgumentException("Must have at least 1 component.", "components");
            Contract.Requires(Contract.ForAll(components, x => x != null));
            
            if (components.Any(x => null == x))
                throw new ArgumentException("All components expressions must be non null.", "components");
            Components = components.ToArray().AsReadOnly();
        }

        [ContractInvariantMethod]
        private void CodeContractInvariants() {
            Contract.Invariant(Components != null);
            Contract.Invariant(Components.Count > 0);
            Contract.Invariant(Contract.ForAll(Components, x => x != null));
        }

        /// <summary>
        /// The coordinate expressions.
        /// </summary>
        public ReadOnlyCollection<Expression> Components { get; private set; }

        /// <inheritdoc/>
        public override Type Type {
            get {
                Contract.Assume(Components[0] != null);
                return Components[0].Type;
            }
        }

        /// <inheritdoc/>
        public override Expression Reduce() {
            Contract.Ensures(Contract.Result<Expression>() != null);

            if (Components.All(x => x.IsMemoryLocationOrConstant()))
                return CreateExpression(Components);

            return new BlockExpressionBuilder().AddUsingMemoryLocationsOrConstants(
                x => new[] { CreateExpression(x) },
                Components.ToArray()
            ).GetExpression();
        }

        private Expression CreateExpression(IList<Expression> inputs) {
            Contract.Requires(null != inputs);
            Contract.Requires(inputs.Count > 0);
            Contract.Requires(Contract.ForAll(inputs, x => x != null));
            Contract.Ensures(Contract.Result<Expression>() != null);

            var gen = ReductionExpressionGenerator;
            var result = gen.GenerateOrThrow("SQUARE", inputs[0]);
            for (int i = 1; i < inputs.Count; i++) {
                result = gen.GenerateOrThrow("ADD", result, gen.GenerateOrThrow("SQUARE", inputs[i]));
            }
            return result;
        }

    }
}
