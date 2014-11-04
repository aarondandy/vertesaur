namespace Vertesaur
{
    /// <summary>
    /// A point in 2D space with two coordinate values: X and Y.
    /// </summary>
    /// <typeparam name="TValue">The coordinate value type.</typeparam>
    public interface IPoint2<out TValue> :
        IPlanarGeometry,
        ICoordinatePair<TValue>
    { }
}
