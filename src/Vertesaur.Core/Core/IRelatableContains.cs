namespace Vertesaur
{
    /// <summary>
    /// Functionality to determine if another object can be completely within the interior of this object.
    /// </summary>
    /// <typeparam name="TObject">The other object type.</typeparam>
    public interface IRelatableContains<in TObject>
    {
        /// <summary>
        /// Determines if some <paramref name="other"/> object is completely within the interior of this object.
        /// </summary>
        /// <param name="other">An object to test.</param>
        /// <returns>True when this object contains the <paramref name="other"/>.</returns>
        bool Contains(TObject other);
    }
}
