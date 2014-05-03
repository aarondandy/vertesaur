using System;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;

namespace Vertesaur.Generation.Expressions
{
    /// <summary>
    /// An expression representing the magnitude of a set of expressions representing coordinates.
    /// </summary>
    public class MagnitudeExpression : ReducibleExpressionBase
    {

        /// <summary>
        /// Creates a new magnitude expression.
        /// </summary>
        /// <param name="components">The coordinate expressions.</param>
        /// <param name="reductionExpressionGenerator">The optional expression generator used for reduction.</param>
        public MagnitudeExpression(Expression[] components, IExpressionGenerator reductionExpressionGenerator = null)
            : this(new SquaredMagnitudeExpression(components, reductionExpressionGenerator), reductionExpressionGenerator) {
            Contract.Requires(components != null);
            Contract.Requires(components.Length != 0);
            Contract.Requires(Contract.ForAll(components, x => x != null));
        }

        private MagnitudeExpression(SquaredMagnitudeExpression innerExpression, IExpressionGenerator reductionExpressionGenerator = null) {
            if(innerExpression == null) throw new ArgumentNullException("innerExpression");
            Contract.EndContractBlock();
            InnerExpression = innerExpression;
        }

        [ContractInvariantMethod]
        private void CodeContractInvariants() {
            Contract.Invariant(InnerExpression != null);
        }

        /// <summary>
        /// The inner squared magnitude expression.
        /// </summary>
        public SquaredMagnitudeExpression InnerExpression { get; private set; }

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
