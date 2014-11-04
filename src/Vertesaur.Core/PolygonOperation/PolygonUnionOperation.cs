using System.Diagnostics.Contracts;

namespace Vertesaur.PolygonOperation
{

    /// <summary>
    /// An operation that will find the geometric union of two polygons.
    /// </summary>
    public class PolygonUnionOperation
    {

        private static readonly PolygonBinaryOperationOptions DefaultInverseIntersectionOptions;
        private static readonly PolygonIntersectionOperation DefaultInverseIntersectionOperation;

        static PolygonUnionOperation() {
            DefaultInverseIntersectionOptions = new PolygonBinaryOperationOptions {
                InvertLeftHandSide = true,
                InvertRightHandSide = true,
                InvertResult = true
            };
            DefaultInverseIntersectionOperation = new PolygonIntersectionOperation(DefaultInverseIntersectionOptions);
        }

        /// <summary>
        /// An inverted intersection operation that inverts both arguments as well as the result.
        /// </summary>
        protected PolygonIntersectionOperation InverseIntersectionOperation { get; private set; }

        /// <summary>
        /// Creates a new default polygon union operation.
        /// </summary>
        public PolygonUnionOperation() : this(null) { }

        internal PolygonUnionOperation(PolygonIntersectionOperation inverseIntersectionOperation) {
            InverseIntersectionOperation = inverseIntersectionOperation ?? DefaultInverseIntersectionOperation;
        }

        [ContractInvariantMethod]
        private void CodeContractInvariants(){
            Contract.Invariant(InverseIntersectionOperation != null);
        }

        /// <summary>
        /// Calculates the resulting union of two polygon geometries.
        /// </summary>
        /// <param name="a">A polygon.</param>
        /// <param name="b">A polygon.</param>
        /// <returns>The union of <paramref name="a"/> and <paramref name="b"/>.</returns>
        public IPlanarGeometry Union(Polygon2 a, Polygon2 b) {
            Contract.Ensures((a != null || b != null) || Contract.Result<IPlanarGeometry>() == null);
            if (null == a)
                return b;
            if (null == b)
                return a;

            var result = InverseIntersectionOperation.Intersect(a, b) as Polygon2;
            return result;
        }

    }
}
