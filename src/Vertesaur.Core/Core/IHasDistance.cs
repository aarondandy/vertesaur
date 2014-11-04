namespace Vertesaur
{
    /// <summary>
    /// Provides functionality to calculate the distance to another object.
    /// </summary>
    /// <typeparam name="TObject">The object type to find the distance to.</typeparam>
    /// <typeparam name="TValue">The distance result value type.</typeparam>
    public interface IHasDistance<in TObject, out TValue>
    {

        /// <summary>
        /// Calculates the distance between this instance and another.
        /// </summary>
        /// <param name="other">The object to calculate distance to.</param>
        /// <returns>The distance to the other object.</returns>
        /// <ensures csharp="!(result &lt; 0)">Result is not negative.</ensures>
        /// <remarks>
        /// If squared distance is required use <see cref="Vertesaur.IHasDistance{TObject,TValue}.DistanceSquared(TObject)"/>.
        /// </remarks>
        TValue Distance(TObject other);

        /// <summary>
        /// Calculates the squared distance between this instance and another.
        /// </summary>
        /// <param name="other">The object to calculate squared distance to.</param>
        /// <returns>The squared distance to the other object.</returns>
        /// <remarks>
        /// While most users of this library will be interested in the
        /// <see cref="Vertesaur.IHasDistance{TObject,TValue}.Distance(TObject)">distance</see>
        /// between two objects some algorithms and situations will require or
        /// benefit from the squared distance between them. The squared distance
        /// often requires less operations and may be able to avoid a call to the
        /// square-root function. For some situations such as ordering objects by
        /// their distance to another object the squared distance may offer some
        /// benefit.
        /// </remarks>
        /// <ensures csharp="!(result &lt; 0)">Result is not negative.</ensures>
        TValue DistanceSquared(TObject other);

    }
}
