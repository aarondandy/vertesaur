namespace Vertesaur
{
    /// <summary>
    /// Provides functionality to calculate the centroid of an object.
    /// </summary>
    /// <typeparam name="TPoint">The centroid point result value type.</typeparam>
    /// <remarks>
    /// While it is not enforced it is recommended
    /// that the generic argument <typeparamref name="TPoint"/> be a type that
    /// represents a point.
    /// </remarks>
    public interface IHasCentroid<out TPoint>
    {
        /// <summary>
        /// Calculates the centroid.
        /// </summary>
        /// <returns>The calculated centroid.</returns>
        TPoint GetCentroid();

    }
}
