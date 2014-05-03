using System;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;

namespace Vertesaur.Generation.Expressions
{
    /// <summary>
    /// An expression to calculate the distance between two points.
    /// </summary>
    public class DistanceExpression : ReducibleExpressionBase
    {

        /// <summary>
        /// Creates a new dot product expression.
        /// </summary>
        /// <param name="components">The ordered components of the two vectors in the order of first vectors coordinates then second vectors coordinates (ex: x0,y0,x1,y1).</param>
        /// <param name="reductionExpressionGenerator">The optional expression generator that can be used to produce reduced expressions.</param>
        public DistanceExpression(Expression[] components, IExpressionGenerator reductionExpressionGenerator = null)
            : this(new SquaredDistanceExpression(components, reductionExpressionGenerator), reductionExpressionGenerator) {
            Contract.Requires(components != null);
            Contract.Requires(components.Length != 0);
            Contract.Requires(components.Length % 2 == 0);
            Contract.Requires(Contract.ForAll(components, x => null != x));
        }

        private DistanceExpression(SquaredDistanceExpression innerExpression, IExpressionGenerator reductionExpressionGenerator = null)
            : base(reductionExpressionGenerator) {
            if (innerExpression == null) throw new ArgumentNullException("innerExpression");
            Contract.EndContractBlock();
            InnerExpression = innerExpression;
        }

        [ContractInvariantMethod]
        private void CodeContractInvariants() {
            Contract.Invariant(InnerExpression != null);
        }

        /// <summary>
        /// The internal squared distance expression.
        /// </summary>
        public SquaredDistanceExpression InnerExpression { get; private set; }

        /// <inheritdoc/>
        public override Type Type {
            get {
                Contract.Ensures(Contract.Result<Type>() != null);
                return InnerExpression.Type;
            }
        }

        /// <inheritdoc/>
        public override Expression Reduce() {
            Contract.Ensures(Contract.Result<Expression>() != null);
            // TODO: use some square root utility method that does not take the square root of a square
            return ReductionExpressionGenerator.Generate("SquareRoot", InnerExpression)
                ?? new SquareRootExpression(InnerExpression, ReductionExpressionGenerator);
        }

    }
}
