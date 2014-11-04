namespace Vertesaur
{
    /// <summary>
    /// Functionality to determine if this object touches another.
    /// </summary>
    /// <typeparam name="TObject">The other object type.</typeparam>
    public interface IRelatableTouches<in TObject>
    {
        /// <summary>
        /// Determines if this object and some <paramref name="other"/> share only
        /// boundaries.
        /// </summary>
        /// <param name="other">An object.</param>
        /// <returns>True when only boundaries occupy the same space.</returns>
        /// <remarks>
        /// Two objects are touching only when they intersect only at their boundaries
        /// and there is no intersection between interior regions. At least one object
        /// must have a dimensionality greater than 0, meaning both can not be points.
        /// </remarks>
        bool Touches(TObject other);
    }
}
