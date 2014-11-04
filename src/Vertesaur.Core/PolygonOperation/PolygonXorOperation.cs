namespace Vertesaur.PolygonOperation
{
    /// <summary>
    /// An operation that will find the symmetric difference for two polygons.
    /// Also known as exclusive-or (XOR).
    /// </summary>
    public class PolygonXorOperation
    {

        private static readonly PolygonDifferenceOperation DefaultDifferenceOperation;
        private static readonly PolygonUnionOperation DefaultUnionOperation;

        static PolygonXorOperation() {
            DefaultDifferenceOperation = new PolygonDifferenceOperation();
            DefaultUnionOperation = new PolygonUnionOperation();
        }

        private readonly PolygonDifferenceOperation _differenceOperation;
        private readonly PolygonUnionOperation _unionOperation;

        /// <summary>
        /// Constructs a default symmetric difference operation.
        /// </summary>
        public PolygonXorOperation()
            : this(null, null) { }

        internal PolygonXorOperation(PolygonDifferenceOperation differenceOperation, PolygonUnionOperation unionOperation) {
            _differenceOperation = differenceOperation ?? DefaultDifferenceOperation;
            _unionOperation = unionOperation ?? DefaultUnionOperation;
        }

        /// <summary>
        /// Calculates the symmetric difference between two polygons.
        /// </summary>
        /// <param name="a">A polygon.</param>
        /// <param name="b">A polygon.</param>
        /// <returns>The symmetric difference of <paramref name="a"/> and <paramref name="b"/>.</returns>
        public IPlanarGeometry Xor(Polygon2 a, Polygon2 b) {
            if (null == a)
                return b;
            if (null == b)
                return a;

            var removedFromA = _differenceOperation.Difference(a, b) as Polygon2;
            var removedFromB = _differenceOperation.Difference(b, a) as Polygon2;
            var unionedLeftovers = _unionOperation.Union(removedFromA, removedFromB);
            return unionedLeftovers;
        }

    }
}
