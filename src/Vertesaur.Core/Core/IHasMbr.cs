namespace Vertesaur
{
    /// <summary>
    /// Provides functionality to calculate the minimum bounding rectangle (envelope) of an object.
    /// </summary>
    /// <typeparam name="TValue">The coordinate value type.</typeparam>
    /// <typeparam name="TMbr">The MBR type.</typeparam>
    public interface IHasMbr<out TMbr, out TValue>
        where TMbr : IMbr<TValue>
    {
        /// <summary>
        /// Calculates the minimum bounding rectangle for this instance.
        /// </summary>
        /// <returns>The minimum bounding rectangle.</returns>
        TMbr GetMbr();
    }
}
