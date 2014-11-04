namespace Vertesaur
{
    /// <summary>
    /// Provides functionality to calculate the intersection with another object.
    /// </summary>
    /// <typeparam name="TObject">The object type to calculate the intersection with.</typeparam>
    /// <typeparam name="TResult">the intersection result type.</typeparam>
    public interface IHasIntersectionOperation<in TObject, out TResult>
    {
        /// <summary>
        /// Calculates the intersection between this instance and <paramref name="other">another</paramref>.
        /// </summary>
        /// <param name="other">The object to calculate the intersection with.</param>
        /// <returns>The intersection result.</returns>
        TResult Intersection(TObject other);
    }
}
