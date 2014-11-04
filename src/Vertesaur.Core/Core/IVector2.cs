namespace Vertesaur
{
    /// <summary>
    /// A vector within 2D space.
    /// </summary>
    /// <typeparam name="TValue">The element type.</typeparam>
    public interface IVector2<out TValue> :
        IPlanarGeometry,
        ICoordinatePair<TValue>,
        IHasMagnitude<TValue>
    {
    }
}
