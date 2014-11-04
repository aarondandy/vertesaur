namespace Vertesaur
{
    /// <summary>
    /// A vector within 3D space.
    /// </summary>
    /// <typeparam name="TValue">The element type.</typeparam>
    public interface IVector3<out TValue> :
        I3DGeometry,
        ICoordinateTriple<TValue>,
        IHasMagnitude<TValue>
    {
    }
}