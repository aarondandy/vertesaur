using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using Vertesaur.Generation.Contracts;
using Vertesaur.Utility;

namespace Vertesaur.Generation.Expressions
{
    /// <summary>
    /// An expression representing the squared distance between two points.
    /// </summary>
    public class SquaredDistanceExpression : ReducibleExpressionBase
    {

        /// <summary>
        /// Creates a new dot product expression.
        /// </summary>
        /// <param name="components">The ordered components of the two vectors in the order of first vectors coordinates then second vectors coordinates (ex: x0,y0,x1,y1).</param>
        /// <param name="reductionExpressionGenerator">The optional expression generator that can be used to produce reduced expressions.</param>
        public SquaredDistanceExpression(IList<Expression> components, IExpressionGenerator reductionExpressionGenerator = null)
            : base(reductionExpressionGenerator) {
            if (null == components) throw new ArgumentNullException("components");
            if (components.Count == 0) throw new ArgumentException("Must have at least 1 component.", "components");
            if ((components.Count % 2) != 0) throw new ArgumentException("Must have an even number of components.", "components");
            Contract.Requires(components.All(x => null != x));
            Contract.Ensures(Components != null);
            Contract.Ensures(Components.Count > 0);

            if (components.Any(x => null == x))
                throw new ArgumentException("All components expressions must be non null.", "components");
            Components = components.ToArray().AsReadOnly();
        }

        [ContractInvariantMethod]
        private void CodeContractInvariants() {
            Contract.Invariant(Components != null);
        }

        /// <summary>
        /// The coordinate expressions to find the dot product of.
        /// </summary>
        public ReadOnlyCollection<Expression> Components { get; private set; }

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
            var halfCount = Components.Count / 2;
            var deltas = new Expression[halfCount];
            for (int i = 0; i < halfCount; i++) {
                deltas[i] = ReductionExpressionGenerator.Generate("Subtract", Components[i], Components[halfCount + i]);
            }
            Contract.Assume(deltas.Length != 0);
            return ReductionExpressionGenerator.Generate("SquaredMagnitude", deltas);
        }

    }
}
