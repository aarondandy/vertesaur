namespace Vertesaur
{

    /// <summary>
    /// Provides functionality to calculate the magnitude of an object.
    /// </summary>
    /// <typeparam name="TValue">The magnitude result value type.</typeparam>
    public interface IHasMagnitude<out TValue>
    {

        /// <summary>
        /// Calculates the magnitude of this instance.
        /// </summary>
        /// <returns>The magnitude.</returns>
        /// <remarks>
        /// <para>
        /// If squared magnitude is required use <see cref="Vertesaur.IHasMagnitude{TValue}.GetMagnitudeSquared()"/>.
        /// </para>
        /// <para>
        /// Note that this could also represent the length of an object or the perimeter of an object.
        /// </para>
        /// </remarks>
        TValue GetMagnitude();
        /// <summary>
        /// Calculates the squared magnitude of this instance.
        /// </summary>
        /// <returns>The squared magnitude.</returns>
        /// <remarks>
        /// <para>
        /// While most users of this library will be interested in the
        /// <see cref="Vertesaur.IHasMagnitude{TValue}.GetMagnitude()">magnitude</see>
        /// or length of an object some algorithms and situations will require or
        /// benefit from the squared magnitude of an object. The squared magnitude
        /// often requires less operations and may be able to avoid a call to the
        /// square-root function. For some situations such as ordering objects by
        /// magnitude the squared magnitude may offer some benefit.
        /// </para>
        /// <para>
        /// Note that this could also represent the squared length of an object or the squared perimeter of an object.
        /// </para>
        /// </remarks>
        TValue GetMagnitudeSquared();

    }

}
