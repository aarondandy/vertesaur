using System;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using System.Reflection;
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
            if (typeof(double) == LeftParameter.Type && typeof(double) == RightParameter.Type)
                return Call(MathAtan2DoubleMethod, LeftParameter, RightParameter);
            if (typeof (float) == LeftParameter.Type && typeof (float) == RightParameter.Type) {
                var call = Call(MathAtan2DoubleMethod,
                    ReductionExpressionGenerator.GenerateConversionExpression(typeof (double), LeftParameter),
                    ReductionExpressionGenerator.GenerateConversionExpression(typeof (double), RightParameter)
                );
                Contract.Assume(call != null);
                return ReductionExpressionGenerator.GenerateConversionExpression(typeof (float), call);
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

            var xGreaterZero = gen.Generate("ATAN", gen.Generate("DIVIDE", y, x));
            var xEqualZero = Condition(
                gen.Generate("EQUAL", y, gen.Generate("ZERO", y.Type)),
                // y == 0 && x == 0
                gen.Generate("INVALID", y.Type) ?? gen.Generate("ZERO", y.Type),
                Condition(
                    gen.Generate("GREATER", y, gen.Generate("ZERO", y.Type)),
                // y > 0 && x == 0
                    gen.Generate("HALFPI", y.Type),
                // y < 0 && x == 0
                    gen.Generate("NEGATE", gen.Generate("HALFPI", y.Type))
                )
            );
            var xLessZero = Condition(
                gen.Generate("GREATEREQUAL", y, gen.Generate("ZERO", x.Type)),
                // y >= 0 && x < 0
                gen.Generate("ADD",
                    gen.Generate("ATAN", gen.Generate("DIVIDE", y, x)),
                    gen.Generate("PI", x.Type)),
                // y < 0 && x < 0
                gen.Generate("SUBTRACT",
                    gen.Generate("ATAN", gen.Generate("DIVIDE", y, x)),
                    gen.Generate("PI", x.Type))
            );
            var xLequalZero = Condition(
                gen.Generate("EQUAL", x, gen.Generate("ZERO", x.Type)),
                xEqualZero,
                xLessZero
            );

            return Condition(
                gen.Generate("GREATER", x, gen.Generate("ZERO", x.Type)),
                xGreaterZero,
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
