namespace Vertesaur
{

    /// <summary>
    /// Functionality to determine if another object intersects this object.
    /// </summary>
    /// <typeparam name="TObject">The object type to test intersection with.</typeparam>
    public interface IRelatableIntersects<in TObject>
    {
        /// <summary>
        /// Determines if this object and some <paramref name="other"/> share some space.
        /// </summary>
        /// <param name="other">An object to test.</param>
        /// <returns>True if the objects occupy any of the same space.</returns>
        bool Intersects(TObject other);

    }

}
