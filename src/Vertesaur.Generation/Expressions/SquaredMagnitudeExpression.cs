using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using Vertesaur.Generation.Utility;

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
        public SquaredMagnitudeExpression(Expression[] components, IExpressionGenerator reductionExpressionGenerator = null)
            : base(reductionExpressionGenerator) {
            if (null == components) throw new ArgumentNullException("components");
            if (components.Length == 0) throw new ArgumentException("Must have at least 1 component.", "components");
            Contract.Requires(Contract.ForAll(components, x => x != null));

            Components = components; // TODO: clone?

            if (Components.ContainsNull())
                throw new ArgumentException("All components expressions must be non null.", "components");
        }

        [ContractInvariantMethod]
        private void CodeContractInvariants() {
            Contract.Invariant(Components != null);
            Contract.Invariant(Components.Length > 0);
            Contract.Invariant(Contract.ForAll(Components, x => x != null));
        }

        /// <summary>
        /// The coordinate expressions.
        /// </summary>
        private Expression[] Components { get; set; }

        /// <inheritdoc/>
        public override Type Type {
            get {
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

        private Expression CreateExpression(Expression[] inputs) {
            Contract.Requires(null != inputs);
            Contract.Requires(inputs.Length > 0);
            Contract.Requires(Contract.ForAll(inputs, x => x != null));
            Contract.Ensures(Contract.Result<Expression>() != null);

            var gen = ReductionExpressionGenerator;
            var result = gen.GenerateOrThrow("SQUARE", inputs[0]);
            for (int i = 1; i < inputs.Length; i++) {
                result = gen.GenerateOrThrow("ADD", result, gen.GenerateOrThrow("SQUARE", inputs[i]));
            }
            return result;
        }

    }
}
