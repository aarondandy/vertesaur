namespace Vertesaur
{
    /// <summary>
    /// A straight ray of infinite length emanating from a point in a specific direction.
    /// </summary>
    /// <typeparam name="TValue">The element type.</typeparam>
    public interface IRay2<out TValue> :
        IPlanarGeometry,
        IHasMagnitude<TValue>
    {
        /// <summary>
        /// The point of origin for the ray.
        /// </summary>
        IPoint2<TValue> P { get; }

        /// <summary>
        /// The direction of the ray from the point of origin, <see cref="P"/>.
        /// </summary>
        IVector2<TValue> Direction { get; }
    }
}
