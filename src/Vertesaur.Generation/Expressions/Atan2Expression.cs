using System;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using System.Reflection;
using Vertesaur.Generation.Utility;
using Vertesaur.Utility;

namespace Vertesaur.Generation.Expressions
{
    /// <summary>
    /// An arc-tangent 2 expression accepting a y-coordinate and x-coordinate.
    /// </summary>
    public class Atan2Expression : ReducibleBinaryExpressionBase
    {
        private static readonly MethodInfo MathAtan2DoubleMethod;

        static Atan2Expression() {
            MathAtan2DoubleMethod = typeof(Math).GetPublicStaticInvokableMethod("Atan2", typeof(double), typeof(double));
        }

        /// <summary>
        /// Creates a new arc-tangent 2 expression.
        /// </summary>
        /// <param name="left">The y-coordinate expression.</param>
        /// <param name="right">The x-coordinate expression.</param>
        /// <param name="generator">The optional expression generator used during reduction.</param>
        public Atan2Expression(Expression left, Expression right, IExpressionGenerator generator = null)
            : base(left, right, generator) {
            Contract.Requires(null != left);
            Contract.Requires(null != right);
        }

        /// <inheritdoc/>
        public override Expression Reduce() {
            Contract.Ensures(Contract.Result<Expression>() != null);
            var gen = ReductionExpressionGenerator;

            if (
                (typeof (float) == LeftParameter.Type && typeof (float) == RightParameter.Type)
                || (typeof(double) == LeftParameter.Type && typeof(double) == RightParameter.Type)
            ) {
                Contract.Assume(MathAtan2DoubleMethod != null);
                return gen.BuildConversionCall(MathAtan2DoubleMethod, LeftParameter.Type, LeftParameter, RightParameter);
            }

            if (LeftParameter.IsMemoryLocationOrConstant() && RightParameter.IsMemoryLocationOrConstant())
                return GenerateAtan2Expressions(LeftParameter, RightParameter);

            return new BlockExpressionBuilder().AddUsingMemoryLocationsOrConstants(
                args => new[] { GenerateAtan2Expressions(args[0], args[1]) },
                LeftParameter, RightParameter
            ).GetExpression();
        }

        private Expression GenerateAtan2Expressions(Expression y, Expression x) {
            Contract.Requires(y != null);
            Contract.Requires(x != null);
            Contract.Ensures(Contract.Result<Expression>() != null);
            var gen = ReductionExpressionGenerator;

            var yxParams = new[] {y, x};
            var divYx = gen.GenerateOrThrow("DIVIDE", yxParams);
            var atanDivYx = gen.GenerateOrThrow("ATAN", divYx);
            var zeroY = gen.GenerateOrThrow("ZERO", y.Type);
            var zeroX = gen.GenerateOrThrow("ZERO", x.Type);
            var piX = gen.GenerateOrThrow("PI", x.Type);
            var halfPiY = gen.GenerateOrThrow("HALFPI", y.Type);

            var xEqualZero = Condition(
                gen.GenerateOrThrow("EQUAL", y, zeroY),
                // y == 0 && x == 0
                gen.Generate("INVALID", y.Type) ?? zeroY,
                Condition(
                    gen.GenerateOrThrow("GREATER", y, zeroY),
                // y > 0 && x == 0
                    halfPiY,
                // y < 0 && x == 0
                    gen.GenerateOrThrow("NEGATE", halfPiY)
                )
            );
            var xLessZero = Condition(
                gen.GenerateOrThrow("GREATEREQUAL", y, zeroX),
                // y >= 0 && x < 0
                gen.GenerateOrThrow("ADD", atanDivYx, piX),
                // y < 0 && x < 0
                gen.GenerateOrThrow("SUBTRACT", atanDivYx, piX)
            );
            var xLequalZero = Condition(
                gen.GenerateOrThrow("EQUAL", x, zeroX),
                xEqualZero,
                xLessZero
            );
            return Condition(
                gen.GenerateOrThrow("GREATER", x, zeroX),
                atanDivYx,
                xLequalZero
            );
        }

        /// <inheritdoc/>
        public override Type Type {
            get {
                Contract.Ensures(Contract.Result<Type>() != null);
                var t = LeftParameter.Type == RightParameter.Type
                    ? LeftParameter.Type
                    : Reduce().Type;
                return t;
            }
        }
    }
}
