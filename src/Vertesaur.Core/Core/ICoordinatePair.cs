namespace Vertesaur
{
    /// <summary>
    /// An ordered tuple with two coordinate values: X and Y.
    /// </summary>
    /// <typeparam name="TValue">The coordinate value type.</typeparam>
    public interface ICoordinatePair<out TValue>
    {
        /// <summary>
        /// The x-coordinate of the coordinate pair.
        /// </summary>
        TValue X { get; }
        /// <summary>
        /// The y-coordinate of the coordinate pair.
        /// </summary>
        TValue Y { get; }
    }
}
