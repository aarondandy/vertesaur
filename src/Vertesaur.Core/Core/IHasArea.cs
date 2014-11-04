namespace Vertesaur
{

    /// <summary>
    /// Provides functionality to calculate the area of an object.
    /// </summary>
    /// <typeparam name="TValue">The area result value type.</typeparam>
    public interface IHasArea<out TValue>
    {
        /// <summary>
        /// Calculates the area or surface area.
        /// </summary>
        /// <returns>The calculated area.</returns>
        /// <remarks>
        /// While it is not common the result of this method could be a negative or invalid value.
        /// </remarks>
        TValue GetArea();
    }

}
