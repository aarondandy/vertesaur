namespace Vertesaur
{

    /// <summary>
    /// A straight line segment of minimal magnitude between two points.
    /// </summary>
    /// <typeparam name="TValue">The element type.</typeparam>
    public interface ISegment2<out TValue> :
        IPlanarGeometry,
        IHasMagnitude<TValue>
    {
        /// <summary>
        /// A point at one end of the segment.
        /// </summary>
        IPoint2<TValue> A { get; }

        /// <summary>
        /// A point at the other end of the segment.
        /// </summary>
        IPoint2<TValue> B { get; }

    }
}
