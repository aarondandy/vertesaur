namespace Vertesaur
{
    /// <summary>
    /// Functionality to determine if two objects are disjoint.
    /// </summary>
    /// <typeparam name="TObject">The other object type.</typeparam>
    public interface IRelatableDisjoint<in TObject>
    {
        /// <summary>
        /// Determines if this object and some <paramref name="other"/> share no space.
        /// </summary>
        /// <param name="other">An object to test.</param>
        /// <returns>True if the objects do not occupy any of the same space.</returns>
        bool Disjoint(TObject other);
    }
}
