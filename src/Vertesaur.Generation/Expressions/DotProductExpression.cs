using System;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using Vertesaur.Generation.Utility;

namespace Vertesaur.Generation.Expressions
{
    /// <summary>
    /// An expression representing the dot product of two vectors.
    /// </summary>
    public class DotProductExpression : ReducibleExpressionBase
    {

        /// <summary>
        /// Creates a new dot product expression.
        /// </summary>
        /// <param name="components">The ordered components of the two vectors in the order of first vectors coordinates then second vectors coordinates (ex: x0,y0,x1,y1).</param>
        /// <param name="reductionExpressionGenerator">The optional expression generator that can be used to produce reduced expressions.</param>
        public DotProductExpression(Expression[] components, IExpressionGenerator reductionExpressionGenerator = null)
            : base(reductionExpressionGenerator) {
            if (null == components) throw new ArgumentNullException("components");
            if (components.Length == 0) throw new ArgumentException("Must have at least 1 component.", "components");
            if (components.Length % 2 != 0) throw new ArgumentException("Must have an even number of components.", "components");
            Contract.Requires(Contract.ForAll(components, x => x != null));
            Contract.Ensures(Components != null);

            Components = components; // TODO: clone?

            if (Components.ContainsNull())
                throw new ArgumentException("All components expressions must be non null.", "components");
        }

        [ContractInvariantMethod]
        private void CodeContractInvariants() {
            Contract.Invariant(Components != null);
            Contract.Invariant(Components.Length > 0);
            Contract.Invariant(Components.Length % 2 == 0);
            Contract.Invariant(Contract.ForAll(Components, x => x != null));
        }

        /// <summary>
        /// The coordinate expressions to find the dot product of.
        /// </summary>
        private Expression[] Components { get; set; }

        /// <inheritdoc/>
        public override Type Type {
            get {
                Contract.Ensures(Contract.Result<Type>() != null);
                return Components[0].Type;
            }
        }

        /// <inheritdoc/>
        public override Expression Reduce() {
            Contract.Ensures(Contract.Result<Expression>() != null);
            var halfCount = Components.Length / 2;
            Contract.Assume(halfCount <= Components.Length);
            var gen = ReductionExpressionGenerator;
            Contract.Assume(Contract.ForAll(Components, x => x != null));
            var result = gen.GenerateOrThrow(
                "Multiply", Components[0], Components[halfCount]);
            for (int i = 1; i < halfCount; i++) {
                Contract.Assume(halfCount + i < Components.Length);
                var mulComponentExpression = gen.GenerateOrThrow(
                    "Multiply",
                    Components[i],
                    Components[halfCount + i]);
                result = gen.GenerateOrThrow("Add", result, mulComponentExpression);
            }
            return result;
        }

    }
}
