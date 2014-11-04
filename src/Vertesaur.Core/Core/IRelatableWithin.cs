namespace Vertesaur
{
    /// <summary>
    /// Functionality to determine if an object can be contained within another object.
    /// </summary>
    /// <typeparam name="TObject">The object type which may contain this instance.</typeparam>
    public interface IRelatableWithin<in TObject>
    {
        /// <summary>
        /// Determines if the given object contains this instance and this instance
        /// does not intersect the <paramref name="other"/> exterior.
        /// </summary>
        /// <param name="other">The object to test.</param>
        /// <returns>True if this instance is within the given object.</returns>
        /// <remarks>
        /// An object is within another if there is an intersection between interiors
        /// and there is no intersection between the interior of this instance and the
        /// exterior of the other instance.
        /// </remarks>
        bool Within(TObject other);
    }
}
