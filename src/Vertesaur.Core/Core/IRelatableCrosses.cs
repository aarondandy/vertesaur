namespace Vertesaur
{
    /// <summary>
    /// Functionality to determine if this object crosses another.
    /// </summary>
    /// <typeparam name="TObject">The other object type.</typeparam>
    public interface IRelatableCrosses<in TObject>
    {
        /// <summary>
        /// Determines if this object crosses some <paramref name="other"/>.
        /// </summary>
        /// <param name="other">An object to test.</param>
        /// <returns>True if this object crosses the <paramref name="other"/>.</returns>
        bool Crosses(TObject other);
    }
}
