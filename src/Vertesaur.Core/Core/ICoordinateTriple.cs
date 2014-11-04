namespace Vertesaur
{
    /// <summary>
    /// An ordered tuple with three coordinate values: X, Y, and Z.
    /// </summary>
    /// <typeparam name="TValue">The coordinate value type.</typeparam>
    public interface ICoordinateTriple<out TValue>
    {
        /// <summary>
        /// The x-coordinate of the coordinate triple.
        /// </summary>
        TValue X { get; }
        /// <summary>
        /// The y-coordinate of the coordinate triple.
        /// </summary>
        TValue Y { get; }
        /// <summary>
        /// The z-coordinate of the coordinate triple.
        /// </summary>
        TValue Z { get; }
    }
}
