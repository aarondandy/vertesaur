namespace Vertesaur
{
    /// <summary>
    /// Describes the point winding for a closed path or ring
    /// </summary>
    /// <remarks>
    /// Many Vertesaur algorithms use CounterClockwise as the standard point ordering or winding
    /// when describing fills for polygons and rings while other systems may use Clockwise.
    /// Winding must be considered when operating on polygons or rings.
    /// It is best to explicitly state if each individual ring or polygon is a fill or hole by
    /// using the <see cref="Vertesaur.Ring2.Hole"/> property of the <see cref="Vertesaur.Ring2"/> class
    /// or equivalent methods or properties.
    /// </remarks>
    /// <seealso cref="Vertesaur.Ring2"/>
    /// <seealso cref="Vertesaur.Polygon2"/>
    public enum PointWinding : byte
    {
        /// <summary>
        /// Winding is unknown
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// Clockwise point ordering
        /// </summary>
        Clockwise = 1,
        /// <summary>
        /// Counter clockwise point ordering
        /// </summary>
        CounterClockwise = 2
    }
}
