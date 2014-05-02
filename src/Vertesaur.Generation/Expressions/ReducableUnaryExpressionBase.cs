using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;

namespace Vertesaur.Generation.Expressions
{
    /// <summary>
    /// An expression that can be reduced to a compilable expression and which also takes a single expression as input.
    /// </summary>
    public abstract class ReducibleUnaryExpressionBase : ReducibleExpressionBase
    {

        /// <summary>
        /// Creates a new reducible expression with the given single parameter.
        /// </summary>
        /// <param name="unaryParameter">The single input parameter for the expression.</param>
        /// <param name="reductionExpressionGenerator">The optional expression generator used for reduction.</param>
        protected ReducibleUnaryExpressionBase(
            Expression unaryParameter,
            IExpressionGenerator reductionExpressionGenerator = null
        ) : base(reductionExpressionGenerator) {
            if (null == unaryParameter) throw new ArgumentNullException("unaryParameter");
            Contract.EndContractBlock();
            UnaryParameter = unaryParameter;
        }

        [ContractInvariantMethod]
        private void CodeContractInvariant() {
            Contract.Invariant(UnaryParameter != null);
        }

        /// <summary>
        /// The single input parameter for the expression.
        /// </summary>
        public Expression UnaryParameter { get; private set; }

        /// <inheritdoc/>
        public override Type Type {
            get {
                Contract.Ensures(Contract.Result<Type>() != null);
                return UnaryParameter.Type;
            }
        }

    }
}
