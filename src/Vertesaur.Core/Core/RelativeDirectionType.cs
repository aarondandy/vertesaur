namespace Vertesaur
{

    /// <summary>
    /// A relative direction based on context.
    /// </summary>
    public enum RelativeDirectionType : short
    {

        /// <summary>
        /// No direction. This could also represent the relative origin.
        /// </summary>
        None = 0,
        /// <summary>
        /// A direction to the right of the origin.
        /// </summary>
        Right = 1,
        /// <summary>
        /// A direction to the left of the origin.
        /// </summary>
        Left = -1,
        /// <summary>
        /// A direction above the origin.
        /// </summary>
        Up = 2,
        /// <summary>
        /// A direction below the origin.
        /// </summary>
        Down = -2,
        /// <summary>
        /// A direction in front of the origin.
        /// </summary>
        Front = 3,
        /// <summary>
        /// A direction behind the origin.
        /// </summary>
        Back = -3

    }

}
