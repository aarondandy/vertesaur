using System;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using System.Reflection;
using Vertesaur.Generation.Utility;
using Vertesaur.Utility;

namespace Vertesaur.Generation.Expressions
{
    /// <summary>
    /// An expression representing the square-root of another expression. SquareRootExpression(expression) = (expression)^(1/2).
    /// </summary>
    public class SquareRootExpression : ReducibleUnaryExpressionBase
    {

        private static readonly MethodInfo MathSqrtMethod;

        static SquareRootExpression() {
            MathSqrtMethod = typeof(Math).GetPublicStaticInvokableMethod("Sqrt", typeof(double));
        }

        /// <summary>
        /// Creates a new square root expression.
        /// </summary>
        /// <param name="unaryParameter">The expression to find the square root of.</param>
        /// <param name="reductionExpressionGenerator">The optional expression generator used for reduction.</param>
        public SquareRootExpression(Expression unaryParameter, IExpressionGenerator reductionExpressionGenerator = null)
            : base(unaryParameter, reductionExpressionGenerator) {
            Contract.Requires(null != unaryParameter);
        }

        /// <inheritdoc/>
        public override Expression Reduce() {
            Contract.Ensures(Contract.Result<Expression>() != null);

            // TODO: should there be an optimization option?
            var squareExpression = UnaryParameter as SquareExpression;
            if (squareExpression != null)
                return squareExpression.UnaryParameter;

            return ReductionExpressionGenerator.BuildConversionCall(MathSqrtMethod, UnaryParameter, Type);
        }

    }
}
