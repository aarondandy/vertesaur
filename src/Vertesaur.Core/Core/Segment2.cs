using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using Vertesaur.SegmentOperation;

namespace Vertesaur
{

    /// <summary>
    /// A straight line segment of minimal length between two points.
    /// </summary>
    /// <seealso cref="Vertesaur.Line2"/>
    public sealed class Segment2 :
        ISegment2<double>,
        IEquatable<Segment2>,
        IHasDistance<Segment2, double>,
        IRelatableIntersects<Segment2>,
        IRelatableIntersects<Ray2>,
        IRelatableIntersects<Line2>,
        IRelatableIntersects<Point2>,
        ISpatiallyEquatable<Segment2>,
        IHasIntersectionOperation<Segment2, IPlanarGeometry>,
        IHasIntersectionOperation<Ray2, IPlanarGeometry>,
        IHasIntersectionOperation<Line2, IPlanarGeometry>,
        IHasIntersectionOperation<Point2, IPlanarGeometry>,
        IHasDistance<Point2, double>,
        IHasCentroid<Point2>,
        IHasMbr<Mbr, double>,
        IComparable<Segment2>,
        ICloneable
    {

        /// <summary>
        /// Determines the distance between a point <paramref name="p"/> and a line segment defined by <paramref name="a"/> and <paramref name="b"/>.
        /// </summary>
        /// <param name="a">An end point of a segment.</param>
        /// <param name="b">An end point of a segment.</param>
        /// <param name="p">A point.</param>
        /// <returns>Distance from a point to a line segment.</returns>
        [Pure] public static double Distance(Point2 a, Point2 b, Point2 p) {
            Contract.Ensures(
                Contract.Result<double>() >= 0.0
                || Double.IsNaN(Contract.Result<double>())
                || Double.IsPositiveInfinity(Contract.Result<double>()));
            Point2.Order(ref a, ref b);
            var d = b - a;
            var v = p - a;

            var aDot = d.Dot(v);
            if (aDot <= 0)
                return v.GetMagnitude();

            var dMag = d.GetMagnitudeSquared();
            return (
                (aDot >= dMag)
                ? (p - b).GetMagnitude()
                : Math.Sqrt(Math.Max(0, v.GetMagnitudeSquared() - ((aDot * aDot) / dMag)))
            );
        }

        /// <summary>
        /// Determines the squared distance between a point <paramref name="p"/> and a line segment defined by <paramref name="a"/> and <paramref name="b"/>.
        /// </summary>
        /// <param name="a">An end point of a segment.</param>
        /// <param name="b">An end point of a segment.</param>
        /// <param name="p">A point.</param>
        /// <returns>Squared distance from a point to a line segment.</returns>
        [Pure] public static double DistanceSquared(Point2 a, Point2 b, Point2 p) {
            Contract.Ensures(
                Contract.Result<double>() >= 0.0
                || Double.IsNaN(Contract.Result<double>())
                || Double.IsPositiveInfinity(Contract.Result<double>()));
            Point2.Order(ref a, ref b);
            var d = b - a;
            var v = p - a;

            var aDot = d.Dot(v);
            if (aDot <= 0)
                return v.GetMagnitudeSquared();

            var dMag = d.GetMagnitudeSquared();
            return (
                (aDot >= dMag)
                ? (p - b).GetMagnitudeSquared()
                : Math.Max(0, v.GetMagnitudeSquared() - ((aDot * aDot) / dMag))
            );
        }

        /// <summary>
        /// Determines the if a point <paramref name="p"/> intersects a line segment defined by <paramref name="a"/> and <paramref name="b"/>.
        /// </summary>
        /// <param name="a">An end point of a segment.</param>
        /// <param name="b">An end point of a segment.</param>
        /// <param name="p">A point.</param>
        /// <returns>True when a point intersects a line segment.</returns>
        [Pure] public static bool Intersects(Point2 a, Point2 b, Point2 p) {
            if (p.Equals(a) || p.Equals(b)) {
                return true;
            }
            Point2.Order(ref a, ref b);
            var d = b - a;
            var v = p - a;
            var aDot = d.Dot(v);
            return (
                (aDot <= 0)
                ? (0 == v.X && 0 == v.Y)
                : (
                    !(aDot >= d.GetMagnitudeSquared())
                    && (d.X * v.Y) - (d.Y * v.X) == 0.0
                )
            );
        }

        /// <summary>
        /// Determines if two segments composed of the given points intersect.
        /// </summary>
        /// <param name="a">An end point on the first segment.</param>
        /// <param name="b">Another end point on the first segment.</param>
        /// <param name="c">An end point on the second segment.</param>
        /// <param name="d">Another end point on the second segment.</param>
        /// <returns><c>true</c> when the two segments intersect.</returns>
        public static bool Intersects(Point2 a, Point2 b, Point2 c, Point2 d) {
            return Intersection(a, b, c, d) != null; // TODO: optimize
        }

        /// <summary>
        /// Calculates the geometry resulting from the intersection of two segments.
        /// </summary>
        /// <param name="a">An end point on the first segment.</param>
        /// <param name="b">Another end point on the first segment.</param>
        /// <param name="c">An end point on the second segment.</param>
        /// <param name="d">Another end point on the second segment.</param>
        /// <returns>The resulting intersection geometry.</returns>
        public static IPlanarGeometry Intersection(Point2 a, Point2 b, Point2 c, Point2 d) {
            Point2.SegmentOrder(ref a, ref b, ref c, ref d);
            return SegmentIntersectionOperation.Intersection(a, b, c, d);
        }

        /// <summary>
        /// Compares two segments defined by their end points.
        /// </summary>
        /// <param name="a">A point on the first segment.</param>
        /// <param name="b">A point on the first segment.</param>
        /// <param name="c">A point on the second segment.</param>
        /// <param name="d">A point on the second segment.</param>
        /// <returns>A comparison value.</returns>
        [Obsolete]
        public static int Compare(Point2 a, Point2 b, Point2 c, Point2 d) {
            Point2 aSmall;
            Point2 aLarge;
            Point2 bSmall;
            Point2 bLarge;
            if (a.CompareTo(b) <= 0) {
                aSmall = a;
                aLarge = b;
            }
            else {
                aSmall = b;
                aLarge = a;
            }
            if (c.CompareTo(d) <= 0) {
                bSmall = c;
                bLarge = d;
            }
            else {
                bSmall = d;
                bLarge = c;
            }
            var compareResult = aSmall.CompareTo(bSmall);
            return 0 != compareResult
                ? compareResult
                : aLarge.CompareTo(bLarge);
        }

        /// <summary>
        /// The first point defining an end of the line segment.
        /// </summary>
        public readonly Point2 A;

        /// <summary>
        /// The second point defining the other end of the line segment.
        /// </summary>
        public readonly Point2 B;

        /// <summary>
        /// Constructs a segment between two points.
        /// </summary>
        /// <param name="a">A point.</param>
        /// <param name="b">A point.</param>
        public Segment2(Point2 a, Point2 b) {
            A = a;
            B = b;
        }
        /// <summary>
        /// Constructs a segment between two points.
        /// </summary>
        /// <param name="a">A point.</param>
        /// <param name="b">A point.</param>
        public Segment2(IPoint2<double> a, IPoint2<double> b)
            : this(new Point2(a), new Point2(b)) {
            Contract.Requires(a != null);
            Contract.Requires(b != null);
        }

        /// <summary>
        /// Constructs a segment from the given point <paramref name="p"/> and following a given vector <paramref name="d"/>.
        /// </summary>
        /// <param name="p">A point.</param>
        /// <param name="d">A direction.</param>
        public Segment2(Point2 p, Vector2 d) {
            A = p;
            B = p + d;
        }
        /// <summary>
        /// Constructs a segment from the given point <paramref name="p"/> and following a given vector <paramref name="d"/>.
        /// </summary>
        /// <param name="p">A point.</param>
        /// <param name="d">A direction.</param>
        public Segment2(IPoint2<double> p, IVector2<double> d)
            : this(new Point2(p), new Vector2(d)) {
            Contract.Requires(p != null);
            Contract.Requires(d != null);
        }
        /// <summary>
        /// Creates a new segment using the same end points as the given <paramref name="segment"/>.
        /// </summary>
        /// <param name="segment">The segment to copy from.</param>
        public Segment2(Segment2 segment) {
            if (segment == null) throw new ArgumentNullException("segment");
            Contract.EndContractBlock();
            A = segment.A;
            B = segment.B;
        }

        /// <summary>
        /// The direction of the line segment from A to B with the same magnitude.
        /// </summary>
        public Vector2 Direction {
            get { return B.Difference(A); }
        }

        /// <inheritdoc/>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        IPoint2<double> ISegment2<double>.A { get { return A; } }

        /// <inheritdoc/>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        IPoint2<double> ISegment2<double>.B { get { return B; } }

        /// <inheritdoc/>
        public bool Equals(Segment2 other) {
            return !ReferenceEquals(null, other)
                && A.Equals(other.A)
                && B.Equals(other.B);
        }

        /// <summary>
        /// Determines if this segment is geometrically equal to another. 
        /// </summary>
        /// <param name="other">Another ray</param>
        /// <returns><c>true</c> if the segments are spatially equal.</returns>
        public bool SpatiallyEqual(Segment2 other) {
            return !ReferenceEquals(null, other) && (
                A.Equals(other.A)
                ? B.Equals(other.B)
                : (A.Equals(other.B) && B.Equals(other.A))
            );
        }

        /// <inheritdoc/>
        public override bool Equals(object obj) {
            return Equals(obj as Segment2);
        }

        /// <inheritdoc/>
        public override int GetHashCode() {
            return (A.CompareTo(B) < 0 ? A.GetHashCode() : B.GetHashCode()) ^ -3939399;
        }

        /// <inheritdoc/>
        public override string ToString() {
            Contract.Ensures(!String.IsNullOrEmpty(Contract.Result<string>()));
            var result = String.Concat('(', A, ") (", B, ')');
            Contract.Assume(!String.IsNullOrEmpty(result));
            return result;
        }

        /// <summary>
        /// Calculates the length of this segment.
        /// </summary>
        /// <returns>The length.</returns>
        public double GetMagnitude() {
            Contract.Ensures(
                Contract.Result<double>() >= 0.0
                || Double.IsNaN(Contract.Result<double>())
                || Double.IsPositiveInfinity(Contract.Result<double>()));
            return A.Distance(B);
        }

        /// <summary>
        /// Calculates the squared length of this segment.
        /// </summary>
        /// <returns>The squared length.</returns>
        public double GetMagnitudeSquared() {
            Contract.Ensures(
                Contract.Result<double>() >= 0.0
                || Double.IsNaN(Contract.Result<double>())
                || Double.IsPositiveInfinity(Contract.Result<double>()));
            return A.DistanceSquared(B);
        }

        /// <summary>
        /// Creates a new segment with the same end points as this segment.
        /// </summary>
        /// <returns>
        /// A new identical segment.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        /// <remarks>Functions as a deep clone.</remarks>
        public Segment2 Clone() {
            Contract.Ensures(Contract.Result<Segment2>() != null);
            return new Segment2(this);
        }

        object ICloneable.Clone() {
            return Clone();
        }

        /// <summary>
        /// Calculates a minimum bounding rectangle for this segment.
        /// </summary>
        /// <returns>A minimum bounding rectangle.</returns>
        public Mbr GetMbr() {
            Contract.Ensures(Contract.Result<Mbr>() != null);
            return new Mbr(A, B);
        }

        /// <summary>
        /// Calculates the centroid.
        /// </summary>
        /// <returns>A centroid.</returns>
        public Point2 GetCentroid() {
            return new Point2(
                (A.X + B.X) / 2.0,
                (A.Y + B.Y) / 2.0
            );
        }

        /// <summary>
        /// Calculates the distance between this segment and <paramref name="p"/>
        /// </summary>
        /// <param name="p">The point to calculate distance to.</param>
        /// <returns>The distance.</returns>
        public double Distance(Point2 p) {
            Contract.Ensures(
                Contract.Result<double>() >= 0.0
                || Double.IsNaN(Contract.Result<double>())
                || Double.IsPositiveInfinity(Contract.Result<double>()));
            return Distance(A, B, p);
        }

        /// <summary>
        /// Calculates the squared distance between this segment and <paramref name="p"/>
        /// </summary>
        /// <param name="p">The point to calculate squared distance to.</param>
        /// <returns>The squared distance.</returns>
        public double DistanceSquared(Point2 p) {
            Contract.Ensures(
                Contract.Result<double>() >= 0.0
                || Double.IsNaN(Contract.Result<double>())
                || Double.IsPositiveInfinity(Contract.Result<double>()));
            return DistanceSquared(A, B, p);
        }

        /// <summary>
        /// Calculates the distance between this segment and the given <paramref name="other"/>
        /// </summary>
        /// <param name="other">The segment to calculate distance to.</param>
        /// <returns>The distance.</returns>
        public double Distance(Segment2 other) {
            Contract.Ensures(
                Contract.Result<double>() >= 0.0
                || Double.IsNaN(Contract.Result<double>())
                || Double.IsPositiveInfinity(Contract.Result<double>()));
            if (ReferenceEquals(null, other))
                return Double.NaN;
            throw new NotImplementedException();
        }

        /// <summary>
        /// Calculates the squared distance between this segment and the given <paramref name="other"/>
        /// </summary>
        /// <param name="other">The segment to calculate squared distance to.</param>
        /// <returns>The squared distance.</returns>
        public double DistanceSquared(Segment2 other) {
            Contract.Ensures(
                Contract.Result<double>() >= 0.0
                || Double.IsNaN(Contract.Result<double>())
                || Double.IsPositiveInfinity(Contract.Result<double>()));
            if (ReferenceEquals(null, other))
                return Double.NaN;
            throw new NotImplementedException();
        }

        /// <summary>
        /// Determines if a <paramref name="point"/> intersects this segment.
        /// </summary>
        /// <param name="point">A point.</param>
        /// <returns><c>true</c> when intersecting.</returns>
        public bool Intersects(Point2 point) {
            return Intersects(A, B, point);
        }

        /// <inheritdoc/>
        public IPlanarGeometry Intersection(Point2 other)
        {
            if (Intersects(other))
                return other;
            return null;
        }

        /// <summary>
        /// Determines if this segment intersect another <paramref name="segment"/>.
        /// </summary>
        /// <param name="segment">A segment.</param>
        /// <returns><c>true</c> when another object intersects this object.</returns>
        public bool Intersects(Segment2 segment) {
            return segment != null && Intersects(A, B, segment.A, segment.B);
        }

        /// <summary>
        /// Calculates the intersection geometry between this segment and another.
        /// </summary>
        /// <param name="segment">The segment to find the intersection with.</param>
        /// <returns>The intersection geometry or <c>null</c> for no intersection.</returns>
        public IPlanarGeometry Intersection(Segment2 segment)
        {
            return segment == null
                ? null
                : Intersection(A, B, segment.A, segment.B);
        }

        /// <summary>
        /// Determines if this segment intersect another <paramref name="ray"/>.
        /// </summary>
        /// <param name="ray">A ray.</param>
        /// <returns><c>true</c> when another object intersects this object.</returns>
        public bool Intersects(Ray2 ray) {
            return ray != null && ray.Intersects(this);
        }

        /// <summary>
        /// Calculates the intersection geometry between this segment and a ray.
        /// </summary>
        /// <param name="ray">The ray to find the intersection with.</param>
        /// <returns>The intersection geometry or <c>null</c> for no intersection.</returns>
        public IPlanarGeometry Intersection(Ray2 ray)
        {
            if (ray == null)
                return null;
            return ray.Intersection(this);
        }

        /// <summary>
        /// Determines if this segment intersect another <paramref name="line"/>.
        /// </summary>
        /// <param name="line">A line.</param>
        /// <returns><c>true</c> when another object intersects this object.</returns>
        public bool Intersects(Line2 line) {
            return line != null && line.Intersects(this);
        }

        /// <summary>
        /// Calculates the intersection geometry between this segment and a line.
        /// </summary>
        /// <param name="line">The line to find the intersection with.</param>
        /// <returns>The intersection geometry or <c>null</c> for no intersection.</returns>
        public IPlanarGeometry Intersection(Line2 line) {
            if (line == null)
                return null;
            return line.Intersection(this);
        }

        /// <summary>
        /// Compares the segments, first by their smallest point.
        /// </summary>
        /// <param name="other">The other segment to compare.</param>
        /// <returns>A comparison value, see <see cref="IComparable.CompareTo"/>.</returns>
        [Obsolete("This is garbage and will probably do more harm than good.")]
        public int CompareTo(Segment2 other) {
            if (ReferenceEquals(null, other))
                return 1;
            return Compare(A, B, other.A, other.B);
        }
    }
}
