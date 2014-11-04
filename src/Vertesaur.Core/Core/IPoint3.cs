namespace Vertesaur
{
    /// <summary>
    /// A point in 3D space with two coordinate values: X, Y and Z.
    /// </summary>
    /// <typeparam name="TValue">The element type.</typeparam>
    public interface IPoint3<out TValue> :
        I3DGeometry,
        ICoordinateTriple<TValue>
    { }
}
