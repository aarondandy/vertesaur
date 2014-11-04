namespace Vertesaur
{
    /// <summary>
    /// A straight line of infinite length defined by a point and a direction.
    /// </summary>
    /// <typeparam name="TValue">The coordinate value type.</typeparam>
    public interface ILine2<out TValue> :
        IPlanarGeometry,
        IHasMagnitude<TValue>
    {
        /// <summary>
        /// A point on the line that is used to define the line.
        /// </summary>
        IPoint2<TValue> P { get; }

        /// <summary>
        /// The direction vector from <see cref="P"/> that defines the line.
        /// </summary>
        /// <remarks>
        /// The line extends from the defined point (P) in both
        /// this direction as well as the oposite direction.
        /// </remarks>
        IVector2<TValue> Direction { get; }
    }
}
