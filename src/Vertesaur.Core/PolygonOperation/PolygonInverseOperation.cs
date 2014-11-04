using System.Diagnostics.Contracts;
using System.Linq;

namespace Vertesaur.PolygonOperation
{
    /// <summary>
    /// An operation that will 
    /// </summary>
    public static class PolygonInverseOperation
    {

        /// <summary>
        /// Inverts a polygon.
        /// </summary>
        /// <param name="polygon">The polygon to get the inverse of.</param>
        /// <returns>An inverted polygon.</returns>
        public static Polygon2 Invert(Polygon2 polygon) {
            Contract.Ensures(polygon == null ? Contract.Result<Polygon2>() == null : Contract.Result<Polygon2>() != null);
            return null == polygon ? null : new Polygon2(polygon.Select(Invert));
        }

        /// <summary>
        /// Inverts a ring.
        /// </summary>
        /// <param name="ring">The ring to get the inverse of.</param>
        /// <returns>An inverted polygon.</returns>
        public static Ring2 Invert(Ring2 ring) {
            Contract.Ensures(ring == null ? Contract.Result<Ring2>() == null : Contract.Result<Ring2>() != null);
            return null == ring ? null : ring.GetInverse();
        }
    }
}
