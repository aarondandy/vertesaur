using System.Diagnostics.Contracts;

namespace Vertesaur.PolygonOperation
{
    /// <summary>
    /// An operation that will find the geometric difference of one polygon from another.
    /// </summary>
    public class PolygonDifferenceOperation
    {

        private static readonly PolygonBinaryOperationOptions DefaultInverseRightIntersectionOptions;
        private static readonly PolygonIntersectionOperation DefaultInverseRightIntersectionOperation;

        static PolygonDifferenceOperation() {
            DefaultInverseRightIntersectionOptions = new PolygonBinaryOperationOptions {
                InvertLeftHandSide = false,
                InvertRightHandSide = true,
                InvertResult = false
            };
            DefaultInverseRightIntersectionOperation = new PolygonIntersectionOperation(DefaultInverseRightIntersectionOptions);
        }

        /// <summary>
        /// An operation used to invert the right argument.
        /// </summary>
        protected PolygonIntersectionOperation RightInverseIntersectionOperation { get; private set; }

        /// <summary>
        /// Creates a default polygon difference operation.
        /// </summary>
        public PolygonDifferenceOperation() : this(null) { }

        internal PolygonDifferenceOperation(PolygonIntersectionOperation inverseRightOperation) {
            RightInverseIntersectionOperation = inverseRightOperation ?? DefaultInverseRightIntersectionOperation;
        }

        [ContractInvariantMethod]
        private void CodeContractInvariants(){
            Contract.Invariant(RightInverseIntersectionOperation != null);
        }

        /// <summary>
        /// Calculates the resulting difference of polygon <paramref name="b"/> subtracted from polygon <paramref name="a"/>.
        /// </summary>
        /// <param name="a">The polygon to be subtracted from.</param>
        /// <param name="b">The polygon used to subtract from a.</param>
        /// <returns>The difference resulting from subtracting <paramref name="b"/> from <paramref name="a"/>.</returns>
        public IPlanarGeometry Difference(Polygon2 a, Polygon2 b) {
            var result = RightInverseIntersectionOperation.Intersect(a, b);
            return result;
        }

    }
}
