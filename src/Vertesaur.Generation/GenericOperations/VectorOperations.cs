using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using Vertesaur.Generation.Expressions;
using Vertesaur.Generation.Utility;

namespace Vertesaur.Generation.GenericOperations
{
    /// <summary>
    /// Vector related generic operations that are generated at run-time.
    /// </summary>
    /// <typeparam name="TValue">The value type the operations are primarily for.</typeparam>
    public class VectorOperations<TValue>
    {

        static VectorOperations() {
            _default = new VectorOperations<TValue>(
#if !NO_MEF
                new MefCombinedExpressionGenerator()
#else
                CombinedExpressionGenerator.GenerateDefaultMefReplacement()
#endif
            );
        }

        /// <summary>
        /// The default vector operation implementation.
        /// </summary>
        public static VectorOperations<TValue> Default {
            get {
                Contract.Ensures(Contract.Result<VectorOperations<TValue>>() != null);
                return _default;
            }
        }

        /// <summary>
        /// A delegate for methods that reduce a 2D coordinate down to a single result value of the same component type.
        /// </summary>
        /// <param name="x">The x-coordinate.</param>
        /// <param name="y">The y-coordinate.</param>
        /// <returns>The result value.</returns>
        public delegate TValue CoordinateToValue2D(TValue x, TValue y);

        /// <summary>
        /// A delegate for methods that reduce two 2D coordinates down to a single result value of the same component type.
        /// </summary>
        /// <param name="x0">The first x-coordinate.</param>
        /// <param name="y0">The first y-coordinate.</param>
        /// <param name="x1">The second x-coordinate.</param>
        /// <param name="y1">The second y-coordinate.</param>
        /// <returns>The result value.</returns>
        public delegate TValue TwoCoordinateToValue2D(TValue x0, TValue y0, TValue x1, TValue y1);

        /// <summary>
        /// Creates a new vector operation implementation using the given expression generator.
        /// </summary>
        /// <param name="expressionGenerator">The expression generator which is used to generate expression for the generic operations at run-time.</param>
        public VectorOperations(IExpressionGenerator expressionGenerator) {
            if (null == expressionGenerator) throw new ArgumentNullException("expressionGenerator");
            Contract.EndContractBlock();
            ExpressionGenerator = expressionGenerator;

            Magnitude2D = CreateCoordinateToValue2D("Magnitude");
            SquaredMagnitude2D = CreateCoordinateToValue2D("SquaredMagnitude");
            DotProduct2D = CreateTwoCoordinateToValue2D("DotProduct");
            PerpendicularDotProduct2D = CreatePerpendicularDotProduct2D();
            Distance2D = CreateTwoCoordinateToValue2D("Distance");
            DistanceSquared2D = CreateTwoCoordinateToValue2D("SquaredDistance");
        }

        [ContractInvariantMethod]
        [Conditional("CONTRACTS_FULL")]
        private void CodeContractInvariant() {
            Contract.Invariant(ExpressionGenerator != null);
        }

        private TwoCoordinateToValue2D CreatePerpendicularDotProduct2D() {
            var x0 = typeof(TValue).CreateParameterExpression();
            var y0 = typeof(TValue).CreateParameterExpression();
            var x1 = typeof(TValue).CreateParameterExpression();
            var y1 = typeof(TValue).CreateParameterExpression();

            var mula = ExpressionGenerator.Generate("Multiply", x0, y1);
            var mulb = ExpressionGenerator.Generate("Multiply", y0, x1);
            if (mula == null || mulb == null)
                return null;

            var expression = ExpressionGenerator.Generate("Subtract", mula, mulb);
            if (expression == null)
                return null;
            return Expression.Lambda<TwoCoordinateToValue2D>(
                expression, x0, y0, x1, y1
            ).Compile();
        }

        private CoordinateToValue2D CreateCoordinateToValue2D(string expressionName) {
            Contract.Requires(!string.IsNullOrEmpty(expressionName));
            var tParam0 = typeof(TValue).CreateParameterExpression();
            var tParam1 = typeof(TValue).CreateParameterExpression();
            var expression = ExpressionGenerator.Generate(expressionName, tParam0, tParam1);
            if (null == expression)
                return null;
            return Expression.Lambda<CoordinateToValue2D>(
                expression, tParam0, tParam1
            ).Compile();
        }

        private TwoCoordinateToValue2D CreateTwoCoordinateToValue2D(string expressionName) {
            Contract.Requires(!string.IsNullOrEmpty(expressionName));
            var tParam0 = typeof(TValue).CreateParameterExpression();
            var tParam1 = typeof(TValue).CreateParameterExpression();
            var tParam2 = typeof(TValue).CreateParameterExpression();
            var tParam3 = typeof(TValue).CreateParameterExpression();
            var expression = ExpressionGenerator.Generate(expressionName, tParam0, tParam1, tParam2, tParam3);
            if (null == expression)
                return null;
            return Expression.Lambda<TwoCoordinateToValue2D>(
                expression, tParam0, tParam1, tParam2, tParam3
            ).Compile();
        }

        /// <summary>
        /// The expression generator that was used to create the run-time executable generic functions.
        /// </summary>
        public IExpressionGenerator ExpressionGenerator { get; private set; }

        /// <summary>
        /// Calculates the magnitude of a 2D vector.
        /// </summary>
        public readonly CoordinateToValue2D Magnitude2D;
        /// <summary>
        /// Calculates the squared magnitude of a 2D vector.
        /// </summary>
        public readonly CoordinateToValue2D SquaredMagnitude2D;
        /// <summary>
        /// Calculates the distance between two points.
        /// </summary>
        public readonly TwoCoordinateToValue2D Distance2D;
        /// <summary>
        /// Calculated the squared distance between two points.
        /// </summary>
        public readonly TwoCoordinateToValue2D DistanceSquared2D;

        /// <summary>
        /// Calculates the dot product of two 2D vectors.
        /// </summary>
        public readonly TwoCoordinateToValue2D DotProduct2D;
        /// <summary>
        /// Calculates the perpendicular dot product of two 2D vectors.
        /// </summary>
        public readonly TwoCoordinateToValue2D PerpendicularDotProduct2D;

        private static VectorOperations<TValue> _default;
    }
}
