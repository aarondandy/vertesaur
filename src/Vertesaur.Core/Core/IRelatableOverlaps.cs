namespace Vertesaur
{
    /// <summary>
    /// Functionality to determine if another object can be overlapped by this object.
    /// </summary>
    /// <typeparam name="TObject">The other object type.</typeparam>
    public interface IRelatableOverlaps<in TObject>
    {
        /// <summary>
        /// Determines if this object and some <paramref name="other"/> share space at
        /// the highest dimension of the two.
        /// </summary>
        /// <param name="other">An object.</param>
        /// <returns>True when the objects share space of the same dimension.
        /// </returns>
        /// <remarks>
        /// Take care with the definition of this operation as it is a bit confusing.
        /// While this operation may seem a lot like intersects at first it has a
        /// major and very important difference. This operation returns true only when
        /// the dimensionality of the result of intersecting the two objects is equal
        /// to the larger of the dimensionalities of this object and another.
        /// 
        /// Take for example a line and a square in 2D space where the line intersects
        /// the square such that the intersection of the two results in a segment. It
        /// is important to understand that while all three objects (the square, line,
        /// and resulting segment) exist in 2D space only the square has a
        /// dimensionality of 2 while the line and segment each have a dimensionality
        /// of 1. For the objects to overlap in this example the dimensionality of the
        /// result (1) must match the largest dimensionality of the inputs (1 and 2).
        /// While the objects share some space the dimensionality of the result is
        /// less than 2 so the objects to not overlap. Another failing case would be
        /// two squares that share only one edge.
        /// 
        /// NOTE: you may get into some awkward situations where a square has an area
        /// of 0 for example
        /// </remarks>
        bool Overlaps(TObject other);
    }
}
