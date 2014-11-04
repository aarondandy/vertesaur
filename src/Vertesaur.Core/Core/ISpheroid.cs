namespace Vertesaur
{
    /// <summary>
    /// A spheroid.
    /// </summary>
    /// <typeparam name="TValue">The element type.</typeparam>
    public interface ISpheroid<out TValue>
    {
        /// <summary>
        /// The semi-major axis.
        /// </summary>
        TValue A { get; }
        /// <summary>
        /// The semi-minor axis.
        /// </summary>
        TValue B { get; }
        /// <summary>
        /// The flattening value.
        /// </summary>
        TValue F { get; }
        /// <summary>
        /// The inverse flattening value.
        /// </summary>
        TValue InvF { get; }
        /// <summary>
        /// First eccentricity.
        /// </summary>
        TValue E { get; }
        /// <summary>
        /// First eccentricity squared.
        /// </summary>
        TValue ESquared { get; }
        /// <summary>
        /// Second eccentricity.
        /// </summary>
        TValue ESecond { get; }
        /// <summary>
        /// Second eccentricity squared.
        /// </summary>
        TValue ESecondSquared { get; }

    }
}
