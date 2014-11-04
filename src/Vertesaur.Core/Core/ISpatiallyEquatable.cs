namespace Vertesaur
{
    /// <summary>
    /// Functionality to determine if an object is geometrically equivalent to this instance.
    /// </summary>
    /// <typeparam name="TObject">The other object type.</typeparam>
    public interface ISpatiallyEquatable<in TObject>
    {
        /// <summary>
        /// Determines if this object and some <paramref name="other"/> are geometrically equivalent.
        /// </summary>
        /// <param name="other">An object.</param>
        /// <returns>True when geometrically equivalent.</returns>
        bool SpatiallyEqual(TObject other);
    }
}
