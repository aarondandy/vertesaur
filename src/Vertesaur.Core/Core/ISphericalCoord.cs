namespace Vertesaur
{

    /// <summary>
    /// A spherical coordinate in 3D space.
    /// </summary>
    /// <typeparam name="TValue">The element type.</typeparam>
    public interface ISphericalCoordinate<out TValue> :
        I3DGeometry,
        IHasMagnitude<TValue>
    {

        /// <summary>
        /// Rho value.
        /// </summary>
        TValue Rho { get; }
        /// <summary>
        /// Theta value.
        /// </summary>
        TValue Theta { get; }
        /// <summary>
        /// Phi value.
        /// </summary>
        TValue Phi { get; }

    }
}
